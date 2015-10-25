using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace LibraryEditor
{
    internal class FixedListView : ListView
    {
        private int _lastItemIndexClicked1 = -1;
        private int _lastItemIndexClicked2 = -1;
        private bool _shiftOn;
        protected override void OnKeyDown(KeyEventArgs e)
        {
            _shiftOn = e.Shift;
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            _shiftOn = false;
            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            ListViewItem it = GetItemAt(e.X, e.Y);

            if (it == null)
            {
                //lastItemIndexClicked2 = -1;
            }
            else
            {
                _lastItemIndexClicked2 = it.Index;
            }
            if (!_shiftOn || (_lastItemIndexClicked1 < 0))
            {
                _lastItemIndexClicked1 = _lastItemIndexClicked2;
            }
            base.OnMouseDown(e);
        }

        protected override void OnVirtualItemsSelectionRangeChanged(
            ListViewVirtualItemsSelectionRangeChangedEventArgs e)
        {
            try
            {
                int start = _lastItemIndexClicked1;
                int end = _lastItemIndexClicked2;
                if (end < start)
                {
                    int temp = start;
                    start = end;
                    end = temp;
                }
                if ((start >= 0) && (end >= 0))
                {
                    ArrayList toRemove = new ArrayList();
                    foreach (int index in SelectedIndices)
                    {
                        if ((index < start) || (index > end))
                            toRemove.Add(index);
                    }
                    if (toRemove.Count > 0)
                    {
                        foreach (int index in toRemove)
                        {
                            SelectedIndices.Remove(index);
                        }
                    }
                }
                ListViewVirtualItemsSelectionRangeChangedEventArgs te =
                    new ListViewVirtualItemsSelectionRangeChangedEventArgs(start,
                                                                           end, e.IsSelected);
                base.OnVirtualItemsSelectionRangeChanged(te);
            }
            catch
            {
            }
        }
    }
}
