using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.IO
{
    public class TokenWriter
    {
        private List<Token[]> Collection;
        private List<Token> Line;

        public TokenWriter()
        {
            Collection = new List<Token[]>();
            Line = new List<Token>();
        }

        public uint LineCount
        {
            get
            {
                return (uint)Collection.Count;
            }
        }

        public uint LineLength
        {
            get
            {
                return (uint)Line.Count;
            }
        }

        public void ClearAll()
        {
            Collection.Clear();
        }

        public void ClearLine()
        {
            Line.Clear();
        }

        public void ClearLastLine()
        {
            if (Collection.Count == 0)
                return;
            Collection.RemoveAt(Collection.Count - 1);
        }

        public void NewLine()
        {
            if (Line.Count == 0)
                return;
            Collection.Add(Line.ToArray());
            Line.Clear();
        }

        public void Write(Token Token)
        {
            Line.Add(Token);
        }

        public void Write(Token[] Tokens)
        {
            Line.AddRange(Tokens);
        }

        public void Write(Token[][] Lines)
        {
            foreach (Token[] Line in Lines)
            {
                Write(Line);
                NewLine();
            }
        }

        public Token[][] Compile()
        {
            NewLine();
            return Collection.ToArray();
        }
    }
}
