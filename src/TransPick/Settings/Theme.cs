using System;
using System.IO;
using System.Runtime.Serialization;
using System.Windows.Media;

namespace TransPick.Settings
{
    [Serializable]
    public class Theme : ISettings, ISerializable
    {
        #region ::Singleton::

        // Singleton pattern to store settings
        [NonSerialized]
        private static Theme _instance = null;

        public static Theme GetInstance()
        {
            if (_instance == null)
                _instance = new Theme();

            return _instance;
        }

        #endregion

        #region ::Fields::

        [NonSerialized]
        private bool _isDeserialized = false;

        public bool IsDeserialized
        {
            get
            {
                return _isDeserialized;
            }
        }

        private bool _isUsingDarkMode = false;

        public bool IsUsingDarkMode
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

        public Color PrimaryColor
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

        public Color SecondaryColor
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

        public Theme() { }

        public Theme(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            _isUsingDarkMode = (bool)info.GetValue("using_dark_mode", typeof(bool));
            _primaryColor = (Color)info.GetValue("primary_color", typeof(Color));
            _secondaryColor = (Color)info.GetValue("secondary_color", typeof(Color));
        }

        #endregion

        #region ::Serializer and Deserializer::

        // Add data to be serialized or deserialized.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("using_dark_mode", _isUsingDarkMode, typeof(bool));
            info.AddValue("primary_color", _primaryColor, typeof(Color));
            info.AddValue("secondary_color", _secondaryColor, typeof(Color));
        }

        public void Serialize(string fileName, IFormatter formatter)
        {
            // Create an instance of the type and serialize it.
            using (FileStream stream = new FileStream(fileName, FileMode.Create))
            {
                formatter.Serialize(stream, this);
                stream.Close();
            }
        }

        public void Deserialize(string fileName, IFormatter formatter)
        {
            if (!File.Exists(fileName))
                throw new FileNotFoundException();

            using (FileStream stream = new FileStream(fileName, FileMode.Open))
            {
                Theme instance = (Theme)formatter.Deserialize(stream);
                _instance = instance;
                stream.Close();
            }
        }

        #endregion
    }
}
