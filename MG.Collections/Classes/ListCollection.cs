﻿using MG.Collections.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace MG.Collections
{
    /// <summary>
    /// An indentical implementation to <see cref="List{T}"/> but with the ability for derived classes to override
    /// the Add, Insert, Set, and Remove methods.  Similar to the way <see cref="System.Collections.ObjectModel.Collection{T}"/>
    /// allows.
    /// </summary>
    /// <typeparam name="T">The type of elements in the <see cref="ListCollection{T}"/>.</typeparam>
    public class ListCollection<T> : IList<T>, IReadOnlyList<T>, ISearchableList<T>, IList, ICollection, IReadOnlySortableList<T>
    {
        #region PRIVATE FIELDS/CONSTANTS
        private const int DEFAULT_CAPACITY = 0;
        private readonly List<T> InnerList;

        #endregion

        #region EVENTS
        /// <summary>
        /// An event that occurs when the <see cref="ListCollection{T}"/> is reversed through the 'Reverse' methods.
        /// </summary>
        public event ReversedEventHandler? Reversed;
        /// <summary>
        /// An event that occurs when the <see cref="ListCollection{T}"/> is sorted through the 'Sort' methods.
        /// </summary>
        public event SortedEventHandler<T>? Sorted;

        #endregion

        #region INDEXERS
        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The index value for zero-based indexing.</param>
        /// <returns>
        ///     The element at the specified index.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0
        ///     -or-
        ///     <paramref name="index"/> is greater than or equal than <see cref="Count"/>.
        /// </exception>
        public T this[int index]
        {
            get => InnerList[index];
            set => this.SetItem(index, value);
        }

        #endregion

        #region PROPERTIES
        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold without resizing.
        /// </summary>
        /// <returns>
        ///     The number of elements that the <see cref="ListCollection{T}"/> can contain before resizing is required.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="Capacity"/> is set to a value that is less than <see cref="Count"/>.
        /// </exception>
        /// <exception cref="OutOfMemoryException">
        ///     There is not enough memory available on the system.
        /// </exception>
        public int Capacity
        {
            get => this.InnerList.Capacity;
            set => this.InnerList.Capacity = value;
        }
        /// <summary>
        /// Gets the number of elements contained in the <see cref="ListCollection{T}"/>.
        /// </summary>
        /// <returns>
        ///     The number of elements contained in the <see cref="ListCollection{T}"/>.
        /// </returns>
        public int Count => this.InnerList.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="ListCollection{T}"/> is read-only.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="ListCollection{T}"/> is read-only;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsReadOnly { get; protected set; }

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// The default constructor.  Initializes an empty <see cref="ListCollection{T}"/> with the default capacity.
        /// </summary>
        public ListCollection()
            : this(DEFAULT_CAPACITY)
        {
        }
        /// <summary>
        /// Initializes an empty <see cref="ListCollection{T}"/> with the specified capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new collection can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public ListCollection(int capacity)
        {
            this.InnerList = new List<T>(capacity);
        }

        /// <summary>
        /// Initializes a new <see cref="ListCollection{T}"/> instance that contains elements copied from the specified
        /// collection and has sufficient capacity to accomodate the number of elements copied.
        /// </summary>
        /// <remarks>
        ///     If <paramref name="items"/> is null, no exception is thrown, and, instead, an empty
        ///     <see cref="ListCollection{T}"/> instance is initialized.
        /// </remarks>
        /// <param name="items">
        ///     The collection whose elements will be copied to the <see cref="ListCollection{T}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
        public ListCollection(IEnumerable<T> items)
            : this(items, false)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="ListCollection{T}"/> instance that contains elements copied from the specified
        /// collection and has sufficient capacity to accomodate the number of elements copied.  It also provides an option to
        /// initialize the collection even if <paramref name="items"/> is <see langword="null"/>.
        /// </summary>
        /// <remarks>
        ///     If <paramref name="items"/> is null and <paramref name="initializeIfNull"/> is 
        ///     <see langword="false"/>, an <see cref="ArgumentNullException"/> exception is thrown.  If, however,
        ///     <paramref name="initializeIfNull"/> is <see langword="true"/>, an empty
        ///     <see cref="ListCollection{T}"/> instance is initialized with the default capacity instead.
        /// </remarks>
        /// <param name="items">
        ///     The collection whose elements will be copied to the <see cref="ListCollection{T}"/>.
        /// </param>
        /// <param name="initializeIfNull">
        ///     Indicates that the <see cref="ListCollection{T}"/> should be initialized even if <paramref name="items"/>
        ///     is found to be <see langword="null"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="initializeIfNull"/> is <see langword="false"/> and
        ///     <paramref name="items"/> is <see langword="null"/>.
        /// </exception>
        public ListCollection(IEnumerable<T> items, bool initializeIfNull)
        {
            this.InnerList = initializeIfNull && items is null
                ? new List<T>(DEFAULT_CAPACITY)
                : new List<T>(items);
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
            return this.InnerList.Contains(item);
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
            this.InnerList.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence
        /// within the entire <see cref="UniqueListBase{T}"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="UniqueListBase{T}"/>.  The value can be null for reference types.</param>
        public int IndexOf(T item)
        {
            return this.InnerList.IndexOf(item);
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
            return this.InnerList.ToArray();
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
            return this.InnerList.TrueForAll(ToPredicate(match));
        }

        #endregion

        #region LIST SPECIAL METHODS
        /// <summary>
        /// Searches the entire sorted <see cref="ListCollection{T}"/> for an element using the default comparer
        /// and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate.  The value can be <see langword="null"/> for reference types.</param>
        /// <returns>
        ///     The zero-based index of <paramref name="item"/> in the sorted <see cref="ListCollection{T}"/>,
        ///     if <paramref name="item"/> is found; otherwise, a negative number that is the bitwise complement
        ///     of the index of the next element that is larger than <paramref name="item"/> or, if there is no
        ///     larger element, the bitwise complement of <see cref="Count"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}.Default"/> cannot find
        ///     an implementation of the <see cref="IComparable{T}"/> generic interface or the <see cref="IComparable"/>
        ///     interface for type <typeparamref name="T"/>.
        /// </exception>
        public int BinarySearch(T item)
        {
            return this.BinarySearch(item);
        }
        /// <summary>
        /// Searches the entire sorted <see cref="ListCollection{T}"/> for an element using the specified comparer and
        /// returns the zero-based index of the elements.
        /// </summary>
        /// <param name="item">The object to locate.  The value can be <see langword="null"/> for reference types.</param>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements.
        ///     -or-
        ///     <see langword="null"/> to use the default comparer <see cref="Comparer{T}.Default"/>.
        /// </param>
        /// <returns>
        ///     The zero-based index of <paramref name="item"/> in the sorted <see cref="ListCollection{T}"/>, if
        ///     <paramref name="item"/> is found; otherwise, a negative number that is bitwise complement of the index
        ///     of the next element that is larger than <paramref name="item"/> or, if there is no larger element, the
        ///     bitwise complement of <see cref="Count"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="comparer"/> is <see langword="null"/>, and the default comparer <see cref="Comparer{T}.Default"/> 
        ///     cannot find an implementation of the <see cref="IComparable{T}"/> generic interface or the <see cref="IComparable"/>
        ///     interface for type <typeparamref name="T"/>.
        /// </exception>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return this.BinarySearch(item, comparer);
        }
        /// <summary>
        ///     Searches a range of elements in the sorted <see cref="ListCollection{T}"/>
        ///     for an element using the specified comparer and returns the zero-based index
        ///     of the element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate. The value can be <see langword="null"/> for reference types.</param>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements.
        ///     -or-
        ///     <see langword="null"/> to use the default comparer <see cref="Comparer{T}.Default"/>.
        /// </param>
        /// <returns>
        ///     The zero-based index of <paramref name="item"/> in the sorted <see cref="ListCollection{T}"/>, if
        ///     <paramref name="item"/> is found; otherwise, a negative number that is bitwise complement of the index
        ///     of the next element that is larger than <paramref name="item"/> or, if there is no larger element, the
        ///     bitwise complement of <see cref="Count"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> and <paramref name="count"/> do not denote a valid range in the
        ///     <see cref="ListCollection{T}"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="comparer"/> is <see langword="null"/>, and the default comparer <see cref="Comparer{T}.Default"/> 
        ///     cannot find an implementation of the <see cref="IComparable{T}"/> generic interface or the <see cref="IComparable"/>
        ///     interface for type <typeparamref name="T"/>.
        /// </exception>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            return this.BinarySearch(index, count, item, comparer);
        }
        /// <summary>
        /// Converts the elements in the current <see cref="ListCollection{T}"/> to another
        ///     type, and returns a list containing the converted elements.
        /// </summary>
        /// <typeparam name="TOutput">
        ///     The type of the elements of the target array.
        /// </typeparam>
        /// <param name="converter">
        ///     A <see cref="Converter{TInput, TOutput}"/> delegate that converts each element from one type
        ///     to another type.
        /// </param>
        /// <returns>
        ///     A <see cref="List{T}"/> of the target type containing the converted elements from the
        ///     current <see cref="ListCollection{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            return this.InnerList.ConvertAll(converter);
        }
        /// <summary>
        /// Copies the entire <see cref="ListCollection{T}"/> to a compatible one-dimensional array, starting at the
        /// beginning of the target array.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="Array"/> that is the destination of the elements copied from
        ///     <see cref="ListCollection{T}"/>.  The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     The number of elements in the source <see cref="ListCollection{T}"/> is greater than the number of
        ///     elements that the destination array can contain.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="array"/> is <see langword="null"/>.
        /// </exception>
        public void CopyTo(T[] array)
        {
            this.CopyTo(array);
        }
        /// <summary>
        /// Copies a range of elements from the <see cref="ListCollection{T}"/> to a compatible one-dimensional array,
        /// starting at the specified index of the target array.
        /// </summary>
        /// <param name="index">
        ///     The zero-based index in the source <see cref="ListCollection{T}"/> at which copying begins.
        /// </param>
        /// <param name="array">
        ///     The one-dimensional <see cref="Array"/> that is the destination of the elements copied from
        ///     <see cref="ListCollection{T}"/>.  The <see cref="Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        ///     The zero-based index in <paramref name="array"/> at which copying begins.
        /// </param>
        /// <param name="count">The number of elements to copy.</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> is equal to or greater than <see cref="Count"/> of the source <see cref="ListCollection{T}"/>.
        ///     -or-
        ///     The number of elements from <paramref name="index"/> to the end of the source <see cref="ListCollection{T}"/> is greater
        ///     than the available space from <paramref name="arrayIndex"/> to the end of the destination array.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="array"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        /// </exception>
        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            this.InnerList.CopyTo(index, array, arrayIndex, count);
        }
        /// <summary>
        /// Performs the specified action on each element of the <see cref="ListCollection{T}"/>.
        /// </summary>
        /// <param name="action">
        ///     The <see cref="Action{T}"/> delegate to perform on each element of the <see cref="ListCollection{T}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="action"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        ///     An element in the collection has been modified.
        /// </exception>
        public void ForEach(Action<T> action)
        {
            this.InnerList.ForEach(action);
        }
        /// <summary>
        /// Inserts the elements of a collection into the <see cref="ListCollection{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">
        ///     The collection whose elements should be insert into the <see cref="ListCollection{T}"/>.  The collection
        ///     itself cannot be <see langword="null"/>, but it can contain elements that are <see langword="null"/>, if 
        ///     type <typeparamref name="T"/> is a reference type.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="index"/> is greater than <see cref="Count"/>.
        /// </exception>
        public void InsertRange(int index, IEnumerable<T> collection)
        {
            this.InnerList.InsertRange(index, collection);
        }
        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Func{T, TResult}"/> delegate that defines the conditions of the elements to remove.
        /// </param>
        /// <returns>
        ///     The number of elements removed from the <see cref="ListCollection{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="match"/> is <see langword="null"/>.
        /// </exception>
        public int RemoveAll(Func<T, bool> match)
        {
            return this.InnerList.RemoveAll(ToPredicate(match));
        }
        /// <summary>
        /// Removes a range of elements from the <see cref="ListCollection{T}"/>.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements
        ///     in the <see cref="ListCollection{T}"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        /// </exception>
        public void RemoveRange(int index, int count)
        {
            this.InnerList.RemoveRange(index, count);
        }
        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="ListCollection{T}"/>,
        /// if that number is less than a threshold value.
        /// </summary>
        public void TrimExcess()
        {
            this.InnerList.TrimExcess();
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
            return this.InnerList.Exists(ToPredicate(match));
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
        [return: MaybeNull]
        public T Find(Func<T, bool> match)
        {
            return this.InnerList.Find(ToPredicate(match));
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
            return this.InnerList.FindAll(ToPredicate(match));
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
            return this.InnerList.FindIndex(ToPredicate(match));
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
            return this.InnerList.FindIndex(startIndex, ToPredicate(match));
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
            return this.InnerList.FindIndex(startIndex, count, ToPredicate(match));
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
        [return: MaybeNull]
        public T FindLast(Func<T, bool> match)
        {
            return this.InnerList.FindLast(ToPredicate(match));
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
            return this.InnerList.FindLastIndex(ToPredicate(match));
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
            return this.InnerList.FindLastIndex(startIndex, ToPredicate(match));
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
            return this.InnerList.FindLastIndex(startIndex, count, ToPredicate(match));
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
            return this.InnerList.GetRange(index, count);
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
            return this.InnerList.IndexOf(item, index);
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
            return this.InnerList.IndexOf(item, index, count);
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
            return this.InnerList.LastIndexOf(item);
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
            return this.InnerList.LastIndexOf(item, index);
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
            return this.InnerList.LastIndexOf(item, index, count);
        }

        #endregion

        #region SORTING METHODS
        /// <summary>
        /// Reverses the order of the elements in the entire <see cref="ListCollection{T}"/>.
        /// </summary>
        public void Reverse()
        {
            this.InnerList.Reverse();
            this.OnReversed();
        }
        /// <summary>
        /// Reverses the order of the elements in the specified range.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements in 
        ///     the <see cref="ListCollection{T}"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        /// </exception>
        public void Reverse(int index, int count)
        {
            this.InnerList.Reverse(index, count);
            this.OnReversed(index, count);
        }
        /// <summary>
        /// Sorts the elements in the entire <see cref="ListCollection{T}"/> using the default comparer.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}.Default"/> cannot find an implementation of the 
        ///     <see cref="IComparable{T}"/> generic interface or the <see cref="IComparable"/> interface for type 
        ///     <typeparamref name="T"/>.
        /// </exception>
        public void Sort()
        {
            this.InnerList.Sort();
            this.OnSorted(Comparer<T>.Default);
        }
        /// <summary>
        /// Sorts the elements in the entire <see cref="ListCollection{T}"/> using the specified comparer.
        /// </summary>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <see langword="null"/> to use the default comparer
        ///     <see cref="Comparer{T}.Default"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     The implementation of <paramref name="comparer"/> caused an error during the sort. For example,
        ///     <paramref name="comparer"/> might not return 0 when comparing an item with itself.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}.Default"/> cannot find an implementation of the 
        ///     <see cref="IComparable{T}"/> generic interface or the <see cref="IComparable"/> interface for type 
        ///     <typeparamref name="T"/>.
        /// </exception>
        public void Sort(IComparer<T> comparer)
        {
            this.InnerList.Sort(comparer);
            this.OnSorted(comparer);
        }
        /// <summary>
        /// Sorts the elements in a range of elements in <see cref="ListCollection{T}"/> using the specified comparer.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements, or <see langword="null"/> to use the default comparer
        ///     <see cref="Comparer{T}.Default"/>.
        /// </param>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> and <paramref name="count"/> do not specify a valid range in the <see cref="ListCollection{T}"/>.
        ///     -or-
        ///     The implementation of <paramref name="comparer"/> caused an error during the sort. For example,
        ///     <paramref name="comparer"/> might not return 0 when comparing an item with itself.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <paramref name="comparer"/> is <see langword="null"/>, and the default comparer <see cref="Comparer{T}.Default"/> 
        ///     cannot find an implementation of the <see cref="IComparable{T}"/> generic interface or the <see cref="IComparable"/> 
        ///     interface for type <typeparamref name="T"/>.
        /// </exception>
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            this.Sort(index, count, comparer);
            this.OnSorted(comparer, index, count);
        }

        #endregion

        #region ENUMERATORS
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="UniqueListBase{T}"/>.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="IEnumerable"/>.
        /// </summary>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region SEARCHABLE LIST INTERFACE METHODS
        IList<T> ISearchableList<T>.FindAll(Func<T, bool> match)
        {
            return this.FindAll(match);
        }
        IList<T> ISearchableList<T>.GetRange(int index, int count)
        {
            return this.GetRange(index, count);
        }

        #endregion

        #region EXTRA ILIST METHODS
        /// <summary>
        /// Inserts an element into the <see cref="ListCollection{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be <see langword="null"/> for reference types.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="index"/> is greater than <see cref="Count"/>.
        /// </exception>
        public void Insert(int index, T item)
        {
            this.InsertItem(index, item);
        }
        /// <summary>
        /// Removes the element at the specified index of the <see cref="ListCollection{T}"/>.
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

        #region NON-GENERIC ILIST INTERFACE EXPLICITS

        #region EXPLICIT INDEXER
        object? IList.this[int index]
        {
            get => this[index];
            set
            {
                this[index] = value is T item
                    ? item
                    : throw new NotSupportedException();
            }
        }

        #endregion

        #region EXPLICIT PROPERTIES
        bool IList.IsFixedSize => false;
        bool ICollection.IsSynchronized => ((ICollection)InnerList).IsSynchronized;
        object ICollection.SyncRoot => ((ICollection)InnerList).SyncRoot;

        #endregion

        #region EXPLICIT METHODS
        int IList.Add(object? value)
        {
            int index = -1;
            if (value is T item)
            {
                this.Add(item);
                index = this.IndexOf(item);
            }

            return index;
        }
        bool IList.Contains(object? value)
        {
            return value is T item && this.Contains(item);
        }
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)InnerList).CopyTo(array, index);
        }
        int IList.IndexOf(object? value)
        {
            return ((IList)this.InnerList).IndexOf(value);
        }
        void IList.Insert(int index, object? value)
        {
            if (value is T item)
            {
                this.Insert(index, item);
            }
        }
        void IList.Remove(object? value)
        {
            if (value is T item)
            {
                _ = this.Remove(item);
            }
        }

        #endregion

        #endregion

        #region PROTECTED AND OVERRIDABLE METHODS
        /// <summary>
        /// Adds an object to the end of the <see cref="ListCollection{T}"/>.
        /// </summary>
        /// <param name="item">The object to add.</param>
        protected virtual void AddItem(T item)
        {
            this.InnerList.Add(item);
        }
        /// <summary>
        /// Removes all elements from the <see cref="ListCollection{T}"/>.
        /// </summary>
        protected virtual void ClearItems()
        {
            this.InnerList.Clear();
        }

        /// <summary>
        /// Inserts an elements into the <see cref="ListCollection{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected virtual bool InsertItem(int index, T item)
        {
            try
            {
                this.InnerList.Insert(index, item);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Removes the specified element from the <see cref="ListCollection{T}"/>.
        /// </summary>
        /// <param name="item">The element to remove.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="item"/> is successfully removed; otherwise <see langword="false"/>.
        ///     This method also returns <see langword="false"/> if <paramref name="item"/> was not found in 
        ///     the <see cref="ListCollection{T}"/>.
        /// </returns>
        protected virtual bool RemoveItem(T item)
        {
            return this.InnerList.Remove(item);
        }
        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        protected virtual bool RemoveItemAt(int index)
        {
            try
            {
                this.InnerList.RemoveAt(index);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Replaces the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item">The new value for the element at the specified index.</param>
        protected virtual bool SetItem(int index, T item)
        {
            try
            {
                this.InnerList[index] = item;
                return true;
            }
            catch
            {
                return false;
            }
        }

        #region EVENT TRIGGERS
        /// <summary>
        /// The method that invokes the <see cref="Reversed"/> event with -1 as the index and count.
        /// </summary>
        protected void OnReversed()
        {
            this.OnReversed(-1, -1);
        }
        /// <summary>
        /// The method that invoces the <see cref="Reversed"/> event with the specified index and count of
        /// the Reverse operation.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to reverse.</param>
        /// <param name="count">The number of elements in the range to reverse.</param>
        protected virtual void OnReversed(int index, int count)
        {
            this.Reversed?.Invoke(this, new ReversedEventArgs(index, count));
        }
        /// <summary>
        /// The method that invokes the <see cref="Sorted"/> event.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range that was sorted.</param>
        /// <param name="count">The length of the range that was sorted.</param>
        /// <param name="comparerUsed">
        ///     The <see cref="IComparer{T}"/> implementation that was used when comparing elements, or <see langword="null"/> to use the default comparer
        ///     <see cref="Comparer{T}.Default"/>.
        /// </param>
        protected virtual void OnSorted(IComparer<T> comparerUsed, int index = -1, int count = -1)
        {
            if (null == comparerUsed)
                comparerUsed = Comparer<T>.Default;

            this.Sorted?.Invoke(this, new SortedEventArgs<T>(index, count, comparerUsed));
        }

        #endregion

        #endregion

        #region PRIVATE/BACKEND METHODS
        /// <summary>
        /// Converts the given <see cref="Func{T, TResult}"/> into a <see cref="Predicate{T}"/> delegate.
        /// </summary>
        /// <param name="func">The function to convert.</param>
        /// <returns>
        ///     A <see cref="Predicate{T}"/> delegate converted from <paramref name="func"/>.
        /// </returns>
        protected static Predicate<T> ToPredicate(Func<T, bool> func)
        {
            return new Predicate<T>(func);
        }

        #endregion
    }
}
