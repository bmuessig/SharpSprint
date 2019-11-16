using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.IO;
using SharpSprint.Points;

namespace SharpSprint.Elements
{
    public class Zone : Multipoint
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Distance Width { get; set; }
        public List<Point> Path { get; set; }

        // Optional parameters
        public Distance Clear { get; set; } // 4000
        public bool Cutout { get; set; } // False
        public bool Soldermask { get; set; } // False
        public bool Hatch { get; set; } // False
        public bool HatchAuto { get; set; } // True
        public Distance HatchWidth { get; set; }

        // Default optional parameters
        private const int ClearDefault = 4000;
        private const bool CutoutDefault = false;
        private const bool SoldermaskDefault = false;
        private const bool HatchDefault = false;
        private const bool HatchAutoDefault = true;

        // Required and optional count
        private const byte RequiredArgCount = 5; // 2 Properties + 1 Path (3 points min)
        private const byte OptionalArgCount = 6;

        private Zone()
        {
            this.Path = new List<Point>();

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Hatch = HatchDefault;
            this.HatchAuto = HatchAutoDefault;
            this.HatchWidth = new Distance(0);
        }

        public Zone(Layer Layer, Distance Width, params Point[] Path)
        {
            this.Layer = Layer;
            this.Width = Width;
            this.Path = new List<Point>(Path);

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Hatch = HatchDefault;
            this.HatchAuto = HatchAutoDefault;
            this.HatchWidth = new Distance(0);
        }

        public Zone(Layer Layer, Distance Width, bool Hatch, params Point[] Path)
        {
            this.Layer = Layer;
            this.Width = Width;
            this.Path = new List<Point>(Path);

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Hatch = Hatch;
            this.HatchAuto = HatchAutoDefault;
            this.HatchWidth = new Distance(0);
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

            // Then, make sure we actually have a ZONE element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "ZONE")
                return false;

            // Otherwise, it looks alright
            return true;
        }
        
        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out Zone Result)
        {
            Result = null;

            // Check if we have got a valid signature
            if (!Identify(Tokens, Pointer))
                return false;

            // Now, check if we have got any duplicates. This would be a syntax error.
            if (Tokens[Pointer].HasDuplicates())
                return false;

            // Define the working variables
            Zone zone = new Zone();
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
            zone.Layer = (Layer)token.FirstValue;

            // WIDTH
            if (!Tokens[Pointer].Get("WIDTH", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Store the value
            zone.Width = new Distance(token.FirstValue);

            // PATH
            // Set up the array
            Tokens[Pointer].ArrayPointer = 0;
            Tokens[Pointer].ArrayPrefix = "P";
            // Loop through all points
            uint pointCount = 0;
            while (Tokens[Pointer].ArrayGet(out token))
            {
                // Increase the point counter
                pointCount++;
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Tuple)
                    return false;
                // Add the new point to the list
                zone.Path.Add(new Vector(new Distance(token.FirstValue), new Distance(token.SecondValue)));
            }
            // Make sure that we have at least 3 path points
            if (pointCount < 3)
                return false;

            // Now to the optional parameters
            uint optCount = 0;

            // CLEAR
            if (Tokens[Pointer].Get("CLEAR", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Store the value
                zone.Clear = new Distance(token.FirstValue);
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
                zone.Cutout = token.BoolValue;
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
                zone.Soldermask = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // HATCH
            if (Tokens[Pointer].Get("HATCH", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                zone.Hatch = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // HATCH_AUTO
            if (Tokens[Pointer].Get("HATCH_AUTO", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                zone.HatchAuto = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // HATCH_WIDTH
            if (Tokens[Pointer].Get("HATCH_WIDTH", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Store the value
                zone.HatchWidth = new Distance(token.FirstValue);
                // Increment the optional argument count
                optCount++;
            }
            else if (!zone.HatchAuto) // If HATCH_AUTO is manually disabled, HATCH_WIDTH is required
                return false;

            // Make sure all tokens have been consumed
            if (Tokens[Pointer].Count > RequiredArgCount + pointCount - 3 + optCount + 1)
                return false;

            // Return the successful new element
            Result = zone;
            return true;
        }

        public bool Write(out TokenRow[] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Write the type first
            writer.Write(new Token("ZONE", Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (int)Layer));
            else
                return false;

            // Width
            writer.Write(new Token("WIDTH", Width.Value));

            // Points
            if (Path.Count >= 3)
            {
                for (int counter = 0; counter < Path.Count; counter++)
                {
                    if (counter > 0)
                    {
                        if ((Path[counter].X.Value == Path[counter - 1].X.Value) && (Path[counter].Y.Value == Path[counter - 1].Y.Value))
                            continue;
                    }
                    writer.Write(new Token(string.Format("P{0}", counter), Path[counter].X.Value, Path[counter].Y.Value));
                }
            }
            else
                return false;

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

            // Hatch
            if (Hatch != HatchDefault)
                writer.Write(new Token("HATCH", Hatch));

            // HatchAuto
            if (HatchAuto != HatchAutoDefault)
                writer.Write(new Token("HATCH_AUTO", HatchAuto));

            // HatchWidth
            if (!HatchAuto)
                writer.Write(new Token("HATCH_WIDTH", HatchWidth.Value));

            Tokens = writer.Compile();
            return true;
        }
    }
}
