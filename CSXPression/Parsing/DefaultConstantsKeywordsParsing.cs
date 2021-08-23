using CSXPression.Tokens;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CSXPression.Parsing
{
    public partial class Parser
    {
        protected static readonly Regex defaultConstantsRegex = new(@"^[\p{L}_](?>[\p{L}_0-9]*)", RegexOptions.Compiled);

        protected virtual IDictionary<string, object> DefaultConstants { get; set; } = new Dictionary<string, object>()
        {
            { "Pi", Math.PI },
            { "E", Math.E },
            { "null", null},
            { "true", true },
            { "false", false },
            // TODO : Manage this keyword
            //{ "this", null }
        };

        public bool ParseDefaultConstants(string expression, Stack<IToken> stack, ref int i)
        {
            Match constantsMatch = defaultConstantsRegex.Match(expression.Substring(i));

            Dictionary<string, object> constantsValuesMapping = new(DefaultConstants, Options.IgnoreCase ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

            if (constantsMatch.Success
                && constantsValuesMapping.ContainsKey(constantsMatch.Value))
            {
                i += constantsMatch.Length - 1;
                stack.Push(new ConstantToken(constantsValuesMapping[constantsMatch.Value]));
                return true;
            }

            return false;
        }
    }
}
