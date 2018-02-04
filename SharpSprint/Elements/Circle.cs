using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.IO;
using SharpSprint.Primitives;

namespace SharpSprint.Elements
{
    public class Circle : Entity
    {
        // Required Parameters
        public Layer Layer { get; set; }
        public Size Width { get; set; }
        public Position Center { get; set; }
        public Size Radius { get; set; }

        // Optional Parameters
        public Size Clear { get; set; } // 4000
        public bool Cutout { get; set; } // False
        public bool Soldermask { get; set; } // False
        public FineAngle Start { get; set; } // 0
        public FineAngle Stop { get; set; } // 0
        public bool Fill { get; set; } // False

        public bool Read(Token[] Tokens)
        {
            throw new NotImplementedException();
        }
        
        public bool Write(out Token[] Tokens)
        {
            throw new NotImplementedException();
        }
    }
}
