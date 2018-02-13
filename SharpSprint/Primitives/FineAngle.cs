using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public struct FineAngle : Angle
    {
        private uint RawAngle;

        public uint Value
        {
            get { return RawAngle; }
            set { RawAngle = value % 360000; }
        }

        public decimal Angle
        {
            get { return (decimal)(Value / 1000); }
            set { Value = (uint)Math.Round((decimal)(value * 1000), 0); }
        }

        public FineAngle(uint Value)
            : this()
        {
            this.Value = Value;
        }

        public static FineAngle FromAngle(decimal Angle)
        {
            return new FineAngle() { Angle = Angle };
        }
    }
}
