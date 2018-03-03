using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class IntegerAngle : Angle
    {
        private uint AbsoluteAngle;

        public uint Value
        {
            get
            {
                if(Relative == null)
                    return AbsoluteAngle;

                return (uint)((Relative.Value + RelativeOffset) % 360);
            }

            set
            {
                if (Relative == null)
                {
                    AbsoluteAngle = value % 360;
                    return;
                }

                int delta = (int)(value - this.Value);
                RelativeOffset = (int)((RelativeOffset + delta) % 360);
            }
        }

        public decimal Angle
        {
            get { return (decimal)Value; }
            set { Value = (uint)Math.Round(value, 0); }
        }

        public IntegerAngle Relative { get; set; }

        public int RelativeOffset { get; set; }

        public decimal RelativeOffsetAngle
        {
            get { return (decimal)(RelativeOffset % 360); }
            set { RelativeOffset = (int)Math.Round(value % 360, 0); }
        }

        public IntegerAngle()
        {
            this.AbsoluteAngle = 0;
            this.Relative = null;
            this.RelativeOffset = 0;
        }

        internal IntegerAngle(uint Value)
        {
            this.Value = Value;
            this.Relative = null;
            this.RelativeOffset = 0;
        }

        public IntegerAngle(IntegerAngle Relative, int RelativeOffset = 0)
        {
            this.AbsoluteAngle = 0;
            this.Relative = Relative;
            this.RelativeOffset = RelativeOffset;
        }

        public static IntegerAngle FromAngle(decimal Angle)
        {
            return new IntegerAngle() { Angle = Angle };
        }

        public static IntegerAngle FromRelativeAngle(IntegerAngle Relative, decimal RelativeOffsetAngle)
        {
            return new IntegerAngle(Relative) { RelativeOffsetAngle = RelativeOffsetAngle };
        }
    }
}
