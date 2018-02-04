using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class Size
    {
        public Dist Width;
        public Dist Height;

        public Size(Dist Width, Dist Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public static Size FromMillimeters(float Width, float Height)
        {
            return new Size(Dist.FromMillimeters(Width), Dist.FromMillimeters(Height));
        }

        public static Size FromInches(float Width, float Height)
        {
            return new Size(Dist.FromInches(Width), Dist.FromInches(Height));
        }
    }
}
