﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MG.Collections
{
    /// <summary>
    /// An class that exposes only the read-only operations of a <see cref="HashSet{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of elements contained in the set.</typeparam>
    [Serializable]
    public class ReadOnlySet<T> : IReadOnlySet<T>, ICollection, IDeserializationCallback, ISerializable
    {
        private HashSet<T> _set;

        /// <summary>
        /// The inner set of values that is exposed as read-only.
        /// </summary>
#if !NET5_0_OR_GREATER
        protected ISet<T> InnerSet => _set;
#else
        protected IReadOnlySet<T> InnerSet => _set;
#endif
        public int Count => _set.Count;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this.InnerSet;

        public ReadOnlySet(IEnumerable<T> items)
            : this(items, !typeof(T).Equals(typeof(string))
                ?  EqualityComparer<T>.Default
                : (IEqualityComparer<T>)StringComparer.CurrentCulture)
        {
        }
        public ReadOnlySet(IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            _set = new HashSet<T>(items, comparer);
        }

        #region ENUMERATORS
        public IEnumerator<T> GetEnumerator() => _set.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region METHODS
        public bool Contains(T item) => _set.Contains(item);
        /// <summary>
        /// Copies the elements of a <see cref="ReadOnlySet{T}"/> object to an array,
        /// starting at the specified array index.
        /// </summary>
        /// <param name="newArray">
        ///     The one-dimensional array that is the destination fo the elements copied from the
        ///     <see cref="ReadOnlySet{T}"/> object.  The array must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">
        ///     The zero-based index in array at which copying begins.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="newArray"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="ArgumentException">
        ///     <paramref name="arrayIndex"/> is greater than the length of <paramref name="newArray"/>.
        /// </exception>
        public void CopyTo(T[] newArray, int arrayIndex) => _set.CopyTo(newArray, arrayIndex);
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)_set).CopyTo(array, index);
        }
        /// <summary>
        /// Implements the <see cref="ISerializable"/> interface and returns the data needed to serialize
        /// <see cref="ReadOnlySet{T}"/> object.
        /// </summary>
        /// <param name="info">
        ///     A <see cref="SerializationInfo"/> object that contains the information required to serialize
        ///     <see cref="ReadOnlySet{T}"/> object.
        /// </param>
        /// <param name="context">
        ///     A <see cref="StreamingContext"/> structure that contains the source and destination of the serialized
        ///     stream associated with the <see cref="ReadOnlySet{T}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="info"/> is <see langword="null"/>.</exception>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            _set.GetObjectData(info, context);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other) => _set.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<T> other) => _set.IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<T> other) => _set.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<T> other) => _set.IsSupersetOf(other);

        public void OnDeserialization(object sender) => _set.OnDeserialization(sender);

        public bool Overlaps(IEnumerable<T> other) => _set.Overlaps(other);
        public bool SetEquals(IEnumerable<T> other) => _set.SetEquals(other);

        #endregion
    }
}
