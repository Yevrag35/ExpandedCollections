using System.ComponentModel;

#pragma warning disable IDE0130

namespace MG.Collections.Wpf
{
    /// <summary>
    /// Notifies listeners when a new <see cref="ICollectionView"/> is generated.
    /// </summary>
    public interface INotifyViewGenerated
    {
        /// <summary>
        /// Occurs when a new <see cref="ICollectionView"/> is generated.
        /// </summary>
        event ViewGeneratedEventHandler ViewGenerated;
    }
}
