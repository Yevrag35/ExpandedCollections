using System;
using System.ComponentModel;

namespace MG.Collections.Wpf
{
    /// <summary>
    /// Notifies listeners when a new <see cref="ICollectionView"/> is in the process of being generated.
    /// </summary>
    public interface INotifyViewGenerating
    {
        /// <summary>
        /// Occurs when a new <see cref="ICollectionView"/> is currently being generated.
        /// </summary>
        event EventHandler ViewGenerating;
    }
}
