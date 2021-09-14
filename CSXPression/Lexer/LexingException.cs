using System;

namespace CSXPression.Lexer
{
    /// <summary>
    /// To specify the kind of exception that occured during the lexing part of the parsing of the code
    /// </summary>
    public enum LexingExceptionKind
    {
        /// <summary>
        /// The current position in the code can not be read to be Tokenized
        /// </summary>
        CanNotReadNextToken,

        /// <summary>
        /// A char was expected at the current position but is not found
        /// </summary>
        ExpectButNotFound
    }

    /// <summary>
    /// This Exception is thrown when an error occured during the Lexing process
    /// </summary>
    public class LexingException : Exception
    {
        /// <summary>
        /// Initialize a new instance of the class <see cref="LexingException"/>
        /// </summary>
        /// <param name="type">The type of lexing exception</param>
        /// <param name="code"> The source code on which the exception occured</param>
        /// <param name="position">The position in the code where the exception occured</param>
        public LexingException(LexingExceptionKind type, string code, int position) : base()
        {
            Code = code;
            Position = position;
            Kind = type;
        }

        /// <summary>
        /// Initialize a new instance of the class <see cref="LexingException"/>
        /// </summary>
        /// <param name="type">The type of lexing exception</param>
        /// <param name="code"> The source code on which the exception occured</param>
        /// <param name="position">The position in the code where the exception occured</param>
        /// <param name="message">The message that describes the error</param>
        public LexingException(LexingExceptionKind type, string code, int position, string message) : base(message)
        {
            Code = code;
            Position = position;
            Kind = type;
        }

        /// <summary>
        /// Initialize a new instance of the class <see cref="LexingException"/>
        /// </summary>
        /// <param name="type">The type of lexing exception</param>
        /// <param name="code"> The source code on which the exception occured</param>
        /// <param name="position">The position in the code where the exception occured</param>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in visual Basic) if no inner exception is specified</param>
        public LexingException(LexingExceptionKind type, string code, int position, string message, Exception innerException) : base(message, innerException)
        {
            Code = code;
            Position = position;
            Kind = type;
        }

        /// <summary>
        /// The source code on which the exception occured
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// The position in the code where the exception occured
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// The kind of lexing exception
        /// </summary>
        public LexingExceptionKind Kind { get; }
    }
}
