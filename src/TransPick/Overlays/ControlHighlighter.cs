using GameOverlay.Windows;
using System;
using TransPick.Unmanaged;
using TransPick.Unmanaged.Types;

namespace TransPick.Overlays
{
	internal class ControlHighlighter
	{
		#region ::Fields::

		private OverlayBase _overlay;

		private readonly bool _isShowInfo;

		#endregion

		#region ::Constructor::

		internal ControlHighlighter(bool isShowInfo)
		{
			_isShowInfo = isShowInfo;
		}

		#endregion

		#region ::Overlay Drawer::

		private void DrawGraphics(object sender, DrawGraphicsEventArgs e)
		{
			var gfx = e.Graphics;
			var brushes = _overlay.Brushes;
			var fonts = _overlay.Fonts;

			// Get control information.
			IntPtr hWnd = Window.WindowFromPoint(InputDevices.GetCursorPoint());
			RECT rect;
			Window.GetWindowRect(hWnd, out rect);

			// Draw area rectangle.
			gfx.DrawRectangle(brushes["red"], rect.Left, rect.Top, rect.Right, rect.Bottom, 2.0f);

			// Draw area size box.
			string text = $"{rect.Right - rect.Left} X {rect.Bottom - rect.Top}";
			gfx.DrawTextWithBackground(fonts["arial-12"], brushes["red"], brushes["white"], rect.Left + 6, rect.Top + 6, text);
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
	}
}
