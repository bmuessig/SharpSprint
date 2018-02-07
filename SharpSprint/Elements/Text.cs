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

        // Internal parameters for compatibility with ID_TEXT and VALUE_TEXT
        protected string Keyword;
        protected bool IsComponentText;
        protected bool IsVisible;

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

        public static bool Create(Token[][] Tokens, ref uint Pointer, out Text Result)
        {
            throw new NotImplementedException();
        }

        public bool Write(out IO.Token[][] Tokens)
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
