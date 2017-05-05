using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    public partial class ucLogging : UserControl
    {

        public bool EnableLogging
        {
            get
            {
                return this.chkEnableLogging.Checked;
            }
            set
            {
                this.chkEnableLogging.Checked = value;
            }
        }

        public string LogFile
        {
            get
            {
                return this.txtFileName.Text;
            }
            set
            {
                this.txtFileName.Text = value;
            }
        }

        public Issues.IssuePriorityType LogLevel
        {
            get
            {
                Issues.IssuePriorityType retVal = 0;
                if (this.chkErrors.Checked)
                    retVal |= Issues.IssuePriorityType.Error;
                if (this.chkWarning.Checked)
                    retVal |= Issues.IssuePriorityType.Warning;
                if (this.chkInfo.Checked)
                    retVal |= Issues.IssuePriorityType.Informational;
                return retVal;
            }
            set
            {
                chkInfo.Checked = (value & Issues.IssuePriorityType.Informational) != 0;
                chkWarning.Checked = (value & Issues.IssuePriorityType.Warning) != 0;
                chkErrors.Checked = (value & Issues.IssuePriorityType.Error) != 0;
            }
        }

        public bool RollOver
        {
            get { return this.chkRollover.Checked; }
            set { this.chkRollover.Checked = value; }
        }

        public ucLogging()
        {
            InitializeComponent();

            chkTraceSources.Items.Clear();
            chkTraceSources.Items.AddRange(
                AppDomain.CurrentDomain.GetAssemblies().SelectMany(a=>a.ExportedTypes).Select(t=>t.GetCustomAttribute<TraceSourceAttribute>()).Where(o=>o != null).Distinct().ToArray()
            );
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            SaveFileDialog dlgOpen = new SaveFileDialog()
            {
                CheckFileExists = false,
                AddExtension = true,
                Filter = "Log Files (*.log)|*.log",
                InitialDirectory = Path.GetDirectoryName(!String.IsNullOrEmpty(this.LogFile) ? this.LogFile : Assembly.GetEntryAssembly().Location),
                Title = "Log File Location"
            };
            if (dlgOpen.ShowDialog() == DialogResult.OK)
                this.LogFile = dlgOpen.FileName;


        }


        private FileSystemWatcher m_fsWatcher;

        /// <summary>
        /// Text has changed
        /// </summary>
        private void txtFileName_TextChanged(object sender, EventArgs e)
        {

            if (m_fsWatcher != null)
            {
                m_fsWatcher.Changed -= this.m_fsWatcher_Changed;
                m_fsWatcher.Created -= this.m_fsWatcher_Changed;
                m_fsWatcher.Deleted -= this.m_fsWatcher_Changed;
                m_fsWatcher.Dispose();
            }

            string filter = !this.RollOver ? Path.GetFileName(this.LogFile) : String.Format("{0}_????????{1}", Path.GetFileNameWithoutExtension(this.LogFile), Path.GetExtension(this.LogFile));
            m_fsWatcher = new FileSystemWatcher(Path.GetDirectoryName(this.LogFile), filter)
            {
                EnableRaisingEvents = true,
            };
            m_fsWatcher.Changed += new FileSystemEventHandler(m_fsWatcher_Changed);
            m_fsWatcher.Created += new FileSystemEventHandler(m_fsWatcher_Changed);
            m_fsWatcher.Deleted += new FileSystemEventHandler(m_fsWatcher_Changed);

            m_fsWatcher.BeginInit();
            m_fsWatcher.EndInit();

            if (!m_fileScan.IsBusy)
                m_fileScan.RunWorkerAsync(this.LogFile);
        }

        /// <summary>
        /// File has changed
        /// </summary>
        void m_fsWatcher_Changed(object sender, FileSystemEventArgs e)
        {

            if (!m_fileScan.IsBusy)
                m_fileScan.RunWorkerAsync(this.LogFile);
        }

        private void m_fileScan_DoWork(object sender, DoWorkEventArgs e)
        {
            long fSize = 0;
            try
            {
                string logFile = e.Argument as string;
                // Get the file size
                if (this.RollOver)
                    foreach (var fil in Directory.GetFiles(Path.GetDirectoryName(logFile), String.Format("{0}_????????{1}", Path.GetFileNameWithoutExtension(logFile), Path.GetExtension(logFile))))
                        fSize += new FileInfo(fil).Length;
                else if (!String.IsNullOrEmpty(logFile))
                    fSize = new FileInfo(logFile).Length;

            }
            catch
            {
            }
            e.Result = fSize;
        }

        private long m_logSize = 0;

        private void m_fileScan_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            m_logSize = (long)e.Result;

        }

        private void fSizeRefresh_Tick(object sender, EventArgs e)
        {
            lblFileSize.Text = String.Format("{0:###,###} KB", ((long)m_logSize) / 1024);
        }

        private void chkEnableLogging_CheckedChanged(object sender, EventArgs e)
        {
            pnlFile.Enabled = chkEnableLogging.Checked;
        }

    }
}
