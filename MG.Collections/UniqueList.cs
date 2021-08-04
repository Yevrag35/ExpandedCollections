using MG.Collections.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MG.Collections
{
    /// <summary>
    /// A class that provides the same functionality as <see cref="List{T}"/>, but enforces every element to be
    /// unique according to the default or custom-defined equality comparer.
    /// </summary>
    /// <typeparam name="T">The element type in the <see cref="UniqueList{T}"/>.</typeparam>
    public class UniqueList<T> : UniqueListBase<T>
    {
        #region PRIVATE FIELDS/CONSTANTS
        private const int DEFAULT_CAPACITY = 0;

        #endregion

        #region INDEXERS
        public T this[int index]
        {
            get => base.GetByIndex(index);
            set
            {
                if (base.TryIsValidIndex(index, out int positiveIndex))
                    base.ReplaceValueAtIndex(positiveIndex, value);
                
                else
                    throw new ArgumentOutOfRangeException();
            }
        }

        #endregion

        #region PROPERTIES


        #region INTERFACE EXPLICIT PROPERTIES


        #endregion

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// The default constructor.  Initializes an empty list using the default 
        /// <see cref="IEqualityComparer{T}"/> for <typeparamref name="T"/>.
        /// </summary>
        public UniqueList()
            : base(DEFAULT_CAPACITY)
        {
        }
        /// <summary>
        /// Initializes an empty <see cref="UniqueList{T}"/> with the specified capacity using the default
        /// <see cref="IEqualityComparer{T}"/> for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="capacity"></param>
        public UniqueList(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Initializes an empty <see cref="UniqueList{T}"/> with the default capacity using the specified
        /// <see cref="IEqualityComparer{T}"/> to determine uniqueness.
        /// </summary>
        /// <param name="equalityComparer">The comparer used to define if an incoming element is unique.</param>
        public UniqueList(IEqualityComparer<T> equalityComparer)
            : base(DEFAULT_CAPACITY, equalityComparer)
        {
        }

        /// <summary>
        /// Initializes an empty <see cref="UniqueList{T}"/> with the specified capacity using the specified
        /// <see cref="IEqualityComparer{T}"/> to determine uniqueness.
        /// </summary>
        /// <param name="capacity">The number of new elements the list can initially store.</param>
        /// <param name="equalityComparer">The comparer used to define if an incoming element is unique.</param>
        public UniqueList(int capacity, IEqualityComparer<T> equalityComparer)
            : base(capacity, equalityComparer)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="UniqueList{T}"/> instance that contains elements copied from the specified
        /// collection and has sufficient capacity to accomodate the number of unique elements copied.
        /// </summary>
        /// <remarks>
        ///     <paramref name="items"/> will be enumerated for uniqueness according to the default
        ///     <see cref="IEqualityComparer{T}"/> for type <typeparamref name="T"/>.
        /// </remarks>
        /// <param name="items">
        ///     The collection whose elements will be enumerated for uniqueness and added
        ///     to the list.
        /// </param>
        public UniqueList(IEnumerable<T> items)
            : base(items, EqualityComparer<T>.Default)
        {
        }

        /// <summary>
        /// Initializes a new <see cref="UniqueList{T}"/> instance that contains elements copied from the specified
        /// collection and has sufficient capacity to accomodate the number of unique elements copied.
        /// </summary>
        /// <remarks>
        ///     <paramref name="collection"/> will be enumerated for uniqueness according to the provided 
        ///     <see cref="IEqualityComparer{T}"/>.
        /// </remarks>
        /// <param name="collection">
        ///     The collection whose elements will be enumerated for uniqueness and added
        ///     to the list.
        /// </param>
        /// <param name="equalityComparer">
        ///     The equality comparer that determines whether an element is unique.
        /// </param>
        public UniqueList(IEnumerable<T> collection, IEqualityComparer<T> equalityComparer)
            : base(collection, equalityComparer)
        {   
        }

        #endregion

        #region LIST METHODS
        /// <summary>
        /// Copies the elements of the <see cref="UniqueList{T}"/> to a new array.
        /// </summary>
        /// <returns>
        ///     An array containing copies of the elements of the <see cref="UniqueList{T}"/>.  If the list contains no elements, 
        ///     an empty array is returned.
        /// </returns>
        public T[] ToArray()
        {
            return InnerList.ToArray();
        }

        #region SET METHODS
        //public int ExceptWith(IEnumerable<T> other)
        //{
        //    InnerSet.ExceptWith(other);
        //    return InnerList.RemoveAll(x => other.Contains(x, InnerSet.Comparer));
        //}
        //void ISet<T>.ExceptWith(IEnumerable<T> other) => this.ExceptWith(other);
        //public bool IsProperSubsetOf(IEnumerable<T> other)
        //{
        //    return InnerSet.IsProperSubsetOf(other);
        //}
        //public bool IsProperSupersetOf(IEnumerable<T> other)
        //{
        //    return InnerSet.IsProperSupersetOf(other);
        //}
        //public bool IsSubsetOf(IEnumerable<T> other)
        //{
        //    return InnerSet.IsSubsetOf(other);
        //}
        //public bool IsSupersetOf(IEnumerable<T> other)
        //{
        //    return InnerSet.IsSupersetOf(other);
        //}
        //public bool Overlaps(IEnumerable<T> other)
        //{
        //    return InnerSet.Overlaps(other);
        //}

        #endregion

        #region INTERFACE EXPLICIT METHODS


        #endregion

        #region PRIVATE METHODS
        //private IList<T> GetEnumerableAsList(IEnumerable<T> enumerable)
        //{
        //    if (enumerable is T[] tArr)
        //        return tArr;

        //    else if (enumerable is IList<T> iList)
        //        return iList;

        //    else if (enumerable is Collection<T> col)
        //        return col;

        //    else
        //        return new List<T>(enumerable);
        //}

        

        #endregion

        #endregion
    }
}
