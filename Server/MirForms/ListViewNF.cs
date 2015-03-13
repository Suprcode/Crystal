using System;
using System.Collections;
using System.Windows.Forms;

namespace Server
{
    public class ListViewNF : ListView
    {
        public class ListViewColumnSorter : IComparer
        {
            private int _columnToSort;

            private SortOrder _orderOfSort;

            private readonly CaseInsensitiveComparer _objectCompare;

            public ListViewColumnSorter()
            {
                // Initialize the column to '0'
                _columnToSort = 0;

                // Initialize the sort order to 'none'
                _orderOfSort = SortOrder.None;

                // Initialize the CaseInsensitiveComparer object
                _objectCompare = new CaseInsensitiveComparer();
            }

            public int Compare(object x, object y)
            {
                // Cast the objects to be compared to ListViewItem objects
                ListViewItem listviewX = (ListViewItem)x;
                ListViewItem listviewY = (ListViewItem)y;

                // Compare the two items
                int compareResult = _objectCompare.Compare(listviewX.SubItems[_columnToSort].Text, listviewY.SubItems[_columnToSort].Text);

                // Calculate correct return value based on object comparison
                if (_orderOfSort == SortOrder.Ascending)
                {
                    // Ascending sort is selected, return normal result of compare operation
                    return compareResult;
                }
                if (_orderOfSort == SortOrder.Descending)
                {
                    // Descending sort is selected, return negative result of compare operation
                    return (-compareResult);
                }
                // Return '0' to indicate they are equal
                return 0;
            }

            public int SortColumn
            {
                set
                {
                    _columnToSort = value;
                }
                get
                {
                    return _columnToSort;
                }
            }

            public SortOrder Order
            {
                set
                {
                    _orderOfSort = value;
                }
                get
                {
                    return _orderOfSort;
                }
            }

        }
        private readonly Timer _itemSelectionChangedTimer = new Timer();
        private readonly Timer _selectedIndexChangedTimer = new Timer();

        private ListViewItemSelectionChangedEventArgs _itemSelectionChangedEventArgs;
        private EventArgs _selectedIndexChangedEventArgs;
        private readonly ListViewColumnSorter _sorter;

        public ListViewNF()
        {
            _itemSelectionChangedTimer.Interval = 1;
            _selectedIndexChangedTimer.Interval = 1;

            _itemSelectionChangedTimer.Tick += (sender, e) =>
            {
                OnItemSelectionChanged(_itemSelectionChangedEventArgs);
                _itemSelectionChangedEventArgs = null;
            };
            _selectedIndexChangedTimer.Tick += (sender, e) =>
            {
                OnSelectedIndexChanged(_selectedIndexChangedEventArgs);
                _selectedIndexChangedEventArgs = null;
            };

            //Activate double buffering
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);

            //Enable the OnNotifyMessage event so we get a chance to filter out 
            // Windows messages before they get to the form's WndProc
            SetStyle(ControlStyles.EnableNotifyMessage, true);

            _sorter = new ListViewColumnSorter();
            ListViewItemSorter = _sorter;
            ColumnClick += ListViewNF_ColumnClick;
        }

        void ListViewNF_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column == _sorter.SortColumn)
            {
                _sorter.Order = _sorter.Order == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            }
            else
            {
                _sorter.SortColumn = e.Column;
                _sorter.Order = SortOrder.Ascending;
            }
            Sort();
        }

        protected override void OnNotifyMessage(Message m)
        {
            //Filter out the WM_ERASEBKGND message
            if (m.Msg != 0x14)
            {
                base.OnNotifyMessage(m);
            }
        }



        protected override void OnItemSelectionChanged(ListViewItemSelectionChangedEventArgs e)
        {
            if (_itemSelectionChangedTimer.Enabled)
            {
                _itemSelectionChangedTimer.Stop();
                base.OnItemSelectionChanged(e);
            }
            else
            {
                _itemSelectionChangedEventArgs = e;
                _itemSelectionChangedTimer.Start();
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (_selectedIndexChangedTimer.Enabled)
            {
                _selectedIndexChangedTimer.Stop();
                base.OnSelectedIndexChanged(e);
            }
            else
            {
                _selectedIndexChangedEventArgs = e;
                _selectedIndexChangedTimer.Start();
            }
        }

    }
}