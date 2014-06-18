namespace TestConfiguration.Forms
{
    partial class ReportWriter
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
            this.reportTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.htmlReport = new System.Windows.Forms.RadioButton();
            this.xmlReport = new System.Windows.Forms.RadioButton();
            this.csvReport = new System.Windows.Forms.RadioButton();
            this.fileName = new System.Windows.Forms.TextBox();
            this.chooseFilename = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.reportTypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // reportTypeGroupBox
            // 
            this.reportTypeGroupBox.Controls.Add(this.htmlReport);
            this.reportTypeGroupBox.Controls.Add(this.xmlReport);
            this.reportTypeGroupBox.Controls.Add(this.csvReport);
            this.reportTypeGroupBox.Location = new System.Drawing.Point(12, 12);
            this.reportTypeGroupBox.Name = "reportTypeGroupBox";
            this.reportTypeGroupBox.Size = new System.Drawing.Size(364, 58);
            this.reportTypeGroupBox.TabIndex = 0;
            this.reportTypeGroupBox.TabStop = false;
            this.reportTypeGroupBox.Text = "Report Type";
            // 
            // htmlReport
            // 
            this.htmlReport.AutoSize = true;
            this.htmlReport.Location = new System.Drawing.Point(260, 19);
            this.htmlReport.Name = "htmlReport";
            this.htmlReport.Size = new System.Drawing.Size(90, 17);
            this.htmlReport.TabIndex = 2;
            this.htmlReport.TabStop = true;
            this.htmlReport.Text = "HTML Report";
            this.htmlReport.UseVisualStyleBackColor = true;
            // 
            // xmlReport
            // 
            this.xmlReport.AutoSize = true;
            this.xmlReport.Location = new System.Drawing.Point(135, 19);
            this.xmlReport.Name = "xmlReport";
            this.xmlReport.Size = new System.Drawing.Size(82, 17);
            this.xmlReport.TabIndex = 1;
            this.xmlReport.TabStop = true;
            this.xmlReport.Text = "XML Report";
            this.xmlReport.UseVisualStyleBackColor = true;
            // 
            // csvReport
            // 
            this.csvReport.AutoSize = true;
            this.csvReport.Location = new System.Drawing.Point(15, 19);
            this.csvReport.Name = "csvReport";
            this.csvReport.Size = new System.Drawing.Size(81, 17);
            this.csvReport.TabIndex = 0;
            this.csvReport.TabStop = true;
            this.csvReport.Text = "CSV Report";
            this.csvReport.UseVisualStyleBackColor = true;
            // 
            // fileName
            // 
            this.fileName.Location = new System.Drawing.Point(12, 76);
            this.fileName.Name = "fileName";
            this.fileName.Size = new System.Drawing.Size(320, 20);
            this.fileName.TabIndex = 1;
            this.fileName.TextChanged += new System.EventHandler(this.fileName_TextChanged);
            // 
            // chooseFilename
            // 
            this.chooseFilename.Location = new System.Drawing.Point(338, 76);
            this.chooseFilename.Name = "chooseFilename";
            this.chooseFilename.Size = new System.Drawing.Size(38, 20);
            this.chooseFilename.TabIndex = 2;
            this.chooseFilename.Text = "...";
            this.chooseFilename.UseVisualStyleBackColor = true;
            this.chooseFilename.Click += new System.EventHandler(this.chooseFilename_Click);
            // 
            // okButton
            // 
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(225, 111);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(306, 111);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.Description = "Choose a folder to save the test report.";
            // 
            // ReportWriter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, 148);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.chooseFilename);
            this.Controls.Add(this.fileName);
            this.Controls.Add(this.reportTypeGroupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportWriter";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Test Report Writer";
            this.reportTypeGroupBox.ResumeLayout(false);
            this.reportTypeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox reportTypeGroupBox;
        private System.Windows.Forms.RadioButton htmlReport;
        private System.Windows.Forms.RadioButton xmlReport;
        private System.Windows.Forms.RadioButton csvReport;
        private System.Windows.Forms.TextBox fileName;
        private System.Windows.Forms.Button chooseFilename;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    }
}