using System;

namespace TransPick.Unmanaged.Types
{
    /// <summary>
    /// The return value specifies key modifier.
    /// </summary>
    [Flags]
    internal enum KeyModifier
    {
        None = 0,
        Alt = 1,
        Control = 2,
        Shift = 4,
        // Either WINDOWS key was held down. These keys are labeled with the Windows logo.
        // Keyboard shortcuts that involve the WINDOWS key are reserved for use by the
        // operating system.
        Windows = 8
    }
}
