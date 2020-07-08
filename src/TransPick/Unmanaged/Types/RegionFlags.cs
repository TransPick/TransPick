using System;

namespace TransPick.Unmanaged.Types
{
    /// <summary>
    /// The return value specifies the type of the region that the function obtains. It can be one of the following values.
    /// </summary>
    [Flags]
    internal enum RegionFlags
    {
        ERROR = 0,
        NULLREGION = 1,
        SIMPLEREGION = 2,
        COMPLEXREGION = 3
    }
}
