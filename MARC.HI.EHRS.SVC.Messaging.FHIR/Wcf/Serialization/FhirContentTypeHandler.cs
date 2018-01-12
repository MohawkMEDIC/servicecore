using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Wcf.Serialization
{
    /// <summary>
    /// Content-type handler
    /// </summary>
    public class FhirContentTypeHandler : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            if (contentType.StartsWith("application/fhir+xml") ||
                contentType.StartsWith("text/xml") ||
                contentType.StartsWith("application/xml"))
                return WebContentFormat.Xml;
            else
                return WebContentFormat.Raw;
        }
    }
}
