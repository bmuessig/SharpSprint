using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.IO;

namespace SharpSprint.Elements
{
    public class SMDPad : Pad
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Position Position { get; set; }
        public Rectangle Size { get; set; }

        // Optional parameters
        public Size Clear { get; set; } // 4000
        public bool Soldermask { get; set; } // True
        public CoarseAngle Rotation { get; set; } // 0
        public bool Thermal { get; set; } // False
        public ushort ThermalTracksWidth { get; set; } // 100
        public SMDPadThermalTracks ThermalTracks { get; set; }
        public ulong PadID { get; set; }
        public List<Pad> Connections { get; set; }

        // Default optional parameters
        private const uint ClearDefault = 4000;
        private const bool SoldermaskDefault = true;
        private const uint RotationDefault = 0;
        private const bool ThermalDefault = false;
        private const ushort ThermalTracksWidthDefault = 100;

        public bool Read(IO.Token[][] Tokens, ref uint Pointer)
        {
            throw new NotImplementedException();
        }

        public bool Write(out IO.Token[][] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Write the type first
            writer.Write(new Token("SMDPAD", Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (ulong)Layer));
            else
                return false;

            // Position
            writer.Write(new Token("POS", Position.X.Value, Position.Y.Value));

            // Size
            writer.Write(new Token("SIZE_X", Size.Width.Value));
            writer.Write(new Token("SIZE_Y", Size.Height.Value));

            // Then write the optional values
            // Clear
            if (Clear.Value != ClearDefault)
                writer.Write(new Token("CLEAR", Clear.Value));

            // Soldermask
            if (Soldermask != SoldermaskDefault)
                writer.Write(new Token("SOLDERMASK", Soldermask));

            // Rotation
            if (Rotation.Value != RotationDefault)
                writer.Write(new Token("ROTATION", Rotation.Value));

            // Thermal
            if (Thermal != ThermalDefault)
                writer.Write(new Token("THERMAL", Thermal));

            // ThermalTracksWidth
            if (ThermalTracksWidth != ThermalTracksWidthDefault)
                writer.Write(new Token("THERMAL_TRACKS_WIDTH", ThermalTracksWidth));

            // ThermalTracks
            if (ThermalTracks != 0)
                writer.Write(new Token("THERMAL_TRACKS", (ulong)ThermalTracks));

            // PadID
            // TODO: Maybe this needs some automation to prevent duplicates?
            if (PadID != 0)
                writer.Write(new Token("PAD_ID", PadID));

            // Connections
            if (Connections != null)
            {
                uint counter = 0;
                foreach (Pad pad in Connections)
                {
                    if (pad.PadID == 0)
                        return false;
                    writer.Write(new Token(string.Format("CON{0}", counter), pad.PadID));
                    counter++;
                }
            }

            Tokens = writer.Compile();
            return true;
        }

        public enum SMDPadThermalTracks : byte
        {
            None = 0,
            All = 0xFF,
            
            North = (1 << 0),
            NorthEast = (1 << 1),
            East = (1 << 2),
            SouthEast = (1 << 3),
            South = (1 << 4),
            SouthWest = (1 << 5),
            West = (1 << 6),
            NorthWest = (1 << 7)
        }
    }
}
