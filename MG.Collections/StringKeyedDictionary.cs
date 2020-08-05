using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.Collections
{
    public class StringKeyedDictionary<T> : IDictionary<string, T>, IDictionary
    {
        private Dictionary<string, T> _dict;

        bool IDictionary.IsFixedSize => false;
        bool IDictionary.IsReadOnly => false;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => ((ICollection)_dict).SyncRoot;
        public ICollection<string> Keys => _dict.Keys;
        ICollection IDictionary.Keys => _dict.Keys;
        public ICollection<T> Values => _dict.Values;
        ICollection IDictionary.Values => _dict.Values;

        public int Count => _dict.Count;
        bool ICollection<KeyValuePair<string, T>>.IsReadOnly => ((ICollection<KeyValuePair<string, T>>)_dict).IsReadOnly;

        public T this[string key]
        {
            get => _dict[key];
            set => _dict[key] = value;
        }
        object IDictionary.this[object key]
        {
            get => this.TryGetKey(key, out string strKey) && _dict.TryGetValue(strKey, out T val) ? val : default;
            set
            {
                if (value is T tVal && this.TryGetKey(key, out string strKey))
                {
                    _dict[strKey] = tVal;
                }
            }
        }

        public StringKeyedDictionary() : this(true) { }
        public StringKeyedDictionary(bool ignoreCase) : this(ignoreCase, 0) { }
        public StringKeyedDictionary(bool ignoreCase, int capacity)
        {
            IEqualityComparer<string> comparer = null;
            if (ignoreCase)
                comparer = new IgnoreCase();

            _dict = new Dictionary<string, T>(capacity, comparer);
        }
        public StringKeyedDictionary(int capacity, IEqualityComparer<string> equalityComparer)
        {
            _dict = new Dictionary<string, T>(capacity, equalityComparer);
        }

        public StringKeyedDictionary(IDictionary<string, T> dictionary) : this(dictionary, new IgnoreCase()) { }
        public StringKeyedDictionary(IDictionary<string, T> dictionary, IEqualityComparer<string> comparer)
        {
            _dict = new Dictionary<string, T>(dictionary, comparer);
        }

        #region EQUALITY COMPARERS
        private class IgnoreCase : IEqualityComparer<string>
        {
            private StringComparison _comparison;

            public IgnoreCase(StringComparison comparison = StringComparison.CurrentCultureIgnoreCase)
            {
                _comparison = comparison;
            }

            public bool Equals(string x, string y)
            {
                if (x == null && y == null)
                    return true;

                else if ((x == null && y != null) || (x != null && y == null))
                    return false;

                else
                    return x.Equals(y, _comparison);
            }
            public int GetHashCode(string x)
            {
                if (_comparison == StringComparison.CurrentCultureIgnoreCase)
                    return x.ToLower().GetHashCode();

                else
                    return x.GetHashCode();
            }
        }

        #endregion

        #region METHODS
        public void Add(string key, T value) => _dict.Add(key, value);
        public void Clear() => _dict.Clear();
        public bool ContainsKey(string key) => _dict.ContainsKey(key);
        public bool Remove(string key) => _dict.Remove(key);
        public bool TryGetValue(string key, out T value) => _dict.TryGetValue(key, out value);
        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item) => ((ICollection<KeyValuePair<string, T>>)_dict).Add(item);
        void IDictionary.Add(object key, object value)
        {
            if (value is T tVal && this.TryGetKey(key, out string strKey))
            {
                _dict.Add(strKey, tVal);
            }
        }
        bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item) => ((ICollection<KeyValuePair<string, T>>)_dict).Contains(item);
        bool IDictionary.Contains(object key)
        {
            return this.TryGetKey(key, out string strKey) ? _dict.ContainsKey(strKey) : false;
        }
        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, T>>)_dict).CopyTo(array, arrayIndex);
        void ICollection.CopyTo(Array array, int index) => ((ICollection)_dict).CopyTo(array, index);
        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item) => ((ICollection<KeyValuePair<string, T>>)_dict).Remove(item);
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => _dict.GetEnumerator();
        IDictionaryEnumerator IDictionary.GetEnumerator() => _dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();
        void IDictionary.Remove(object key)
        {
            if (this.TryGetKey(key, out string strKey))
            {
                _dict.Remove(strKey);
            }
        }

        #endregion

        #region BACKEND METHODS
        private bool TryGetKey(object key, out string strKey)
        {
            strKey = null;
            if (key is string already)
                strKey = already;

            else if (key is IConvertible icon)
                strKey = Convert.ToString(icon);

            return strKey != null;
        }

        #endregion
    }
}
