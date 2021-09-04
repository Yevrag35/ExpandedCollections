using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace MG.Collections.Wpf
{
    /// <summary>
    /// Represents a method that handles the <see cref=""/>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void ViewGeneratedEventHandler(object sender, ViewGeneratedEventArgs e);
    /// <summary>
    /// An <see cref="EventArgs"/> class containing information that an <see cref="IObservableList{T}"/> generated
    /// an <see cref="ICollectionView"/>.
    /// </summary>
    public class ViewGeneratedEventArgs : EventArgs
    {
        /// <summary>
        /// Indicates whether the newly generated <see cref="ICollectionView"/> is a <see cref="ListCollectionView"/>.
        /// </summary>
        /// <returns>
        ///     <see langword="true"/> if the <see cref="ICollectionView"/> is a list view and, thus, implements 
        ///     <see cref="IList"/>; otherwise, <see langword="false"/>.
        /// </returns>
        public bool IsListView { get; }

        /// <summary>
        /// The default constructor. Intializes a new instance of the <see cref="ViewGeneratedEventArgs"/> class.
        /// </summary>
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
            this.IsListView = view is ListCollectionView || ImplementsIList(view);
        }

        private static bool ImplementsIList(ICollectionView view)
        {
            Type iList = typeof(IList);
            return null != view
                .GetType()
                    .GetInterfaces()
                        .FirstOrDefault(x =>
                            iList
                                .Equals(x));
        }
    }
}
