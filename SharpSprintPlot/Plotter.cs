using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Elements;
using System.Drawing;
using SharpSprint;

namespace SharpSprintPlot
{
    public class Plotter
    {
        public Plotter(uint PixelsPerMillimeter)
        {
            this.PixelsPerMillimeter = PixelsPerMillimeter;
        }

        Graphics Canvas;

        public uint PixelsPerMillimeter { get; set; }

        public Bitmap Draw(Group Group, Graphics Canvas)
        {
            return null;
        }

        public bool Measure(Entity Entity, out Rectangle Result, bool SkipUnsupportedEntities = true)
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
                    if (!this.Measure(ent, out curRect))
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
            }

            return false;
        }

        public bool Draw(Entity Entity)
        {
            if (Entity == null)
                return false;

            if (Entity is Circle)
            {

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
    }
}
