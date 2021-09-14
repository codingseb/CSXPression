namespace CSXPression.Lexer
{
    /// <summary>
    /// Interface to implement by a lexer to allow XSXPression to use it
    /// </summary>
    public interface ILexer
    {
        /// <summary>
        /// The <see cref="CodeScanner"/> to use for lexing the given code.
        /// </summary>
        CodeScanner Scanner { get; set; }

        /// <summary>
        /// Get the next <see cref="Token"/ in the code. Without moving the position
        /// </summary>
        /// <returns>The next <see cref="Token"/></returns>
        Token Peek();

        /// <summary>
        /// Read the next <see cref="Token"/ in the code. And set the position at the start of the next token.
        /// </summary>
        /// <returns>The read <see cref="Token"/></returns>
        Token Read();
    }
}
