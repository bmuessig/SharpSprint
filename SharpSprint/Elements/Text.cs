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
        public Dist Height { get; set; }

        // Optional parameters
        public Dist Clear { get; set; } // 4000
        public bool Cutout { get; set; } // False
        public bool Soldermask { get; set; } // False
        public TextStyle Style { get; set; } // Normal
        public TextThickness Thickness { get; set; } // Normal
        public CoarseAngle Rotation { get; set; } // 0
        public bool MirrorHorizontal { get; set; } // False
        public bool MirrorVertical { get; set; } // False

        // Default optional parameters
        private const uint ClearDefault = 4000;
        private const bool CutoutDefault = false;
        private const bool SoldermaskDefault = false;
        private const TextStyle StyleDefault = TextStyle.Normal;
        private const TextThickness ThicknessDefault = TextThickness.Normal;
        private const uint RotationDefault = 0;
        private const bool MirrorHorizontalDefault = false;
        private const bool MirrorVerticalDefault = false;

        public bool Read(IO.Token[][] Tokens, ref uint Pointer)
        {
            throw new NotImplementedException();
        }

        public bool Write(out IO.Token[][] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Write the type first
            writer.Write(new Token("TEXT", Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (ulong)Layer));
            else
                return false;

            // Position
            writer.Write(new Token("POS", Position.X.Value, Position.Y.Value));

            // Content
            writer.Write(new Token("TEXT", Content));

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
