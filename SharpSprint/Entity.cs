using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpSprint.IO;

namespace SharpSprint
{
    public interface Entity
    {
        bool Write(out TokenRow[] Tokens);
    }
}
