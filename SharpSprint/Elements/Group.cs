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

        public Group(params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
        }

        public bool Read(Token[][] Tokens, ref uint Pointer)
        {
            throw new NotImplementedException();
        }

        public bool Write(out Token[][] Tokens)
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
                    Token[][] EntityTokens;
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
