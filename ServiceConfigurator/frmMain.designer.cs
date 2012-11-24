namespace ServiceConfigurator
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
            this.chkEnableFeature = new System.Windows.Forms.CheckBox();
            this.pnlConfigure = new System.Windows.Forms.Panel();
            this.tsMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // trvOptions
            // 
            this.trvOptions.Dock = System.Windows.Forms.DockStyle.Left;
            this.trvOptions.ImageIndex = 1;
            this.trvOptions.ImageList = this.imlMain;
            this.trvOptions.Location = new System.Drawing.Point(0, 25);
            this.trvOptions.Name = "trvOptions";
            this.trvOptions.SelectedImageIndex = 0;
            this.trvOptions.Size = new System.Drawing.Size(196, 444);
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
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(585, 25);
            this.tsMain.TabIndex = 1;
            this.tsMain.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(125, 22);
            this.toolStripButton1.Text = "Configure Options";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(138, 22);
            this.toolStripButton2.Text = "Unconfigure Options";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // chkEnableFeature
            // 
            this.chkEnableFeature.AutoSize = true;
            this.chkEnableFeature.Location = new System.Drawing.Point(202, 28);
            this.chkEnableFeature.Name = "chkEnableFeature";
            this.chkEnableFeature.Size = new System.Drawing.Size(59, 17);
            this.chkEnableFeature.TabIndex = 2;
            this.chkEnableFeature.Text = "Enable";
            this.chkEnableFeature.UseVisualStyleBackColor = true;
            this.chkEnableFeature.CheckedChanged += new System.EventHandler(this.chkEnableFeature_CheckedChanged);
            // 
            // pnlConfigure
            // 
            this.pnlConfigure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlConfigure.Enabled = false;
            this.pnlConfigure.Location = new System.Drawing.Point(202, 51);
            this.pnlConfigure.Name = "pnlConfigure";
            this.pnlConfigure.Size = new System.Drawing.Size(383, 418);
            this.pnlConfigure.TabIndex = 3;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(585, 469);
            this.Controls.Add(this.pnlConfigure);
            this.Controls.Add(this.chkEnableFeature);
            this.Controls.Add(this.trvOptions);
            this.Controls.Add(this.tsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmMain";
            this.Text = "MARC-HI Service Configuration";
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
        private System.Windows.Forms.CheckBox chkEnableFeature;
        private System.Windows.Forms.Panel pnlConfigure;
    }
}

