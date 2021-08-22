using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// This interface is the basic code to implement to be a token
    /// </summary>
    public interface IToken
    {
        /// <summary>
        /// To get recursively the <see cref="Expression"/> of the current token.
        /// </summary>
        /// <param name="evaluator">The evaluator used to build this expression</param>
        /// <returns>The <see cref="Expression"/> of the current token</returns>
        Expression GetExpression(ExpressionEvaluator evaluator);
    }
}
