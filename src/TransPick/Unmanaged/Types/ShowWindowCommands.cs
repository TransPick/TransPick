using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransPick.Unmanaged.Types
{
    /// <summary>
    /// The return value specifies the initial state of the screen.
    /// </summary>
    [Flags]
    internal enum ShowWindowCommands
    {
        SW_SHOWNORMAL = 1,
        SW_SHOWMINIMIZED = 2,
        SW_SHOWMAXIMIZED = 3
    }
}
