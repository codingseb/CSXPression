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
        protected IDictionary<string, ExpressionType> OperatorsDictionary => new Dictionary<string, ExpressionType>(Options.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal)
        {
            { "+", ExpressionType.Add },
            { "-", ExpressionType.Subtract },
        };

        protected virtual bool ParseOperators(string expression, Stack<IToken> stack, ref int i)
        {
            string regexPattern = "^(" + string.Join("|", OperatorsDictionary
                .Keys
                .OrderByDescending(key => key.Length)
                .Select(Regex.Escape)) + ")";

            Match match = Regex.Match(expression.Substring(i), regexPattern, Options.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

            if (match.Success)
            {
                stack.Push(new BinaryOperatorToken(OperatorsDictionary[match.Value]));
                i += match.Length - 1;
                return true;
            }

            return false;
        }
    }
}
