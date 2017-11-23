﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MARC.HI.EHRS.SVC.Core.Services;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Backbone;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Resources;
using MARC.HI.EHRS.SVC.Messaging.FHIR.Util;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR.Handlers
{
    /// <summary>
    /// Represents the default StructureDefinition handler
    /// </summary>
    public class StructureDefinitionHandler : IFhirResourceHandler
    {
        /// <summary>
        /// Gets the resource name
        /// </summary>
        public string ResourceName
        {
            get
            {
                return "StructureDefinition";
            }
        }

        /// <summary>
        /// Create the specified definition
        /// </summary>
        public FhirOperationResult Create(DomainResourceBase target, Core.Services.TransactionMode mode)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Delete
        /// </summary>
        public FhirOperationResult Delete(string id, Core.Services.TransactionMode mode)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Get the resource definition
        /// </summary>
        public ResourceDefinition GetResourceDefinition()
        {
            return new ResourceDefinition()
            {
                ConditionalCreate = false,
                ConditionalDelete = ConditionalDeleteStatus.NotSupported,
                ConditionalUpdate = false,
                Interaction = new List<InteractionDefinition>()
                {
                    new InteractionDefinition()
                    {
                        Type = TypeRestfulInteraction.Read
                    },
                    new InteractionDefinition()
                    {
                        Type = TypeRestfulInteraction.VersionRead
                    },
                    new InteractionDefinition()
                    {
                        Type = TypeRestfulInteraction.Search
                    }
                },
                Type = "StructureDefinition",
                ReadHistory = true,
                UpdateCreate = false,
                Versioning = ResourceVersionPolicy.Versioned
            };
        }

        /// <summary>
        /// Get structure definition
        /// </summary>
        public StructureDefinition GetStructureDefinition()
        {
            return typeof(StructureDefinition).GetStructureDefinition(false);
        }

        /// <summary>
        /// Query for the specified search structure definition
        /// </summary>
        public FhirQueryResult Query(NameValueCollection parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Read the specified structure definition
        /// </summary>
        public FhirOperationResult Read(string id, string versionId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Update
        /// </summary>
        public FhirOperationResult Update(string id, DomainResourceBase target, Core.Services.TransactionMode mode)
        {
            throw new NotSupportedException();
        }
    }
}
