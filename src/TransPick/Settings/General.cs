using System;
using System.IO;
using System.Runtime.Serialization;
using TransPick.Entities;
using TransPick.Entities.Classes;
using TransPick.Entities.Enums;

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
