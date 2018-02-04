using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpSprint.IO
{
    public struct Token
    {
        //CIRCLE, LAYER=3, WIDTH=6000, CENTER=350000 / 250000, RADIUS=80000, START=90000, STOP=270000;

        public TokenType Type;
        public IndentTransition Indent;
        public string Handle;
        public ulong FirstValue;
        public ulong SecondValue;
        public bool BoolValue;
        public string TextValue;
        
        public string Encoded
        {
            get
            {
                switch (Type)
                {
                    case TokenType.Keyword:
                        return string.Format("{0}", Handle);
                    case TokenType.Value:
                        return string.Format("{0}={1}", Handle, FirstValue);
                    case TokenType.Tuple:
                        return string.Format("{0}={1}/{2}", Handle, FirstValue, SecondValue);
                    case TokenType.Bool:
                        return string.Format("{0}={1}", Handle, BoolValue ? "true" : "false");
                    case TokenType.Text:
                        return string.Format("{0}=|{1}|", Handle, TextValue.Replace('|', '-')); // Strings may not contain the pipe symbol, so replace it
                    default:
                        return null;
                }
            }
        }

        public Token(string Handle)
        {
            this.Type = TokenType.Keyword;
            this.Indent = IndentTransition.None;
            this.Handle = Handle;
            this.FirstValue = 0;
            this.SecondValue = 0;
            this.BoolValue = false;
            this.TextValue = string.Empty;
        }

        public Token(string Handle, IndentTransition Indent)
        {
            this.Type = TokenType.Keyword;
            this.Indent = Indent;
            this.Handle = Handle;
            this.FirstValue = 0;
            this.SecondValue = 0;
            this.BoolValue = false;
            this.TextValue = string.Empty;
        }

        public Token(string Handle, ulong Value)
        {
            this.Type = TokenType.Value;
            this.Indent = IndentTransition.None;
            this.Handle = Handle;
            this.FirstValue = Value;
            this.SecondValue = 0;
            this.BoolValue = false;
            this.TextValue = string.Empty;
        }

        public Token(string Handle, ulong FirstValue, ulong SecondValue)
        {
            this.Type = TokenType.Tuple;
            this.Indent = IndentTransition.None;
            this.Handle = Handle;
            this.FirstValue = FirstValue;
            this.SecondValue = SecondValue;
            this.BoolValue = false;
            this.TextValue = string.Empty;
        }

        public Token(string Handle, bool Value)
        {
            this.Type = TokenType.Value;
            this.Indent = IndentTransition.None;
            this.Handle = Handle;
            this.FirstValue = 0;
            this.SecondValue = 0;
            this.BoolValue = Value;
            this.TextValue = string.Empty;
        }

        public Token(string Handle, string TextValue)
        {
            this.Type = TokenType.Text;
            this.Indent = IndentTransition.None;
            this.Handle = Handle;
            this.FirstValue = 0;
            this.SecondValue = 0;
            this.BoolValue = false;
            this.TextValue = TextValue;
        }

        public enum TokenType : byte
        {
            Invalid,
            Keyword,
            Value,
            Tuple,
            Bool,
            Text
        }

        public enum IndentTransition : byte
        {
            None,
            In,
            Out
        }
    }
}
