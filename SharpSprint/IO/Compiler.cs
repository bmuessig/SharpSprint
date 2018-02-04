using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.IO
{
    public class Compiler
    {
        // 
        public string CompileLine(Token[] Tokens)
        {
            StringBuilder sb = new StringBuilder();

            foreach (Token token in Tokens)
            {
                switch (token.Type)
                {
                    case Token.TokenType.Keyword:

                        break;
                    case Token.TokenType.Value:

                        break;
                    case Token.TokenType.Tuple:

                        break;
                    case Token.TokenType.Bool:

                        break;
                    case Token.TokenType.Text:

                        break;
                }
            }

            return sb.ToString();
        }
    }
}
