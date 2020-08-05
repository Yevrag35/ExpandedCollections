using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.Collections
{
    public class StringKeyedDictionary<T> : IDictionary<string, T>
    {
        private Dictionary<string, T> _dict;

        public ICollection<string> Keys => _dict.Keys;
        public ICollection<T> Values => _dict.Values;

        public int Count => _dict.Count;
        bool ICollection<KeyValuePair<string, T>>.IsReadOnly => ((ICollection<KeyValuePair<string, T>>)_dict).IsReadOnly;

        public T this[string key]
        {
            get => _dict[key];
            set => _dict[key] = value;
        }

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
        public StringKeyedDictionary(IDictionary dictionary, IEqualityComparer<string> comparer)
        {
            _dict = new Dictionary<string, T>(dictionary.Count, comparer);
            dictionary.Keys.OfType<string>().Distinct
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
        public bool ContainsKey(string key) => _dict.ContainsKey(key);
        public bool Remove(string key) => _dict.Remove(key);
        public bool TryGetValue(string key, out T value) => _dict.TryGetValue(key, out value);
        void ICollection<KeyValuePair<string, T>>.Add(KeyValuePair<string, T> item) => ((ICollection<KeyValuePair<string, T>>)_dict).Add(item);
        public void Clear() => _dict.Clear();
        bool ICollection<KeyValuePair<string, T>>.Contains(KeyValuePair<string, T> item) => ((ICollection<KeyValuePair<string, T>>)_dict).Contains(item);
        void ICollection<KeyValuePair<string, T>>.CopyTo(KeyValuePair<string, T>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, T>>)_dict).CopyTo(array, arrayIndex);
        bool ICollection<KeyValuePair<string, T>>.Remove(KeyValuePair<string, T> item) => ((ICollection<KeyValuePair<string, T>>)_dict).Remove(item);
        public IEnumerator<KeyValuePair<string, T>> GetEnumerator() => _dict.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _dict.GetEnumerator();

        #endregion
    }
}
