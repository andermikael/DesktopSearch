namespace DesktopSearch1
{
    partial class IndexForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonClean = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonBrowse = new System.Windows.Forms.Button();
            this.textBoxPath = new System.Windows.Forms.TextBox();
            this.buttonIndex = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.AddToIndexButton = new System.Windows.Forms.Button();
            this.RootDirectoriesCombobox = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.RootDirectoriesCombobox);
            this.groupBox1.Controls.Add(this.AddToIndexButton);
            this.groupBox1.Controls.Add(this.buttonClean);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.buttonBrowse);
            this.groupBox1.Controls.Add(this.textBoxPath);
            this.groupBox1.Controls.Add(this.buttonIndex);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(758, 141);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Index";
            // 
            // buttonClean
            // 
            this.buttonClean.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonClean.Location = new System.Drawing.Point(307, 103);
            this.buttonClean.Name = "buttonClean";
            this.buttonClean.Size = new System.Drawing.Size(75, 23);
            this.buttonClean.TabIndex = 5;
            this.buttonClean.Text = "Clean up";
            this.buttonClean.Click += new System.EventHandler(this.buttonClean_Click);
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.label1.Location = new System.Drawing.Point(16, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(344, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Index and search the following folder:";
            // 
            // buttonBrowse
            // 
            this.buttonBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonBrowse.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonBrowse.Location = new System.Drawing.Point(673, 68);
            this.buttonBrowse.Name = "buttonBrowse";
            this.buttonBrowse.Size = new System.Drawing.Size(75, 23);
            this.buttonBrowse.TabIndex = 2;
            this.buttonBrowse.Text = "Browse...";
            this.buttonBrowse.Click += new System.EventHandler(this.buttonBrowse_Click);
            // 
            // textBoxPath
            // 
            this.textBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPath.Location = new System.Drawing.Point(19, 68);
            this.textBoxPath.Name = "textBoxPath";
            this.textBoxPath.Size = new System.Drawing.Size(638, 20);
            this.textBoxPath.TabIndex = 1;
            // 
            // buttonIndex
            // 
            this.buttonIndex.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonIndex.Location = new System.Drawing.Point(187, 103);
            this.buttonIndex.Name = "buttonIndex";
            this.buttonIndex.Size = new System.Drawing.Size(112, 23);
            this.buttonIndex.TabIndex = 3;
            this.buttonIndex.Text = "Rebuild the index";
            this.buttonIndex.Click += new System.EventHandler(this.buttonIndex_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.Location = new System.Drawing.Point(12, 171);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(758, 67);
            this.labelStatus.TabIndex = 9;
            // 
            // AddToIndexButton
            // 
            this.AddToIndexButton.Location = new System.Drawing.Point(22, 103);
            this.AddToIndexButton.Name = "AddToIndexButton";
            this.AddToIndexButton.Size = new System.Drawing.Size(75, 23);
            this.AddToIndexButton.TabIndex = 6;
            this.AddToIndexButton.Text = "Add";
            this.AddToIndexButton.UseVisualStyleBackColor = true;
            this.AddToIndexButton.Click += new System.EventHandler(this.AddToIndexButton_Click);
            // 
            // RootDirectoriesCombobox
            // 
            this.RootDirectoriesCombobox.FormattingEnabled = true;
            this.RootDirectoriesCombobox.Location = new System.Drawing.Point(22, 41);
            this.RootDirectoriesCombobox.Name = "RootDirectoriesCombobox";
            this.RootDirectoriesCombobox.Size = new System.Drawing.Size(635, 21);
            this.RootDirectoriesCombobox.TabIndex = 7;
            // 
            // IndexForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 247);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.labelStatus);
            this.Name = "IndexForm";
            this.Text = "IndexForm";
            this.Load += new System.EventHandler(this.IndexForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonClean;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonBrowse;
        private System.Windows.Forms.TextBox textBoxPath;
        private System.Windows.Forms.Button buttonIndex;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button AddToIndexButton;
        private System.Windows.Forms.ComboBox RootDirectoriesCombobox;
    }
}