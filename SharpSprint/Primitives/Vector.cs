﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Primitives
{
    public class Vector
    {
        public Distance X;
        public Distance Y;

        public Vector(Distance X, Distance Y)
        {
            this.X = X;
            this.Y = Y;
        }

        public static Vector FromMillimeters(decimal X, decimal Y)
        {
            return new Vector(Distance.FromMillimeters(X), Distance.FromMillimeters(Y));
        }

        public static Vector FromInches(decimal X, decimal Y)
        {
            return new Vector(Distance.FromInches(X), Distance.FromInches(Y));
        }
    }
}