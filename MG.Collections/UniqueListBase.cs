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
    public abstract class UniqueListBase<T> : ICollection<T>, ICollection, IReadOnlySet<T>
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
        /// The equality comparer used to determine uniqueness in the <see cref="UniqueListBase{T}"/>.
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
        /// and has the default initial capacity and default equality comparer for <typeparamref name="T"/>.
        /// </summary>
        public UniqueListBase()
            : this(0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueListBase{T}"/> class that is empty
        /// and has the specified initial capacity and default equality comparer for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="capacity">The number of elements that the new collection can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public UniqueListBase(int capacity)
            : this(capacity, GetDefaultComparer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueListBase{T}"/> class that
        /// contains elements copied from the specified <see cref="IEnumerable{T}"/> and has
        /// sufficient capacity to accommodate the number of elements copied and the default
        /// equality comparer for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="ArgumentNullException"/>
        public UniqueListBase(IEnumerable<T> collection)
            : this(collection, GetDefaultComparer())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueListBase{T}"/> class that is empty
        /// and has the default initial capacity and the specified equality comparer for <typeparamref name="T"/>.
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
        public void Add(T item)
        {
            this.AddItem(item);
        }
        /// <summary>
        /// Removes all elements from the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        public void Clear()
        {
            this.ClearItems();
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
  //      /// <summary>
  //      /// Sorts the elements in the entire <see cref="UniqueListBase{T}"/> using the default comparer.
  //      /// </summary>
  //      /// <exception cref="InvalidOperationException"/>
  //      public void Sort() => InnerList.Sort();
  //      /// <summary>
  //      /// Sorts the elements in the entire <see cref="UniqueListBase{T}"/> using the specified comparer.
  //      /// </summary>
  //      /// <param name="comparer">
  //      /// The <see cref="IComparer{T}"/> implementation to use when comparing elements.
  //      /// </param>
  //      /// <exception cref="InvalidOperationException"/>
  //      /// <exception cref="ArgumentException"/>
  //      /// <exception cref="ArgumentNullException"/>
		//public void Sort(IComparer<T> comparer)
  //      {
  //          if (comparer == null)
  //              throw new ArgumentNullException("comparer");

  //          InnerList.Sort(comparer);
  //      }
        /// <summary>
        /// Removes the specific object from the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        /// <param name="item">
        ///     The object to remove from the <see cref="UniqueListBase{T}"/>.
        /// </param>
        public bool Remove(T item)
        {
            return this.RemoveItem(item);
        }
        /// <summary>
        /// Copies the elements of the <see cref="UniqueList{T}"/> to a new array.
        /// </summary>
        /// <returns>
        ///     An array containing copies of the elements of the <see cref="UniqueList{T}"/>.  If the list contains no elements, 
        ///     an empty array is returned.
        /// </returns>
        public T[] ToArray()
        {
            return InnerList.ToArray();
        }

        #endregion

        #region READONLY LIST METHODS


        #endregion

        #region READONLY SET METHODS
        /// <summary>
        /// Determines whether this <see cref="UniqueListBase{T}"/> object is a proper subset of the specified collection.
        /// </summary>
        /// <param name="other">The collection compare to the current <see cref="UniqueListBase{T}"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="UniqueListBase{T}"/> is a proper subset of <paramref name="other"/>;
        ///     otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return this.InnerSet.IsProperSubsetOf(other);
        }
        /// <summary>
        /// Determines whether this <see cref="UniqueListBase{T}"/> object is a proper superset of the specified collection.
        /// </summary>
        /// <param name="other">The collection compare to the current <see cref="UniqueListBase{T}"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="UniqueListBase{T}"/> is a proper superset of <paramref name="other"/>;
        ///     otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return this.InnerSet.IsProperSupersetOf(other);
        }
        /// <summary>
        /// Determines whether this <see cref="UniqueListBase{T}"/> object is a subset of the specified collection.
        /// </summary>
        /// <param name="other">The collection compare to the current <see cref="UniqueListBase{T}"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="UniqueListBase{T}"/> is a subset of <paramref name="other"/>;
        ///     otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return this.IsSubsetOf(other);
        }
        /// <summary>
        /// Determines whether this <see cref="UniqueListBase{T}"/> object is a superset of the specified collection.
        /// </summary>
        /// <param name="other">The collection compare to the current <see cref="UniqueListBase{T}"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="UniqueListBase{T}"/> is a superset of <paramref name="other"/>;
        ///     otherwise <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return this.InnerSet.IsSupersetOf(other);
        }
        /// <summary>
        /// Determines whether the current <see cref="UniqueListBase{T}"/> object and a specified collection share
        /// common elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current <see cref="UniqueListBase{T}"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="UniqueListBase{T}"/> and <paramref name="other"/> share at least
        ///     one common element; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool Overlaps(IEnumerable<T> other)
        {
            return this.InnerSet.Overlaps(other);
        }
        /// <summary>
        /// Determines whether this <see cref="UniqueListBase{T}"/> object and the specified collection contain
        /// the same elements.
        /// </summary>
        /// <param name="other">The collection to compare to the current <see cref="UniqueListBase{T}"/>.</param>
        /// <returns>
        ///     <see langword="true"> if the <see cref="UniqueListBase{T}"/> object is equal to <paramref name="other"/>;
        ///     otherwise, <see langword="false"/>.</see>
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="other"/> is <see langword="null"/>.</exception>
        public bool SetEquals(IEnumerable<T> other)
        {
            return this.SetEquals(other);
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
        /// Adds an object to the end of the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        /// <param name="item">The object to add.</param>
        protected virtual void AddItem(T item)
        {
            if (InnerSet.Add(item))
            {
                InnerList.Add(item);
            }
        }
        /// <summary>
        /// Removes all elements from the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        protected virtual void ClearItems()
        {
            this.InnerList.Clear();
            this.InnerSet.Clear();
        }

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
        /// <summary>
        /// Inserts an elements into the <see cref="UniqueListBase{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected virtual void InsertItem(int index, T item)
        {
            if (InnerSet.Add(item))
            {
                try
                {
                    InnerList.Insert(index, item);
                }
                catch
                {
                    _ = InnerSet.Remove(item);
                }
            }
        }
        /// <summary>
        /// Removes the specified element from the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        /// <param name="item">The element to remove.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="item"/> is successfully removed; otherwise <see langword="false"/>.
        ///     This method also returns <see langword="false"/> if <paramref name="item"/> was not found in 
        ///     the <see cref="UniqueListBase{T}"/>.
        /// </returns>
        protected virtual bool RemoveItem(T item)
        {
            bool result = InnerSet.Remove(item);
            if (result)
                InnerList.Remove(item);

            return result;
        }
        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="item">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="index"/> is equal to or greater than <see cref="Count"/>.
        /// </exception>
        protected virtual void RemoveItemAt(int index)
        {
            T item = this.InnerList[index];
            if (this.InnerSet.Remove(item))
            {
                this.InnerList.RemoveAt(index);
            }
        }
        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than zero
        ///     -or
        ///     <paramref name="index"/> is greater than <see cref="Count"/>.
        /// </exception>
        protected virtual void SetItem(int index, T item)
        {
            T value = InnerList[index];
            if (InnerSet.Add(item))
            {
                _ = InnerSet.Remove(value);
                InnerList[index] = item;
            }
        }

        #endregion

        private static IEqualityComparer<T> GetDefaultComparer()
        {
            return !typeof(T).Equals(typeof(string))
                ? EqualityComparer<T>.Default
                : (IEqualityComparer<T>)StringComparer.CurrentCulture;
        }

        protected private bool TryIsValidIndex(int index, out int positiveIndex)
        {
            return this.InnerList.TryIsValidIndex(index, out positiveIndex);
        }
    }
}