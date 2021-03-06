﻿namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    public partial class frmStartScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStartScreen));
            MARC.HI.EHRS.SVC.Configuration.Data.DbConnectionString dbConnectionString1 = new MARC.HI.EHRS.SVC.Configuration.Data.DbConnectionString();
            this.pnlLogo = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lblText = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rdoEasy = new System.Windows.Forms.RadioButton();
            this.rdoAdvanced = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dbSelector = new MARC.HI.EHRS.SVC.Configuration.UI.DatabaseSelector();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.pnlLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlLogo
            // 
            this.pnlLogo.BackColor = System.Drawing.Color.White;
            this.pnlLogo.Controls.Add(this.label1);
            this.pnlLogo.Controls.Add(this.lblText);
            this.pnlLogo.Controls.Add(this.pictureBox1);
            this.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlLogo.Location = new System.Drawing.Point(0, 0);
            this.pnlLogo.Name = "pnlLogo";
            this.pnlLogo.Size = new System.Drawing.Size(537, 70);
            this.pnlLogo.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(261, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Welcome to the MARC-HI Service Configuration Tool.";
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.Location = new System.Drawing.Point(73, 18);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(181, 20);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "Service Configuration";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(67, 70);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // rdoEasy
            // 
            this.rdoEasy.AutoSize = true;
            this.rdoEasy.Checked = true;
            this.rdoEasy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoEasy.Location = new System.Drawing.Point(26, 91);
            this.rdoEasy.Name = "rdoEasy";
            this.rdoEasy.Size = new System.Drawing.Size(226, 17);
            this.rdoEasy.TabIndex = 0;
            this.rdoEasy.TabStop = true;
            this.rdoEasy.Text = "Easy Configuration (Recommended)";
            this.rdoEasy.UseVisualStyleBackColor = true;
            // 
            // rdoAdvanced
            // 
            this.rdoAdvanced.AutoSize = true;
            this.rdoAdvanced.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoAdvanced.Location = new System.Drawing.Point(26, 416);
            this.rdoAdvanced.Name = "rdoAdvanced";
            this.rdoAdvanced.Size = new System.Drawing.Size(161, 17);
            this.rdoAdvanced.TabIndex = 1;
            this.rdoAdvanced.Text = "Advanced Configuration";
            this.rdoAdvanced.UseVisualStyleBackColor = true;
            this.rdoAdvanced.CheckedChanged += new System.EventHandler(this.rdoAdvanced_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(47, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(478, 31);
            this.label2.TabIndex = 3;
            this.label2.Text = "Select this option if you are deploying this service on a single server or do not" +
    " want to setup additional configuration options such as terminology or custom va" +
    "lidation rules";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(47, 436);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(478, 31);
            this.label3.TabIndex = 4;
            this.label3.Text = "Select this option of you wish to deploy this service on multiple servers, or wis" +
    "h to customize the deployment options prior to configuring the Shared Health Rec" +
    "ord";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dbSelector);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(26, 145);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(490, 265);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database Parameters";
            // 
            // dbSelector
            // 
            dbConnectionString1.Database = "";
            dbConnectionString1.Host = "";
            dbConnectionString1.Name = null;
            dbConnectionString1.Password = "";
            dbConnectionString1.Provider = null;
            dbConnectionString1.UserName = "";
            this.dbSelector.ConnectionString = dbConnectionString1;
            this.dbSelector.Location = new System.Drawing.Point(24, 112);
            this.dbSelector.MinimumSize = new System.Drawing.Size(0, 137);
            this.dbSelector.Name = "dbSelector";
            this.dbSelector.Size = new System.Drawing.Size(436, 137);
            this.dbSelector.TabIndex = 1;
            this.dbSelector.Validated += new System.EventHandler(this.dbSelector_Validated);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(21, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(439, 93);
            this.label4.TabIndex = 0;
            this.label4.Text = resources.GetString("label4.Text");
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(441, 478);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnContinue
            // 
            this.btnContinue.Enabled = false;
            this.btnContinue.Location = new System.Drawing.Point(360, 478);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(75, 23);
            this.btnContinue.TabIndex = 7;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // frmStartScreen
            // 
            this.AcceptButton = this.btnContinue;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(537, 508);
            this.Controls.Add(this.btnContinue);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rdoAdvanced);
            this.Controls.Add(this.rdoEasy);
            this.Controls.Add(this.pnlLogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmStartScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MARC-HI Service Configuration";
            this.pnlLogo.ResumeLayout(false);
            this.pnlLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlLogo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.RadioButton rdoEasy;
        private System.Windows.Forms.RadioButton rdoAdvanced;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnContinue;
        private DatabaseSelector dbSelector;
    }
}