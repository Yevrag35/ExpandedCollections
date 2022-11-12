using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#pragma warning disable CA1010 // Collections should implement generic interface
#pragma warning disable CA1710 // Identifiers should have correct suffix
#pragma warning disable IDE0130

namespace MG.Collections
{
    /// <summary>
    /// A class that provides the same functionality as <see cref="List{T}"/>, but enforces every element to be
    /// unique according to the default or custom-defined equality comparer.
    /// </summary>
    /// <typeparam name="T">The element type in the <see cref="UniqueList{T}"/>.</typeparam>
    public class UniqueList<T> : UniqueListBase<T>, IReadOnlyList<T>, IList<T>, ISearchableList<T>
    {
        #region PRIVATE FIELDS/CONSTANTS
        private const int DEFAULT_CAPACITY = 0;

        #endregion

        #region INDEXERS
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <remarks>
        ///     If <paramref name="index"/> is negative, instead of starting at the zero-based position, the operation
        ///     will start at the last index of the <see cref="UniqueList{T}"/> and count backwards.
        /// </remarks>
        /// <param name="index">The positive index value for zero-based indexing or negative value for reverse indexing.</param>
        /// <returns>
        ///     For the get accessor, the element at the specified proper index if found; otherwise,
        ///         the default value of <typeparamref name="T"/>.
        /// </returns>
        [MaybeNull]
        public T this[int index]
        {
            get => base.GetByIndex(index) ?? default;
            set
            {
                if (!base.TryIsValidIndex(index, out int positiveIndex))
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                this.SetItem(positiveIndex, value);
            }
        }

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// The default constructor.  Initializes an empty list using the default 
        /// <see cref="IEqualityComparer{T}"/> for <typeparamref name="T"/>.
        /// </summary>
        public UniqueList()
            : base(DEFAULT_CAPACITY)
        {
        }
        /// <summary>
        /// Initializes an empty <see cref="UniqueList{T}"/> with the specified capacity using the default
        /// <see cref="IEqualityComparer{T}"/> for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="capacity">The number of elements that the new collection can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public UniqueList(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes an empty <see cref="UniqueList{T}"/> with the default capacity using the specified
        /// <see cref="IEqualityComparer{T}"/> to determine uniqueness.
        /// </summary>
        /// <param name="equalityComparer">The comparer used to define if an incoming element is unique.</param>
        public UniqueList(IEqualityComparer<T> equalityComparer)
            : base(DEFAULT_CAPACITY, equalityComparer)
        {
        }

        /// <summary>
        /// Initializes an empty <see cref="UniqueList{T}"/> with the specified capacity using the specified
        /// <see cref="IEqualityComparer{T}"/> to determine uniqueness.
        /// </summary>
        /// <param name="capacity">The number of new elements the list can initially store.</param>
        /// <param name="equalityComparer">The comparer used to define if an incoming element is unique.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public UniqueList(int capacity, IEqualityComparer<T> equalityComparer)
            : base(capacity, equalityComparer)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="UniqueList{T}"/> instance that contains elements copied from the specified
        /// collection and has sufficient capacity to accomodate the number of unique elements copied.
        /// </summary>
        /// <remarks>
        ///     <paramref name="items"/> will be enumerated for uniqueness according to the default
        ///     <see cref="IEqualityComparer{T}"/> for type <typeparamref name="T"/>.
        ///     
        ///     If <paramref name="items"/> is null, no exception is thrown, and, instead, an empty
        ///     <see cref="UniqueList{T}"/> instance is initialized.
        /// </remarks>
        /// <param name="items">
        ///     The collection whose elements will be enumerated for uniqueness and added
        ///     to the list.
        /// </param>
        public UniqueList(IEnumerable<T> items)
            : base(items)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="UniqueList{T}"/> instance that contains elements copied from the specified
        /// collection and has sufficient capacity to accomodate the number of unique elements copied.
        /// </summary>
        /// <remarks>
        ///     <paramref name="collection"/> will be enumerated for uniqueness according to the provided 
        ///     <see cref="IEqualityComparer{T}"/>.
        ///     
        ///     If <paramref name="collection"/> is null, no exception is thrown, and, instead, an empty
        ///     <see cref="UniqueList{T}"/> instance is initialized.
        /// </remarks>
        /// <param name="collection">
        ///     The collection whose elements will be enumerated for uniqueness and added
        ///     to the list.
        /// </param>
        /// <param name="equalityComparer">
        ///     The equality comparer that determines whether an element is unique.
        /// </param>
        public UniqueList(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer)
            : base(collection, equalityComparer)
        {
        }

        #endregion

        protected IList<T> FindAllValues(Func<T, bool> match)
        {
            return base.FindAll(match);
        }
        protected IList<T> GetRangeOfValues(int index, int count)
        {
            return base.GetRange(index, count);
        }

        IList<T> ISearchableList<T>.FindAll(Func<T, bool> match)
        {
            return this.FindAllValues(match);
        }
        IList<T> ISearchableList<T>.GetRange(int index, int count)
        {
            return this.GetRangeOfValues(index, count);
        }

        #region EXTRA ILIST METHODS
        /// <summary>
        /// Inserts an element into the <see cref="UniqueList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be <see langword="null"/> for reference types.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="index"/> is greater than <see cref="UniqueListBase{T}.Count"/>.
        /// </exception>
        public void Insert(int index, T item)
        {
            this.InsertItem(index, item);
        }
        /// <summary>
        /// Removes the element at the specified index of the <see cref="UniqueList{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="index"/> is greater than <see cref="UniqueListBase{T}.Count"/>.
        /// </exception>
        public void RemoveAt(int index)
        {
            this.RemoveItemAt(index);
        }

        #endregion
    }
}