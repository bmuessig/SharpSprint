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

        public float Angle
        {
            get { return (float)(Value / 100); }
            set { Value = (uint)Math.Round((float)(value * 100), 0); }
        }

        public CoarseAngle(uint Value)
            : this()
        {
            this.Value = Value;
        }

        public static CoarseAngle FromAngle(float Angle)
        {
            return new CoarseAngle() { Angle = Angle };
        }
    }
}
