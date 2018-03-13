using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class CoarseAngle : Angle
    {
        private uint AbsoluteAngle;

        public uint Value
        {
            get
            {
                if (Relative == null)
                    return AbsoluteAngle;

                return (uint)((Relative.Value + RelativeOffset) % 36000);
            }

            set
            {
                if (Relative == null)
                {
                    AbsoluteAngle = value % 36000;
                    return;
                }

                int delta = (int)(value - this.Value);
                RelativeOffset = (int)((RelativeOffset + delta) % 36000);
            }
        }

        public decimal Degrees
        {
            get { return (decimal)(Value / 100); }
            set { Value = (uint)Math.Round((decimal)(value * 100), 0); }
        }

        public double Radians
        {
            // Invert the Degrees since our radian angles are clockwise, while the degrees are counter-clockwise
            get { return ((double)((360 - (Degrees % 360)) % 360) * Math.PI) / 180; }
            set { Degrees = (360 - (decimal)(((Radians * 180) / Math.PI) % (2 * Math.PI))) % 360; }
        }

        public CoarseAngle Relative { get; set; }

        public int RelativeOffset { get; set; }

        public decimal RelativeOffsetAngle
        {
            get { return (decimal)((RelativeOffset % 36000) / 100); }
            set { RelativeOffset = (int)Math.Round((value % 36000) * 100, 0); }
        }

        public CoarseAngle()
        {
            this.AbsoluteAngle = 0;
            this.Relative = null;
            this.RelativeOffset = 0;
        }

        internal CoarseAngle(uint Value)
        {
            this.Value = Value;
            this.Relative = null;
            this.RelativeOffset = 0;
        }

        public CoarseAngle(CoarseAngle Relative, int RelativeOffset = 0)
        {
            this.AbsoluteAngle = 0;
            this.Relative = Relative;
            this.RelativeOffset = RelativeOffset;
        }

        public static CoarseAngle FromAngle(decimal Angle)
        {
            return new CoarseAngle() { Degrees = Angle };
        }

        public static CoarseAngle FromRelativeAngle(CoarseAngle Relative, decimal RelativeOffsetAngle)
        {
            return new CoarseAngle(Relative) { RelativeOffsetAngle = RelativeOffsetAngle };
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

        public static implicit operator CoarseAngle(FineAngle A)
        {
            return CoarseAngle.FromAngle(A.Degrees);
        }

        public static implicit operator CoarseAngle(IntegerAngle A)
        {
            return CoarseAngle.FromAngle(A.Degrees);
        }

        public static bool operator !=(CoarseAngle A, Angle B)
        {
            return !(A == B);
        }

        public static CoarseAngle operator +(CoarseAngle A, Angle B)
        {
            return CoarseAngle.FromAngle(A.Degrees + B.Degrees);
        }

        public static CoarseAngle operator -(CoarseAngle A, Angle B)
        {
            return CoarseAngle.FromAngle(A.Degrees - B.Degrees);
        }

        public static CoarseAngle operator *(CoarseAngle A, uint N)
        {
            return new CoarseAngle((uint)(N * A.Value));
        }

        public static CoarseAngle operator *(uint N, CoarseAngle A)
        {
            return (A * N);
        }

        public static CoarseAngle operator /(CoarseAngle A, uint N)
        {
            return new CoarseAngle((uint)(A.Value / N));
        }

        public static bool operator >(CoarseAngle A, Angle B)
        {
            return (A.Degrees > B.Degrees);
        }

        public static bool operator >=(CoarseAngle A, Angle B)
        {
            return (A.Degrees >= B.Degrees);
        }

        public static bool operator <(CoarseAngle A, Angle B)
        {
            return (A.Degrees < B.Degrees);
        }

        public static bool operator <=(CoarseAngle A, Angle B)
        {
            return (A.Degrees <= B.Degrees);
        }

        public static bool operator ==(CoarseAngle A, Angle B)
        {
            if ((object)A == null && (object)B == null)
                return true;
            if ((object)A == null || (object)B == null)
                return false;

            return (A.Degrees == B.Degrees);
        }
    }
}
