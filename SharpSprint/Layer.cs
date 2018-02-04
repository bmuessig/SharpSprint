using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint
{
    public enum Layer : byte
    {
        CopperTop = 1,
        SilkscreenTop = 2,
        CopperBottom = 3,
        SilkscreenBottom = 4,
        CopperInnerTop = 5,
        CopperInnerBottom = 6,
        Mechanical = 7
    }
}
