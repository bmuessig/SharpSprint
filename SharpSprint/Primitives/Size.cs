using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class Size
    {
        public Distance Width;
        public Distance Height;

        public Size(Distance Width, Distance Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public static Size FromMillimeters(float Width, float Height)
        {
            return new Size(Distance.FromMillimeters(Width), Distance.FromMillimeters(Height));
        }

        public static Size FromInches(float Width, float Height)
        {
            return new Size(Distance.FromInches(Width), Distance.FromInches(Height));
        }
    }
}
