using CSXPression.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace CSXPression.Parsing
{
    public partial class Parser
    {
        protected IDictionary<string, ExpressionType> BinaryOperatorsDictionary => new Dictionary<string, ExpressionType>(Options.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal)
        {
            { "+", ExpressionType.Add },
            { "-", ExpressionType.Subtract },
            { "*", ExpressionType.Multiply },
            { "/", ExpressionType.Divide },
            { "%", ExpressionType.Modulo },
            { "<", ExpressionType.LessThan },
            { ">", ExpressionType.GreaterThan },
            { "<=", ExpressionType.LessThanOrEqual },
            { ">=", ExpressionType.GreaterThanOrEqual },
            { "is", ExpressionType.TypeIs },
            { "==", ExpressionType.Equal },
            { "!=", ExpressionType.NotEqual },
            { "&&", ExpressionType.AndAlso },
            { "||", ExpressionType.OrElse },
            { "&", ExpressionType.And },
            { "|", ExpressionType.Or },
            { "^", ExpressionType.ExclusiveOr },
            { "<<", ExpressionType.LeftShift },
            { ">>", ExpressionType.RightShift },
            { "??", ExpressionType.Coalesce },
        };

        protected IDictionary<string, ExpressionType> UnaryOperatorsDictionary => new Dictionary<string, ExpressionType>(Options.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal)
        {
            { "+", ExpressionType.UnaryPlus },
            { "-", ExpressionType.Negate },
            { "!", ExpressionType.Not },
            { "~", ExpressionType.Not },
        };
        protected virtual bool ParseOperators(string expression, Stack<IToken> stack, ref int i)
        {
            string regexPattern = "^(" + string.Join("|",
                BinaryOperatorsDictionary.Keys
                    .Concat(UnaryOperatorsDictionary.Keys)
                    .Distinct()
                    .OrderByDescending(key => key.Length)
                    .Select(Regex.Escape)) + ")";

            Match match = Regex.Match(expression.Substring(i), regexPattern, Options.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

            if (match.Success)
            {
                string op = match.Value;

                if (UnaryOperatorsDictionary.ContainsKey(op)
                    && (!BinaryOperatorsDictionary.ContainsKey(op)
                    || stack.Count == 0 || stack.Peek() is not BinaryOperatorToken))
                {
                    stack.Push(new UnaryOperatorToken(UnaryOperatorsDictionary[op]));
                }
                else
                {
                    stack.Push(new BinaryOperatorToken(BinaryOperatorsDictionary[op]));
                }

                i += match.Length - 1;
                return true;
            }

            return false;
        }
    }
}
