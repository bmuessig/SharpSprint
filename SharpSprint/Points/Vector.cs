using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;

namespace SharpSprint.Points
{
    public class Vector : Point
    {
        public bool MirrorX { get; set; }
       
        public bool MirrorY { get; set; }

        public bool AdjustX { get; set; }

        public Distance Length
        {
            get
            {
                return new Distance((uint)(Math.Sqrt(X.Value * X.Value + Y.Value * Y.Value)));
            }

            set
            {
                if (X == null)
                    X = new Distance();
                if (Y == null)
                    Y = new Distance();

                if(AdjustX)
                    X = new Distance((uint)(Math.Sqrt(value.Value * value.Value - Y.Value * Y.Value)));
                else
                    Y = new Distance((uint)(Math.Sqrt(value.Value * value.Value - X.Value * X.Value)));
            }
        }

        public decimal Angle
        {
            get
            {
                // Errors
                if (X == null)
                    X = new Distance();
                if (Y == null)
                    Y = new Distance();
                if (X.Value == 0 || Y.Value == 0)
                    return 0;

                return (decimal)((180 * Math.Atan(Y.Value / X.Value)) / Math.PI);
            }

            set
            {
                if (X == null)
                    X = new Distance();
                if (Y == null)
                    Y = new Distance();
                // Make sure, we have got one valid value
                if ((X.Value == 0 && !AdjustX) || (Y.Value == 0 && AdjustX))
                    return; // TODO: Handle this error better

                // Calculate the remaining side
                // Note, that C#'s trig functions are using radians
                if(AdjustX)
                    X = new Distance((uint)Math.Round(Y.Value / Math.Tan(((double)value * Math.PI) / 180), 0));
                else
                    Y = new Distance((uint)Math.Round(X.Value * Math.Tan(((double)value * Math.PI) / 180), 0));
            }
        }

        public FineAngle AngleFine
        {
            get { return FineAngle.FromAngle(this.Angle); }
            set { this.Angle = value.Angle; }
        }

        public CoarseAngle AngleCoarse
        {
            get { return CoarseAngle.FromAngle(this.Angle); }
            set { this.Angle = value.Angle; }
        }

        public IntegerAngle AngleInteger
        {
            get { return IntegerAngle.FromAngle(this.Angle); }
            set { this.Angle = value.Angle; }
        }

        public Vector(Distance X, Distance Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public override string ToString()
        {
            return string.Format("({0}; {1})", X.ToString(), Y.ToString());
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

        public static Distance operator *(Vector A, Vector B)
        {
            return new Distance(A.X.Value * B.X.Value + A.Y.Value * B.Y.Value);
        }

        public static Vector operator /(Vector V, uint N)
        {
            return new Vector(V.X / N, V.Y / N);
        }

        public static bool operator ==(Vector A, Vector B)
        {
            if ((object)A == null && (object)B == null)
                return true;
            if ((object)A == null || (object)B == null)
                return false;

            return ((A.X == B.X) && (A.Y == B.Y));
        }

        public static bool operator !=(Vector A, Vector B)
        {
            return !(A == B);
        }

        public override bool Equals(Object O)
        {
            if (O == null)
                return false;
            if (O.GetType() != typeof(Vector))
                return false;

            Vector V = (Vector)O;
            return (this.X == V.X && this.Y == V.Y && this.AdjustX == V.AdjustX);
        }

        public override int GetHashCode()
        {
            return (X.GetHashCode() ^ Y.GetHashCode());
        }
    }
}
