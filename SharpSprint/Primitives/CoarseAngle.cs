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

        public decimal Angle
        {
            get { return (decimal)(Value / 100); }
            set { Value = (uint)Math.Round((decimal)(value * 100), 0); }
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
            return new CoarseAngle() { Angle = Angle };
        }

        public static CoarseAngle FromRelativeAngle(CoarseAngle Relative, decimal RelativeOffsetAngle)
        {
            return new CoarseAngle(Relative) { RelativeOffsetAngle = RelativeOffsetAngle };
        }
    }
}
