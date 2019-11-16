using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class FineAngle : Angle
    {
        private int absoluteAngle;

        public int Value
        {
            get { return absoluteAngle; }
            set { absoluteAngle = value % 360000; }
        }

        public decimal Degrees
        {
            get { return (decimal)Value / 1000; }
            set { Value = (int)Math.Round(value * 1000, 0); }
        }

        public double Radians
        {
            // Invert the Degrees since our radian angles are clockwise, while the degrees are counter-clockwise
            get { return ((double)((360m - (Degrees % 360)) % 360) * Math.PI) / 180d; }
            set { Degrees = (360m - (decimal)(((Radians * 180) / Math.PI) % (2 * Math.PI))) % 360; }
        }

        public FineAngle()
        {
            this.absoluteAngle = 0;
        }

        internal FineAngle(int Value)
        {
            this.Value = Value;
        }

        public static FineAngle FromDegrees(decimal Angle)
        {
            return new FineAngle() { Degrees = Angle };
        }

        public static FineAngle FromRadians(double Angle)
        {
            return new FineAngle() { Radians = Angle };
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

        public static explicit operator decimal(FineAngle A)
        {
            return A.Degrees;
        }

        public static explicit operator FineAngle(decimal A)
        {
            return FineAngle.FromDegrees(A);
        }

        public static explicit operator FineAngle(CoarseAngle A)
        {
            return FineAngle.FromDegrees(A.Degrees);
        }

        public static explicit operator FineAngle(IntegerAngle A)
        {
            return FineAngle.FromDegrees(A.Degrees);
        }

        public static bool operator !=(FineAngle A, Angle B)
        {
            if ((object)A == null && (object)B == null)
                return false;
            if ((object)A == null || (object)B == null)
                return true;

            return A.Degrees != B.Degrees;
        }

        public static FineAngle operator +(FineAngle A, Angle B)
        {
            return FineAngle.FromDegrees(A.Degrees + B.Degrees);
        }

        public static FineAngle operator -(FineAngle A, Angle B)
        {
            return FineAngle.FromDegrees(A.Degrees - B.Degrees);
        }

        public static FineAngle operator *(FineAngle A, int N)
        {
            return new FineAngle(N * A.Value);
        }

        public static FineAngle operator *(int N, FineAngle A)
        {
            return A * N;
        }

        public static FineAngle operator /(FineAngle A, int N)
        {
            return new FineAngle(A.Value / N);
        }

        public static bool operator >(FineAngle A, Angle B)
        {
            return A.Degrees > B.Degrees;
        }

        public static bool operator >=(FineAngle A, Angle B)
        {
            return A.Degrees >= B.Degrees;
        }

        public static bool operator <(FineAngle A, Angle B)
        {
            return A.Degrees < B.Degrees;
        }

        public static bool operator <=(FineAngle A, Angle B)
        {
            return A.Degrees <= B.Degrees;
        }

        public static bool operator ==(FineAngle A, Angle B)
        {
            if ((object)A == null && (object)B == null)
                return true;
            if ((object)A == null || (object)B == null)
                return false;

            return A.Degrees == B.Degrees;
        }
    }
}
