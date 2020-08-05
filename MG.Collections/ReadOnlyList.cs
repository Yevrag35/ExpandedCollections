using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.Collections
{
    /// <summary>
    /// A strongly typed, read-only list of objects that can be accessed by index and provides advanced methods for 
    /// searching and sorting through its contents.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadOnlyList<T> : ICollection, ISearchableList<T>, ISortableList<T>
    {
        /// <summary>
        /// The internal backing <see cref="List{T}"/> that all methods of <see cref="ReadOnlyList{T}"/> invoke against.
        /// </summary>
        protected List<T> InnerList { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <returns>The number of elements contained within the <see cref="ReadOnlyList{T}"/>.</returns>
        public int Count => this.InnerList.Count;

        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => ((ICollection)this.InnerList).SyncRoot;

        /// <summary>
        /// Gets the element at the specified index.  If <paramref name="index"/> is negative, then searching is done in reverse order.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index or the default value of <typeparamref name="T"/> if the index is out of range.</returns>
        public T this[int index]
        {
            get
            {
                if (index < 0)
                    index = this.Count + index;

                if (index >= 0 && index < this.Count)
                    return this.InnerList[index];

                else
                    return default;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlyList{T}"/> that is empty
        /// and has the default capacity.
        /// </summary>
        protected ReadOnlyList() => this.InnerList = new List<T>();
        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlyList{T}"/> that is empty
        /// and has the specified capacity.
        /// </summary>
        /// <param name="capacity">The number of new elements the list can initially store.</param>
        protected ReadOnlyList(int capacity) => this.InnerList = new List<T>(capacity);
        /// <summary>
        /// Initializes a new instance of <see cref="ReadOnlyList{T}"/> that contains the elements
        /// copied from the specified collection.
        /// </summary>
        /// <param name="items">The collection whose elements are copied to the new list.</param>
        public ReadOnlyList(IEnumerable<T> items) => this.InnerList = new List<T>(items);

        #region METHODS
        /// <summary>
        /// Searches the entire sorted <see cref="ReadOnlyList{T}"/> for an element using the default comparer
        /// and returns the zero-based index of the element.
        /// </summary>
        /// <param name="item">The object to locate.  The value can be null for reference types.</param>
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
        public int BinarySearch(T item) => this.InnerList.BinarySearch(item);

        /// <summary>
        /// Determines whether an element is in the <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="item">
        /// The object to locate in the <see cref="ReadOnlyList{T}"/>.  The value can be null for reference types.
        /// </param>
        public bool Contains(T item) => this.InnerList.Contains(item);

        /// <summary>
        /// Converts the elements in the current <see cref="ReadOnlyList{T}"/> to another type, and returns a list containing
        /// the converted elements.
        /// </summary>
        /// <typeparam name="TOutput">The <see cref="Type"/> of the elements of the target array.</typeparam>
        /// <param name="converter">
        ///     A <see cref="Converter{TInput, TOutput}"/> delegate the converts each elements from one type to another type.
        /// </param>
        /// <returns>A <see cref="IList{T}"/> of the target type containing the converted elements from the current <see cref="ReadOnlyList{T}"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="converter"/> is <see langword="null"/>.</exception>
        public IList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => this.InnerList.ConvertAll(converter);

        /// <summary>
        ///     Determines whether the <see cref="ReadOnlyList{T}"/> contains elements that
        ///     match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the 
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
        public bool Exists(Predicate<T> match) => this.InnerList.Exists(match);

        /// <summary>
        ///     Searches for an element that matches the conditions defined by the specified
        ///     predicate, and returns the first occurrence within the entire <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="match">
        ///     The <see cref="Predicate{T}"/> delegate that defines the conditions of the
        ///     elements to search for.
        /// </param>
        /// <returns>
        ///     The first element that matches the conditions if found; otherwise the default value for <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public T Find(Predicate<T> match) => this.InnerList.Find(match);

        /// <summary>
        /// Retrieves all of the elements that match the conditions defined by the specified predicate.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     An <see cref="IList{T}"/> containing all of the elements that match the conditions if found; 
        ///     otherwise, an empty list.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public IList<T> FindAll(Predicate<T> match) => this.InnerList.FindAll(match);

        /// <summary>
        /// Searches for an element that match the conditions defined by the specified predicate, and returns the zero-based
        /// index of the first occurrence within the entire <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     The zero-based index of the first occurrence of an element that matches the conditions defined
        ///     by <paramref name="match"/> if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public int FindIndex(Predicate<T> match) => this.InnerList.FindIndex(match);

        /// <summary>
        /// Searches for an element that match the conditions defined by the specified predicate, and returns the zero-based
        /// index of the first occurrence within the range of elements in the <see cref="ReadOnlyList{T}"/> that extends from the 
        /// specified index to the last element.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     The zero-based index of the first occurrence of an element that matches the conditions defined
        ///     by <paramref name="match"/> if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public int FindIndex(int startIndex, Predicate<T> match) => this.InnerList.FindIndex(startIndex, match);

        /// <summary>
        /// Searches for an element that match the conditions defined by the specified predicate, and returns the zero-based
        /// index of the first occurrence within the range of elements in the <see cref="ReadOnlyList{T}"/> that starts at the 
        /// specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="startIndex">The zero-based starting index of the search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <param name="match">The <see cref="Predicate{T}"/> delegate the defines the conditions of the elements to search for.</param>
        /// <returns>
        ///     The zero-based index of the first occurrence of an element that matches the conditions defined
        ///     by <paramref name="match"/> if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="match"/> is <see langword="null"/>.</exception>
        public int FindIndex(int startIndex, int count, Predicate<T> match) => this.InnerList.FindIndex(startIndex, count, match);


        public T FindLast(Predicate<T> match) => this.InnerList.FindLast(match);
        public int FindLastIndex(Predicate<T> match) => this.InnerList.FindLastIndex(match);
        public int FindLastIndex(int startIndex, Predicate<T> match) => this.InnerList.FindLastIndex(startIndex, match);
        public int FindLastIndex(int startIndex, int count, Predicate<T> match) => this.InnerList.FindLastIndex(startIndex, count, match);
        public IList<T> GetRange(int index, int count) => this.InnerList.GetRange(index, count);

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
        public int IndexOf(T item) => this.InnerList.IndexOf(item);
        public int IndexOf(T item, int index) => this.InnerList.IndexOf(item, index);
        public int IndexOf(T item, int index, int count) => this.InnerList.IndexOf(item, index, count);
        public int LastIndexOf(T item) => this.InnerList.LastIndexOf(item);
        public int LastIndexOf(T item, int index) => this.InnerList.LastIndexOf(item, index);
        public int LastIndexOf(T item, int index, int count) => this.InnerList.LastIndexOf(item, index, count);
        public void Reverse() => this.InnerList.Reverse();
        public void Reverse(int index, int count) => this.InnerList.Reverse(index, count);
        public void Sort() => this.InnerList.Sort();
        public void Sort(IComparer<T> comparer) => this.InnerList.Sort(comparer);
        public T[] ToArray() => this.InnerList.ToArray();
        public bool TrueForAll(Predicate<T> match) => this.InnerList.TrueForAll(match);

        #endregion

        #region ENUMERATORS
        public IEnumerator<T> GetEnumerator() => this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region INTERFACE EXPLICIT METHODS
        void ICollection.CopyTo(Array array, int index) => ((ICollection)this.InnerList).CopyTo(array, index);

        #endregion
    }
}
