using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;

namespace SharpSprint.Elements
{
    public interface Pad : Element
    {
        Position Position { get; set; }
        CoarseAngle Rotation { get; set; }
        bool Thermal { get; set; }
        ushort ThermalTracksWidth { get; set; }
        ulong PadID { get; set; }
        List<Pad> Connections { get; set; }
    }
}
