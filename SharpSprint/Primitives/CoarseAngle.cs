using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class CoarseAngle : Angle
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
                if (Value < 0)
                    return;
                Value = (uint)Math.Round((float)(Value * 100), 0);
            }
        }
    }
}
