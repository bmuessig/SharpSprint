using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public struct CoarseAngle : Angle
    {
        public uint Value { get; set; }

        public float Angle
        {
            get
            {
                return (float)(Value / 100);
            }

            set
            {
                value = Math.Abs(value % 360); // Convert the angle to a valid one
                Value = (uint)Math.Round((float)(Value * 100), 0);
            }
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
