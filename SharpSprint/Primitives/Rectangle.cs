using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public struct Rectangle
    {
        public Size Width;
        public Size Height;

        public Rectangle(Size Width, Size Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public static Rectangle FromMillimeters(float Width, float Height)
        {
            return new Rectangle(Size.FromMillimeters(Width), Size.FromMillimeters(Height));
        }

        public static Rectangle FromInches(float Width, float Height)
        {
            return new Rectangle(Size.FromInches(Width), Size.FromInches(Height));
        }
    }
}
