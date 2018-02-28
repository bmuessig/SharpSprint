using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;

namespace SharpSprint.Elements
{
    public interface Pad : Element
    {
        Vector Position { get; set; }
        CoarseAngle Rotation { get; set; }
        bool Thermal { get; set; }
        ushort ThermalTracksWidth { get; set; }
        uint PadID { get; set; }
        List<uint> Connections { get; set; }
    }
}
