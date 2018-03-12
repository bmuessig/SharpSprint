using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Elements;

namespace SharpSprint.IO
{
    public class Parser
    {
        public static uint Tokenize(string InputLines, out TokenRow[] OutputTokens)
        {
            List<TokenRow> lines = new List<TokenRow>();
            TokenRow line = new TokenRow();

            OutputTokens = null;

            StringBuilder builder = new StringBuilder();
            uint lineNumber = 0;
            bool inString = false;
            char lastChr = (char)0;

            foreach (char chr in InputLines)
            {
                // Check if there was a semicolon last
                if (lastChr == ';' && chr != '\r' && chr != '\t' && chr != ' ')
                    return lineNumber;

                // Make sure we have proper line endings
                // If there was a carriage return, we want a linefeed
                // Unix-style, linefeed-only endings are just fine too
                if (lastChr == '\r' && chr != '\n')
                    return lineNumber;

                // Handle line endings first, so that they don't slip though
                if (chr == '\r' || chr == '\n')
                {
                    // Are we currently within a string
                    if (inString)
                        return lineNumber;

                    // Check, if we have missed an end terminator
                    if (lastChr != ';' && lastChr != '\r')
                        return lineNumber;

                    // If we've got a CR, skip to the next char
                    if (chr == '\r')
                    {
                        lastChr = chr;
                        continue;
                    }

                    // Now that we are here, we had an optional CR and now have a LF
                    // Check if we need to push the line
                    if (line.Count > 0)
                    {
                        // Add the line to the set if it is not empty
                        lines.Add(line);

                        // And clear the line
                        line.Clear();
                    }

                    // We need to update the line number
                    lineNumber++;

                    // Update the last char
                    lastChr = chr;
                }
                else if (chr == '|')
                {
                    // We can't have two strings back to back
                    if (!inString && lastChr == '|')
                        return lineNumber;

                    // Update inString
                    inString = !inString;

                    // Append to the output
                    builder.Append(chr);

                    // And update lastChr
                    lastChr = chr;
                }
                else if (!inString && (chr == ';' || chr == ','))
                {
                    // Check first, whether the line is empty
                    if (!string.IsNullOrWhiteSpace(builder.ToString()))
                    {
                        Token result;
                        if (Token.FromString(builder.ToString().Trim(), out result))
                            line.Add(result);
                        else
                            return lineNumber;
                    }

                    // Reset the builder
                    builder.Clear();

                    // Update the last char
                    lastChr = chr;
                }
                else if ((!char.IsControl(chr) && inString)
                    || (!char.IsControl(chr) && !inString && chr != ' ' && chr != '\t'))
                {
                    // Append the char if it is not a control char, not a whitespace char (unless it's a string)
                    builder.Append(chr);
                    lastChr = chr;
                } // All chars that are unhandled and aren't whitespace are syntax errors
                else if (chr != ' ' && chr != '\t')
                    return lineNumber;
            }

            // Now finish off the last line
            // Check first, whether the line is empty
            if (!string.IsNullOrWhiteSpace(builder.ToString()))
            {
                Token result;
                if (Token.FromString(builder.ToString().Trim(), out result))
                    line.Add(result);
                else
                    return lineNumber;
            }

            // Add the line to the set if it is not empty
            if (line.Count > 0)
                lines.Add(line);

            // Write the output array
            OutputTokens = lines.ToArray();
            return 0;
        }
        
        // Count = 0: Process all
        public static bool Parse(TokenRow[] Rows, ref uint Pointer, out Entity[] Result, uint Count = 0)
        {
            // Working variables
            List<Entity> entities = new List<Entity>();

            // Assign a value to result ahead of time to allow early exiting
            Result = null;

            // Make sure the pointer is not out of bounds
            if (Pointer >= Rows.Length)
                return false;

            // Check, if we need to process all following tokens
            bool processAll = (Count == 0);

            // Now, try all element signatures on the input
            // The elements are arranged by most used first for speed
            for (; (Count > 0 || processAll) && Pointer < Rows.Length; Pointer++ )
            {
                // Track
                if (Track.Identify(Rows, Pointer))
                {
                    // Try parsing the element
                    Track e;
                    if (!Track.Read(Rows, ref Pointer, out e))
                        return false;

                    // Finally add it
                    entities.Add(e);

                    // Update the counter
                    if (!processAll)
                        Count--;

                    // And proceed
                    continue;
                }

                // SMDPad
                if (SMDPad.Identify(Rows, Pointer))
                {
                    // Try parsing the element
                    SMDPad e;
                    if (!SMDPad.Read(Rows, ref Pointer, out e))
                        return false;

                    // Finally add it
                    entities.Add(e);

                    // Update the counter
                    if (!processAll)
                        Count--;

                    // And proceed
                    continue;
                }

                // THTPad
                if (THTPad.Identify(Rows, Pointer))
                {
                    // Try parsing the element
                    THTPad e;
                    if (!THTPad.Read(Rows, ref Pointer, out e))
                        return false;

                    // Finally add it
                    entities.Add(e);

                    // Update the counter
                    if (!processAll)
                        Count--;

                    // And proceed
                    continue;
                }

                // Group
                if (Group.Identify(Rows, Pointer))
                {
                    // Try parsing the element
                    Group e;
                    if (!Group.Read(Rows, ref Pointer, out e))
                        return false;

                    // Finally add it
                    entities.Add(e);

                    // Update the counter
                    if (!processAll)
                        Count--;

                    // And proceed
                    continue;
                }

                // Zone
                if (Zone.Identify(Rows, Pointer))
                {
                    // Try parsing the element
                    Zone e;
                    if (!Zone.Read(Rows, ref Pointer, out e))
                        return false;

                    // Finally add it
                    entities.Add(e);

                    // Update the counter
                    if (!processAll)
                        Count--;

                    // And proceed
                    continue;
                }

                // Text
                if (Text.Identify(Rows, Pointer))
                {
                    // Try parsing the element
                    Text e;
                    if (!Text.Read(Rows, ref Pointer, out e))
                        return false;

                    // Finally add it
                    entities.Add(e);

                    // Update the counter
                    if (!processAll)
                        Count--;

                    // And proceed
                    continue;
                }

                // Circle
                if (Circle.Identify(Rows, Pointer))
                {
                    // Try parsing the element
                    Circle e;
                    if (!Circle.Read(Rows, ref Pointer, out e))
                        return false;

                    // Finally add it
                    entities.Add(e);

                    // Update the counter
                    if (!processAll)
                        Count--;

                    // And proceed
                    continue;
                }

                // Component
                if (Component.Identify(Rows, Pointer))
                {
                    // Try parsing the element
                    Component e;
                    if (!Component.Read(Rows, ref Pointer, out e))
                        return false;

                    // Finally add it
                    entities.Add(e);

                    // Update the counter
                    if (!processAll)
                        Count--;

                    // And proceed
                    continue;
                }

                // Maybe add plugins in the future?
                // They would go here

                // Unknown element => fail
                return false;
            }

            return true;
        }
    }
}
