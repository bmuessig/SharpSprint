using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.Elements;
using SharpSprint.IO;

namespace SharpSprint
{
    public class Board
    {
        private Group Entities;

        public List<Entity> Canvas
        {
            get { return Entities.Entities; }
        }

        public Board()
        {
            Entities = new Group();
        }

        public bool Compile(out string Result)
        {
            Result = null;

            Token[][] tokens;
            if (!Entities.Write(out tokens))
                return false;

            ushort indent = 0;
            return Compiler.CompileBlock(tokens, ref indent, out Result);
        }
    }
}
