using System;
using System.Collections.Generic;
using System.Text;

namespace MG.Collections.Exceptions
{
    public class KeySelectorSetException : InvalidOperationException
    {
        private const string MSG = "The key selector has already been set.";

        public KeySelectorSetException()
            : base(MSG)
        {
        }
    }
}
