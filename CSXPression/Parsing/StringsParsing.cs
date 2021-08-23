using CSXPression.Tokens;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CSXPression.Parsing
{
    public partial class Parser
    {
        protected static readonly Regex stringBeginningRegex = new("^(?<interpolated>[$])?(?<escaped>[@])?[\"]", RegexOptions.Compiled);
        protected static readonly Regex endOfStringWithDollar = new("^([^\"{\\\\]|\\\\[\\\\\"0abfnrtv])*[\"{]", RegexOptions.Compiled);
        protected static readonly Regex endOfStringWithoutDollar = new("^([^\"\\\\]|\\\\[\\\\\"0abfnrtv])*[\"]", RegexOptions.Compiled);
        protected static readonly Regex endOfStringWithDollarWithAt = new("^[^\"{]*[\"{]", RegexOptions.Compiled);
        protected static readonly Regex endOfStringWithoutDollarWithAt = new("^[^\"]*[\"]", RegexOptions.Compiled);
        protected static readonly Regex endOfStringInterpolationRegex = new("^('\"'|[^}\"])*[}\"]", RegexOptions.Compiled);
        protected static readonly Regex stringBeginningForEndBlockRegex = new("[$]?[@]?[\"]$", RegexOptions.Compiled);

        protected static readonly IDictionary<char, string> stringEscapedCharDict = new Dictionary<char, string>()
        {
            { '\\', @"\" },
            { '"', "\"" },
            { '0', "\0" },
            { 'a', "\a" },
            { 'b', "\b" },
            { 'f', "\f" },
            { 'n', "\n" },
            { 'r', "\r" },
            { 't', "\t" },
            { 'v', "\v" }
        };

        protected virtual bool ParseString(string expression, Stack<IToken> stack, ref int i)
        {
            if (!Functionalities.StringParsing)
                return false;

            Match stringBeginningMatch = stringBeginningRegex.Match(expression.Substring(i));

            if (stringBeginningMatch.Success)
            {
                bool isEscaped = stringBeginningMatch.Groups["escaped"].Success;
                bool isInterpolated = stringBeginningMatch.Groups["interpolated"].Success;

                i += stringBeginningMatch.Length;

                Regex stringRegexPattern = new($"^[^{(isEscaped ? "" : @"\\")}{(isInterpolated ? "{}" : "")}\"]*");

                bool endOfString = false;

                IToken currentToken = new ConstantToken();
                string currentString = string.Empty;

                while (!endOfString && i < expression.Length)
                {
                    Match stringMatch = stringRegexPattern.Match(expression.Substring(i, expression.Length - i));

                    currentString += stringMatch.Value;
                    i += stringMatch.Length;

                    if (expression.Substring(i)[0] == '"')
                    {
                        if (expression.Substring(i).Length > 1 && expression.Substring(i)[1] == '"')
                        {
                            i += 2;
                            currentString += '"';
                        }
                        else
                        {
                            endOfString = true;

                            if (currentToken is ConstantToken constantToken)
                            {
                                constantToken.Value = currentString;
                            }
                            else if (currentToken is StringConcatToken stringConcatToken)
                            {
                                stringConcatToken.TokensToConcat.Add(new ConstantToken(currentString));
                            }

                            stack.Push(currentToken);
                        }
                    }
                    else if (expression.Substring(i)[0] == '{')
                    {
                        i++;

                        if (expression.Substring(i)[0] == '{')
                        {
                            currentString += '{';
                            i++;
                        }
                        else
                        {
                            string innerExpression = string.Empty;
                            int bracketCount = 1;
                            for (; i < expression.Length; i++)
                            {
                                if (i + 3 <= expression.Length && expression.Substring(i, 3).Equals("'\"'"))
                                {
                                    innerExpression += "'\"'";
                                    i += 2;
                                }
                                else
                                {
                                    Match internalStringMatch = stringBeginningRegex.Match(expression.Substring(i));

                                    if (internalStringMatch.Success)
                                    {
                                        string innerString = internalStringMatch.Value + GetCodeUntilEndOfString(expression.Substring(i + internalStringMatch.Length), internalStringMatch);
                                        innerExpression += innerString;
                                        i += innerString.Length - 1;
                                    }
                                    else
                                    {
                                        string s = expression.Substring(i, 1);

                                        if (s.Equals("{")) bracketCount++;

                                        if (s.Equals("}"))
                                        {
                                            bracketCount--;
                                            i++;
                                            if (bracketCount == 0) break;
                                        }

                                        innerExpression += s;
                                    }
                                }
                            }

                            if (bracketCount > 0)
                            {
                                string beVerb = bracketCount == 1 ? "is" : "are";
                                throw new ParsingException($"{bracketCount} '}}' character {beVerb} missing in expression : [{expression}]");
                            }

                            StringConcatToken stringConcatToken = currentToken as StringConcatToken ?? new();

                            if (currentToken is ConstantToken constantToken)
                            {
                                constantToken.Value = currentString;
                                stringConcatToken.TokensToConcat.Add(constantToken);
                                currentToken = stringConcatToken;
                            }
                            else
                            {
                                stringConcatToken.TokensToConcat.Add(new ConstantToken(currentString));
                            }

                            currentString = string.Empty;

                            stringConcatToken.TokensToConcat.Add(Parse(innerExpression));
                        }
                    }
                    else if (expression.Substring(i, expression.Length - i)[0] == '}')
                    {
                        i++;

                        if (expression.Substring(i, expression.Length - i)[0] == '}')
                        {
                            currentString += '}';
                            i++;
                        }
                        else
                        {
                            throw new ParsingException("A character '}' must be escaped in a interpolated string.");
                        }
                    }
                    else if (expression.Substring(i, expression.Length - i)[0] == '\\')
                    {
                        i++;

                        if (stringEscapedCharDict.TryGetValue(expression.Substring(i, expression.Length - i)[0], out string escapedString))
                        {
                            currentString += escapedString;
                            i++;
                        }
                        else
                        {
                            throw new ParsingException("There is no corresponding escaped character for \\" + expression.Substring(i, 1));
                        }
                    }
                }

                if (!endOfString)
                    throw new ParsingException("A \" character is missing.");

                return true;
            }

            return false;
        }

        protected virtual string GetCodeUntilEndOfString(string subExpr, Match stringBeginningMatch)
        {
            StringBuilder stringBuilder = new();

            GetCodeUntilEndOfString(subExpr, stringBeginningMatch, ref stringBuilder);

            return stringBuilder.ToString();
        }

        protected virtual void GetCodeUntilEndOfString(string subExpr, Match stringBeginningMatch, ref StringBuilder stringBuilder)
        {
            Match codeUntilEndOfStringMatch = stringBeginningMatch.Value.Contains("$") ?
                (stringBeginningMatch.Value.Contains("@") ? endOfStringWithDollarWithAt.Match(subExpr) : endOfStringWithDollar.Match(subExpr)) :
                (stringBeginningMatch.Value.Contains("@") ? endOfStringWithoutDollarWithAt.Match(subExpr) : endOfStringWithoutDollar.Match(subExpr));

            if (codeUntilEndOfStringMatch.Success)
            {
                if (codeUntilEndOfStringMatch.Value.EndsWith("\""))
                {
                    stringBuilder.Append(codeUntilEndOfStringMatch.Value);
                }
                else if (codeUntilEndOfStringMatch.Value.EndsWith("{") && codeUntilEndOfStringMatch.Length < subExpr.Length)
                {
                    if (subExpr[codeUntilEndOfStringMatch.Length] == '{')
                    {
                        stringBuilder.Append(codeUntilEndOfStringMatch.Value);
                        stringBuilder.Append('{');
                        GetCodeUntilEndOfString(subExpr.Substring(codeUntilEndOfStringMatch.Length + 1), stringBeginningMatch, ref stringBuilder);
                    }
                    else
                    {
                        string interpolation = GetCodeUntilEndOfStringInterpolation(subExpr.Substring(codeUntilEndOfStringMatch.Length));
                        stringBuilder.Append(codeUntilEndOfStringMatch.Value);
                        stringBuilder.Append(interpolation);
                        GetCodeUntilEndOfString(subExpr.Substring(codeUntilEndOfStringMatch.Length + interpolation.Length), stringBeginningMatch, ref stringBuilder);
                    }
                }
                else
                {
                    stringBuilder.Append(subExpr);
                }
            }
            else
            {
                stringBuilder.Append(subExpr);
            }
        }

        protected virtual string GetCodeUntilEndOfStringInterpolation(string subExpr)
        {
            Match endOfStringInterpolationMatch = endOfStringInterpolationRegex.Match(subExpr);
            string result = subExpr;

            if (endOfStringInterpolationMatch.Success)
            {
                if (endOfStringInterpolationMatch.Value.EndsWith("}"))
                {
                    result = endOfStringInterpolationMatch.Value;
                }
                else
                {
                    Match stringBeginningForEndBlockMatch = stringBeginningForEndBlockRegex.Match(endOfStringInterpolationMatch.Value);

                    string subString = GetCodeUntilEndOfString(subExpr.Substring(endOfStringInterpolationMatch.Length), stringBeginningForEndBlockMatch);

                    result = endOfStringInterpolationMatch.Value + subString
                        + GetCodeUntilEndOfStringInterpolation(subExpr.Substring(endOfStringInterpolationMatch.Length + subString.Length));
                }
            }

            return result;
        }
    }
}
