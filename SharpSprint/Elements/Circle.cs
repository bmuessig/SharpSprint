using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.IO;
using SharpSprint.Primitives;

namespace SharpSprint.Elements
{
    public class Circle : Element
    {
        // Required parameters
        public Layer Layer { get; set; }
        public Distance Width { get; set; }
        public Position Center { get; set; }
        public Distance Radius { get; set; }

        // Optional parameters
        public Distance Clear { get; set; } // 4000
        public bool Cutout { get; set; } // False
        public bool Soldermask { get; set; } // False
        public FineAngle Start { get; set; } // 0
        public FineAngle Stop { get; set; } // 0
        public bool Fill { get; set; } // False

        // Default optional parameters
        private const uint ClearDefault = 4000;
        private const bool CutoutDefault = false;
        private const bool SoldermaskDefault = false;
        private const uint StartDefault = 0;
        private const uint StopDefault = 0;
        private const bool FillDefault = false;

        // Required and optional count
        private const byte RequiredArgCount = 4;
        private const byte OptionalArgCount = 6;

        public Circle(Layer Layer, Distance Width, Position Center, Distance Radius, bool Fill = FillDefault)
        {
            this.Layer = Layer;
            this.Width = Width;
            this.Center = Center;
            this.Radius = Radius;

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Start = new FineAngle(StartDefault);
            this.Stop = new FineAngle(StopDefault);
            this.Fill = Fill;
        }

        public Circle(Layer Layer, Distance Width, Position Center, Distance Radius, FineAngle Start, FineAngle Stop, bool Fill = FillDefault)
        {
            this.Layer = Layer;
            this.Width = Width;
            this.Center = Center;
            this.Radius = Radius;

            this.Clear = new Distance(ClearDefault);
            this.Cutout = CutoutDefault;
            this.Soldermask = SoldermaskDefault;
            this.Start = new FineAngle(StartDefault);
            this.Stop = new FineAngle(StopDefault);
            this.Fill = Fill;
        }

        public static bool Identify(TokenRow[] Tokens, uint Pointer)
        {
            // First, make sure we have met the amount of required arguments
            if (Tokens[Pointer].Count < RequiredArgCount + 1)
                return false;

            // Then, make sure we actually have a CIRCLE element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "CIRCLE")
                return false;

            // Otherwise, it looks alright
            return true;
        }
        
        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out Circle Result)
        {
            throw new NotImplementedException();
        }
        
        public bool Write(out TokenRow[] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Write the type first
            writer.Write(new Token("CIRCLE", Token.IndentTransition.None));

            // Now write the required values
            // Layer
            if (Layer >= Layer.CopperTop && Layer <= Layer.Mechanical)
                writer.Write(new Token("LAYER", (ulong)Layer));
            else
                return false;

            // Width
            writer.Write(new Token("WIDTH", Width.Value));

            // Center
            writer.Write(new Token("CENTER", Center.X.Value, Center.Y.Value));

            // Radius
            writer.Write(new Token("RADIUS", Radius.Value));

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

            // Start
            if (Start.Value != StartDefault)
                writer.Write(new Token("START", Start.Value));

            // Stop
            if (Stop.Value != StopDefault)
                writer.Write(new Token("STOP", Stop.Value));
            
            // Fill
            if (Fill != FillDefault)
                writer.Write(new Token("FILL", Fill));

            Tokens = writer.Compile();
            return true;
        }
    }
}
