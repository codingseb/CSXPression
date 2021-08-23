using CSXPression.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace CSXPression.Parsing
{
    public partial class Parser
    {
        protected virtual bool ParseParentheses(string expression, Stack<IToken> stack, ref int i)
        {
            string s = expression.Substring(i, 1);

            if (s.Equals(")", StringComparison.Ordinal))
            {
                throw new ParsingException($"To much ')' characters are defined in expression : [{expression}] : no corresponding '(' fund.");
            }

            if (s.Equals("(", StringComparison.Ordinal))
            {
                i++;

                List<string> expressionsBetweenParentheses = GetExpressionsBetweenParenthesesOrOtherImbricableBrackets(expression, ref i, false);

                stack.Push(new ParenthesesToken(Parse(expressionsBetweenParentheses[0])));

                return true;
            }

            return false;
        }

        protected virtual List<string> GetExpressionsBetweenParenthesesOrOtherImbricableBrackets(string expression, ref int i, bool checkSeparator, string separator = ",", string startChar = "(", string endChar = ")")
        {
            List<string> expressionsList = new();

            string s;
            string currentExpression = string.Empty;
            int bracketCount = 1;
            for (; i < expression.Length; i++)
            {
                string subExpr = expression.Substring(i);
                Match internalStringMatch = stringBeginningRegex.Match(subExpr);
                Match internalCharMatch = internalCharRegex.Match(subExpr);

                if (Functionalities.StringParsing && internalStringMatch.Success)
                {
                    string innerString = internalStringMatch.Value + GetCodeUntilEndOfString(expression.Substring(i + internalStringMatch.Length), internalStringMatch);
                    currentExpression += innerString;
                    i += innerString.Length - 1;
                }
                else if (Functionalities.CharParsing && internalCharMatch.Success)
                {
                    currentExpression += internalCharMatch.Value;
                    i += internalCharMatch.Length - 1;
                }
                else
                {
                    s = expression.Substring(i, 1);

                    if (s.Equals(startChar))
                    {
                        bracketCount++;
                    }
                    else if (s.Equals("("))
                    {
                        i++;
                        currentExpression += "(" + GetExpressionsBetweenParenthesesOrOtherImbricableBrackets(expression, ref i, false, ",", "(", ")").SingleOrDefault() + ")";
                        continue;
                    }
                    else if (s.Equals("{"))
                    {
                        i++;
                        currentExpression += "{" + GetExpressionsBetweenParenthesesOrOtherImbricableBrackets(expression, ref i, false, ",", "{", "}").SingleOrDefault() + "}";
                        continue;
                    }
                    else if (s.Equals("["))
                    {
                        i++;
                        currentExpression += "[" + GetExpressionsBetweenParenthesesOrOtherImbricableBrackets(expression, ref i, false, ",", "[", "]").SingleOrDefault() + "]";
                        continue;
                    }

                    if (s.Equals(endChar))
                    {
                        bracketCount--;
                        if (bracketCount == 0)
                        {
                            if (!currentExpression.Trim().Equals(string.Empty))
                                expressionsList.Add(currentExpression);
                            break;
                        }
                    }

                    if (checkSeparator && s.Equals(separator) && bracketCount == 1)
                    {
                        expressionsList.Add(currentExpression);
                        currentExpression = string.Empty;
                    }
                    else
                    {
                        currentExpression += s;
                    }
                }
            }

            if (bracketCount > 0)
            {
                string beVerb = bracketCount == 1 ? "is" : "are";
                throw new ParsingException($"{bracketCount} '{endChar}' character {beVerb} missing in expression : [{expression}]");
            }

            return expressionsList;
        }
    }
}
