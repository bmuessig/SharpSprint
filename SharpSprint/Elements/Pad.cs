using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Elements
{
    public interface Pad : Element
    {
        ulong PadID { get; set; }
        List<uint> Connections { get; set; }
    }
}
