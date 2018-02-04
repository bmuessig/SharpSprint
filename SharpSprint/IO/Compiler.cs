using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.IO
{
    public class Compiler
    {
        public bool CompileBlock(Token[][] Lines, ref ushort Indent, out string Result)
        {
            StringBuilder sb = new StringBuilder();
            Result = null;
            
            foreach (Token[] line in Lines)
            {
                string lineContent;

                // Compile the line
                if (!CompileLine(line, ref Indent, out lineContent))
                    return false;

                // Check if the line is null
                if(lineContent == null)
                    return false;

                // Skip the line if it is empty
                if (lineContent.Length == 0 || lineContent.Trim() == ";")
                    continue;

                // Append the content
                sb.Append(lineContent);

                // Create a new line at the end
                sb.Append("\r\n");
            }

            Result = sb.ToString();
            return true;
        }

        public bool CompileLine(Token[] Tokens, ref ushort Indent, out string Result)
        {
            StringBuilder sb = new StringBuilder();
            Result = null;

            // Indent the output
            for(int tab = 0; tab < Indent; tab++)
                sb.Append("   ");

            foreach (Token token in Tokens)
            {
                // Get the encoded representation
                string encoded = token.Encoded;

                // Fail if we have got nothing
                if(encoded == null)
                    return false;

                // Skip it if it is just empty
                if (string.IsNullOrWhiteSpace(encoded))
                    continue;

                // Handle indenting
                if(token.Indent == Token.IndentTransition.In)
                    Indent++;
                else if(token.Indent == Token.IndentTransition.Out)
                    Indent--;

                // If neccessairy, append a comma
                if (sb.Length > 0)
                    sb.Append(", ");

                // Write the encoded content
                sb.Append(encoded);
            }

            // Terminate the line with a semicolon
            sb.Append(';');

            Result = sb.ToString();
            return true;
        }
    }
}
