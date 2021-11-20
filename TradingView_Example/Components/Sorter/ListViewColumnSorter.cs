using System;
using System.Collections;
using System.Windows.Forms;
using TradingView_Example.Properties;

namespace TradingView_Example.Components.Sorter
{
    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ListViewColumnSorter : IComparer
    {
        /// <summary>
        /// The event does not have any data, so EventHandler is adequate
        /// </summary>
        private EventHandler onSortChanged;

        /// <summary>
        /// Define the event member using the event keyword
        /// </summary>
        public event EventHandler SortChanged
        {
            add
            {
                onSortChanged += value;
            }
            remove
            {
                onSortChanged -= value;
            }
        }

        /// <summary>
        /// Specifies the column to be sorted
        /// </summary>
        private int ColumnToSort;

        /// <summary>
        /// Specifies the order in which to sort (i.e. 'Ascending').
        /// </summary>
        public SortOrder OrderOfSort
        {
            get { return (SortOrder)Settings.Default.ListViewColumnSorterOrderOfSort; }
            set 
            {
                Settings.Default.ListViewColumnSorterOrderOfSort = (int)value;
                Settings.Default.Save();
            }
        }


        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private readonly CaseInsensitiveComparer ObjectCompare;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ListViewColumnSorter()
        {
            // Initialize the column to '0'
            ColumnToSort = 0;

            // Initialize the CaseInsensitiveComparer object
            ObjectCompare = new CaseInsensitiveComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(object x, object y)
        {
            int compareResult;
            ListViewItem listviewX, listviewY;

            // Cast the objects to be compared to ListViewItem objects
            listviewX = (ListViewItem)x;
            listviewY = (ListViewItem)y;

            // Deal with NEWKEY and AUTOKEY
            if (listviewX.SubItems[0].Text == "*NEW" || listviewY.SubItems[0].Text == "*NEW") 
                return -1;

            if (listviewX.SubItems[0].Text == "[AUTO]") 
                return listviewY.SubItems[0].Text == "*NEW" ? 1 : -1;

            if (listviewY.SubItems[0].Text == "[AUTO]") 
                return listviewX.SubItems[0].Text == "*NEW" ? 1 : -1;

            // Compare the two items
            compareResult = ObjectCompare.Compare(listviewX.SubItems[ColumnToSort].Text, listviewY.SubItems[ColumnToSort].Text);

            // Calculate correct return value based on object comparison
            if (OrderOfSort == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                onSortChanged?.Invoke(this, new EventArgs());
                return compareResult;
            }
            else if (OrderOfSort == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                onSortChanged?.Invoke(this, new EventArgs());
                return -compareResult;
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }

        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public int SortColumn
        {
            set
            {
                ColumnToSort = value;
            }
            get
            {
                return ColumnToSort;
            }
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            set
            {
                OrderOfSort = value;
            }
            get
            {
                return OrderOfSort;
            }
        }
    }
}