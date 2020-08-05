using System;
using System.Collections.Generic;

namespace MG.Collections
{
    /// <summary>
    /// Represents a collection that can be searched through and exposes advanced searching methods
    /// to accommodate this.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISearchableList<T> : IReadOnlyCollection<T>
    {
        bool Exists(Predicate<T> match);
        T Find(Predicate<T> match);
        IList<T> FindAll(Predicate<T> match);
        int FindIndex(Predicate<T> match);
        int FindIndex(int startIndex, Predicate<T> match);
        int FindIndex(int startIndex, int count, Predicate<T> match);
        T FindLast(Predicate<T> match);
        int FindLastIndex(Predicate<T> match);
        int FindLastIndex(int startIndex, Predicate<T> match);
        int FindLastIndex(int startIndex, int count, Predicate<T> match);
        IList<T> GetRange(int index, int count);
        int IndexOf(T item, int index);
        int IndexOf(T item, int index, int count);
        bool TrueForAll(Predicate<T> match);
    }
}
