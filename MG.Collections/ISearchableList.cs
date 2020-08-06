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
        /// <summary>
        ///     Determines whether the <see cref="ISearchableList{T}"/> contains elements that
        ///     match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Func{T, TResult}"/> delegate that defines the conditions of the 
        ///     elements to search for.
        /// </param>
        /// <returns>
        /// <see langword="true"/>:
        ///     if the <see cref="ISearchableList{T}"/> contains one or more elements that
        ///     <paramref name="match"/> defined.
        /// <see langword="false"/>:
        ///     otherwise.
        /// </returns>
        bool Exists(Func<T, bool> match);
        T Find(Func<T, bool> match);
        IList<T> FindAll(Func<T, bool> match);
        int FindIndex(Func<T, bool> match);
        int FindIndex(int startIndex, Func<T, bool> match);
        int FindIndex(int startIndex, int count, Func<T, bool> match);
        T FindLast(Func<T, bool> match);
        int FindLastIndex(Func<T, bool> match);
        int FindLastIndex(int startIndex, Func<T, bool> match);
        int FindLastIndex(int startIndex, int count, Func<T, bool> match);
        IList<T> GetRange(int index, int count);
        int IndexOf(T item, int index);
        int IndexOf(T item, int index, int count);
        bool TrueForAll(Func<T, bool> match);
    }
}
