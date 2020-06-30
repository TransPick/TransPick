using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransPick.Entities.Classes
{
    internal class Language
    {
        internal string DisplayName { get; set; }
        internal CultureInfo Culture { get; set; }
    }
}
