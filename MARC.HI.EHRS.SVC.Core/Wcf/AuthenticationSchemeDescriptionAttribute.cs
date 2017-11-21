using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MARC.HI.EHRS.SVC.Core.Wcf
{

    /// <summary>
    /// Authorization scheme applied
    /// </summary>
    public enum AuthenticationScheme
    {
        /// <summary>
        /// No authorization scheme (passthru)
        /// </summary>
        None,
        /// <summary>
        /// Basic auth scheme
        /// </summary>
        Basic,
        /// <summary>
        /// OAuth 1.0 scheme
        /// </summary>
        OAuth,
        /// <summary>
        /// OAuth2 schema
        /// </summary>
        OAuth2,
        /// <summary>
        /// Custom scheme
        /// </summary>
        Custom
    }

    /// <summary>
    /// Represents an authorization manager type attribute for identifying the scheme
    /// </summary>
    public class AuthenticationSchemeDescriptionAttribute : System.Attribute
    {

        /// <summary>
        /// Authorization scheme
        /// </summary>
        /// <param name="scheme"></param>
        public AuthenticationSchemeDescriptionAttribute(AuthenticationScheme scheme)
        {
            this.Scheme = scheme;
        }

        /// <summary>
        /// Gets or sets the auth scheme
        /// </summary>
        public AuthenticationScheme Scheme { get; set; }

    }
}
