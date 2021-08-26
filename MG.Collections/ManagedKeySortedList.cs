using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MG.Collections.Extensions;

using Strings = MG.Collections.Properties.Resources;

namespace MG.Collections
{
    public class ManagedKeySortedList<TKey, TValue> : IDictionary<TKey, TValue>, IList<TValue>, IDictionary,
        IList, IEnumerable<TValue>
    {
        #region PRIVATE FIELDS/CONSTANTS
        private readonly SortedList<TKey, TValue> InnerList;

        #endregion

        #region INDEXERS
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        /// <returns>
        ///     The value associated with the specified key. If the specified key is not found,
        ///     the default value of <typeparamref name="TValue"/> is returned;
        ///     a set operation creates a new element using the specified key.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        public TValue this[TKey key]
        {
            get => this.GetValueOrDefault(key);
            set => this.InnerList[key] = value;
        }

        /// <summary>
        /// Gets the element at the specified index.  The 'set' accessor is not supported on the
        /// <see cref="ManagedKeySortedList{TKey, TValue}"/> class and will throw a
        /// <see cref="NotSupportedException"/> if attempted.
        /// </summary>
        /// <remarks>
        ///     When negative indicies are specified, instead of starting at the zero-based position, 
        ///     it will begin at the index of the last element of the 
        ///     <see cref="ManagedKeySortedList{TKey, TValue}"/> and count backwards.
        /// </remarks>
        /// <param name="index">
        ///     The zero-based index (positive or negative) of the element to get or set.
        /// </param>
        /// <returns>
        ///     The element at the specified or calculated index.
        /// </returns>
        public TValue this[int index]
        {
            get => this.InnerList.Values.GetByIndex(index);
            set => throw new NotSupportedException(Strings.NotSupportedException_SortedListSet);
        }

        #region NON-GENERIC DICTIONARY INDEXER
        object IDictionary.this[object key]
        {
            get
            {
                if (key is TKey tKey)
                    return this[tKey];

                else
                    return null;
            }
            set => throw new NotSupportedException(Strings.NotSupportedException_SortedListSet);
        }

        #endregion

        #region NON-GENERIC LIST INDEXER
        object IList.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException(Strings.NotSupportedException_SortedListSet);
        }

        #endregion

        #endregion

        #region PROPERTIES
        /// <summary>
        /// Gets or sets the number of elements that the <see cref="ManagedKeySortedList{TKey, TValue}"/>
        /// can contain.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <see cref="ManagedKeySortedList{TKey, TValue}.Capacity"/> is set to a value that is less
        ///     than <see cref="ManagedKeySortedList{TKey, TValue}.Count"/>.
        /// </exception>
        /// <exception cref="OutOfMemoryException">There is not enough memory available on the system.</exception>
        public int Capacity
        {
            get => this.InnerList.Capacity;
            set => this.InnerList.Capacity = value;
        }
        /// <summary>
        /// Gets the number of elements contained in the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        public int Count => this.InnerList.Count;
        /// <summary>
        /// Gets a value indicating whether the <see cref="ManagedKeySortedList{TKey, TValue}"/> has a fixed size.
        /// </summary>
        /// <returns>
        ///     By default, this will return <see langword="false"/>, however, this value can be overridden
        ///     in derived classes.
        /// </returns>
        public bool IsFixedSize { get; protected set; }
        /// <summary>
        /// Gets a value whether the <see cref="ManagedKeySortedList{TKey, TValue}"/> is read-only.
        /// </summary>
        /// <remarks>
        ///     By default, this will always be <see langword="false"/>, but the value can be overriden in
        ///     derived types.
        /// </remarks>
        public bool IsReadOnly { get; protected set; }
        /// <summary>
        /// Gets a value indicating whether access to the <see cref="ManagedKeySortedList{TKey, TValue}"/>
        /// is synchronized (thread safe).
        /// </summary>
        /// <returns>
        ///     This value will always return <see langword="false"/>.
        /// </returns>
        public bool IsSynchronized => false;
        /// <summary>
        /// Gets a collection containing the keys in the <see cref="ManagedKeySortedList{TKey, TValue}"/>
        /// in sorted order.
        /// </summary>
        public IList<TKey> Keys => this.InnerList.Keys;
        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <returns>
        ///     The current instance.
        /// </returns>
        public object SyncRoot => this;

        #region PROTECTED

        /// <summary>
        /// The function that is used to retrieve a value of type <typeparamref name="TKey"/>
        /// from a given <typeparamref name="TValue"/> that is used as the sorted key.
        /// </summary>
        protected Func<TValue, TKey> KeySelector { get; }

        #endregion

        #region IDICTIONARY EXPLICIT PROPERTIES
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => this.Keys;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => this.InnerList.Values;

        #endregion

        #region NON-GENERIC IDICTIONARY PROPERTIES
        ICollection IDictionary.Keys => this.Keys.Cast<object>().ToArray();
        ICollection IDictionary.Values => this.Cast<object>().ToArray();

        #endregion

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// The default constructor with the specified key selecting function.
        /// </summary>
        /// <param name="keySelector">
        ///     The function that is executing on each <typeparamref name="TValue"/>
        ///     in order to retrieve the <typeparamref name="TKey"/> value.
        /// </param>
        public ManagedKeySortedList(Func<TValue, TKey> keySelector)
        {
            this.KeySelector = keySelector;
            this.InnerList = new SortedList<TKey, TValue>();
        }

        #endregion

        #region GENERIC LIST METHODS
        /// <summary>
        /// Adds an item to the end of the list.
        /// </summary>
        /// <param name="item">The item to be added to the end of the <see cref="ManagedKeySortedList{TKey, TItem}"/>.</param>
        /// <exception cref="ArgumentNullException">
        ///     <see cref="KeySelector"/> returned a 
        ///     <see langword="null"/> key.
        /// </exception>
        /// /// <exception cref="InvalidOperationException">
        ///     The <see cref="KeySelector"/> threw an <see cref="Exception"/> when fed
        ///     <paramref name="item"/>.
        /// </exception>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="item"/> was successfully added to the 
        ///     <see cref="ManagedKeySortedList{TKey, TItem}"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Add(TValue item)
        {
            bool result = false;
            TKey key = this.GetKey(item);
            if (!this.InnerList.ContainsKey(key))
            {
                this.InnerList.Add(key, item);
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Removes all elements from the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        public void Clear()
        {
            this.InnerList.Clear();
        }

        /// <summary>
        /// Determines whether the <see cref="ManagedKeySortedList{TKey, TValue}"/> contains a specific value.
        /// </summary>
        /// <param name="item">
        ///     The value to locate in the <see cref="ManagedKeySortedList{TKey, TValue}"/>. The value
        ///     can be <see langword="null"/> for reference types.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="ManagedKeySortedList{TKey, TValue}"/> contains an element with
        ///     the specified value; otherwise, <see langword="false"/>.
        /// </returns>
        public bool Contains(TValue item)
        {
            return this.InnerList.ContainsValue(item);
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <param name="item">
        ///     The object to remove from the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the item is successfully removed; otherwise, <see langword="false"/>. 
        ///     This method also returns false if item was not found in the 
        ///     <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="item"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        ///     The <see cref="KeySelector"/> threw an <see cref="Exception"/> when fed
        ///     <paramref name="item"/>.
        /// </exception>
        public bool Remove(TValue item)
        {
            return this.InnerList.Remove(this.GetKey(item));
        }

        #endregion

        #region GENERIC DICTIONARY METHODS

        /// <summary>
        /// Determines whether the <see cref="ManagedKeySortedList{TKey, TValue}"/> contains a specific key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="ManagedKeySortedList{TKey, TValue}"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="ManagedKeySortedList{TKey, TValue}"/> contains 
        ///     an element with the specified key; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        public bool ContainsKey(TKey key)
        {
            return this.InnerList.ContainsKey(key);
        }

        /// <summary>
        /// Copies the entire <see cref="ManagedKeySortedList{TKey, TValue}"/> to a compatible one-dimensional
        ///     array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied
        ///     from the <see cref="ManagedKeySortedList{TKey, TValue}"/>. The <see cref="Array"/> must have 
        ///     zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="ArgumentNullException"><paramref name="array"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">
        ///     The number of elements in the source <see cref="ManagedKeySortedList{TKey, TValue}"/> is greater
        ///     than the available space from <paramref name="arrayIndex"/> to the end of the destination array.
        /// </exception>
        public void CopyTo(TValue[] array, int arrayIndex)
        {

        }

        /// <summary>
        /// Tries to get the value associated with the specified key in the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>
        ///     A <typeparamref name="TValue"/> instance. When the method is successful, the returned object is the
        ///     value associated with the specified key. When the method fails,
        ///     it returns the default value for <typeparamref name="TValue"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        public TValue GetValueOrDefault(TKey key)
        {
            return this.GetValueOrDefault(key, default);
        }

        /// <summary>
        /// Tries to get the value associated with the specified key in the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="defaultValue">
        ///     The default value to return when the <see cref="ManagedKeySortedList{TKey, TValue}"/> 
        ///     cannot find a value associated with the specified key.
        /// </param>
        /// <returns>
        ///     A <typeparamref name="TValue"/> instance. When the method is successful, the returned object is the
        ///     value associated with the specified key. When the method fails,
        ///     it returns <paramref name="defaultValue"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        public TValue GetValueOrDefault(TKey key, TValue defaultValue)
        {
            return this.TryGetValue(key, out TValue result)
                ? result
                : defaultValue;
        }

        /// <summary>
        /// Searches for the specified value and returns the zero-based index of the first
        /// occurrence within the entire <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <param name="value">
        ///     The value to locate in the <see cref="ManagedKeySortedList{TKey, TValue}"/>. The value
        ///     can be <see langword="null"/> for reference types.
        /// </param>
        /// <returns>
        ///     The zero-based index of the first occurrence of value within the entire 
        ///     <see cref="ManagedKeySortedList{TKey, TValue}"/>, if found; otherwise, -1.
        /// </returns>
        public int IndexOf(TValue value)
        {
            return this.InnerList.IndexOfValue(value);
        }

        /// <summary>
        /// Removes the element with the specified key from the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        ///    <see langword="true"/> if the element is successfully removed; otherwise, <see langword="false"/>. This method also
        ///    returns false if <paramref name="key"/> was not found in the original <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </returns>
        public bool Remove(TKey key)
        {
            if (null == key)
                return false;

            return this.InnerList.Remove(key);
        }

        /// <summary>
        /// Removes the element at the specified index of the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0. -or- <paramref name="index"/> is equal to or greater than
        ///     <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </exception>
        public void RemoveAt(int index)
        {
            this.InnerList.RemoveAt(index);
        }

        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="ManagedKeySortedList{TKey, TValue}"/>,
        ///     if that number is less than a threshold value.
        /// </summary>
        public void TrimExcess()
        {
            this.InnerList.TrimExcess();
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">
        ///     When this method returns, the value associated with the specified key, if the
        ///     key is found; otherwise, the default value for the type of the value parameter.
        ///     This parameter is passed uninitialized.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="ManagedKeySortedList{TKey, TValue}"/> contains 
        ///     an element with the specified key; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return this.InnerList.TryGetValue(key, out value);
        }

        #region IDICTIONARY EXPLICITS
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            // *NOTE* - This interface-explicit will add a non-functioned 'key' to the list.
            this.InnerList.Add(key, value);
        }
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.InnerList).Add(item);
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.ContainsKey(item.Key);
        }
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)this.InnerList).CopyTo(array, arrayIndex);
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        #endregion

        #region ILIST EXPLICITS
        void ICollection<TValue>.Add(TValue item)
        {
            _ = this.Add(item);
        }
        void IList<TValue>.Insert(int index, TValue item)
        {
            _ = this.Add(item);
        }

        #endregion

        #endregion

        #region NON-GENERIC DICTIONARY METHODS
        void IDictionary.Add(object key, object value)
        {
            if (key is TKey tKey && value is TValue tVal)
            {
                this.InnerList.Add(tKey, tVal);
            }
        }
        bool IDictionary.Contains(object key)
        {
            return key is TKey tKey && this.ContainsKey(tKey);
        }
        void IDictionary.Remove(object key)
        {
            if (key is TKey tKey)
            {
                _ = this.Remove(tKey);
            }
        }

        #endregion

        #region NON-GENERIC LIST METHODS
        int IList.Add(object value)
        {
            if (value is TValue tVal)
            {
                TKey key = this.KeySelector(tVal);
                this.InnerList.Add(key, tVal);
                return this.InnerList.IndexOfKey(key);
            }
            else
            {
                return -1;
            }
        }
        bool IList.Contains(object value)
        {
            return value is TValue tVal && this.Contains(tVal);
        }
        void ICollection.CopyTo(Array array, int index)
        {
            if (array is TValue[] valArr)
            {
                this.CopyTo(valArr, index);
            }
            else
            {
                throw new InvalidCastException(nameof(array));
            }
        }
        int IList.IndexOf(object value)
        {
            return value is TValue tVal
                ? this.IndexOf(tVal)
                : -1;
        }
        void IList.Insert(int index, object value)
        {
            if (value is TValue tVal)
            {
                _ = this.Add(tVal);
            }
        }
        void IList.Remove(object value)
        {
            if (value is TValue tVal)
            {
                _ = this.Remove(tVal);
            }
        }

        #endregion

        #region ENUMERATORS
        public virtual IEnumerator<TValue> GetEnumerator()
        {
            return this.InnerList.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return this.InnerList.GetEnumerator();
        }
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new NonGenericDictionaryEnumerator(this.InnerList.GetEnumerator());
        }

        #endregion

        #region KEY METHODS
        /// <summary>
        /// Executes the stored <see cref="KeySelector"/> to retrieve the key from the specified item.
        /// </summary>
        /// <param name="item">The item to feed into the <see cref="KeySelector"/>.</param>
        /// <returns>The key calculated from the <see cref="KeySelector"/>.</returns>
        /// <exception cref="InvalidOperationException">
        ///     The <see cref="KeySelector"/> threw an <see cref="Exception"/> when fed
        ///     <paramref name="item"/>.
        /// </exception>
        protected virtual TKey GetKey(TValue item)
        {
            try
            {
                return this.KeySelector(item);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(
                    string.Format(
                        "{0} returned an exception: {1}",
                        nameof(this.KeySelector),
                        e.Message
                    ), e);
            }
        }

        #endregion

        private class NonGenericDictionaryEnumerator : IDictionaryEnumerator
        {
            private IEnumerator<KeyValuePair<TKey, TValue>> Enumerator { get; }

            public object Current => this.Entry;
            public DictionaryEntry Entry => new DictionaryEntry(this.Enumerator.Current.Key, this.Enumerator.Current.Value);
            public object Key => this.Enumerator.Current.Key;
            public object Value => this.Enumerator.Current.Value;

            public NonGenericDictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
            {
                this.Enumerator = enumerator;
            }

            public bool MoveNext()
            {
                return this.Enumerator.MoveNext();
            }
            public void Reset()
            {
                this.Enumerator.Reset();
            }
        }
    }
}
