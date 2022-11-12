using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MG.Collections.Exceptions
{
    /// <summary>
    /// An exception class that occurs when a function delegate encounters an exception.
    /// </summary>
    [Serializable]
    public class KeyFunctionException : FormattedException
    {
        private const string BASE_MSG = "The function to retrieve the key threw an exception";

        /// <summary>
        /// The function that caused the exception.
        /// </summary>
        public virtual object? Function { get; }

        #region CONSTRUCTORS
        public KeyFunctionException()
            : base(BASE_MSG)
        {
        }
        public KeyFunctionException(string innerExceptionMessage)
            : base(WithMessage, BASE_MSG, innerExceptionMessage)
        {
        }
        public KeyFunctionException(Exception innerException)
            : base(innerException, WithMessage, BASE_MSG, innerException.Message)
        {
        }
        public KeyFunctionException(object? function)
            : this()
        {
            this.Function = function;
        }
        public KeyFunctionException(object? function, Exception innerException)
            : this(innerException)
        {
            this.Function = function;
        }
        public KeyFunctionException(object function, string innerExceptionMessage)
            : this(innerExceptionMessage)
        {
            this.Function = function;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected KeyFunctionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        #endregion

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        public sealed override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (null == info)
            {
                throw new ArgumentNullException(nameof(info));
            }

            info.AddValue("FunctionAsString", this.Function?.ToString());
            object? serialized = this.GetSerializedFunction();
            if (null != serialized)
            {
                info.AddValue(nameof(Function), serialized);
            }

            base.GetObjectData(info, context);
        }

        /// <summary>
        /// Overridable method that can return a serialized version of the function used
        /// when the <see cref="KeyFunctionException"/> occurred.
        /// </summary>
        /// <returns>
        ///     Without overriding, this method returns <see langword="null"/>; otherwise, 
        ///     the serializable object representing the function used.
        /// </returns>
        protected virtual object? GetSerializedFunction()
        {
            return null;
        }
    }
}
