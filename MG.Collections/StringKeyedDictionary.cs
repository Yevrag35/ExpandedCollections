using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MG.Collections.Exceptions;

namespace MG.Collections
{
    public class StringKeyedDictionary<T> : IDictionary<string, T>, IReadOnlyDictionary<string, T>, IDictionary
    {
        #region PRIVATE FIELDS
        private Dictionary<string, T> _dict;
        private bool _isReadOnly;

        #endregion

        #region PROPERTIES
        public int Count => _dict.Count;
        public bool IsReadOnly => _isReadOnly;
        public ICollection<string> Keys => _dict.Keys;
        public ICollection<T> Values => _dict.Values;

        #endregion

        #region INDEXERS
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

        #endregion

        #region CONSTRUCTORS

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

        #endregion

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

        #region DICTIONARY METHODS
        public void Add(string key, T value)
        {
            this.Validate();
            _dict.Add(key, value);
        }
        public void Clear()
        {
            this.Validate();
            _dict.Clear();
        }
        public bool ContainsKey(string key) => _dict.ContainsKey(key);
        public bool Remove(string key)
        {
            this.Validate();
            return _dict.Remove(key);
        }
        public bool TryGetValue(string key, out T value) => _dict.TryGetValue(key, out value);

        #endregion

        #region NON-DICTIONARY METHODS
        public static StringKeyedDictionary<T> NewReadOnly(IDictionary<string, T> dictionary, bool ignoreCase)
        {
            IEqualityComparer<string> comparer = null;
            if (ignoreCase)
                comparer = new IgnoreCase();

            return NewReadOnly(dictionary, comparer);
        }
        public static StringKeyedDictionary<T> NewReadOnly(IDictionary<string, T> dictionary, IEqualityComparer<string> comparer)
        {
            var dict = new StringKeyedDictionary<T>(dictionary, comparer)
            {
                _isReadOnly = true
            };
            return dict;
        }

        #endregion

        #region ENUMERATORS
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => _dict.GetEnumerator();
        IDictionaryEnumerator IDictionary.GetEnumerator() => _dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();

        #endregion

        #region INTERFACE EXPLICIT MEMBERS

        #region PROPERTIES

        bool IDictionary.IsFixedSize => _isReadOnly;
        bool ICollection.IsSynchronized => false;
        ICollection IDictionary.Keys => _dict.Keys;
        IEnumerable<string> IReadOnlyDictionary<string, T>.Keys => this.Keys;
        object ICollection.SyncRoot => ((ICollection)_dict).SyncRoot;
        ICollection IDictionary.Values => _dict.Values;
        IEnumerable<T> IReadOnlyDictionary<string, T>.Values => this.Values;

        #endregion

        #region METHODS

        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item)
        {
            this.Validate();
            ((ICollection<KeyValuePair<string, T>>)_dict).Add(item);
        }
        void IDictionary.Add(object key, object value)
        {
            this.Validate();
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
        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item)
        {
            this.Validate();
            return ((ICollection<KeyValuePair<string, T>>)_dict).Remove(item);
        }
        void IDictionary.Remove(object key)
        {
            this.Validate();
            if (this.TryGetKey(key, out string strKey))
            {
                _dict.Remove(strKey);
            }
        }

        #endregion

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
        private void Validate()
        {
            if (_isReadOnly)
                throw new ReadOnlyException();
        }

        #endregion
    }
}
