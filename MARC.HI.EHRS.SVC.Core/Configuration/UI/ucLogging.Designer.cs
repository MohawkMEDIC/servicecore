namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    partial class ucLogging
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.chkRollover = new System.Windows.Forms.CheckBox();
            this.pnlFile = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkInfo = new System.Windows.Forms.CheckBox();
            this.chkWarning = new System.Windows.Forms.CheckBox();
            this.chkErrors = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.chkEnableLogging = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblFileSize = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_fileScan = new System.ComponentModel.BackgroundWorker();
            this.fSizeRefresh = new System.Windows.Forms.Timer(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.chkTraceSources = new System.Windows.Forms.CheckedListBox();
            this.pnlFile.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(387, 31);
            this.label1.TabIndex = 2;
            this.label1.Text = "Service Logging & Tracing";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.UseMnemonic = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 11);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "File:";
            // 
            // txtFileName
            // 
            this.txtFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFileName.Location = new System.Drawing.Point(62, 6);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ReadOnly = true;
            this.txtFileName.Size = new System.Drawing.Size(247, 26);
            this.txtFileName.TabIndex = 4;
            this.txtFileName.TextChanged += new System.EventHandler(this.txtFileName_TextChanged);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.Location = new System.Drawing.Point(320, 3);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(51, 35);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // chkRollover
            // 
            this.chkRollover.AutoSize = true;
            this.chkRollover.Checked = true;
            this.chkRollover.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRollover.Location = new System.Drawing.Point(18, 46);
            this.chkRollover.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkRollover.Name = "chkRollover";
            this.chkRollover.Size = new System.Drawing.Size(217, 24);
            this.chkRollover.TabIndex = 6;
            this.chkRollover.Text = "Rollover log files each day";
            this.chkRollover.UseVisualStyleBackColor = true;
            // 
            // pnlFile
            // 
            this.pnlFile.Controls.Add(this.groupBox1);
            this.pnlFile.Controls.Add(this.label2);
            this.pnlFile.Controls.Add(this.btnBrowse);
            this.pnlFile.Controls.Add(this.chkRollover);
            this.pnlFile.Controls.Add(this.txtFileName);
            this.pnlFile.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFile.Enabled = false;
            this.pnlFile.Location = new System.Drawing.Point(0, 61);
            this.pnlFile.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlFile.Name = "pnlFile";
            this.pnlFile.Size = new System.Drawing.Size(387, 228);
            this.pnlFile.TabIndex = 7;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkInfo);
            this.groupBox1.Controls.Add(this.chkWarning);
            this.groupBox1.Controls.Add(this.chkErrors);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(18, 82);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(352, 129);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Messages";
            // 
            // chkInfo
            // 
            this.chkInfo.AutoSize = true;
            this.chkInfo.Checked = true;
            this.chkInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInfo.Location = new System.Drawing.Point(218, 82);
            this.chkInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkInfo.Name = "chkInfo";
            this.chkInfo.Size = new System.Drawing.Size(116, 24);
            this.chkInfo.TabIndex = 10;
            this.chkInfo.Text = "Information";
            this.chkInfo.UseVisualStyleBackColor = true;
            // 
            // chkWarning
            // 
            this.chkWarning.AutoSize = true;
            this.chkWarning.Checked = true;
            this.chkWarning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWarning.Location = new System.Drawing.Point(102, 82);
            this.chkWarning.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkWarning.Name = "chkWarning";
            this.chkWarning.Size = new System.Drawing.Size(102, 24);
            this.chkWarning.TabIndex = 9;
            this.chkWarning.Text = "Warnings";
            this.chkWarning.UseVisualStyleBackColor = true;
            // 
            // chkErrors
            // 
            this.chkErrors.AutoSize = true;
            this.chkErrors.Checked = true;
            this.chkErrors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkErrors.Location = new System.Drawing.Point(14, 82);
            this.chkErrors.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkErrors.Name = "chkErrors";
            this.chkErrors.Size = new System.Drawing.Size(78, 24);
            this.chkErrors.TabIndex = 8;
            this.chkErrors.Text = "Errors";
            this.chkErrors.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.Location = new System.Drawing.Point(9, 25);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(334, 52);
            this.label3.TabIndex = 8;
            this.label3.Text = "Messages of the following type will be appended to the trace log";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(0, 289);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(387, 31);
            this.label4.TabIndex = 8;
            this.label4.Text = "Trace Log Size";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label4.UseMnemonic = false;
            // 
            // chkEnableLogging
            // 
            this.chkEnableLogging.AutoSize = true;
            this.chkEnableLogging.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkEnableLogging.Location = new System.Drawing.Point(0, 31);
            this.chkEnableLogging.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkEnableLogging.Name = "chkEnableLogging";
            this.chkEnableLogging.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.chkEnableLogging.Size = new System.Drawing.Size(387, 30);
            this.chkEnableLogging.TabIndex = 8;
            this.chkEnableLogging.Text = "Log service messages to a file";
            this.chkEnableLogging.UseVisualStyleBackColor = true;
            this.chkEnableLogging.CheckedChanged += new System.EventHandler(this.chkEnableLogging_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(8, 15);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(158, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Current Log Size:";
            // 
            // lblFileSize
            // 
            this.lblFileSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFileSize.Location = new System.Drawing.Point(174, 15);
            this.lblFileSize.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblFileSize.Name = "lblFileSize";
            this.lblFileSize.Size = new System.Drawing.Size(196, 20);
            this.lblFileSize.TabIndex = 8;
            this.lblFileSize.Text = "0 KB";
            this.lblFileSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblFileSize);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 320);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(387, 54);
            this.panel1.TabIndex = 11;
            // 
            // m_fileScan
            // 
            this.m_fileScan.DoWork += new System.ComponentModel.DoWorkEventHandler(this.m_fileScan_DoWork);
            this.m_fileScan.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.m_fileScan_RunWorkerCompleted);
            // 
            // fSizeRefresh
            // 
            this.fSizeRefresh.Enabled = true;
            this.fSizeRefresh.Interval = 500;
            this.fSizeRefresh.Tick += new System.EventHandler(this.fSizeRefresh_Tick);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label6.Dock = System.Windows.Forms.DockStyle.Top;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label6.Location = new System.Drawing.Point(0, 374);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(387, 31);
            this.label6.TabIndex = 12;
            this.label6.Text = "Trace Sources";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label6.UseMnemonic = false;
            // 
            // chkTraceSources
            // 
            this.chkTraceSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkTraceSources.FormattingEnabled = true;
            this.chkTraceSources.Location = new System.Drawing.Point(0, 405);
            this.chkTraceSources.Name = "chkTraceSources";
            this.chkTraceSources.Size = new System.Drawing.Size(387, 206);
            this.chkTraceSources.TabIndex = 13;
            // 
            // ucLogging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkTraceSources);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.pnlFile);
            this.Controls.Add(this.chkEnableLogging);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ucLogging";
            this.Size = new System.Drawing.Size(387, 611);
            this.pnlFile.ResumeLayout(false);
            this.pnlFile.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.CheckBox chkRollover;
        private System.Windows.Forms.Panel pnlFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkInfo;
        private System.Windows.Forms.CheckBox chkWarning;
        private System.Windows.Forms.CheckBox chkErrors;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkEnableLogging;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblFileSize;
        private System.Windows.Forms.Panel panel1;
        private System.ComponentModel.BackgroundWorker m_fileScan;
        private System.Windows.Forms.Timer fSizeRefresh;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckedListBox chkTraceSources;
    }
}
