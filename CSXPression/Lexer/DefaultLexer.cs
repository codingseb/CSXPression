﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CSXPression.Lexer
{
    /// <summary>
    /// The base of the Lexer implementation
    /// </summary>
    public class DefaultLexer : ILexer
    {
        /// <inheritdoc/>
        public virtual CodeScanner Scanner { get; set; }

        /// <summary>
        /// Current Position
        /// </summary>
        public int Position => Scanner.Position;

        /// <inheritdoc/>
        public virtual Token Peek()
        {
            Scanner.PushPosition();
            Token token = Read();
            Scanner.PopPosition();
            return token;
        }

        /// <inheritdoc />
        public virtual Token Read()
        {
            if (Scanner.IsEndOfCode)
                return new Token();

            return new Token();

            throw new LexingException(LexingExceptionKind.CanNotReadNextToken, Scanner.Code, Position, $"An error occured during parsing the code at {Position}, the next char [{Scanner.Peek()}] can not be tokenized");
        }

        protected virtual char Accept() => Scanner.Read().Value;

        protected virtual bool IsNext(Predicate<char> predicate)
        {
            char? next = Scanner.Peek();
            return next.HasValue && predicate(next.Value);
        }

        protected virtual void SkipWhiteSpaces()
        {
            while (IsNext(char.IsWhiteSpace))
                Accept();
        }
    }
}
