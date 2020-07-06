using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TransPick.Features.Unmanaged;

namespace TransPick.Features.Overylay
{
	internal class CrossHair : IDisposable
	{
		#region ::Fields::

		private readonly GraphicsWindow _window;

		private readonly Dictionary<string, SolidBrush> _brushes;
		private readonly Dictionary<string, Font> _fonts;

		private readonly bool _isShowInfos = false;

		#endregion

		#region ::Constructor::

		internal CrossHair(bool isShowInfos)
		{
			_brushes = new Dictionary<string, SolidBrush>();
			_fonts = new Dictionary<string, Font>();
			_isShowInfos = isShowInfos;

			var gfx = new Graphics()
			{
				MeasureFPS = true,
				PerPrimitiveAntiAliasing = true,
				TextAntiAliasing = true
			};

			// Initialize GraphicWindow
			_window = new GraphicsWindow(Monitor.GetLeft(), Monitor.GetTop(), Monitor.GetWidth(), Monitor.GetHeight(), gfx)
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
			_fonts["consolas"] = gfx.CreateFont("Consolas", 14);
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

			System.Drawing.Point cursorPoint = Cursor.GetCursorPoint();

			// Draw horizontal Left/Right line.
			gfx.DashedLine(_brushes["red"], Monitor.GetLeft(), cursorPoint.Y, cursorPoint.X-1, cursorPoint.Y, 1.0f);
			gfx.DashedLine(_brushes["red"], cursorPoint.X+1, cursorPoint.Y, Monitor.GetRight(), cursorPoint.Y, 1.0f);

			// Draw vertical Top/Bottom line.
			gfx.DashedLine(_brushes["blue"], cursorPoint.X, Monitor.GetTop(), cursorPoint.X, cursorPoint.Y-1, 1.0f);
			gfx.DashedLine(_brushes["blue"], cursorPoint.X, cursorPoint.Y+1, cursorPoint.X, Monitor.GetBottom(), 1.0f);
		}

		#endregion

		#region ::IDisposable Support::

		~CrossHair()
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
