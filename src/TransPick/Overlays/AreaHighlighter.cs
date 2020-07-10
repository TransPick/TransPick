using GameOverlay.Windows;
using System;
using TransPick.Unmanaged;
using Point = System.Drawing.Point;

namespace TransPick.Overlays
{
	internal class AreaHighlighter
	{
		#region ::Fields::

		private OverlayBase _overlay;

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
			_isShowInfo = isShowInfo;
		}

		#endregion

		#region ::Overlay Drawer::

		private int GetWidth(int a, int b)
        {
			if (a >= b)
            {
				return Math.Abs(a - b);
            }
			else
            {
				return Math.Abs(b - a);
            }				
        }

		private int GetHeight(int a, int b)
		{
			if (a >= b)
			{
				return Math.Abs(a - b);
			}
			else
			{
				return Math.Abs(b - a);
			}
		}

		private void DrawGraphics(object sender, DrawGraphicsEventArgs e)
		{
			var gfx = e.Graphics;
			var brushes = _overlay.Brushes;
			var fonts = _overlay.Fonts;

			if (_isSetFirstPoint && !_isSetSecondPoint)
			{
				Point cursorPoint = InputDevices.GetCursorPoint();

				// Draw area rectangle.
				gfx.DrawRectangle(brushes["red"], _firstX, _firstY, cursorPoint.X, cursorPoint.Y, 2.0f);

				// Draw area size box.
				string text = $"{GetWidth(_firstX, cursorPoint.X)} X {GetHeight(_firstY, cursorPoint.Y)}";
				gfx.DrawTextWithBackground(fonts["arial-12"], brushes["red"], brushes["white"], cursorPoint.X + 6, cursorPoint.Y + 6, text);
			}
			else if (_isSetSecondPoint)
			{
				// Draw area rectangle.
				gfx.DrawRectangle(brushes["red"], _firstX, _firstY, _secondX, _secondY, 2.0f);

				// Draw area size box.
				string text = $"{GetWidth(_firstX, _secondX)} X {GetHeight(_firstY, _secondY)}";
				gfx.DrawTextWithBackground(fonts["arial-12"], brushes["red"], brushes["white"], _firstX + 6, _firstY + 6, text);

			}
		}

		#endregion

		#region ::Highlighter Starting & Stopping Methods::

		internal void Start()
		{
			_overlay = new OverlayBase(DrawGraphics, _isShowInfo);
			_overlay.Run();
		}

		internal async void StartAsync()
        {
			_overlay = new OverlayBase(DrawGraphics, _isShowInfo);
			var result = await _overlay.RunAsync();
		}

		internal void Stop()
        {
			if (_overlay != null)
            {
				_overlay.Dispose();
            }
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
	}
}
