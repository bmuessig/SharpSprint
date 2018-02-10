using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.IO;

namespace SharpSprint.Elements
{
    public class Text : Element
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Position Position { get; set; }
        public string Content { get; set; }
        public Distance Height { get; set; }

        // Optional parameters
        public Distance Clear { get; set; } // 4000
        public bool Cutout { get; set; } // False
        public bool Soldermask { get; set; } // False
        public TextStyle Style { get; set; } // Normal
        public TextThickness Thickness { get; set; } // Normal
        public CoarseAngle Rotation { get; set; } // 0
        public bool MirrorHorizontal { get; set; } // False
        public bool MirrorVertical { get; set; } // False

        // Default optional parameters
        protected const uint ClearDefault = 4000;
        protected const bool CutoutDefault = false;
        protected const bool SoldermaskDefault = false;
        protected const TextStyle StyleDefault = TextStyle.Normal;
        protected const TextThickness ThicknessDefault = TextThickness.Normal;
        protected const uint RotationDefault = 0;
        protected const bool MirrorHorizontalDefault = false;
        protected const bool MirrorVerticalDefault = false;
        protected const bool VisibleDefault = true;

        // Required and optional count
        protected const byte RequiredArgCount = 4;
        private const byte OptionalArgCount = 8;

        // Internal parameters for compatibility with ID_TEXT and VALUE_TEXT
        protected string Keyword;
        protected bool IsComponentText;
        protected bool IsVisible;

        protected Text()
        {
            this.Content = string.Empty;

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Style = StyleDefault;
            this.Thickness = ThicknessDefault;
            this.Rotation = new CoarseAngle(RotationDefault);
            this.MirrorHorizontal = MirrorHorizontalDefault;
            this.MirrorVertical = MirrorVerticalDefault;
        }

        public Text(Layer Layer, Position Position, string Content, Distance Height, TextStyle Style = StyleDefault,
            TextThickness Thickness = ThicknessDefault, bool MirrorHorizontal = MirrorHorizontalDefault,
                bool MirrorVertical = MirrorVerticalDefault)
        {
            this.IsComponentText = false;
            this.Layer = Layer;
            this.Position = Position;
            this.Content = Content;
            this.Height = Height;

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Style = Style;
            this.Thickness = Thickness;
            this.Rotation = new CoarseAngle(RotationDefault);
            this.MirrorHorizontal = MirrorHorizontal;
            this.MirrorVertical = MirrorVertical;
        }

        public Text(Layer Layer, Position Position, string Content, Distance Height, CoarseAngle Rotation,
            TextStyle Style = StyleDefault, TextThickness Thickness = ThicknessDefault,
            bool MirrorHorizontal = MirrorHorizontalDefault, bool MirrorVertical = MirrorVerticalDefault)
        {
            this.IsComponentText = false;
            this.Layer = Layer;
            this.Position = Position;
            this.Content = Content;
            this.Height = Height;

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Style = Style;
            this.Thickness = Thickness;
            this.Rotation = Rotation;
            this.MirrorHorizontal = MirrorHorizontal;
            this.MirrorVertical = MirrorVertical;
        }

        public static bool Identify(TokenRow[] Tokens, uint Pointer)
        {
            // First, make sure we have met the amount of required arguments
            if (Tokens[Pointer].Count < RequiredArgCount + 1)
                return false;

            // Then, make sure we actually have a TEXT element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "TEXT")
                return false;

            // Otherwise, it looks alright
            return true;
        }

        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out Text Result)
        {
            Result = null;

            // Check if we have got a valid signature
            if (!Identify(Tokens, Pointer))
                return false;

            return Read(Tokens, false, ref Pointer, out Result);
        }

        protected static bool Read(TokenRow[] Tokens, bool AllowVisibility, ref uint Pointer, out Text Result)
        {
            Result = null;

            // Check if the pointer is within range
            if (Pointer >= Tokens.Length)
                return false;

            // Now, check if we have got any duplicates. This would be a syntax error.
            if (Tokens[Pointer].HasDuplicates())
                return false;

            // Define the working variables
            Text text = new Text();
            Token token;

            // Now, locate the required argument tokens and make sure they are present
            // LAYER
            if (!Tokens[Pointer].Get("LAYER", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Make sure the value is in range
            if (token.FirstValue < (ulong)Layer.CopperTop || token.FirstValue > (ulong)Layer.Mechanical)
                return false;
            // Store the value
            text.Layer = (Layer)token.FirstValue;

            // POSITION
            if (!Tokens[Pointer].Get("POS", out token))
                return false;
            // Make sure it is a point
            if (token.Type != Token.TokenType.Tuple)
                return false;
            // Store the value
            text.Position = new Position(new Distance(token.FirstValue), new Distance(token.SecondValue));

            // CONTENT
            if (!Tokens[Pointer].Get("TEXT", out token))
                return false;
            // Make sure it is a text value
            if (token.Type != Token.TokenType.Text)
                return false;
            // Store the value
            text.Content = token.TextValue;

            // HEIGHT
            if (!Tokens[Pointer].Get("HEIGHT", out token))
                return false;
            // Make sure it is a numeric value
            if (token.Type != Token.TokenType.Value)
                return false;
            // Store the value
            text.Height = new Distance(token.FirstValue);

            // Now to the optional parameters
            uint optCount = 0;

            // CLEAR
            if (Tokens[Pointer].Get("CLEAR", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Store the value
                text.Clear = new Distance(token.FirstValue);
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
                text.Cutout = token.BoolValue;
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
                text.Soldermask = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // STYLE
            if (Tokens[Pointer].Get("STYLE", out token))
            {
                // Make sure it is a numeric value
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Make sure the value is in range
                if (token.FirstValue > (ulong)TextStyle.Wide)
                    return false;
                // Store the value
                text.Style = (TextStyle)token.FirstValue;
                // Increment the optional argument count
                optCount++;
            }

            // THICKNESS
            if (Tokens[Pointer].Get("THICKNESS", out token))
            {
                // Make sure it is a numeric value
                if (token.Type != Token.TokenType.Value)
                    return false;
                // Make sure the value is in range
                if (token.FirstValue > (ulong)TextThickness.Thick)
                    return false;
                // Store the value
                text.Thickness = (TextThickness)token.FirstValue;
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
                text.Rotation = new CoarseAngle((uint)token.FirstValue);
                // Increment the optional argument count
                optCount++;
            }

            // MIRROR_HORZ
            if (Tokens[Pointer].Get("MIRROR_HORZ", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                text.MirrorHorizontal = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // MIRROR_VERT
            if (Tokens[Pointer].Get("MIRROR_VERT", out token))
            {
                // Make sure we have got the correct type
                if (token.Type != Token.TokenType.Boolean)
                    return false;
                // Store the value
                text.MirrorVertical = token.BoolValue;
                // Increment the optional argument count
                optCount++;
            }

            // VISIBLE
            if (AllowVisibility)
            {
                if (Tokens[Pointer].Get("VISIBLE", out token))
                {
                    // Make sure we have got the correct type
                    if (token.Type != Token.TokenType.Boolean)
                        return false;
                    // Store the value
                    text.IsVisible = token.BoolValue;
                    // Increment the optional argument count
                    optCount++;
                }
            }

            // Make sure all tokens have been consumed
            if (Tokens[Pointer].Count > RequiredArgCount + optCount + 1)
                return false;

            // Return the successful new element
            Result = text;
            return true;
        }

        public bool Write(out TokenRow[] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            if (string.IsNullOrWhiteSpace(Keyword))
                Keyword = "TEXT";

            // Write the type first
            writer.Write(new Token(Keyword, Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (ulong)Layer));
            else
                return false;

            // ComponentVisible
            if (IsComponentText && IsVisible != VisibleDefault)
                writer.Write(new Token("VISIBLE", IsVisible));

            // Position
            writer.Write(new Token("POS", Position.X.Value, Position.Y.Value));

            // Height
            writer.Write(new Token("HEIGHT", Height.Value));

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

            // Style
            if (Style != StyleDefault)
                writer.Write(new Token("STYLE", (ulong)Style));

            // Thickness
            if (Thickness != ThicknessDefault)
                writer.Write(new Token("THICKNESS", (ulong)Thickness));

            // Rotation
            if (Rotation.Value != RotationDefault)
                writer.Write(new Token("ROTATION", Rotation.Value));

            // MirrorHorizontal
            if (MirrorHorizontal != MirrorHorizontalDefault)
                writer.Write(new Token("MIRROR_HORZ", MirrorHorizontal));

            // MirrorVertical
            if (MirrorVertical != MirrorVerticalDefault)
                writer.Write(new Token("MIRROR_VERT", MirrorVertical));

            // Content
            writer.Write(new Token("TEXT", Content));

            Tokens = writer.Compile();
            return true;
        }

        public enum TextStyle : byte
        {
            Narrow = 0,
            Normal = 1,
            Wide = 2
        }

        public enum TextThickness : byte
        {
            Thin = 0,
            Normal = 1,
            Thick = 2
        }
    }
}
