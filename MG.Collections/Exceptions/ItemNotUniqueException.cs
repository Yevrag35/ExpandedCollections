using System;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace MG.Collections.Exceptions
{
    [Serializable]
    public class ItemNotUniqueException : FormattedException
    {
        private const string DEF_MSG = "The specified item to be added is not unique";

        public object? OffendingItem { get; }

        public ItemNotUniqueException()
            : base(DEF_MSG)
        {
        }

        public ItemNotUniqueException(object offendingItem)
            : base(DEF_MSG)
        {
            this.OffendingItem = offendingItem;
        }

        public ItemNotUniqueException(object offendingItem, string message)
            : base(WithMessage, DEF_MSG, message)
        {
            this.OffendingItem = offendingItem;
        }

        protected ItemNotUniqueException(SerializationInfo info, StreamingContext context)
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

            info.AddValue(nameof(OffendingItem), this.OffendingItem?.ToString());

            base.GetObjectData(info, context);
        }
    }
}
