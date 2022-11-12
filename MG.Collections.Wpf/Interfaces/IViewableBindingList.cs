using System.Collections;
using System.Collections.Specialized;
using System.Windows.Data;

namespace MG.Collections.Wpf.Interfaces
{
    /// <summary>
    /// An interface representing a non-generic list for WPF applications that hosts its own 
    /// <see cref="BindingListCollectionView"/> of itself.
    /// </summary>
    public interface IViewableBindingList : IList, INotifyCollectionChanged, INotifyViewGenerated, INotifyViewGenerating
    {
        /// <summary>
        /// Indicates whether <see cref="View"/> has been generated.
        /// </summary>
        bool IsViewGenerated { get; }
        /// <summary>
        /// Represents the current <see cref="IViewableBindingList"/> as a list view for grouping, sorting,
        /// filtering, and navigating as a data collection.
        /// </summary>
        /// <remarks>
        ///     This is <see langword="null"/> until after calling <see cref="GenerateView"/>.
        /// </remarks>
        BindingListCollectionView View { get; }

        /// <summary>
        /// Generates the <see cref="BindingListCollectionView"/> and defines it as <see cref="View"/>.
        /// </summary>
        void GenerateView();
    }
}
