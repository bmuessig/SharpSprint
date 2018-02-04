using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class Position
    {
        public Distance X;
        public Distance Y;

        public Position(Distance X, Distance Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Position FromMillimeters(float X, float Y)
        {
            return new Position(Distance.FromMillimeters(X), Distance.FromMillimeters(Y));
        }

        public static Position FromInches(float X, float Y)
        {
            return new Position(Distance.FromInches(X), Distance.FromInches(Y));
        }
    }
}
