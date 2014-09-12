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
        /// Gets or sets the reference
        /// </summary>
        [XmlElement("reference")]
        public FhirString Reference { get; set; }

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
                Display = instance.ToString(),
                //Type = new PrimitiveCode<string>(instance.GetType().GetCustomAttribute<XmlRootAttribute>() != null ? instance.GetType().GetCustomAttribute<XmlRootAttribute>().ElementName : instance.GetType().Name),
                Reference = String.IsNullOrEmpty(instance.VersionId) ?
                    baseUri.ToString() + String.Format("/{0}/{1}", instance.GetType().Name, instance.Id) :
                    baseUri.ToString() + String.Format("/{0}/{1}/_history/{2}", instance.GetType().Name, instance.Id, instance.VersionId)
            };
        }

        /// <summary>
        /// Create resource refernece for local
        /// </summary>
        public static Resource CreateLocalResourceReference(ResourceBase instance)
        {
            IdRef idRef = instance.MakeIdRef();
            return new Resource() {
                Reference = idRef.Value,
                Display = instance.ToString()
            };
        }

        /// <summary>
        /// Write text
        /// </summary>
        internal override void WriteText(System.Xml.XmlWriter w)
        {
            w.WriteStartElement("a");
            w.WriteAttributeString("href", this.Reference.Value);
            w.WriteString((this.Display ?? this.Reference).Value);
            w.WriteEndElement();// a
        }

        /// <summary>
        /// Fetch the resource described by this item
        /// </summary>
        public ResourceBase FetchResource(Uri baseUri, Type resourceType)
        {
            return this.FetchResource(baseUri, null, null, resourceType);
        }

        /// <summary>
        /// Fetch a resource from the specified uri with the specified credentials
        /// </summary>
        public ResourceBase FetchResource(Uri baseUri, ICredentials credentials, ResourceBase context, Type resourceType)
        {
            // Request uri
            Uri requestUri = null;

            // Contained?
            if (this.Reference.Value.StartsWith("#") && context.Contained != null)
            {
                var res = context.Contained.Find(o => o.Item.Id == this.Reference.Value.ToString().Substring(1));
                if (res != null)
                    return res.Item;
            }
            else if (Uri.TryCreate(this.Reference.Value, UriKind.RelativeOrAbsolute, out requestUri))
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

                    XmlSerializer xsz = new XmlSerializer(resourceType);
                    return xsz.Deserialize(response.GetResponseStream()) as ResourceBase;
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                throw;
            }

        }

        /// <summary>
        /// Create a reasource reference to the specified resource
        /// </summary>
        public static Resource<T> CreateResourceReference<T>(T instance, Uri baseUri) where T : ResourceBase
        {
            return new Resource<T>()
            {
                Display = instance.ToString(),
                Reference = String.IsNullOrEmpty(instance.VersionId) ?
                    baseUri.ToString() + String.Format("/{0}/{1}", instance.GetType().Name, instance.Id) :
                    baseUri.ToString() + String.Format("/{0}/{1}/_history/{2}", instance.GetType().Name, instance.Id, instance.VersionId)
            };
        }

        /// <summary>
        /// Create a reasource reference to the specified resource
        /// </summary>
        public static Resource<T> CreateLocalResourceReference<T>(T instance) where T : ResourceBase
        {
            IdRef idRef = instance.MakeIdRef();
            return new Resource<T>()
            {
                Display = instance.ToString(),
                Reference = idRef.Value
            };
        }
    }

    /// <summary>
    /// Identifies a resource link
    /// </summary>
    public class Resource<T> : Resource
        where T : ResourceBase
    {

        /// <summary>
        /// Fetch the resource described by this item
        /// </summary>
        public T FetchResource(Uri baseUri, ResourceBase context)
        {
            return this.FetchResource(baseUri, null, context);
        }

        /// <summary>
        /// Fetch a resource from the specified uri with the specified credentials
        /// </summary>
        public T FetchResource(Uri baseUri, ICredentials credentials, ResourceBase context) 
        {
            return (T)base.FetchResource(baseUri, credentials, context, typeof(T));
        }

        /// <summary>
        /// Gets or sets the type
        /// </summary>
        [XmlIgnore]
        public PrimitiveCode<String> Type 
        {
            get
            {
                Object[] atts = typeof(T).GetCustomAttributes(typeof(XmlRootAttribute), true);
                if (atts.Length == 1)
                    return new PrimitiveCode<String>((atts[0] as XmlRootAttribute).ElementName);
                return new PrimitiveCode<string>(typeof(T).Name);
            }
        }

    }
}
