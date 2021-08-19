using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// This interface is the basic code to implement to be a token
    /// </summary>
    public interface IToken
    {
        Expression GetExpression(Evaluator evaluator);
    }
}
