using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.IO;
using SharpSprint.Points;

namespace SharpSprint.Elements
{
    public class THTPad : Pad
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Point Position { get; set; }
        public Distance Size { get; set; }
        public Distance Drill { get; set; }
        public THTPadForm Form { get; set; }

        // Optional parameters
        public Distance Clear { get; set; } // 4000
        public bool Soldermask { get; set; } // True
        public CoarseAngle Rotation { get; set; } // 0
        public bool Via { get; set; } // False
        public bool Thermal { get; set; } // False
        public ushort ThermalTracksWidth { get; set; } // 100
        public bool ThermalTracksIndividual { get; set; } // False
        public THTPadThermalTracks ThermalTracks { get; set; }
        public uint PadID { get; set; }
        public List<uint> Connections { get; set; }

        // Default optional parameters
        private const int ClearDefault = 4000;
        private const bool SoldermaskDefault = true;
        private const int RotationDefault = 0;
        private const bool ViaDefault = false;
        private const bool ThermalDefault = false;
        private const ushort ThermalTracksWidthDefault = 100;
        private const bool ThermalTracksIndividualDefault = false;
        private const THTPadThermalTracks ThermalTracksDefault = THTPadThermalTracks.None;

        // Required and optional count
        private const byte RequiredArgCount = 5;
        private const byte OptionalArgCount = 9;

        private THTPad()
        {
            this.Clear = new Distance(ClearDefault);
            this.Soldermask = SoldermaskDefault;
            this.Rotation = new CoarseAngle(RotationDefault);
            this.Via = ViaDefault;
            this.Thermal = ThermalDefault;
            this.ThermalTracksWidth = ThermalTracksWidthDefault;
            this.ThermalTracksIndividual = ThermalTracksIndividualDefault;
            this.ThermalTracks = ThermalTracksDefault;
            this.Connections = new List<uint>();
        }

        public THTPad(Layer Layer, Point Position, Distance Size, Distance Drill, THTPadForm Form, uint PadId = 0,
            params uint[] Connections)
        {
            this.Layer = Layer;
            this.Position = Position;
            this.Size = Size;
            this.Drill = Drill;
            this.Form = Form;

            this.Clear = new Distance(ClearDefault);
            this.Soldermask = SoldermaskDefault;
            this.Rotation = new CoarseAngle(RotationDefault);
            this.Via = ViaDefault;
            this.Thermal = ThermalDefault;
            this.ThermalTracksWidth = ThermalTracksWidthDefault;
            this.ThermalTracksIndividual = ThermalTracksIndividualDefault;
            this.ThermalTracks = ThermalTracksDefault;
            this.PadID = PadId;
            this.Connections = new List<uint>(Connections);
        }

        public THTPad(Layer Layer, Point Position, Distance Size, Distance Drill, THTPadForm Form, CoarseAngle Rotation,
            uint PadId = 0, params uint[] Connections)
        {
            this.Layer = Layer;
            this.Position = Position;
            this.Size = Size;
            this.Drill = Drill;
            this.Form = Form;

            this.Clear = new Distance(ClearDefault);
            this.Soldermask = SoldermaskDefault;
            this.Rotation = Rotation;
            this.Via = ViaDefault;
            this.Thermal = ThermalDefault;
            this.ThermalTracksWidth = ThermalTracksWidthDefault;
            this.ThermalTracksIndividual = ThermalTracksIndividualDefault;
            this.ThermalTracks = ThermalTracksDefault;
            this.PadID = PadId;
            this.Connections = new List<uint>(Connections);
        }

        public THTPad(Layer Layer, Point Position, Distance Size, Distance Drill, THTPadForm Form, bool Via, uint PadId = 0,
            params uint[] Connections)
        {
            this.Layer = Layer;
            this.Position = Position;
            this.Size = Size;
            this.Drill = Drill;
            this.Form = Form;

            this.Clear = new Distance(ClearDefault);
            this.Soldermask = SoldermaskDefault;
            this.Rotation = new CoarseAngle(RotationDefault);
            this.Via = Via;
            this.Thermal = ThermalDefault;
            this.ThermalTracksWidth = ThermalTracksWidthDefault;
            this.ThermalTracksIndividual = ThermalTracksIndividualDefault;
            this.ThermalTracks = ThermalTracksDefault;
            this.PadID = PadId;
            this.Connections = new List<uint>(Connections);
        }

        public THTPad(Layer Layer, Point Position, Distance Size, Distance Drill, THTPadForm Form, CoarseAngle Rotation,
            bool Via, uint PadId = 0, params uint[] Connections)
        {
            this.Layer = Layer;
            this.Position = Position;
            this.Size = Size;
            this.Drill = Drill;
            this.Form = Form;

            this.Clear = new Distance(ClearDefault);
            this.Soldermask = SoldermaskDefault;
            this.Rotation = Rotation;
            this.Via = Via;
            this.Thermal = ThermalDefault;
            this.ThermalTracksWidth = ThermalTracksWidthDefault;
            this.ThermalTracksIndividual = ThermalTracksIndividualDefault;
            this.ThermalTracks = ThermalTracksDefault;
            this.PadID = PadId;
            this.Connections = new List<uint>(Connections);
        }

        public static bool Identify(TokenRow[] Tokens, uint Pointer)
        {
            // Input sanity check
            if (Tokens == null)
                return false;

            // First, make sure we have met the amount of required arguments
            if (Tokens[Pointer].Count < RequiredArgCount + 1)
                return false;

            // Also, check if the pointer is within range
            if (Pointer >= Tokens.Length)
                return false;

            // Then, make sure we actually have a PAD element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "PAD")
                return false;

            // Otherwise, it looks alright
            return true;
        }

        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out THTPad Result)
        {
            Result = null;

            // Check if we have got a valid signature
            if (!Identify(Tokens, Pointer))
                return false;

            // Now, check if we have got any duplicates. This would be a syntax error.
            if (Tokens[Pointer].HasDuplicates())
                return false;

            // Define the working variables
            THTPad pad = new THTPad();
            Token token;

            // Now, locate the required argument tokens and make sure they are present
            // LAYER
            if (!Tokens[Pointer].Get("LAYER", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Make sure the value is in range
            if (token.FirstValue < (uint)Layer.CopperTop || token.FirstValue > (uint)Layer.Mechanical)
                return false;
            // Store the value
            pad.Layer = (Layer)token.FirstValue;

            // POSITION
            if (!Tokens[Pointer].Get("POS", out token))
                return false;
            // Make sure it is a point
            if (token.Type != Token.TokenType.Tuple)
                return false;
            // Store the value
            pad.Position = new Vector(new Distance(token.FirstValue), new Distance(token.SecondValue));

            // SIZE
            if (!Tokens[Pointer].Get("SIZE", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Store the value
            pad.Size = new Distance(token.FirstValue);

            // DRILL
            if (!Tokens[Pointer].Get("DRILL", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Store the value
            pad.Drill = new Distance(token.FirstValue);

            // FORM
            if (!Tokens[Pointer].Get("FORM", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Make sure the value is in range
            if (token.FirstValue < (uint)THTPadForm.Round || token.FirstValue > (uint)THTPadForm.HighRectangular)
                return false;
            // Store the value
            pad.Form = (THTPadForm)token.FirstValue;

            // Now to the optional parameters
            uint optCount = 0;

            // CLEAR
            if (Tokens[Pointer].Get("CLEAR", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Store the value
                pad.Clear = new Distance(token.FirstValue);
                // Increment the optional argument count
                optCount++;
            }

            // SOLDERMASK
            if (Tokens[Pointer].Get("SOLDERMASK", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                pad.Soldermask = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // ROTATION
            if (Tokens[Pointer].Get("ROTATION", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                if (token.FirstValue > uint.MaxValue)
                    return false;
                // Store the value
                pad.Rotation = new CoarseAngle(token.FirstValue);
                // Increment the optional argument count
                optCount++;
            }

            // VIA
            if (Tokens[Pointer].Get("VIA", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                pad.Via = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // THERMAL
            if (Tokens[Pointer].Get("THERMAL", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                pad.Thermal = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // THERMAL_TRACKS_WIDTH
            if (Tokens[Pointer].Get("THERMAL_TRACKS_WIDTH", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Check range
                if (token.FirstValue > ushort.MaxValue)
                    return false;
                // Store the value
                pad.ThermalTracksWidth = (ushort)token.FirstValue;
                // Increment the optional argument count
                optCount++;
            }

            // THERMAL_TRACKS_INDIVIDUAL
            if (Tokens[Pointer].Get("THERMAL_TRACKS_INDIVIDUAL", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                pad.ThermalTracksIndividual = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // THERMAL_TRACKS
            if (Tokens[Pointer].Get("THERMAL_TRACKS", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Check range
                if (token.FirstValue > uint.MaxValue)
                    return false;
                // Store the value
                pad.ThermalTracks = (THTPadThermalTracks)token.FirstValue;
                // Increment the optional argument count
                optCount++;
            }

            // PAD_ID
            if (Tokens[Pointer].Get("PAD_ID", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Store the value
                pad.PadID = (uint)token.FirstValue;
                // Increment the optional argument count
                optCount++;
            }

            // CONx
            // Set up the array
            Tokens[Pointer].ArrayPointer = 0;
            Tokens[Pointer].ArrayPrefix = "CON";
            // Loop through all points
            uint connCount = 0;
            // Loop through all connections
            while (Tokens[Pointer].ArrayGet(out token))
            {
                // Increase the connection count
                connCount++;
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Add the new connection to the list
                pad.Connections.Add((uint)token.FirstValue);
            }

            // Make sure all tokens have been consumed
            if (Tokens[Pointer].Count > RequiredArgCount + optCount + connCount + 1)
                return false;

            // Return the successful new element
            Result = pad;
            return true;
        }

        public bool Write(out TokenRow[] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Write the type first
            writer.Write(new Token("PAD", Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (int)Layer));
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
                writer.Write(new Token("FORM", (int)Form));
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
                writer.Write(new Token("THERMAL_TRACKS", (int)ThermalTracks));

            // PadID
            // TODO: Maybe this needs some automation to prevent duplicates?
            if (PadID != 0)
                writer.Write(new Token("PAD_ID", (int)PadID));

            // Connections
            if (Connections != null)
            {
                // Write all connections
                uint counter = 0;
                foreach (uint conn in Connections)
                {
                    if (conn == 0)
                        return false;
                    writer.Write(new Token(string.Format("CON{0}", counter), (int)conn));
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
