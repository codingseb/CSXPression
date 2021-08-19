using CSXPression.Parsing;
using System;

namespace CSXPression
{
    public class ExpressionEvaluator
    {
        public IParser Parser { get; set; }

        public ExpressionEvaluator()
        {
            Parser = new Parser();
        }

        public ExpressionEvaluator(IParser parser)
        {
            Parser = parser;
        }

        public virtual ParsedExpression<T> Parse<T>(string code)
        {
            return new ParsedExpression<T>(Parser.Parse(code), code, this);
        }

        public virtual T Evaluate<T>(string code)
        {
            return Parse<T>(code).Evaluate();
        }

        public virtual object Evaluate(string code)
        {
            return Evaluate<object>(code);
        }

        public Func<T> Compile<T>(string code)
        {
            return Parse<T>(code).CompiledDelegate;
        }
    }
}
