using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public interface Angle
    {
        int Value { get; set; }
        decimal Degrees { get; set; }
        double Radians { get; set; }
    }
}
