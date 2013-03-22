namespace MARC.HI.EHRS.SVC.Messaging.Persistence.Data.Configuration.UI.Panels
{
    partial class pnlConfigureMessagePersistence
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
            this.label2 = new System.Windows.Forms.Label();
            this.dbSelector = new MARC.HI.EHRS.SVC.ConfigurationApplciation.DatabaseSelector();
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
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(408, 32);
            this.label2.TabIndex = 18;
            this.label2.Text = "Use the options below to select the database that will be used for storing messag" +
    "es that are processed by this service.";
            // 
            // dbSelector
            // 
            this.dbSelector.DatabaseConfigurator = null;
            this.dbSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.dbSelector.Location = new System.Drawing.Point(0, 52);
            this.dbSelector.MinimumSize = new System.Drawing.Size(0, 137);
            this.dbSelector.Name = "dbSelector";
            this.dbSelector.Size = new System.Drawing.Size(408, 137);
            this.dbSelector.TabIndex = 19;
            // 
            // pnlConfigureMessagePersistence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dbSelector);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "pnlConfigureMessagePersistence";
            this.Size = new System.Drawing.Size(408, 335);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private ConfigurationApplciation.DatabaseSelector dbSelector;
    }
}
