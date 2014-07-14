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

using CommonCode.Reports;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.IO;

namespace TestConfiguration.Forms
{
    public partial class ReportWriter : Form
    {
        IEnumerable<ReportEntry> _reportEntries;
        public ReportWriter(IEnumerable<ReportEntry> reportEntries)
        {
            _reportEntries = reportEntries ?? new List<ReportEntry>();
            InitializeComponent();
            okButton.Enabled = false;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                var reportWriter = (IReportWriter)cmbReportWriters.SelectedItem;
                var path = Path.Combine(fileName.Text, ReportHelper.GetReportFileName(reportWriter.Extension));
                reportWriter.WriteReport(path, _reportEntries.ToList());
            }
            catch
            {
                MessageBox.Show(@"There was an error writing the report file.", @"Error writing report.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chooseFilename_Click(object sender, EventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    fileName.Text = folderBrowser.SelectedPath;
                    okButton.Enabled = true;
                }
            }
        }

        private void fileName_TextChanged(object sender, EventArgs e)
        {
            okButton.Enabled = !string.IsNullOrEmpty(fileName.Text);
        }

        private void ReportWriter_Load(object sender, EventArgs e)
        {
            var writerTypes = ReportHelper.GetReportWriters();
            foreach (var writer in writerTypes)
            {
                cmbReportWriters.Items.Add(writer);
            }
            cmbReportWriters.SelectedIndex = 0;
        }
    }
}
