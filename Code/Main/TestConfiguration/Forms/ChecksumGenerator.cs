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
using System.Security.Cryptography;
using System.Windows.Forms;

namespace TestConfiguration.Forms
{
    public partial class ChecksumGenerator : Form
    {
        public ChecksumGenerator()
        {
            InitializeComponent();
        }

        private void fileSelectButton_Click(object sender, EventArgs e)
        {
            openFileDialog.ShowDialog();
            fileName.Text = openFileDialog.FileName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var checksum = string.Empty;

            if (md5RadioButton.Checked)
            {
                checksum = CalculateMD5();  
            }
            else if (sha1RadioButton.Checked)
            {
                checksum = CalculateSHA1();
            }
            else if (sha256RadioButton.Checked)
            {
                checksum = CalculateSHA256();  
            }

            using (var viewChecksum = new ViewChecksum(checksum))
            {
                viewChecksum.ShowDialog();
            }
        }

        private string CalculateSHA256()
        {
            string checksum;

            using (var stream = File.OpenRead(openFileDialog.FileName))
            {
                var sha = new SHA256Managed();
                var computedChecksum = sha.ComputeHash(stream);
                checksum = BitConverter.ToString(computedChecksum).Replace("-", string.Empty).ToLower();
            }

            return checksum;
        }

        private string CalculateSHA1()
        {
            string checksum;

            using (var stream = File.OpenRead(openFileDialog.FileName))
            {
                var sha = new SHA1Managed();
                var computedChecksum = sha.ComputeHash(stream);
                checksum = BitConverter.ToString(computedChecksum).Replace("-", string.Empty).ToLower();
            }

            return checksum;
        }

        private string CalculateMD5()
        {
            string checksum;
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(openFileDialog.FileName))
                {
                    checksum = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", String.Empty).ToLower();
                }
            }

            return checksum;
        }
    }
}
