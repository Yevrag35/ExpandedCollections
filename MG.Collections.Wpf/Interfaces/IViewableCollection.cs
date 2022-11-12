using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace MG.Collections.Wpf
{
    /// <summary>
    /// An interface representing a generic collection for WPF applications that hosts its own 
    /// <see cref="ICollectionView"/> of itself.
    /// </summary>
    public interface IViewableCollection : IEnumerable, INotifyCollectionChanged, INotifyViewGenerated, INotifyViewGenerating
    {
        /// <summary>
        /// Indicates whether <see cref="View"/> has been generated.
        /// </summary>
        bool IsViewGenerated { get; }
        /// <summary>
        /// Represents the current <see cref="IViewableCollection"/> as view for grouping, sorting,
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
