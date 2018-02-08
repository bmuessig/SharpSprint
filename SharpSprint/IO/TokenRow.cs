﻿using System;
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
            if (!ExactMatch)
                Keyword = Keyword.Trim().ToUpper();

            foreach (Token t in this)
            {
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

            // If there are no elements, there cannot be a result
            if (this.Count == 0)
                return false;

            // If the case does not matter, make it all uppercase
            if (!ExactMatch)
                Keyword = Keyword.Trim().ToUpper();

            // This is supposed to run for one loop, no matter where we start in the array
            for (int i = 0; i < this.Count; i++, InternalPointer = (uint)((InternalPointer + 1) % this.Count))
            {
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
            List<string> keywords = new List<string>();
            foreach (Token t in this)
            {
                if (ExactMatch)
                {
                    if (keywords.Contains(t.Handle))
                        return true;
                    keywords.Add(t.Handle);
                }
                else
                {
                    if (keywords.Contains(t.Handle.Trim().ToUpper()))
                        return true;
                    keywords.Add(t.Handle.Trim().ToUpper());
                }
            }

            return false;
        }

        public bool ArrayGet(out Token Result, bool IncrementPointer = true, bool ExactMatch = false)
        {
            Result = new Token();

            // If there are no elements, there cannot be a result
            if (this.Count == 0)
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