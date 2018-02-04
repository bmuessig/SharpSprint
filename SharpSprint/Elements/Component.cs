﻿using System;
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
        public Text IDText { get; set; }
        public Text ValueText { get; set; }

        // Optional parameters
        public bool IDVisible { get; set; } // True
        public bool ValueVisible { get; set; } // True
        public string Comment { get; set; }
        public bool UsePickplace { get; set; } // False
        public string Package { get; set; }
        public IntegerAngle Rotation { get; set; }

        // Default optional parameters
        private const bool IDVisibleDefault = true;
        private const bool ValueVisibleDefault = true;
        private const bool UsePickplaceDefault = false;

        public Component(Text IDText, Text ValueText, params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.IDVisible = IDVisibleDefault;
            this.ValueVisible = ValueVisibleDefault;
            this.Comment = string.Empty;
            this.UsePickplace = UsePickplaceDefault;
            this.Package = string.Empty;
            this.Rotation = new IntegerAngle(0);
        }

        public Component(Text IDText, bool IDVisible, Text ValueText, bool ValueVisible, params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.IDVisible = IDVisible;
            this.ValueVisible = ValueVisible;
            this.Comment = string.Empty;
            this.UsePickplace = UsePickplaceDefault;
            this.Package = string.Empty;
            this.Rotation = new IntegerAngle(0);
        }

        public Component(Text IDText, bool IDVisible, Text ValueText, bool ValueVisible, bool UsePickplace,
            params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.IDVisible = IDVisible;
            this.ValueVisible = ValueVisible;
            this.Comment = string.Empty;
            this.UsePickplace = UsePickplace;
            this.Package = string.Empty;
            this.Rotation = new IntegerAngle(0);
        }

        public Component(Text IDText, bool IDVisible, Text ValueText, bool ValueVisible, string Package,
            params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.IDVisible = IDVisible;
            this.ValueVisible = ValueVisible;
            this.Comment = string.Empty;
            this.UsePickplace = true;
            this.Package = Package;
            this.Rotation = new IntegerAngle(0);
        }

        public Component(Text IDText, bool IDVisible, Text ValueText, bool ValueVisible, string Package,
            IntegerAngle Rotation, params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.IDVisible = IDVisible;
            this.ValueVisible = ValueVisible;
            this.Comment = string.Empty;
            this.UsePickplace = true;
            this.Package = Package;
            this.Rotation = Rotation;
        }

        public Component(Text IDText, bool IDVisible, Text ValueText, bool ValueVisible, string Package,
            IntegerAngle Rotation, string Comment, params Entity[] Entities)
        {
            this.Entities = new List<Entity>(Entities);
            this.IDText = IDText;
            this.ValueText = ValueText;

            this.IDVisible = IDVisible;
            this.ValueVisible = ValueVisible;
            this.Comment = Comment;
            this.UsePickplace = true;
            this.Package = Package;
            this.Rotation = Rotation;
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

            // End the begin line
            writer.NewLine();

            // ID Text
            if (IDText != null)
            {
                Token[][] IDTokens;
                if (IDText.Write(out IDTokens))
                {
                    if (IDTokens.Length == 1)
                    {
                        writer.Write(IDTokens[0]);
                        if (IDVisible != IDVisibleDefault)
                            writer.Write(new Token("VISIBLE", IDVisible));
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
                Token[][] ValueTokens;
                if (ValueText.Write(out ValueTokens))
                {
                    if (ValueTokens.Length == 1)
                    {
                        writer.Write(ValueTokens[0]);
                        if (ValueVisible != ValueVisibleDefault)
                            writer.Write(new Token("VISIBLE", ValueVisible));
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
            writer.Write(new Token("END_COMPONENT", Token.IndentTransition.Out));
            Tokens = writer.Compile();
            return true;
        }
    }
}
