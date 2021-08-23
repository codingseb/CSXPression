using System;
using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// This token is an unary token that cast to the specified type it's operand
    /// </summary>
    public class CastToken : IUnaryOperatorToken
    {
        public CastToken(Type destinationType)
        {
            DestinationType = destinationType;
        }

        public CastToken() {}

        /// <summary>
        /// The destination type to which cast the operand
        /// </summary>
        public Type DestinationType { get; set; }

        /// <inheritdoc/>
        public IToken Operand { get; set; }

        /// <inheritdoc/>
        public string PrecedenceId => "Unary";

        /// <inheritdoc/>
        public Expression GetExpression(ExpressionEvaluator evaluator)
        {
            return Expression.Convert(Operand.GetExpression(evaluator), DestinationType);
        }
    }
}
