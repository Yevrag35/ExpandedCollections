using System;
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
        protected HashSet<T> InnerSet { get; }

        public int Count => this.InnerSet.Count;
        public IEqualityComparer<T> Comparer => this.InnerSet.Comparer;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this.InnerSet;

        public ReadOnlySet(IEnumerable<T> items)
            : this(items, EqualityComparer<T>.Default)
        {
        }
        public ReadOnlySet(IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            this.InnerSet = new HashSet<T>(items, comparer);
        }

        #region ENUMERATORS
        public IEnumerator<T> GetEnumerator() => InnerSet.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        #endregion

        #region METHODS
        public bool Contains(T item) => this.InnerSet.Contains(item);
        public void CopyTo(T[] newArray, int arrayIndex) => this.InnerSet.CopyTo(newArray, arrayIndex);
        void ICollection.CopyTo(Array array, int index)
        {
            ((ICollection)this.InnerSet).CopyTo(array, index);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        /// <exception cref="ArgumentNullException"/>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.InnerSet.GetObjectData(info, context);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other) => this.InnerSet.IsProperSubsetOf(other);
        public bool IsProperSupersetOf(IEnumerable<T> other) => this.InnerSet.IsProperSupersetOf(other);
        public bool IsSubsetOf(IEnumerable<T> other) => this.InnerSet.IsSubsetOf(other);
        public bool IsSupersetOf(IEnumerable<T> other) => this.InnerSet.IsSupersetOf(other);

        public void OnDeserialization(object sender) => this.InnerSet.OnDeserialization(sender);

        public bool Overlaps(IEnumerable<T> other) => this.InnerSet.Overlaps(other);
        public bool SetEquals(IEnumerable<T> other) => this.InnerSet.SetEquals(other);

        #endregion
    }
}
