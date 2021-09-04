using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
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
    /// An <see cref="EventArgs"/> class containing information that an <see cref="IObservableList{T}"/> generated
    /// an <see cref="ICollectionView"/>.
    /// </summary>
    public class ViewGeneratedEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates whether the newly generated <see cref="ICollectionView"/> implements <see cref="IList"/>.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="ICollectionView"/> is a list view and, thus, implements 
        ///     <see cref="IList"/>; otherwise, <see langword="false"/>.  This property will also return 
        ///     <see langword="false"/> if no information about the generated <see cref="ICollectionView"/> was provided.
        /// </returns>
        public bool IsListView { get; }

        /// <summary>
        /// The default constructor. Intializes a new instance of the <see cref="ViewGeneratedEventArgs"/> class.
        /// </summary>
        /// <remarks>
        ///     Because no information about the generated <see cref="ICollectionView"/> was provided, the instance
        ///     assumes the view does not implement <see cref="IList"/>.
        /// </remarks>
        public ViewGeneratedEventArgs()
            : base()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewGeneratedEventArgs"/> class and uses the provided 
        /// <see cref="ICollectionView"/> to determine if it implements <see cref="IList"/> or not.
        /// </summary>
        /// <param name="view">The view that was generated.</param>
        public ViewGeneratedEventArgs(ICollectionView view)
            : base()
        {
            this.IsListView = view is ListCollectionView;
        }
    }
}
