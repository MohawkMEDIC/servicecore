/*
 * Copyright 2010-2018 Mohawk College of Applied Arts and Technology
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you 
 * may not use this file except in compliance with the License. You may 
 * obtain a copy of the License at 
 * 
 * http://www.apache.org/licenses/LICENSE-2.0 
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the 
 * License for the specific language governing permissions and limitations under 
 * the License.
 * 
 * User: fyfej
 * Date: 1-9-2017
 */

using System;
using System.Drawing.Design;
using System.Security.Cryptography.X509Certificates;

namespace MARC.HI.EHRS.SVC.Messaging.HAPI.TransportProtocol
{
	/// <summary>
	/// An editor for the propertygrid that show certificates
	/// </summary>
	public class X509CertificateEditor : UITypeEditor
	{
		/// <summary>
		/// Edit value
		/// </summary>
		public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			X509Certificate2 currentCertificate = value as X509Certificate2;
			SllpTransport.SllpConfigurationObject configObj = context.Instance as SllpTransport.SllpConfigurationObject;

			X509Store store = null;

			if (context.PropertyDescriptor.Name == "ServerCertificate")
				store = new X509Store(configObj.ServerCertificateStore, configObj.ServerCertificateLocation);
			else
				store = new X509Store(configObj.TrustedCaCertificateStore, configObj.TrustedCaCertificateLocation);

			try
			{
				store.Open(OpenFlags.ReadOnly);
				// pick a certificate from the store
				var certs = X509Certificate2UI.SelectFromCollection(store.Certificates.Find(X509FindType.FindByApplicationPolicy, "1.3.6.1.5.5.7.3.1", true), "Select Certificate", "Select a certificate from the specified store", X509SelectionFlag.SingleSelection); // show certificate details dialog
				if (certs.Count > 0)
					return certs[0];
				else
					return value;
			}
			finally
			{
				store.Close();
			}
		}

		/// <summary>
		/// Modal editor
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}