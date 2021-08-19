using System.Linq.Expressions;

namespace ExpressionsTests.Tokens
{
    /// <summary>
    /// This interface is the basic code to implement to be a token
    /// </summary>
    public interface IToken
    {
        Expression GetExpression();
    }
}
