namespace TestConfiguration.Forms
{
    partial class ChecksumGenerator
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
            this.fileName = new System.Windows.Forms.TextBox();
            this.fileSelectButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.md5RadioButton = new System.Windows.Forms.RadioButton();
            this.sha1RadioButton = new System.Windows.Forms.RadioButton();
            this.sha256RadioButton = new System.Windows.Forms.RadioButton();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.generateButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // fileName
            // 
            this.fileName.Location = new System.Drawing.Point(32, 39);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(342, 20);
            this.fileName.TabIndex = 0;
            // 
            // fileSelectButton
            // 
            this.fileSelectButton.Location = new System.Drawing.Point(380, 39);
            this.fileSelectButton.Name = "fileSelectButton";
            this.fileSelectButton.Size = new System.Drawing.Size(43, 20);
            this.fileSelectButton.TabIndex = 1;
            this.fileSelectButton.Text = "...";
            this.fileSelectButton.UseVisualStyleBackColor = true;
            this.fileSelectButton.Click += new System.EventHandler(this.fileSelectButton_Click);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(364, 96);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "Close";
            this.okButton.UseVisualStyleBackColor = true;
            // 
            // md5RadioButton
            // 
            this.md5RadioButton.AutoSize = true;
            this.md5RadioButton.Checked = true;
            this.md5RadioButton.Location = new System.Drawing.Point(32, 65);
            this.md5RadioButton.Name = "md5RadioButton";
            this.md5RadioButton.Size = new System.Drawing.Size(48, 17);
            this.md5RadioButton.TabIndex = 3;
            this.md5RadioButton.TabStop = true;
            this.md5RadioButton.Text = "MD5";
            this.md5RadioButton.UseVisualStyleBackColor = true;
            // 
            // sha1RadioButton
            // 
            this.sha1RadioButton.AutoSize = true;
            this.sha1RadioButton.Location = new System.Drawing.Point(86, 65);
            this.sha1RadioButton.Name = "sha1RadioButton";
            this.sha1RadioButton.Size = new System.Drawing.Size(53, 17);
            this.sha1RadioButton.TabIndex = 4;
            this.sha1RadioButton.Text = "SHA1";
            this.sha1RadioButton.UseVisualStyleBackColor = true;
            // 
            // sha256RadioButton
            // 
            this.sha256RadioButton.AutoSize = true;
            this.sha256RadioButton.Location = new System.Drawing.Point(145, 65);
            this.sha256RadioButton.Name = "sha256RadioButton";
            this.sha256RadioButton.Size = new System.Drawing.Size(65, 17);
            this.sha256RadioButton.TabIndex = 5;
            this.sha256RadioButton.Text = "SHA256";
            this.sha256RadioButton.UseVisualStyleBackColor = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.InitialDirectory = ".";
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(283, 96);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(75, 23);
            this.generateButton.TabIndex = 6;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // ChecksumGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 131);
            this.ControlBox = false;
            this.Controls.Add(this.generateButton);
            this.Controls.Add(this.sha256RadioButton);
            this.Controls.Add(this.sha1RadioButton);
            this.Controls.Add(this.md5RadioButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.fileSelectButton);
            this.Controls.Add(this.fileName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChecksumGenerator";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Checksum Generator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fileName;
        private System.Windows.Forms.Button fileSelectButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.RadioButton md5RadioButton;
        private System.Windows.Forms.RadioButton sha1RadioButton;
        private System.Windows.Forms.RadioButton sha256RadioButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Button generateButton;
    }
}