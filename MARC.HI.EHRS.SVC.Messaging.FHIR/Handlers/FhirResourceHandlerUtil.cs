using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Syndication;
using System.ServiceModel.Web;
using MARC.Everest.Connectors;
using System.Xml.Serialization;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Handlers
{
    /// <summary>
    /// Message processing tool
    /// </summary>
    public static class FhirResourceHandlerUtil
    {

      
        // Message processors
        private static List<IFhirResourceHandler> s_messageProcessors = new List<IFhirResourceHandler>();

        /// <summary>
        /// FHIR message processing utility
        /// </summary>
        static FhirResourceHandlerUtil()
        {

            foreach (var t in typeof(FhirResourceHandlerUtil).Assembly.GetTypes().Where(o => o.GetInterface(typeof(IFhirResourceHandler).FullName) != null))
            {
                var ctor = t.GetConstructor(Type.EmptyTypes);
                if (ctor == null)
                    continue; // cannot construct
                var processor = ctor.Invoke(null) as IFhirResourceHandler;
                s_messageProcessors.Add(processor);
            }

        }

        /// <summary>
        /// Register resource handler
        /// </summary>
        public static void RegisterResourceHandler(IFhirResourceHandler handler)
        {
            s_messageProcessors.Add(handler);
        }

        /// <summary>
        /// Register resource handler
        /// </summary>
        public static void UnRegisterResourceHandler(IFhirResourceHandler handler)
        {
            s_messageProcessors.Remove(handler);
        }
      
        /// <summary>
        /// Get the message processor type based on resource name
        /// </summary>
        public static IFhirResourceHandler GetResourceHandler(String resourceName)
        {
            return s_messageProcessors.Find(o => o.ResourceName.ToLower() == resourceName.ToLower());
        }

        /// <summary>
        /// Get all resource handlers
        /// </summary>
        public static IEnumerable<IFhirResourceHandler> ResourceHandlers
        {
            get
            {
                return s_messageProcessors;
            }
        }
    }
}
