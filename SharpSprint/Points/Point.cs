using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;

namespace SharpSprint.Points
{
    public class Point
    {
        public Distance X { get; set; }

        public Distance Y { get; set; }

        public Point()
        {
            X = new Distance();
            Y = new Distance();
        }

        public Point(Point Copy)
        {
            this.X = new Distance(Copy.X);
            this.Y = new Distance(Copy.Y);
        }

        public Point(Distance X, Distance Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
}
