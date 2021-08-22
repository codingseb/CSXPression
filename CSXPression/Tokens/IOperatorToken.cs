namespace CSXPression.Tokens
{
    /// <summary>
    /// An interface that represent a operator token
    /// </summary>
    public interface IOperatorToken
    {
        /// <summary>
        /// A string that identify the operator for operators priorities management
        /// </summary>
        string PrecedenceId { get; }
    }
}
