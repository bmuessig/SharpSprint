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

        public decimal Angle
        {
            get { return (decimal)(Value / 1000); }
            set { Value = (uint)Math.Round((decimal)(value * 1000), 0); }
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
            return new FineAngle() { Angle = Angle };
        }

        public static FineAngle FromRelativeAngle(FineAngle Relative, decimal RelativeOffsetAngle)
        {
            return new FineAngle(Relative) { RelativeOffsetAngle = RelativeOffsetAngle };
        }
    }
}
