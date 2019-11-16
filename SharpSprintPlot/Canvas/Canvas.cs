using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.Plot.Canvas
{
    public interface Canvas
    {
        // This interface will translate the most basic drawing commands directly
        // to either vector graphics (such as SVG) or raster graphics (such as BMP)

        // Implement the most part of the System.Drawing.Graphics here
        void DrawArc();
        void DrawCircle();
        void DrawLines();
        void FillPie();
        void FillPolygon();
        void FillCircle();
    }
}
