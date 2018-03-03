using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.IO
{
    public class TokenWriter
    {
        private List<TokenRow> Collection;
        private TokenRow Line;

        public TokenWriter()
        {
            Collection = new List<TokenRow>();
            Line = new TokenRow();
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
            Collection.Add(Line);
            Line = new TokenRow();
        }

        public void Write(Token Token)
        {
            Line.Add(Token);
        }

        public void Write(TokenRow Tokens)
        {
            Line.AddRange(Tokens);
        }

        public void Write(TokenRow[] Lines)
        {
            foreach (TokenRow Line in Lines)
            {
                Write(Line);
                NewLine();
            }
        }

        public TokenRow[] Compile()
        {
            NewLine();
            return Collection.ToArray();
        }
    }
}
