using System.Collections.Generic;

namespace CSXPression.Lexer
{
    /// <summary>
    /// This class wrap a string code expression or script, allow to read it step by step and keep track of the current position
    /// </summary>
    public class CodeScanner
    {
        protected Stack<int> positionStack = new();

        /// <summary>
        /// The scanned code
        /// </summary>
        public virtual string Code { get; }

        /// <summary>
        /// The current position in the code
        /// </summary>
        public virtual int Position { get; private set; }

        /// <summary>
        /// Indicate if the <see cref="Position"/> is at the end of the code.
        /// </summary>
        public virtual bool IsEndOfCode => Position >= Code.Length;

        /// <summary>
        /// Constructor of the code scanner
        /// </summary>
        /// <param name="code">The code to scan</param>
        public CodeScanner(string code) => Code = code;

        /// <summary>
        /// Read the char at the current <see cref="Position"/> and set the <see cref="Position"/> to the next char
        /// </summary>
        /// <returns>Return the read char or null if the position is at the end of the <see cref="Code"/></returns>
        public virtual char? Read() => IsEndOfCode ? null :Code[Position++];

        /// <summary>
        /// Get the char at the current <see cref="Position"/> but without moving the <see cref="Position"/>
        /// </summary>
        /// <returns>The char at the current <see cref="Position"or null if the position is at the end of the <see cref="Code"/></returns>
        public virtual char? Peek() => IsEndOfCode ? null: Code[Position];

        /// <summary>
        /// Save the current <see cref="Position"/>
        /// </summary>
        public virtual void PushPosition() => positionStack.Push(Position);

        /// <summary>
        /// Restore the last <see cref="Position"/>
        /// </summary>
        public virtual void PopPosition() => Position = positionStack.Pop();
    }
}
