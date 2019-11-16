using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class CoarseAngle : Angle
    {
        private int absoluteAngle;

        public int Value
        {
            get { return absoluteAngle; }
            set { absoluteAngle = value % 36000; }
        }

        public decimal Degrees
        {
            get { return (decimal)Value / 100; }
            set { Value = (int)Math.Round(value * 100, 0); }
        }

        public double Radians
        {
            // Invert the Degrees since our radian angles are clockwise, while the degrees are counter-clockwise
            get { return ((double)((360m - (Degrees % 360)) % 360) * Math.PI) / 180d; }
            set { Degrees = (360m - (decimal)(((Radians * 180) / Math.PI) % (2 * Math.PI))) % 360; }
        }

        public CoarseAngle()
        {
            this.absoluteAngle = 0;
        }

        internal CoarseAngle(int Value)
        {
            this.Value = Value;
        }

        public CoarseAngle(CoarseAngle Relative, int RelativeOffset = 0)
        {
            this.absoluteAngle = 0;
        }

        public static CoarseAngle FromDegrees(decimal Angle)
        {
            return new CoarseAngle() { Degrees = Angle };
        }

        public static CoarseAngle FromRadians(double Angle)
        {
            return new CoarseAngle() { Radians = Angle };
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

            return (this == (Angle)O);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static explicit operator decimal(CoarseAngle A)
        {
            return A.Degrees;
        }

        public static explicit operator CoarseAngle(decimal A)
        {
            return CoarseAngle.FromDegrees(A);
        }

        public static explicit operator CoarseAngle(FineAngle A)
        {
            return CoarseAngle.FromDegrees(A.Degrees);
        }

        public static explicit operator CoarseAngle(IntegerAngle A)
        {
            return CoarseAngle.FromDegrees(A.Degrees);
        }

        public static bool operator !=(CoarseAngle A, Angle B)
        {
            if ((object)A == null && (object)B == null)
                return false;
            if ((object)A == null || (object)B == null)
                return true;

            return A.Degrees != B.Degrees;
        }

        public static CoarseAngle operator +(CoarseAngle A, Angle B)
        {
            return CoarseAngle.FromDegrees(A.Degrees + B.Degrees);
        }

        public static CoarseAngle operator -(CoarseAngle A, Angle B)
        {
            return CoarseAngle.FromDegrees(A.Degrees - B.Degrees);
        }

        public static CoarseAngle operator *(CoarseAngle A, int N)
        {
            return new CoarseAngle(N * A.Value);
        }

        public static CoarseAngle operator *(int N, CoarseAngle A)
        {
            return A * N;
        }

        public static CoarseAngle operator /(CoarseAngle A, int N)
        {
            return new CoarseAngle(A.Value / N);
        }

        public static bool operator >(CoarseAngle A, Angle B)
        {
            return A.Degrees > B.Degrees;
        }

        public static bool operator >=(CoarseAngle A, Angle B)
        {
            return A.Degrees >= B.Degrees;
        }

        public static bool operator <(CoarseAngle A, Angle B)
        {
            return A.Degrees < B.Degrees;
        }

        public static bool operator <=(CoarseAngle A, Angle B)
        {
            return A.Degrees <= B.Degrees;
        }

        public static bool operator ==(CoarseAngle A, Angle B)
        {
            if ((object)A == null && (object)B == null)
                return true;
            if ((object)A == null || (object)B == null)
                return false;

            return A.Degrees == B.Degrees;
        }
    }
}
