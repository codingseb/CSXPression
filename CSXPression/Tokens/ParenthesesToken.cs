using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// -- Describe here to what is this class used for. (What is it's purpose) --
    /// </summary>
    public class ParenthesesToken : IToken
    {
        public ParenthesesToken(IToken subToken)
        {
            SubToken = subToken;
        }

        /// <summary>
        /// The root token of the tokens tree in parenthese
        /// </summary>
        public IToken SubToken { get; set; }

        /// <inheritdoc/>
        public Expression GetExpression(ExpressionEvaluator evaluator)
        {
            return SubToken.GetExpression(evaluator);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Type = {GetType()}, SubToken = ({SubToken})";
        }
    }
}
