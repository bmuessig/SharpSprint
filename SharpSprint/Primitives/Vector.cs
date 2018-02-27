﻿using System;
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
                    X = new Distance((ulong)Math.Round(Y.Value / Math.Tan(((double)value * Math.PI) / 180), 0));
                else
                    Y = new Distance((ulong)Math.Round(X.Value * Math.Tan(((double)value * Math.PI) / 180), 0));
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

        public static bool operator ==(Vector A, Vector B)
        {
            return ((A.X == B.X) && (A.Y == B.Y));
        }

        public static bool operator !=(Vector A, Vector B)
        {
            return !(A == B);
        }
    }
}
