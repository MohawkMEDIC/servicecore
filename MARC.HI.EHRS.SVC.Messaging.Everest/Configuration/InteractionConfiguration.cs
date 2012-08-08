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
        /// Response headers
        /// </summary>
        public XmlNodeList ResponseHeaders { get; set; }
    }
}
