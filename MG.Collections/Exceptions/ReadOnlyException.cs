using System;
using System.Runtime.Serialization;

namespace MG.Collections.Exceptions
{
    /// <summary>
    /// The exception that is thrown indicating that a modifying operation was attempted against a 
    /// read-only collection.
    /// </summary>
    [Serializable]
    public class ReadOnlyException : FormattedException
    {
        private const string DEF_MSG = "Collection was of a fixed size";

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyException"/> class.
        /// </summary>
        public ReadOnlyException() : base(DEF_MSG) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyException"/> class with
        /// a specified error message.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public ReadOnlyException(string message) : base(WithMessage, DEF_MSG, message) { }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyException"/> class with
        /// a specified error message and a reference to the inner exception that is the 
        /// cause of this exception.
        /// </summary>
        /// <param name="inner">The exception that is the cause of the current exception.</param>
        public ReadOnlyException(Exception inner)
            : base(inner, WithMessage, DEF_MSG, inner.Message)
        {
        }

        protected ReadOnlyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
