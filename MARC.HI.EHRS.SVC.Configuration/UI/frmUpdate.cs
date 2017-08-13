using MARC.HI.EHRS.SVC.Configuration.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MARC.HI.EHRS.SVC.Configuration.UI
{
    public partial class frmUpdate : Form
    {
        public frmUpdate()
        {
            InitializeComponent();
            this.InitializeUpdates();
        }

        /// <summary>
        /// Initialization of updates
        /// </summary>
        private void InitializeUpdates()
        {
            foreach (var itm in DatabaseConfiguratorRegistrar.Features)
            {
                if (itm.ConnectionString == null ||
                    itm.ConnectionString.Provider == null)
                    continue;
                var group = new ListViewGroup(itm.Name);
                lvUpdates.Groups.Add(group);

                foreach (var upd in itm.ConnectionString.Provider.PlanUpdate(itm.Updates, itm.ConnectionString))
                {
                    var item = new ListViewItem(group) { Text = upd.Name, Tag = upd };
                    item.SubItems.Add(upd.Description);
                    item.Checked = true;
                    lvUpdates.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Deploy the updates
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {
            var progress = new frmProgress();
            EventHandler<ProgressChangedEventArgs> progHandler = (o, ev) =>
            {
                if (ev.UserState == null)
                    progress.OverallStatus = ev.ProgressPercentage;
                else
                {
                    progress.ActionStatus = ev.ProgressPercentage;
                    progress.ActionStatusText = ev.UserState.ToString();
                }
                Application.DoEvents();
            };

            progress.Show();

            int i = 0;
            foreach (IDataboundFeature feature in DatabaseConfiguratorRegistrar.Features)
            {
                try
                {
                    feature.ConnectionString.Provider.ProgressChanged += progHandler;

                    progress.OverallStatus = (int)((++i / (float)DatabaseConfiguratorRegistrar.Features.Count) * 100);
                    progress.OverallStatusText = String.Format("Applying updates for {0}...", feature.ToString());
                    //pnl.EnableConfiguration = true;

                    // Apply the updates
                    if (feature is IReportProgressChanged)
                        (feature as IReportProgressChanged).ProgressChanged += progHandler;

                    try
                    {
                        feature.ConnectionString.Provider.DeployUpdate(lvUpdates.CheckedItems.OfType<ListViewItem>().Select(o => o.Tag as IDataUpdate), feature.ConnectionString);
                        feature.AfterUpdate();
                    }
                    finally
                    {
                        if (feature is IReportProgressChanged)
                            (feature as IReportProgressChanged).ProgressChanged -= progHandler;
                    }
                }
                catch (Exception ex)
                {
#if DEBUG
                    MessageBox.Show(ex.ToString(), "Error Configuring Service");

#else
                        MessageBox.Show(ex.Message, "Error Configuring Service");
#endif
                }
                finally
                {
                    feature.ConnectionString.Provider.ProgressChanged -= progHandler;
                    progress.Close();
                }

            }

            this.Close();
        }
    }
}