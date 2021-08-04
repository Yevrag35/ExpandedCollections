using System;
using System.Collections.Generic;
using System.Text;

namespace MG.Collections.Exceptions
{
    public class ItemNotUniqueException : FormattedException
    {
        private const string DEF_MSG = "The specified item to be added is not unique";

        public object OffendingItem { get; }

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
    }
}
