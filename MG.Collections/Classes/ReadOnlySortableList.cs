using MG.Collections.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using Strings = MG.Collections.Properties.Resources;

#pragma warning disable CA1010 // Collections should implement generic interface
#pragma warning disable CA1710 // Identifiers should have correct suffix
#pragma warning disable IDE0130

namespace MG.Collections
{
    /// <summary>
    /// A read-only list whose contents don't change but can still be reversed and sorted.
    /// </summary>
    /// <typeparam name="T">The element type of the <see cref="ReadOnlySortableList{T}"/>.</typeparam>
    public class ReadOnlySortableList<T> : ReadOnlyList<T>, IReadOnlySortableList<T>, IReadOnlyList<T>
    {
        #region PRIVATE FIELDS/CONSTANTS
        private const int DEFAULT_CAPACITY = 0;
        private IComparer<T> _sortComparer;

        #endregion

        #region PROPERTIES
        /// <summary>
        /// The default <see cref="IComparer{T}"/> implementation that the <see cref="ReadOnlySortableList{T}"/>
        /// uses to execute <see cref="Sort"/> when one is not provided.
        /// </summary>
        /// <returns>
        ///     The <see cref="IComparer{T}"/> implements the list uses to execute <see cref="Sort"/>.
        ///     If the set accessor was passed a <see langword="null"/> value, then the default comparer for type 
        ///     <typeparamref name="T"/> is set instead.
        /// </returns>
        public virtual IComparer<T> DefaultComparer
        {
            get => _sortComparer ?? GetDefaultComparer();
            set
            {
                if (null == value)
                    value = GetDefaultComparer();

                _sortComparer = value;
            }
        }

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySortableList{T}"/> class that is empty, has
        /// the default capacity, and uses the default comparer for <typeparamref name="T"/>.
        /// </summary>
        protected ReadOnlySortableList()
            : this(DEFAULT_CAPACITY)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySortableList{T}"/> class that is empty, has
        /// the specified capacity, and uses the default comparer for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="capacity">The number of new elements the list can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        protected ReadOnlySortableList(int capacity)
            : this(capacity, GetDefaultComparer())
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySortableList{T}"/> class that is empty, has the
        /// default capacity, and uses the specified comparer for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <see langword="null"/>
        ///     to use the default comparer <see cref="Comparer{T}.Default"/>.
        /// </param>
        protected ReadOnlySortableList(IComparer<T> comparer)
            : this(DEFAULT_CAPACITY, comparer)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySortableList{T}"/> class that is empty, has the
        /// specified capacity, and uses the specified comparer for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="capacity">The number of new elements the list can initially store.</param>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <see langword="null"/>
        ///     to use the default comparer <see cref="Comparer{T}.Default"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        protected ReadOnlySortableList(int capacity, IComparer<T> comparer)
            : base(capacity)
        {
            this.DefaultComparer = comparer;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySortableList{T}"/> class that wraps the specified
        /// collection to make it read-only and uses the default comparer for <typeparamref name="T"/> for sorting
        /// operations.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public ReadOnlySortableList(IEnumerable<T> collection)
            : this(collection, GetDefaultComparer())
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySortableList{T}"/> class that wraps the specified
        /// collection to make it read-only and uses the specified comparer for <typeparamref name="T"/> for sorting
        /// operations.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <see langword="null"/>
        ///     to use the default comparer <see cref="Comparer{T}.Default"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public ReadOnlySortableList(IEnumerable<T> collection, IComparer<T> comparer)
            : base(collection)
        {
            this.DefaultComparer = comparer;
        }

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Reverses the order of the elements in the entire <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        public void Reverse()
        {
            this.Reverse(0, this.Count);
        }
        /// <summary>
        /// Reverse the order of the elements starting the specified index and extends to the last element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0 or is <see cref="ReadOnlyList{T}.Count"/> or greater.
        /// </exception>
        public void Reverse(int index)
        {
            this.Reverse(index, this.Count - index);
        }
        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements in the <see cref="ReadOnlyList{T}"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        /// </exception>
        public void Reverse(int index, int count)
        {
            if (this.Count <= 1)
                return;

            InnerList.Reverse(index, count);
        }
        /// <summary>
        /// Sorts the elements in the entire <see cref="ReadOnlyList{T}"/> using the default comparer.
        /// </summary>
        /// <exception cref="ArgumentException">
        ///     The implementation of <paramref name="comparer"/> caused an error during the sort.  For example, <paramref name="comparer"/>
        ///     might not return 0 when comparing an item with itself.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}"/> cannot find an implementation of the <see cref="IComparable{T}"/>
        ///     generic interface of the <see cref="IComparable"/> interface for type <typeparamref name="T"/>.
        /// </exception>
        public void Sort()
        {
            this.Sort(0, this.Count, this.DefaultComparer);
        }
        /// <summary>
        /// Sorts the elements in the entire <see cref="ReadOnlySortableList{T}"/> using the specified comparer.
        /// </summary>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements, or null to use the default comparer
        ///     <see cref="Comparer{T}.Default"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     The implementation of <paramref name="comparer"/> caused an error during the sort.  For example, <paramref name="comparer"/>
        ///     might not return 0 when comparing an item with itself.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}"/> cannot find an implementation of the <see cref="IComparable{T}"/>
        ///     generic interface of the <see cref="IComparable"/> interface for type <typeparamref name="T"/>.
        /// </exception>
        public void Sort(IComparer<T> comparer)
        {
            this.Sort(0, this.Count, comparer);
        }
        /// <summary>
        /// Sorts the elements starting at the specified index in the <see cref="ReadOnlySortableList{T}"/> 
        /// and extends to the last element using <see cref="DefaultComparer"/>.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <exception cref="ArgumentException">
        ///     The implementation of the <see cref="DefaultComparer"/> caused an error during the sort.  For example, 
        ///     <see cref="DefaultComparer"/> might not return 0 when comparing an item with itself.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0 or greater than the current count of elements in the list.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}"/> cannot find an implementation of the 
        ///     <see cref="IComparable{T}"/> generic interface of the <see cref="IComparable"/> interface for 
        ///     type <typeparamref name="T"/>.
        /// </exception>
        public void Sort(int index)
        {
            if (index < 0 || index >= this.Count)
                throw FormattedException.NewFormat<ArgumentOutOfRangeException>(
                    Strings.ArgumentOutOfRange_NoCount_SortableList,
                    nameof(index),
                    nameof(ReadOnlySortableList<T>),
                    this.Count
                );

            this.Sort(index, this.Count - index, this.DefaultComparer);
        }
        /// <summary>
        /// Sorts the elements in the range of elements in <see cref="ReadOnlySortableList{T}"/> using 
        /// <see cref="DefaultComparer"/>.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <exception cref="ArgumentException">
        ///     The implementation of the <see cref="DefaultComparer"/> caused an error during the sort.  For example, 
        ///     <see cref="DefaultComparer"/> might not return 0 when comparing an item with itself.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0 -or- <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}"/> cannot find an implementation of the 
        ///     <see cref="IComparable{T}"/> generic interface of the <see cref="IComparable"/> interface for 
        ///     type <typeparamref name="T"/>.
        /// </exception>
        public void Sort(int index, int count)
        {
            this.Sort(index, count, this.DefaultComparer);
        }
        /// <summary>
        /// Sorts the elements in the range of elements in <see cref="ReadOnlySortableList{T}"/> using the specified 
        /// comparer.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <see langword="null"/> 
        ///     to use the default comparer <see cref="Comparer{T}.Default"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     The implementation of <paramref name="comparer"/> caused an error during the sort.  For example, 
        ///     <paramref name="comparer"/> might not return 0 when comparing an item with itself.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0 -or- <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}"/> cannot find an implementation of the 
        ///     <see cref="IComparable{T}"/> generic interface of the <see cref="IComparable"/> interface for 
        ///     type <typeparamref name="T"/>.
        /// </exception>
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            if (this.Count <= 1)
                return;

            InnerList.Sort(index, count, comparer);
        }

        #endregion

        #region PRIVATE METHODS
        private static IComparer<T> GetDefaultComparer()
        {
            return !typeof(T).Equals(typeof(string))
                ? Comparer<T>.Default
                : (IComparer<T>)StringComparer.CurrentCultureIgnoreCase;
        }

        #endregion
    }
}
