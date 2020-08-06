using System;
using System.Collections.Generic;
using System.Text;

namespace MG.Collections.Exceptions
{
    public class ReadOnlyException : NotSupportedException
    {
        private const string DEF_MSG = "Collection was of a fixed size.";

        public ReadOnlyException() : base() { }
        public ReadOnlyException(string message) : this(message, null) { }
        public ReadOnlyException(string message, Exception inner) : base(string.Format("{0} - {1}", DEF_MSG, message), inner) { }
    }
}
