using System;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable IDE0130

namespace MG.Collections
{
    /// <summary>
    /// Provides a readonly abstraction of a set.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlySet<T> : IReadOnlyCollection<T>
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
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        bool IsProperSubsetOf(IEnumerable<T> other);
        /// <summary>
        /// Determines whether the current set is a proper (strict) superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///     <see langword="true"/> if the current set is a proper superset of <paramref name="other"/>;
        ///     otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        bool IsProperSupersetOf(IEnumerable<T> other);
        /// <summary>
        /// Determines whether the current set is a proper subset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///     <see langword="true"/> if the current set is a subset of <paramref name="other"/>;
        ///     otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        bool IsSubsetOf(IEnumerable<T> other);
        /// <summary>
        /// Determines whether the current set is a superset of a specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///     <see langword="true"/> if the current set is a superset of <paramref name="other"/>;
        ///     otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        bool IsSupersetOf(IEnumerable<T> other);
        /// <summary>
        /// Determines whether the current set overlaps with the specified collection.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <returns>
        ///     <see langword="true"/> if the current set and <paramref name="other"/> share at least one
        ///     common element; otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        bool Overlaps(IEnumerable<T> other);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns>
        ///     <see langword="true"/> if the current set is equal to <paramref name="other"/>;
        ///     otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        bool SetEquals(IEnumerable<T> other);
    }
}
