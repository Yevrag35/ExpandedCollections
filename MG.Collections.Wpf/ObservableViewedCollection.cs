using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

#pragma warning disable IDE0130

namespace MG.Collections.Wpf
{
    /// <summary>
    /// A dynamic data collection that is inherited from <see cref="ObservableCollection{T}"/> which provides
    /// notifications when items get added, removed, or when the collection is refreshed.  It also has members 
    /// that can generate and store an <see cref="ICollectionView"/> that represents it.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ObservableViewedCollection<T> : ObservableCollection<T>, IObservableList<T>
    {
        #region EVENTS
        /// <summary>
        /// Occurs when a new <see cref="ICollectionView"/> is generated for the <see cref="ObservableViewedCollection{T}"/>.
        /// </summary>
        public event ViewGeneratedEventHandler ViewGenerated;
        /// <summary>
        /// Occurs when a new <see cref="ICollectionView"/> is currently being generated for <see cref="ObservableViewedCollection{T}"/>.
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
        public ICollectionView View { get; private set; }

        #endregion

        #region CONSTRUCTORS
        public ObservableViewedCollection()
            : base()
        {
        }

        public ObservableViewedCollection(IEnumerable<T> items)
            : base(items)
        {
        }

        public ObservableViewedCollection(List<T> list)
            : base(list)
        {
        }

        #endregion

        #region VIEW METHODS

        /// <summary>
        /// Generates the <see cref="ICollectionView"/> and defines it as <see cref="View"/> for the current 
        /// <see cref="ObservableViewedCollection{T}"/>.
        /// </summary>
        public void GenerateView()
        {
            this.OnViewGenerating();
            this.View = CollectionViewSource.GetDefaultView(this);
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
            this.ViewGenerated?.Invoke(this, new ViewGeneratedEventArgs(this.View));
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
