using System;
using System.Runtime.Serialization;

namespace MG.Collections.Exceptions
{
    [Serializable]
    public sealed class KeySelectorSetException : ArgumentException
    {
        private const string MSG = "The key selector has already been set.";

        public KeySelectorSetException()
            : base(MSG)
        {
        }

        private KeySelectorSetException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
