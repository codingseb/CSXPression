using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace CSXPression.Tokens
{
    /// <summary>
    /// This token is used to make a string.Concat of all sub Tokens
    /// </summary>
    public class StringConcatToken : IToken
    {
        /// <summary>
        /// The list of all sub tokens to string.Concat
        /// </summary>
        public List<IToken> TokensToConcat { get; } = new List<IToken>();

        public Expression GetExpression(ExpressionEvaluator evaluator)
        {
            return Expression.Call(typeof(string).GetMethod(nameof(string.Concat), new[] { typeof(object[]) }),
                Expression.NewArrayInit(typeof(object),
                TokensToConcat.Select(t => Expression.Convert(t.GetExpression(evaluator), typeof(object)))));
        }
    }
}
