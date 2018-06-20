namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    public partial class frmProgress
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
            this.pgMain = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pgAction = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // pgMain
            // 
            this.pgMain.Location = new System.Drawing.Point(13, 115);
            this.pgMain.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pgMain.Name = "pgMain";
            this.pgMain.Size = new System.Drawing.Size(702, 35);
            this.pgMain.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 84);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Overall Progress...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(155, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Performing Actions...";
            // 
            // pgAction
            // 
            this.pgAction.Location = new System.Drawing.Point(13, 44);
            this.pgAction.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pgAction.Name = "pgAction";
            this.pgAction.Size = new System.Drawing.Size(702, 35);
            this.pgAction.TabIndex = 2;
            // 
            // frmProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 171);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pgAction);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pgMain);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmProgress";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Applying Actions...";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pgMain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pgAction;
    }
}