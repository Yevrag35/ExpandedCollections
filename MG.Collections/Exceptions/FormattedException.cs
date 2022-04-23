using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace MG.Collections.Exceptions
{
    /// <summary>
    /// An <see langword="abstract"/> exception class that provides a formatted base for message with arguments.
    /// </summary>
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

        [Obsolete]
        [return: MaybeNull]
        public static T NewFormat<T>(string message, params object[] arguments)
            where T : Exception
        {
            return Activator.CreateInstance(typeof(T), string.Format(message, arguments)) as T;
        }
    }
}
