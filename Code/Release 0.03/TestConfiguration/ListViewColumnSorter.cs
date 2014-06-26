/**
* Smoke Tester Tool : Post deployment smoke testing tool.
* 
* http://www.stephenhaunts.com
* 
* This file is part of Smoke Tester Tool.
* 
* Smoke Tester Tool is free software: you can redistribute it and/or modify it under the terms of the
* GNU General Public License as published by the Free Software Foundation, either version 2 of the
* License, or (at your option) any later version.
* 
* Smoke Tester Tool is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* 
* See the GNU General Public License for more details <http://www.gnu.org/licenses/>.
* 
* Curator: Stephen Haunts
*/
using System;
using System.Collections;
using System.Windows.Forms;

namespace TestConfiguration
{
    public class ListViewItemSorter : IComparer
    {
        private int _sortColumn;
        private SortOrder _sortOrder;

        public ListViewItemSorter()
        {
            _sortColumn = 0;
            _sortOrder = SortOrder.Ascending;
        }

        public ListViewItemSorter(int column, SortOrder order)
        {
            _sortColumn = column;
            _sortOrder = order;
        }

        public int Compare(object x, object y)
        {
            var returnValue = String.CompareOrdinal(((ListViewItem)x).SubItems[_sortColumn].Text, ((ListViewItem)y).SubItems[_sortColumn].Text);

            if (_sortOrder == SortOrder.Descending)
            {
                returnValue *= -1;
            }

            return returnValue;
        }

        public int SortColumn
        {
            get
            {
                return _sortColumn;
            }
            set
            {
                _sortColumn = value;
            }
        }

        public SortOrder SortOrder 
        {
            get
            {
                return _sortOrder;
            }
            set
            {
                _sortOrder = value;
            } 
        }
    }
}
