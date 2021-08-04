using System;

namespace MG.Collections.Exceptions
{
    public abstract class FormattedException : Exception
    {
        protected static string WithMessage = "{0}: {1}";

        public FormattedException(string message, params object[] arguments)
            : base(string.Format(message, arguments))
        {
        }

        public FormattedException(Exception innerException, string message, params object[] arguments)
            : base(string.Format(message, arguments), innerException)
        {
        }
    }
}
