using CSXPression.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace CSXPression.Parsing
{
    public partial class Parser
    {
        /// <summary>
        /// The list of all <see cref="IOperatorToken.PrecedenceId"/> in order of operators precedence.
        /// Used in <see cref="ProcessStack(Stack{IToken})"/> to build the tokens tree the right way/>
        /// </summary>
        // Based on https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/
        public virtual List<string> OperatorsPrecedence { get; set; } = new List<string>()
        {
            nameof(ExpressionType.UnaryPlus),
            nameof(ExpressionType.Negate),
            nameof(ExpressionType.Not),
            nameof(ExpressionType.OnesComplement),
            nameof(ExpressionType.Convert),
            nameof(ExpressionType.Multiply),
            nameof(ExpressionType.Divide),
            nameof(ExpressionType.Modulo),
            nameof(ExpressionType.Add),
            nameof(ExpressionType.Subtract),
            nameof(ExpressionType.LeftShift),
            nameof(ExpressionType.RightShift),
            nameof(ExpressionType.LessThan),
            nameof(ExpressionType.GreaterThan),
            nameof(ExpressionType.LessThanOrEqual),
            nameof(ExpressionType.GreaterThanOrEqual),
            nameof(ExpressionType.TypeIs),
            nameof(ExpressionType.Equal),
            nameof(ExpressionType.NotEqual),
            nameof(ExpressionType.And),
            nameof(ExpressionType.ExclusiveOr),
            nameof(ExpressionType.Or),
            nameof(ExpressionType.AndAlso),
            nameof(ExpressionType.OrElse),
            nameof(ExpressionType.Coalesce),
        };

        /// <summary>
        /// This method is used to build a tokens tree from a flat tokens stack
        /// </summary>
        /// <param name="stack">The stack of tokens prebuild in the parsing process</param>
        /// <returns>The root token of the tokens tree</returns>
        public virtual IToken ProcessStack(Stack<IToken> stack)
        {
            // TODO Better exception
            if (stack.Count == 0)
            {
                throw new Exception("Empty expression or no token found");
            }

            List<IToken> tokensList = stack.Reverse().ToList();

            OperatorsPrecedence.ForEach(precedenceId =>
            {
                for (int i = tokensList.Count - 1; i >= 0; i--)
                {
                    if (tokensList[i] is IOperatorToken operatorToken
                        && operatorToken.PrecedenceId.Equals(precedenceId, StringComparison.Ordinal))
                    {
                        IToken rightToken = tokensList[i + 1];
                        tokensList.RemoveAt(i + 1);

                        if (operatorToken is IBinaryOperatorToken binaryOperatorToken)
                        {
                            binaryOperatorToken.LeftOperand = tokensList[i - 1];
                            tokensList.RemoveAt(i - 1);
                            i--;
                            binaryOperatorToken.RightOperand = rightToken;
                        }
                        else if (operatorToken is IUnaryOperatorToken unaryOperatorToken)
                        {
                            unaryOperatorToken.Operand = rightToken;
                        }
                    }
                }
            });

            return tokensList[0];
        }
    }
}
