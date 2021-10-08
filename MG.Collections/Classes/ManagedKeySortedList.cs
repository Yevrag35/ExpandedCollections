using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MG.Collections.Extensions;

using Strings = MG.Collections.Properties.Resources;

#pragma warning disable IDE0130

namespace MG.Collections
{
    /// <summary>
    /// A <see cref="SortedList{TKey, TValue}"/> class where <typeparamref name="TKey"/> is automatically retrieved with 
    /// a specified function from each <typeparamref name="TValue"/> element added.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
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
        /// <remarks>
        ///     The set accessors' value must generate a <typeparamref name="TKey"/> key that equals <paramref name="key"/>, 
        ///     otherwise, an <see cref="ArgumentException"/> is thrown.
        /// </remarks>
        /// <param name="key">The key whose value to get or set.</param>
        /// <returns>
        ///     The value associated with the specified key. If the specified key is not found,
        ///     the default value of <typeparamref name="TValue"/> is returned;
        ///     A set operation throws a <see cref="KeyNotFoundException"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">The value's key provided to the set accessor does not match the original key.</exception>
        /// <exception cref="InvalidOperationException">
        ///     The <see cref="KeySelector"/> threw an <see cref="Exception"/> when fed
        ///     <paramref name="value"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException"><paramref name="key"/> was not found in the <see cref="ManagedKeySortedList{TKey, TValue}"/>.</exception>
        public TValue this[TKey key]
        {
            get => this.GetValueOrDefault(key);
            set
            {
                if (!this.SetItem(key, value))
                    throw new ArgumentException("The new value's key does not equate to the output of the managed key function.");
            }
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
        /// <exception cref="ArgumentException">The value's key provided to the set accessor does not match the original key.</exception>
        /// <exception cref="InvalidOperationException">
        ///     The <see cref="KeySelector"/> threw an <see cref="Exception"/> when fed
        ///     <paramref name="value"/>.
        /// </exception>
        public TValue this[int index]
        {
            get => InnerList.Values.GetByIndex(index);
            set
            {
                int realIndex = IndexHelper.GetPositiveIndex(index, this.Count);
                TKey key = this.GetKey(value);
                if (null == key || !InnerList.ContainsKey(key) || InnerList.IndexOfKey(key) != realIndex)
                    throw new KeyNotFoundException(string.Format("No matching key at index {0} was found.", realIndex));

                if (!this.SetItem(key, value))
                    throw new ArgumentException("The new value's key does not equate to the output of the managed key function.");
            }
        }

        #region NON-GENERIC DICTIONARY INDEXER
        object IDictionary.this[object key]
        {
            get
            {
                return key is TKey tKey
                    ? this[tKey]
                    : (object)null;
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
            get => InnerList.Capacity;
            set => InnerList.Capacity = value;
        }
        /// <summary>
        /// Gets the <see cref="IComparer{T}"/> for the managed key sorted list.
        /// </summary>
        /// <returns>
        ///     The <see cref="IComparer{T}"/> for the current <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </returns>
        public IComparer<TKey> Comparer
        {
            get => InnerList.Comparer;
        }
        /// <summary>
        /// Gets the number of elements contained in the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        public int Count
        {
            get => InnerList.Count;
        }
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
        public IList<TKey> Keys
        {
            get => InnerList.Keys;
        }
        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <returns>
        ///     The current instance.
        /// </returns>
        public object SyncRoot
        {
            get => this;
        }

        #region PROTECTED

        /// <summary>
        /// The function that is used to retrieve a value of type <typeparamref name="TKey"/>
        /// from a given <typeparamref name="TValue"/> that is used as the sorted key.
        /// </summary>
        protected Func<TValue, TKey> KeySelector { get; }

        #endregion

        #region IDICTIONARY EXPLICIT PROPERTIES
        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            get => this.Keys;
        }
        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            get => InnerList.Values;
        }

        #endregion

        #region NON-GENERIC IDICTIONARY PROPERTIES
        ICollection IDictionary.Keys
        {
            get => this.Keys.Cast<object>().ToArray();
        }
        ICollection IDictionary.Values
        {
            get => this.Cast<object>().ToArray();
        }

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
            : this(0, GetDefaultComparer(), keySelector)
        {
        }

        public ManagedKeySortedList(int capacity, Func<TValue, TKey> keySelector)
            : this(capacity, GetDefaultComparer(), keySelector)
        {
        }

        public ManagedKeySortedList(int capacity, IComparer<TKey> comparer, Func<TValue, TKey> keySelector)
        {
            this.KeySelector = keySelector;
            this.InnerList = new SortedList<TKey, TValue>(capacity, comparer);
        }

        public ManagedKeySortedList(IComparer<TKey> comparer, Func<TValue, TKey> keySelector)
            : this(0, comparer, keySelector)
        {
        }

        public ManagedKeySortedList(IEnumerable<TValue> items, Func<TValue, TKey> keySelector)
            : this(items, GetDefaultComparer(), keySelector)
        {
        }

        public ManagedKeySortedList(IEnumerable<TValue> items, IComparer<TKey> comparer, Func<TValue, TKey> keySelector)
        {
            this.KeySelector = keySelector;
            this.InnerList = AddInitialValues(items, comparer, keySelector);
        }

        #endregion

        #region GENERIC LIST METHODS
        /// <summary>
        /// Adds an item to the end of the list.
        /// </summary>
        /// <param name="item">The item to be added to the end of the <see cref="ManagedKeySortedList{TKey, TItem}"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="item"/> was successfully added to the 
        ///     <see cref="ManagedKeySortedList{TKey, TItem}"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     An exception adding <paramref name="item"/> created an
        ///     <see cref="ArgumentException"/> that was not due to an existing key.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="KeySelector"/> threw an exception.
        /// </exception>
        public void Add(TValue item)
        {
            _ = this.AddItem(item);
        }

        /// <summary>
        /// Removes all elements from the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        public void Clear()
        {
            this.ClearItems();
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
            return InnerList.ContainsValue(item);
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
            return InnerList.IndexOfValue(value);
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
            TKey key = InnerList.Keys[index];
            _ = this.Remove(key);
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
        public bool RemoveValue(TValue item)
        {
            TKey key = this.GetKey(item);
            return this.RemoveItem(key);
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
            return InnerList.ContainsKey(key);
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
            this.InnerList.Values.CopyTo(array, arrayIndex);
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
        /// Searches for the specified key and returns the zero-based index within the entire <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key to lcoate in the <see cref="ManagedKeySortedList{TKey, TValue}"/>.</param>
        /// <returns>
        ///     The zero-based index of <paramref name="key"/> if found; otherwise, -1.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        public int IndexOfKey(TKey key)
        {
            return InnerList.IndexOfKey(key);
        }
        /// <summary>
        /// Removes the element with the specified key from the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>
        ///    <see langword="true"/> if the element is successfully removed; otherwise, <see langword="false"/>. This method also
        ///    returns false if <paramref name="key"/> was not found in the original <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        public bool Remove(TKey key)
        {
            return this.RemoveItem(key);
        }
        /// <summary>
        /// Sets the capacity to the actual number of elements in the <see cref="ManagedKeySortedList{TKey, TValue}"/>,
        ///     if that number is less than a threshold value.
        /// </summary>
        public void TrimExcess()
        {
            InnerList.TrimExcess();
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
            
            return InnerList.TryGetValue(key, out value);
        }

        #region IDICTIONARY EXPLICITS
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            this.Add(value);
        }
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Value);
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            return this.ContainsKey(item.Key);
        }
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)InnerList).CopyTo(array, arrayIndex);
        }
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            return this.Remove(item.Key);
        }

        #endregion

        #region ILIST EXPLICITS
        void IList<TValue>.Insert(int index, TValue item)
        {
            this.Add(item);
        }
        bool ICollection<TValue>.Remove(TValue item)
        {
            return this.RemoveValue(item);
        }

        #endregion

        #endregion

        #region NON-GENERIC DICTIONARY METHODS
        void IDictionary.Add(object key, object value)
        {
            if (value is TValue tVal)
            {
                this.Add(tVal);
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
                this.Add(tVal);
                return InnerList.IndexOfValue(tVal);
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
                this.Add(tVal);
            }
        }
        void IList.Remove(object value)
        {
            if (value is TValue tVal)
            {
                _ = this.RemoveValue(tVal);
            }
        }

        #endregion

        #region EXTENDED METHODS
        /// <summary>
        /// Creates a shallow copy of a range of elements in the source <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <param name="index">The zero-based index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns>A shallow copy of a range of elements in the <see cref="ManagedKeySortedList{TKey, TValue}"/>.</returns>
        /// <exception cref="ArgumentException">
        ///     <paramref name="index"/> and <paramref name="count"/> do not denote a valid range of elements.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="count"/> is less than 0.
        /// </exception>
        public List<TValue> GetRange(int index, int count)
        {
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException(string.Format("{0} -or- {1} is less than 0.", nameof(index), nameof(count)));

            int endingIndex = index + (count - 1);
            if (count < index || this.Count <= endingIndex)
                throw new ArgumentException(string.Format("{0} and {1} do not denote a valid range of elements", nameof(index), nameof(count)));

            var list = new List<TValue>(count);
            for (int i = index; i <= endingIndex; i++)
            {
                list.Add(InnerList.Values[i]);
            }

            return list;
        }

        /// <summary>
        /// Attempts to add the specified value to the end of the list.
        /// </summary>
        /// <remarks>
        ///     Any exception that occurs is suppressed.
        /// </remarks>
        /// <param name="value">The element to add to the end of the <see cref="ManagedKeySortedList{TKey, TValue}"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="value"/> was added to the end of the list with the 
        ///     calculated <typeparamref name="TKey"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool TryAdd(TValue value)
        {
            try
            {
                return this.AddItem(value);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region ENUMERATORS
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="ManagedKeySortedList{TKey, TValue}"/> values.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the values of the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </returns>
        public virtual IEnumerator<TValue> GetEnumerator()
        {
            return InnerList.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return InnerList.GetEnumerator();
        }
        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new NonGenericDictionaryEnumerator(InnerList.GetEnumerator());
        }

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

        #endregion

        #region PROTECTED OVERRIDABLES

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
        /// <summary>
        /// Attempts to add the specified value to the end of the list.
        /// </summary>
        /// <param name="value">The element to add to the end of the <see cref="ManagedKeySortedList{TKey, TValue}"/>.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="value"/> was added to the list with the 
        ///     calculated <typeparamref name="TKey"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     An exception adding <paramref name="value"/> created an
        ///     <see cref="ArgumentException"/> that was not due to an existing key.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="KeySelector"/> threw an exception.
        /// </exception>
        protected virtual bool AddItem(TValue value)
        {
            TKey key = this.GetKey(value);
            try
            {
                InnerList.Add(key, value);
                return true;
            }
            catch (ArgumentException e)
            {
                if (e.GetBaseException().Message.IndexOf("same key already exists") < 0)
                    throw new ArgumentException("An error occured.  See inner exception for details.", e);
            }
            catch (Exception allOther)
            {
                throw new InvalidOperationException("An error occured.  See inner exception for details.", allOther);
            }

            return false;
        }
        /// <summary>
        /// Attemps to add the specified value to the end of the list.  Instead of using the stored function to retrieve the key,
        /// a separate function is specified from a new, generic input <typeparamref name="TInput"/>.
        /// </summary>
        /// <typeparam name="TInput">The generic type to retrieve <typeparamref name="TKey"/> from.</typeparam>
        /// <param name="input">The input value to retrieve the <typeparamref name="TKey"/> key from.</param>
        /// <param name="value">The value to store in the <see cref="ManagedKeySortedList{TKey, TValue}"/>.</param>
        /// <param name="keySelector">The function to retrieve the key from a <typeparamref name="TInput"/> value.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="value"/> was added to the list with the 
        ///     calculated <typeparamref name="TKey"/>; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Either <paramref name="input"/> or <paramref name="keySelector"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     <see cref="KeySelector"/> threw an exception.
        /// </exception>
        protected virtual bool AddItem<TInput>(TInput input, TValue value, Func<TInput, TKey> keySelector)
        {
            if (null == input)
                throw new ArgumentNullException(nameof(input));

            if (null == keySelector)
                throw new ArgumentNullException(nameof(keySelector));

            TKey key = keySelector(input);

            try
            {
                InnerList.Add(key, value);
                return true;
            }
            catch (ArgumentException e)
            {
                if (e.GetBaseException().Message.IndexOf("same key already exists") < 0)
                    throw new ArgumentException("An error occured.  See inner exception for details.", e);
            }
            catch (Exception allOther)
            {
                throw new InvalidOperationException("An error occured.  See inner exception for details.", allOther);
            }

            return false;
        }
        /// <summary>
        /// Removes all elements from the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        protected virtual void ClearItems()
        {
            this.InnerList.Clear();
        }
        /// <summary>
        /// Sets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get or set.</param>
        /// <param name="value">The value to replace the original one with.</param>
        /// <returns>
        ///     <see langword="true"/> if the calculated key from <see cref="GetKey(TValue)"/> equals 
        ///     <paramref name="key"/> and the original value was overwritten by <paramref name="value"/>;
        ///     otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        /// <exception cref="InvalidOperationException">
        ///     The <see cref="KeySelector"/> threw an <see cref="Exception"/> when fed
        ///     <paramref name="value"/>.
        /// </exception>
        /// <exception cref="KeyNotFoundException"><paramref name="key"/> was not found in the <see cref="ManagedKeySortedList{TKey, TValue}"/>.</exception>
        protected virtual bool SetItem(TKey key, TValue value)
        {
            TKey origKey = this.GetKey(value);
            if (!key.Equals(origKey))
                return false;
                
            InnerList[key] = value;
            return true;
        }
        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </summary>
        /// <param name="key">
        ///     The key to remove from the <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the key and value are successfully removed; otherwise, <see langword="false"/>. 
        ///     This method also returns false if <paramref name="key"/> was not found in the 
        ///     <see cref="ManagedKeySortedList{TKey, TValue}"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is <see langword="null"/>.</exception>
        protected virtual bool RemoveItem(TKey key)
        {
            return InnerList.Remove(key);
        }

        #endregion

        #region PRIVATE METHODS
        private static IComparer<TKey> GetDefaultComparer()
        {
            return !typeof(TKey).Equals(typeof(string))
                ? Comparer<TKey>.Default
                : (IComparer<TKey>)StringComparer.CurrentCultureIgnoreCase;
        }
        private static SortedList<TKey, TValue> AddInitialValues(IEnumerable<TValue> itemsToAdd, IComparer<TKey> comparer, Func<TValue, TKey> keySelector)
        {
            var list = new SortedList<TKey, TValue>(5, comparer);
            foreach (TValue item in itemsToAdd)
            {
                try
                {
                    list.Add(keySelector(item), item);
                }
                catch (ArgumentException e)   // swallow key exists exception.
                {
                    // Rethrow if it's not a key exists exception.
                    if (e.Message.IndexOf("same key already exists", StringComparison.CurrentCultureIgnoreCase) < 0)
                        throw new ArgumentException(e.Message, e);
                }
            }

            return list;
        }

        #endregion
    }
}
