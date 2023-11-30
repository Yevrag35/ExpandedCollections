using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MG.Collections.Exceptions
{
    /// <summary>
    /// An exception class that provides a formatted base for message with arguments.
    /// </summary>
    [Serializable]
    public abstract class FormattedException : Exception
    {
        protected static readonly string WithMessage = "{0}: {1}";

        protected FormattedException(string message, params object?[] arguments)
            : base(string.Format(CultureInfo.CurrentCulture, message, arguments))
        {
        }

        protected FormattedException(Exception? innerException, string message, params object?[] arguments)
            : base(string.Format(CultureInfo.CurrentCulture, message, arguments), innerException)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected FormattedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (null == info)
            {
                throw new ArgumentNullException(nameof(info));
            }

            base.GetObjectData(info, context);
        }
    }
}
