using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;

namespace SharpSprint.Elements
{
    public interface Element : Entity
    {
        Layer Layer { get; set; }
        Size Clear { get; set; }
        bool Soldermask { get; set; }
    }
}
