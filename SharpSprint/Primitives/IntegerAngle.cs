using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public struct IntegerAngle : Angle
    {
        private uint RawAngle;

        public uint Value
        {
            get { return RawAngle; }
            set { RawAngle = value % 360; }
        }

        public float Angle
        {
            get { return (float)Value; }
            set { Value = (uint)Math.Round(value, 0); }
        }

        public IntegerAngle(uint Value)
            : this()
        {
            this.Value = Value;
        }

        public static IntegerAngle FromAngle(float Angle)
        {
            return new IntegerAngle() { Angle = Angle };
        }
    }
}
