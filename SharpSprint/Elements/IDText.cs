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

        public IDText(Layer Layer, Position Position, string Content, Distance Height, bool Visible = VisibleDefault)
            : base(Layer, Position, Content, Height)
        {
            this.Keyword = "ID_TEXT";
            this.IsComponentText = true;
            this.Visible = Visible;
            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Style = StyleDefault;
            this.Thickness = ThicknessDefault;
            this.Rotation = new CoarseAngle(RotationDefault);
            this.MirrorHorizontal = MirrorHorizontalDefault;
            this.MirrorVertical = MirrorVerticalDefault;
        }
    }
}
