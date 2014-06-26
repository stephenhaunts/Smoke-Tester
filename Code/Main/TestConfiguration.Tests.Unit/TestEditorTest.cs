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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ConfigurationTests.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestConfiguration.Forms;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace TestConfiguration.Tests.Unit
{
    [TestClass]
    public class TestEditorTest
    {
        private TestEditor_Accessor _testEditorAccessor;

        public TestContext TestContext { private get; set; }

        [TestInitialize]
        public void MyTestInitialize()
        {
            _testEditorAccessor = new TestEditor_Accessor();
        }

        [TestMethod]
        public void TestEditorConstructorTest()
        {
            var target = new TestEditor(false);
            Assert.IsNotNull(target);
        }

        [TestMethod]
        public void ConstructorToCreateTestFromTemplateTest()
        {
            const bool fromTemplate = true;

            _testEditorAccessor = new TestEditor_Accessor(fromTemplate);

            Assert.IsNotNull(_testEditorAccessor._configurationTestSuite);
        }

        [TestMethod]
        public void ConstructorToNotCreateTestFromTemplateTest()
        {
            bool fromTemplate = false;

            _testEditorAccessor = new TestEditor_Accessor(fromTemplate);

            Assert.IsNull(_testEditorAccessor._configurationTestSuite);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void CreateTestMenusTest()
        {
            _testEditorAccessor.CreateTestMenus();
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void AddTestToListTest()
        {
            Test test = new FileExistsTest();

            _testEditorAccessor.AddTestToList(test);

            Assert.AreEqual(1, _testEditorAccessor.lstListOfTests.Items.Count);
        }

        [TestMethod]
        public void DefaultConstructorTest()
        {
            var target = new TestEditor();
            Assert.IsNotNull(target);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void GetSelectedIndexFromListControlTest()
        {
            var listBox = new ListBox();
            listBox.Items.AddRange(new object[]{new FileExistsTest(), new FolderExistsTest(), new RegistryKeyTest()});      
            listBox.SelectedItem = listBox.Items[2];

            object sender = listBox;
            int[] expected = new int[] { 2 };
            int[] actual = TestEditor_Accessor.GetSelectedIndexFromListControl(sender);

            Assert.AreEqual(expected.Length, actual.Length); 
            Assert.AreEqual(expected[0], actual[0]);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void GetSelectedIndicesTest()
        {
            _testEditorAccessor.lstListOfTests = TestUtil.ListBoxWithThreeItems;
            _testEditorAccessor.lstListOfTests.SelectedItem = _testEditorAccessor.lstListOfTests.Items[2];
            int[] expected = new int[] { 2 }; ;
            int[] actual = _testEditorAccessor.GetSelectedIndices();
            Assert.AreEqual(expected.Length, actual.Length);
            Assert.AreEqual(expected[0], actual[0]);
        }      

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void GetTestTypesTest()
        {
            var expected = TestUtil.TestTypes.ToList();
            var actual = TestEditor_Accessor.GetTestTypes();
            var actualTypes = actual.ToList();
            expected.ForEach(t => Assert.IsTrue(actualTypes.Contains(t)));
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void InitializeTestSuiteTest()
        {
            _testEditorAccessor.InitializeTestSuite();
            Assert.IsNotNull(_testEditorAccessor._configurationTestSuite);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void LoadTestsFromExamplesTest()
        {
            _testEditorAccessor.LoadTestsFromExamples();
            Assert.IsNotNull(_testEditorAccessor._configurationTestSuite);
            Assert.IsNotNull(_testEditorAccessor._configurationTestSuite.Tests);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void MoveListItemsTest()
        {
            _testEditorAccessor.lvwListOfTest.Items.AddRange(TestUtil.ThreeListViewItems);
            _testEditorAccessor.lstListOfTests.Items.AddRange(TestUtil.ThreeTestObjects);
            _testEditorAccessor.lstListOfTests.SelectedItem = _testEditorAccessor.lstListOfTests.Items[1];
            _testEditorAccessor.MoveListItems("up");
            Assert.AreEqual("FolderExistTest", (_testEditorAccessor.lstListOfTests.Items[0] as Test).TestName);
            Assert.AreEqual("FileExistTest", (_testEditorAccessor.lstListOfTests.Items[1] as Test).TestName);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void MoveSelectedItemsTest()
        {
            _testEditorAccessor.lvwListOfTest.Items.AddRange(TestUtil.ThreeListViewItems);
            _testEditorAccessor.lstListOfTests.Items.AddRange(TestUtil.ThreeTestObjects);
            int[] selectedIndices = new int[] { 1 };
            var moveType = TestEditor_Accessor.MoveType.Up;
            _testEditorAccessor.MoveSelectedItems(selectedIndices, moveType);
            Assert.AreEqual("FolderExistTest", (_testEditorAccessor.lstListOfTests.Items[0] as Test).TestName);
            Assert.AreEqual("FileExistTest", (_testEditorAccessor.lstListOfTests.Items[1] as Test).TestName);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void RemoveAllTestsFromListTest()
        {
            _testEditorAccessor.lvwListOfTest.Items.AddRange(TestUtil.ThreeListViewItems);
            _testEditorAccessor.lstListOfTests.Items.AddRange(TestUtil.ThreeTestObjects);
            _testEditorAccessor._configurationTestSuite = TestUtil.ConfigurationTestSuiteWithThreeTests;
            _testEditorAccessor.RemoveAllTestsFromList();
            Assert.AreEqual(0, _testEditorAccessor.lvwListOfTest.Items.Count);
            Assert.AreEqual(0, _testEditorAccessor.lstListOfTests.Items.Count);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void RemoveTestsFromListTest()
        {
            _testEditorAccessor.lvwListOfTest.Items.AddRange(TestUtil.ThreeListViewItems);
            _testEditorAccessor.lstListOfTests.Items.AddRange(TestUtil.ThreeTestObjects);
            int[] selectedIndices = new int[] { 1 };
            _testEditorAccessor.RemoveTestsFromList(selectedIndices);
            Assert.AreEqual(2, _testEditorAccessor.lvwListOfTest.Items.Count);
            Assert.AreEqual(2, _testEditorAccessor.lstListOfTests.Items.Count);
        }


        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void RunSelectedTestsTest()
        {
            _testEditorAccessor.lvwListOfTest.Items.AddRange(TestUtil.ThreeListViewItems);
            int[] selectedIndices = new int[]{1};
            _testEditorAccessor.RunSelectedTests(selectedIndices);
            Assert.IsTrue(_testEditorAccessor.lvwListOfTest.Items[1].Text != string.Empty);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void RunTestTest()
        {
            _testEditorAccessor.lvwListOfTest.Items.AddRange(TestUtil.ThreeListViewItems);
            _testEditorAccessor.RunTest();
            Assert.IsTrue(_testEditorAccessor.lvwListOfTest.Items[0].Text != string.Empty);
            Assert.IsTrue(_testEditorAccessor.lvwListOfTest.Items[1].Text != string.Empty);
            Assert.IsTrue(_testEditorAccessor.lvwListOfTest.Items[2].Text != string.Empty);
        }
        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void ShowIdleStatusTest()
        {
            _testEditorAccessor.ShowIdleStatus();
            Assert.AreEqual("Ready...", _testEditorAccessor.tslStatus.Text);
        }
    
        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void ShowStatusTest()
        {
            const string status = "Counting...";
            _testEditorAccessor.ShowStatus(status);
            Assert.AreEqual(status, _testEditorAccessor.tslStatus.Text);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void RunTestFromListItemTest()
        {
            var item = TestUtil.ThreeListViewItems[0];
            TestEditor_Accessor.RunTestFromListItem(item);
            Assert.IsTrue(item.Text != string.Empty);
            Assert.IsTrue(item.SubItems[2].Text != string.Empty);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void TestMenu_ClickTest()
        {
            object sender = TestUtil.FileExistTestMenuItem;
            var e = new EventArgs();
            _testEditorAccessor.TestMenu_Click(sender, e);
            Assert.AreEqual(1, _testEditorAccessor.lstListOfTests.Items.Count);
        }

        [TestMethod]
        [DeploymentItem("TestConfiguration.exe")]
        public void WriteFileTest()
        {
            _testEditorAccessor._configurationTestSuite = TestUtil.ConfigurationTestSuiteWithThreeTests;
            string fileName = Path.Combine(TestContext.DeploymentDirectory,string.Format("FILE{0:ddMMyyyyhhmmss}.xml",DateTime.Now));

            _testEditorAccessor.WriteXmlFile(fileName);
            
            Assert.IsTrue(File.Exists(fileName));
        }

        [TestMethod()]
        [DeploymentItem("TestConfiguration.exe")]
        public void LoadTestsIfNotNullConfigNotInitializedTest()
        {
            _testEditorAccessor.LoadTestsToList();
            Assert.AreEqual(0, _testEditorAccessor.lstListOfTests.Items.Count);
        }

        [TestMethod()]
        [DeploymentItem("TestConfiguration.exe")]
        public void LoadTestsIfNotNullConfigInitializedTest()
        {
            _testEditorAccessor._configurationTestSuite = TestUtil.ConfigurationTestSuiteWithThreeTests;
            _testEditorAccessor.LoadTestsToList();
            Assert.AreEqual(3, _testEditorAccessor.lstListOfTests.Items.Count);
        }
    }
}
