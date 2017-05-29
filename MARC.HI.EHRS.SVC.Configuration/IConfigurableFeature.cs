using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration
{
    /// <summary>
    /// Configuration panel
    /// </summary>
    public interface IConfigurableFeature
    {

        /// <summary>
        /// Gets or sets whether the feature is or should be enabled
        /// </summary>
        bool EnableConfiguration { get; set; }

        /// <summary>
        /// Gets the name of the panel
        /// </summary>
        String Name { get; }

        /// <summary>
        /// True if configuration should be enabled
        /// </summary>
        bool AlwaysConfigure { get; }

        /// <summary>
        /// Panel which controls the object
        /// </summary>
        Control Panel { get; }

        /// <summary>
        /// Configure the object
        /// </summary>
        void Configure(XmlDocument configurationDom);

        /// <summary>
        /// Unconfigure the object
        /// </summary>
        void UnConfigure(XmlDocument configurationDom);

        /// <summary>
        /// True if the object is configured
        /// </summary>
        bool IsConfigured(XmlDocument configurationDom);


        /// <summary>
        /// True if the object is deemed suitable for persisting
        /// </summary>
        bool Validate(XmlDocument configurationDom);

        /// <summary>
        /// Perform a configuration for the user in "easy" configuration mode (no inputs)
        /// </summary>
        void EasyConfigure(XmlDocument configFile);
    }
}
