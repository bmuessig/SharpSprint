using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.IO;
using SharpSprint.Primitives;

namespace SharpSprint.Elements
{
    public class Component : Entity
    {
        // Required parameters
        public List<Entity> Entities { get; private set; }
        public IDText IDText { get; set; }
        public ValueText ValueText { get; set; }

        // Optional parameters
        public string Comment { get; set; }
        public bool UsePickplace { get; set; } // False
        public string Package { get; set; }
        public IntegerAngle Rotation { get; set; }

        // Default optional parameters
        private const bool UsePickplaceDefault = false;
        private const uint RotationDefault = 0;

        // Required and optional count
        private const byte RequiredArgCount = 0;
        private const byte OptionalArgCount = 4;

        public Component(IDText IDText, ValueText ValueText, params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.Comment = string.Empty;
            this.UsePickplace = UsePickplaceDefault;
            this.Package = string.Empty;
            this.Rotation = new IntegerAngle(RotationDefault);
        }

        public Component(IDText IDText, ValueText ValueText, bool UsePickplace, params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.Comment = string.Empty;
            this.UsePickplace = UsePickplace;
            this.Package = string.Empty;
            this.Rotation = new IntegerAngle(RotationDefault);
        }

        public Component(IDText IDText, ValueText ValueText, string Package, params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.Comment = string.Empty;
            this.UsePickplace = true;
            this.Package = Package;
            this.Rotation = new IntegerAngle(RotationDefault);
        }

        public Component(IDText IDText, ValueText ValueText, string Package, IntegerAngle Rotation, params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.Comment = string.Empty;
            this.UsePickplace = true;
            this.Package = Package;
            this.Rotation = Rotation;
        }

        public Component(IDText IDText, ValueText ValueText, string Package, IntegerAngle Rotation, string Comment,
            params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.Comment = Comment;
            this.UsePickplace = true;
            this.Package = Package;
            this.Rotation = Rotation;
        }

        public static bool Identify(TokenRow[] Tokens, uint Pointer)
        {
            // First, make sure we have met the amount of required arguments
            if (Tokens[Pointer].Count < RequiredArgCount + 1)
                return false;

            // Then, make sure we actually have a BEGIN_COMPONENT element next
            if (Tokens[Pointer][0].Type != Token.TokenType.Keyword
                || Tokens[Pointer][0].Handle.ToUpper().Trim() != "BEGIN_COMPONENT")
                return false;

            // Otherwise, it looks alright
            return true;
        }
        
        public static bool Read(TokenRow[] Tokens, ref uint Pointer, out Component Result)
        {
            throw new NotImplementedException();
        }

        public bool Write(out TokenRow[] Tokens)
        {
            TokenWriter writer = new TokenWriter();
            Tokens = null;

            // Start of block
            writer.Write(new Token("BEGIN_COMPONENT", Token.IndentTransition.In));

            // Check for any optional arguments to be added
            // Comment
            if (!string.IsNullOrEmpty(Comment))
                writer.Write(new Token("COMMENT", Comment));

            // UsePickplace
            if (UsePickplace != UsePickplaceDefault)
                writer.Write(new Token("USE_PICKPLACE", UsePickplace));

            // Package
            if (!string.IsNullOrEmpty(Package))
                writer.Write(new Token("PACKAGE", Package));

            // Rotation
            if (Rotation.Value != RotationDefault)
                writer.Write(new Token("ROTATION", Rotation.Value));

            // End the begin line
            writer.NewLine();

            // ID Text
            if (IDText != null)
            {
                TokenRow[] IDTokens;
                if (IDText.Write(out IDTokens))
                {
                    if (IDTokens.Length == 1)
                    {
                        writer.Write(IDTokens[0]);
                        writer.NewLine();
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;

            // Value Text
            if (ValueText != null)
            {
                TokenRow[] ValueTokens;
                if (ValueText.Write(out ValueTokens))
                {
                    if (ValueTokens.Length == 1)
                    {
                        writer.Write(ValueTokens[0]);
                        writer.NewLine();
                    }
                    else
                        return false;
                }
                else
                    return false;
            }
            else
                return false;

            // Write the other entities
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
            writer.Write(new Token("END_COMPONENT", Token.IndentTransition.Out));
            Tokens = writer.Compile();
            return true;
        }
    }
}
