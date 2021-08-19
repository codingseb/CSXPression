using ExpressionsTests.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace ExpressionsTests
{
    public class ExpCompiler
    {
        protected static Regex numberRegex = new Regex(@"^(?<sign>[+-])?([0-9][0-9_']*[0-9]|\d)(?<hasdecimal>\.?([0-9][0-9_]*[0-9]|\d)(e[+-]?([0-9][0-9_]*[0-9]|\d))?)?(?<type>ul|[fdulm])?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        protected delegate bool ParsingMethodDelegate(string expression, Stack<IToken> stack, ref int i);

        private IList<ParsingMethodDelegate> parsingMethods;
        protected virtual IList<ParsingMethodDelegate> ParsingMethods => parsingMethods ??= new List<ParsingMethodDelegate>()
        {
            ParseNumber,
            ParseOperators,
        };

        protected static readonly IDictionary<string, Func<string, CultureInfo, object>> numberSuffixToParse = new Dictionary<string, Func<string, CultureInfo, object>>(StringComparer.OrdinalIgnoreCase) // Always Case insensitive, like in C#
        {
            { "f", (number, culture) => float.Parse(number, NumberStyles.Any, culture) },
            { "d", (number, culture) => double.Parse(number, NumberStyles.Any, culture) },
            { "u", (number, culture) => uint.Parse(number, NumberStyles.Any, culture) },
            { "l", (number, culture) => long.Parse(number, NumberStyles.Any, culture) },
            { "ul", (number, culture) => ulong.Parse(number, NumberStyles.Any, culture) },
            { "m", (number, culture) => decimal.Parse(number, NumberStyles.Any, culture) }
        };

        public Func<object> Compile(string expression)
        {
            Stack<IToken> stack = new();

            for(int i = 0; i < expression.Length; i++)
            {
                if (!ParsingMethods.Any(parsingMethod => parsingMethod(expression, stack, ref i)))
                {
                    string s = expression.Substring(i, 1);

                    if (!s.Trim().Equals(string.Empty))
                    {
                        throw new Exception($"Invalid character [{(int)s[0]}:{s}]");
                    }
                }
            }

            Stack<IToken> reverseStack = new Stack<IToken>(stack);

            while (reverseStack.Count > 1)
            {
                IToken left = reverseStack.Pop();

                BinaryOperatorToken binaryOperatorToken = reverseStack.Pop() as BinaryOperatorToken;

                IToken right = reverseStack.Pop();

                binaryOperatorToken.LeftOperand = left;
                binaryOperatorToken.RightOperand = right;

                reverseStack.Push(binaryOperatorToken);
            }

            var lambdaExpression = Expression.Lambda<Func<object>>(Expression.Convert(reverseStack.Pop().GetExpression(), typeof(object)));

            return lambdaExpression.Compile();
        }

        protected virtual bool ParseNumber(string expression, Stack<IToken> stack, ref int i)
        {
            string restOfExpression = expression.Substring(i);
            Match numberMatch = numberRegex.Match(restOfExpression);

            if (numberMatch.Success
                && (!numberMatch.Groups["sign"].Success
                || stack.Count == 0
                || stack.Peek() is BinaryOperatorToken))
            {
                i += numberMatch.Length;
                i--;

                object numberValue = null;

                if (numberMatch.Groups["type"].Success)
                {

                    string type = numberMatch.Groups["type"].Value;
                    string numberNoType = numberMatch.Value.Replace(type, string.Empty).Replace("_", "");

                    if (numberSuffixToParse.TryGetValue(type, out Func<string, CultureInfo, object> parseFunc))
                    {
                        numberValue = parseFunc(numberNoType, CultureInfo.InvariantCulture);
                    }
                }
                else if (numberMatch.Groups["hasdecimal"].Success)
                {
                    numberValue = double.Parse(numberMatch.Value.Replace("_", ""), NumberStyles.Any, CultureInfo.InvariantCulture);
                }
                else
                {
                    numberValue = int.Parse(numberMatch.Value.Replace("_", ""), NumberStyles.Any, CultureInfo.InvariantCulture);
                }

                stack.Push(new NumberToken(numberValue, numberValue.GetType()));

                return true;
            }
            else
            {
                return false;
            }
        }

        protected virtual bool ParseOperators(string expression, Stack<IToken> stack, ref int i)
        {
            string subExpression = expression.Substring(i, 1);

            if(subExpression.Equals("+"))
            {
                stack.Push(new BinaryOperatorToken(ExpressionType.Add));
                return true;
            }
            else if(subExpression.Equals("-"))
            {
                stack.Push(new BinaryOperatorToken(ExpressionType.Subtract));
                return true;
            }

            return false;
        }
    }
}
