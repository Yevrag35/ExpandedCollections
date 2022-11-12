using System;
using System.Collections.Generic;

namespace MG.Collections.Events
{
    public delegate void SortedEventHandler<T>(object sender, SortedEventArgs<T> e);
    public class SortedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// The comparer used to sort the list.
        /// </summary>
        public IComparer<T> Comparer { get; }

        public int Index { get; } = -1;
        public int Count { get; } = -1;

        public SortedEventArgs()
            : base()
        {
        }

        public SortedEventArgs(IComparer<T> comparer)
        {
            this.Comparer = comparer;
        }

        public SortedEventArgs(int index, int count, IComparer<T> comparer)
            : this(comparer)
        {
            this.Index = index;
            this.Count = count;
        }
    }
}
