using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public struct FineAngle : Angle
    {
        public uint Value { get; set; }

        public float Angle
        {
            get
            {
                return (float)(Value / 1000);
            }

            set
            {
                value = Math.Abs(value % 360); // Convert the angle to a valid one
                Value = (uint)Math.Round((float)(Value * 1000), 0);
            }
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
