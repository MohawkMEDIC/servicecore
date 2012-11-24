using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ServiceConfigurator
{
    public partial class frmProgress : Form
    {
        public frmProgress()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the status text
        /// </summary>
        public int Status { get { return progressBar1.Value; } set { progressBar1.Value = value; Application.DoEvents(); } }

        /// <summary>
        /// Gets or sets the status text
        /// </summary>
        public string StatusText { get { return label1.Text; } set { label1.Text = value; Application.DoEvents(); } }
    }
}
