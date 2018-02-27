using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class Vector
    {
        public Distance X { get; set; }
        
        public Distance Y { get; set; }

        public bool AdjustX { get; set; }

        public Distance Length
        {
            get
            {
                return new Distance((ulong)(Math.Sqrt(X.Value * X.Value + Y.Value * Y.Value)));
            }

            set
            {
                if (X == null)
                    X = new Distance();
                if (Y == null)
                    Y = new Distance();

                if(AdjustX)
                    X = new Distance((ulong)(Math.Sqrt(value.Value * value.Value - Y.Value * Y.Value)));
                else
                    Y = new Distance((ulong)(Math.Sqrt(value.Value * value.Value - X.Value * X.Value)));
            }
        }

        public Vector(Distance X, Distance Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Vector FromMillimeters(decimal X, decimal Y)
        {
            return new Vector(Distance.FromMillimeters(X), Distance.FromMillimeters(Y));
        }

        public static Vector FromInches(decimal X, decimal Y)
        {
            return new Vector(Distance.FromInches(X), Distance.FromInches(Y));
        }

        public static Vector operator +(Vector A, Vector B)
        {
            return new Vector(A.X + B.X, A.Y + B.Y);
        }

        public static Vector operator -(Vector A, Vector B)
        {
            return new Vector(A.X - B.X, A.Y - B.Y);
        }

        public static Vector operator *(Vector V, uint N)
        {
            return new Vector(V.X * N, V.Y * N);
        }

        public static Vector operator *(uint N, Vector V)
        {
            return (V * N);
        }

        public static Vector operator /(Vector V, uint N)
        {
            return new Vector(V.X / N, V.Y / N);
        }
    }
}
