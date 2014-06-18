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
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using CommonCode.ReportWriter;

namespace TestConfiguration.Forms
{
    public partial class ReportWriter : Form
    {
        private ReportBuilder _builder;

        public ReportWriter(ReportBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException();
            }

            _builder = builder;

            InitializeComponent();
            okButton.Enabled = false;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var fileFriendlyDate = DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace("/", "").Replace(" ", "-").Replace(":", "");
            var fullFileName = Path.GetFullPath(fileName.Text + @"\" + fileFriendlyDate);

            if (csvReport.Checked)
            {
                fullFileName = Path.GetFullPath(fullFileName + ".csv");
                _builder.WriteReport(fullFileName, ReportType.CsvReport);
            }
            else if (xmlReport.Checked)
            {
                fullFileName = Path.GetFullPath(fullFileName + ".xml");
                _builder.WriteReport(fullFileName, ReportType.XmlReport);
            }
            else if (htmlReport.Checked)
            {
                fullFileName = Path.GetFullPath(fullFileName + ".html");
                _builder.WriteReport(fullFileName, ReportType.HtmlReport);
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
    }
}
