using MG.Collections.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MG.Collections
{
    public class StringSortedList<T> : IDictionary<string, T>, IDictionary, IReadOnlyDictionary<string, T>
    {
        #region PRIVATE FIELDS
        private Func<T, string> _keySelector;
        private bool _isKeySelectorSet;
        private SortedList<string, T> _list;

        #endregion

        #region INDEXERS
        public T this[string key]
        {
            get { return _list[key]; }

            set { _list[key] = value; }
        }
        public T this[int index]
        {
            get
            {
                if (index >= 0)
                    return _list.Values[index];

                else
                {
                    int goHere = _list.Count + index;
                    return goHere >= 0 ? _list.Values[goHere] : default;
                }
            }
        }

        #endregion

        #region PROPERTIES
        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsKeySelectorSet
        {
            get { return _isKeySelectorSet; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IList<string> Keys
        {
            get { return _list.Keys; }
        }

        public IList<T> Values
        {
            get { return _list.Values; }
        }

        #endregion

        #region CONSTRUCTORS
        public StringSortedList()
            : this(0)
        {
        }
        public StringSortedList(Func<T, string> keySelector)
            : this()
        {
            _keySelector = keySelector;
            _isKeySelectorSet = true;
        }
        public StringSortedList(int capacity)
            : this(capacity, StringComparer.CurrentCultureIgnoreCase)
        {
        }
        public StringSortedList(int capacity, Func<T, string> keySelector)
            : this(capacity)
        {
            _keySelector = keySelector;
            _isKeySelectorSet = true;
        }
        public StringSortedList(IComparer<string> comparer)
            : this(0, comparer)
        {
        }
        public StringSortedList(IComparer<string> comparer, Func<T, string> keySelector)
            : this(comparer)
        {
            _keySelector = keySelector;
            _isKeySelectorSet = true;
        }
        public StringSortedList(int capacity, IComparer<string> comparer)
        {
            _list = new SortedList<string, T>(capacity, comparer);
        }
        public StringSortedList(int capacity, IComparer<string> comparer, Func<T, string> keySelector)
            : this(capacity, comparer)
        {
            _keySelector = keySelector;
            _isKeySelectorSet = true;
        }

        #endregion

        #region ENUMERATORS
        public IEnumerator<T> GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }
        IEnumerator<KeyValuePair<string, T>> IEnumerable<KeyValuePair<string, T>>.GetEnumerator()
        {
            return _list.GetEnumerator();
        }
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)_list).GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region METHODS
        public void Add(T value)
        {
            if (!_isKeySelectorSet)
                throw new KeySelectorNotSetException();

            string key = this.GetKey(value);
            this.Add(key, value);
        }
        public void Add(string key, T value)
        {
            _list.Add(key, value);
        }
        public void AddKeySelector(Func<T, string> keySelector)
        {
            if (_isKeySelectorSet)
                throw new KeySelectorNotSetException();

            _keySelector = keySelector;
            _isKeySelectorSet = true;
        }
        public void Clear()
        {
            _list.Clear();
        }
        public bool ContainsKey(string key)
        {
            return _list.ContainsKey(key);
        }
        public bool ContainsValue(T value)
        {
            return _list.ContainsValue(value);
        }
        private string GetKey(T value)
        {
            return _keySelector(value);
        }
        public bool Remove(string key)
        {
            return _list.Remove(key);
        }
        public bool TryGetValue(string key, out T value)
        {
            bool result = _list.TryGetValue(key, out T outVal);
            value = outVal;
            return result;
        }

        #endregion

        #region INTERFACE EXPLICITS
        object IDictionary.this[object key]
        {
            get { return ((IDictionary)_list)[key]; }
            set { ((IDictionary)_list)[key] = value; }
        }

        bool IDictionary.IsFixedSize
        {
            get { return ((IDictionary)_list).IsFixedSize; }
        }
        bool ICollection.IsSynchronized
        {
            get { return false; }
        }
        ICollection IDictionary.Keys
        {
            get { return ((IDictionary)_list).Keys; }
        }
        ICollection<string> IDictionary<string, T>.Keys
        {
            get { return this.Keys; }
        }
        IEnumerable<string> IReadOnlyDictionary<string, T>.Keys
        {
            get { return this.Keys; }
        }
        object ICollection.SyncRoot
        {
            get { return this; }
        }
        ICollection IDictionary.Values
        {
            get { return ((IDictionary)_list).Values; }
        }
        ICollection<T> IDictionary<string, T>.Values
        {
            get { return this.Values; }
        }
        IEnumerable<T> IReadOnlyDictionary<string, T>.Values
        {
            get { return this.Values; }
        }

        #region METHODS
        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item)
        {
            ((ICollection<KeyValuePair<string, T>>)_list).Add(item);
        }
        void IDictionary.Add(object key, object value)
        {
            ((IDictionary)_list).Add(key, value);
        }
        bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item)
        {
            return ((ICollection<KeyValuePair<string, T>>)_list).Contains(item);
        }
        bool IDictionary.Contains(object key)
        {
            return ((IDictionary)_list).Contains(key);
        }
        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<string, T>>)_list).CopyTo(array, arrayIndex);
        }
        void ICollection.CopyTo(Array array, int index)
        {
            ((IDictionary)_list).CopyTo(array, index);
        }
        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
        {
            return ((ICollection<KeyValuePair<string, T>>)_list).Remove(item);
        }
        void IDictionary.Remove(object key)
        {
            ((IDictionary)_list).Remove(key);
        }

        #endregion

        #endregion
    }
}
