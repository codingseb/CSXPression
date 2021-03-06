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

        public ConstantToken() {}

        /// <summary>
        /// The constant value
        /// </summary>
        public object Value { get; set; }

        /// <inheritdoc/>
        public Expression GetExpression(ExpressionEvaluator evaluator)
        {
            return Expression.Constant(Value);
        }

        public override string ToString()
        {
            return $"Type = {GetType()}, Value = {Value}, ValueType = {Value?.GetType()}";
        }
    }
}
