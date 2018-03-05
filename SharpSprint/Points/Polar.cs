using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;

namespace SharpSprint.Points
{
    public class Polar : Point
    {
        public Point Origin { get; set; }

        public Distance Radius { get; set; }

        public decimal Angle { get; set; }

        public FineAngle AngleFine
        {
            get { return FineAngle.FromAngle(this.Angle); }
            set { this.Angle = value.Degrees; }
        }

        public CoarseAngle AngleCoarse
        {
            get { return CoarseAngle.FromAngle(this.Angle); }
            set { this.Angle = value.Degrees; }
        }

        public IntegerAngle AngleInteger
        {
            get { return IntegerAngle.FromAngle(this.Angle); }
            set { this.Angle = value.Degrees; }
        }

        public new Distance X
        {
            get
            {
                return new Distance((uint)Math.Round(Origin.X.Value + Radius.Value * Math.Cos(((double)Angle * Math.PI) / 180), 0));
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public new Distance Y
        {
            get
            {
                return new Distance((uint)Math.Round(Origin.Y.Value + Radius.Value * Math.Sin(((double)Angle * Math.PI) / 180), 0));
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
