using System;
using System.Runtime.Serialization;

namespace MG.Collections.Exceptions
{
    /// <summary>
    /// An exception that occurs when the KeySelector delegate function is <see langword="null"/>.
    /// </summary>
    [Serializable]
    public sealed class KeySelectorNotSetException : ArgumentNullException
    {
        private const string MSG = "The key selector has not been set.  No key was extracted.";

        public KeySelectorNotSetException()
            : base(MSG)
        {
        }

        private KeySelectorNotSetException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
