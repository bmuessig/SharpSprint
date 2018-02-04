using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.IO;

namespace SharpSprint
{
    public interface Entity
    {
        bool Read(Token[] Tokens);
        bool Write(out Token[] Tokens);
    }
}
