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

        public float Angle
        {
            get { return (float)(Value / 1000); }
            set { Value = (uint)Math.Round((float)(value * 1000), 0); }
        }

        public FineAngle(uint Value)
            : this()
        {
            this.Value = Value;
        }

        public static FineAngle FromAngle(float Angle)
        {
            return new FineAngle() { Angle = Angle };
        }
    }
}
