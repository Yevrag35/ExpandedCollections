using System.Collections.Generic;

#pragma warning disable IDE0130

namespace MG.Collections
{
    /// <summary>
    /// Represents a read-only indexable collection whose contents don't change but can still be reversed and sorted.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlySortableList<T> : IReadOnlyList<T>
    {
        /// <summary>
        /// Reverses the order of the elements in the entire <see cref="IReadOnlySortableList{T}"/>.
        /// </summary>
        void Reverse();
        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        void Reverse(int index, int count);
        /// <summary>
        /// Sorts the elements in the entire <see cref="IReadOnlySortableList{T}"/> using the default comparer.
        /// </summary>
        void Sort();
        /// <summary>
        /// Sorts the elements in the entire <see cref="IReadOnlySortableList{T}"/> using the specified comparer.
        /// </summary>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <see langword="null"/> to 
        ///     use the default comparer <see cref="Comparer{T}.Default"/>.
        /// </param>
        void Sort(IComparer<T> comparer);
        /// <summary>
        /// Sorts the elements in the range of elements in <see cref="IReadOnlySortableList{T}"/> using the specified comparer.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements, or null to use the default comparer
        ///     <see cref="Comparer{T}.Default"/>.
        /// </param>
        void Sort(int index, int count, IComparer<T> comparer);
    }
}
