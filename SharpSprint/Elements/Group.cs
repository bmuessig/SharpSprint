using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.IO;

namespace SharpSprint.Elements
{
    public class Group : Entity
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
            Token token;

            // Now, consume all inner arguments, until we hit our END_GROUP token
            while (Pointer < Tokens.Length)
            {
                if (Tokens[Pointer].Count == 1)
                {
                    if (Tokens[Pointer][0].Type == Token.TokenType.Keyword
                        && Tokens[Pointer][0].Handle.ToUpper().Trim() == "END_GROUP")
                        break;
                }
            }

            // Make sure all tokens have been consumed
            //if (Tokens[Pointer].Count > RequiredArgCount + optCount + 1)
                //return false;

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
