namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    partial class ucCoreProperties
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDeviceId = new System.Windows.Forms.TextBox();
            this.btnBrowseDevice = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtCustodianName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCustodianId = new System.Windows.Forms.TextBox();
            this.btnBrowseCustodian = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtELID = new System.Windows.Forms.TextBox();
            this.btnBrowseELID = new System.Windows.Forms.Button();
            this.txtEPID = new System.Windows.Forms.TextBox();
            this.btnBrowseEPID = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtECID = new System.Windows.Forms.TextBox();
            this.btnBrowseECID = new System.Windows.Forms.Button();
            this.txtJurisdictionName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtJurisdictionId = new System.Windows.Forms.TextBox();
            this.btnBrowseJurisdiction = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this.label1.Size = new System.Drawing.Size(386, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Device Information";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label1.UseMnemonic = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtDeviceId);
            this.panel1.Controls.Add(this.btnBrowseDevice);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 20);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(386, 75);
            this.panel1.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(8, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(370, 29);
            this.label5.TabIndex = 2;
            this.label5.Text = "The following information will be used to identify the system on which this servi" +
    "ce runs to solicitors";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Device ID:";
            // 
            // txtDeviceId
            // 
            this.txtDeviceId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeviceId.Location = new System.Drawing.Point(67, 43);
            this.txtDeviceId.Name = "txtDeviceId";
            this.txtDeviceId.ReadOnly = true;
            this.txtDeviceId.Size = new System.Drawing.Size(265, 20);
            this.txtDeviceId.TabIndex = 88;
            // 
            // btnBrowseDevice
            // 
            this.btnBrowseDevice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseDevice.Location = new System.Drawing.Point(338, 41);
            this.btnBrowseDevice.Name = "btnBrowseDevice";
            this.btnBrowseDevice.Size = new System.Drawing.Size(38, 23);
            this.btnBrowseDevice.TabIndex = 2;
            this.btnBrowseDevice.Text = "...";
            this.btnBrowseDevice.UseVisualStyleBackColor = true;
            this.btnBrowseDevice.Click += new System.EventHandler(this.btnBrowseDevice_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtCustodianName);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.txtCustodianId);
            this.panel2.Controls.Add(this.btnBrowseCustodian);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 115);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(386, 95);
            this.panel2.TabIndex = 6;
            // 
            // txtCustodianName
            // 
            this.txtCustodianName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustodianName.Location = new System.Drawing.Point(67, 66);
            this.txtCustodianName.Name = "txtCustodianName";
            this.txtCustodianName.Size = new System.Drawing.Size(309, 20);
            this.txtCustodianName.TabIndex = 4;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Name:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(8, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(370, 29);
            this.label6.TabIndex = 6;
            this.label6.Text = "The following information is used to logically identify the custodian of any reco" +
    "rds created by this sservice or other instances within the logical group.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Group ID:";
            // 
            // txtCustodianId
            // 
            this.txtCustodianId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustodianId.Location = new System.Drawing.Point(67, 40);
            this.txtCustodianId.Name = "txtCustodianId";
            this.txtCustodianId.ReadOnly = true;
            this.txtCustodianId.Size = new System.Drawing.Size(265, 20);
            this.txtCustodianId.TabIndex = 88;
            // 
            // btnBrowseCustodian
            // 
            this.btnBrowseCustodian.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseCustodian.Location = new System.Drawing.Point(338, 38);
            this.btnBrowseCustodian.Name = "btnBrowseCustodian";
            this.btnBrowseCustodian.Size = new System.Drawing.Size(38, 23);
            this.btnBrowseCustodian.TabIndex = 3;
            this.btnBrowseCustodian.Text = "...";
            this.btnBrowseCustodian.UseVisualStyleBackColor = true;
            this.btnBrowseCustodian.Click += new System.EventHandler(this.btnBrowseCustodian_Click);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(0, 95);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(386, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Custodianship Information";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label2.UseMnemonic = false;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.AutoScrollMinSize = new System.Drawing.Size(0, 188);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.txtELID);
            this.panel3.Controls.Add(this.btnBrowseELID);
            this.panel3.Controls.Add(this.txtEPID);
            this.panel3.Controls.Add(this.btnBrowseEPID);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Controls.Add(this.txtECID);
            this.panel3.Controls.Add(this.btnBrowseECID);
            this.panel3.Controls.Add(this.txtJurisdictionName);
            this.panel3.Controls.Add(this.label9);
            this.panel3.Controls.Add(this.label10);
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.txtJurisdictionId);
            this.panel3.Controls.Add(this.btnBrowseJurisdiction);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 230);
            this.panel3.MinimumSize = new System.Drawing.Size(0, 188);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(386, 190);
            this.panel3.TabIndex = 8;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(4, 151);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 13);
            this.label14.TabIndex = 19;
            this.label14.Text = "Enterprise LID:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 125);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 13);
            this.label13.TabIndex = 19;
            this.label13.Text = "Enterprise PID:";
            // 
            // txtELID
            // 
            this.txtELID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtELID.Location = new System.Drawing.Point(88, 148);
            this.txtELID.Name = "txtELID";
            this.txtELID.ReadOnly = true;
            this.txtELID.Size = new System.Drawing.Size(244, 20);
            this.txtELID.TabIndex = 88;
            // 
            // btnBrowseELID
            // 
            this.btnBrowseELID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseELID.Location = new System.Drawing.Point(338, 146);
            this.btnBrowseELID.Name = "btnBrowseELID";
            this.btnBrowseELID.Size = new System.Drawing.Size(38, 23);
            this.btnBrowseELID.TabIndex = 9;
            this.btnBrowseELID.Text = "...";
            this.btnBrowseELID.UseVisualStyleBackColor = true;
            this.btnBrowseELID.Click += new System.EventHandler(this.btnBrowseELID_Click);
            // 
            // txtEPID
            // 
            this.txtEPID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEPID.Location = new System.Drawing.Point(88, 122);
            this.txtEPID.Name = "txtEPID";
            this.txtEPID.ReadOnly = true;
            this.txtEPID.Size = new System.Drawing.Size(244, 20);
            this.txtEPID.TabIndex = 88;
            // 
            // btnBrowseEPID
            // 
            this.btnBrowseEPID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseEPID.Location = new System.Drawing.Point(338, 120);
            this.btnBrowseEPID.Name = "btnBrowseEPID";
            this.btnBrowseEPID.Size = new System.Drawing.Size(38, 23);
            this.btnBrowseEPID.TabIndex = 8;
            this.btnBrowseEPID.Text = "...";
            this.btnBrowseEPID.UseVisualStyleBackColor = true;
            this.btnBrowseEPID.Click += new System.EventHandler(this.btnBrowseEPID_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 99);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(78, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "Enterprise CID:";
            // 
            // txtECID
            // 
            this.txtECID.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtECID.Location = new System.Drawing.Point(88, 96);
            this.txtECID.Name = "txtECID";
            this.txtECID.ReadOnly = true;
            this.txtECID.Size = new System.Drawing.Size(244, 20);
            this.txtECID.TabIndex = 88;
            // 
            // btnBrowseECID
            // 
            this.btnBrowseECID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseECID.Location = new System.Drawing.Point(338, 94);
            this.btnBrowseECID.Name = "btnBrowseECID";
            this.btnBrowseECID.Size = new System.Drawing.Size(38, 23);
            this.btnBrowseECID.TabIndex = 7;
            this.btnBrowseECID.Text = "...";
            this.btnBrowseECID.UseVisualStyleBackColor = true;
            this.btnBrowseECID.Click += new System.EventHandler(this.btnBrowseECID_Click);
            // 
            // txtJurisdictionName
            // 
            this.txtJurisdictionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJurisdictionName.Location = new System.Drawing.Point(88, 70);
            this.txtJurisdictionName.Name = "txtJurisdictionName";
            this.txtJurisdictionName.Size = new System.Drawing.Size(288, 20);
            this.txtJurisdictionName.TabIndex = 6;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(44, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "Name:";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.Location = new System.Drawing.Point(8, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(370, 29);
            this.label10.TabIndex = 12;
            this.label10.Text = "The following information is used to identify the jurisdiction to which records s" +
    "tored in this service belong.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(76, 13);
            this.label11.TabIndex = 10;
            this.label11.Text = "Jurisdiction ID:";
            // 
            // txtJurisdictionId
            // 
            this.txtJurisdictionId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJurisdictionId.Location = new System.Drawing.Point(88, 44);
            this.txtJurisdictionId.Name = "txtJurisdictionId";
            this.txtJurisdictionId.ReadOnly = true;
            this.txtJurisdictionId.Size = new System.Drawing.Size(244, 20);
            this.txtJurisdictionId.TabIndex = 88;
            // 
            // btnBrowseJurisdiction
            // 
            this.btnBrowseJurisdiction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowseJurisdiction.Location = new System.Drawing.Point(338, 42);
            this.btnBrowseJurisdiction.Name = "btnBrowseJurisdiction";
            this.btnBrowseJurisdiction.Size = new System.Drawing.Size(38, 23);
            this.btnBrowseJurisdiction.TabIndex = 5;
            this.btnBrowseJurisdiction.Text = "...";
            this.btnBrowseJurisdiction.UseVisualStyleBackColor = true;
            this.btnBrowseJurisdiction.Click += new System.EventHandler(this.btnBrowseJurisdiction_Click);
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(0, 210);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(386, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Jurisdiction Information";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.label3.UseMnemonic = false;
            // 
            // ucCoreProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(0, 418);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.Name = "ucCoreProperties";
            this.Size = new System.Drawing.Size(386, 420);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDeviceId;
        private System.Windows.Forms.Button btnBrowseDevice;
        private System.Windows.Forms.TextBox txtCustodianName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCustodianId;
        private System.Windows.Forms.Button btnBrowseCustodian;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtELID;
        private System.Windows.Forms.Button btnBrowseELID;
        private System.Windows.Forms.TextBox txtEPID;
        private System.Windows.Forms.Button btnBrowseEPID;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtECID;
        private System.Windows.Forms.Button btnBrowseECID;
        private System.Windows.Forms.TextBox txtJurisdictionName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtJurisdictionId;
        private System.Windows.Forms.Button btnBrowseJurisdiction;
    }
}
