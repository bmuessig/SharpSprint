using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class FineAngle : Angle
    {
        private uint AbsoluteAngle;

        public uint Value
        {
            get
            {
                if (Relative == null)
                    return AbsoluteAngle;

                return (uint)((Relative.Value + RelativeOffset) % 360000);
            }

            set
            {
                if (Relative == null)
                {
                    AbsoluteAngle = value % 360000;
                    return;
                }

                int delta = (int)(value - this.Value);
                RelativeOffset = (int)((RelativeOffset + delta) % 360000);
            }
        }

        // FIXME: Here's a loss of precision for some odd reason
        public decimal Degrees
        {
            get { return (decimal)(Value / 1000); }
            set { Value = (uint)Math.Round((decimal)(value * 1000), 0); }
        }

        public double Radians
        {
            // Invert the Degrees since our radian angles are clockwise, while the degrees are counter-clockwise
            get { return ((double)((360 - (Degrees % 360)) % 360) * Math.PI) / 180; }
            set { Degrees = (360 - (decimal)(((Radians * 180) / Math.PI) % (2 * Math.PI))) % 360; }
        }

        public FineAngle Relative { get; set; }

        public int RelativeOffset { get; set; }

        public decimal RelativeOffsetAngle
        {
            get { return (decimal)((RelativeOffset % 360000) / 1000); }
            set { RelativeOffset = (int)Math.Round((value % 360000) * 1000, 0); }
        }

        public FineAngle()
        {
            this.AbsoluteAngle = 0;
            this.Relative = null;
            this.RelativeOffset = 0;
        }

        internal FineAngle(uint Value)
        {
            this.Value = Value;
            this.Relative = null;
            this.RelativeOffset = 0;
        }

        public FineAngle(FineAngle Relative, int RelativeOffset = 0)
        {
            this.AbsoluteAngle = 0;
            this.Relative = Relative;
            this.RelativeOffset = RelativeOffset;
        }

        public static FineAngle FromAngle(decimal Angle)
        {
            return new FineAngle() { Degrees = Angle };
        }

        public static FineAngle FromRelativeAngle(FineAngle Relative, decimal RelativeOffsetAngle)
        {
            return new FineAngle(Relative) { RelativeOffsetAngle = RelativeOffsetAngle };
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

        public static implicit operator FineAngle(CoarseAngle A)
        {
            return FineAngle.FromAngle(A.Degrees);
        }

        public static implicit operator FineAngle(IntegerAngle A)
        {
            return FineAngle.FromAngle(A.Degrees);
        }

        public static bool operator !=(FineAngle A, Angle B)
        {
            return !(A == B);
        }

        public static FineAngle operator +(FineAngle A, Angle B)
        {
            return FineAngle.FromAngle(A.Degrees + B.Degrees);
        }

        public static FineAngle operator -(FineAngle A, Angle B)
        {
            return FineAngle.FromAngle(A.Degrees - B.Degrees);
        }

        public static FineAngle operator *(FineAngle A, uint N)
        {
            return new FineAngle((uint)(N * A.Value));
        }

        public static FineAngle operator *(uint N, FineAngle A)
        {
            return (A * N);
        }

        public static FineAngle operator /(FineAngle A, uint N)
        {
            return new FineAngle((uint)(A.Value / N));
        }

        public static bool operator >(FineAngle A, Angle B)
        {
            return (A.Degrees > B.Degrees);
        }

        public static bool operator >=(FineAngle A, Angle B)
        {
            return (A.Degrees >= B.Degrees);
        }

        public static bool operator <(FineAngle A, Angle B)
        {
            return (A.Degrees < B.Degrees);
        }

        public static bool operator <=(FineAngle A, Angle B)
        {
            return (A.Degrees <= B.Degrees);
        }

        public static bool operator ==(FineAngle A, Angle B)
        {
            if ((object)A == null && (object)B == null)
                return true;
            if ((object)A == null || (object)B == null)
                return false;

            return (A.Degrees == B.Degrees);
        }
    }
}
