using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.IO
{
    public struct Token
    {
        //CIRCLE, LAYER=3, WIDTH=6000, CENTER=350000 / 250000, RADIUS=80000, START=90000, STOP=270000;

        public string Handle;
        public TokenType Type;
        public ulong FirstValue;
        public ulong SecondValue;
        public string TextValue;

        public enum TokenType : byte
        {

        }
    }
}
