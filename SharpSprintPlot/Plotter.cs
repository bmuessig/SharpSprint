using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Elements;
using System.Drawing;
using SharpSprint;

namespace SharpSprint.Plot
{
    public class Plotter
    {
        public Plotter(uint PixelsPerMillimeter)
        {
            this.PixelsPerMillimeter = PixelsPerMillimeter;
        }

        Graphics Canvas;
        ElementStyle Style;

        public uint PixelsPerMillimeter { get; set; }

        public bool Measure(Entity Entity, out Rectangle Result, bool SkipUnsupportedEntities = true)
        {
            return Measure(Entity, PixelsPerMillimeter, out Result, SkipUnsupportedEntities);
        }

        public static bool Measure(Entity Entity, uint PixelsPerMillimeter, out Rectangle Result, bool SkipUnsupportedEntities = true)
        {
            Result = new Rectangle();

            if (Entity == null)
                return false;

            if (Entity is Circle)
            {
                Circle c = (Circle)Entity;

                // Input sanity checks
                if (c.Center == null || c.Radius == null || c.Width == null)
                    return false;

                // Finally build a bounding box rectangle out of the coordinates
                Result = new Rectangle(
                    (int)(PixelsPerMillimeter * (c.Center.X.Millimeters - c.Radius.Millimeters - (c.Width.Millimeters / 2))),
                    (int)(PixelsPerMillimeter * (c.Center.Y.Millimeters - c.Radius.Millimeters - (c.Width.Millimeters / 2))),
                    (int)(PixelsPerMillimeter * ((c.Radius.Millimeters * 2) + c.Width.Millimeters)),
                    (int)(PixelsPerMillimeter * ((c.Radius.Millimeters * 2) + c.Width.Millimeters)));
                return true;
            }
            else if (Entity is Component)
            {
                Component c = (Component)Entity;

                // Input sanity checks
            }
            else if (Entity is Group)
            {
                Group g = (Group)Entity;
                
                // Input sanity checks
                if (g.Entities == null)
                    return false;

                // If there are no elements, the code below won't work
                // Therefore just return a zero value
                if (g.Entities.Count < 1)
                {
                    Result = new Rectangle(0, 0, 0, 0);
                    return true;
                }

                Rectangle curRect;
                int minX = 0, minY = 0, maxX = 0, maxY = 0;
                bool firstValue = true;

                // Loop through the elements and recursively figure out the sizes
                foreach (SharpSprint.Entity ent in g.Entities)
                {
                    // Sanity check
                    if (ent == null)
                        return false;

                    // Calculate our current rectangle
                    if (!Measure(ent, PixelsPerMillimeter, out curRect))
                    {
                        // Make sure we skip unsupported entities if so desired
                        if (!SkipUnsupportedEntities)
                            return false;
                        continue;
                    }

                    // Find the extreme point values
                    // X
                    if (curRect.X < minX || firstValue)
                        minX = curRect.X;
                    if (curRect.X > maxX || firstValue)
                        maxX = curRect.X;
                    // Y
                    if (curRect.Y < minY || firstValue)
                        minY = curRect.Y;
                    if (curRect.Y > maxY || firstValue)
                        maxY = curRect.Y;

                    // Clear the first value flag
                    if(firstValue)
                        firstValue = false;
                }
            }
            else if (Entity is SMDPad)
            {
                SMDPad p = (SMDPad)Entity;

                

            }
            else if (Entity is THTPad)
            {
                THTPad p = (THTPad)Entity;

                // Input sanity checks
                if (p.Position == null || p.Size == null)
                    return false;

                // Finally build a bounding box rectangle out of the coordinates
                Result = new Rectangle(
                    (int)((p.Position.X.Millimeters + (p.Size.Millimeters / 2)) * PixelsPerMillimeter),
                    (int)((p.Position.Y.Millimeters + (p.Size.Millimeters / 2)) * PixelsPerMillimeter),
                    (int)(p.Size.Millimeters * PixelsPerMillimeter),
                    (int)(p.Size.Millimeters * PixelsPerMillimeter));
                return true;
            }
            else if (Entity is Track)
            {
                Track t = (Track)Entity;

                // Input sanity checks
                if (t.Path == null || t.Width == null)
                    return false;
                if (t.Path.Count < 2)
                    return false;

                // Variables
                int curX, curY, minX = 0, minY = 0, maxX = 0, maxY = 0;
                int width = (int)(t.Width.Millimeters * PixelsPerMillimeter);
                bool firstValue = true;

                foreach (SharpSprint.Points.Point pt in t.Path)
                {
                    // Sanity check
                    if (pt == null)
                        return false;

                    // Calculate the current point
                    curX = (int)(pt.X.Millimeters * PixelsPerMillimeter);
                    curY = (int)(pt.Y.Millimeters * PixelsPerMillimeter);

                    // Find the extreme point values
                    // X
                    if (curX < minX || firstValue)
                        minX = curX;
                    if (curX > maxX || firstValue)
                        maxX = curX;
                    // Y
                    if (curY < minY || firstValue)
                        minY = curY;
                    if (curY > maxY || firstValue)
                        maxY = curY;

                    // Clear the first value flag
                    if (firstValue)
                        firstValue = false;
                }

                // The trace has a width as well
                minX -= width;
                minY -= width;
                maxY += width;
                maxX += width;

                // Finally build a bounding box rectangle out of the coordinates
                Result = new Rectangle(minX, minY, maxX - minX, maxY - minY);
                return true;
            }
            else if (Entity is Zone)
            {
                Zone z = (Zone)Entity;

                // Input sanity checks
                if (z.Path == null || z.Width == null)
                    return false;
                if (z.Path.Count < 3)
                    return false;

                // Variables
                int curX, curY, minX = 0, minY = 0, maxX = 0, maxY = 0;
                int width = (int)(z.Width.Millimeters * PixelsPerMillimeter);
                bool firstValue = true;

                foreach (SharpSprint.Points.Point pt in z.Path)
                {
                    // Sanity check
                    if (pt == null)
                        return false;

                    // Calculate the current point
                    curX = (int)(pt.X.Millimeters * PixelsPerMillimeter);
                    curY = (int)(pt.Y.Millimeters * PixelsPerMillimeter);

                    // Find the extreme point values
                    // X
                    if (curX < minX || firstValue)
                        minX = curX;
                    if (curX > maxX || firstValue)
                        maxX = curX;
                    // Y
                    if (curY < minY || firstValue)
                        minY = curY;
                    if (curY > maxY || firstValue)
                        maxY = curY;

                    // Clear the first value flag
                    if (firstValue)
                        firstValue = false;
                }

                // The trace has a width as well
                minX -= width;
                minY -= width;
                maxY += width;
                maxX += width;

                // Finally build a bounding box rectangle out of the coordinates
                Result = new Rectangle(minX, minY, maxX - minX, maxY - minY);
                return true;
            }

            return false;
        }

        public bool Draw(Entity Entity, bool SkipUnsupportedEntities = true)
        {
            return Draw(Entity, Color.Empty, Color.Empty, SkipUnsupportedEntities);
        }

        public static bool Draw(Circle Circle, ElementStyle Style, uint PixelsPerMillimeter, out Bitmap Render, out Rectangle Box)
        {
            Render = null;
            Box = Rectangle.Empty;

            // Input sanity checks
            if (Circle == null)
                return false;
            if (Circle.Center == null || Circle.Radius == null || Circle.Width == null)
                return false;

            // Measure the bounding box
            if (!Measure(Circle, PixelsPerMillimeter, out Box, false))
                return false;

            // Get the layer color
            Color layerColor = GetLayerColor(Style, Circle.Layer);
            if(layerColor == Color.Empty || Style.StrokeColor == Color.Empty || Style.DrillColor == Color.Empty)
                return false;

            // Create a new bitmap
            Render = new Bitmap(Box.Width + 1, Box.Height + 1);

            // Check if there is anything to draw
            if (Circle.Radius.Value == 0)
                return true;

            // Create the pens and brushes
            Pen primaryPen = new Pen(layerColor, (float)Circle.Width.Millimeters * PixelsPerMillimeter);
            SolidBrush primaryBrush = new SolidBrush(layerColor);

            // Create a new graphics context
            Graphics gfx = Graphics.FromImage(Render);

            // Check if we can speed up things by drawing a whole circle
            if (Circle.Start.Value == 0 && Circle.Stop.Value == 0)
            {
                // Is the circle filled
                if (Circle.Fill)
                    gfx.FillEllipse(primaryBrush, 0, 0, Box.Width, Box.Height);
                else
                    gfx.DrawEllipse(primaryPen, 0, 0, Box.Width, Box.Height);
            }
            else // Or just a pie
            {
                // Is the pie filled
                if (Circle.Fill)
                    gfx.FillPie(primaryBrush, 0, 0, Box.Width, Box.Height,
                        (int)Circle.Start.Angle, (int)Circle.Stop.Angle);
                else
                    gfx.DrawPie(primaryPen, 0, 0, Box.Width, Box.Height,
                        (int)Circle.Start.Angle, (int)Circle.Stop.Angle);
            }

            return true;
        }

        public bool Draw(Entity Entity, Color ForeColor, Color BackgroundColor, bool SkipUnsupportedEntities = true)
        {
            if (Entity == null)
                return false;

            

            

            if (Entity is Circle)
            {
                Circle c = (Circle)Entity;
                // Input sanity checks
                if (c.Center == null || c.Radius == null || c.Width == null)
                    return false;

                
                return true;
            }
            else if (Entity is Component)
            {

            }
            else if (Entity is Group)
            {

            }
            else if (Entity is SMDPad)
            {
                SMDPad p = (SMDPad)Entity;
            }
            else if (Entity is THTPad)
            {
                THTPad p = (THTPad)Entity;
                
            }
            else if (Entity is Track)
            {
                Track t = (Track)Entity;
            }
            else if (Entity is Zone)
            {
                Zone z = (Zone)Entity;
            }

            return false;
        }

        private static Color GetLayerColor(ElementStyle Style, Layer Layer)
        {
            if (Style == null)
                return Color.Empty;
            if (Style.LayerColorOverride)
                return Style.LayerColor;
            if (Style.LayerColors == null)
                return Color.Empty;
            if (!Style.LayerColors.ContainsKey(Layer))
                return Color.Empty;
            return Style.LayerColors[Layer];
        }

        private static Point SprintPointToDrawing(SharpSprint.Points.Point SprintPoint, uint PixelsPerMillimeter)
        {
            if (SprintPoint == null)
                return Point.Empty;

            return new Point((int)(SprintPoint.X.Millimeters * PixelsPerMillimeter),
                (int)(SprintPoint.Y.Millimeters * PixelsPerMillimeter));
        }

        private static Point[] SprintPointsToDrawing(SharpSprint.Points.Point[] SprintPoints, uint PixelsPerMillimeter)
        {
            if (SprintPoints == null)
                return null;

            List<Point> pointList = new List<Point>();
            foreach (SharpSprint.Points.Point sprintPoint in SprintPoints)
            {
                Point point = SprintPointToDrawing(sprintPoint, PixelsPerMillimeter);
                if (point == null)
                    return null;
                pointList.Add(point);
            }

            return pointList.ToArray();
        }
    }
}
