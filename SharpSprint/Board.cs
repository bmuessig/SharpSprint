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
            uint line = 0;

            // Run the input through the lexer to produce tokens
            if ((line = Tokenizer.Tokenize(InputLines, out rows)) != 0)
                return line; // We have an error on a particular line

            // Now, try all element signatures on the input
            // The elements are arranged by most used first for speed
            while (line < rows.Length)
            {
                // Track
                if (Track.Identify(rows, line))
                {
                    // Try parsing the element
                    Track e;
                    if (!Track.Read(rows, ref line, out e))
                        return line;

                    // Finally add it
                    Canvas.Add(e);

                    // And proceed
                    continue;
                }

                // SMDPad
                if (SMDPad.Identify(rows, line))
                {
                    // Try parsing the element
                    SMDPad e;
                    if (!SMDPad.Read(rows, ref line, out e))
                        return line;

                    // Finally add it
                    Canvas.Add(e);

                    // And proceed
                    continue;
                }

                // THTPad
                if (THTPad.Identify(rows, line))
                {
                    // Try parsing the element
                    THTPad e;
                    if (!THTPad.Read(rows, ref line, out e))
                        return line;

                    // Finally add it
                    Canvas.Add(e);

                    // And proceed
                    continue;
                }

                // Group
                if (Group.Identify(rows, line))
                {
                    // Try parsing the element
                    Group e;
                    if (!Group.Read(rows, ref line, out e))
                        return line;

                    // Finally add it
                    Canvas.Add(e);

                    // And proceed
                    continue;
                }

                // Zone
                if (Zone.Identify(rows, line))
                {
                    // Try parsing the element
                    Zone e;
                    if (!Zone.Read(rows, ref line, out e))
                        return line;

                    // Finally add it
                    Canvas.Add(e);

                    // And proceed
                    continue;
                }

                // Text
                if (Text.Identify(rows, line))
                {
                    // Try parsing the element
                    Text e;
                    if (!Text.Read(rows, ref line, out e))
                        return line;

                    // Finally add it
                    Canvas.Add(e);

                    // And proceed
                    continue;
                }

                // Circle
                if (Circle.Identify(rows, line))
                {
                    // Try parsing the element
                    Circle e;
                    if (!Circle.Read(rows, ref line, out e))
                        return line;

                    // Finally add it
                    Canvas.Add(e);

                    // And proceed
                    continue;
                }

                // Component
                if (Component.Identify(rows, line))
                {
                    // Try parsing the element
                    Component e;
                    if (!Component.Read(rows, ref line, out e))
                        return line;

                    // Finally add it
                    Canvas.Add(e);

                    // And proceed
                    continue;
                }

                // Maybe add plugins in the future?
                // They would go here

                // Unknown element => fail
                return line;
            }

            return 0;
        }
    }
}
