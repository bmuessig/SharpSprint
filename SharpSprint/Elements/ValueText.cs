using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.IO;

namespace SharpSprint.Elements
{
    public class ValueText : Text
    {
        // Optional parameters
        public bool Visible
        {
            get { return IsVisible; }
            set { IsVisible = value; }
        }

        public ValueText(Layer Layer, Position Position, string Content, Distance Height, bool Visible = VisibleDefault,
            TextStyle Style = StyleDefault, TextThickness Thickness = ThicknessDefault,
            bool MirrorHorizontal = MirrorHorizontalDefault, bool MirrorVertical = MirrorVerticalDefault)
            : base(Layer, Position, Content, Height, Style, Thickness, MirrorHorizontal, MirrorVertical)
        {
            this.Keyword = "VALUE_TEXT";
            this.IsComponentText = true;
            this.IsVisible = Visible;
        }

        public ValueText(Layer Layer, Position Position, string Content, Distance Height, CoarseAngle Rotation,
            bool Visible = VisibleDefault, TextStyle Style = StyleDefault, TextThickness Thickness = ThicknessDefault,
            bool MirrorHorizontal = MirrorHorizontalDefault, bool MirrorVertical = MirrorVerticalDefault)
            : base(Layer, Position, Content, Height, Rotation, Style,Thickness, MirrorHorizontal, MirrorVertical)
        {
            this.Keyword = "VALUE_TEXT";
            this.IsComponentText = true;
            this.IsVisible = Visible;
        }

        public static bool Create(Token[][] Tokens, ref uint Pointer, out ValueText Result)
        {
            throw new NotImplementedException();
        }
    }
}
