namespace MARC.HI.EHRS.SVC.QM.Persistence.Data.Configuration.UI.Panels
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
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numMaxAge = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dbSelector = new MARC.HI.EHRS.SVC.ConfigurationApplciation.DatabaseSelector();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxAge)).BeginInit();
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
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(408, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Connection";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label3.Dock = System.Windows.Forms.DockStyle.Top;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(0, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(408, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Query Culling";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(17, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Persist queries for ";
            // 
            // numMaxAge
            // 
            this.numMaxAge.Location = new System.Drawing.Point(116, 13);
            this.numMaxAge.Maximum = new decimal(new int[] {
            365,
            0,
            0,
            0});
            this.numMaxAge.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMaxAge.Name = "numMaxAge";
            this.numMaxAge.Size = new System.Drawing.Size(82, 20);
            this.numMaxAge.TabIndex = 4;
            this.numMaxAge.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(204, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "days";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(408, 32);
            this.label2.TabIndex = 30;
            this.label2.Text = "Use the options below to select the database that will be used for storing querie" +
    "s that are processed by this service.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.numMaxAge);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 209);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(408, 40);
            this.panel1.TabIndex = 31;
            // 
            // dbSelector
            // 
            this.dbSelector.DatabaseConfigurator = null;
            this.dbSelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.dbSelector.Location = new System.Drawing.Point(0, 52);
            this.dbSelector.MinimumSize = new System.Drawing.Size(0, 137);
            this.dbSelector.Name = "dbSelector";
            this.dbSelector.Size = new System.Drawing.Size(408, 137);
            this.dbSelector.TabIndex = 1;
            // 
            // pnlConfigureMessagePersistence
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dbSelector);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "pnlConfigureMessagePersistence";
            this.Size = new System.Drawing.Size(408, 335);
            ((System.ComponentModel.ISupportInitialize)(this.numMaxAge)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numMaxAge;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        private ConfigurationApplciation.DatabaseSelector dbSelector;
    }
}
