using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.Points;

namespace SharpSprint.Elements
{
    public interface Multipoint : Element
    {
        Distance Width { get; set; }
        List<Point> Path { get; set; }

        bool Cutout { get; set; }
    }
}
