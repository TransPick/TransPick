using System;
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

        [NonSerialized]
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

        #endregion

        #region ::Constructors::

        internal Theme() { }

        internal Theme(bool isUsingDarkMode, Color primaryColor, Color secondaryColor)
        {
            _isUsingDarkMode = isUsingDarkMode;
            _primaryColor = primaryColor;
            _secondaryColor = secondaryColor;
        }

        #endregion
    }
}
