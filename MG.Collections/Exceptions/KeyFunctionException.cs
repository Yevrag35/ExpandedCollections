using System;

namespace MG.Collections.Exceptions
{
    public class KeyFunctionException : FormattedException
    {
        private const string BASE_MSG = "The function to retrieve the key threw an exception";

        public object Function { get; }

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
        public KeyFunctionException(object function)
            : this()
        {
            this.Function = function;
        }
        public KeyFunctionException(Exception innerException, object function)
            : this(innerException)
        {
            this.Function = function;
        }
        public KeyFunctionException(object function, string innerExceptionMessage)
            : this(innerExceptionMessage)
        {
            this.Function = function;
        }

        #endregion
    }
}
