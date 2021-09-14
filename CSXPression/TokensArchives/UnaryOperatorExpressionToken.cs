using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// This token represent a expression operator with only 1 operand
    /// </summary>
    public class UnaryOperatorExpressionToken : IUnaryOperatorToken
    {
        public UnaryOperatorExpressionToken(ExpressionType expressionType)
        {
            ExpressionType = expressionType;
        }

        /// <inheritdoc/>
        public IToken Operand { get; set; }

        public ExpressionType ExpressionType { get; set; }

        ///<inheritdoc/>
        public string PrecedenceId => "Unary";

        /// <inheritdoc/>
        public Expression GetExpression(ExpressionEvaluator evaluator)
        {
            Expression operandExpression = Operand.GetExpression(evaluator);
            return Expression.MakeUnary(ExpressionType, operandExpression, operandExpression.Type);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Type = {GetType()}, ExpressionType = {ExpressionType}";
        }
    }
}
