using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration.Wcf
{
    /// <summary>
    /// WCF Extension methods
    /// </summary>
    public static class WcfExtensionMethods
    {

        /// <summary>
        /// Get the specified WCF service
        /// </summary>
        public static WcfServiceInfo GetWcfService(this XmlDocument me, String serviceName)
        {
            var serviceXml = me.SelectSingleNode($"/configuration/system.serviceModel/services/service[@name='{serviceName}']") as XmlElement;
            if (serviceXml == null)
                return null;
            else
                return new WcfServiceInfo(serviceXml);
        }

        /// <summary>
        /// Adds or updates the specified wcf service
        /// </summary>
        public static XmlElement AddOrUpdateWcfService(this XmlDocument me, WcfServiceInfo service)
        {
            var serviceXml = me.GetOrCreateElement("/configuration/system.serviceModel/services");
            return service.ToXml(serviceXml);
        }

    }
}
