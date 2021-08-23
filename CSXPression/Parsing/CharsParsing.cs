using CSXPression.Tokens;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CSXPression.Parsing
{
    public partial class Parser
    {
        protected Regex internalCharRegex = new(@"^['](\\[\\'0abfnrtv]|[^'])[']", RegexOptions.Compiled);

        protected static readonly IDictionary<char, char> charEscapedCharDict = new Dictionary<char, char>()
        {
            { '\\', '\\' },
            { '\'', '\'' },
            { '0', '\0' },
            { 'a', '\a' },
            { 'b', '\b' },
            { 'f', '\f' },
            { 'n', '\n' },
            { 'r', '\r' },
            { 't', '\t' },
            { 'v', '\v' }
        };

        protected virtual bool ParseChar(string expression, Stack<IToken> stack, ref int i)
        {
            if (!Functionalities.CharParsing)
                return false;

            string s = expression.Substring(i, 1);

            if (s.Equals("'"))
            {
                i++;

                if (expression.Substring(i, 1).Equals(@"\"))
                {
                    i++;
                    char escapedChar = expression[i];

                    if (charEscapedCharDict.ContainsKey(escapedChar))
                    {
                        stack.Push(new ConstantToken(charEscapedCharDict[escapedChar]));
                        i++;
                    }
                    else
                    {
                        throw new ParsingException("Not known escape sequence in literal character");
                    }
                }
                else if (expression.Substring(i, 1).Equals("'"))
                {
                    throw new ParsingException("Empty literal character is not valid");
                }
                else
                {
                    stack.Push(new ConstantToken(expression[i]));
                    i++;
                }

                if (expression.Substring(i, 1).Equals("'"))
                {
                    return true;
                }
                else
                {
                    throw new ParsingException("Too much characters in the literal character");
                }
            }
            else
            {
                return false;
            }
        }
    }
}
