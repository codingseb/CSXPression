using CSXPression.Tokens;
using System;
using System.Linq.Expressions;

namespace CSXPression
{
    /// <summary>
    /// Represent a encapsulated parsed expression tree with some additionnal features
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ParsedExpression<T>
    {
        private Func<T> compiledDelegate;

        /// <summary>
        /// The code used to parse this Expression
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// The lambda expression tree of the expression
        /// </summary>
        public Expression Expression { get; }

        /// <summary>
        /// The cached compiled delegate of the expression
        /// </summary>
        public Func<T> CompiledDelegate => compiledDelegate ??= Expression.Lambda<Func<T>>(Expression).Compile();

        /// <summary>
        /// The evaluator used as environment of the expression evaluation
        /// To customize the evaluation
        /// </summary>
        public ExpressionEvaluator Evaluator { get; }

        /// <summary>
        /// Contructor of the ParsedExpression
        /// </summary>
        /// <param name="rootToken">The root token to build the expression</param>
        /// <param name="code">The code used to parse this Expression</param>
        /// <param name="evaluator">The evaluator to use as environment of the expression evaluation</param>
        internal ParsedExpression(IToken rootToken, string code, ExpressionEvaluator evaluator)
        {
            Code = code;
            Evaluator = evaluator;
            Expression = Expression.Convert(rootToken.GetExpression(Evaluator), typeof(T));
        }

        /// <summary>
        /// Evaluate the expression
        /// </summary>
        /// <returns> the result of the evaluation</returns>
        public T Evaluate()
        {
            return CompiledDelegate();
        }
    }
}
