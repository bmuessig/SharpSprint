using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.IO;

namespace SharpSprint.Elements
{
    public class IDText : Text
    {
        // Optional parameters
        public bool Visible
        {
            get { return IsVisible; }
            set { IsVisible = value; }
        }

        // Optional count
        private const byte OptionalArgCount = 9;

        private IDText(Text Base)
            : base(Base.Layer, Base.Position, Base.Content, Base.Height)
        {
            this.Keyword = "ID_TEXT";
            this.IsComponentText = true;

            this.Clear = Base.Clear;
            this.Cutout = Base.Cutout;
            this.Soldermask = Base.Soldermask;
            this.Style = Base.Style;
            this.Thickness = Base.Thickness;
            this.Rotation = Base.Rotation;
            this.MirrorHorizontal = Base.MirrorHorizontal;
            this.MirrorVertical = Base.MirrorVertical;
        }

        public IDText(Layer Layer, Point Position, string Content, Distance Height, bool Visible = VisibleDefault,
            TextStyle Style = StyleDefault, TextThickness Thickness = ThicknessDefault,
            bool MirrorHorizontal = MirrorHorizontalDefault, bool MirrorVertical = MirrorVerticalDefault)
            : base(Layer, Position, Content, Height, Style, Thickness, MirrorHorizontal, MirrorVertical)
        {
            this.Keyword = "ID_TEXT";
            this.IsComponentText = true;
            this.IsVisible = Visible;
        }

        public IDText(Layer Layer, Point Position, string Content, Distance Height, CoarseAngle Rotation,
            bool Visible = VisibleDefault, TextStyle Style = StyleDefault, TextThickness Thickness = ThicknessDefault,
            bool MirrorHorizontal = MirrorHorizontalDefault, bool MirrorVertical = MirrorVerticalDefault)
            : base(Layer, Position, Content, Height, Rotation, Style,Thickness, MirrorHorizontal, MirrorVertical)
        {
            this.Keyword = "ID_TEXT";
            this.IsComponentText = true;
            this.IsVisible = Visible;
        }

        public static new bool Identify(TokenRow[] Tokens, uint Pointer)
        {
            // First, make sure we have met the amount of required arguments
            if (Tokens[Pointer].Count < RequiredArgCount + 1)
                return false;

            // Also, check if the pointer is within range
            if (Pointer >= Tokens.Length)
                return false;

            // Then, make sure we actually have a ID_TEXT element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "ID_TEXT")
                return false;

            // Otherwise, it looks alright
            return true;
        }

        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out IDText Result)
        {
            Result = null;

            // Check if we have got a valid signature
            if (!Identify(Tokens, Pointer))
                return false;

            // Read the default text properties with visibility checking enabled
            Text text;
            if (!Read(Tokens, true, ref Pointer, out text))
                return false;

            Result = new IDText(text);
            return true;
        }
    }
}
