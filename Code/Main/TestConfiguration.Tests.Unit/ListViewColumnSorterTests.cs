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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;

namespace TestConfiguration.Tests.Unit
{
    [TestClass]
    public class ListViewColumnSorterTests
    {
        [TestMethod]
        public void CompareReturnsNegative1()
        {
            var listViewItemSorter = new ListViewItemSorter();
            var listViewItem1 = new ListViewItem("A");
            var listViewItem2 = new ListViewItem("B");

            var actual = listViewItemSorter.Compare(listViewItem1, listViewItem2);
            Assert.AreEqual(-1, actual);
        }
        [TestMethod]
        public void CompareReturnsPositive1()
        {
            var listViewItemSorter = new ListViewItemSorter();
            listViewItemSorter.SortOrder = SortOrder.Descending;
            var listViewItem1 = new ListViewItem("A");
            var listViewItem2 = new ListViewItem("B");

            var actual = listViewItemSorter.Compare(listViewItem1, listViewItem2);
            Assert.AreEqual(1, actual);
        }
        [TestMethod]
        public void GetSortOrderReturnsAscending()
        {
            var listViewItemSorter = new ListViewItemSorter();
            Assert.AreEqual(SortOrder.Ascending, listViewItemSorter.SortOrder);
        }
        [TestMethod]
        public void GetSortColumnReturns1()
        {
            var listViewItemSorter = new ListViewItemSorter();
            listViewItemSorter.SortColumn = 1;
            Assert.AreEqual(1, listViewItemSorter.SortColumn);
        }
        [TestMethod]
        public void ConstructorInitializesSortColumnAndSortOrder()
        {
            var listViewItemSorter = new ListViewItemSorter(1, SortOrder.Descending);
            Assert.AreEqual(SortOrder.Descending, listViewItemSorter.SortOrder);
        }
    }
}
