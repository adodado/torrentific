// ***********************************************************************
// Assembly         : Torrentific
// Author           : Admir Cosic
// Created          : 02-07-2017
//
// Last Modified By : Admir Cosic
// Last Modified On : 02-07-2017
// ***********************************************************************
// <copyright file="CustomDataGrid.cs" company="None">
//     Copyright ©  2017
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace Torrentific.Controls
{
    /// <summary>
    /// Extended version of the default DataGrid which implements support for 'SelectedItems' as bindable property.
    /// </summary>
    /// <seealso cref="System.Windows.Controls.DataGrid" />
    public class CustomDataGrid : DataGrid
    {
        /// <summary>
        /// The selected items list property
        /// </summary>
        public static readonly DependencyProperty SelectedItemsListProperty =
            DependencyProperty.Register("SelectedItemsList", typeof(IList), typeof(CustomDataGrid),
                new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDataGrid"/> class.
        /// </summary>
        public CustomDataGrid()
        {
            SelectionChanged += CustomDataGrid_SelectionChanged;
        }

        /// <summary>
        /// Gets or sets the selected items list.
        /// </summary>
        /// <value>The selected items list.</value>
        public IList SelectedItemsList
        {
            get { return (IList) GetValue(SelectedItemsListProperty); }
            set { SetValue(SelectedItemsListProperty, value); }
        }

        /// <summary>
        /// Handles the SelectionChanged event of the CustomDataGrid control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="SelectionChangedEventArgs"/> instance containing the event data.</param>
        private void CustomDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedItemsList = SelectedItems;
        }
    }
}