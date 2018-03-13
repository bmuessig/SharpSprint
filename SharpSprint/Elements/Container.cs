using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Elements
{
    public interface Container : Entity
    {
        List<Entity> Entities { get; }
    }
}
