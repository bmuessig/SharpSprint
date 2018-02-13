using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public struct CoarseAngle : Angle
    {
        private uint RawAngle;

        public uint Value
        {
            get { return RawAngle; }
            set { RawAngle = value % 36000; }
        }

        public decimal Angle
        {
            get { return (decimal)(Value / 100); }
            set { Value = (uint)Math.Round((decimal)(value * 100), 0); }
        }

        public CoarseAngle(uint Value)
            : this()
        {
            this.Value = Value;
        }

        public static CoarseAngle FromAngle(decimal Angle)
        {
            return new CoarseAngle() { Angle = Angle };
        }
    }
}
