using System;
using System.IO;
using System.Runtime.Serialization;
using TransPick.Entities;
using TransPick.Entities.Classes;
using TransPick.Entities.Interfaces;

namespace TransPick.Settings
{
    [Serializable]
    public class General : ISettings, ISerializable
    {
        #region ::Singleton::

        // Singleton pattern to store settings
        [NonSerialized]
        private static General _instance = null;

        public static General GetInstance()
        {
            if (_instance == null)
                _instance = new General();

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

        // Application language information
        private Language _language = new Language();

        public Language Language
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

        #endregion

        #region ::Constructors::

        public General() { }

        public General(SerializationInfo info, StreamingContext context)
        {
            // Reset the property value using the GetValue method.
            _language = (Language)info.GetValue("language", typeof(Language));
        }

        #endregion

        #region ::Serializer and Deserializer::

        // Add data to be serialized or deserialized.
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            // Use the AddValue method to specify serialized values.
            info.AddValue("language", _language, typeof(Language));
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
                General instance = (General)formatter.Deserialize(stream);
                _instance = instance;
                stream.Close();
            }
        }

        #endregion
    }
}
