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
using System.Text;
using System.Windows.Forms;

namespace TestConfiguration.Forms
{
    public partial class FileListGenerator : Form
    {
        public FileListGenerator()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            var fileNames = new StringBuilder();

            foreach (var filename in openFileDialog.FileNames)
            {
                fileNames.Append(Path.GetFileName(filename) + ";");
            }

            numberFilesSelectedLabel.Text = @"Number of Files Selected : " + openFileDialog.FileNames.Length + @"<Copied to the clipboard.>";
            Clipboard.SetText(fileNames.ToString());
        }
    }
}
