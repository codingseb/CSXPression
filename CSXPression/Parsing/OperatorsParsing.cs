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
        /// <summary>
        /// Dictionary used to map the string operator detection to its corresponding <see cref="ExpressionType"/>
        /// to build a <see cref="BinaryOperatorExpressionToken" />.
        /// <para>
        /// Default Dictionary based on https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/
        /// and https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.expressiontype
        /// </para>
        /// </summary>
        public virtual IDictionary<string, ExpressionType> BinaryOperatorsDictionary => new Dictionary<string, ExpressionType>(Options.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal)
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

        /// <summary>
        /// Dictionary used to map the string operator detection to its corresponding <see cref="ExpressionType"/>
        /// to build a <see cref="UnaryOperatorExpressionToken" />.
        /// <para>
        /// Default Dictionary based on https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/
        /// and https://docs.microsoft.com/en-us/dotnet/api/system.linq.expressions.expressiontype
        /// </para>
        /// </summary>
        public virtual IDictionary<string, ExpressionType> UnaryOperatorsDictionary => new Dictionary<string, ExpressionType>(Options.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal)
        {
            { "+", ExpressionType.UnaryPlus },
            { "-", ExpressionType.Negate },
            { "!", ExpressionType.Not },
            { "~", ExpressionType.OnesComplement },
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
                    || stack.Count == 0 || stack.Peek() is IOperatorToken))
                {
                    stack.Push(new UnaryOperatorExpressionToken(UnaryOperatorsDictionary[op]));
                }
                else
                {
                    stack.Push(new BinaryOperatorExpressionToken(BinaryOperatorsDictionary[op]));
                }

                i += match.Length - 1;
                return true;
            }

            return false;
        }
    }
}
