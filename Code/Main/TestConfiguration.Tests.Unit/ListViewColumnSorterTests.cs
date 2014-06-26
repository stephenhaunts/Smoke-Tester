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
