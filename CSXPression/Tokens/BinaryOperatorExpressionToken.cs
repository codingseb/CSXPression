using CSXPression.Utils;
using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// This token represent a operator with 2 operands
    /// </summary>
    public class BinaryOperatorExpressionToken : IBinaryOperatorToken
    {
        public BinaryOperatorExpressionToken(ExpressionType expressionType)
        {
            ExpressionType = expressionType;
        }

        /// <inheritdoc/>
        public IToken LeftOperand { get; set; }

        /// <inheritdoc/>
        public IToken RightOperand { get; set; }

        public ExpressionType ExpressionType { get; set; }

        ///<inheritdoc/>
        public string PrecedenceId => ExpressionType.ToString();

        /// <inheritdoc/>
        public virtual Expression GetExpression(ExpressionEvaluator evaluator)
        {
            Expression left = LeftOperand.GetExpression(evaluator);
            Expression right = RightOperand.GetExpression(evaluator);

            if ((left.Type == typeof(string) || right.Type == typeof(string)) && ExpressionType == ExpressionType.Add)
            {
                return Expression.Call(typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(object), typeof(object) }),
                    Expression.Convert(left, typeof(object)),
                    Expression.Convert(right, typeof(object)));
            }
            else
            {
                if (left.Type != right.Type)
                {
                    if (left.Type.IsImplicitlyCastableTo(right.Type))
                    {
                        left = Expression.Convert(left, right.Type);
                    }
                    else if (right.Type.IsImplicitlyCastableTo(left.Type))
                    {
                        right = Expression.Convert(right, left.Type);
                    }
                }

                return Expression.MakeBinary(ExpressionType, left, right);
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Type = {GetType()}, ExpressionType = {ExpressionType}";
        }
    }
}
