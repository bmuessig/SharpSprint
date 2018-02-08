using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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
            get { return this.ToString(); }
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
            this.Type = TokenType.Boolean;
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

        public override string ToString()
        {
            return Token.ToString(this);
        }

        public static string ToString(Token Token)
        {
            switch (Token.Type)
            {
                case TokenType.Keyword:
                    return string.Format("{0}", Token.Handle);
                case TokenType.Value:
                    return string.Format("{0}={1}", Token.Handle, Token.FirstValue);
                case TokenType.Tuple:
                    return string.Format("{0}={1}/{2}", Token.Handle, Token.FirstValue, Token.SecondValue);
                case TokenType.Boolean:
                    return string.Format("{0}={1}", Token.Handle, Token.BoolValue ? "true" : "false");
                case TokenType.Text:
                    return string.Format("{0}=|{1}|", Token.Handle, Token.TextValue.Replace('|', '-')); // Strings may not contain the pipe symbol, so replace it
            }

            return null;
        }

        private static Regex RegexLineParser = new Regex(@"(\w+)(?:[ \t]*=[ \t]*(?:(\d+)(?:[ \t]*\/[ \t]*(\d+))?|(true|false)|\|(.*?)\|))?[ \t]*(?:,|;[ \t]*$)", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static Regex RegexParser = new Regex(@"^(\w+)(?:[ \t]*=[ \t]*(?:(\d+)(?:[ \t]*\/[ \t]*(\d+))?|(true|false)|\|(.*?)\|))?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static Token[] LineFromString(string Input, bool AllowDropValues = false)
        {
            List<Token> tokens = new List<Token>();
            MatchCollection matches = RegexLineParser.Matches(Input);
            if (matches.Count == 0)
                return null;

            // Make sure the entire string is consumed
            if (!AllowDropValues)
            {
                if (RegexLineParser.Replace(Input, string.Empty).Trim().Length > 0)
                    return null;
            }

            foreach (Match match in matches)
            {
                // Not sure if this is really needed
                if (!match.Success)
                    return null;

                // Now evaluate the result
                // Group 1: Keyword
                // Group 2: First Value
                // Group 3: Second Value
                // Group 4: true/false
                // Group 5: Text

                // We don't have a keyword
                if (string.IsNullOrWhiteSpace(match.Groups[1].Value))
                    return null;

                string keyword = match.Groups[1].Value.Trim();
                
                // One number
                if (match.Groups[2].Length > 0)
                {
                    ulong val1;

                    if (!ulong.TryParse(match.Groups[2].Value.Trim(), out val1))
                        return null; // Invalid number

                    // Is it a pair?
                    if (match.Groups[3].Length > 0)
                    {
                        ulong val2;

                        if (!ulong.TryParse(match.Groups[3].Value.Trim(), out val2))
                            return null; // Invalid number

                        tokens.Add(new Token(keyword, val1, val2));
                    }
                    else
                        tokens.Add(new Token(keyword, val1));
                }
                else if (match.Groups[4].Length > 0)
                { // We have got a boolean
                    if (match.Groups[4].Value.ToUpper() == "TRUE")
                        tokens.Add(new Token(keyword, true));
                    else if (match.Groups[4].Value.ToUpper() == "FALSE")
                        tokens.Add(new Token(keyword, false));
                    else
                        return null; // Invalid bool
                }
                else if (match.Groups[5].Length > 0) // We have got a string
                    tokens.Add(new Token(keyword, match.Groups[5].Value));
                else // We have a keyword on it's own without a value
                    tokens.Add(new Token(keyword));
            }

            return tokens.ToArray();
        }

        public static bool FromString(string Input, out Token Result)
        {
            Result = new Token();

            Match match = RegexParser.Match(Input);
            if (!match.Success)
                return false;

            // Now evaluate the result
            // Group 1: Keyword
            // Group 2: First Value
            // Group 3: Second Value
            // Group 4: true/false
            // Group 5: Text

            // We don't have a keyword
            if (string.IsNullOrWhiteSpace(match.Groups[1].Value))
                return false;

            string keyword = match.Groups[1].Value.Trim();

            // One number
            if (match.Groups[2].Length > 0)
            {
                ulong val1;

                if (!ulong.TryParse(match.Groups[2].Value.Trim(), out val1))
                    return false; // Invalid number

                // Is it a pair?
                if (match.Groups[3].Length > 0)
                {
                    ulong val2;

                    if (!ulong.TryParse(match.Groups[3].Value.Trim(), out val2))
                        return false; // Invalid number

                    Result = new Token(keyword, val1, val2);
                }
                else
                    Result = new Token(keyword, val1);
            }
            else if (match.Groups[4].Length > 0)
            { // We have got a boolean
                if (match.Groups[4].Value.ToUpper() == "TRUE")
                    Result = new Token(keyword, true);
                else if (match.Groups[4].Value.ToUpper() == "FALSE")
                    Result = new Token(keyword, false);
                else
                    return false; // Invalid bool
            }
            else if (match.Groups[5].Length > 0) // We have got a string
                Result = new Token(keyword, match.Groups[5].Value);
            else // We have a keyword on it's own without a value
                Result = new Token(keyword);

            return true;
        }

        public enum TokenType : byte
        {
            Invalid,
            Keyword,
            Value,
            Tuple,
            Boolean,
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
