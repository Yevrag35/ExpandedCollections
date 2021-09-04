using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

#pragma warning disable IDE0130

namespace MG.Collections.Wpf
{
    /// <summary>
    /// A list class for WPF applications that provides the same functionality as would a combined 
    /// <see cref="ObservableCollection{T}"/> and  <see cref="List{T}"/> implementation, but also enforces every 
    /// element to be unique according to a custom or the default <see cref="IEqualityComparer{T}"/> allowing it 
    /// to provide access to the non-modifying <see cref="ISet{T}"/> methods.
    /// </summary>
    /// <typeparam name="T">The element type in the <see cref="UniqueObservableList{T}"/>.</typeparam>
    public class UniqueObservableList<T> : UniqueList<T>, IViewableList, IViewableCollection, INotifyPropertyChanged
    {
        #region EVENT HANDLERS
        /// <summary>
        /// Occurs when the <see cref="UniqueObservableList{T}"/> changes.
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Occurs when a new <see cref="ListCollectionView"/> is generated for the <see cref="UniqueObservableList{T}"/>.
        /// </summary>
        public event ViewGeneratedEventHandler ViewGenerated;
        /// <summary>
        /// Occurs when a new <see cref="ListCollectionView"/> is currently being generated for the <see cref="UniqueObservableList{T}"/>.
        /// </summary>
        public event EventHandler ViewGenerating;

        /// <summary>
        /// Calls the <see cref="CollectionChanged"/> event (if defined) passing the specified event arguments.
        /// </summary>
        /// <param name="e">The event arguments to pass to <see cref="CollectionChanged"/>.</param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            this.CollectionChanged?.Invoke(this, e);
        }
        private void OnAdd(object changedItem, int index)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItem, index));
        }
        private void OnRemove(object removedItem, int index)
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItem, index));
        }
        private void OnReplace(int index)
        {
            object item = this[index];
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, index));
        }
        private void OnReset()
        {
            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion

        #region ILIST EXPLICIT INDEXER
        object IList.this[int index]
        {
            get => this[index];
            set
            {
                this[index] = value is T item
                    ? item
                    : throw new NotSupportedException(
                        string.Format("value must be of the type '{0}'", typeof(T).FullName));
            }
        }

        #endregion

        #region PROPERTIES

        #region ILIST EXPLICIT PROPERTIES
        bool IList.IsFixedSize => false;
        bool IList.IsReadOnly => false;

        #endregion

        ///// <summary>
        ///// Gets or sets a value that indicates whether the <see cref="UniqueObservableList{T}"/> (after applying the sort
        ///// and filters, if any) is already in the correct order for grouping.
        ///// </summary>
        ///// <returns>
        /////     <see langword="true"/> if the <see cref="UniqueObservableList{T}"/> is already in the correct order for grouping;
        /////     otherwise, <see langword="false"/>.
        ///// </returns>
        ///// <exception cref="InvalidOperationException"><see cref="View"/> is <see langword="null"/>.</exception>
        //public bool IsDataInGroupOrder
        //{
        //    get => (this.View?.IsDataInGroupOrder).GetValueOrDefault();
        //    set
        //    {
        //        if (!this.IsViewGenerated)
        //            throw new InvalidOperationException("View properties cannot be set until a View is generated.");

        //        this.View.IsDataInGroupOrder = value;
        //        this.NotifyChange(nameof(IsDataInGroupOrder));
        //    }
        //}
        /// <summary>
        /// Indicates whether <see cref="View"/> has been generated.
        /// </summary>
        public virtual bool IsViewGenerated
        {
            get => null != this.View;
        }
        /// <summary>
        /// An optional starting filter for the <see cref="View"/> to use immediately after initialization.
        /// </summary>
        protected virtual Predicate<object> StartingFilter { get; }
        /// <summary>
        /// Optional starting property names for <see cref="View"/> that participate in filtering data in real time.
        /// </summary>
        /// <remarks>
        ///     Default is an empty <see cref="string"/> array.
        /// </remarks>
        protected virtual string[] StartingLiveFilteringProperties { get; } = new string[0];
        /// <summary>
        /// Optional starting property names for <see cref="View"/> that participate in grouping data in real time.
        /// </summary>
        /// <remarks>
        ///     Default is an empty <see cref="string"/> array.
        /// </remarks>
        protected virtual string[] StartingLiveGroupingProperties { get; } = new string[0];
        /// <summary>
        /// Optional starting property names for <see cref="View"/> that participate in sorting data in real time.
        /// </summary>
        /// <remarks>
        ///     Default is an empty <see cref="string"/> array.
        /// </remarks>
        protected virtual string[] StartingLiveSortingProperties { get; } = new string[0];
        /// <summary>
        /// Represents the current <see cref="UniqueObservableList{T}"/> as a collection view for grouping, sorting,
        /// filtering, and navigating as a data collection.
        /// </summary>
        /// <remarks>
        ///     This is <see langword="null"/> until after calling <see cref="GenerateView"/>.
        /// </remarks>
        public ListCollectionView View { get; private set; }
        ICollectionView IViewableCollection.View => this.View;
        /// <summary>
        /// Indicates whether the current <see cref="View"/> is being filtered.
        /// </summary>
        public bool ViewIsFiltered => null != this.View?.Filter;
        ///// <summary>
        ///// Gets or sets a value that indicates whether <see cref="View"/> is enabled to filter data in real time.
        ///// </summary>
        ///// <remarks>
        /////     If <see cref="StartingLiveFilteringProperties"/> is not empty, then this will default to <see langword="true"/>.
        ///// </remarks>
        ///// <exception cref="InvalidOperationException"><see cref="View"/> is <see langword="null"/>.</exception>
        //public bool ViewIsLiveFiltering
        //{
        //    get => (this.View?.IsLiveFiltering).GetValueOrDefault();
        //    set
        //    {
        //        if (!this.IsViewGenerated)
        //            throw new InvalidOperationException("View properties cannot be set until a View is generated.");

        //        this.View.IsLiveFiltering = value;
        //        this.NotifyChange(nameof(ViewIsLiveFiltering));
        //    }
        //}
        ///// <summary>
        ///// Gets or sets a value that indicates whether <see cref="View"/> is enabled to group data in real time.
        ///// </summary>
        ///// <remarks>
        /////     If <see cref="StartingLiveGroupingProperties"/> is not empty, then this will default to <see langword="true"/>.
        ///// </remarks>
        ///// <exception cref="InvalidOperationException"><see cref="View"/> is <see langword="null"/>.</exception>
        //public bool ViewIsLiveGrouping
        //{
        //    get => (this.View?.IsLiveGrouping).GetValueOrDefault();
        //    set
        //    {
        //        if (!this.IsViewGenerated)
        //            throw new InvalidOperationException("View properties cannot be set until a View is generated.");

        //        this.View.IsLiveGrouping = value;
        //        this.NotifyChange(nameof(ViewIsLiveGrouping));
        //    }
        //}
        ///// <summary>
        ///// Gets or sets a value that indicates whether <see cref="View"/> is enabled to sort data in real time.
        ///// </summary>
        ///// <remarks>
        /////     If <see cref="StartingLiveSortingProperties"/> is not empty, then this will default to <see langword="true"/>.
        ///// </remarks>
        ///// <exception cref="InvalidOperationException"><see cref="View"/> is <see langword="null"/>.</exception>
        //public bool ViewIsLiveSorting
        //{
        //    get => (this.View?.IsLiveSorting).GetValueOrDefault();
        //    set
        //    {
        //        if (!this.IsViewGenerated)
        //            throw new InvalidOperationException("View properties cannot be set until a View is generated.");

        //        this.View.IsLiveSorting = value;
        //        this.NotifyChange(nameof(ViewIsLiveSorting));
        //    }
        //}
        /// <summary>
        /// Indicates whether <see cref="View"/> needs to be refreshed.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the view needs to be refreshed; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ViewNeedsRefresh => (this.View?.NeedsRefresh).GetValueOrDefault();

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueObservableList{T}"/> class that is empty
        /// and has the default initial capacity and default equality comparer for <typeparamref name="T"/>.
        /// </summary>
        public UniqueObservableList()
            : base()
        {
            //this.GenerateView();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueObservableList{T}"/> class that is empty
        /// and has the specified initial capacity and default equality comparer for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="capacity">The number of elements that the new collection can initially store.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public UniqueObservableList(int capacity)
            : base(capacity)
        {
            //this.GenerateView();
        }
        /// <summary>
        /// Initializes a new <see cref="UniqueObservableList{T}"/> instance that contains elements copied from the specified
        /// collection and has sufficient capacity to accomodate the number of unique elements copied.
        /// </summary>
        /// <remarks>
        ///     <paramref name="items"/> will be enumerated for uniqueness according to the default
        ///     <see cref="IEqualityComparer{T}"/> for type <typeparamref name="T"/>.
        ///     
        ///     If <paramref name="items"/> is null, no exception is thrown, and, instead, an empty
        ///     <see cref="UniqueObservableList{T}"/> instance is initialized.
        /// </remarks>
        /// <param name="items">
        ///     The collection whose elements will be enumerated for uniqueness and added
        ///     to the list.
        /// </param>
        public UniqueObservableList(IEnumerable<T> items)
            : base(items)
        {
            //this.GenerateView();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueObservableList{T}"/> class that is empty
        /// and has the default initial capacity and the specified equality comparer for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="equalityComparer">
        ///     The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the list, or
        ///     <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> implementation for the
        ///     type <typeparamref name="T"/>.
        /// </param>
        public UniqueObservableList(IEqualityComparer<T> equalityComparer)
            : base(equalityComparer)
        {
            //this.GenerateView();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueObservableList{T}"/> class that is empty, has the specified
        /// initial capacity, and uses the specified equality comparer for the <typeparamref name="T"/> type.
        /// </summary>
        /// <param name="capacity">The number of elements that the new collection can initially store.</param>
        /// <param name="equalityComparer">
        ///     The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the list, or
        ///     <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> implementation for the
        ///     type <typeparamref name="T"/>.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public UniqueObservableList(int capacity, IEqualityComparer<T> equalityComparer)
            : base(capacity, equalityComparer)
        {
            //this.GenerateView();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueObservableList{T}"/> class that uses the specified comparer for 
        /// the <typeparamref name="T"/> type, contains elements copied from the specified collection, and sufficient capacity
        /// to accommodate the number of elements copied.
        /// </summary>
        /// <remarks>
        ///     If <paramref name="items"/> is null, no exception is thrown, and, instead, an empty
        ///     <see cref="UniqueObservableList{T}"/> instance is initialized.
        /// </remarks>
        /// <param name="items">The collection whose elements are copied to the new list.</param>
        /// <param name="equalityComparer">
        ///     The <see cref="IEqualityComparer{T}"/> implementation to use when comparing values in the list, or
        ///     <see langword="null"/> to use the default <see cref="EqualityComparer{T}"/> implementation for the
        ///     type <typeparamref name="T"/>.
        /// </param>
        public UniqueObservableList(IEnumerable<T> items, IEqualityComparer<T> equalityComparer)
            : base(items, equalityComparer)
        {
            //this.GenerateView();
        }

        #endregion

        #region UNIQUELIST OVERRIDES
        /// <summary>
        /// Adds an object to the end of the <see cref="UniqueObservableList{T}"/>.
        /// </summary>
        /// <param name="item">The object to add.</param>
        protected override void AddItem(T item)
        {
            _ = this.InsertItem(this.Count, item, true);
        }
        /// <summary>
        /// Removes all elements from the <see cref="UniqueObservableList{T}"/>.
        /// </summary>
        protected override void ClearItems()
        {
            base.ClearItems();
            this.OnReset();
        }
        /// <summary>
        /// Inserts an elements into the <see cref="UniqueObservableList{T}"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert.</param>
        protected override void InsertItem(int index, T item)
        {
            _ = this.InsertItem(index, item, true);
        }
        private int InsertItem(int index, T item, bool inserting)
        {
            base.InsertItem(index, item);
            int retIndex = this.IndexOf(item);
            if (retIndex > -1)
            {
                this.OnAdd(item, retIndex);
            }

            return retIndex;
        }
        /// <summary>
        /// Removes the specified element from the <see cref="UniqueObservableList{T}"/>.
        /// </summary>
        /// <param name="item">The element to remove.</param>
        /// <returns>
        ///     <see langword="true"/> if <paramref name="item"/> is successfully removed; otherwise <see langword="false"/>.
        ///     This method also returns <see langword="false"/> if <paramref name="item"/> was not found in 
        ///     the <see cref="UniqueObservableList{T}"/>.
        /// </returns>
        protected override bool RemoveItem(T item)
        {
            int index = this.IndexOf(item);
            return this.RemoveItem(item, index);
        }
        /// <summary>
        /// Removes the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     <paramref name="index"/> is less than 0.
        ///     -or-
        ///     <paramref name="index"/> is equal to or greater than <see cref="UniqueListBase{T}.Count"/>.
        /// </exception>
        protected override void RemoveItemAt(int index)
        {
            T item = this[index];
            if (null != item)
            {
                _ = this.RemoveItem(item, index);
            }
        }
        private bool RemoveItem(T item, int index)
        {
            bool result = index > -1 && base.RemoveItem(item);
            if (result)
            {
                this.OnRemove(item, index);
            }

            return result;
        }
        /// <summary>
        /// Replaces the item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to replace.</param>
        /// <param name="item"></param>
        protected override bool SetItem(int index, T item)
        {
            bool result = base.SetItem(index, item);
            if (result)
            {
                this.OnReplace(index);
            }

            return result;
        }

        #endregion

        #region ILIST EXPLICIT METHODS
        int IList.Add(object value)
        {
            int index = -1;
            if (value is T item)
            {
                index = this.InsertItem(this.Count, item, true);
            }

            return index;
        }
        bool IList.Contains(object value)
        {
            return value is T item && this.Contains(item);
        }
        int IList.IndexOf(object value)
        {
            return value is T item
                ? this.IndexOf(item)
                : -1;
        }
        void IList.Insert(int index, object value)
        {
            if (value is T item)
                this.Insert(index, item);
        }
        void IList.Remove(object value)
        {
            if (value is T item)
                _ = this.Remove(item);
        }

        #endregion

        #region VIEW METHODS
        /// <summary>
        /// Generates the <see cref="ListCollectionView"/> and defines it as <see cref="View"/> for the current <see cref="UniqueObservableList{T}"/>.
        /// </summary>
        public void GenerateView()
        {
            this.OnViewGenerating();
            this.View = new ListCollectionView(this);
            this.ApplyStartingProperties(this.View);
            this.OnViewGenerated();
        }
        /// <summary>
        /// A definable method that is called after the <see cref="View"/> is generated.
        /// </summary>
        /// <remarks>
        ///     By default, it simply calls the <see cref="ViewGenerated"/> event.
        /// </remarks>
        protected virtual void OnViewGenerated()
        {
            this.ViewGenerated?.Invoke(this, new ViewGeneratedEventArgs(GeneratedViewType.List));
        }
        /// <summary>
        /// A definable method that is called before the <see cref="View"/> is generated.
        /// </summary>
        /// <remarks>
        ///     By default, it simply calls the <see cref="ViewGenerating"/> event.
        /// </remarks>
        protected virtual void OnViewGenerating()
        {
            this.ViewGenerating?.Invoke(this, EventArgs.Empty);
        }

        private static bool ApplyProperties(IList<string> propList, string[] propsToAdd)
        {
            bool result = false;
            if (null != propsToAdd && propsToAdd.Length <= 0)
            {
                Array.ForEach(propsToAdd, (p) =>
                {
                    propList.Add(p);
                });

                result = true;
            }

            return result;
        }
        private void ApplyStartingProperties(ListCollectionView view)
        {
            if (ApplyProperties(view.LiveFilteringProperties, this.StartingLiveFilteringProperties))
                view.IsLiveFiltering = true;

            if (ApplyProperties(view.LiveGroupingProperties, this.StartingLiveGroupingProperties))
                view.IsLiveGrouping = true;

            if (ApplyProperties(view.LiveSortingProperties, this.StartingLiveSortingProperties))
                view.IsLiveSorting = true;
        }

        #endregion

        #region OTHER METHODS
        /// <summary>
        /// Takes a given <see cref="Predicate{T}"/> and simply negates the <see cref="bool"/> result.
        /// </summary>
        /// <param name="predicate">The predicate to negate.</param>
        /// <returns>
        ///     A <see cref="bool"/> value that is opposite to the resolved value of <paramref name="predicate"/>.
        /// </returns>
        protected static Predicate<T> NegatePredicate(Predicate<T> predicate)
        {
            return x => !predicate(x);
        }
        /// <summary>
        /// Calls the <see cref="PropertyChanged"/> event if defined for the specified property name.
        /// </summary>
        /// <remarks>
        ///     Simply a convenience method that is equivalent to 
        ///     this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        /// </remarks>
        /// <param name="propertyName">The name of the property to notify on.</param>
        protected virtual void NotifyChange(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}