using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharpSprint.IO
{
    public class Parser
    {

        public static uint Tokenize(string InputLines, out Token[][] OutputTokens)
        {
            List<Token[]> lines = new List<Token[]>();
            List<Token> line = new List<Token>();

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
                if(lastChr == '\r' && chr != '\n')
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
                        lines.Add(line.ToArray());

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
                lines.Add(line.ToArray());

            // Write the output array
            OutputTokens = lines.ToArray();
            return 0;
        }
    }
}
