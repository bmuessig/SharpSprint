using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class IntegerAngle : Angle
    {
        private int absoluteAngle;

        public int Value
        {
            get { return absoluteAngle; }
            set { absoluteAngle = value % 360; }
        }

        public decimal Degrees
        {
            get { return (decimal)Value; }
            set { Value = (int)Math.Round(value, 0); }
        }

        public double Radians
        {
            // Invert the Degrees since our radian angles are clockwise, while the degrees are counter-clockwise
            get { return ((double)((360m - (Degrees % 360)) % 360) * Math.PI) / 180d; }
            set { Degrees = (360m - (decimal)(((Radians * 180) / Math.PI) % (2 * Math.PI))) % 360; }
        }

        public IntegerAngle()
        {
            this.absoluteAngle = 0;
        }

        internal IntegerAngle(int Value)
        {
            this.Value = Value;
        }

        public static IntegerAngle FromDegrees(decimal Angle)
        {
            return new IntegerAngle() { Degrees = Angle };
        }

        public static IntegerAngle FromRadians(double Angle)
        {
            return new IntegerAngle() { Radians = Angle };
        }

        public override string ToString()
        {
            return string.Format("{0}°", this.Degrees);
        }

        public override bool Equals(Object O)
        {
            if (O == null)
                return false;
            if (!(O is Angle))
                return false;

            return this == (Angle)O;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static explicit operator decimal(IntegerAngle A)
        {
            return A.Degrees;
        }

        public static explicit operator IntegerAngle(decimal A)
        {
            return IntegerAngle.FromDegrees(A);
        }

        public static explicit operator IntegerAngle(FineAngle A)
        {
            return IntegerAngle.FromDegrees(A.Degrees);
        }

        public static explicit operator IntegerAngle(CoarseAngle A)
        {
            return IntegerAngle.FromDegrees(A.Degrees);
        }

        public static bool operator !=(IntegerAngle A, Angle B)
        {
            if ((object)A == null && (object)B == null)
                return false;
            if ((object)A == null || (object)B == null)
                return true;

            return A.Degrees != B.Degrees;
        }

        public static IntegerAngle operator +(IntegerAngle A, Angle B)
        {
            return IntegerAngle.FromDegrees(A.Degrees + B.Degrees);
        }

        public static IntegerAngle operator -(IntegerAngle A, Angle B)
        {
            return IntegerAngle.FromDegrees(A.Degrees - B.Degrees);
        }

        public static IntegerAngle operator *(IntegerAngle A, int N)
        {
            return new IntegerAngle(N * A.Value);
        }

        public static IntegerAngle operator *(int N, IntegerAngle A)
        {
            return A * N;
        }

        public static IntegerAngle operator /(IntegerAngle A, int N)
        {
            return new IntegerAngle(A.Value / N);
        }

        public static bool operator >(IntegerAngle A, Angle B)
        {
            return A.Degrees > B.Degrees;
        }

        public static bool operator >=(IntegerAngle A, Angle B)
        {
            return A.Degrees >= B.Degrees;
        }

        public static bool operator <(IntegerAngle A, Angle B)
        {
            return A.Degrees < B.Degrees;
        }

        public static bool operator <=(IntegerAngle A, Angle B)
        {
            return A.Degrees <= B.Degrees;
        }

        public static bool operator ==(IntegerAngle A, Angle B)
        {
            if ((object)A == null && (object)B == null)
                return true;
            if ((object)A == null || (object)B == null)
                return false;

            return A.Degrees == B.Degrees;
        }
    }
}
