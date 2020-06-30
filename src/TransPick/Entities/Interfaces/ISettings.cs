using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TransPick.Entities.Interfaces
{
    interface ISettings
    {
        void Serialize(string fileName, IFormatter formatter);
        void Deserialize(string fileName, IFormatter formatter);
    }
}
