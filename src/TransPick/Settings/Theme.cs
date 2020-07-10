using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace TransPick.Settings
{
    [Serializable]
    internal class Theme
    {
        #region ::Singleton::

        // Singleton pattern to store settings
        [NonSerialized]
        private static Theme _instance;

        internal static Theme GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Theme();
            }

            return _instance;
        }

        #endregion

        #region ::Application Theme Properties::

        private bool _isDeserialized;

        internal bool IsDeserialized
        {
            get
            {
                return _isDeserialized;
            }
        }

        private bool _isUsingDarkMode;

        internal bool IsUsingDarkMode
        {
            get
            {
                return _isUsingDarkMode;
            }
            set
            {
                _isUsingDarkMode = value;
            }
        }

        private Color _primaryColor;

        internal Color PrimaryColor
        {
            get
            {
                return _primaryColor;
            }
            set
            {
                _primaryColor = value;
            }
        }

        private Color _secondaryColor;

        internal Color SecondaryColor
        {
            get
            {
                return _secondaryColor;
            }
            set
            {
                _secondaryColor = value;
            }
        }

        private Dictionary<string, GameOverlay.Drawing.Color> _brushRegistry = new Dictionary<string, GameOverlay.Drawing.Color>();

        internal Dictionary<string, GameOverlay.Drawing.Color> BrushRegistry
        {
            get
            {
                return _brushRegistry;
            }
        }

        private Dictionary<string, GameOverlay.Drawing.Font> _fontRegistry = new Dictionary<string, GameOverlay.Drawing.Font>();

        internal Dictionary<string, GameOverlay.Drawing.Font> FontRegistry
        {
            get
            {
                return _fontRegistry;
            }
        }

        #endregion

        #region ::Constructors::

        private void InitializeElements()
        {
            // Add brushes to temp registry.
            _brushRegistry.Add("black", new GameOverlay.Drawing.Color(0, 0, 0));
            _brushRegistry.Add("white", new GameOverlay.Drawing.Color(255, 255, 255));
            _brushRegistry.Add("red", new GameOverlay.Drawing.Color(255, 0, 0));
            _brushRegistry.Add("green", new GameOverlay.Drawing.Color(0, 255, 0));
            _brushRegistry.Add("blue", new GameOverlay.Drawing.Color(0, 0, 255));
            _brushRegistry.Add("background", new GameOverlay.Drawing.Color(0, 0, 0, 0));
            _brushRegistry.Add("grid", new GameOverlay.Drawing.Color(0, 0, 0, 0.5f));

            // Add fonts to temp registry.
            _fontRegistry.Add("arial-12", new GameOverlay.Drawing.Font(new Factory1(FactoryType.Isolated), "Arial", 12));
            _fontRegistry.Add("consolas-14", new GameOverlay.Drawing.Font(new Factory1(FactoryType.Isolated), "Consolas", 14));
        }

        internal Theme()
        {
            InitializeElements();
        }

        internal Theme(bool isUsingDarkMode, Color primaryColor, Color secondaryColor)
        {
            InitializeElements();

            _isUsingDarkMode = isUsingDarkMode;
            _primaryColor = primaryColor;
            _secondaryColor = secondaryColor;
        }

        #endregion
    }
}
