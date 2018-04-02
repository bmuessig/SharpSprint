using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SprintMap
{
    public class LineTracer
    {
        // Simple line based vectorizer
        // Just works by using horizontal lines
        // First, it horizontally maps all consecuitve pixels to line segments
        // Then it checks verically, how many rectangles could be constructed (optional)
        // If the edges must be square, FlatEnded tracks are used
        // Otherwise, a mix of normal tracks and zones is used to construct the bitmap
    }
}
