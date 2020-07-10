using GameOverlay.Windows;
using TransPick.Unmanaged;
using Point = System.Drawing.Point;

namespace TransPick.Overlays
{
    internal class FixedAreaHighlighter
    {
		#region ::Fields::

		private OverlayBase _overlay;

		private bool _isPointFixed;
		private Point _point = new Point();

		private readonly int _width;
		private readonly int _height;

		private readonly bool _isShowInfo;

		#endregion

		#region ::Constructor::

		internal FixedAreaHighlighter(int width, int height, bool isShowInfo)
		{
			_width = width;
			_height = height;
			_isShowInfo = isShowInfo;
		}

		#endregion

		#region ::Overlay Drawer::

		private void DrawGraphics(object sender, DrawGraphicsEventArgs e)
		{
			var gfx = e.Graphics;
			var brushes = _overlay.Brushes;
			var fonts = _overlay.Fonts;

			string text = $"{_width} X {_height}";

			if (!_isPointFixed)
			{
				Point cursorPoint = InputDevices.GetCursorPoint();

				// Draw area rectangle.
				gfx.DrawRectangle(brushes["red"], cursorPoint.X, cursorPoint.Y, cursorPoint.X + _width, cursorPoint.Y + _height, 2.0f);
				
				// Draw area size box.
				gfx.DrawTextWithBackground(fonts["arial-12"], brushes["red"], brushes["white"], cursorPoint.X + 6, cursorPoint.Y + 6, text);
			}
			else
			{
				// Draw area rectangle.
				gfx.DrawRectangle(brushes["red"], _point.X, _point.Y, _point.X + _width, _point.Y + _height, 2.0f);
				
				// Draw area size box.
				gfx.DrawTextWithBackground(fonts["crial-12"], brushes["red"], brushes["white"], _point.X + 6, _point.Y + 6, text);
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

		internal void SetPoint(Point point)
		{
			_point = point;
			_isPointFixed = true;
		}

		#endregion
	}
}
