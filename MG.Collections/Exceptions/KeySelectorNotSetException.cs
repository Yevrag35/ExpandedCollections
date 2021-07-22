using System;

namespace MG.Collections.Exceptions
{
    public class KeySelectorNotSetException : InvalidOperationException
    {
        private const string MSG = "The key selector has not been set.  No key was extracted.";

        public KeySelectorNotSetException()
            : base(MSG)
        {
        }
    }
}
