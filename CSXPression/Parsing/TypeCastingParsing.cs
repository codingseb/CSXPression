using CSXPression.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace CSXPression.Parsing
{
    public partial class Parser
    {
        protected virtual Regex CastRegex => new(@"^\((?>\s*)(?<typeName>[\p{L}_][\p{L}_0-9\.\[\]<>]*[?]?)(?>\s*)\)", (Options.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None) | RegexOptions.Compiled);

        protected virtual bool ParseCast(string expression, Stack<IToken> stack, ref int i)
        {
            Match castMatch = CastRegex.Match(expression.Substring(i));

            if (castMatch.Success)
            {
                string typeName = castMatch.Groups["typeName"].Value;

                int typeIndex = 0;
                Type type = EvaluateType(typeName, ref typeIndex);

                if (type != null)
                {
                    i += castMatch.Length - 1;
                    stack.Push(new CastToken(type));
                    return true;
                }
            }

            return false;
        }
    }
}
