using System;
using System.Collections.Generic;
using TransPick.Capturers.Types;
using TransPick.Settings.Types;

namespace TransPick.Settings
{
    [Serializable]
    internal class General
    {
        #region ::Singleton::

        // Singleton pattern to store settings
        [NonSerialized]
        private static General _instance = null;

        internal static General GetInstance()
        {
            if (_instance == null)
                _instance = new General();

            return _instance;
        }

        #endregion

        #region ::Fields::

        [NonSerialized]
        private bool _isDeserialized = false;

        internal bool IsDeserialized
        {
            get
            {
                return _isDeserialized;
            }
        }

        private List<Language> _languageList = new List<Language>();

        internal List<Language> LanguageList
        {
            get
            {
                return _languageList;
            }
            set
            {
                _languageList = value;
            }
        }

        // Application language information
        private Language _language = new Language();

        internal Language Language
        {
            get
            {
                return _language;
            }
            set
            {
                _language = value;
            }
        }

        private CapturerType _lastCapturerType = CapturerType.AllScreen;

        internal CapturerType LastCapturerType
        {
            get
            {
                return _lastCapturerType;
            }
            set
            {
                _lastCapturerType = value;
            }
        }

        #endregion

        #region ::Constructors::

        internal General() { }

        #endregion
    }
}
