using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace MG.Collections.Wpf
{
    /// <summary>
    /// A class that is inherited from <see cref="ObservableCollection{T}"/> and has members that can generate and store
    /// a <see cref="ICollectionView"/> that represents it.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public class ObservableViewedCollection<T> : ObservableCollection<T>, IObservableList<T>
    {
        #region PRIVATE FIELDS/CONSTANTS
        private ICollectionView _backingView;

        #endregion

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
            get => null != _backingView;
        }
        /// <summary>
        /// Represents the current <see cref="ObservableViewedCollection{T}"/> as a collection view for grouping, sorting,
        /// filtering, and navigating as a data collection.
        /// </summary>
        /// <remarks>
        ///     This is <see langword="null"/> until after calling <see cref="GenerateView"/>.
        /// </remarks>
        public ICollectionView View
        {
            get => _backingView;
        }

        #endregion

        /// <summary>
        /// Generates the <see cref="ICollectionView"/> and defines it as <see cref="View"/> for the current 
        /// <see cref="ObservableViewedCollection{T}"/>.
        /// </summary>
        public void GenerateView()
        {
            this.OnViewGenerating();
            _backingView = CollectionViewSource.GetDefaultView(this);
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
            this.ViewGenerated?.Invoke(this, new ViewGeneratedEventArgs(_backingView));
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
    }
}
