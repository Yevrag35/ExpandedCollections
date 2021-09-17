using MG.Collections.Extensions.List;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#pragma warning disable CA1010 // Collections should implement generic interface
#pragma warning disable CA1710 // Identifiers should have correct suffix
#pragma warning disable IDE0130

namespace MG.Collections
{
    /// <summary>
    /// A strongly typed, read-only list of objects that can be accessed by index and provides advanced methods for 
    /// searching and sorting through its contents.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadOnlyList<T> : IReadOnlyCollection<T>, IReadOnlyList<T>, ICollection, IEnumerable<T>, ISearchableList<T>
    {
        #region PROPERTIES

        /// <summary>
        /// The internal backing <see cref="List{T}"/> that all methods of <see cref="ReadOnlyList{T}"/> invoke against.
        /// </summary>
        private protected List<T> InnerList { get; }
        /// <summary>
        /// Gets the number of elements contained in the <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <returns>The number of elements contained within the <see cref="ReadOnlyList{T}"/>.</returns>
        public int Count
        {
            get => this.InnerList.Count;
        }

        #endregion

        #region INDEXER

        /// <summary>
        /// Gets the element at the specified index.  If <paramref name="index"/> is negative, then searching is done in reverse order.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index or the default value of <typeparamref name="T"/> if the index is out of range.</returns>
        public T this[int index]
        {
            get => InnerList.GetByIndex(index);
        }

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlyList{T}"/> that is empty
        /// and has the default capacity.
        /// </summary>
        protected ReadOnlyList()
        {
            this.InnerList = new List<T>();
        }
        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlyList{T}"/> that is empty
        /// and has the specified capacity.
        /// </summary>
        /// <param name="capacity">The number of new elements the list can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        protected ReadOnlyList(int capacity)
        {
            this.InnerList = new List<T>(capacity);
        }
        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlyList{T}"/> that contains the elements
        /// copied from the specified collection.
        /// </summary>
        /// <param name="items">The collection whose elements are copied to the new list.</param>
        /// <exception cref="ArgumentNullException"><paramref name="items"/> is <see langword="null"/>.</exception>
        public ReadOnlyList(IEnumerable<T> items)
        {
            this.InnerList = new List<T>(items);
        }

        #endregion

        #region LIST METHODS
        /// <summary>
        /// Searches the entire sorted <see cref="ReadOnlyList{T}"/> for an element using the default comparer
        /// and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate.  The value can be <see langword="null"/> for reference types.</param>
        /// <returns>
        ///     The zero-based index of <paramref name="item"/> in the sorted <see cref="ReadOnlyList{T}"/>, if
        ///     <paramref name="item"/> is found; otherwise, a negative number that is the bitwise complement of the index of
        ///     the next element that is larger than <paramref name="item"/> or, if there is no larger element, the bitwise
        ///     complement of <see cref="ReadOnlyList{T}.Count"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}.Default"/> cannot find an implementation of the <see cref="IComparable{T}"/>
        ///     interface or the <see cref="IComparable"/> interface for type <typeparamref name="T"/>.
        /// </exception>
        public int BinarySearch(T item)
        {
            return this.InnerList.BinarySearch(item);
        }
        /// <summary>
        /// Searches the entire sorted <see cref="ReadOnlyList{T}"/> for an element using the specified comparer
        /// and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate.  The value can be <see langword="null"/> for reference types.</param>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements,
        ///     or <see langword="null"/> to use the default comparer <see cref="Comparer{T}.Default"/>.
        /// </param>
        /// <returns>
        ///     The zero-based index of <paramref name="item"/> in the sorted <see cref="ReadOnlyList{T}"/>, if
        ///     <paramref name="item"/> is found; otherwise, a negative number that is the bitwise complement of the index of
        ///     the next element that is larger than <paramref name="item"/> or, if there is no larger element, the bitwise
        ///     complement of <see cref="ReadOnlyList{T}.Count"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}.Default"/> cannot find an implementation of the <see cref="IComparable{T}"/>
        ///     interface or the <see cref="IComparable"/> interface for type <typeparamref name="T"/>.
        /// </exception>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return this.InnerList.BinarySearch(item, comparer);
        }
        /// <summary>
        /// Searches a range of elements in the sorted <see cref="ReadOnlyList{T}"/> for an element using the specified comparer
        /// and returns the zero-based index of the element.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range to search.</param>
        /// <param name="count">The length of the range to search.</param>
        /// <param name="item">The object to locate.  The value can be <see langword="null"/> for reference types.</param>
        /// <param name="comparer">
        ///     The <see cref="IComparer{T}"/> implementation to use when comparing elements,
        ///     or <see langword="null"/> to use the default comparer <see cref="Comparer{T}.Default"/>.
        /// </param>
        /// <returns>
        ///     The zero-based index of <paramref name="item"/> in the sorted <see cref="ReadOnlyList{T}"/>, if
        ///     <paramref name="item"/> is found; otherwise, a negative number that is the bitwise complement of the index of
        ///     the next element that is larger than <paramref name="item"/> or, if there is no larger element, the bitwise
        ///     complement of <see cref="ReadOnlyList{T}.Count"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> and <paramref name="count"/> do not denote a valid range in the <see cref="ReadOnlyList{T}"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0 -or- <paramref name="count"/> is less than 0.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The default comparer <see cref="Comparer{T}.Default"/> cannot find an implementation of the <see cref="IComparable{T}"/>
        ///     interface or the <see cref="IComparable"/> interface for type <typeparamref name="T"/>.
        /// </exception>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            return this.InnerList.BinarySearch(index, count, item, comparer);
        }
        /// <summary>
        /// Determines whether an element is in the <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="ReadOnlyList{T}"/>.  The value can be null for reference types.
        /// </param>
        public bool Contains(T item)
        {
            return this.InnerList.Contains(item);
        }
        /// <summary>
        /// Converts the elements in the current <see cref="ReadOnlyList{T}"/> to another type, and returns a list containing
        /// the converted elements.
        /// </summary>
        /// <typeparam name="TOutput">The <see cref="Type"/> of the elements of the target array.</typeparam>
        /// <param name="converter">
        ///     A <see cref="Converter{TInput, TOutput}"/> delegate the converts each elements from one type to another type.
        /// </param>
        /// <returns>A <see cref="List{T}"/> of the target type containing the converted elements from the current <see cref="ReadOnlyList{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
        public List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            return this.InnerList.ConvertAll(converter);
        }
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
            return this .InnerList.FindAll(ToPredicate(match));
        }
        IList<T> ISearchableList<T>.FindAll(Func<T, bool> match) => this.FindAll(match);
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
            return this .InnerList.FindIndex(ToPredicate(match));
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
            return this .InnerList.FindIndex(startIndex, count, ToPredicate(match));
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
        IList<T> ISearchableList<T>.GetRange(int index, int count) => this.GetRange(index, count);
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first occurrence
        /// within the entire <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="item">
        ///     The object to locate in the <see cref="ReadOnlyList{T}"/>.  
        ///     The value can be <see langword="null"/> for reference types.
        /// </param>
        /// <returns>
        ///     The zero-based index of the first occurrence of <paramref name="item"/> within the entire
        ///     <see cref="ReadOnlyList{T}"/>, if found; otherwise -1.
        /// </returns>
        public int IndexOf(T item)
        {
            return this.InnerList.IndexOf(item);
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
        /// <summary>
        /// Copies the elements of the <see cref="ReadOnlyList{T}"/> to a new array.
        /// </summary>
        /// <returns>
        ///     An array containing copies of the elements of the <see cref="ReadOnlyList{T}"/>.
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

        private static Predicate<T> ToPredicate(Func<T, bool> func)
        {
            return new Predicate<T>(func);
        }

        #endregion

        #region ENUMERATORS
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <returns>A <see cref="List{T}.Enumerator"/> for the <see cref="ReadOnlyList{T}"/>.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region OPERATORS
        public static explicit operator ReadOnlyCollection<T>(ReadOnlyList<T> readOnly)
        {
            return new ReadOnlyCollection<T>(readOnly.InnerList);
        }
        public static explicit operator ReadOnlyList<T>(ReadOnlyCollection<T> collection)
        {
            return new ReadOnlyList<T>(collection);
        }
        public static explicit operator ReadOnlyList<T>(List<T> list)
        {
            return new ReadOnlyList<T>(list);
        }

        #endregion

        #region INTERFACE EXPLICIT PROPERTIES
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => ((ICollection)this.InnerList).SyncRoot;

        #endregion

        #region INTERFACE EXPLICIT METHODS
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this.InnerList).CopyTo(array, index);
        }

        #endregion
    }
}
