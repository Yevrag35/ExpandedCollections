using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MG.Collections.Exceptions
{
    /// <summary>
    /// An exception thrown when an attempt to add a key to a unique list or dictionary that already exists.
    /// </summary>
    [Serializable]
    public class KeyAlreadyExistsException : FormattedException
    {
        private const string DEF_MSG = "The specified key already exists";
        private const string DEF_MSG_2 = "The specified key, '{0}', already exists";

        public object? Key { get; }

        public KeyAlreadyExistsException()
            : base(DEF_MSG)
        {
        }

        public KeyAlreadyExistsException(Exception innerException)
            : base(innerException, DEF_MSG)
        {
        }

        public KeyAlreadyExistsException(object? key)
            : base(DEF_MSG_2, key?.ToString())
        {
            this.Key = key;
        }

        public KeyAlreadyExistsException(object? key, Exception innerException)
            : base(innerException, DEF_MSG_2, key?.ToString())
        {
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
        protected KeyAlreadyExistsException(SerializationInfo info, StreamingContext context)
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

            info.AddValue(nameof(Key), this.Key?.ToString());

            base.GetObjectData(info, context);
        }
    }
}
