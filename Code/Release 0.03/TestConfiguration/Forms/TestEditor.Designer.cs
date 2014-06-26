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
namespace TestConfiguration.Forms
{
    partial class TestEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
         
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestEditor));
            this.pgTestConfiguration = new System.Windows.Forms.PropertyGrid();
            this.tspConfiguration = new System.Windows.Forms.ToolStrip();
            this.tsbTests = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsbSave = new System.Windows.Forms.ToolStripSplitButton();
            this.mnuSave = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuSaveAndRun = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRunSelectedTest01 = new System.Windows.Forms.ToolStripButton();
            this.tsbRunAllTests01 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRemoveTest01 = new System.Windows.Forms.ToolStripButton();
            this.tsbRemoveAllTests01 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbWriteTestReport = new System.Windows.Forms.ToolStripButton();
            this.lstListOfTests = new System.Windows.Forms.ListBox();
            this.cntxtMain = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuShowTest = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRemoveTest = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRemoveAllTests = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuMoveUp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMoveDown = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRunSelectedTest = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRunAllTests = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.btnMoveDown = new System.Windows.Forms.Button();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblTotalTestCount = new System.Windows.Forms.Label();
            this.lblListOfTest = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSaveAs = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblTestName = new System.Windows.Forms.Label();
            this.txtTestName = new System.Windows.Forms.TextBox();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tpgConfiguation = new System.Windows.Forms.TabPage();
            this.tpgTestRun = new System.Windows.Forms.TabPage();
            this.lvwListOfTest = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imgLstIcons = new System.Windows.Forms.ImageList(this.components);
            this.tspTestRun = new System.Windows.Forms.ToolStrip();
            this.tsbRunSelectedTest02 = new System.Windows.Forms.ToolStripButton();
            this.tsbRunAllTests02 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbWriteTestReport2 = new System.Windows.Forms.ToolStripButton();
            this.stsStatus = new System.Windows.Forms.StatusStrip();
            this.tslStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspConfiguration.SuspendLayout();
            this.cntxtMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tpgConfiguation.SuspendLayout();
            this.tpgTestRun.SuspendLayout();
            this.tspTestRun.SuspendLayout();
            this.stsStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // pgTestConfiguration
            // 
            this.pgTestConfiguration.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgTestConfiguration.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pgTestConfiguration.Location = new System.Drawing.Point(0, 0);
            this.pgTestConfiguration.Name = "pgTestConfiguration";
            this.pgTestConfiguration.Size = new System.Drawing.Size(493, 439);
            this.pgTestConfiguration.TabIndex = 0;
            this.pgTestConfiguration.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgTestConfiguration_PropertyValueChanged);
            // 
            // tspConfiguration
            // 
            this.tspConfiguration.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tspConfiguration.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbTests,
            this.tsbSave,
            this.toolStripSeparator2,
            this.tsbRunSelectedTest01,
            this.tsbRunAllTests01,
            this.toolStripSeparator3,
            this.tsbRemoveTest01,
            this.tsbRemoveAllTests01,
            this.toolStripSeparator5,
            this.tsbWriteTestReport});
            this.tspConfiguration.Location = new System.Drawing.Point(3, 3);
            this.tspConfiguration.Name = "tspConfiguration";
            this.tspConfiguration.Size = new System.Drawing.Size(858, 25);
            this.tspConfiguration.TabIndex = 1;
            this.tspConfiguration.Text = "toolStrip1";
            // 
            // tsbTests
            // 
            this.tsbTests.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbTests.Image = global::TestConfiguration.Properties.Resources._04;
            this.tsbTests.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbTests.Name = "tsbTests";
            this.tsbTests.Size = new System.Drawing.Size(29, 22);
            this.tsbTests.Text = "Create New Test";
            // 
            // tsbSave
            // 
            this.tsbSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuSave,
            this.mnuSaveAs,
            this.toolStripSeparator1,
            this.mnuSaveAndRun});
            this.tsbSave.Image = global::TestConfiguration.Properties.Resources._06;
            this.tsbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSave.Name = "tsbSave";
            this.tsbSave.Size = new System.Drawing.Size(32, 22);
            this.tsbSave.Text = "Save Test";
            this.tsbSave.ButtonClick += new System.EventHandler(this.btnSave_Click);
            // 
            // mnuSave
            // 
            this.mnuSave.Name = "mnuSave";
            this.mnuSave.Size = new System.Drawing.Size(154, 22);
            this.mnuSave.Text = "Save...";
            this.mnuSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // mnuSaveAs
            // 
            this.mnuSaveAs.Name = "mnuSaveAs";
            this.mnuSaveAs.Size = new System.Drawing.Size(154, 22);
            this.mnuSaveAs.Text = "Save As...";
            this.mnuSaveAs.Click += new System.EventHandler(this.mnuSaveAs_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // mnuSaveAndRun
            // 
            this.mnuSaveAndRun.Name = "mnuSaveAndRun";
            this.mnuSaveAndRun.Size = new System.Drawing.Size(154, 22);
            this.mnuSaveAndRun.Text = "Save and Run...";
            this.mnuSaveAndRun.Click += new System.EventHandler(this.mnuSaveAndRun_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbRunSelectedTest01
            // 
            this.tsbRunSelectedTest01.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunSelectedTest01.Image = global::TestConfiguration.Properties.Resources._07;
            this.tsbRunSelectedTest01.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunSelectedTest01.Name = "tsbRunSelectedTest01";
            this.tsbRunSelectedTest01.Size = new System.Drawing.Size(23, 22);
            this.tsbRunSelectedTest01.Text = "Run Selected Test";
            this.tsbRunSelectedTest01.Click += new System.EventHandler(this.mnuRunSelectedTest_Click);
            // 
            // tsbRunAllTests01
            // 
            this.tsbRunAllTests01.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunAllTests01.Image = global::TestConfiguration.Properties.Resources._08;
            this.tsbRunAllTests01.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunAllTests01.Name = "tsbRunAllTests01";
            this.tsbRunAllTests01.Size = new System.Drawing.Size(23, 22);
            this.tsbRunAllTests01.Text = "Run All Tests";
            this.tsbRunAllTests01.Click += new System.EventHandler(this.mnuRunAllTests_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbRemoveTest01
            // 
            this.tsbRemoveTest01.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemoveTest01.Image = global::TestConfiguration.Properties.Resources._14;
            this.tsbRemoveTest01.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveTest01.Name = "tsbRemoveTest01";
            this.tsbRemoveTest01.Size = new System.Drawing.Size(23, 22);
            this.tsbRemoveTest01.Text = "Remove Selected Test";
            this.tsbRemoveTest01.Click += new System.EventHandler(this.mnuRemoveTest_Click);
            // 
            // tsbRemoveAllTests01
            // 
            this.tsbRemoveAllTests01.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRemoveAllTests01.Image = global::TestConfiguration.Properties.Resources._13;
            this.tsbRemoveAllTests01.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRemoveAllTests01.Name = "tsbRemoveAllTests01";
            this.tsbRemoveAllTests01.Size = new System.Drawing.Size(23, 22);
            this.tsbRemoveAllTests01.Text = "Remove All Tests";
            this.tsbRemoveAllTests01.Click += new System.EventHandler(this.mnuRemoveAllTests_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbWriteTestReport
            // 
            this.tsbWriteTestReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbWriteTestReport.Image = ((System.Drawing.Image)(resources.GetObject("tsbWriteTestReport.Image")));
            this.tsbWriteTestReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWriteTestReport.Name = "tsbWriteTestReport";
            this.tsbWriteTestReport.Size = new System.Drawing.Size(23, 22);
            this.tsbWriteTestReport.Text = "Write Out Test Report";
            this.tsbWriteTestReport.Click += new System.EventHandler(this.tsbWriteTestReport_Click);
            // 
            // lstListOfTests
            // 
            this.lstListOfTests.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstListOfTests.ContextMenuStrip = this.cntxtMain;
            this.lstListOfTests.FormattingEnabled = true;
            this.lstListOfTests.ItemHeight = 17;
            this.lstListOfTests.Location = new System.Drawing.Point(0, 79);
            this.lstListOfTests.Name = "lstListOfTests";
            this.lstListOfTests.Size = new System.Drawing.Size(313, 293);
            this.lstListOfTests.TabIndex = 2;
            this.lstListOfTests.SelectedIndexChanged += new System.EventHandler(this.lstListOfTests_SelectedIndexChanged);
            this.lstListOfTests.DoubleClick += new System.EventHandler(this.SelectTestRunTestItem_Handler);
            // 
            // cntxtMain
            // 
            this.cntxtMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuShowTest,
            this.toolStripSeparator7,
            this.mnuRemoveTest,
            this.mnuRemoveAllTests,
            this.toolStripMenuItem1,
            this.mnuMoveUp,
            this.mnuMoveDown,
            this.toolStripMenuItem2,
            this.mnuRunSelectedTest,
            this.mnuRunAllTests});
            this.cntxtMain.Name = "cntxtMain";
            this.cntxtMain.Size = new System.Drawing.Size(165, 176);
            this.cntxtMain.Opening += new System.ComponentModel.CancelEventHandler(this.cntxtMain_Opening);
            // 
            // mnuShowTest
            // 
            this.mnuShowTest.Name = "mnuShowTest";
            this.mnuShowTest.Size = new System.Drawing.Size(164, 22);
            this.mnuShowTest.Text = "Show Test";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(161, 6);
            // 
            // mnuRemoveTest
            // 
            this.mnuRemoveTest.Image = ((System.Drawing.Image)(resources.GetObject("mnuRemoveTest.Image")));
            this.mnuRemoveTest.Name = "mnuRemoveTest";
            this.mnuRemoveTest.Size = new System.Drawing.Size(164, 22);
            this.mnuRemoveTest.Text = "Remove Test";
            this.mnuRemoveTest.Click += new System.EventHandler(this.mnuRemoveTest_Click);
            // 
            // mnuRemoveAllTests
            // 
            this.mnuRemoveAllTests.Image = global::TestConfiguration.Properties.Resources._13;
            this.mnuRemoveAllTests.Name = "mnuRemoveAllTests";
            this.mnuRemoveAllTests.Size = new System.Drawing.Size(164, 22);
            this.mnuRemoveAllTests.Text = "Remove All Tests";
            this.mnuRemoveAllTests.Click += new System.EventHandler(this.mnuRemoveAllTests_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            // 
            // mnuMoveUp
            // 
            this.mnuMoveUp.Image = global::TestConfiguration.Properties.Resources._16;
            this.mnuMoveUp.Name = "mnuMoveUp";
            this.mnuMoveUp.Size = new System.Drawing.Size(164, 22);
            this.mnuMoveUp.Tag = "up";
            this.mnuMoveUp.Text = "Move Up";
            this.mnuMoveUp.Click += new System.EventHandler(this.mnuMove_Click);
            // 
            // mnuMoveDown
            // 
            this.mnuMoveDown.Image = global::TestConfiguration.Properties.Resources._15;
            this.mnuMoveDown.Name = "mnuMoveDown";
            this.mnuMoveDown.Size = new System.Drawing.Size(164, 22);
            this.mnuMoveDown.Tag = "down";
            this.mnuMoveDown.Text = "Move Down";
            this.mnuMoveDown.Click += new System.EventHandler(this.mnuMove_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(161, 6);
            // 
            // mnuRunSelectedTest
            // 
            this.mnuRunSelectedTest.Image = global::TestConfiguration.Properties.Resources._07;
            this.mnuRunSelectedTest.Name = "mnuRunSelectedTest";
            this.mnuRunSelectedTest.Size = new System.Drawing.Size(164, 22);
            this.mnuRunSelectedTest.Text = "Run";
            this.mnuRunSelectedTest.Click += new System.EventHandler(this.mnuRunSelectedTest_Click);
            // 
            // mnuRunAllTests
            // 
            this.mnuRunAllTests.Image = global::TestConfiguration.Properties.Resources._08;
            this.mnuRunAllTests.Name = "mnuRunAllTests";
            this.mnuRunAllTests.Size = new System.Drawing.Size(164, 22);
            this.mnuRunAllTests.Text = "Run All";
            this.mnuRunAllTests.Click += new System.EventHandler(this.mnuRunAllTests_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 28);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnMoveDown);
            this.splitContainer1.Panel1.Controls.Add(this.btnMoveUp);
            this.splitContainer1.Panel1.Controls.Add(this.lstListOfTests);
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pgTestConfiguration);
            this.splitContainer1.Size = new System.Drawing.Size(858, 439);
            this.splitContainer1.SplitterDistance = 361;
            this.splitContainer1.TabIndex = 3;
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveDown.BackgroundImage = global::TestConfiguration.Properties.Resources._15;
            this.btnMoveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMoveDown.Font = new System.Drawing.Font("Wingdings 3", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnMoveDown.Location = new System.Drawing.Point(319, 167);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(35, 32);
            this.btnMoveDown.TabIndex = 7;
            this.btnMoveDown.Tag = "down";
            this.btnMoveDown.UseVisualStyleBackColor = true;
            this.btnMoveDown.Click += new System.EventHandler(this.btnMoveItem_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoveUp.BackgroundImage = global::TestConfiguration.Properties.Resources._16;
            this.btnMoveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnMoveUp.Font = new System.Drawing.Font("Wingdings 3", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.btnMoveUp.Location = new System.Drawing.Point(319, 129);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(35, 32);
            this.btnMoveUp.TabIndex = 6;
            this.btnMoveUp.Tag = "up";
            this.btnMoveUp.UseVisualStyleBackColor = true;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveItem_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lblTotalTestCount);
            this.panel3.Controls.Add(this.lblListOfTest);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 46);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(361, 30);
            this.panel3.TabIndex = 5;
            // 
            // lblTotalTestCount
            // 
            this.lblTotalTestCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTotalTestCount.AutoSize = true;
            this.lblTotalTestCount.Location = new System.Drawing.Point(264, 6);
            this.lblTotalTestCount.Name = "lblTotalTestCount";
            this.lblTotalTestCount.Size = new System.Drawing.Size(49, 17);
            this.lblTotalTestCount.TabIndex = 2;
            this.lblTotalTestCount.Text = "0 Tests";
            this.lblTotalTestCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblListOfTest
            // 
            this.lblListOfTest.AutoSize = true;
            this.lblListOfTest.Location = new System.Drawing.Point(0, 6);
            this.lblListOfTest.Name = "lblListOfTest";
            this.lblListOfTest.Size = new System.Drawing.Size(77, 17);
            this.lblListOfTest.TabIndex = 1;
            this.lblListOfTest.Text = "List of Tests";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnSaveAs);
            this.panel2.Controls.Add(this.btnCancel);
            this.panel2.Controls.Add(this.btnSave);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 378);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(361, 61);
            this.panel2.TabIndex = 4;
            // 
            // btnSaveAs
            // 
            this.btnSaveAs.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveAs.Location = new System.Drawing.Point(103, 11);
            this.btnSaveAs.Name = "btnSaveAs";
            this.btnSaveAs.Size = new System.Drawing.Size(102, 38);
            this.btnSaveAs.TabIndex = 2;
            this.btnSaveAs.Text = "Save As";
            this.btnSaveAs.UseVisualStyleBackColor = true;
            this.btnSaveAs.Click += new System.EventHandler(this.btnSaveAs_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(211, 11);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(102, 38);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(0, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(97, 38);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTestName);
            this.panel1.Controls.Add(this.txtTestName);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(361, 46);
            this.panel1.TabIndex = 3;
            // 
            // lblTestName
            // 
            this.lblTestName.AutoSize = true;
            this.lblTestName.Location = new System.Drawing.Point(0, 15);
            this.lblTestName.Name = "lblTestName";
            this.lblTestName.Size = new System.Drawing.Size(71, 17);
            this.lblTestName.TabIndex = 0;
            this.lblTestName.Text = "Test Name";
            // 
            // txtTestName
            // 
            this.txtTestName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTestName.Location = new System.Drawing.Point(77, 12);
            this.txtTestName.Name = "txtTestName";
            this.txtTestName.Size = new System.Drawing.Size(236, 25);
            this.txtTestName.TabIndex = 0;
            this.txtTestName.TextChanged += new System.EventHandler(this.txtTestName_TextChanged);
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tpgConfiguation);
            this.tabMain.Controls.Add(this.tpgTestRun);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.Location = new System.Drawing.Point(0, 0);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(872, 500);
            this.tabMain.TabIndex = 4;
            // 
            // tpgConfiguation
            // 
            this.tpgConfiguation.Controls.Add(this.splitContainer1);
            this.tpgConfiguation.Controls.Add(this.tspConfiguration);
            this.tpgConfiguation.Location = new System.Drawing.Point(4, 26);
            this.tpgConfiguation.Name = "tpgConfiguation";
            this.tpgConfiguation.Padding = new System.Windows.Forms.Padding(3);
            this.tpgConfiguation.Size = new System.Drawing.Size(864, 470);
            this.tpgConfiguation.TabIndex = 0;
            this.tpgConfiguation.Text = "Configuration";
            this.tpgConfiguation.UseVisualStyleBackColor = true;
            // 
            // tpgTestRun
            // 
            this.tpgTestRun.Controls.Add(this.lvwListOfTest);
            this.tpgTestRun.Controls.Add(this.tspTestRun);
            this.tpgTestRun.Location = new System.Drawing.Point(4, 26);
            this.tpgTestRun.Name = "tpgTestRun";
            this.tpgTestRun.Padding = new System.Windows.Forms.Padding(3);
            this.tpgTestRun.Size = new System.Drawing.Size(864, 470);
            this.tpgTestRun.TabIndex = 1;
            this.tpgTestRun.Text = "Test Run";
            this.tpgTestRun.UseVisualStyleBackColor = true;
            // 
            // lvwListOfTest
            // 
            this.lvwListOfTest.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.lvwListOfTest.ContextMenuStrip = this.cntxtMain;
            this.lvwListOfTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwListOfTest.FullRowSelect = true;
            this.lvwListOfTest.LargeImageList = this.imgLstIcons;
            this.lvwListOfTest.Location = new System.Drawing.Point(3, 28);
            this.lvwListOfTest.Name = "lvwListOfTest";
            this.lvwListOfTest.Size = new System.Drawing.Size(858, 439);
            this.lvwListOfTest.SmallImageList = this.imgLstIcons;
            this.lvwListOfTest.StateImageList = this.imgLstIcons;
            this.lvwListOfTest.TabIndex = 3;
            this.lvwListOfTest.UseCompatibleStateImageBehavior = false;
            this.lvwListOfTest.View = System.Windows.Forms.View.Details;
            this.lvwListOfTest.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvwListOfTest_ColumnClick);
            this.lvwListOfTest.SelectedIndexChanged += new System.EventHandler(this.lvwListOfTest_SelectedIndexChanged);
            this.lvwListOfTest.DoubleClick += new System.EventHandler(this.SelectConfigurationTestItem_Handler);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Result";
            this.columnHeader1.Width = 74;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "TestName";
            this.columnHeader2.Width = 203;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Error Message";
            this.columnHeader3.Width = 385;
            // 
            // imgLstIcons
            // 
            this.imgLstIcons.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgLstIcons.ImageStream")));
            this.imgLstIcons.TransparentColor = System.Drawing.Color.Transparent;
            this.imgLstIcons.Images.SetKeyName(0, "01.png");
            this.imgLstIcons.Images.SetKeyName(1, "02.png");
            this.imgLstIcons.Images.SetKeyName(2, "03.png");
            this.imgLstIcons.Images.SetKeyName(3, "04.png");
            this.imgLstIcons.Images.SetKeyName(4, "05.png");
            this.imgLstIcons.Images.SetKeyName(5, "06.png");
            // 
            // tspTestRun
            // 
            this.tspTestRun.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tspTestRun.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbRunSelectedTest02,
            this.tsbRunAllTests02,
            this.toolStripSeparator4,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator6,
            this.tsbWriteTestReport2});
            this.tspTestRun.Location = new System.Drawing.Point(3, 3);
            this.tspTestRun.Name = "tspTestRun";
            this.tspTestRun.Size = new System.Drawing.Size(858, 25);
            this.tspTestRun.TabIndex = 4;
            this.tspTestRun.Text = "toolStrip2";
            // 
            // tsbRunSelectedTest02
            // 
            this.tsbRunSelectedTest02.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunSelectedTest02.Image = global::TestConfiguration.Properties.Resources._07;
            this.tsbRunSelectedTest02.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunSelectedTest02.Name = "tsbRunSelectedTest02";
            this.tsbRunSelectedTest02.Size = new System.Drawing.Size(23, 22);
            this.tsbRunSelectedTest02.Text = "Run Selected Test";
            this.tsbRunSelectedTest02.Click += new System.EventHandler(this.mnuRunSelectedTest_Click);
            // 
            // tsbRunAllTests02
            // 
            this.tsbRunAllTests02.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRunAllTests02.Image = global::TestConfiguration.Properties.Resources._08;
            this.tsbRunAllTests02.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRunAllTests02.Name = "tsbRunAllTests02";
            this.tsbRunAllTests02.Size = new System.Drawing.Size(23, 22);
            this.tsbRunAllTests02.Text = "Run All Tests";
            this.tsbRunAllTests02.Click += new System.EventHandler(this.mnuRunAllTests_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::TestConfiguration.Properties.Resources._14;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Remove Selected Test";
            this.toolStripButton1.Click += new System.EventHandler(this.mnuRemoveTest_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::TestConfiguration.Properties.Resources._13;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "Remove All Tests";
            this.toolStripButton2.Click += new System.EventHandler(this.mnuRemoveAllTests_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbWriteTestReport2
            // 
            this.tsbWriteTestReport2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbWriteTestReport2.Image = ((System.Drawing.Image)(resources.GetObject("tsbWriteTestReport2.Image")));
            this.tsbWriteTestReport2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWriteTestReport2.Name = "tsbWriteTestReport2";
            this.tsbWriteTestReport2.Size = new System.Drawing.Size(23, 22);
            this.tsbWriteTestReport2.Text = "Write Out Test Report";
            this.tsbWriteTestReport2.Click += new System.EventHandler(this.tsbWriteTestReport2_Click);
            // 
            // stsStatus
            // 
            this.stsStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslStatus});
            this.stsStatus.Location = new System.Drawing.Point(0, 500);
            this.stsStatus.Name = "stsStatus";
            this.stsStatus.Size = new System.Drawing.Size(872, 22);
            this.stsStatus.TabIndex = 5;
            this.stsStatus.Text = "statusStrip1";
            // 
            // tslStatus
            // 
            this.tslStatus.Name = "tslStatus";
            this.tslStatus.Size = new System.Drawing.Size(48, 17);
            this.tslStatus.Text = "Ready...";
            // 
            // TestEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(872, 522);
            this.Controls.Add(this.tabMain);
            this.Controls.Add(this.stsStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Smoke Tester : Test Configurations Editor ";
            this.Load += new System.EventHandler(this.TestEditor_Load);
            this.Shown += new System.EventHandler(this.TestEditor_Shown);
            this.tspConfiguration.ResumeLayout(false);
            this.tspConfiguration.PerformLayout();
            this.cntxtMain.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabMain.ResumeLayout(false);
            this.tpgConfiguation.ResumeLayout(false);
            this.tpgConfiguation.PerformLayout();
            this.tpgTestRun.ResumeLayout(false);
            this.tpgTestRun.PerformLayout();
            this.tspTestRun.ResumeLayout(false);
            this.tspTestRun.PerformLayout();
            this.stsStatus.ResumeLayout(false);
            this.stsStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PropertyGrid pgTestConfiguration;
        private System.Windows.Forms.ToolStrip tspConfiguration;
        private System.Windows.Forms.ToolStripButton tsbRunSelectedTest01;
        private System.Windows.Forms.ListBox lstListOfTests;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblListOfTest;
        private System.Windows.Forms.Label lblTestName;
        private System.Windows.Forms.TextBox txtTestName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolStripSplitButton tsbSave;
        private System.Windows.Forms.ToolStripMenuItem mnuSave;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveAndRun;
        private System.Windows.Forms.ToolStripMenuItem mnuSaveAs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label lblTotalTestCount;
        private System.Windows.Forms.Button btnMoveDown;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tpgConfiguation;
        private System.Windows.Forms.TabPage tpgTestRun;
        private System.Windows.Forms.StatusStrip stsStatus;
        private System.Windows.Forms.ListView lvwListOfTest;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStrip tspTestRun;
        private System.Windows.Forms.ToolStripStatusLabel tslStatus;
        private System.Windows.Forms.Button btnSaveAs;
        private System.Windows.Forms.ImageList imgLstIcons;
        private System.Windows.Forms.ToolStripDropDownButton tsbTests;
        private System.Windows.Forms.ContextMenuStrip cntxtMain;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveTest;
        private System.Windows.Forms.ToolStripMenuItem mnuRemoveAllTests;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveUp;
        private System.Windows.Forms.ToolStripMenuItem mnuMoveDown;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuRunSelectedTest;
        private System.Windows.Forms.ToolStripButton tsbRunAllTests01;
        private System.Windows.Forms.ToolStripButton tsbRunSelectedTest02;
        private System.Windows.Forms.ToolStripButton tsbRunAllTests02;
        private System.Windows.Forms.ToolStripMenuItem mnuRunAllTests;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbRemoveTest01;
        private System.Windows.Forms.ToolStripButton tsbRemoveAllTests01;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbWriteTestReport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbWriteTestReport2;
        private System.Windows.Forms.ToolStripMenuItem mnuShowTest;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
    }
}