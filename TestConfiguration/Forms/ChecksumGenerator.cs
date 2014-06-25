using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using ConfigurationTests.Tests;

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
            string checksum;

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
