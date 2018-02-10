using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Elements;
using SharpSprint.IO;

namespace SharpSprint
{
    public class Board
    {
        private Group Entities;

        public List<Entity> Canvas
        {
            get { return Entities.Entities; }
        }

        public Board()
        {
            Entities = new Group();
        }

        public bool Compile(out string Result)
        {
            Result = null;

            TokenRow[] tokens;
            if (!Entities.Write(out tokens))
                return false;

            ushort indent = 0;
            return Compiler.CompileBlock(tokens, ref indent, out Result);
        }

        public uint Load(string InputLines, bool Append = false)
        {
            // Clear the existing elements if desired
            if (!Append)
                Canvas.Clear();

            TokenRow[] rows;
            Entity[] entities;
            uint line = 0;

            // Run the input through the lexer to produce tokens
            if ((line = Parser.Tokenize(InputLines, out rows)) != 0)
                return line; // We have an error on a particular line

            // Run the tokens through the parser to turn them into Entities and Elements
            if ((line = Parser.Parse(rows, out entities)) != 0)
                return line;

            // Finally, add the new elements to the list
            Canvas.AddRange(entities);

            return 0;
        }
    }
}
