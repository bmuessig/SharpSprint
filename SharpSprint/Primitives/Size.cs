using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public struct Size
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

        public Size(ulong Value)
        {
            this.Value = Value;
        }

        public static Size FromMillimeters(float Millimeters)
        {
            Size s = new Size();
            s.Millimeters = Millimeters;
            return s;
        }

        public static Size FromInches(float Inches)
        {
            Size s = new Size();
            s.Inches = Inches;
            return s;
        }
    }
}
