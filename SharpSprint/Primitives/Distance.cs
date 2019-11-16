using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class Distance
    {
        private int rawValue;

        public int Value
        {
            get { return rawValue; }
            set { rawValue = value; }
        }

        public decimal Millimeters
        {
            get { return Value / 10000; }
            set { Value = (int)Math.Round(value * 10000, 0); }
        }
    
        public decimal Inches
        {
            get { return (Value * 0.0393701m) / 10000; }
            set { Value = (int)Math.Round(value * 393.701m, 0); }
        }

        public Distance()
        {
            rawValue = 0;
        }

        internal Distance(int Value)
        {
            this.Value = Value;
        }

        public Distance(Distance Copy)
        {
            this.Value = Copy.Value;
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

        public static Distance operator +(Distance A, Distance B)
        {
            return new Distance(A.Value + B.Value);
        }

        public static Distance operator -(Distance A, Distance B)
        {
            return new Distance(A.Value - B.Value);
        }

        public static Distance operator *(Distance D, int N)
        {
            return new Distance(N * D.Value);
        }

        public static Distance operator *(int N, Distance D)
        {
            return D * N;
        }

        public static Distance operator /(Distance D, int N)
        {
            if (N == 0)
                throw new DivideByZeroException();

            return new Distance(D.Value / N);
        }

        public static bool operator >(Distance A, Distance B)
        {
            return A.Value > B.Value;
        }

        public static bool operator >=(Distance A, Distance B)
        {
            return A.Value >= B.Value;
        }

        public static bool operator <(Distance A, Distance B)
        {
            return A.Value < B.Value;
        }

        public static bool operator <=(Distance A, Distance B)
        {
            return A.Value <= B.Value;
        }

        public static bool operator ==(Distance A, Distance B)
        {
            if ((object)A == null && (object)B == null)
                return true;
            if ((object)A == null || (object)B == null)
                return false;
            
            return A.Value == B.Value;
        }

        public static bool operator !=(Distance A, Distance B)
        {
            if ((object)A == null && (object)B == null)
                return false;
            if ((object)A == null || (object)B == null)
                return true;

            return A.Value != B.Value;
        }

        public override bool Equals(Object O)
        {
            if (O == null)
                return false;
            if (!(O is Distance))
                return false;

            Distance D = (Distance)O;
            return this.Value == D.Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public static explicit operator decimal(Distance A)
        {
            return A.Millimeters;
        }

        public static explicit operator Distance(decimal A)
        {
            return Distance.FromMillimeters(A);
        }
    }
}
