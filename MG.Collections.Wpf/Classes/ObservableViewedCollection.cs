using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

#pragma warning disable IDE0130

namespace MG.Collections.Wpf
{
    /// <summary>
    /// A dynamic data collection that is inherited from <see cref="ObservableCollection{T}"/> which provides
    /// notifications when items get added, removed, or when the collection is refreshed.  Its members can also 
    /// generate and store a <see cref="ListCollectionView"/> that represents it.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ObservableViewedCollection<T> : ObservableCollection<T>, IViewableList, IViewableCollection
    {
        #region EVENTS
        /// <summary>
        /// Occurs when a new <see cref="ListCollectionView"/> is generated for the <see cref="ObservableViewedCollection{T}"/>.
        /// </summary>
        public event ViewGeneratedEventHandler ViewGenerated;
        /// <summary>
        /// Occurs when a new <see cref="ListCollectionView"/> is currently being generated for <see cref="ObservableViewedCollection{T}"/>.
        /// </summary>
        public event EventHandler ViewGenerating;

        #endregion

        #region PROPERTIES
        /// <summary>
        /// Indicates whether <see cref="View"/> has been generated.
        /// </summary>
        public bool IsViewGenerated
        {
            get => null != this.View;
        }
        /// <summary>
        /// Represents the current <see cref="ObservableViewedCollection{T}"/> as a collection view for grouping, sorting,
        /// filtering, and navigating as a data collection.
        /// </summary>
        /// <remarks>
        ///     This is <see langword="null"/> until after calling <see cref="GenerateView"/>.
        /// </remarks>
        public ListCollectionView View { get; private set; }
        ICollectionView IViewableCollection.View => this.View;

        #endregion

        #region CONSTRUCTORS
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableViewedCollection{T}"/> class that is empty.
        /// </summary>
        public ObservableViewedCollection()
            : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableViewedCollection{T}"/> class that contains elements
        /// copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <exception cref="ArgumentNullException"><paramref name="collection"/> is <see langword="null"/>.</exception>
        public ObservableViewedCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ObservableViewedCollection{T}"/> class that contains elements
        /// copied from the specified <see cref="List{T}"/>.
        /// </summary>
        /// <param name="list">The list from which the elements are copied.</param>
        /// <exception cref="ArgumentNullException"><paramref name="list"/> is <see langword="null"/>.</exception>
        public ObservableViewedCollection(List<T> list)
            : base(list)
        {
        }

        #endregion

        #region VIEW METHODS

        /// <summary>
        /// Generates the <see cref="ListCollectionView"/> and defines it as <see cref="View"/> for the current 
        /// <see cref="ObservableViewedCollection{T}"/>.
        /// </summary>
        public void GenerateView()
        {
            this.OnViewGenerating();
            this.View = new ListCollectionView(this);
            this.OnViewGenerated();
        }

        /// <summary>
        /// An overridable method that is called after the <see cref="View"/> is generated.
        /// </summary>
        /// <remarks>
        ///     By default, it simply calls the <see cref="ViewGenerated"/> event.
        /// </remarks>
        protected virtual void OnViewGenerated()
        {
            this.ViewGenerated?.Invoke(this, new ViewGeneratedEventArgs(GeneratedViewType.List));
        }
        /// <summary>
        /// An overridable method that is called before the <see cref="View"/> is generated.
        /// </summary>
        /// <remarks>
        ///     By default, it simply calls the <see cref="ViewGenerating"/> event.
        /// </remarks>
        protected virtual void OnViewGenerating()
        {
            this.ViewGenerating?.Invoke(this, EventArgs.Empty);
        }

        #endregion
    }
}
