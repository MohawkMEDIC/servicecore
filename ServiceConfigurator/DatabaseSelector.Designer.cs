namespace MARC.HI.EHRS.SVC.ConfigurationApplciation
{
    partial class DatabaseSelector
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
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtDatabaseAddress = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbxDatabase = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbxProviderType = new System.Windows.Forms.ComboBox();
            this.btnNew = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(107, 82);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(273, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.Validated += new System.EventHandler(this.connectionParameter_Validated);
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserName.Location = new System.Drawing.Point(107, 56);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(273, 20);
            this.txtUserName.TabIndex = 3;
            this.txtUserName.Validated += new System.EventHandler(this.connectionParameter_Validated);
            // 
            // txtDatabaseAddress
            // 
            this.txtDatabaseAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabaseAddress.Location = new System.Drawing.Point(107, 30);
            this.txtDatabaseAddress.Name = "txtDatabaseAddress";
            this.txtDatabaseAddress.Size = new System.Drawing.Size(273, 20);
            this.txtDatabaseAddress.TabIndex = 2;
            this.txtDatabaseAddress.Validated += new System.EventHandler(this.connectionParameter_Validated);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 13);
            this.label9.TabIndex = 38;
            this.label9.Text = "Server Address";
            // 
            // cbxDatabase
            // 
            this.cbxDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxDatabase.Enabled = false;
            this.cbxDatabase.FormattingEnabled = true;
            this.cbxDatabase.Location = new System.Drawing.Point(107, 108);
            this.cbxDatabase.Name = "cbxDatabase";
            this.cbxDatabase.Size = new System.Drawing.Size(207, 21);
            this.cbxDatabase.TabIndex = 6;
            this.cbxDatabase.DropDown += new System.EventHandler(this.cbxDatabase_DropDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(48, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Password";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(41, 59);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(60, 13);
            this.label7.TabIndex = 33;
            this.label7.Text = "User Name";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 111);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Database Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(98, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Database Software";
            // 
            // cbxProviderType
            // 
            this.cbxProviderType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxProviderType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxProviderType.FormattingEnabled = true;
            this.cbxProviderType.Location = new System.Drawing.Point(107, 3);
            this.cbxProviderType.Name = "cbxProviderType";
            this.cbxProviderType.Size = new System.Drawing.Size(273, 21);
            this.cbxProviderType.TabIndex = 1;
            this.cbxProviderType.Validated += new System.EventHandler(this.connectionParameter_Validated);
            // 
            // btnNew
            // 
            this.btnNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNew.Location = new System.Drawing.Point(319, 106);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(61, 23);
            this.btnNew.TabIndex = 7;
            this.btnNew.Text = "New...";
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // DatabaseSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.txtDatabaseAddress);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbxDatabase);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cbxProviderType);
            this.MinimumSize = new System.Drawing.Size(0, 137);
            this.Name = "DatabaseSelector";
            this.Size = new System.Drawing.Size(397, 137);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtDatabaseAddress;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbxDatabase;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbxProviderType;
        private System.Windows.Forms.Button btnNew;
    }
}
