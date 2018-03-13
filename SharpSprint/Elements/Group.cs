using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.IO;

namespace SharpSprint.Elements
{
    public class Group : Container
    {
        // Required Parameters
        public List<Entity> Entities { get; private set; }

        // Required and optional count
        private const byte RequiredArgCount = 0;
        private const byte OptionalArgCount = 0;

        public Group(params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
        }

        public static bool Identify(TokenRow[] Tokens, uint Pointer)
        {
            // Input sanity check
            if (Tokens == null)
                return false;

            // First, make sure we have met the amount of required arguments
            if (Tokens[Pointer].Count != RequiredArgCount + 1)
                return false;

            // Also, check if the pointer is within range
            if (Pointer >= Tokens.Length)
                return false;

            // Then, make sure we actually have a GROUP element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "GROUP")
                return false;

            // Otherwise, it looks alright
            return true;
        }

        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out Group Result)
        {
            Result = null;

            // Check if we have got a valid signature
            if (!Identify(Tokens, Pointer))
                return false;

            // Define the working variables
            Group group = new Group();
            bool endFound = false;
            Entity[] entities;

            // Skip ahead to first group element (or the end if it's empty)
            // Then, consume all inner arguments, until we hit our END_GROUP token or the end of input
            for (Pointer++; Pointer < Tokens.Length; Pointer++ )
            {
                // Fail, if the TokenRow is null
                if (Tokens[Pointer] == null)
                    return false;

                // Skip empty rows
                if (Tokens[Pointer].Count < 1)
                    continue;

                // Check if we have hit the end element
                if (Tokens[Pointer][0].Type == Token.TokenType.Keyword
                    && Tokens[Pointer][0].Handle.ToUpper().Trim() == "END_GROUP")
                {
                    // Make sure that the end is only a single token
                    if (Tokens[Pointer].Count != 1)
                        return false;

                    // Store that we have found an end
                    endFound = true;

                    // Break out of the loop
                    break;
                }
                else
                {
                    // If it's not an end, try parsing it as one of the other elements
                    if (!Parser.Parse(Tokens, ref Pointer, out entities, 1))
                        return false;

                    // Now, add the new entities to the group
                    group.Entities.AddRange(entities);
                }
            }

            // Make sure we have not just hit the end of the stream
            if (!endFound)
                return false;

            // Return the successful new element
            Result = group;
            return true;
        }

        public bool Write(out TokenRow[] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Start of block
            writer.Write(new Token("GROUP", Token.IndentTransition.In));
            writer.NewLine();

            // Write the group entities
            if (Entities.Count > 0)
            {
                foreach (Entity entity in Entities)
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

            // End of block
            writer.Write(new Token("END_GROUP", Token.IndentTransition.Out));
            Tokens = writer.Compile();
            return true;
        }
    }
}
