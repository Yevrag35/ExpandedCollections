using MG.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MG.Collections
{
    /// <summary>
    /// A class that provides the same functionality as <see cref="List{T}"/>, but enforces every element to be
    /// unique according to the default or custom-defined equality comparer.
    /// </summary>
    /// <typeparam name="T">The element type in the <see cref="UniqueList{T}"/>.</typeparam>
    public class UniqueList<T> : UniqueListBase<T>, IReadOnlyList<T>, IList<T>
    {
        #region PRIVATE FIELDS/CONSTANTS
        private const int DEFAULT_CAPACITY = 0;

        #endregion

        #region INDEXERS
        public T this[int index]
        {
            get => base.GetByIndex(index);
            set
            {
                if (base.TryIsValidIndex(index, out int positiveIndex))
                    this.SetItem(positiveIndex, value);
                
                else
                    throw new ArgumentOutOfRangeException();
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
        /// <param name="capacity"></param>
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

        #region EXTRA ILIST METHODS
        public void Insert(int index, T item)
        {
            this.InsertItem(index, item);
        }
        public void RemoveAt(int index)
        {
            this.RemoveItemAt(index);
        }

        #endregion
    }
}