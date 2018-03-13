using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.IO;
using SharpSprint.Primitives;

namespace SharpSprint.Elements
{
    public class Component : Container
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

        private Component()
        {
            this.Entities = new List<Entity>();
            this.IDText = null;
            this.ValueText = null;

            this.Comment = string.Empty;
            this.UsePickplace = UsePickplaceDefault;
            this.Package = string.Empty;
            this.Rotation = new IntegerAngle(RotationDefault);
        }

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
            // Input sanity check
            if (Tokens == null)
                return false;

            // First, make sure we have met the amount of required arguments
            if (Tokens[Pointer].Count < RequiredArgCount + 1)
                return false;

            // Also, check if the pointer is within range
            if (Pointer >= Tokens.Length)
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
            Result = null;

            // Check if we have got a valid signature
            if (!Identify(Tokens, Pointer))
                return false;

            // Define the working variables
            Component component = new Component();
            bool idFound = false, valueFound = false, endFound = false;
            Token token;
            Entity[] entities;

            // First, check if there are any optional arguments
            if (Tokens[Pointer].Count > 1)
            {
                uint optCount = 0;

                // Syntax error, because duplicates are not allowed
                if (Tokens[Pointer].HasDuplicates())
                    return false;

                // COMMENT
                if (Tokens[Pointer].Get("COMMENT", out token))
                {
                    // Make sure we have got the correct type and valid content
                    if (token.Type != Token.TokenType.Text || token.TextValue == null)
                        return false;
                    // Store the value
                    component.Comment = token.TextValue;
                    // Increment the optional argument count
                    optCount++;
                }

                // USEPICKPLACE
                if (Tokens[Pointer].Get("USE_PICKPLACE", out token))
                {
                    // Make sure we have got the correct type
                    if (token.Type != Token.TokenType.Boolean)
                        return false;
                    // Store the value
                    component.UsePickplace = token.BoolValue;
                    // Increment the optional argument count
                    optCount++;
                }

                // PACKAGE
                if (Tokens[Pointer].Get("PACKAGE", out token))
                {
                    // Make sure we have got the correct type and valid content
                    if (token.Type != Token.TokenType.Text || token.TextValue == null)
                        return false;
                    // Store the value
                    component.Package = token.TextValue;
                    // Increment the optional argument count
                    optCount++;
                }

                // ROTATION
                if (Tokens[Pointer].Get("ROTATION", out token))
                {
                    // Make sure we have got the correct type
                    if (token.Type != Token.TokenType.Value)
                        return false;
                    if (token.FirstValue > uint.MaxValue)
                        return false;
                    // Store the value
                    component.Rotation = new IntegerAngle((uint)token.FirstValue);
                    // Increment the optional argument count
                    optCount++;
                }

                // Make sure, we've used up all arguments
                if (RequiredArgCount + optCount + 1 != Tokens[Pointer].Count)
                    return false;
            }

            // Skip ahead to first group element
            // Then, consume all inner arguments, until we hit our END_COMPONENT token or the end of input
            for (Pointer++; Pointer < Tokens.Length; Pointer++)
            {
                // Fail, if the TokenRow is null
                if (Tokens[Pointer] == null)
                    return false;

                // Skip empty rows
                if (Tokens[Pointer].Count < 1)
                    continue;

                // Check if we have hit the end element
                if (Tokens[Pointer][0].Type == Token.TokenType.Keyword
                    && Tokens[Pointer][0].Handle.ToUpper().Trim() == "END_COMPONENT")
                {
                    // Make sure that the end is only a single token
                    if (Tokens[Pointer].Count != 1)
                        return false;

                    // Store that we have found an end
                    endFound = true;

                    // Break out of the loop
                    break;
                }
                else if (IDText.Identify(Tokens, Pointer))
                {
                    // We can't have two IDText elements
                    if (idFound)
                        return false;

                    // Try parsing the element
                    IDText e;
                    if (!IDText.Read(Tokens, ref Pointer, out e))
                        return false;

                    // Finally add it
                    component.IDText = e;

                    // Mark it as found
                    idFound = true;
                }
                else if (ValueText.Identify(Tokens, Pointer))
                {
                    // We can't have two ValueText elements
                    if (valueFound)
                        return false;

                    // Try parsing the element
                    ValueText e;
                    if (!ValueText.Read(Tokens, ref Pointer, out e))
                        return false;

                    // Finally add it
                    component.ValueText = e;

                    // Mark it as found
                    valueFound = true;
                }
                else
                {
                    // If it's not an end, try parsing it as one of the other elements
                    if (!Parser.Parse(Tokens, ref Pointer, out entities, 1))
                        return false;

                    // Now, add the new entities to the group
                    component.Entities.AddRange(entities);
                }
            }

            // Make sure we have not just hit the end of the stream
            // Also make sure we have found our ID and ValueText
            if (!endFound || !idFound || !valueFound)
                return false;

            // Return the successful new element
            Result = component;
            return true;
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
