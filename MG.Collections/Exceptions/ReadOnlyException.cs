﻿using System;

namespace MG.Collections.Exceptions
{
    /// <summary>
    /// The exception that is thrown indicating that a modifying operation was attempted against a 
    /// read-only collection.
    /// </summary>
    public class ReadOnlyException : InvalidOperationException
    {
        private const string DEF_MSG = "Collection was of a fixed size.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyException"/> class.
        /// </summary>
        public ReadOnlyException() : base(DEF_MSG) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ReadOnlyException(string message) : this(message, null) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyException"/> class with
        /// a specified error message and a reference to the inner exception that is the 
        /// cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ReadOnlyException(string message, Exception inner) : 
            base(string.Format("{0} - {1}", DEF_MSG, message), inner)
        {
        }
    }
}
