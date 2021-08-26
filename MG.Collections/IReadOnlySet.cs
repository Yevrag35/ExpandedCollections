using System;
using System.Collections;
using System.Collections.Generic;

namespace MG.Collections
{
    /// <summary>
    /// Provides a readonly abstraction of a set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlySet<T> : IReadOnlyCollection<T>, IEnumerable<T>, IEnumerable
    {
        /// <summary>
        /// Determines if the set contains a specific item.
        /// </summary>
        /// <param name="item">The item to check if the set contains.</param>
        /// <returns>
        ///     <see langword="true"/> if found; otherwise <see langword="false"/>.
        /// </returns>
        bool Contains(T item);
        /// <summary>
        /// Determines whether the current set is a proper (strict) subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///     <see langword="true"/> if the current set is a proper subset of <paramref name="other"/>;
        ///     otherwise <see langword="false"/>.
        /// </returns>
        bool IsProperSubsetOf(IEnumerable<T> other);
        bool IsProperSupersetOf(IEnumerable<T> other);
        bool IsSubsetOf(IEnumerable<T> other);
        bool IsSupersetOf(IEnumerable<T> other);
        bool Overlaps(IEnumerable<T> other);
        bool SetEquals(IEnumerable<T> other);
    }
}
