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
    public class SearchableReadOnlyList<T> : ICollection, ISearchableList<T>, ISortableList<T>
    {
        /// <summary>
        /// The inner <see cref="List{T}"/> holding the elements of the <see cref="SearchableReadOnlyList{T}"/>.
        /// </summary>
        protected List<T> InnerList { get; }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="SearchableReadOnlyList{T}"/>.
        /// </summary>
        /// <returns>The number of elements contained within the <see cref="SearchableReadOnlyList{T}"/>.</returns>
        public int Count => this.InnerList.Count;

        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => ((ICollection)this.InnerList).SyncRoot;

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        public T this[int index] => this.InnerList[index];

        /// <summary>
        /// Initializes a new instance of <see cref="SearchableReadOnlyList{T}"/> that is empty
        /// and has the default capacity.
        /// </summary>
        protected SearchableReadOnlyList() => this.InnerList = new List<T>();
        /// <summary>
        /// Initializes a new instance of <see cref="SearchableReadOnlyList{T}"/> that is empty
        /// and has the specified capacity.
        /// </summary>
        /// <param name="capacity">The number of new elements the list can initially store.</param>
        protected SearchableReadOnlyList(int capacity) => this.InnerList = new List<T>(capacity);
        /// <summary>
        /// Initializes a new instance of <see cref="SearchableReadOnlyList{T}"/> that contains the elements
        /// copied from the specified collection.
        /// </summary>
        /// <param name="items">The collection whose elements are copied to the new list.</param>
        public SearchableReadOnlyList(IEnumerable<T> items) => this.InnerList = new List<T>(items);

        public void CopyTo(Array array, int index) => ((ICollection)this.InnerList).CopyTo(array, index);
        public IEnumerator<T> GetEnumerator() => this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public int BinarySearch(T item) => this.InnerList.BinarySearch(item);
        public bool Contains(T item) => this.InnerList.Contains(item);
        public IList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter) => this.InnerList.ConvertAll(converter);
        public bool Exists(Predicate<T> match) => this.InnerList.Exists(match);
        public T Find(Predicate<T> match) => this.InnerList.Find(match);
        public IList<T> FindAll(Predicate<T> match) => this.InnerList.FindAll(match);
        public int FindIndex(Predicate<T> match) => this.InnerList.FindIndex(match);
        public int FindIndex(int startIndex, Predicate<T> match) => this.InnerList.FindIndex(startIndex, match);
        public int FindIndex(int startIndex, int count, Predicate<T> match) => this.InnerList.FindIndex(startIndex, count, match);
        public T FindLast(Predicate<T> match) => this.InnerList.FindLast(match);
        public int FindLastIndex(Predicate<T> match) => this.InnerList.FindLastIndex(match);
        public int FindLastIndex(int startIndex, Predicate<T> match) => this.InnerList.FindLastIndex(startIndex, match);
        public int FindLastIndex(int startIndex, int count, Predicate<T> match) => this.InnerList.FindLastIndex(startIndex, count, match);
        public IList<T> GetRange(int index, int count) => this.InnerList.GetRange(index, count);
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
    }
}
