using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public struct IntegerAngle : Angle
    {
        public uint Value { get; set; }

        public float Angle
        {
            get
            {
                return (float)Value;
            }

            set
            {
                value = Math.Abs(value % 360); // Convert the angle to a valid one
                Value = (uint)Math.Round((float)Value, 0);
            }
        }
    }
}
