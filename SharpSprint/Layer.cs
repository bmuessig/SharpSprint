using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint
{
    public enum Layer : byte
    {
        Copper_Top = 1,
        Silkscreen_Top = 2,
        Copper_Bottom = 3,
        Silkscreen_Bottom = 4,
        Copper_Inner_Top = 5,
        Copper_Inner_Bottom = 6,
        Mechanical = 7
    }
}
