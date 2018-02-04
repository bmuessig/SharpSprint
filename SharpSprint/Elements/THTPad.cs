using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;

namespace SharpSprint.Elements
{
    public class THTPad : Pad
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Position Position { get; set; }
        public Size Size { get; set; }
        public Size Drill { get; set; }
        public ViaForm Form { get; set; }

        // Optional parameters
        public Size Clear { get; set; } // 4000
        public bool Soldermask { get; set; } // True
        public CoarseAngle Rotation { get; set; } // 0
        public bool Via { get; set; } // False
        public bool Thermal { get; set; } // False
        public ushort TermalTracksWidth { get; set; } // 100
        public bool ThermalTracksIndividual { get; set; } // False
        public uint ThermalTracks { get; set; }
        public ulong PadID { get; set; }
        public List<uint> Connections { get; set; }

        public enum ViaForm : byte
        {
            Round = 1,
            Octagon = 2,
            Square = 3,
            TransverseRounded = 4,
            TransverseOctagon = 5,
            TransverseRectangular = 6,
            HighRounded = 7,
            HighOctagon = 8,
            HighRectangular = 9
        }

        public bool Read(IO.Token[] Tokens)
        {
            throw new NotImplementedException();
        }

        public bool Write(out IO.Token[] Tokens)
        {
            throw new NotImplementedException();
        }
    }
}
