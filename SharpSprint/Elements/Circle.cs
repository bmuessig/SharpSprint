using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.IO;
using SharpSprint.Primitives;
using SharpSprint.Points;

namespace SharpSprint.Elements
{
    public class Circle : Element
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Distance Width { get; set; }
        public Point Center { get; set; }
        public Distance Radius { get; set; }

        // Optional parameters
        public Distance Clear { get; set; } // 4000
        public bool Cutout { get; set; } // False
        public bool Soldermask { get; set; } // False
        public FineAngle Start { get; set; } // 0
        public FineAngle Stop { get; set; } // 0
        public bool Fill { get; set; } // False

        // Default optional parameters
        private const uint ClearDefault = 4000;
        private const bool CutoutDefault = false;
        private const bool SoldermaskDefault = false;
        private const uint StartDefault = 0;
        private const uint StopDefault = 0;
        private const bool FillDefault = false;

        // Required and optional count
        private const byte RequiredArgCount = 4;
        private const byte OptionalArgCount = 6;

        private Circle()
        {
            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Start = new FineAngle(StartDefault);
            this.Stop = new FineAngle(StopDefault);
            this.Fill = FillDefault;
        }

        public Circle(Layer Layer, Distance Width, Point Center, Distance Radius, bool Fill = FillDefault)
        {
            this.Layer = Layer;
            this.Width = Width;
            this.Center = Center;
            this.Radius = Radius;

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Start = new FineAngle(StartDefault);
            this.Stop = new FineAngle(StopDefault);
            this.Fill = Fill;
        }

        public Circle(Layer Layer, Distance Width, Point Center, Distance Radius, FineAngle Start, FineAngle Stop, bool Fill = FillDefault)
        {
            this.Layer = Layer;
            this.Width = Width;
            this.Center = Center;
            this.Radius = Radius;

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Start = new FineAngle(StartDefault);
            this.Stop = new FineAngle(StopDefault);
            this.Fill = Fill;
        }

        public static bool Identify(TokenRow[] Tokens, uint Pointer)
        {
            // First, make sure we have met the amount of required arguments
            if (Tokens[Pointer].Count < RequiredArgCount + 1)
                return false;

            // Also, check if the pointer is within range
            if (Pointer >= Tokens.Length)
                return false;

            // Then, make sure we actually have a CIRCLE element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "CIRCLE")
                return false;

            // Otherwise, it looks alright
            return true;
        }
        
        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out Circle Result)
        {
            Result = null;

            // Check if we have got a valid signature
            if (!Identify(Tokens, Pointer))
                return false;

            // Now, check if we have got any duplicates. This would be a syntax error.
            if (Tokens[Pointer].HasDuplicates())
                return false;

            // Define the working variables
            Circle circle = new Circle();
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
            circle.Layer = (Layer)token.FirstValue;

            // WIDTH
            if (!Tokens[Pointer].Get("WIDTH", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Store the value
            circle.Width = new Distance(token.FirstValue);

            // CENTER
            if (!Tokens[Pointer].Get("CENTER", out token))
                return false;
            // Make sure it is a point
            if (token.Type != Token.TokenType.Tuple)
                return false;
            // Store the value
            circle.Center = new Point(new Distance(token.FirstValue), new Distance(token.SecondValue));

            // RADIUS
            if (!Tokens[Pointer].Get("RADIUS", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Store the value
            circle.Radius = new Distance(token.FirstValue);

            // Now to the optional parameters
            uint optCount = 0;

            // CLEAR
            if (Tokens[Pointer].Get("CLEAR", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Store the value
                circle.Clear = new Distance(token.FirstValue);
                // Increment the optional argument count
                optCount++;
            }

            // CUTOUT
            if (Tokens[Pointer].Get("CUTOUT", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                circle.Cutout = token.BoolValue;
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
                circle.Soldermask = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // START
            if (Tokens[Pointer].Get("START", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                if (token.FirstValue > uint.MaxValue)
                    return false;
                // Store the value
                circle.Start = new FineAngle((uint)token.FirstValue);
                // Increment the optional argument count
                optCount++;
            }

            // STOP
            if (Tokens[Pointer].Get("STOP", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                if (token.FirstValue > uint.MaxValue)
                    return false;
                // Store the value
                circle.Stop = new FineAngle((uint)token.FirstValue);
                // Increment the optional argument count
                optCount++;
            }

            // FILL
            if (Tokens[Pointer].Get("FILL", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                circle.Fill = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // Make sure all tokens have been consumed
            if (Tokens[Pointer].Count > RequiredArgCount + optCount + 1)
                return false;

            // Return the successful new element
            Result = circle;
            return true;
        }
        
        public bool Write(out TokenRow[] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Write the type first
            writer.Write(new Token("CIRCLE", Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (uint)Layer));
            else
                return false;

            // Width
            writer.Write(new Token("WIDTH", Width.Value));

            // Center
            writer.Write(new Token("CENTER", Center.X.Value, Center.Y.Value));

            // Radius
            writer.Write(new Token("RADIUS", Radius.Value));

            // Then write the optional values
            // Clear
            if (Clear.Value != ClearDefault)
                writer.Write(new Token("CLEAR", Clear.Value));

            // Cutout
            if (Cutout != CutoutDefault)
                writer.Write(new Token("CUTOUT", Cutout));

            // Soldermask
            if (Soldermask != SoldermaskDefault)
                writer.Write(new Token("SOLDERMASK", Soldermask));

            // Start
            if (Start.Value != StartDefault)
                writer.Write(new Token("START", Start.Value));

            // Stop
            if (Stop.Value != StopDefault)
                writer.Write(new Token("STOP", Stop.Value));
            
            // Fill
            if (Fill != FillDefault)
                writer.Write(new Token("FILL", Fill));

            Tokens = writer.Compile();
            return true;
        }

        public class CircleTrackPoint : Point
        {
            public Circle Circle { get; set; }

            public TrackPointPosition Position { get; set; }

            public new Distance X
            {
                get
                {
                    switch (Position)
                    {
                        case TrackPointPosition.Center:
                            return Circle.Center.X;
                        case TrackPointPosition.Start:
                            return new Distance((uint)Math.Round(Circle.Center.X.Value +
                                (Circle.Radius.Value * (decimal)Math.Cos(Circle.Start.Radians)), 0));
                        case TrackPointPosition.Stop:
                            return new Distance((uint)Math.Round(Circle.Center.X.Value +
                                (Circle.Radius.Value * (decimal)Math.Cos(Circle.Stop.Radians)), 0));
                    }

                    return null;
                }

                set
                {
                    switch (Position)
                    {
                        case TrackPointPosition.Center:
                            Circle.Center.X = value;
                            return;
                        case TrackPointPosition.Start:
                            //TODO
                            return;
                        case TrackPointPosition.Stop:
                            //TODO
                            return;
                    }
                }
            }

            public new Distance Y
            {
                get
                {
                    switch (Position)
                    {
                        case TrackPointPosition.Center:
                            return Circle.Center.Y;
                        case TrackPointPosition.Start:
                            return new Distance((uint)Math.Round(Circle.Center.Y.Value +
                                (Circle.Radius.Value * (decimal)Math.Sin(Circle.Start.Radians)), 0));
                        case TrackPointPosition.Stop:
                            return new Distance((uint)Math.Round(Circle.Center.Y.Value +
                                (Circle.Radius.Value * (decimal)Math.Sin(Circle.Stop.Radians)), 0));
                    }

                    return null;
                }

                set
                {
                    switch (Position)
                    {
                        case TrackPointPosition.Center:
                            Circle.Center.Y = value;
                            return;
                        case TrackPointPosition.Start:
                            //TODO
                            return;
                        case TrackPointPosition.Stop:
                            //TODO
                            return;
                    }
                }
            }

            public CircleTrackPoint(Circle Circle, TrackPointPosition Position)
            {
                this.Circle = Circle;
                this.Position = Position;
            }

            public enum TrackPointPosition : byte
            {
                Center,
                Start,
                Stop
            }
        }
    }
}
