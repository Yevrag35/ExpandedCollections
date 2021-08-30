using MG.Collections.Extensions;
using MG.Collections.Extensions.List;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable CA1010 // Collections should implement generic interface
#pragma warning disable CA1710 // Identifiers should have correct suffix
#pragma warning disable IDE0130

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
        private protected List<T> InnerList;
        /// <summary>
        /// The internal, backing <see cref="HashSet{T}"/> set that determines uniqueness in the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        private protected HashSet<T> InnerSet;

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
        /// <remarks>
        ///     If <paramref name="collection"/> is null, no exception is thrown, and, instead, an empty
        ///     <see cref="UniqueListBase{T}"/> instance is initialized.
        /// </remarks>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <param name="equalityComparer">
        ///     The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the list, or
        ///     <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> implementation for the
        ///     type <typeparamref name="T"/>.
        /// </param>
        public UniqueListBase(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer)
        {
            if (null != collection)
            {
                InnerSet = new HashSet<T>(collection, equalityComparer);
                InnerList = new List<T>(InnerSet);
            }
            else
            {
                InnerSet = new HashSet<T>(equalityComparer);
                InnerList = new List<T>();
            }
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
        public bool Contains(T item)
        {
            return InnerSet.Contains(item);
        }
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
        public void CopyTo(T[] array, int arrayIndex)
        {
            InnerList.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence
        /// within the entire <see cref="UniqueListBase{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="UniqueListBase{T}"/>.  The value can be null for reference types.</param>
        public int IndexOf(T item)
        {
            return InnerList.IndexOf(item);
        }
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

        /// <summary>
        /// Determines whether every element in the <see cref="ReadOnlyList{T}"/> matches the conditions
        /// defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate that defines the conditions to check against the elements.</param>
        /// <returns>
        ///     <see langword="true"/>: if every element in the list matches the conditions defined; 
        ///     otherwise, <see langword="false"/>.
        ///     If the list has no elements, the return value is <see langword="true"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public bool TrueForAll(Func<T, bool> match)
        {
            return InnerList.TrueForAll(ToPredicate(match));
        }

        #endregion

        #region READONLY LIST METHODS
        /// <summary>
        ///     Determines whether the <see cref="ReadOnlyList{T}"/> contains elements that
        ///     match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Func{T, TResult}"/> delegate that defines the conditions of the 
        ///     elements to search for.
        /// </param>
        /// <returns>
        /// <see langword="true"/>:
        ///     if the <see cref="ReadOnlyList{T}"/> contains one or more elements that
        ///     <paramref name="match"/> defined.
        /// <see langword="false"/>:
        ///     otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public bool Exists(Func<T, bool> match)
        {
            return InnerList.Exists(ToPredicate(match));
        }

        /// <summary>
        ///     Searches for an element that matches the conditions defined by the specified
        ///     predicate, and returns the first occurrence within the entire <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Func{T, TResult}"/> delegate that defines the conditions of the
        ///     elements to search for.
        /// </param>
        /// <returns>
        ///     The first element that matches the conditions if found; otherwise the default value for <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public T Find(Func<T, bool> match)
        {
            return InnerList.Find(ToPredicate(match));
        }

        /// <summary>
        /// Retrieves all of the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     An <see cref="List{T}"/> containing all of the elements that match the conditions if found; 
        ///     otherwise, an empty list.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public List<T> FindAll(Func<T, bool> match)
        {
            return InnerList.FindAll(ToPredicate(match));
        }

        /// <summary>
        /// Searches for an element that match the conditions defined by the specified predicate, and returns the zero-based
        /// index of the first occurrence within the entire <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     The zero-based index of the first occurrence of an element that matches the conditions defined
        ///     by <paramref name="match"/> if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public int FindIndex(Func<T, bool> match)
        {
            return InnerList.FindIndex(ToPredicate(match));
        }

        /// <summary>
        /// Searches for an element that match the conditions defined by the specified predicate, and returns the zero-based
        /// index of the first occurrence within the range of elements in the <see cref="ReadOnlyList{T}"/> that extends from the 
        /// specified index to the last element.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     The zero-based index of the first occurrence of an element that matches the conditions defined
        ///     by <paramref name="match"/> if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="ReadOnlyList{T}"/>.
        /// </exception>
        public int FindIndex(int startIndex, Func<T, bool> match)
        {
            return InnerList.FindIndex(startIndex, ToPredicate(match));
        }

        /// <summary>
        /// Searches for an element that match the conditions defined by the specified predicate, and returns the zero-based
        /// index of the first occurrence within the range of elements in the <see cref="ReadOnlyList{T}"/> that starts at the 
        /// specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     The zero-based index of the first occurrence of an element that matches the conditions defined
        ///     by <paramref name="match"/> if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="ReadOnlyList{T}"/>.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        ///     -or-
        ///     <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section of the list.
        /// </exception>
        public int FindIndex(int startIndex, int count, Func<T, bool> match)
        {
            return InnerList.FindIndex(startIndex, count, ToPredicate(match));
        }

        /// <summary>
        /// Searches for an elements that matches the conditions defined by the specified predicate, and returns the last occurrence within the
        /// entire <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     The last elements that matches the conditions defined by the specified predicate, if found; otherwise, the default value for
        ///     type <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public T FindLast(Func<T, bool> match)
        {
            return InnerList.FindLast(ToPredicate(match));
        }

        /// <summary>
        /// Searches for an elements that matches the conditions defined by the specified predicate, and returns the zero-based index of the
        /// lat occurrence within the entire <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of an element that matches the conditions defined by
        ///     <paramref name="match"/>, if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public int FindLastIndex(Func<T, bool> match)
        {
            return InnerList.FindLastIndex(ToPredicate(match));
        }

        /// <summary>
        /// Searches for an elements that matches the conditions defined by the specified predicate, and returns the zero-based index of the
        /// lat occurrence within the range of elements in the <see cref="ReadOnlyList{T}"/> that extends from the first element to the 
        /// specified index.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the backward search.</param>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of an element that matches the conditions defined by
        ///     <paramref name="match"/>, if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="ReadOnlyList{T}"/>.
        /// </exception>
        public int FindLastIndex(int startIndex, Func<T, bool> match)
        {
            return InnerList.FindLastIndex(startIndex, ToPredicate(match));
        }

        /// <summary>
        /// Searches for an elements that matches the conditions defined by the specified predicate, and returns the zero-based index of the
        /// lat occurrence within the range of elements in the <see cref="ReadOnlyList{T}"/> that contains the specified number
        /// of elements and ends at the specified index.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Func{T, TResult}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of an element that matches the conditions defined by
        ///     <paramref name="match"/>, if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="startIndex"/> is outside the range of valid indexes for the <see cref="ReadOnlyList{T}"/>.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        ///     -or-
        ///     <paramref name="startIndex"/> and <paramref name="count"/> do not specify a valid section of the list.
        /// </exception>
        public int FindLastIndex(int startIndex, int count, Func<T, bool> match)
        {
            return InnerList.FindLastIndex(startIndex, count, ToPredicate(match));
        }

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>A shallow copy of a range of elements in the <see cref="ReadOnlyList{T}"/>.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        /// </exception>
        public List<T> GetRange(int index, int count)
        {
            return InnerList.GetRange(index, count);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence
        /// within the range of elements in the <see cref="ReadOnlyList{T}"/> that extends from the specified index to the last element.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ReadOnlyList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// <param name="index">The zero-based starting index of the search.  0 (zero) is valid in an empty list.</param>
        /// <returns>
        ///     The zero-based index of the first occurrence of <paramref name="item"/> within the range of elements in the
        ///     <see cref="ReadOnlyList{T}"/>, if found; otherwise -1.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is outside the range of valid indexes for the <see cref="ReadOnlyList{T}"/>.
        /// </exception>
        public int IndexOf(T item, int index)
        {
            return InnerList.IndexOf(item, index);
        }
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence
        /// within the range of elements in the <see cref="ReadOnlyList{T}"/> that extends from the specified index to the last element.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ReadOnlyList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// <param name="index">The zero-based starting index of the search.  0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>
        ///     The zero-based index of the first occurrence of <paramref name="item"/> within the range of elements in the
        ///     <see cref="ReadOnlyList{T}"/>, if found; otherwise -1.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is outside the range of valid indexes for the <see cref="ReadOnlyList{T}"/>.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        ///     -or-
        ///     <paramref name="index"/> and <paramref name="count"/> do not specify a vliad section in the list.
        /// </exception>
        public int IndexOf(T item, int index, int count)
        {
            return InnerList.IndexOf(item, index, count);
        }
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence
        /// within the entire <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ReadOnlyList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// <returns>
        ///     The zero-based index of the last occurrence of <paramref name="item"/> within the entire
        ///     <see cref="ReadOnlyList{T}"/>, if found; otherwise -1.
        /// </returns>
        public int LastIndexOf(T item)
        {
            return InnerList.LastIndexOf(item);
        }
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence
        /// within the range of elements in the <see cref="ReadOnlyList{T}"/> that extends from the first element to the specified index.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ReadOnlyList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of <paramref name="item"/> within the range of elements in the
        ///     <see cref="ReadOnlyList{T}"/>, if found; otherwise -1.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is outside the range of valid indexes for the <see cref="ReadOnlyList{T}"/>.
        /// </exception>
        public int LastIndexOf(T item, int index)
        {
            return InnerList.LastIndexOf(item, index);
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last occurrence
        /// within the range of elements in the <see cref="ReadOnlyList{T}"/> that contains the specified number
        /// of elements and ends at the specified index.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ReadOnlyList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>
        ///     The zero-based index of the last occurrence of <paramref name="item"/> within the range of elements in the
        ///     <see cref="ReadOnlyList{T}"/> that contains <paramref name="count"/> number of elements and ends at
        ///     <paramref name="index"/>, if found; otherwise, -1;
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is outside the range of valid indexes for the <see cref="ReadOnlyList{T}"/>.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        ///     -or-
        ///     <paramref name="index"/> and <paramref name="count"/> do not specify a vliad section in the list.
        /// </exception>
        public int LastIndexOf(T item, int index, int count)
        {
            return InnerList.LastIndexOf(item, index, count);
        }

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
            return InnerSet.IsProperSubsetOf(other);
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
            return InnerSet.IsProperSupersetOf(other);
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
            return InnerSet.IsSupersetOf(other);
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
            return InnerSet.Overlaps(other);
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
            InnerList.Clear();
            InnerSet.Clear();
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
            return InnerList.GetByIndex(index);
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
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="index"/> is equal to or greater than <see cref="Count"/>.
        /// </exception>
        protected virtual void RemoveItemAt(int index)
        {
            T item = InnerList[index];
            if (InnerSet.Remove(item))
            {
                InnerList.RemoveAt(index);
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
        protected virtual bool SetItem(int index, T item)
        {
            bool result = false;
            T value = InnerList[index];
            if (InnerSet.Add(item))
            {
                result = InnerSet.Remove(value);
                InnerList[index] = item;
            }

            return result;
        }

        #endregion

        private static IEqualityComparer<T> GetDefaultComparer()
        {
            return !typeof(T).Equals(typeof(string))
                ? EqualityComparer<T>.Default
                : (IEqualityComparer<T>)StringComparer.CurrentCulture;
        }

        private static Predicate<T> ToPredicate(Func<T, bool> func)
        {
            return new Predicate<T>(func);
        }

        protected private bool TryIsValidIndex(int index, out int positiveIndex)
        {
            return InnerList.TryIsValidIndex(index, out positiveIndex);
        }
    }
}