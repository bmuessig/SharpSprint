using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;

namespace SharpSprint.Points
{
    public class RefPoint : Point
    {
        // You can attach this point to a pad (and it's edges) or just about any other element

        public enum SMDPadRef : byte
        {

        }

        public enum TrackPointRef : byte
        {

        }
    }
}
