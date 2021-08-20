using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// Interface for objects that must implement a ExpressionType property
    /// </summary>
    public interface IExpressionTypeToken
    {
        public ExpressionType ExpressionType { get; }
    }
}
