using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class Position
    {
        public Size X;
        public Size Y;

        public Position(Size X, Size Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Position FromMillimeters(float X, float Y)
        {
            return new Position(Size.FromMillimeters(X), Size.FromMillimeters(Y));
        }

        public static Position FromInches(float X, float Y)
        {
            return new Position(Size.FromInches(X), Size.FromInches(Y));
        }
    }
}
