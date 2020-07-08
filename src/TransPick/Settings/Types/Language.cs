using System.Globalization;

namespace TransPick.Settings.Types
{
    internal class Language
    {
        internal string DisplayName { get; set; }
        internal CultureInfo Culture { get; set; }
    }
}
