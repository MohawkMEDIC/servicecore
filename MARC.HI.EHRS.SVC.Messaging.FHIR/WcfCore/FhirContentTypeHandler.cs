using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.WcfCore
{
    public class FhirContentTypeHandler : WebContentTypeMapper
    {

        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {

            if (contentType.StartsWith("application/fhir+xml", StringComparison.OrdinalIgnoreCase))
            {
                return WebContentFormat.Xml;
            }
            else
            {
                return WebContentFormat.Default;
            }

        }

    }

}
