using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.IO
{
    public class TokenRow : List<Token>
    {
        public string ArrayPrefix { get; set; }

        public uint ArrayPointer { get; set; }

        private uint InternalPointer = 0;

        public bool Contains(string Keyword, bool ExactMatch = false)
        {
            if (string.IsNullOrWhiteSpace(Keyword))
                return false;

            if (!ExactMatch)
                Keyword = Keyword.Trim().ToUpper();

            foreach (Token t in this)
            {
                if (string.IsNullOrWhiteSpace(t.Handle))
                    continue;

                if (ExactMatch)
                {
                    if (t.Handle == Keyword)
                        return true;
                }
                else
                {
                    if (t.Handle.Trim().ToUpper() == Keyword)
                        return true;
                }
            }

            return false;
        }

        public bool Get(string Keyword, out Token Result, bool ExactMatch = false)
        {
            Result = new Token();

            // If there are no elements or no keyword, there cannot be a result
            if (this.Count == 0 || string.IsNullOrWhiteSpace(Keyword))
                return false;

            // If the case does not matter, make it all uppercase
            if (!ExactMatch)
                Keyword = Keyword.Trim().ToUpper();

            // This is supposed to run for one loop, no matter where we start in the array
            for (int i = 0; i < this.Count; i++, InternalPointer = (uint)((InternalPointer + 1) % this.Count))
            {
                if (string.IsNullOrWhiteSpace(this[(int)InternalPointer].Handle))
                    continue;

                if (ExactMatch)
                {
                    if (this[(int)InternalPointer].Handle == Keyword)
                    {
                        Result = this[(int)InternalPointer];
                        return true;
                    }
                }
                else
                {
                    if (this[(int)InternalPointer].Handle.Trim().ToUpper() == Keyword)
                    {
                        Result = this[(int)InternalPointer];
                        return true;
                    }
                }
            }

            // The above code works, but might be slower than the above code, if arguments are present in the default order
            /*
            foreach (Token t in this)
            {
                if (ExactMatch)
                {
                    if (t.Handle == Keyword)
                    {
                        Result = t;
                        return true;
                    }
                }
                else
                {
                    if (t.Handle.Trim().ToUpper() == Keyword)
                    {
                        Result = t;
                        return true;
                    }
                }
            }
             */

            return false;
        }

        public bool HasDuplicates(bool ExactMatch = false)
        {
            string mainKeyword = string.Empty;
            List<string> arguments = new List<string>();

            for (int ptr = 0; ptr < this.Count; ptr++)
            {
                if (string.IsNullOrWhiteSpace(this[ptr].Handle))
                    continue;

                // If the first token is just a keyword (as it should be), don't add it to the duplicates list
                if (ptr == 0 && this[ptr].Type == Token.TokenType.Keyword)
                {
                    // But instead, store it
                    mainKeyword = ExactMatch ? this[ptr].Handle : this[ptr].Handle.ToUpper();
                    continue;
                }

                if (ExactMatch)
                {
                    // First, check if the argument is a keyword and equal to the first one on the line
                    if (this[ptr].Type == Token.TokenType.Keyword && this[ptr].Handle == mainKeyword)
                        return true;

                    // Check if the argument is already known
                    if (arguments.Contains(this[ptr].Handle))
                        return true;

                    // Finally, add the argument to the list
                    arguments.Add(this[ptr].Handle);
                }
                else
                {
                    // First, check if the argument is a keyword and equal to the first one on the line
                    if (this[ptr].Type == Token.TokenType.Keyword && this[ptr].Handle == mainKeyword.ToUpper())
                        return true;

                    // Check if the argument is already known
                    if (arguments.Contains(this[ptr].Handle.Trim().ToUpper()))
                        return true;

                    // Finally, add the argument to the list
                    arguments.Add(this[ptr].Handle.Trim().ToUpper());
                }
            }

            return false;
        }

        public bool ArrayGet(out Token Result, bool IncrementPointer = true, bool ExactMatch = false)
        {
            Result = new Token();

            // If there are no elements or an invalid search string, there cannot be a result
            if (this.Count == 0 || string.IsNullOrWhiteSpace(ArrayPrefix))
                return false;

            // Now, assemble the search string
            string search = string.Format("{0}{1}",
                ExactMatch ? ArrayPrefix : ArrayPrefix.ToUpper().Trim(), ArrayPointer);

            // This is supposed to run for one loop, no matter where we start in the array
            // i, in this case, is just the helper variable for the counter
            // We also don't want to land on the field we started on, but one field later
            for (int i = 0; i <= this.Count; i++)
            {
                if (ExactMatch)
                {
                    if (this[(int)InternalPointer].Handle == search)
                    {
                        Result = this[(int)InternalPointer];
                        InternalPointer = (uint)((InternalPointer + 1) % this.Count);
                        if (IncrementPointer)
                            ArrayPointer++;
                        return true;
                    }
                }
                else
                {
                    if (this[(int)InternalPointer].Handle.Trim().ToUpper() == search)
                    {
                        Result = this[(int)InternalPointer];
                        InternalPointer = (uint)((InternalPointer + 1) % this.Count);
                        if (IncrementPointer)
                            ArrayPointer++;
                        return true;
                    }
                }

                InternalPointer = (uint)((InternalPointer + 1) % this.Count);
            }

            return false;
        }
    }
}
