using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// This token represent a constant like a number, a string or other inline
    /// </summary>
    public class ConstantToken : IToken
    {
        public ConstantToken(object value)
        {
            Value = value;
        }

        public object Value { get; }

        public Expression GetExpression(ExpressionEvaluator evaluator)
        {
            return Expression.Constant(Value);
        }

        public override string ToString()
        {
            return $"Type = {GetType()}, Value = {Value}, NumberType = {Value?.GetType()}";
        }
    }
}
