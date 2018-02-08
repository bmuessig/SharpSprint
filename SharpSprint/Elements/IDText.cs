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

        public IDText(Layer Layer, Position Position, string Content, Distance Height, bool Visible = VisibleDefault,
            TextStyle Style = StyleDefault, TextThickness Thickness = ThicknessDefault,
            bool MirrorHorizontal = MirrorHorizontalDefault, bool MirrorVertical = MirrorVerticalDefault)
            : base(Layer, Position, Content, Height, Style, Thickness, MirrorHorizontal, MirrorVertical)
        {
            this.Keyword = "ID_TEXT";
            this.IsComponentText = true;
            this.IsVisible = Visible;
        }

        public IDText(Layer Layer, Position Position, string Content, Distance Height, CoarseAngle Rotation,
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

            // Then, make sure we actually have a ID_TEXT element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "ID_TEXT")
                return false;

            // Otherwise, it looks alright
            return true;
        }

        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out IDText Result)
        {
            throw new NotImplementedException();
        }
    }
}
