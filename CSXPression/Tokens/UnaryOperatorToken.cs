using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// This token represent a operator with 1 operand
    /// </summary>
    public class UnaryOperatorToken : IToken, IExpressionTypeToken
    {
        public UnaryOperatorToken(ExpressionType expressionType)
        {
            ExpressionType = expressionType;
        }

        public IToken Operand { get; set; }

        public ExpressionType ExpressionType { get; }

        public Expression GetExpression(ExpressionEvaluator evaluator)
        {
            var operandExpression = Operand.GetExpression(evaluator);
            return Expression.MakeUnary(ExpressionType, operandExpression, operandExpression.Type);
        }
        
        public override string ToString()
        {
            return $"Type = {GetType()}, ExpressionType = {ExpressionType}";
        }
    }
}
