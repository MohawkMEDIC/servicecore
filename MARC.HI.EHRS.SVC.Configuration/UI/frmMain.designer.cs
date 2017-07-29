namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    public partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.trvOptions = new System.Windows.Forms.TreeView();
            this.imlMain = new System.Windows.Forms.ImageList(this.components);
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.pnlConfigure = new System.Windows.Forms.Panel();
            this.lblConfigured = new System.Windows.Forms.Label();
            this.chkEnable = new System.Windows.Forms.CheckBox();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // trvOptions
            // 
            this.trvOptions.Dock = System.Windows.Forms.DockStyle.Left;
            this.trvOptions.ImageIndex = 1;
            this.trvOptions.ImageList = this.imlMain;
            this.trvOptions.Location = new System.Drawing.Point(0, 31);
            this.trvOptions.Name = "trvOptions";
            this.trvOptions.SelectedImageIndex = 0;
            this.trvOptions.Size = new System.Drawing.Size(196, 481);
            this.trvOptions.TabIndex = 0;
            this.trvOptions.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvOptions_AfterSelect);
            // 
            // imlMain
            // 
            this.imlMain.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imlMain.ImageStream")));
            this.imlMain.TransparentColor = System.Drawing.Color.Transparent;
            this.imlMain.Images.SetKeyName(0, "button_ok.png");
            this.imlMain.Images.SetKeyName(1, "edit_delete_mail.png");
            this.imlMain.Images.SetKeyName(2, "edit_clear_history.png");
            this.imlMain.Images.SetKeyName(3, "configure.png");
            // 
            // tsMain
            // 
            this.tsMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(742, 31);
            this.tsMain.TabIndex = 1;
            this.tsMain.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(133, 28);
            this.toolStripButton1.Text = "Configure Options";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(146, 28);
            this.toolStripButton2.Text = "Unconfigure Options";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // pnlConfigure
            // 
            this.pnlConfigure.AutoScroll = true;
            this.pnlConfigure.AutoSize = true;
            this.pnlConfigure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlConfigure.Enabled = false;
            this.pnlConfigure.Location = new System.Drawing.Point(196, 67);
            this.pnlConfigure.Name = "pnlConfigure";
            this.pnlConfigure.Size = new System.Drawing.Size(546, 445);
            this.pnlConfigure.TabIndex = 3;
            // 
            // lblConfigured
            // 
            this.lblConfigured.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblConfigured.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblConfigured.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfigured.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.lblConfigured.Location = new System.Drawing.Point(196, 31);
            this.lblConfigured.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblConfigured.Name = "lblConfigured";
            this.lblConfigured.Size = new System.Drawing.Size(546, 19);
            this.lblConfigured.TabIndex = 0;
            this.lblConfigured.Text = "This Feature Is Already Configured";
            this.lblConfigured.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblConfigured.Visible = false;
            // 
            // chkEnable
            // 
            this.chkEnable.AutoSize = true;
            this.chkEnable.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkEnable.Location = new System.Drawing.Point(196, 50);
            this.chkEnable.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkEnable.Name = "chkEnable";
            this.chkEnable.Size = new System.Drawing.Size(546, 17);
            this.chkEnable.TabIndex = 4;
            this.chkEnable.Text = "Enable Feature";
            this.chkEnable.UseVisualStyleBackColor = true;
            this.chkEnable.Visible = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 512);
            this.Controls.Add(this.pnlConfigure);
            this.Controls.Add(this.chkEnable);
            this.Controls.Add(this.lblConfigured);
            this.Controls.Add(this.trvOptions);
            this.Controls.Add(this.tsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "Service Configuration";
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView trvOptions;
        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ImageList imlMain;
        private System.Windows.Forms.Panel pnlConfigure;
        private System.Windows.Forms.Label lblConfigured;
        private System.Windows.Forms.CheckBox chkEnable;
    }
}

