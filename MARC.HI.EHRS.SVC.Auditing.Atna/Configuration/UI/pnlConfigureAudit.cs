using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using AtnaApi.Transport;

namespace MARC.HI.EHRS.SVC.Auditing.Atna.Configuration.UI
{
    public partial class pnlConfigureAudit : UserControl
    {
        
        
        /// <summary>
        /// Get IP Address
        /// </summary>
        private IPAddress GetIPAddress()
        {
            byte[] address = new byte[4];
            string[] part = txtIp.Text.Split('.');
            for (int i = 0; i < part.Length; i++)
                address[i] = Byte.Parse(part[i]);
            return new IPAddress(address);
        }
        
        /// <summary>
        /// Get the publisher
        /// </summary>
        public Type Publisher { 
            get {
                if (this.cbxTransport.SelectedItem == null)
                    return null;
                return (this.cbxTransport.SelectedItem as TypeDescription).Type; } 

            set {
                foreach(TypeDescription td in cbxTransport.Items)
                    if(td.Type == value)
                        cbxTransport.SelectedItem = td;
            }
        }


        /// <summary>
        /// GEt the endpoint
        /// </summary>
        public IPEndPoint Endpoint
        {
            get
            {
                return new IPEndPoint(GetIPAddress(), (int)txtPort.Value);
            }
            set
            {
                txtIp.Text = value.Address.ToString();
                txtPort.Value = value.Port;
            }
        }
                
        /// <summary>
        /// Type description
        /// </summary>
        private class TypeDescription
        {
            // The type
            private Type m_type;

            /// <summary>
            /// Gets the type associated with this description
            /// </summary>
            public Type Type { get { return this.m_type; } }

            public TypeDescription(Type type)
            {
                this.m_type = type;
            }

            /// <summary>
            /// Represent as a string
            /// </summary>
            public override string ToString()
            {
                object[] desc = this.m_type.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (desc.Length > 0)
                    return (desc[0] as DescriptionAttribute).Description;
                else
                    return this.m_type.Name;
            }
        }

        public pnlConfigureAudit()
        {
            InitializeComponent();
            InitializeSenders();
        }

        /// <summary>
        /// Initialize the sender 
        /// </summary>
        private void InitializeSenders()
        {
            foreach(Type t in this.GetType().Assembly.GetTypes())
                if (t.GetInterface(typeof(ITransporter).FullName) != null)
                    cbxTransport.Items.Add(new TypeDescription(t));
        }

        /// <summary>
        /// Pad IP address text
        /// </summary>
        private void SetIpText(String ipAddress)
        {

            IPAddress ipAdd = null;

            StringBuilder fullIp = new StringBuilder();
            errValid.Clear();
            foreach (var strP in ipAddress.Split('.'))
            {
                byte b = 0;
                if (Byte.TryParse(strP, out b))
                    fullIp.AppendFormat("{0:000}.", b);
                else
                    errValid.SetError(txtIp, "Invalid IP Address");
            }
            if(fullIp.Length > 0)
                fullIp.Remove(fullIp.Length - 1, 1);
            txtIp.Text = fullIp.ToString();

        }

        /// <summary>
        /// IP address input helper
        /// </summary>
        private void txtIp_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Decimal)
            {
                // Get the last byte that was entered
                string lastByteText = txtIp.Text,
                    ipAddress = txtIp.Text;

                int byteNumber = txtIp.SelectionStart / 4;
                if (txtIp.SelectionStart % 4 != 0 && byteNumber != 3)
                {

                    lastByteText = lastByteText.Substring(byteNumber * 4, 3).Trim();
                    if (lastByteText.Length != 3)
                    {
                        ipAddress = ipAddress.Remove(byteNumber * 4, 3);
                        ipAddress = ipAddress.Insert(byteNumber * 4, String.Format("{0}{1}", new String('0', 3 - lastByteText.Length), lastByteText));
                        txtIp.Text = ipAddress;
                        txtIp.Select((byteNumber + 1) * 4, 0);
                    }
                }
            }
        }

        private void txtIp_Validated(object sender, EventArgs e)
        {
            this.SetIpText(txtIp.Text);
        }

    }
}
