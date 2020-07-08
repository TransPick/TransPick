using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransPick.Unmanaged;
using TransPick.Unmanaged.Types;

namespace TransPick.Selectors
{
    internal class FixedAreaSelector : IDisposable
    {
		#region ::Fields::

		private readonly GraphicsWindow _window;

		private readonly Dictionary<string, SolidBrush> _brushes;
		private readonly Dictionary<string, Font> _fonts;

		private readonly int _width = 0;
		private readonly int _height = 0;
		private readonly bool _isShowInfos = false;

		#endregion

		#region ::Constructor::

		internal FixedAreaSelector(int width, int height, bool isShowInfos)
		{
			_brushes = new Dictionary<string, SolidBrush>();
			_fonts = new Dictionary<string, Font>();
			_width = width;
			_height = height;
			_isShowInfos = isShowInfos;

			var gfx = new Graphics()
			{
				MeasureFPS = true,
				PerPrimitiveAntiAliasing = true,
				TextAntiAliasing = true
			};

			// Initialize GraphicWindow
			_window = new GraphicsWindow(Display.GetLeft(), Display.GetTop(), Display.GetWidth(), Display.GetHeight(), gfx)
			{
				FPS = 60,
				IsTopmost = true,
				IsVisible = true
			};

			// Subscribe the GraphicWindow event.
			_window.SetupGraphics += SetupGraphics;
			_window.DrawGraphics += DrawGraphics;
			_window.DestroyGraphics += DestroyGraphics;
		}

		#endregion

		#region ::Graphics Window Runner::

		internal void Run()
		{
			_window.Create();
			_window.Join();
		}

		#endregion

		#region ::Graphics Window Events::

		private void SetupGraphics(object sender, SetupGraphicsEventArgs e)
		{
			var gfx = e.Graphics;

			if (e.RecreateResources)
			{
				foreach (var pair in _brushes) pair.Value.Dispose();
			}

			// Add brushes.
			_brushes["black"] = gfx.CreateSolidBrush(0, 0, 0);
			_brushes["white"] = gfx.CreateSolidBrush(255, 255, 255);
			_brushes["red"] = gfx.CreateSolidBrush(255, 0, 0);
			_brushes["green"] = gfx.CreateSolidBrush(0, 255, 0);
			_brushes["blue"] = gfx.CreateSolidBrush(0, 0, 255);
			_brushes["background"] = gfx.CreateSolidBrush(0, 0, 0, 0);
			_brushes["grid"] = gfx.CreateSolidBrush(0, 0, 0, 0.5f);

			if (e.RecreateResources) return;

			// Add fonts.
			_fonts["arial"] = gfx.CreateFont("Arial", 12);
			_fonts["consolas"] = gfx.CreateFont("Consolas", 12);
		}

		private void DestroyGraphics(object sender, DestroyGraphicsEventArgs e)
		{
			foreach (var pair in _brushes) pair.Value.Dispose();
			foreach (var pair in _fonts) pair.Value.Dispose();
		}

		private void DrawGraphics(object sender, DrawGraphicsEventArgs e)
		{
			var gfx = e.Graphics;

			gfx.ClearScene(_brushes["background"]);

			if (_isShowInfos)
			{
				var padding = 10;
				var infoText = new StringBuilder()
					.Append("FPS: ").Append(gfx.FPS.ToString().PadRight(padding)).Append("\r\n")
					.Append("FrameTime: ").Append(e.FrameTime.ToString().PadRight(padding)).Append("\r\n")
					.Append("FrameCount: ").Append(e.FrameCount.ToString().PadRight(padding)).Append("\r\n")
					.Append("DeltaTime: ").Append(e.DeltaTime.ToString().PadRight(padding))
					.ToString();

				gfx.DrawTextWithBackground(_fonts["consolas"], _brushes["green"], _brushes["grid"], 20, 20, infoText);
			}

			System.Drawing.Point cursorPoint = InputDevices.GetCursorPoint();

			// Draw objects.
			gfx.DrawRectangle(_brushes["red"], cursorPoint.X, cursorPoint.Y, cursorPoint.X + _width, cursorPoint.Y + _height, 2.0f);
			gfx.DrawTextWithBackground(_fonts["consolas"], _brushes["red"], _brushes["white"], cursorPoint.X + 6, cursorPoint.Y + 6, $"{_width} X {_height}");
		}

		#endregion

		#region ::IDisposable Support::

		~FixedAreaSelector()
		{
			Dispose(false);
		}

		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				_window.Dispose();

				disposedValue = true;
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
