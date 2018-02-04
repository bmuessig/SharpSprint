using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class Position
    {
        public Dist X;
        public Dist Y;

        public Position(Dist X, Dist Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Position FromMillimeters(float X, float Y)
        {
            return new Position(Dist.FromMillimeters(X), Dist.FromMillimeters(Y));
        }

        public static Position FromInches(float X, float Y)
        {
            return new Position(Dist.FromInches(X), Dist.FromInches(Y));
        }
    }
}
