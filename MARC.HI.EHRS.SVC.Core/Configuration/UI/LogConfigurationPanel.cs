using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using MARC.HI.EHRS.SVC.Core.Logging;
using System.Diagnostics;
using MARC.HI.EHRS.SVC.Core.Issues;
using System.IO;
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    /// <summary>
    /// Log configuration panel
    /// </summary>
    public class LogConfigurationPanel : IAlwaysDeployedConfigurationPanel
    {
        #region IConfigurationPanel Members

        /// <summary>
        /// Initialize defaults
        /// </summary>
        public LogConfigurationPanel()
        {
            this.LogFile = "svc.log";
            this.LogSeverities = IssuePriorityType.Error | IssuePriorityType.Warning;
            this.EnableConfiguration = true;
            this.RollOver = true;
            this.m_panel.EnableLogging = true;
        }

        private ucLogging m_panel = new ucLogging();

        /// <summary>
        /// True when config need sync
        /// </summary>
        private bool m_needsSync = true;

        /// <summary>
        /// Log severities
        /// </summary>
        public IssuePriorityType? LogSeverities { get; set; }

        /// <summary>
        /// Log file
        /// </summary>
        public String LogFile { get; set; }

        /// <summary>
        /// Log file rollover
        /// </summary>
        public bool RollOver { get; set; }

        /// <summary>
        /// Get the name of the configuration panel
        /// </summary>
        public string Name
        {
            get { return "Service Core/Logging"; }
        }

        /// <summary>
        /// Enable the configuration?
        /// </summary>
        public bool EnableConfiguration
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the control panel
        /// </summary>
        public System.Windows.Forms.Control Panel
        {
            get { return this.m_panel; }
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public void Configure(System.Xml.XmlDocument configurationDom)
        {

            if (!this.m_panel.EnableLogging)
            {
                this.UnConfigure(configurationDom);
                return;
            }
            
            // Configure
            XmlElement diagnosticNode = configurationDom.SelectSingleNode("//*[local-name() = 'system.diagnostics']") as XmlElement;
            if (diagnosticNode == null)
                diagnosticNode = configurationDom.DocumentElement.AppendChild(configurationDom.CreateElement("system.diagnostics")) as XmlElement;

            // Shared listeners
            XmlElement sharedListenerNode = diagnosticNode.SelectSingleNode("./*[local-name() = 'sharedListeners']") as XmlElement;
            if (sharedListenerNode == null)
                sharedListenerNode = diagnosticNode.AppendChild(configurationDom.CreateElement("sharedListeners")) as XmlElement;

            // Configure the log
            string listenerName = this.RollOver ? typeof(RollOverTextWriterTraceListener).AssemblyQualifiedName : typeof(TextWriterTraceListener).AssemblyQualifiedName,
                shrListenName = this.RollOver ? "rollOver" : "textWriter";
            XmlElement listenerNode = sharedListenerNode.SelectSingleNode(String.Format("./*[local-name() = 'add'][@name = '{0}']", shrListenName)) as XmlElement;
            
            if (listenerNode == null)
                listenerNode = sharedListenerNode.AppendChild(configurationDom.CreateElement("add")) as XmlElement;
            listenerNode.RemoveAll();

            // Initialize data
            listenerNode.Attributes.Append(configurationDom.CreateAttribute("name")).Value = shrListenName;
            listenerNode.Attributes.Append(configurationDom.CreateAttribute("type")).Value = listenerName;
            if (listenerNode.Attributes["initializeData"] == null)
                listenerNode.Attributes.Append(configurationDom.CreateAttribute("initializeData"));
            listenerNode.Attributes["initializeData"].Value = this.LogFile;

            // Log level..
            XmlElement filterNode = listenerNode.SelectSingleNode("./*[local-name() = 'filter']") as XmlElement;
            if (this.LogSeverities.HasValue)
            {
                if (filterNode == null)
                    filterNode = listenerNode.AppendChild(configurationDom.CreateElement("filter")) as XmlElement;
                filterNode.RemoveAll();
                filterNode.Attributes.Append(configurationDom.CreateAttribute("type")).Value = "System.Diagnostics.EventTypeFilter";
                var initializeData = filterNode.Attributes.Append(configurationDom.CreateAttribute("initializeData"));
                for (int i = 1; i <= 4; i *= 2)
                    if (((int)this.LogSeverities.Value & i) == i)
                        initializeData.Value += string.Format("{0}, ", (IssuePriorityType)i);
                initializeData.Value = initializeData.Value.Substring(0, initializeData.Value.Length - 2);
            }

            // Trace settings
            XmlElement traceNode = diagnosticNode.SelectSingleNode("./*[local-name() = 'trace']") as XmlElement;;
            if (traceNode == null)
                traceNode = diagnosticNode.AppendChild(configurationDom.CreateElement("trace")) as XmlElement;
            traceNode.RemoveAll();
            traceNode.Attributes.Append(configurationDom.CreateAttribute("autoflush")).Value = true.ToString();
            XmlElement listenNode = traceNode.AppendChild(configurationDom.CreateElement("listeners")) as XmlElement,
                addNode = listenNode.AppendChild(configurationDom.CreateElement("add")) as XmlElement;
            addNode.Attributes.Append(configurationDom.CreateAttribute("name")).Value = shrListenName;
            this.m_needsSync = true;
 
        }

        /// <summary>
        /// Unconfigure the log writer
        /// </summary>
        public void UnConfigure(System.Xml.XmlDocument configurationDom)
        {
            string shrListenName = this.RollOver ? "rollOver" : "textWriter";
            XmlElement listenerNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'system.diagnostics']/*[local-name() = 'sharedListeners']/*[local-name() = 'add'][@name = '{0}']", shrListenName)) as XmlElement,
                traceNode = configurationDom.SelectSingleNode(String.Format("//*[local-name() = 'system.diagnostics']/*[local-name() = 'trace']/*[local-name() = 'listeners']/*[local-name() = 'add'][@name = '{0}']", shrListenName)) as XmlElement;

            if (listenerNode != null)
                listenerNode.ParentNode.RemoveChild(listenerNode);
            if (traceNode != null)
                traceNode.ParentNode.RemoveChild(traceNode);
            this.m_needsSync = true;

        }

        /// <summary>
        /// Determine if the logging is configured
        /// </summary>
        public bool IsConfigured(System.Xml.XmlDocument configurationDom)
        {

            if (!m_needsSync)
                return true;
            this.m_needsSync = false;
            XmlElement listenerNode = configurationDom.SelectSingleNode("//*[local-name() = 'system.diagnostics']/*[local-name() = 'sharedListeners']/*[local-name() = 'add'][@name = 'rollOver' or @name = 'textWriter']") as XmlElement,
                traceNode = configurationDom.SelectSingleNode("//*[local-name() = 'system.diagnostics']/*[local-name() = 'trace']/*[local-name() = 'listeners']/*[local-name() = 'add'][@name = 'rollOver' or @name = 'textWriter']") as XmlElement;

            if (listenerNode != null)
            {
                this.RollOver = listenerNode.Attributes["type"].Value == typeof(RollOverTextWriterTraceListener).AssemblyQualifiedName;

                if(listenerNode.Attributes["initializeData"] != null)
                    this.LogFile = listenerNode.Attributes["initializeData"].Value;

                XmlAttribute filterNode = listenerNode.SelectSingleNode("./*[local-name() = 'filter'][@type = 'System.Diagnostics.EventTypeFilter']/@initializeData") as XmlAttribute;
                if (filterNode != null)
                {
                    IssuePriorityType type = 0;
                    for (int i = 1; i <= 4; i *= 2)
                        if (filterNode.Value.Contains(((IssuePriorityType)i).ToString()))
                            type |= (IssuePriorityType)i;
                    this.LogSeverities = type;
                    this.m_panel.LogLevel = this.LogSeverities.Value;
                }
                else
                    this.LogSeverities = null;

                // Correct
                if (!Path.IsPathRooted(this.LogFile))
                    this.LogFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Path.GetFileName(this.LogFile));


                // Copy to form
                this.m_panel.LogFile = this.LogFile;
                this.m_panel.RollOver = this.RollOver;
                this.m_panel.EnableLogging = listenerNode != null && traceNode != null;
                
            }

            this.EnableConfiguration = true;
            return true;
        }

        /// <summary>
        /// Validate the configuration
        /// </summary>
        public bool Validate(System.Xml.XmlDocument configurationDom)
        {
            // copy from form
            this.LogFile = this.m_panel.LogFile;
            this.LogSeverities = this.m_panel.LogLevel;
            this.EnableConfiguration = this.m_panel.EnableLogging;
            this.RollOver = this.m_panel.RollOver;

            if (!Path.IsPathRooted(this.LogFile))
                this.LogFile = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Path.GetFileName(this.LogFile));
            // Validate
            return (!this.EnableConfiguration) ^ (!string.IsNullOrEmpty(this.LogFile) && Path.IsPathRooted(this.LogFile));
 
        }

        #endregion

        public override string ToString()
        {
            return this.Name;
        }
    }
}
