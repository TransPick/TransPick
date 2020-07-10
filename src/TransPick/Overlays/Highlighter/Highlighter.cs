using GameOverlay.Windows;

namespace TransPick.Overlays.Highlighter
{
    internal abstract class Highlighter
    {
		#region ::Fields::

		internal OverlayBase _overlay;

		internal bool _isShowInfo;

		#endregion

		#region ::Overlay Drawer::

		protected abstract void DrawGraphics(object sender, DrawGraphicsEventArgs e);

		#endregion

		#region ::Highlighter Starter&Stopper::

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
