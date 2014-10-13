namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Configuration.UI
{
    partial class pnlConfigureFhir
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(pnlConfigureFhir));
            this.label1 = new System.Windows.Forms.Label();
            this.pbFhirLogo = new System.Windows.Forms.PictureBox();
            this.pnlAboutFhir = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlConfiguration = new System.Windows.Forms.Panel();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.grpSSL = new System.Windows.Forms.GroupBox();
            this.btnChooseCert = new System.Windows.Forms.Button();
            this.txtCertificate = new System.Windows.Forms.TextBox();
            this.cbxStoreLocation = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbxStore = new System.Windows.Forms.ComboBox();
            this.chkPKI = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.chkMetaData = new System.Windows.Forms.CheckBox();
            this.chkDebug = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnChooseIndex = new System.Windows.Forms.Button();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lsvProfiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.pbFhirLogo)).BeginInit();
            this.pnlAboutFhir.SuspendLayout();
            this.pnlConfiguration.SuspendLayout();
            this.grpSSL.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(0, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(372, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Endpoint Configuration";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbFhirLogo
            // 
            this.pbFhirLogo.Image = ((System.Drawing.Image)(resources.GetObject("pbFhirLogo.Image")));
            this.pbFhirLogo.Location = new System.Drawing.Point(3, 3);
            this.pbFhirLogo.Name = "pbFhirLogo";
            this.pbFhirLogo.Size = new System.Drawing.Size(34, 34);
            this.pbFhirLogo.TabIndex = 1;
            this.pbFhirLogo.TabStop = false;
            // 
            // pnlAboutFhir
            // 
            this.pnlAboutFhir.BackColor = System.Drawing.Color.White;
            this.pnlAboutFhir.Controls.Add(this.label3);
            this.pnlAboutFhir.Controls.Add(this.label2);
            this.pnlAboutFhir.Controls.Add(this.pbFhirLogo);
            this.pnlAboutFhir.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAboutFhir.Location = new System.Drawing.Point(0, 0);
            this.pnlAboutFhir.Name = "pnlAboutFhir";
            this.pnlAboutFhir.Size = new System.Drawing.Size(372, 100);
            this.pnlAboutFhir.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Location = new System.Drawing.Point(0, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(372, 60);
            this.label3.TabIndex = 3;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(43, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(319, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fast Health Interoperability Resources";
            // 
            // pnlConfiguration
            // 
            this.pnlConfiguration.Controls.Add(this.txtAddress);
            this.pnlConfiguration.Controls.Add(this.grpSSL);
            this.pnlConfiguration.Controls.Add(this.label7);
            this.pnlConfiguration.Controls.Add(this.chkMetaData);
            this.pnlConfiguration.Controls.Add(this.chkDebug);
            this.pnlConfiguration.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlConfiguration.Location = new System.Drawing.Point(0, 119);
            this.pnlConfiguration.Name = "pnlConfiguration";
            this.pnlConfiguration.Size = new System.Drawing.Size(372, 214);
            this.pnlConfiguration.TabIndex = 10;
            // 
            // txtAddress
            // 
            this.txtAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAddress.Location = new System.Drawing.Point(60, 6);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(300, 20);
            this.txtAddress.TabIndex = 2;
            this.txtAddress.Validated += new System.EventHandler(this.txtAddress_Validated);
            // 
            // grpSSL
            // 
            this.grpSSL.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSSL.Controls.Add(this.btnChooseCert);
            this.grpSSL.Controls.Add(this.txtCertificate);
            this.grpSSL.Controls.Add(this.cbxStoreLocation);
            this.grpSSL.Controls.Add(this.label4);
            this.grpSSL.Controls.Add(this.cbxStore);
            this.grpSSL.Controls.Add(this.chkPKI);
            this.grpSSL.Controls.Add(this.label5);
            this.grpSSL.Controls.Add(this.label6);
            this.grpSSL.Enabled = false;
            this.grpSSL.Location = new System.Drawing.Point(9, 78);
            this.grpSSL.Name = "grpSSL";
            this.grpSSL.Size = new System.Drawing.Size(351, 133);
            this.grpSSL.TabIndex = 6;
            this.grpSSL.TabStop = false;
            this.grpSSL.Text = "Security Configuration";
            // 
            // btnChooseCert
            // 
            this.btnChooseCert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseCert.Location = new System.Drawing.Point(312, 80);
            this.btnChooseCert.Name = "btnChooseCert";
            this.btnChooseCert.Size = new System.Drawing.Size(33, 20);
            this.btnChooseCert.TabIndex = 9;
            this.btnChooseCert.Text = "...";
            this.btnChooseCert.UseVisualStyleBackColor = true;
            this.btnChooseCert.Click += new System.EventHandler(this.btnChooseCert_Click);
            // 
            // txtCertificate
            // 
            this.txtCertificate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCertificate.Location = new System.Drawing.Point(97, 80);
            this.txtCertificate.Name = "txtCertificate";
            this.txtCertificate.ReadOnly = true;
            this.txtCertificate.Size = new System.Drawing.Size(209, 20);
            this.txtCertificate.TabIndex = 8;
            // 
            // cbxStoreLocation
            // 
            this.cbxStoreLocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxStoreLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStoreLocation.FormattingEnabled = true;
            this.cbxStoreLocation.Location = new System.Drawing.Point(97, 26);
            this.cbxStoreLocation.Name = "cbxStoreLocation";
            this.cbxStoreLocation.Size = new System.Drawing.Size(248, 21);
            this.cbxStoreLocation.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Store Location:";
            // 
            // cbxStore
            // 
            this.cbxStore.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxStore.FormattingEnabled = true;
            this.cbxStore.Location = new System.Drawing.Point(97, 53);
            this.cbxStore.Name = "cbxStore";
            this.cbxStore.Size = new System.Drawing.Size(248, 21);
            this.cbxStore.TabIndex = 7;
            // 
            // chkPKI
            // 
            this.chkPKI.AutoSize = true;
            this.chkPKI.Location = new System.Drawing.Point(37, 110);
            this.chkPKI.Name = "chkPKI";
            this.chkPKI.Size = new System.Drawing.Size(193, 17);
            this.chkPKI.TabIndex = 10;
            this.chkPKI.Text = "Require clients to have a certificate";
            this.chkPKI.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 83);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Certificate:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Certificate Store:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Address:";
            // 
            // chkMetaData
            // 
            this.chkMetaData.AutoSize = true;
            this.chkMetaData.Location = new System.Drawing.Point(60, 55);
            this.chkMetaData.Name = "chkMetaData";
            this.chkMetaData.Size = new System.Drawing.Size(237, 17);
            this.chkMetaData.TabIndex = 5;
            this.chkMetaData.Text = "Enable meta-data publishing on this endpoint";
            this.chkMetaData.UseVisualStyleBackColor = true;
            // 
            // chkDebug
            // 
            this.chkDebug.AutoSize = true;
            this.chkDebug.Location = new System.Drawing.Point(60, 32);
            this.chkDebug.Name = "chkDebug";
            this.chkDebug.Size = new System.Drawing.Size(190, 17);
            this.chkDebug.TabIndex = 4;
            this.chkDebug.Text = "Enable debugging on this endpoint";
            this.chkDebug.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label8.Dock = System.Windows.Forms.DockStyle.Top;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label8.Location = new System.Drawing.Point(0, 333);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(372, 19);
            this.label8.TabIndex = 11;
            this.label8.Text = "Landing Page";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnChooseIndex);
            this.panel1.Controls.Add(this.txtIndex);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 352);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(372, 37);
            this.panel1.TabIndex = 12;
            // 
            // btnChooseIndex
            // 
            this.btnChooseIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChooseIndex.Location = new System.Drawing.Point(321, 8);
            this.btnChooseIndex.Name = "btnChooseIndex";
            this.btnChooseIndex.Size = new System.Drawing.Size(33, 20);
            this.btnChooseIndex.TabIndex = 12;
            this.btnChooseIndex.Text = "...";
            this.btnChooseIndex.UseVisualStyleBackColor = true;
            this.btnChooseIndex.Click += new System.EventHandler(this.btnChooseIndex_Click);
            // 
            // txtIndex
            // 
            this.txtIndex.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtIndex.Location = new System.Drawing.Point(60, 8);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.ReadOnly = true;
            this.txtIndex.Size = new System.Drawing.Size(255, 20);
            this.txtIndex.TabIndex = 11;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(36, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Index:";
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label10.Dock = System.Windows.Forms.DockStyle.Top;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label10.Location = new System.Drawing.Point(0, 389);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(372, 19);
            this.label10.TabIndex = 13;
            this.label10.Text = "Resource Handlers";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lsvProfiles
            // 
            this.lsvProfiles.CheckBoxes = true;
            this.lsvProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lsvProfiles.Dock = System.Windows.Forms.DockStyle.Top;
            this.lsvProfiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lsvProfiles.HideSelection = false;
            this.lsvProfiles.Location = new System.Drawing.Point(0, 408);
            this.lsvProfiles.Name = "lsvProfiles";
            this.lsvProfiles.Size = new System.Drawing.Size(372, 131);
            this.lsvProfiles.TabIndex = 14;
            this.lsvProfiles.UseCompatibleStateImageBehavior = false;
            this.lsvProfiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Resource";
            this.columnHeader1.Width = 136;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Profile";
            this.columnHeader2.Width = 185;
            // 
            // pnlConfigureFhir
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.lsvProfiles);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pnlConfiguration);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pnlAboutFhir);
            this.Name = "pnlConfigureFhir";
            this.Size = new System.Drawing.Size(372, 549);
            ((System.ComponentModel.ISupportInitialize)(this.pbFhirLogo)).EndInit();
            this.pnlAboutFhir.ResumeLayout(false);
            this.pnlAboutFhir.PerformLayout();
            this.pnlConfiguration.ResumeLayout(false);
            this.pnlConfiguration.PerformLayout();
            this.grpSSL.ResumeLayout(false);
            this.grpSSL.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbFhirLogo;
        private System.Windows.Forms.Panel pnlAboutFhir;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlConfiguration;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.GroupBox grpSSL;
        private System.Windows.Forms.Button btnChooseCert;
        private System.Windows.Forms.TextBox txtCertificate;
        private System.Windows.Forms.ComboBox cbxStoreLocation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbxStore;
        private System.Windows.Forms.CheckBox chkPKI;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkMetaData;
        private System.Windows.Forms.CheckBox chkDebug;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnChooseIndex;
        private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ListView lsvProfiles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}
