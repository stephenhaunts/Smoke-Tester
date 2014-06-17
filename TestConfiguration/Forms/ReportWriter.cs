using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestConfiguration.Forms
{
    public partial class ReportWriter : Form
    {
        public ReportWriter()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {

        }

        private void chooseFilename_Click(object sender, EventArgs e)
        {
            using (var folderBrowser = new FolderBrowserDialog())
            {
                if (folderBrowser.ShowDialog() == DialogResult.OK)
                {
                    fileName.Text = folderBrowser.SelectedPath;
                }
            }
        }
    }
}
