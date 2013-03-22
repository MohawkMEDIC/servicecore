namespace MARC.HI.EHRS.SVC.Terminology.Configuration
{
    partial class pnlConfigureTerminology
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
            this.label1 = new System.Windows.Forms.Label();
            this.chkEnableDb = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkEnableCTS = new System.Windows.Forms.CheckBox();
            this.txtCTSUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.numCacheSize = new System.Windows.Forms.NumericUpDown();
            this.chkSNOMED = new System.Windows.Forms.CheckBox();
            this.chkLOINC = new System.Windows.Forms.CheckBox();
            this.chkICD = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dbSelector = new MARC.HI.EHRS.SVC.ConfigurationApplciation.DatabaseSelector();
            this.panel2 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.numCacheSize)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(408, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Memory Pool Size";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkEnableDb
            // 
            this.chkEnableDb.AutoSize = true;
            this.chkEnableDb.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkEnableDb.Location = new System.Drawing.Point(0, 70);
            this.chkEnableDb.Name = "chkEnableDb";
            this.chkEnableDb.Padding = new System.Windows.Forms.Padding(2);
            this.chkEnableDb.Size = new System.Drawing.Size(408, 21);
            this.chkEnableDb.TabIndex = 28;
            this.chkEnableDb.Text = "Enable local code validation";
            this.chkEnableDb.UseVisualStyleBackColor = true;
            this.chkEnableDb.CheckedChanged += new System.EventHandler(this.chkEnableDb_CheckedChanged);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(0, 228);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(408, 21);
            this.label2.TabIndex = 29;
            this.label2.Text = "Remote Code Validation";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkEnableCTS
            // 
            this.chkEnableCTS.AutoSize = true;
            this.chkEnableCTS.Location = new System.Drawing.Point(3, 7);
            this.chkEnableCTS.Name = "chkEnableCTS";
            this.chkEnableCTS.Size = new System.Drawing.Size(259, 17);
            this.chkEnableCTS.TabIndex = 3;
            this.chkEnableCTS.Text = "Enable centralized code validation (HL7 CTS 1.2)";
            this.chkEnableCTS.UseVisualStyleBackColor = true;
            this.chkEnableCTS.CheckedChanged += new System.EventHandler(this.chkEnableCTS_CheckedChanged);
            // 
            // txtCTSUrl
            // 
            this.txtCTSUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCTSUrl.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtCTSUrl.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.AllUrl;
            this.txtCTSUrl.Enabled = false;
            this.txtCTSUrl.Location = new System.Drawing.Point(100, 30);
            this.txtCTSUrl.Name = "txtCTSUrl";
            this.txtCTSUrl.Size = new System.Drawing.Size(290, 20);
            this.txtCTSUrl.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "MRT URL:";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label4.Dock = System.Windows.Forms.DockStyle.Top;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(0, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(408, 21);
            this.label4.TabIndex = 33;
            this.label4.Text = "Local Code Validation";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(17, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(138, 13);
            this.label10.TabIndex = 34;
            this.label10.Text = "Maximum Memory Pool Size";
            // 
            // numCacheSize
            // 
            this.numCacheSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.numCacheSize.Increment = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numCacheSize.Location = new System.Drawing.Point(278, 3);
            this.numCacheSize.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.numCacheSize.Name = "numCacheSize";
            this.numCacheSize.Size = new System.Drawing.Size(116, 20);
            this.numCacheSize.TabIndex = 1;
            this.numCacheSize.ThousandsSeparator = true;
            this.numCacheSize.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // chkSNOMED
            // 
            this.chkSNOMED.AutoSize = true;
            this.chkSNOMED.Location = new System.Drawing.Point(3, 56);
            this.chkSNOMED.Name = "chkSNOMED";
            this.chkSNOMED.Size = new System.Drawing.Size(157, 17);
            this.chkSNOMED.TabIndex = 5;
            this.chkSNOMED.Text = "Supports SNOMED(TM) CT";
            this.chkSNOMED.UseVisualStyleBackColor = true;
            // 
            // chkLOINC
            // 
            this.chkLOINC.AutoSize = true;
            this.chkLOINC.Location = new System.Drawing.Point(3, 79);
            this.chkLOINC.Name = "chkLOINC";
            this.chkLOINC.Size = new System.Drawing.Size(117, 17);
            this.chkLOINC.TabIndex = 6;
            this.chkLOINC.Text = "Supports LOINC(R)";
            this.chkLOINC.UseVisualStyleBackColor = true;
            // 
            // chkICD
            // 
            this.chkICD.AutoSize = true;
            this.chkICD.Location = new System.Drawing.Point(3, 102);
            this.chkICD.Name = "chkICD";
            this.chkICD.Size = new System.Drawing.Size(101, 17);
            this.chkICD.TabIndex = 7;
            this.chkICD.Text = "Supports ICD10";
            this.chkICD.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.numCacheSize);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 29);
            this.panel1.TabIndex = 40;
            // 
            // dbSelector
            // 
            this.dbSelector.DatabaseConfigurator = null;
            this.dbSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.dbSelector.Enabled = false;
            this.dbSelector.Location = new System.Drawing.Point(0, 91);
            this.dbSelector.MinimumSize = new System.Drawing.Size(0, 137);
            this.dbSelector.Name = "dbSelector";
            this.dbSelector.Size = new System.Drawing.Size(408, 137);
            this.dbSelector.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.chkICD);
            this.panel2.Controls.Add(this.chkEnableCTS);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtCTSUrl);
            this.panel2.Controls.Add(this.chkLOINC);
            this.panel2.Controls.Add(this.chkSNOMED);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 249);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(408, 131);
            this.panel2.TabIndex = 42;
            // 
            // pnlConfigureTerminology
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dbSelector);
            this.Controls.Add(this.chkEnableDb);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "pnlConfigureTerminology";
            this.Size = new System.Drawing.Size(408, 527);
            ((System.ComponentModel.ISupportInitialize)(this.numCacheSize)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkEnableDb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkEnableCTS;
        private System.Windows.Forms.TextBox txtCTSUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numCacheSize;
        private System.Windows.Forms.CheckBox chkSNOMED;
        private System.Windows.Forms.CheckBox chkLOINC;
        private System.Windows.Forms.CheckBox chkICD;
        private System.Windows.Forms.Panel panel1;
        private ConfigurationApplciation.DatabaseSelector dbSelector;
        private System.Windows.Forms.Panel panel2;
    }
}
