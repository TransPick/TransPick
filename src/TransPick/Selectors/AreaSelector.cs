using GameOverlay.Drawing;
using GameOverlay.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using TransPick.Unmanaged;
using TransPick.Unmanaged.Types;
using Point = System.Drawing.Point;

namespace TransPick.Selectors
{
	internal class AreaSelector : IDisposable
	{
		#region ::Fields::

		private bool _disposedValue;

		private readonly GraphicsWindow _window;

		private readonly Dictionary<string, SolidBrush> _brushes;
		private readonly Dictionary<string, Font> _fonts;

		private readonly bool _isShowInfo = false;

		private bool _isSetFirstPoint = false;
		private bool _isSetSecondPoint = false;

		private int _firstX = 0;
		private int _secondX = 0;
		private int _firstY = 0;
		private int _secondY = 0;

		#endregion

		#region ::Events::

		internal delegate void AreaSelectedEventHandler(object sender, AreaEventArgs e);

		internal event AreaSelectedEventHandler AreaSelectedEvent;

		#endregion

		#region ::Constructor::

		internal AreaSelector(bool isShowInfo)
		{
			_brushes = new Dictionary<string, SolidBrush>();
			_fonts = new Dictionary<string, Font>();
			_isShowInfo = isShowInfo;

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
			_window.SetupGraphics += OnSetupGraphics;
			_window.DrawGraphics += OnDrawGraphics;
			_window.DestroyGraphics += OnDestroyGraphics;
		}

        #endregion

        #region ::Key Timer Events::

        private bool IsPointSelected()
        {
			if (!_isSetFirstPoint && 0 != (InputDevices.GetAsyncKeyState(VKeys.LBUTTON) & 0x8000))
			{
				Point point = InputDevices.GetCursorPoint();
				_firstX = point.X;
				_firstY = point.Y;

				_isSetFirstPoint = true;
				MessageBox.Show("SFP");
			}

			if (_isSetFirstPoint && !_isSetSecondPoint && 0 != (InputDevices.GetAsyncKeyState(VKeys.LBUTTON) & 0x0001))
			{
				Point point = InputDevices.GetCursorPoint();
				_secondX = point.X;
				_secondY = point.Y;

				// Invoke AreaSelectedEvent when area selecting is complete.
				AreaSelectedEvent?.Invoke(this, new AreaEventArgs(_firstX, _firstY, _secondX, _secondY));

				_isSetSecondPoint = true;
			}

			if (_isSetSecondPoint)
				return true;
			else
				return false;
		}

		#endregion

		#region ::Graphics Window Runner::

		internal Rect Run()
		{
			_window.Create();
			_window.Join();

			while (true)
            {
				if (IsPointSelected())
					break;
            }

			return GetSelectedArea();
		}

		#endregion

		#region ::Selection Related::

		internal Rect GetSelectedArea()
		{
			int left = _firstX <= _secondX ? _firstX : _secondX;
			int top = _firstY <= _secondY ? _firstY : _secondY;
			int width = 0;
			int height = 0;

			if (_firstX <= _secondX)
				width = Math.Abs(_secondX - _firstX);
			else
				width = Math.Abs(_firstX - _secondX);

			if (_firstY <= _secondY)
				height = Math.Abs(_secondY - _firstY);
			else
				height = Math.Abs(_firstY - _secondY);

			return new Rect(left, top, width, height);
		}

		#endregion

		#region ::Graphics Window Events::

		private void OnSetupGraphics(object sender, SetupGraphicsEventArgs e)
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
			_brushes["background"] = gfx.CreateSolidBrush(0, 0, 0, 0.1f);
			_brushes["grid"] = gfx.CreateSolidBrush(0, 0, 0, 0.5f);

			if (e.RecreateResources) return;

			// Add fonts.
			_fonts["arial"] = gfx.CreateFont("Arial", 12);
			_fonts["consolas"] = gfx.CreateFont("Consolas", 14);
		}

		private void OnDestroyGraphics(object sender, DestroyGraphicsEventArgs e)
		{
			foreach (var pair in _brushes) pair.Value.Dispose();
			foreach (var pair in _fonts) pair.Value.Dispose();
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

		~AreaSelector()
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
