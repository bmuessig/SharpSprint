using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;

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
    }
}
