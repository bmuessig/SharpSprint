using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Elements;
using SharpSprint.IO;

namespace SharpSprint
{
    public class Board : List<Entity>
    {
        public Board(params Entity[] Entities)
        {
            if (Entities.Length > 0)
                this.AddRange(Entities);
        }

        public bool Write(out string Result)
        {
            TokenWriter writer = new TokenWriter();
            Result = null;

            // Compile the entities to tokens first
            if (this.Count > 0)
            {
                foreach (Entity entity in this)
                {
                    TokenRow[] EntityTokens;
                    if (entity.Write(out EntityTokens))
                    {
                        writer.Write(EntityTokens);
                        writer.NewLine();
                    }
                    else
                        return false;
                }
            }
            else
                return false;

            // Now compile the tokens into a string
            ushort indent = 0;
            return Compiler.CompileBlock(writer.Compile(), ref indent, out Result);
        }

        public uint Read(string InputLines, bool Append = false)
        {
            TokenRow[] rows;
            uint line = 0;

            // Input sanity checking
            if (InputLines == null)
                return 1;

            // Clear the existing elements if desired
            if (!Append)
                this.Clear();

            // Run the input through the lexer to produce tokens
            if ((line = Parser.Tokenize(InputLines, out rows)) != 0)
                return line; // We have an error on a particular line

            // Reset the line
            line = 0;

            // Finally parse the tokens into entities
            return this.Read(rows, Append);
        }

        public uint Read(TokenRow[] Tokens, bool Append = false)
        {
            Entity[] entities;
            uint line = 0;

            // Input sanity checking
            if (Tokens == null)
                return 1;

            // Run the tokens through the parser to turn them into Entities and Elements
            if (!Parser.Parse(Tokens, ref line, out entities))
                return line + 1;

            // Finally, add the new elements to the list
            this.AddRange(entities);

            return 0;
        }
    }
}
