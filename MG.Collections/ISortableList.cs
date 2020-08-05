using System;
using System.Collections.Generic;
using System.Text;

namespace MG.Collections
{
    /// <summary>
    /// Represents a collection that can reversed and sorted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISortableList<T> : IEnumerable<T>
    {
        void Reverse();
        void Reverse(int index, int count);
        void Sort();
        void Sort(IComparer<T> comparer);
    }
}
