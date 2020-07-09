using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using TransPick.Unmanaged;
using Point = System.Drawing.Point;

namespace TransPick.Highlighters
{
	internal class AreaHighlighter : IDisposable
	{
		#region ::Fields::

		private bool _disposedValue;

		private readonly GraphicsWindow _window;

		private readonly Dictionary<string, SolidBrush> _brushes;
		private readonly Dictionary<string, Font> _fonts;

		private readonly bool _isShowInfo;

		private bool _isSetFirstPoint;
		private bool _isSetSecondPoint;

		private int _firstX;
		private int _secondX;
		private int _firstY;
		private int _secondY;

		#endregion

		#region ::Constructor::

		internal AreaHighlighter(bool isShowInfo)
		{
			_brushes = new Dictionary<string, SolidBrush>();
			_fonts = new Dictionary<string, Font>();
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

			// Subscribe the GraphicWindow event.
			_window.SetupGraphics += OnSetupGraphics;
			_window.DrawGraphics += OnDrawGraphics;
			_window.DestroyGraphics += OnDestroyGraphics;
		}

		#endregion

		#region ::Area Point Related::

		internal void SetFirstPoint(Point point)
		{
			_firstX = point.X;
			_firstY = point.Y;
			_isSetFirstPoint = true;
		}

		internal void SetSecondPoint(Point point)
		{
			_secondX = point.X;
			_secondY = point.Y;
			_isSetSecondPoint = true;
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

		private void OnSetupGraphics(object sender, SetupGraphicsEventArgs e)
		{
			var gfx = e.Graphics;

			if (e.RecreateResources)
			{
				foreach (var pair in _brushes)
				{
					pair.Value.Dispose();
				}
			}

			// Add brushes.
			_brushes["black"] = gfx.CreateSolidBrush(0, 0, 0);
			_brushes["white"] = gfx.CreateSolidBrush(255, 255, 255);
			_brushes["red"] = gfx.CreateSolidBrush(255, 0, 0);
			_brushes["green"] = gfx.CreateSolidBrush(0, 255, 0);
			_brushes["blue"] = gfx.CreateSolidBrush(0, 0, 255);
			_brushes["background"] = gfx.CreateSolidBrush(0, 0, 0, 0.1f);
			_brushes["grid"] = gfx.CreateSolidBrush(0, 0, 0, 0.5f);

			if (e.RecreateResources)
			{
				return;
			}

			// Add fonts.
			_fonts["arial"] = gfx.CreateFont("Arial", 12);
			_fonts["consolas"] = gfx.CreateFont("Consolas", 14);
		}

		private void OnDestroyGraphics(object sender, DestroyGraphicsEventArgs e)
		{
			foreach (var pair in _brushes)
			{
				pair.Value.Dispose();
			}

			foreach (var pair in _fonts)
			{
				pair.Value.Dispose();
			}
		}

		private void OnDrawGraphics(object sender, DrawGraphicsEventArgs e)
		{
			var gfx = e.Graphics;

			gfx.ClearScene(_brushes["background"]);


			if (!_disposedValue)
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

					gfx.DrawTextWithBackground(_fonts["consolas"], _brushes["green"], _brushes["grid"], 20, 20, infoText);
				}

				if (_isSetFirstPoint && !_isSetSecondPoint)
				{
					Point cursorPoint = InputDevices.GetCursorPoint();

					// Draw objects.
					gfx.DrawRectangle(_brushes["red"], _firstX, _firstY, cursorPoint.X, cursorPoint.Y, 2.0f);

					string text = $"{Math.Abs(_secondX - _firstX)} X {Math.Abs(_secondY - _firstY)}";
					gfx.DrawTextWithBackground(_fonts["consolas"], _brushes["red"], _brushes["white"], cursorPoint.X + 6, cursorPoint.Y + 6, text);
				}
				else if (_isSetSecondPoint)
				{
					// Draw objects.
					gfx.DrawRectangle(_brushes["red"], _firstX, _firstY, _secondX, _secondY, 2.0f);

					string text = $"{Math.Abs(_secondX - _firstX)} X {Math.Abs(_secondY - _firstY)}";
					gfx.DrawTextWithBackground(_fonts["consolas"], _brushes["red"], _brushes["white"], _firstX + 6, _firstY + 6, text);

				}
			}
		}

		#endregion

		#region ::IDisposable Support::

		~AreaHighlighter()
		{
			Dispose(false);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposedValue)
			{
				_window.Dispose();

				_disposedValue = true;
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
