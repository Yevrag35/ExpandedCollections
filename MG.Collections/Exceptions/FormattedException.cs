using System;
using System.Globalization;

namespace MG.Collections.Exceptions
{
    public abstract class FormattedException : Exception
    {
        protected const string WithMessage = "{0}: {1}";

        public FormattedException(string message, params object[] arguments)
            : base(string.Format(CultureInfo.CurrentCulture, message, arguments))
        {
        }

        public FormattedException(Exception innerException, string message, params object[] arguments)
            : base(string.Format(CultureInfo.CurrentCulture, message, arguments), innerException)
        {
        }
    }
}
