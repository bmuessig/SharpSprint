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
                if (Value < 0)
                    return;
                Value = (uint)Math.Round((float)Value, 0);
            }
        }
    }
}
