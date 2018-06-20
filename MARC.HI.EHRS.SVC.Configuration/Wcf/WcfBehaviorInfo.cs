using System;
using System.Xml;

namespace MARC.HI.EHRS.SVC.Configuration.Wcf
{
    /// <summary>
    /// Represents service behavior
    /// </summary>
    public class WcfBehaviorInfo
    {


        // Name
        private string m_name = String.Empty;

        /// <summary>
        /// Default ctor
        /// </summary>
        public WcfBehaviorInfo()
        {
        }

        /// <summary>
        /// Behavior info ctor
        /// </summary>
        public WcfBehaviorInfo(XmlNode behaviorXml)
        {
            this.m_name = behaviorXml.Attributes["name"]?.Value;
            this.Debug = behaviorXml.SelectSingleNode("./serviceDebug") != null;

            // Authorization manager?
            XmlNode serviceCert = behaviorXml.SelectSingleNode("./serviceCredentials/serviceCertificate"),
                clientCert = behaviorXml.SelectSingleNode("./serviceCredentials/clientCertificate"),
                serviceAuthorization = behaviorXml.SelectSingleNode("./serviceAuthorization"),
                userValidator = behaviorXml.SelectSingleNode("./serviceCredentials/userNameAuthentication");

            if (serviceCert != null)
                this.ServiceCertificate = new WcfCertificateCredentialInfo(serviceCert);
            if (clientCert != null)
                this.ClientCertificate = new WcfCertificateValidationInfo(clientCert);
            if (userValidator != null)
                this.UserNameCredentialValidator = Type.GetType(userValidator.Attributes["customUserNamePasswordValidatorType"]?.Value);
            if(serviceAuthorization != null)
            {
                this.ServiceAuthorizationManager = Type.GetType(serviceAuthorization.Attributes["serviceAuthorizationManagerType"]?.Value);
                var policyXml = serviceAuthorization.SelectSingleNode("./authorizationPolicies/add");
                if (policyXml != null)
                    this.AuthorizationPolicy = Type.GetType(policyXml.Attributes["policyType"]?.Value);
            }
        }

        /// <summary>
        /// Represent this object as configuration xml
        /// </summary>
        public XmlElement ToXml(XmlElement parent)
        {
            var behavior = parent.SelectSingleNode($"./behavior[@name='{this.m_name}']") as XmlElement;
            if (behavior == null) {
                behavior = parent.OwnerDocument.CreateElement("behavior");
                behavior.SetAttribute("name", this.m_name ?? Guid.NewGuid().ToConfigName());
                parent.AppendChild(behavior);
            }

            if (this.Debug)
                behavior.UpdateOrCreateChildElement("serviceDebug", new { includeExceptionDetailInFaults = true });

            // Credentials
            var credentialXml = behavior.GetOrCreateElement("serviceCredentials");
            if (this.ServiceCertificate != null)
                this.ServiceCertificate.ToXml(credentialXml);
            if (this.ClientCertificate != null)
                this.ClientCertificate.ToXml(credentialXml.GetOrCreateElement("clientCertificate"));
            if(this.UserNameCredentialValidator != null)
                credentialXml.UpdateOrCreateChildElement("userNameAuthentication", new { userNamePasswordValidationMode = "Custom", customUserNamePasswordValidatorType = this.UserNameCredentialValidator.AssemblyQualifiedName });

            // Service authorization
            if(this.AuthorizationPolicy != null)
            {
                var authPolicies = behavior.UpdateOrCreateChildElement("serviceAuthorization", new { principalPermissionMode = "Custom", serviceAuthorizationManagerType = this.ServiceAuthorizationManager?.AssemblyQualifiedName }).GetOrCreateElement("authorizationPolicies");
                authPolicies.UpdateOrCreateChildElement("add", new { policyType = this.AuthorizationPolicy.AssemblyQualifiedName });
            }

            return behavior;
        }

        /// <summary>
        /// Debug behavior attribute
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Represents a server certificate
        /// </summary>
        public WcfCertificateCredentialInfo ServiceCertificate { get; set; }

        /// <summary>
        /// Gets or sets the client credential info
        /// </summary>
        public WcfCertificateValidationInfo ClientCertificate { get; set; }

        /// <summary>
        /// Gets or sets the service authorization manager
        /// </summary>
        public Type ServiceAuthorizationManager { get; set; }

        /// <summary>
        /// Gets or sets the authorization policy
        /// </summary>
        public Type AuthorizationPolicy { get; set; }

        /// <summary>
        /// Gets or sets the credential validator type
        /// </summary>
        public Type UserNameCredentialValidator { get; set; }

    }
}