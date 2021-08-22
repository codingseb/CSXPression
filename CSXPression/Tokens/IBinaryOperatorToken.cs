namespace CSXPression.Tokens
{
    /// <summary>
    /// This interface represent a operator token that take 2 operand (Left and right)
    /// </summary>
    public interface IBinaryOperatorToken
    {
        /// <summary>
        /// The left operand of the operator
        /// </summary>
        IToken LeftOperand { get; set; }

        /// <summary>
        /// The right operand of the operator
        /// </summary>
        IToken RightOperand { get; set; }
    }
}
