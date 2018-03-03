using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.IO;
using SharpSprint.Points;

namespace SharpSprint.Elements
{
    public class Track : Element
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Distance Width { get; set; }
        public List<Point> Path { get; set; }

        // Optional parameters
        public Distance Clear { get; set; } // 4000
        public bool Cutout { get; set; } // False
        public bool Soldermask { get; set; } // False
        public bool FlatStart { get; set; } // False
        public bool FlatEnd { get; set; } // False

        // Default optional parameters
        private const uint ClearDefault = 4000;
        private const bool CutoutDefault = false;
        private const bool SoldermaskDefault = false;
        private const bool FlatStartDefault = false;
        private const bool FlatEndDefault = false;

        // Required and optional count
        private const byte RequiredArgCount = 4; // 2 Properties + 1 Path (min. 2 points)
        private const byte OptionalArgCount = 5;

        private Track()
        {
            this.Path = new List<Point>(Path);

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.FlatStart = FlatStartDefault;
            this.FlatEnd = FlatEndDefault;
        }

        public Track(Layer Layer, Distance Width, params Point[] Path)
        {
            this.Layer = Layer;
            this.Width = Width;
            this.Path = new List<Point>(Path);

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.FlatStart = FlatStartDefault;
            this.FlatEnd = FlatEndDefault;
        }

        public Track(Layer Layer, Distance Width, bool FlatStart, bool FlatEnd, params Point[] Path)
        {
            this.Layer = Layer;
            this.Width = Width;
            this.Path = new List<Point>(Path);

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.FlatStart = FlatStart;
            this.FlatEnd = FlatEnd;
        }

        public static bool Identify(TokenRow[] Tokens, uint Pointer)
        {
            // First, make sure we have met the amount of required arguments
            if (Tokens[Pointer].Count < RequiredArgCount + 1)
                return false;

            // Also, check if the pointer is within range
            if (Pointer >= Tokens.Length)
                return false;

            // Then, make sure we actually have a TRACK element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "TRACK")
                return false;

            // Otherwise, it looks alright
            return true;
        }

        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out Track Result)
        {
            Result = null;

            // Check if we have got a valid signature
            if (!Identify(Tokens, Pointer))
                return false;

            // Now, check if we have got any duplicates. This would be a syntax error.
            if (Tokens[Pointer].HasDuplicates())
                return false;

            // Define the working variables
            Track track = new Track();
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
            track.Layer = (Layer)token.FirstValue;

            // WIDTH
            if (!Tokens[Pointer].Get("WIDTH", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Store the value
            track.Width = new Distance(token.FirstValue);

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
                track.Path.Add(new Vector(new Distance(token.FirstValue), new Distance(token.SecondValue)));
            }
            // Make sure that we have at least 3 path points
            if (pointCount < 2)
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
                track.Clear = new Distance(token.FirstValue);
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
                track.Cutout = token.BoolValue;
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
                track.Soldermask = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // FLATSTART
            if (Tokens[Pointer].Get("FLATSTART", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                track.FlatStart = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // FLATEND
            if (Tokens[Pointer].Get("FLATEND", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                track.FlatEnd = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // Make sure all tokens have been consumed
            if (Tokens[Pointer].Count > RequiredArgCount + pointCount - 2 + optCount + 1)
                return false;

            // Return the successful new element
            Result = track;
            return true;
        }

        public bool Write(out TokenRow[] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Write the type first
            writer.Write(new Token("TRACK", Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (uint)Layer));
            else
                return false;

            // Width
            writer.Write(new Token("WIDTH", Width.Value));

            // Points
            if (Path.Count >= 2)
            {
                for (int counter = 0; counter < Path.Count; counter++)
                {
                    if (counter > 0)
                    {
                        if ((Path[counter].X.Value == Path[counter - 1].X.Value) && (Path[counter].Y.Value == Path[counter - 1].Y.Value))
                            continue;
                    }
                    writer.Write(new Token(string.Format("P{0}", counter), Path[counter].X.Value, Path[counter].Y.Value));
                    counter++;
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

            // FlatStart
            if (FlatStart != FlatStartDefault)
                writer.Write(new Token("FLATSTART", FlatStart));

            // FlatEnd
            if (FlatEnd != FlatEndDefault)
                writer.Write(new Token("FLATEND", FlatEnd));

            Tokens = writer.Compile();
            return true;
        }
    }
}
