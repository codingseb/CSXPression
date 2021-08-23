namespace CSXPression.Tokens
{
    /// <summary>
    /// An interface that represent a operator token
    /// </summary>
    public interface IOperatorToken : IToken
    {
        /// <summary>
        /// A string that identify the operator for operators priorities management
        /// </summary>
        string PrecedenceId { get; }
    }
}
