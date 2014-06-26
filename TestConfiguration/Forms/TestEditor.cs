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
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Common.Xml;
using CommonCode.ReportWriter;
using ConfigurationTests;
using ConfigurationTests.Attributes;
using ConfigurationTests.Enums;
using ConfigurationTests.Tests;

namespace TestConfiguration.Forms
{
    public partial class TestEditor : Form
    {
        private enum MoveType
        {
            Up = -1,
            Down = 1
        }

        private static ReportBuilder _reportBuilder;
        private ConfigurationTestSuite _configurationTestSuite;
        private string _filename;
        private ListViewItemSorter _listViewItemSorter;

        public TestEditor()
        {
            InitializeComponent();
            _reportBuilder = new ReportBuilder();
                    _listViewItemSorter = new ListViewItemSorter();
                    lvwListOfTest.ListViewItemSorter = _listViewItemSorter;
            
        }

        public TestEditor(ConfigurationTestSuite configurationTestSuite)
            : this()
        {
            _configurationTestSuite = configurationTestSuite;
        }

        public TestEditor(bool fromTemplate)
            : this()
        {
            if (fromTemplate)
                LoadTestsFromExamples();
        }

        private static IEnumerable<Type> GetTestTypes()
        {
            IEnumerable<Type> testsTypes = typeof (Test).Assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof (Test)) && !type.IsAbstract);

            return testsTypes;
        }

        private static IEnumerable<Type> GetPluginTestTypes()
        {
            var defaultPluginPath = Path.Combine(Application.StartupPath, "Plugins");
            var pluginPaths = new List<string> {defaultPluginPath};

            var path = ConfigurationManager.AppSettings["CommaSeparatedPluginPaths"];
            pluginPaths.AddRange(
                (path ?? string.Empty).Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).ToList());

            var testsTypes = new List<Type>();
            var paths = pluginPaths.Where(folder => folder.IsValidDirectory());

            foreach (var assembly in paths.SelectMany(folder => Directory.GetFiles(folder).Select(Assembly.LoadFile)))
            {
                testsTypes.AddRange(
                    assembly.GetTypes().Where(type => type.IsSubclassOf(typeof (Test)) && !type.IsAbstract));
            }

            return testsTypes;
        }

        private static IEnumerable<Type> GetAllTestTypes()
        {
            var testTypes = new List<Type>();

            testTypes.AddRange(GetTestTypes());
            testTypes.AddRange(GetPluginTestTypes());

            return testTypes;
        }

        private void LoadTestsFromExamples()
        {
            _configurationTestSuite = new ConfigurationTestSuite();
            var testTypes = GetAllTestTypes();

            foreach (var test in testTypes.OrderBy(c => c.Name).Select(type => Activator.CreateInstance(type) as Test))
            {
                _configurationTestSuite.Tests.AddRange(test.CreateExamples());
            }
        }

        private void CreateTestMenus()
        {
            var testCategories = Enum.GetNames(typeof (TestCategory));

            foreach (var menuItem in testCategories.Select(testCategory => new ToolStripMenuItem
            {
                Text = testCategory.Replace('_', ' '),
                Name = testCategory
            }))
            {
                tsbTests.DropDownItems.Add(menuItem);
            }

            foreach (var type in GetAllTestTypes().OrderBy(c => c.Name))
            {
                CreateTestMenuItem(type);
            }
        }

        private void CreateTestMenuItem(Type type)
        {
            var attribute = Attribute.GetCustomAttribute(type, typeof (TestCategoryAttribute));
            var testCategoryAttrybute = attribute as TestCategoryAttribute;

            if (testCategoryAttrybute == null) return;

            var categoryMenuItem =
                tsbTests.DropDownItems[testCategoryAttrybute.TestCatgory.ToString()] as ToolStripMenuItem;

            var menuItem = new ToolStripMenuItem
            {
                Text = type.Name,
                Tag = type
            };

            menuItem.Click += TestMenu_Click;
            tsbTests.DropDownItems.Add(menuItem);
            categoryMenuItem.DropDownItems.Add(menuItem);
        }

        private void AddTestToList(Test test)
        {
            lstListOfTests.Items.Add(test);

            var listViewItem = new ListViewItem
            {
                ImageIndex = -1,
                Tag = test,
                Text = ""
            };

            listViewItem.SubItems.Add(test.ToString());
            listViewItem.SubItems.Add("");
            listViewItem.SubItems.Add(lstListOfTests.Items.IndexOf(test).ToString());
            lvwListOfTest.Items.Add(listViewItem);


            lstListOfTests.SelectedItem = test;
            pgTestConfiguration.SelectedObject = test;

            UpdateUi();
        }

        private void UpdateUi()
        {
            var testCount = lstListOfTests.Items.Count;
            lblTotalTestCount.Text = string.Format(CultureInfo.CurrentUICulture, "{0} Test{1}", testCount,
                (testCount > 0 ? "s" : ""));

            var testFormat = lstListOfTests.SelectedItem != null ? " [{1}]" : "";
            Text = string.Format(CultureInfo.CurrentUICulture, "Test Configurations Editor - {0}" + testFormat,
                txtTestName.Text, lstListOfTests.SelectedItem ?? "");
        }

        private void UpdateSeletedItem()
        {
            lstListOfTests.BeginUpdate();
            var item = lstListOfTests.SelectedItem as Test;
            var index = lstListOfTests.Items.IndexOf(lstListOfTests.SelectedItem);
            lstListOfTests.Items[index] = item;
            lstListOfTests.EndUpdate();

            UpdateUi();
        }

        private void RemoveTestsFromList(int[] selectedIndices)
        {
            for (var i = selectedIndices.GetLowerBound(0); i <= selectedIndices.GetUpperBound(0); i++)
            {
                lstListOfTests.Items.RemoveAt(selectedIndices[i]);
                lvwListOfTest.Items.RemoveAt(selectedIndices[i]);
            }
        }

        private void RemoveAllTestsFromList()
        {
            if (_configurationTestSuite == null)
            {
                return;
            }

            lstListOfTests.Items.Clear();
            lvwListOfTest.Items.Clear();
            _configurationTestSuite.Tests = null;
        }

        private void InitializeTestSuite()
        {
            _configurationTestSuite = new ConfigurationTestSuite
            {
                Name = txtTestName.Text,
                Description = string.Empty
            };

            _configurationTestSuite.Tests.AddRange(lstListOfTests.Items.Cast<Test>());
        }

        private void SaveCurrentTestFile()
        {
            InitializeTestSuite();

            if (_filename != string.Empty && File.Exists(_filename))
            {
                WriteXmlFile(_filename);
            }
            else
            {
                SaveNewTestFile();
            }
        }

        private void SaveNewTestFile()
        {
            if (_configurationTestSuite == null)
            {
                InitializeTestSuite();
            }

            using (var dialog = new SaveFileDialog
                {
                    Title = @"Save Test Configuration File",
                    Filter = @"XML Configuration File | *.xml"
                })
            {
                dialog.FileOk += (s, e) =>
                {
                    if (e.Cancel) return;

                    _filename = ((SaveFileDialog) s).FileName;
                    WriteXmlFile(_filename);
                };

                dialog.ShowDialog();
            }
        }

        private void WriteXmlFile(string fileName)
        {
            var xmlString = _configurationTestSuite.ToXmlString();

            File.WriteAllText(fileName, xmlString, Encoding.Unicode);
        }

        private void RunTest()
        {
            SwitchToTestRunView();
            _reportBuilder.ClearEntries();

            foreach (ListViewItem item in lvwListOfTest.Items)
            {
                RunTestFromListItem(item);
            }

            ShowIdleStatus();
        }

        private void RunSelectedTests(int[] selectedIndices)
        {
            SwitchToTestRunView();
            _reportBuilder.ClearEntries();

            for (var i = selectedIndices.GetLowerBound(0); i <= selectedIndices.GetUpperBound(0); i++)
            {
                RunTestFromListItem(lvwListOfTest.Items[selectedIndices[i]]);
            }

            ShowIdleStatus();
        }

        private void SwitchToTestRunView()
        {
            tabMain.SelectedTab = tpgTestRun;
            tabMain.Refresh();
        }

        private void SwitchToConfigurationView()
        {
            tabMain.SelectedTab = tpgConfiguation;
            tabMain.Refresh();

            ShowStatus("Running Tests...");
        }

        private static void RunTestFromListItem(ListViewItem item)
        {
            var test = item.Tag as Test;
            DateTime startTime = DateTime.Now;
            DateTime stopTime;

            try
            {
                if (test != null) test.Run();
                stopTime = DateTime.Now;
                item.Text = @"Pass";
                item.ImageIndex = 0;
                item.SubItems[2].Text = string.Empty;

                var entry = new ReportEntry
                {
                    TestName = test.TestName,
                    Result = true,
                    TestStartTime = startTime,
                    TestStopTime = stopTime
                };

                _reportBuilder.AddEntry(entry);
            }
            catch (Exception e)
            {
                stopTime = DateTime.Now;
                item.Text = @"Fail";
                item.ImageIndex = 1;
                item.SubItems[2].Text = string.Format(CultureInfo.CurrentUICulture, "{0} - {1}", e.Source, e.Message);

                var entry = new ReportEntry
                {
                    TestName = test.TestName,
                    Result = false,
                    ErrorMessage = e.Message,
                    TestStartTime = startTime,
                    TestStopTime = stopTime
                };
                _reportBuilder.AddEntry(entry);
            }
        }

        private void MoveSelectedItems(int[] selectedIndices, MoveType moveType)
        {
            for (int i = selectedIndices.GetLowerBound(0); i <= selectedIndices.GetUpperBound(0); i++)
            {
                var item = lstListOfTests.Items[selectedIndices[i]];
                lstListOfTests.Items.RemoveAt(selectedIndices[i]);
                lstListOfTests.Items.Insert(selectedIndices[i] + ((int) moveType), item);
                lstListOfTests.SelectedItem = item;

                var lstItem = lvwListOfTest.Items[selectedIndices[i]];
                lvwListOfTest.Items.RemoveAt(selectedIndices[i]);
                lvwListOfTest.Items.Insert(selectedIndices[i] + ((int) moveType), lstItem);
            }
        }

        private void MoveListItems(string action)
        {
            var selectedIndices = GetSelectedIndices();

            switch (action)
            {
                case "up":
                    if (selectedIndices.Length == 0)
                    {
                        break;
                    }

                    if (selectedIndices[0] != 0)
                    {
                        MoveSelectedItems(selectedIndices, MoveType.Up);
                    }
                    break;
                case "down":
                    if (selectedIndices.Length == 0)
                    {
                        break;
                    }

                    if (selectedIndices[selectedIndices.Length - 1] != lstListOfTests.Items.Count - 1)
                    {
                        MoveSelectedItems(selectedIndices, MoveType.Down);
                    }
                    break;
            }
        }

        private int[] GetSelectedIndices()
        {
            return lstListOfTests.SelectedIndices.Cast<int>().ToArray();
        }

        private int[] GetSelectedIndicesBySender(object sender)
        {
            if (sender is Control)
            {
                return GetSelectedIndexFromListControl(sender);
            }

            if (!(sender is ToolStripItem)) return GetSelectedIndicesBySender(lstListOfTests);

            var item = sender as ToolStripItem;

            var strip = item.Owner as ContextMenuStrip;
            return GetSelectedIndicesBySender(strip != null ? strip.SourceControl : lstListOfTests);
        }

        private static int[] GetSelectedIndexFromListControl(object sender)
        {
            IList listOfIndexes;

            var box = sender as ListBox;
            if (box != null)
            {
                listOfIndexes = box.SelectedIndices;
            }
            else
            {
                listOfIndexes = new List<int>();
            }

            var view = sender as ListView;
            if (view != null)
            {
                listOfIndexes = view.SelectedIndices;
            }

            return listOfIndexes.Cast<int>().ToArray();
        }

        private void ShowStatus(string status)
        {
            tslStatus.Text = status;
        }

        private void ShowIdleStatus()
        {
            ShowStatus("Ready...");
        }

        private void LoadTestsToList()
        {
            if (_configurationTestSuite == null) return;

            foreach (Test test in _configurationTestSuite.Tests)
            {
                AddTestToList(test);
            }
        }

        private void SelectListBoxTestItem()
        {
            if (lvwListOfTest.SelectedIndices.Count > 0)
            {
                var selectedItem = lvwListOfTest.SelectedItems[0];
                var index = Convert.ToInt32(selectedItem.SubItems[selectedItem.SubItems.Count - 1].Text);
                lstListOfTests.SelectedItem = lstListOfTests.Items[index];
            }
        }

        private void SelectListViewTestItem()
        {
            if (lstListOfTests.SelectedIndices.Count > 0)
            {
                lvwListOfTest.Select();
                lvwListOfTest.SelectedItems.Clear();
                lvwListOfTest.HideSelection = false;
                foreach(ListViewItem item in lvwListOfTest.Items)
                {
                    var index = Convert.ToInt32(item.SubItems[item.SubItems.Count - 1].Text);
                    if (index == lstListOfTests.SelectedIndex)
                    {
                        item.Selected = true;
                        break;
                    }
                }
                if (lvwListOfTest.SelectedItems.Count > 0)
                    lvwListOfTest.SelectedItems[0].EnsureVisible();
            }
        }

        private void GenerateReport()
        {
            if (lstListOfTests.Items.Count == 0)
            {
                MessageBox.Show(@"There are no tests to generate a report for.", @"Can not generate report.",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            if (_reportBuilder.CountEnties == 0)
            {
                MessageBox.Show(@"You must run your test(s) before you can generate a report.", @"No test results to report.",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                return;
            }

            using (var reportWriter = new ReportWriter(_reportBuilder))
            {
                if (reportWriter.ShowDialog() == DialogResult.OK)
                {
                }
            }
        }

        private void TestEditor_Load(object sender, EventArgs e)
        {
            CreateTestMenus();
        }

        private void TestEditor_Shown(object sender, EventArgs e)
        {
            LoadTestsToList();
        }

        private void TestMenu_Click(object sender, EventArgs e)
        {
            var senderType = ((ToolStripItem)sender).Tag as Type;
            var test = Activator.CreateInstance(senderType) as Test;

            AddTestToList(test);
        }

        private void txtTestName_TextChanged(object sender, EventArgs e)
        {
            UpdateUi();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (lstListOfTests.Items.Count > 0)
            {
                if (MessageBox.Show(@"Are you sure you want to cancel?", @"Cancel", MessageBoxButtons.YesNo) ==
                    DialogResult.No)
                {
                    return;
                }
            }

            Close();
        }

        private void btnMoveItem_Click(object sender, EventArgs e)
        {
            var control = sender as Control;
            if (control == null) return;

            var action = control.Tag.ToString().ToLowerInvariant();
            MoveListItems(action);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveCurrentTestFile();
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            SaveNewTestFile();
        }

        private void lstListOfTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            pgTestConfiguration.SelectedObject = lstListOfTests.SelectedItem as Test;
            UpdateUi();
        }

        private void lvwListOfTest_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectListBoxTestItem();
        }

        private void pgTestConfiguration_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.Label == null) return;

            if (e.ChangedItem.Label == "TestName")
            {
                UpdateSeletedItem();
            }
        }

        private void mnuRemoveTest_Click(object sender, EventArgs e)
        {
            var selectedIndices = GetSelectedIndicesBySender(sender);

            RemoveTestsFromList(selectedIndices);
        }

        private void mnuRemoveAllTests_Click(object sender, EventArgs e)
        {
            RemoveAllTestsFromList();
        }

        private void mnuMove_Click(object sender, EventArgs e)
        {
            var toolStripMenuItem = sender as ToolStripMenuItem;

            if (toolStripMenuItem == null) return;

            var action = toolStripMenuItem.Tag.ToString().ToLowerInvariant();
            MoveListItems(action);
        }

        private void mnuRunSelectedTest_Click(object sender, EventArgs e)
        {
            var selectedIndices = GetSelectedIndicesBySender(sender);
            RunSelectedTests(selectedIndices);
        }

        private void mnuRunAllTests_Click(object sender, EventArgs e)
        {
            RunTest();
        }

        private void mnuSaveAndRun_Click(object sender, EventArgs e)
        {
            SaveCurrentTestFile();
            RunTest();
        }

        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            SaveNewTestFile();
        }

        private void tsbWriteTestReport2_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void tsbWriteTestReport_Click(object sender, EventArgs e)
        {
            GenerateReport();
        }

        private void SelectConfigurationTestItem_Handler(object sender, EventArgs e)
        {
            SelectListBoxTestItem();
            SwitchToConfigurationView();
        }

        private void SelectTestRunTestItem_Handler(object sender, EventArgs e)
        {
            SelectListViewTestItem();
            SwitchToTestRunView();
        }

        private void cntxtMain_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var x = sender as ContextMenuStrip;
            var listBox = x.SourceControl as ListBox;
            var listView = x.SourceControl as ListView;
            if (listBox != null)
            {
                mnuShowTest.Text = "Show Test Run";
                mnuShowTest.Click -= SelectConfigurationTestItem_Handler;
                mnuShowTest.Click += SelectTestRunTestItem_Handler;
            }

            if (listView != null)
            {
                mnuShowTest.Text = "Show Test";
                mnuShowTest.Click -= SelectTestRunTestItem_Handler;
                mnuShowTest.Click += SelectConfigurationTestItem_Handler;
            }

        }

        private void lvwListOfTest_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (e.Column != _listViewItemSorter.SortColumn)
            {
                _listViewItemSorter.SortColumn = e.Column;
                _listViewItemSorter.SortOrder = SortOrder.Ascending;
            }
            else
            {
                if (_listViewItemSorter.SortOrder == SortOrder.Ascending)
                    _listViewItemSorter.SortOrder = SortOrder.Descending;
                else
                    _listViewItemSorter.SortOrder = SortOrder.Ascending;
            }
            lvwListOfTest.Sort();
        }
    }
}