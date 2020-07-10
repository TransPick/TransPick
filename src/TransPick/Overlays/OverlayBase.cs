using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TransPick.Settings;
using TransPick.Unmanaged;

namespace TransPick.Overlays
{
    internal class OverlayBase : IDisposable
    {
		#region ::Fields::

		private readonly GraphicsWindow _window;

		private bool _disposeValue;

		internal bool DisposedValue
        {
			get
            {
				return _disposeValue;
            }
        }

		private readonly Dictionary<string, SolidBrush> _brushes = new Dictionary<string, SolidBrush>();

		internal Dictionary<string, SolidBrush> Brushes
        {
            get
            {
				return _brushes;
            }
        }

		private readonly Dictionary<string, Color> _brushRegistry = Theme.GetInstance().BrushRegistry;

		private readonly Dictionary<string, Font> _fonts = new Dictionary<string, Font>();

		internal Dictionary<string, Font> Fonts
        {
            get
            {
				return _fonts;
            }
        }

		private readonly Dictionary<string, Font> _fontRegistry = Theme.GetInstance().FontRegistry;

		private readonly bool _isShowInfo;

		#endregion

		#region ::Delegates::

		internal delegate void DrawingGraphicsDelegate(object sender, DrawGraphicsEventArgs e);
		private readonly DrawingGraphicsDelegate _drawGraphics;

		#endregion

		#region ::Constructors::

		internal OverlayBase(DrawingGraphicsDelegate drawingDelegate, bool isShowInfo)
		{
			_isShowInfo = isShowInfo;

			Graphics gfx = new Graphics();
			gfx.MeasureFPS = true;
			gfx.PerPrimitiveAntiAliasing = true;
			gfx.TextAntiAliasing = true;

			// Initialize GraphicWindow
			_window = new GraphicsWindow(Display.GetLeft(), Display.GetTop(), Display.GetWidth(), Display.GetHeight(), gfx)
			{
				FPS = 60,
				IsTopmost = true,
				IsVisible = true
			};

			_drawGraphics = drawingDelegate;

			// Subscribe the GraphicWindow event.
			_window.SetupGraphics += OnSetupGraphics;
			_window.DrawGraphics += OnDrawGraphics;
			_window.DestroyGraphics += OnDestroyGraphics;
		}

		#endregion

		#region ::Graphics Window Runner::

		internal void Run()
		{
			_window.Create();
			_window.Join();
		}

		internal async Task<bool> RunAsync()
		{
			await Task.Run(async() =>
			{
				_window.Create();
				_window.Join();

				while (true)
				{
					if (_disposeValue)
					{
						return true;
					}
                    else
                    {
						await Task.Delay(10).ConfigureAwait(false);
                    }
				}
			}).ConfigureAwait(false);

			return false;
		}

		#endregion

		#region ::Graphics Window Elements Related::
		
		internal void AddBrush(string name, Color color)
        {
			if (_brushRegistry.ContainsKey(name))
            {
				_brushRegistry[name] = color;
			}
            else
            {
				_brushRegistry.Add(name, color);
			}
        }

		internal void AddFont(string name, Font font)
        {
			if (_fontRegistry.ContainsKey(name))
			{
				_fontRegistry[name] = font;
			}
			else
			{
				_fontRegistry.Add(name, font);
			}
		}

		internal Dictionary<string, SolidBrush> GetBrushes()
        {
			return _brushes;
        }

		internal Dictionary<string, Font> GetFonts()
		{
			return _fonts;
		}

		#endregion

		#region ::Graphics Window Events::

		private void OnSetupGraphics(object sender, SetupGraphicsEventArgs e)
		{
			var gfx = e.Graphics;

			if (e.RecreateResources)
			{
				foreach (var pair in Brushes)
				{
					pair.Value.Dispose();
				}
			}

			foreach(var pair in _brushRegistry)
            {
				Brushes.Add(pair.Key, gfx.CreateSolidBrush(pair.Value));
            }

			if (e.RecreateResources)
			{
				return;
			}

			foreach (var pair in _fontRegistry)
			{
				Fonts.Add(pair.Key, gfx.CreateFont(pair.Value.FontFamilyName, pair.Value.FontSize, pair.Value.Bold, pair.Value.Italic, pair.Value.WordWeapping));
			}
		}

		private void OnDestroyGraphics(object sender, DestroyGraphicsEventArgs e)
		{
			foreach (var pair in Brushes)
			{
				pair.Value.Dispose();
			}

			foreach (var pair in Fonts)
			{
				pair.Value.Dispose();
			}
		}

		private void OnDrawGraphics(object sender, DrawGraphicsEventArgs e)
		{
			var gfx = e.Graphics;

			gfx.ClearScene(Brushes["background"]);


			if (!_disposeValue)
			{
				if (_isShowInfo)
				{
					var padding = 10;
					var infoText = new StringBuilder()
						.Append("FPS: ").Append(gfx.FPS.ToString().PadRight(padding)).Append("\r\n")
						.Append("FrameTime: ").Append(e.FrameTime.ToString().PadRight(padding)).Append("\r\n")
						.Append("FrameCount: ").Append(e.FrameCount.ToString().PadRight(padding)).Append("\r\n")
						.Append("DeltaTime: ").Append(e.DeltaTime.ToString().PadRight(padding))
						.ToString();

					gfx.DrawTextWithBackground(Fonts["consolas-14"], Brushes["green"], Brushes["grid"], 20, 20, infoText);
				}

				// Call-back
				_drawGraphics(sender, e);
			}
		}

		#endregion

		#region ::IDisposable Support::

		~OverlayBase()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposeValue)
			{
				_window.Dispose();

				_disposeValue = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion
	}
}
