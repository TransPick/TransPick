using GameOverlay.Drawing;
using GameOverlay.Windows;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
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

		private IKeyboardMouseEvents _globalHook;

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
		}

		#endregion

		#region ::Graphics Window Runner::

		internal void Run()
		{
			_window.Create();
			_window.Join();

			_globalHook = Hook.GlobalEvents();

			_globalHook.MouseDragStarted += GlobalHookMouseDown;
			_globalHook.MouseDragFinished += GlobalHookMouseUp;
		}

		internal Rect GetSelectedRect()
		{
			return new Rect();
		}

		#endregion

		#region ::Mouse Events::

		private void GlobalHookMouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_left = e.X;
				_top = e.Y;

				_isSetFirstPoint = true;
				System.Windows.MessageBox.Show("Down");
			}
		}

		private void GlobalHookMouseUp(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				_right = e.X;
				_bottom = e.Y;

				_isSetSecondPoint = true;
				System.Windows.MessageBox.Show("Up");
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

			if (_isSetFirstPoint)
            {
				System.Drawing.Point cursorPoint = Unmanaged.Cursor.GetCursorPoint();

				// Draw objects.
				gfx.DrawRectangle(_brushes["red"], _left, _top, cursorPoint.X, cursorPoint.X, 2.0f);
				gfx.DrawTextWithBackground(_fonts["consolas"], _brushes["red"], _brushes["white"], _left + 6, _top + 6, $"{cursorPoint.X - _left} X {cursorPoint.X - _top}");
			}
			else if (_isSetSecondPoint)
            {
				// Draw objects.
				gfx.DrawRectangle(_brushes["red"], _left, _top, _right, _bottom, 2.0f);
				gfx.DrawTextWithBackground(_fonts["consolas"], _brushes["red"], _brushes["white"], _left + 6, _top + 6, $"{_right - _left} X {_bottom - _top}");
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

				_globalHook.MouseDragStarted -= GlobalHookMouseDown;
				_globalHook.MouseDragFinished -= GlobalHookMouseUp;
				_globalHook.Dispose();

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
