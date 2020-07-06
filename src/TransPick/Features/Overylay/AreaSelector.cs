using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using TransPick.Entities.Enums;
using TransPick.Features.Unmanaged;

namespace TransPick.Features.Overylay
{
    internal class AreaSelector : IDisposable
	{
		#region ::Fields::

		private readonly GraphicsWindow _window;

		private readonly Dictionary<string, SolidBrush> _brushes;
		private readonly Dictionary<string, Font> _fonts;

		private readonly bool _isShowInfos = false;

		private Timer _mouseTimer = new Timer(10);

		private bool _isSetFirstPoint = false;
		private bool _isSetSecondPoint = false;

		private int _left = 0;
		private int _right = 0;
		private int _top = 0;
		private int _bottom = 0;

		#endregion

		#region ::Constructor::

		internal AreaSelector(bool isShowInfos)
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

			_mouseTimer.Elapsed += OnMouseTimerTick;
			_mouseTimer.Enabled = true;
		}

		#endregion

		#region ::Graphics Window Runner::

		internal void Run()
		{
			_window.Create();
			_window.Join();
			_mouseTimer.Start();
		}

		internal Rect GetSelectedRect()
		{
			return new Rect(_left, _top, _right - _left, _bottom - _top);
		}

		#endregion

		#region ::Key Timer Events::

		private void OnMouseTimerTick(object sender, EventArgs e)
        {
			if (!_isSetFirstPoint && 0 != (InputDevices.GetAsyncKeyState(VKeys.LBUTTON) & 0x8000))
            {
                System.Drawing.Point point = InputDevices.GetCursorPoint();
				_left = point.X;
				_top = point.Y;
				_isSetFirstPoint = true;
			}
			
			if (_isSetFirstPoint && !_isSetSecondPoint && 0 != (InputDevices.GetAsyncKeyState(VKeys.LBUTTON) & 0x0001))
			{
				System.Drawing.Point point = InputDevices.GetCursorPoint();
				_right = point.X;
				_bottom = point.Y;
				_isSetSecondPoint = true;
			}
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

			if (_isSetFirstPoint && !_isSetSecondPoint)
            {
				System.Drawing.Point cursorPoint = InputDevices.GetCursorPoint();

				// Draw objects.
				gfx.DrawRectangle(_brushes["red"], _left, _top, cursorPoint.X, cursorPoint.Y, 2.0f);
				gfx.DrawTextWithBackground(_fonts["consolas"], _brushes["red"], _brushes["white"], _left + 6, _top + 6, $"{Math.Abs(cursorPoint.X - _left)} X {Math.Abs(cursorPoint.Y - _top)}");
			}
			else if (_isSetSecondPoint)
            {
				// Draw objects.
				gfx.DrawRectangle(_brushes["red"], _left, _top, _right, _bottom, 2.0f);
				gfx.DrawTextWithBackground(_fonts["consolas"], _brushes["red"], _brushes["white"], _left + 6, _top + 6, $"{Math.Abs(_right - _left)} X {Math.Abs(_bottom - _top)}");
			}
		}

		#endregion

		#region ::IDisposable Support::

		~AreaSelector()
		{
			Dispose(false);
		}

		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				_window.Dispose();

				_mouseTimer.Stop();

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
