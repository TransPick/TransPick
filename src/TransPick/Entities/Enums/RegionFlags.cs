using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransPick.Entities.Enums
{
    //Region Flags - The return value specifies the type of the region that the function obtains. It can be one of the following values.
    [Flags]
    internal enum RegionFlags
    {
        ERROR = 0,
        NULLREGION = 1,
        SIMPLEREGION = 2,
        COMPLEXREGION = 3
    }
}
