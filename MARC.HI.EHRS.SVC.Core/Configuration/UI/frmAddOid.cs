using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MARC.HI.EHRS.SVC.Core.DataTypes;
using System.Collections;

namespace MARC.HI.EHRS.SVC.Core.Configuration.UI
{
    /// <summary>
    /// Add an OID
    /// </summary>
    public partial class frmAddOid : Form
    {

        #region Property Grid Code
        /// <summary>
        /// Extension value
        /// </summary>
        public class ExtensionValue
        {
            public String Name { get; set; }
            public Type ExtensionType { get; set; }
            public Object Value { get; set; }

            /// <summary>
            /// Extension value ctor
            /// </summary>
            public ExtensionValue(string name, Type type, Object value)
            {
                this.Name = name;
                this.ExtensionType = type;
                this.Value = value;
            }
        }

        /// <summary>
        /// Custom property descriptor
        /// </summary>
        public class ExtensionValueDescriptor : PropertyDescriptor
        {
            private ExtensionValue m_extensionValue;

            /// <summary>
            /// Creates the extension description
            /// </summary>
            public ExtensionValueDescriptor(ref ExtensionValue extensionValue, Attribute[] attrs) : base(extensionValue.Name, attrs)
            {
                this.m_extensionValue = extensionValue;
            }

            /// <summary>
            /// Property type
            /// </summary>
            public override Type PropertyType
            {
                get { return this.m_extensionValue.ExtensionType; }
            }

            /// <summary>
            /// Can reset value
            /// </summary>
            public override bool CanResetValue(object component)
            {
                return false;
            }

            /// <summary>
            /// Component type
            /// </summary>
            public override Type ComponentType
            {
                get { return this.m_extensionValue.ExtensionType; }
            }

            /// <summary>
            /// Get the value
            /// </summary>
            public override object GetValue(object component)
            {
                return this.m_extensionValue.Value;
            }

            /// <summary>
            /// Is readonly
            /// </summary>
            public override bool IsReadOnly
            {
                get { return false; }
            }

            /// <summary>
            /// Reset the value
            /// </summary>
            public override void ResetValue(object component)
            {
                ;
            }

            /// <summary>
            /// Set value
            /// </summary>
            public override void SetValue(object component, object value)
            {
                this.m_extensionValue.Value = value;
            }

            /// <summary>
            /// Should serialize
            /// </summary>
            public override bool ShouldSerializeValue(object component)
            {
                return false;
            }
        }

        /// <summary>
        /// Property class
        /// </summary>
        public class ExtensionPropertyClass : CollectionBase, ICustomTypeDescriptor
        {
            /// <summary>
            /// Add extensions
            /// </summary>
            public void Add(String extensionName, Type extensionType, Object value)
            {
                base.List.Add(new ExtensionValue(extensionName, extensionType, value));
            }

            /// <summary>
            /// Get a property value
            /// </summary>
            public Object this[string name]
            {
                get
                {
                    foreach (var itm in this.List)
                        if ((itm as ExtensionValue).Name == name)
                            return (itm as ExtensionValue).Value;
                    return null;
                }
                set
                {
                    foreach (var itm in this.List)
                        if ((itm as ExtensionValue).Name == name)
                            (itm as ExtensionValue).Value = value;
                }
            }

            #region ICustomTypeDescriptor Members

            /// <summary>
            /// Get attributes for the property class
            /// </summary>
            public AttributeCollection GetAttributes()
            {
                return null;
            }

            /// <summary>
            /// Get the name of the class
            /// </summary>
            public string GetClassName()
            {
                return "Extension";
            }

            /// <summary>
            /// Get the component name
            /// </summary>
            /// <returns></returns>
            public string GetComponentName()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// Get the converter
            /// </summary>
            public TypeConverter GetConverter()
            {
                return null;
            }

            /// <summary>
            /// Get default event
            /// </summary>
            public EventDescriptor GetDefaultEvent()
            {
                return null;
            }

            /// <summary>
            /// Get the default property
            /// </summary>
            public PropertyDescriptor GetDefaultProperty()
            {
                var ev = this.List[0] as ExtensionValue;
                return new ExtensionValueDescriptor(ref ev, null);
            }

            /// <summary>
            /// Get the editor
            /// </summary>
            public object GetEditor(Type editorBaseType)
            {
                return null;
            }

            /// <summary>
            /// Get events of this item
            /// </summary>
            public EventDescriptorCollection GetEvents(Attribute[] attributes)
            {
                return null;
            }

            /// <summary>
            /// Get all events
            /// </summary>
            public EventDescriptorCollection GetEvents()
            {
                return null;
            }

            /// <summary>
            /// Get properties (Extension values)
            /// </summary>
            public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
            {
                PropertyDescriptor[] extensions = new PropertyDescriptor[this.Count];
                for (int i = 0; i < this.Count; i++)
                {
                    ExtensionValue ev = this.List[i] as ExtensionValue;
                    extensions[i] = new ExtensionValueDescriptor(ref ev, attributes);
                }
                return new PropertyDescriptorCollection(extensions);

            }

            /// <summary>
            /// Get properties
            /// </summary>
            public PropertyDescriptorCollection GetProperties()
            {
                return this.GetProperties(null);
            }

            /// <summary>
            /// Get propert owner
            /// </summary>
            public object GetPropertyOwner(PropertyDescriptor pd)
            {
                return this;
            }

            #endregion
        }
        #endregion 

        private OidRegistrar m_registrar;

        /// <summary>
        /// Registrar 
        /// </summary>
        public OidRegistrar Registrar
        {
            get
            {
                return this.m_registrar;
            }
            set
            {
                this.m_registrar = value;

                if (m_registrar != null)
                {
                    cbxOid.Items.Clear();
                    foreach (var oid in m_registrar)
                        cbxOid.Items.Add(oid);
                }
            }
        }

        // Oid data
        private MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData m_oid;

        /// <summary>
        /// The name of the OID being edited
        /// </summary>
        public MARC.HI.EHRS.SVC.Core.DataTypes.OidRegistrar.OidData Oid
        {
            get { return this.m_oid; }
            set
            {
                this.m_oid = value;
                // Add list view items
                ExtensionPropertyClass epc = new ExtensionPropertyClass();
                foreach (var ext in OidRegistrar.ExtendedAttributes)
                {
                    var extInstance = Oid.Attributes.Find(o => o.Key == ext.Key);
                    object tValue = ext.Value.IsValueType ? Activator.CreateInstance(ext.Value) : null;
                    if (extInstance.Value != null)
                    {
                        if (ext.Value == typeof(String))
                            tValue = extInstance.Value;
                        else if (ext.Value == typeof(bool))
                            tValue = Convert.ToBoolean(extInstance.Value);
                        else if (ext.Value.IsEnum)
                            tValue = Enum.Parse(ext.Value, extInstance.Value);
                        else
                            continue;
                    }

                    epc.Add(ext.Key, ext.Value, tValue);
                }
                this.txtName.Text = Oid.Name;
                this.txtNote.Text = Oid.Description;
                var regOid = this.m_registrar.FindData(Oid.Oid);
                if (regOid != null)
                    cbxOid.SelectedIndex = cbxOid.Items.IndexOf(regOid);
                else
                    cbxOid.Text = Oid.Oid ;

                if(Oid.Ref != null)
                    this.txtUrl.Text = Oid.Ref.ToString();
                this.pgAttributes.SelectedObject = epc;
            }
        }

        public frmAddOid()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Ok being clicked
        /// </summary>
        private void btnOk_Click(object sender, EventArgs e)
        {

            errDefault.Clear();
            // Validate
            if (((Oid == null) ^ (this.txtName.Text != Oid.Name)) &&
                Registrar.GetOid(this.txtName.Text) != null)
                errDefault.SetError(txtName, "OID Name must be unique");
            else if (String.IsNullOrEmpty(this.txtName.Text))
                errDefault.SetError(txtName, "OID Name must be populated");
            if (String.IsNullOrEmpty(this.cbxOid.Text))
                errDefault.SetError(cbxOid, "OID Value must be populated");

            if (!String.IsNullOrEmpty(errDefault.GetError(txtName))
                || !String.IsNullOrEmpty(errDefault.GetError(cbxOid)))
                return;

            // Set
            this.Oid.Name = txtName.Text;
            this.Oid.Oid = cbxOid.SelectedItem != null ? (cbxOid.SelectedItem as OidRegistrar.OidData).Oid : cbxOid.Text;
            this.Oid.Description = txtNote.Text;
            this.Oid.Ref = new Uri(txtUrl.Text);
            // Copy values
            ExtensionPropertyClass evd = pgAttributes.SelectedObject as ExtensionPropertyClass;
            this.Oid.Attributes.RemoveAll(o => OidRegistrar.ExtendedAttributes.ContainsKey(o.Key));
            foreach (var ext in OidRegistrar.ExtendedAttributes)
            {
                var value = evd[ext.Key];
                if (value != null)
                    this.Oid.Attributes.Add(new KeyValuePair<string,string>(ext.Key, Convert.ToString(value)));
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Cancel
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
