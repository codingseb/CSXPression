using CSXPression.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace CSXPression.Parsing
{
    /// <summary>
    /// A customizable parser that can create the tokens tree with a code string by the defined syntax.
    /// By default try to be as near of C# syntax as possible.
    /// </summary>
    public partial class Parser : IParser
    {
        protected delegate bool ParsingMethodDelegate(string expression, Stack<IToken> stack, ref int i);

        private IList<ParsingMethodDelegate> parsingMethods;
        protected virtual IList<ParsingMethodDelegate> ParsingMethods => parsingMethods ??= new List<ParsingMethodDelegate>()
        {
            ParseNumber,
            ParseOperators,
        };

        public ParserOptions Options { get; set; } = new ParserOptions();

        /// <inheritdoc />
        public IToken Parse(string code)
        {
            Stack<IToken> stack = new();

            for (int i = 0; i < code.Length; i++)
            {
                if (!ParsingMethods.Any(parsingMethod => parsingMethod(code, stack, ref i)))
                {
                    string s = code.Substring(i, 1);

                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        throw new Exception($"Invalid character [{(int)s[0]}:{s}]");
                    }
                }
            }

            Stack<IToken> reverseStack = new(stack);

            while (reverseStack.Count > 1)
            {
                IToken left = reverseStack.Pop();

                BinaryOperatorToken binaryOperatorToken = reverseStack.Pop() as BinaryOperatorToken;

                IToken right = reverseStack.Pop();

                binaryOperatorToken.LeftOperand = left;
                binaryOperatorToken.RightOperand = right;

                reverseStack.Push(binaryOperatorToken);
            }

            return reverseStack.Pop();
        }
    }
}
