using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Core.Configuration;
using System.Windows.Forms;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Configuration.UI
{
    /// <summary>
    /// Represents the Everest messaging configuration panel
    /// </summary>
    public class EverestConfigurationPanel : IConfigurationPanel
    {
        #region IConfigurationPanel Members

        /// <summary>
        /// Message persistence
        /// </summary>
        public string Name
        {
            get { return "Message Processing"; }
        }

        /// <summary>
        /// Enable configuration
        /// </summary>
        public bool EnableConfiguration { get; set; }

        /// <summary>
        /// Gets the configuration panel
        /// </summary>
        public System.Windows.Forms.Control Panel
        {
            get { return new Label() { Text = "No Configuration Yet Supported", TextAlign = System.Drawing.ContentAlignment.MiddleCenter, AutoSize = false }; }
        }

        /// <summary>
        /// Configure the option
        /// </summary>
        public void Configure(System.Xml.XmlDocument configurationDom)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Unconfigure the option
        /// </summary>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Determine if the option is configured
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {
            return false;
        }

        /// <summary>
        /// Validate the configuration options
        /// </summary>
        public bool Validate(System.Xml.XmlDocument configurationDom)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
