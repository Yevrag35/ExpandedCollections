using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace MG.Collections.Wpf
{
    /// <summary>
    /// An interface representing a generic observable list for WPF applications that supports live filtering,
    /// sorting, and grouping of its data.
    /// </summary>
    public interface IObservableList<T> : IList<T>, IList, INotifyCollectionChanged
    {
        /// <summary>
        /// Occurs when a new <see cref="ICollectionView"/> is generated.
        /// </summary>
        event ViewGeneratedEventHandler ViewGenerated;
        /// <summary>
        /// Occurs when a new <see cref="ICollectionView"/> is currently being generated.
        /// </summary>
        event EventHandler ViewGenerating;

        /// <summary>
        /// Indicates whether <see cref="View"/> has been generated.
        /// </summary>
        bool IsViewGenerated { get; }
        /// <summary>
        /// Represents the current <see cref="UniqueObservableList{T}"/> as a collection view for grouping, sorting,
        /// filtering, and navigating as a data collection.
        /// </summary>
        /// <remarks>
        ///     This is <see langword="null"/> until after calling <see cref="GenerateView"/>.
        /// </remarks>
        ICollectionView View { get; }

        /// <summary>
        /// Generates the <see cref="ICollectionView"/> and defines it as <see cref="View"/>.
        /// </summary>
        void GenerateView();
    }
}
