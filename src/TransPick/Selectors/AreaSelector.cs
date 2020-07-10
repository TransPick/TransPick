using Gma.System.MouseKeyHook;
using System.Windows.Forms;
using TransPick.Overlays.Highlighter;
using TransPick.Unmanaged;
using Point = System.Drawing.Point;

namespace TransPick.Selectors
{
    internal class AreaSelector
    {
        #region ::Fields::

        private readonly AreaHighlighter _highlighter = new AreaHighlighter(true);

        private IKeyboardMouseEvents _globalHook;

        private bool _isSetFirstPoint;
        private bool _isSetSecondPoint;

        private Point _firstPoint;
        private Point _secondPoint;

        #endregion;

        #region ::Events::

        internal delegate void AreaSelectedEventHandler();
        internal event AreaSelectedEventHandler AreaSelectedEvent;

        #endregion

        #region ::Selector Starting & Stopping Methods::

        public void Start()
        {
            // Subscribe event.
            _globalHook = Hook.GlobalEvents();
            _globalHook.MouseDownExt += OnGlobalHookMouseDown;

            _highlighter.StartAsync();
        }

        public void Stop()
        {
            // Unsubscribe event.
            _globalHook.MouseDownExt -= OnGlobalHookMouseDown;

            // Dispose global hook event.
            _globalHook.Dispose();

            _highlighter.Stop();
        }

        #endregion

        #region ::Mouse Global Hook Event::

        private void OnGlobalHookMouseDown(object sender, MouseEventExtArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (!_isSetFirstPoint)
                {
                    _firstPoint = InputDevices.GetCursorPoint();
                    _highlighter.SetFirstPoint(_firstPoint);
                    _isSetFirstPoint = true;
                }
                else if (!_isSetSecondPoint)
                {
                    _secondPoint = InputDevices.GetCursorPoint();
                    _highlighter.SetSecondPoint(_secondPoint);
                    _isSetSecondPoint = true;

                    AreaSelectedEvent?.Invoke();

                    Stop();
                }
            }
        }

        #endregion

        #region ::Area Point Related::

        internal Point GetFirstPoint()
        {
            return _firstPoint;
        }

        internal Point GetSecondPoint()
        {
            return _secondPoint;
        }

        #endregion
    }
}
