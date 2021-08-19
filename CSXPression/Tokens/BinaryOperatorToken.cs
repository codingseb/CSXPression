using System;
using System.Linq.Expressions;

namespace ExpressionsTests.Tokens
{
    /// <summary>
    /// This token reresent a operator with 2 operand
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

        public virtual Expression GetExpression()
        {
            return Expression.MakeBinary(ExpressionType, LeftOperand.GetExpression(), RightOperand.GetExpression());
        }

        public override string ToString()
        {
            return $"Type = {GetType().FullName}, ExpressionType = {ExpressionType}";
        }
    }
}
