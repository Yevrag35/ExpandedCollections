using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

#pragma warning disable IDE0130

namespace MG.Collections.Wpf
{
    /// <summary>
    /// Represents a method that handles the ? event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Information about the <see cref="ICollectionView"/> that was generated.</param>
    public delegate void ViewGeneratedEventHandler(object sender, ViewGeneratedEventArgs e);
    /// <summary>
    /// An <see cref="EventArgs"/> class containing information that an <see cref="IViewableList"/> generated
    /// an <see cref="ICollectionView"/>.
    /// </summary>
    public class ViewGeneratedEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates the type of view that was generated.
        /// </summary>
        public GeneratedViewType ViewType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewGeneratedEventArgs"/> class with the optional information
        /// about what kind of view was generated.
        /// </summary>
        /// <param name="viewType">The type of view that was generated.  The default is <see cref="GeneratedViewType.Unknown"/>.</param>
        public ViewGeneratedEventArgs(GeneratedViewType viewType = GeneratedViewType.Unknown)
            : base()
        {
            this.ViewType = viewType;
        }
    }

    /// <summary>
    /// Describes the type of <see cref="ICollectionView"/> generated.
    /// </summary>
    public enum GeneratedViewType
    {
        /// <summary>
        /// No information about the <see cref="ICollectionView"/> was provided.
        /// </summary>
        Unknown = -1,
        /// <summary>
        /// The view is of the <see cref="ListCollectionView"/> type.
        /// </summary>
        List,
        /// <summary>
        /// The view is of the <see cref="BindingListCollectionView"/> type.
        /// </summary>
        BindingList,
        /// <summary>
        /// The view is of the <see cref="System.Windows.Controls.ItemCollection"/> type.
        /// </summary>
        ItemCollection
    }
}
