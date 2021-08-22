namespace CSXPression.Tokens
{
    /// <summary>
    /// This interace represent an operator with only one operand
    /// </summary>
    public interface IUnaryOperatorToken
    {
        /// <summary>
        /// The single operand of the operator
        /// </summary>
        IToken Operand { get; set; }
    }
}
