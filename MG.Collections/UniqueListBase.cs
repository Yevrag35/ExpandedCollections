using MG.Collections.Extensions;
using MG.Collections.Extensions.List;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.Collections
{
    /// <summary>
    /// Provides the <see langword="abstract"/> base class to enforce uniqueness in generic collections.
    /// </summary>
    public abstract class UniqueListBase<T> : ICollection<T>, ICollection
    {
        #region FIELDS/CONSTANTS
        /// <summary>
        /// The internal, backing <see cref="List{T}"/> collection that all methods invoke against.
        /// </summary>
        protected private List<T> InnerList;
        /// <summary>
        /// The internal, backing <see cref="HashSet{T}"/> set that determines uniqueness in the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        protected private HashSet<T> InnerSet;

        #endregion

        #region PROPERTIES
        /// <summary>
        /// The equality comparer used to determine uniqueness in the list./>.
        /// </summary>
        public IEqualityComparer<T> Comparer => InnerSet.Comparer;

        /// <summary>
        /// Get the number of elements contained within the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        public int Count => InnerList.Count;

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueListBase{T}"/> class that is empty
        /// and has the default initial capacity.
        /// </summary>
        public UniqueListBase()
            : this(0, EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueListBase{T}"/> class that is empty
        /// and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new collection can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public UniqueListBase(int capacity)
            : this(capacity, EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueListBase{T}"/> class that
        /// contains elements copied from the specified <see cref="IEnumerable{T}"/> and has
        /// sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="ArgumentNullException"/>
        public UniqueListBase(IEnumerable<T> collection)
            : this(collection, EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equalityComparer">
        ///     The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the list, or
        ///     <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> implementation for the
        ///     type <typeparamref name="T"/>.
        /// </param>
        public UniqueListBase(IEqualityComparer<T> equalityComparer)
            : this(0, equalityComparer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueListBase{T}"/> class that is empty, has the specified
        /// initial capacity, and uses the specified equality comparer for the <typeparamref name="T"/> type.
        /// </summary>
        /// <param name="capacity">The number of elements that the new collection can initially store.</param>
        /// <param name="equalityComparer">
        ///     The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the list, or
        ///     <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> implementation for the
        ///     type <typeparamref name="T"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public UniqueListBase(int capacity, IEqualityComparer<T> equalityComparer)
        {
            InnerList = new List<T>(capacity);
            InnerSet = new HashSet<T>(equalityComparer);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueListBase{T}"/> class that uses the specified comparer for 
        /// the <typeparamref name="T"/> type, contains elements copied from the specified collection, and sufficient capacity
        /// to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <param name="equalityComparer">
        ///     The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the list, or
        ///     <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> implementation for the
        ///     type <typeparamref name="T"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public UniqueListBase(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer)
        {
            InnerSet = new HashSet<T>(collection, equalityComparer);
            InnerList = new List<T>(InnerSet);
        }

        #endregion

        #region BASE METHODS
        /// <summary>
        /// Adds an item to the end of the collection.
        /// </summary>
        /// <param name="item">The object to be added to the end of the collection.</param>
        public virtual void Add(T item)
        {
            if (InnerSet.Add(item))
            {
                InnerList.Add(item);
            }
        }
        /// <summary>
        /// Removes all elements from the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        public virtual void Clear()
        {
            InnerList.Clear();
            InnerSet.Clear();
        }
        /// <summary>
        /// Determines whether an element is in the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="UniqueListBase{T}"/>.  The value can be null for reference types.
        /// </param>
        public bool Contains(T item) => InnerSet.Contains(item);
        /// <summary>
        /// Copies the entire <see cref="UniqueListBase{T}"/> to a compatible one-dimensional array, starting at
        /// the specified index of the target array.
        /// </summary>
        /// <param name="array">
        /// The one-dimensional array that is the destination of the elements copied from
        /// <see cref="UniqueListBase{T}"/>.  The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in the target array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public void CopyTo(T[] array, int arrayIndex) => InnerList.CopyTo(array, arrayIndex);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence
        /// within the entire <see cref="UniqueListBase{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="UniqueListBase{T}"/>.  The value can be null for reference types.</param>
        public int IndexOf(T item) => InnerList.IndexOf(item);
        /// <summary>
        /// Sorts the elements in the entire <see cref="UniqueListBase{T}"/> using the default comparer.
        /// </summary>
        /// <exception cref="InvalidOperationException"/>
        public void Sort() => InnerList.Sort();
        /// <summary>
        /// Sorts the elements in the entire <see cref="UniqueListBase{T}"/> using the specified comparer.
        /// </summary>
        /// <param name="comparer">
        /// The <see cref="IComparer{T}"/> implementation to use when comparing elements.
        /// </param>
        /// <exception cref="InvalidOperationException"/>
        /// <exception cref="ArgumentException"/>
        /// <exception cref="ArgumentNullException"/>
		public void Sort(IComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            InnerList.Sort(comparer);
        }
        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="UniqueListBase{T}"/>.  The
        /// value can be null for reference types.
        /// </summary>
        /// <param name="item">
        /// The object to remove from the <see cref="UniqueListBase{T}"/>.
        /// The value can be null for reference types.
        /// </param>
        public virtual bool Remove(T item)
        {
            bool result = InnerSet.Remove(item);
            if (result)
                InnerList.Remove(item);

            return result;
        }

        #endregion

        #region ENUMERATOR
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        public IEnumerator<T> GetEnumerator() => InnerList.GetEnumerator();
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="IEnumerable"/>.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator() => InnerList.GetEnumerator();

        #endregion

        #region INTERFACE MEMBERS

        #region IMPLEMENTED INTERFACE PROPERTIES
        bool ICollection<T>.IsReadOnly => ((ICollection<T>)InnerList).IsReadOnly;
        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;
        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        #endregion

        #region IMPLEMENTED INTERFACE METHODS
        void ICollection.CopyTo(Array array, int index) => ((ICollection)InnerList).CopyTo(array, index);

        #endregion

        #endregion

        #region BACKEND/PRIVATE METHODS
        /// <summary>
        /// Transforms and verifies the specified negative or positive index into a proper <see cref="int"/> value
        /// returning the element of type <typeparamref name="T"/> at the proper index location.
        /// </summary>
        /// <remarks>
        ///     Used for transforming negative index <see cref="int"/> values into postive index positions.  When
        ///     negative indicies are specified, instead of starting the zero-based position, it will begin at the 
        ///     index of the last element of the <see cref="UniqueListBase{T}"/> and count backwards.
        ///     
        ///     Can be overridden for different behavior.
        /// </remarks>
        /// <param name="index">The negative or positive index value.</param>
        /// <returns>
        ///     The element of type <typeparamref name="TItem"/> at the specified proper index position; otherwise, 
        ///     if the index is determined to be out-of-range, then the default value of <typeparamref name="T"/>.
        /// </returns>
        protected virtual T GetByIndex(int index)
        {
            return this.InnerList.GetByIndex(index);
        }

        protected void ReplaceValueAtIndex(int index, T newValue)
        {
            T item = InnerList[index];
            if (InnerSet.Add(newValue))
            {
                InnerSet.Remove(item);
                InnerList[index] = newValue;
            }
        }

        protected bool TryIsValidIndex(int index, out int positiveIndex)
        {
            return this.InnerList.TryIsValidIndex(index, out positiveIndex);
        }

        #endregion
    }
}