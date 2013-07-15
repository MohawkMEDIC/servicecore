using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.HI.EHRS.SVC.Messaging.FHIR.DataTypes;
using System.Xml.Serialization;
using System.Net;
using System.Diagnostics;
using System.ServiceModel.Web;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Resources
{

    /// <summary>
    /// Resource 
    /// </summary>
    [XmlType("Resource1", Namespace  = "http://hl7.org/fhir")]
    public class Resource : Shareable
    {
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        [XmlElement("type")]
        public virtual PrimitiveCode<String> Type
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the reference
        /// </summary>
        [XmlElement("reference")]
        public FhirUri Reference { get; set; }

        /// <summary>
        /// Gets or sets the display
        /// </summary>
        [XmlElement("display")]
        public FhirString Display { get; set; }

        /// <summary>
        /// Create resource refernece (friendly method)
        /// </summary>
        public static Resource CreateResourceReference(ResourceBase instance, Uri baseUri)
        {
            return new Resource()
            {
                Type = new PrimitiveCode<string>(instance.GetType().GetCustomAttribute<XmlRootAttribute>() != null ? instance.GetType().GetCustomAttribute<XmlRootAttribute>().ElementName : instance.GetType().Name),
                Reference = String.IsNullOrEmpty(instance.VersionId) ?
                    new Uri(baseUri.ToString() + String.Format("/{0}/@{1}", instance.GetType().Name, instance.Id)) :
                    new Uri(baseUri.ToString() + String.Format("/{0}/@{1}/history/@{2}", instance.GetType().Name, instance.Id, instance.VersionId))
            };
        }

        /// <summary>
        /// Write text
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            this.Reference.WriteText(w);

        }

        /// <summary>
        /// Fetch the resource described by this item
        /// </summary>
        public T FetchResource<T>(Uri baseUri) where T : Shareable
        {
            return this.FetchResource<T>(baseUri, null);
        }

        /// <summary>
        /// Fetch a resource from the specified uri with the specified credentials
        /// </summary>
        public T FetchResource<T>(Uri baseUri, ICredentials credentials) where T : Shareable
        {
            // Request uri
            Uri requestUri = null;

            if (!this.Reference.Value.IsAbsoluteUri)
                requestUri = new Uri(baseUri, this.Reference.Value.ToString());

            // Make request to URI
            Trace.TraceInformation("Fetching from {0}...", requestUri);
            var webReq = HttpWebRequest.Create(requestUri);
            webReq.Method = "GET";
            webReq.Credentials = credentials;

            // Fetch
            try
            {
                using (var response = webReq.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.Accepted)
                        throw new WebException(String.Format("Server responded with {0}", response.StatusCode));

                    // Get the response stream
                    XmlSerializer xsz = new XmlSerializer(typeof(T));
                    return xsz.Deserialize(response.GetResponseStream()) as T;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                throw;
            }

        }
    }

    /// <summary>
    /// Identifies a resource link
    /// </summary>
    public class Resource<T> : Resource
        where T : Shareable
    {
        /// <summary>
        /// Gets or sets the type
        /// </summary>
        [XmlElement("type")]
        public override PrimitiveCode<String> Type 
        {
            get
            {
                Object[] atts = typeof(T).GetCustomAttributes(typeof(XmlRootAttribute), true);
                if (atts.Length == 1)
                    return new PrimitiveCode<String>((atts[0] as XmlRootAttribute).ElementName);
                return new PrimitiveCode<string>(typeof(T).Name);
            }
            set
            {
                ;
            }        
        }


        /// <summary>
        /// Create a reasource reference to the specified resource
        /// </summary>
        public static Resource<T> CreateResourceReference<T>(T instance, Uri baseUri) where T : ResourceBase
        {
            return new Resource<T>()
            {
                Type = new PrimitiveCode<string>(instance.GetType().GetCustomAttribute<XmlRootAttribute>() != null ? instance.GetType().GetCustomAttribute<XmlRootAttribute>().ElementName : instance.GetType().Name),
                Reference = String.IsNullOrEmpty(instance.VersionId) ?
                    new Uri(baseUri.ToString() + String.Format("/{0}/@{1}", instance.GetType().Name, instance.Id)) :
                    new Uri(baseUri.ToString() + String.Format("/{0}/@{1}/history/@{2}", instance.GetType().Name, instance.Id, instance.VersionId))
            };
        }

    }
}
