using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Primitives;
using SharpSprint.IO;

namespace SharpSprint.Elements
{
    public class Track : Element
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Distance Width { get; set; }
        public List<Position> Path { get; set; }

        // Optional parameters
        public Distance Clear { get; set; } // 4000
        public bool Cutout { get; set; } // False
        public bool Soldermask { get; set; } // False
        public bool FlatStart { get; set; } // False
        public bool FlatEnd { get; set; } // False

        // Default optional parameters
        private const uint ClearDefault = 4000;
        private const bool CutoutDefault = false;
        private const bool SoldermaskDefault = false;
        private const bool FlatStartDefault = false;
        private const bool FlatEndDefault = false;

        public Track(Layer Layer, Distance Width, params Position[] Path)
        {
            this.Layer = Layer;
            this.Width = Width;
            this.Path = new List<Position>(Path);

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.FlatStart = FlatStartDefault;
            this.FlatEnd = FlatEndDefault;
        }

        public Track(Layer Layer, Distance Width, bool FlatStart, bool FlatEnd, params Position[] Path)
        {
            this.Layer = Layer;
            this.Width = Width;
            this.Path = new List<Position>(Path);

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.FlatStart = FlatStart;
            this.FlatEnd = FlatEnd;
        }

        public bool Read(IO.Token[][] Tokens, ref uint Pointer)
        {
            throw new NotImplementedException();
        }

        public bool Write(out IO.Token[][] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Write the type first
            writer.Write(new Token("TRACK", Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (ulong)Layer));
            else
                return false;

            // Width
            writer.Write(new Token("WIDTH", Width.Value));

            // Points
            if (Path.Count >= 2)
            {
                for (int counter = 0; counter < Path.Count; counter++)
                {
                    if (counter > 0)
                    {
                        if ((Path[counter].X.Value == Path[counter - 1].X.Value) && (Path[counter].Y.Value == Path[counter - 1].Y.Value))
                            continue;
                    }
                    writer.Write(new Token(string.Format("P{0}", counter), Path[counter].X.Value, Path[counter].Y.Value));
                    counter++;
                }
            }
            else
                return false;

            // Then write the optional values
            // Clear
            if (Clear.Value != ClearDefault)
                writer.Write(new Token("CLEAR", Clear.Value));

            // Cutout
            if (Cutout != CutoutDefault)
                writer.Write(new Token("CUTOUT", Cutout));

            // Soldermask
            if (Soldermask != SoldermaskDefault)
                writer.Write(new Token("SOLDERMASK", Soldermask));

            // FlatStart
            if (FlatStart != FlatStartDefault)
                writer.Write(new Token("FLATSTART", FlatStart));

            // FlatEnd
            if (FlatEnd != FlatEndDefault)
                writer.Write(new Token("FLATEND", FlatEnd));

            Tokens = writer.Compile();
            return true;
        }
    }
}
