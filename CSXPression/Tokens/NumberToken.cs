using System;
using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// This token represent a number
    /// </summary>
    public class NumberToken : IToken
    {
        public NumberToken(object value, Type numberType)
        {
            Value = value;
            NumberType = numberType;
        }

        public Type NumberType { get; }
        public object Value { get; }

        public Expression GetExpression()
        {
            return Expression.Constant(Value, NumberType);
        }

        public override string ToString()
        {
            return $"Type = {GetType().FullName}, Value = {Value}, NumberType = {NumberType}";
        }
    }
}
