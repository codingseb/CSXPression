using CSXPression.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSXPression.Parsing
{
    /// <summary>
    /// A customizable parser that can create the tokens tree with a code string by the defined syntax.
    /// By default try to be as near of C# syntax as possible.
    /// </summary>
    public partial class Parser : IParser
    {
        /// <summary>
        /// This delegate describe the signature that must follow a parsing method
        /// </summary>
        /// <param name="expression">The current expression code that is parsed</param>
        /// <param name="stack">The stack of token to build that will correspond to the expression</param>
        /// <param name="i">The current position of the parsing in the expression</param>
        /// <returns><c>true</c> if the parsing method was able to parse the given position. <c>false</c> otherwise</returns>
        public delegate bool ParsingMethodDelegate(string expression, Stack<IToken> stack, ref int i);

        private IList<ParsingMethodDelegate> parsingMethods;

        /// <summary>
        /// The list of methods called to parse a <see cref="string"/> expression and convert it to a token stack.
        /// All methods of the list must follow the <see cref="ParsingMethodDelegate"/> signature.
        /// </summary>
        public virtual IList<ParsingMethodDelegate> ParsingMethods => parsingMethods ??= new List<ParsingMethodDelegate>()
        {
            ParseNumber,
            ParseOperators,
            ParseChar,
            ParseParentheses,
            ParseString,
        };

        /// <summary>
        /// Options of the parser to customize the way the parsing is done.
        /// </summary>
        public ParserOptions Options { get; set; } = new ParserOptions();

        /// <summary>
        /// Allow to enable or disable some parts/functionalities of the parsing process
        /// </summary>
        public ParserFunctionalities Functionalities { get; set; } = new ParserFunctionalities();

        /// <inheritdoc />
        public virtual IToken Parse(string code)
        {
            Stack<IToken> stack = new();

            for (int i = 0; i < code.Length; i++)
            {
                if (!ParsingMethods.Any(parsingMethod => parsingMethod(code, stack, ref i)))
                {
                    string s = code.Substring(i, 1);

                    if (!string.IsNullOrWhiteSpace(s))
                    {
                        throw new ParsingException($"Invalid character [{(int)s[0]}:{s}]");
                    }
                }
            }

            return ProcessStack(stack);
        }
    }
}
