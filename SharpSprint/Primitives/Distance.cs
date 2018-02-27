using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class Distance
    {
        private ulong AbsoluteValue;

        public Distance Relative { get; set; }

        public int RelativeOffset { get; set; }

        public ulong Value
        {
            get
            {
                if (Relative == null)
                    return AbsoluteValue;

                if (RelativeOffset < 0)
                    return (Relative.Value - (uint)(-RelativeOffset));
                else
                    return (Relative.Value + (uint)RelativeOffset);
            }

            set
            {
                if (Relative == null)
                {
                    AbsoluteValue = value;
                    return;
                }

                int delta = (int)(value - Value);
                RelativeOffset += delta;
            }
        }

        public decimal Millimeters
        {
            get
            {
                return (decimal)(Value / 10000);
            }

            set
            {
                if (value < 0)
                    value = 0; // Clip value to 0
                Value = (ulong)Math.Round(value * 10000, 0);
            }
        }
    
        public decimal Inches
        {
            get
            {
                return (decimal)((Value * 0.0393701) / 10000);
            }

            set
            {
                if (value < 0)
                    value = 0; // Clip value to 0
                Value = (ulong)Math.Round(value * 393.701m, 0);
            }
        }

        public decimal RelativeOffsetMillimeters
        {
            get
            {
                return (decimal)(RelativeOffset / 10000);
            }

            set
            {
                if (value < 0)
                    value = 0; // Clip value to 0
                RelativeOffset = (int)Math.Round(value * 10000, 0);
            }
        }

        public decimal RelativeOffsetInches
        {
            get
            {
                return (decimal)((RelativeOffset * 0.0393701) / 10000);
            }

            set
            {
                if (value < 0)
                    value = 0; // Clip value to 0
                RelativeOffset = (int)Math.Round(value * 393.701m, 0);
            }
        }

        public Distance()
        {
            AbsoluteValue = 0;
            Relative = null;
            RelativeOffset = 0;
        }

        public Distance(ulong Value)
        {
            this.Value = Value;
        }

        public Distance(Distance Relative, int RelativeOffset = 0)
        {
            this.Relative = Relative;
            this.RelativeOffset = RelativeOffset;
        }

        public override string ToString()
        {
            return string.Format("{0}mm", this.Millimeters);
        }

        public static Distance FromMillimeters(decimal Millimeters)
        {
            return new Distance() { Millimeters = Millimeters };
        }

        public static Distance FromInches(decimal Inches)
        {
            return new Distance() { Inches = Inches };
        }

        public static Distance FromRelativeMillimeters(Distance Relative, decimal RelativeOffsetMillimeters)
        {
            return new Distance(Relative) { RelativeOffsetMillimeters = RelativeOffsetMillimeters };
        }

        public static Distance FromRelativeInches(Distance Relative, decimal RelativeOffsetInches)
        {
            return new Distance(Relative) { RelativeOffsetInches = RelativeOffsetInches };
        }

        public static Distance operator +(Distance A, Distance B)
        {
            return new Distance(A.Value + B.Value);
        }

        public static Distance operator -(Distance A, Distance B)
        {
            return new Distance(A.Value - B.Value);
        }

        public static Distance operator *(Distance D, uint N)
        {
            return new Distance((ulong)(N * D.Value));
        }

        public static Distance operator *(uint N, Distance D)
        {
            return (D * N);
        }

        public static Distance operator /(Distance D, uint N)
        {
            return new Distance((ulong)(D.Value / N));
        }
    }
}
