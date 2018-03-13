using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.IO;
using SharpSprint.Points;

namespace SharpSprint.Elements
{
    public class SMDPad : Pad
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Point Position { get; set; }
        public Distance Width { get; set; }
        public Distance Height { get; set; }

        // Optional parameters
        public Distance Clear { get; set; } // 4000
        public bool Soldermask { get; set; } // True
        public CoarseAngle Rotation { get; set; } // 0
        public bool Thermal { get; set; } // False
        public ushort ThermalTracksWidth { get; set; } // 100
        public SMDPadThermalTracks ThermalTracks { get; set; }
        public uint PadID { get; set; }
        public List<uint> Connections { get; set; }

        // Default optional parameters
        private const uint ClearDefault = 4000;
        private const bool SoldermaskDefault = true;
        private const uint RotationDefault = 0;
        private const bool ThermalDefault = false;
        private const ushort ThermalTracksWidthDefault = 100;

        // Required and optional count
        private const byte RequiredArgCount = 4;
        private const byte OptionalArgCount = 7;

        private SMDPad()
        {
            this.Clear = new Distance(ClearDefault);
            this.Soldermask = SoldermaskDefault;
            this.Rotation = new CoarseAngle(RotationDefault);
            this.Thermal = ThermalDefault;
            this.ThermalTracksWidth = ThermalTracksWidthDefault;
            this.ThermalTracks = SMDPadThermalTracks.None;
            this.PadID = 0;
            this.Connections = new List<uint>();
        }

        public SMDPad(Layer Layer, Point Position, Distance Width, Distance Height, uint PadId = 0, params uint[] Connections)
        {
            this.Layer = Layer;
            this.Position = Position;
            this.Width = Width;
            this.Height = Height;

            this.Clear = new Distance(ClearDefault);
            this.Soldermask = SoldermaskDefault;
            this.Rotation = new CoarseAngle(RotationDefault);
            this.Thermal = ThermalDefault;
            this.ThermalTracksWidth = ThermalTracksWidthDefault;
            this.ThermalTracks = SMDPadThermalTracks.None;
            this.PadID = PadId;
            this.Connections = new List<uint>(Connections);
        }

        public SMDPad(Layer Layer, Point Position, Distance Width, Distance Height, CoarseAngle Rotation, uint PadId = 0, params uint[] Connections)
        {
            this.Layer = Layer;
            this.Position = Position;
            this.Width = Width;
            this.Height = Height;

            this.Clear = new Distance(ClearDefault);
            this.Soldermask = SoldermaskDefault;
            this.Rotation = Rotation;
            this.Thermal = ThermalDefault;
            this.ThermalTracksWidth = ThermalTracksWidthDefault;
            this.ThermalTracks = SMDPadThermalTracks.None;
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

            // Then, make sure we actually have a SMDPAD element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "SMDPAD")
                return false;

            // Otherwise, it looks alright
            return true;
        }
        
        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out SMDPad Result)
        {
            Result = null;

            // Check if we have got a valid signature
            if (!Identify(Tokens, Pointer))
                return false;

            // Now, check if we have got any duplicates. This would be a syntax error.
            if (Tokens[Pointer].HasDuplicates())
                return false;

            // Define the working variables
            SMDPad smdpad = new SMDPad();
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
            smdpad.Layer = (Layer)token.FirstValue;

            // POSITION
            if (!Tokens[Pointer].Get("POS", out token))
                return false;
            // Make sure it is a point
            if (token.Type != Token.TokenType.Tuple)
                return false;
            // Store the value
            smdpad.Position = new Vector(new Distance(token.FirstValue), new Distance(token.SecondValue));

            // SIZE_X
            if (!Tokens[Pointer].Get("SIZE_X", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Store the value
            smdpad.Width = new Distance(token.FirstValue);

            // SIZE_Y
            if (!Tokens[Pointer].Get("SIZE_Y", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Store the value
            smdpad.Height = new Distance(token.FirstValue);

            // Now to the optional parameters
            uint optCount = 0;

            // CLEAR
            if (Tokens[Pointer].Get("CLEAR", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Store the value
                smdpad.Clear = new Distance(token.FirstValue);
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
                smdpad.Soldermask = token.BoolValue;
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
                smdpad.Rotation = new CoarseAngle((uint)token.FirstValue);
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
                smdpad.Thermal = token.BoolValue;
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
                smdpad.ThermalTracksWidth = (ushort)token.FirstValue;
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
                if (token.FirstValue > byte.MaxValue)
                    return false;
                // Store the value
                smdpad.ThermalTracks = (SMDPadThermalTracks)token.FirstValue;
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
                smdpad.PadID = token.FirstValue;
                // Increment the optional argument count
                optCount++;
            }

            // CONx
            // Set up the array
            Tokens[Pointer].ArrayPointer = 0;
            Tokens[Pointer].ArrayPrefix = "CON";
            // Loop through all connections
            uint connCount = 0;
            while (Tokens[Pointer].ArrayGet(out token))
            {
                // Increase the connection count
                connCount++;
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Add the new connection to the list
                smdpad.Connections.Add(token.FirstValue);
            }

            // Make sure all tokens have been consumed
            if (Tokens[Pointer].Count > RequiredArgCount + optCount + connCount + 1)
                return false;

            // Return the successful new element
            Result = smdpad;
            return true;
        }

        public bool Write(out TokenRow[] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Write the type first
            writer.Write(new Token("SMDPAD", Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (uint)Layer));
            else
                return false;

            // Position
            writer.Write(new Token("POS", Position.X.Value, Position.Y.Value));

            // Size
            writer.Write(new Token("SIZE_X", Width.Value));
            writer.Write(new Token("SIZE_Y", Height.Value));

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
                writer.Write(new Token("THERMAL_TRACKS", (uint)ThermalTracks));

            // PadID
            // TODO: Maybe this needs some automation to prevent duplicates?
            if (PadID != 0)
                writer.Write(new Token("PAD_ID", PadID));

            // Connections
            if (Connections != null)
            {
                // Write all connections
                uint counter = 0;
                foreach (uint conn in Connections)
                {
                    if (conn == 0)
                        return false;
                    writer.Write(new Token(string.Format("CON{0}", counter), conn));
                    counter++;
                }
            }

            Tokens = writer.Compile();
            return true;
        }

        public class SMDPadTrackPoint : Point
        {
            public SMDPad Pad { get; set; }

            public TrackPointPosition Position { get; set; }

            public new Distance X
            {
                get
                {
                    // TODO: Add rotational translations below

                    if (Position == TrackPointPosition.CenterMiddle)
                        return Pad.Position.X;

                    if (Pad.Rotation.Value == 0)
                    {
                        switch (Position)
                        {
                            case TrackPointPosition.TopLeft:
                            case TrackPointPosition.CenterLeft:
                            case TrackPointPosition.BottomLeft:
                                return Pad.Position.X - (Pad.Width / 2);

                            case TrackPointPosition.TopMiddle:
                            case TrackPointPosition.BottomMiddle:
                                return Pad.Position.X;

                            case TrackPointPosition.TopRight:
                            case TrackPointPosition.CenterRight:
                            case TrackPointPosition.BottomRight:
                                return Pad.Position.X + (Pad.Width / 2);
                        }
                    }
                    else
                    {

                    }

                    return null;
                }

                set
                {
                    if (Position == TrackPointPosition.CenterMiddle)
                    {
                        Pad.Position.X.Value = value.Value;
                        return;
                    }

                    if (Pad.Rotation.Value == 0)
                    {
                        switch (Position)
                        {
                            case TrackPointPosition.TopLeft:
                            case TrackPointPosition.CenterLeft:
                            case TrackPointPosition.BottomLeft:
                                Pad.Position.X.Value = (value + (Pad.Width / 2)).Value;
                                return;

                            case TrackPointPosition.TopMiddle:
                            case TrackPointPosition.BottomMiddle:
                                Pad.Position.X.Value = value.Value;
                                return;

                            case TrackPointPosition.TopRight:
                            case TrackPointPosition.CenterRight:
                            case TrackPointPosition.BottomRight:
                                Pad.Position.X.Value += (value - (Pad.Width / 2)).Value;
                                return;
                        }
                    }
                    else
                    {

                    }
                }
            }

            public new Distance Y
            {
                get
                {
                    if (Position == TrackPointPosition.CenterMiddle)
                        return Pad.Position.Y;

                    if (Pad.Rotation.Value == 0)
                    {
                        switch (Position)
                        {
                            case TrackPointPosition.TopLeft:
                            case TrackPointPosition.TopMiddle:
                            case TrackPointPosition.TopRight:
                                return Pad.Position.Y - (Pad.Height / 2);

                            case TrackPointPosition.CenterLeft:
                            case TrackPointPosition.CenterRight:
                                return Pad.Position.Y;

                            case TrackPointPosition.BottomLeft:
                            case TrackPointPosition.BottomMiddle:
                            case TrackPointPosition.BottomRight:
                                return Pad.Position.Y + (Pad.Height / 2);
                        }
                    }
                    else
                    {

                    }

                    return null;
                }

                set
                {
                    if (Position == TrackPointPosition.CenterMiddle)
                    {
                        Pad.Position.Y.Value = value.Value;
                        return;
                    }

                    if (Pad.Rotation.Value == 0)
                    {
                        switch (Position)
                        {
                            case TrackPointPosition.TopLeft:
                            case TrackPointPosition.TopMiddle:
                            case TrackPointPosition.TopRight:
                                Pad.Position.Y.Value = (value + (Pad.Height / 2)).Value;
                                return;

                            case TrackPointPosition.CenterLeft:
                            case TrackPointPosition.CenterRight:
                                Pad.Position.Y.Value = value.Value;
                                return;

                            case TrackPointPosition.BottomLeft:
                            case TrackPointPosition.BottomMiddle:
                            case TrackPointPosition.BottomRight:
                                Pad.Position.Y.Value = (value - (Pad.Height / 2)).Value;
                                return;
                        }
                    }
                    else
                    {

                    }
                }
            }

            public enum TrackPointPosition : byte
            {
                TopLeft,
                TopMiddle,
                TopRight,
                CenterLeft,
                CenterMiddle,
                CenterRight,
                BottomLeft,
                BottomMiddle,
                BottomRight
            }
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
