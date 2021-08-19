using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// This token represent a operator with 2 operand
    /// </summary>
    public class BinaryOperatorToken : IToken
    {
        public BinaryOperatorToken(ExpressionType expressionType)
        {
            ExpressionType = expressionType;
        }

        public IToken LeftOperand { get; set; }
        public IToken RightOperand { get; set; }

        public ExpressionType ExpressionType { get; }

        public virtual Expression GetExpression(Evaluator evaluator)
        {
            return Expression.MakeBinary(ExpressionType, LeftOperand.GetExpression(evaluator), RightOperand.GetExpression(evaluator));
        }

        public override string ToString()
        {
            return $"Type = {GetType().FullName}, ExpressionType = {ExpressionType}";
        }
    }
}
