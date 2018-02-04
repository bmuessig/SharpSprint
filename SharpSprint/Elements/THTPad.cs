using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.IO;

namespace SharpSprint.Elements
{
    public class THTPad : Pad
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Position Position { get; set; }
        public Size Size { get; set; }
        public Size Drill { get; set; }
        public THTPadForm Form { get; set; }

        // Optional parameters
        public Size Clear { get; set; } // 4000
        public bool Soldermask { get; set; } // True
        public CoarseAngle Rotation { get; set; } // 0
        public bool Via { get; set; } // False
        public bool Thermal { get; set; } // False
        public ushort ThermalTracksWidth { get; set; } // 100
        public bool ThermalTracksIndividual { get; set; } // False
        public THTPadThermalTracks ThermalTracks { get; set; }
        public ulong PadID { get; set; }
        public List<Pad> Connections { get; set; }

        // Default optional parameters
        private const uint ClearDefault = 4000;
        private const bool SoldermaskDefault = true;
        private const uint RotationDefault = 0;
        private const bool ViaDefault = false;
        private const bool ThermalDefault = false;
        private const ushort ThermalTracksWidthDefault = 100;
        private const bool ThermalTracksIndividualDefault = false;

        public bool Read(IO.Token[][] Tokens, ref uint Pointer)
        {
            throw new NotImplementedException();
        }

        public bool Write(out IO.Token[][] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Write the type first
            writer.Write(new Token("PAD", Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (ulong)Layer));
            else
                return false;

            // Position
            writer.Write(new Token("POS", Position.X.Value, Position.Y.Value));

            // Size
            writer.Write(new Token("SIZE", Size.Value));

            // Drill
            writer.Write(new Token("DRILL", Drill.Value));

            // Form
            if (Form >= THTPadForm.Round && Form <= THTPadForm.HighRectangular)
                writer.Write(new Token("FORM", (ulong)Form));
            else
                return false;

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

            // Via
            if (Via != ViaDefault)
                writer.Write(new Token("VIA", Via));

            // Thermal
            if (Thermal != ThermalDefault)
                writer.Write(new Token("THERMAL", Thermal));

            // ThermalTracksWidth
            if (ThermalTracksWidth != ThermalTracksWidthDefault)
                writer.Write(new Token("THERMAL_TRACKS_WIDTH", ThermalTracksWidth));

            // ThermalTracksIndividual
            if (ThermalTracksIndividual != ThermalTracksIndividualDefault)
                writer.Write(new Token("THERMAL_TRACKS_INDIVIDUAL", ThermalTracksIndividual));

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

        public enum THTPadForm : byte
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

        public enum THTPadThermalTracks : uint
        {
            None = 0,
            All = 0xFFFFFFFF,

            CopperTopNorth = (1u << 0),
            CopperTopNorthEast = (1u << 1),
            CopperTopEast = (1u << 2),
            CopperTopSouthEast = (1u << 3),
            CopperTopSouth = (1u << 4),
            CopperTopSouthWest = (1u << 5),
            CopperTopWest = (1u << 6),
            CopperTopNorthWest = (1u << 7),

            CopperBottomNorth = (1u << 8),
            CopperBottomNorthEast = (1u << 9),
            CopperBottomEast = (1u << 10),
            CopperBottomSouthEast = (1u << 11),
            CopperBottomSouth = (1u << 12),
            CopperBottomSouthWest = (1u << 13),
            CopperBottomWest = (1u << 14),
            CopperBottomNorthWest = (1u << 15),

            CopperInnerTopNorth = (1u << 16),
            CopperInnerTopNorthEast = (1u << 17),
            CopperInnerTopEast = (1u << 18),
            CopperInnerTopSouthEast = (1u << 19),
            CopperInnerTopSouth = (1u << 20),
            CopperInnerTopSouthWest = (1u << 21),
            CopperInnerTopWest = (1u << 22),
            CopperInnerTopNorthWest = (1u << 23),

            CopperInnerBottomNorth = (1u << 24),
            CopperInnerBottomNorthEast = (1u << 25),
            CopperInnerBottomEast = (1u << 26),
            CopperInnerBottomSouthEast = (1u << 27),
            CopperInnerBottomSouth = (1u << 28),
            CopperInnerBottomSouthWest = (1u << 29),
            CopperInnerBottomWest = (1u << 30),
            CopperInnerBottomNorthWest = (1u << 31)
        }
    }
}
