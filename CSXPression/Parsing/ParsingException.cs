using System;

namespace CSXPression.Parsing
{
    /// <summary>
    /// This class is an specific type of exception that can be thrown during the code parsing process.
    /// </summary>
    public class ParsingException : Exception
    {
        public ParsingException() : base()
        {
        }

        public ParsingException(string message) : base(message)
        {
        }

        public ParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
