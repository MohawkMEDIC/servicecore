using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Messaging.Everest.Configuration
{
    /// <summary>
    /// Interaction configuration
    /// </summary>
    public class InteractionConfiguration
    {

        /// <summary>
        /// Gets or sets the identifier for the interaction
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// True if the interaction results in disclosure
        /// </summary>
        /// <remarks>Alters the manner in which the interaction message is persisted</remarks>
        public bool Disclosure { get; set; }
        /// <summary>
        /// Response headers
        /// </summary>
        public XmlNodeList ResponseHeaders { get; set; }
    }
}
