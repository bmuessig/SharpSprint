using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public struct Distance
    {
        public ulong Value;

        public float Millimeters
        {
            get
            {
                return Value / 10000;
            }

            set
            {
                if (value < 0)
                    value = 0; // Clip value to 0
                Value = (ulong)Math.Round(value * 10000, 0);
            }
        }
    
        public float Inches
        {
            get
            {
                return (float)((Value * 0.0393701) / 10000);
            }

            set
            {
                if (value < 0)
                    value = 0; // Clip value to 0
                Value = (ulong)Math.Round(value * 393.701, 0);
            }
        }

        public Distance(ulong Value)
        {
            this.Value = Value;
        }

        public static Distance FromMillimeters(float Millimeters)
        {
            return new Distance() { Millimeters = Millimeters };
        }

        public static Distance FromInches(float Inches)
        {
            return new Distance() { Inches = Inches };
        }
    }
}
