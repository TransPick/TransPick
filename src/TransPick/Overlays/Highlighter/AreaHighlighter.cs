using GameOverlay.Windows;
using TransPick.Unmanaged;
using TransPick.Utilities;
using Point = System.Drawing.Point;

namespace TransPick.Overlays.Highlighter
{
	internal class AreaHighlighter : Highlighter
	{
		#region ::Fields::

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

		protected override void DrawGraphics(object sender, DrawGraphicsEventArgs e)
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
				string text = $"{PropertyCalculator.GetWidth(_firstX, cursorPoint.X)} X {PropertyCalculator.GetHeight(_firstY, cursorPoint.Y)}";
				gfx.DrawTextWithBackground(fonts["arial-12"], brushes["red"], brushes["white"], cursorPoint.X + 6, cursorPoint.Y + 6, text);
			}
			else if (_isSetSecondPoint)
			{
				// Draw area rectangle.
				gfx.DrawRectangle(brushes["red"], _firstX, _firstY, _secondX, _secondY, 2.0f);

				// Draw area size box.
				string text = $"{PropertyCalculator.GetWidth(_firstX, _secondX)} X {PropertyCalculator.GetHeight(_firstY, _secondY)}";
				gfx.DrawTextWithBackground(fonts["arial-12"], brushes["red"], brushes["white"], _secondX + 6, _secondY + 6, text);

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
