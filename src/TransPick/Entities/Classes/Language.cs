using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransPick.Entities.Classes
{
    public class Language
    {
        public string DisplayName { get; set; }
        public CultureInfo Culture { get; set; }
    }
}
