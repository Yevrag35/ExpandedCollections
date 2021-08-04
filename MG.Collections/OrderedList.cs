using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.Collections
{
    public class OrderedList<T> : IList<T>
    {
        private List<T> _list;

        #region INDEXERS
        public T this[int index]
        {
            get => _list[index];
            set
            {

            }
        }

        #endregion

        #region PROPERTIES
        public int Capacity => _list.Capacity;
        public IComparer<T> Comparer { get; }
        public int Count => _list.Count;
        public bool IsReadOnly => false;

        #endregion

        public OrderedList()
            : this(0, Comparer<T>.Default)
        {
        }

        public OrderedList(IComparer<T> comparer)
            : this(0, comparer)
        {
        }
        
        public OrderedList(int capacity)
            : this(capacity, !typeof(T).Equals(typeof(string))
                ? Comparer<T>.Default
                : (IComparer<T>)StringComparer.CurrentCulture)
        {
        }

        public OrderedList(int capacity, IComparer<T> comparer)
        {
            _list = new List<T>(capacity);
            this.Comparer = comparer;
        }

        public OrderedList(IEnumerable<T> items)
            : this(items, Comparer<T>.Default)
        {
        }

        public OrderedList(IEnumerable<T> items, IComparer<T> comparer)
        {
            _list = new List<T>(items);
            _list.Sort(comparer);
        }
        
        public int Add(T item)
        {
            for (int i = 0; i < _list.Count; i++)
            {
                if (Comparer.Compare(item, _list[i]) < 0)
                {
                    this.Insert(i, item);
                    return i;
                }
            }

            _list.Add(item);
            return this.Count - 1;
        }
        void ICollection<T>.Add(T item)
        {
            _ = this.Add(item);
        }
        public void AddRange(IEnumerable<T> items)
        {
            if (null == items || !items.Any())
                return;

            // First order the Enumerable
            IEnumerable<T> ordered = items.OrderBy(e => e, this.Comparer);
            T first = ordered.First();
            int index = this.Add(first) + 1;
            if (index == this.Count)
            {
                _list.AddRange(ordered.Skip(1));
            }
            else
            {
                int i = index;
                foreach (T item in ordered.Skip(1))
                {
                    _list.Insert(i, item);
                    i++;
                }
            }
        }
        public void Clear()
        {
            _list.Clear();
        }
        public bool Contains(T item)
        {
            return _list.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }
        public IEnumerator<T> GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        public int IndexOf(T item)
        {
            return _list.IndexOf(item);
        }
        private void Insert(int index, T item)
        {
            _list.Insert(index, item);
        }
        void IList<T>.Insert(int index, T item)
        {
            this.Add(item);
        }
        public bool Remove(T item)
        {
            return _list.Remove(item);
        }
        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
