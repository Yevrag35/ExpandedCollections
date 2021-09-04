using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;

#pragma warning disable IDE0130

namespace MG.Collections.Wpf
{
    /// <summary>
    /// An interface representing a generic list for WPF applications that hosts its own 
    /// <see cref="ListCollectionView"/> of itself.
    /// </summary>
    public interface IViewableList : IList, INotifyCollectionChanged, INotifyViewGenerated, INotifyViewGenerating
    {
        /// <summary>
        /// Indicates whether <see cref="View"/> has been generated.
        /// </summary>
        bool IsViewGenerated { get; }
        /// <summary>
        /// Represents the current <see cref="IViewableList"/> as a list view for grouping, sorting,
        /// filtering, and navigating as a data collection.
        /// </summary>
        /// <remarks>
        ///     This is <see langword="null"/> until after calling <see cref="GenerateView"/>.
        /// </remarks>
        ListCollectionView View { get; }

        /// <summary>
        /// Generates the <see cref="ListCollectionView"/> and defines it as <see cref="View"/>.
        /// </summary>
        void GenerateView();
    }
}
