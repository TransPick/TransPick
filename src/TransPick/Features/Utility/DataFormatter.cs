using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TransPick.Features.Utility
{
    internal static class DataFormatter
    {
        internal static void Serialize(string filePath, object obj)
        {
            if (Uri.IsWellFormedUriString(filePath, UriKind.RelativeOrAbsolute))
                throw new UriFormatException($"Invalid format of file path({filePath}).");

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using(Stream stream = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Close();
            }
        }

        internal static void Deserialize(string filePath, ref object obj)
        {
            if (Uri.IsWellFormedUriString(filePath, UriKind.RelativeOrAbsolute))
                throw new UriFormatException($"Invalid format of file path({filePath}).");

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Could not find file to deserialize{filePath}.");

            using (Stream stream = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(stream);
                stream.Close();
            }
        }
    }
}
