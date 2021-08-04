using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MG.Collections
{
    public class ManagedKeySortedList<TKey, TItem> : IEnumerable<TItem>
    {
        #region PRIVATE FIELDS/CONSTANTS
        

        #endregion

        #region PROPERTIES

        #region PROTECTED
        protected SortedList<TKey, TItem> InnerList { get; }
        protected Func<TItem, TKey> KeySelector { get; }

        #endregion

        #endregion

        #region CONSTRUCTORS
        public ManagedKeySortedList(Func<TItem, TKey> keySelector)
        {
            this.KeySelector = keySelector;
        }

        #endregion

        #region LIST METHODS
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
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
        public bool Add(TItem item)
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

        #endregion

        #region ENUMERATORS
        public virtual IEnumerator<TItem> GetEnumerator()
        {
            return this.InnerList.Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region KEY METHODS
        /// <summary>
        /// Executes the stored <see cref="KeySelector"/> to retrieve the key from the specified item.
        /// </summary>
        /// <param name="item">The item to feed into the <see cref="KeySelector"/>.</param>
        /// <exception cref="InvalidOperationException">
        ///     The <see cref="KeySelector"/> threw an <see cref="Exception"/> when fed
        ///     <paramref name="item"/>.
        /// </exception>
        /// <returns>The key calculated from the <see cref="KeySelector"/>.</returns>
        protected virtual TKey GetKey(TItem item)
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
    }
}
