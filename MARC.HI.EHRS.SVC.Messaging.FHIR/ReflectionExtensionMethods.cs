using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MARC.Everest.Attributes;
using System.ServiceModel.Web;
using System.Reflection;

namespace MARC.HI.EHRS.SVC.Messaging.FHIR
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class ReflectionExtensionMethods
    {

        /// <summary>
        /// Set URI
        /// </summary>
        public static Uri GetValueSetDefinition(this Type me)
        {
            var structAtt = me.GetCustomAttribute<StructureAttribute>();
            if (structAtt != null)
                return new Uri(String.Format("http://hl7.org/fhir/v3/{1}",
                    WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.ToString(),
                    structAtt.Name
                ));
            else
                return new Uri(String.Format("{0}/ValueSet/{1}",
                    WebOperationContext.Current.IncomingRequest.UriTemplateMatch.BaseUri.ToString(),
                    me.FullName
                ));
        }

        /// <summary>
        /// Get a custom attribute of type T
        /// </summary>
        public static T GetCustomAttribute<T>(this MemberInfo me) where T : System.Attribute
        {
            object[] tAtts = me.GetCustomAttributes(typeof(T), false);
            if (tAtts.Length == 0)
                return null;
            else
                return tAtts[0] as T;
        }

        /// <summary>
        /// Get a custom attribute of type T
        /// </summary>
        public static T[] GetCustomAttributes<T>(this MemberInfo me) where T : System.Attribute
        {
            object[] tAtts = me.GetCustomAttributes(typeof(T), false);
            if (tAtts.Length == 0)
                return null;
            else
            {
                T[] retVal = new T[tAtts.Length];
                for (int i = 0; i < tAtts.Length; i++)
                    retVal[i] = tAtts[i] as T;
                return retVal;
            }
        }


        /// <summary>
        /// Get a custom attribute of type T
        /// </summary>
        public static T GetCustomAttribute<T>(this Type me) where T : System.Attribute
        {
            object[] tAtts = me.GetCustomAttributes(typeof(T), false);
            if (tAtts.Length == 0)
                return null;
            else
                return tAtts[0] as T;
        }

        /// <summary>
        /// Get a custom attribute of type T
        /// </summary>
        public static T GetCustomAttribute<T>(this Assembly me) where T : System.Attribute
        {
            object[] tAtts = me.GetCustomAttributes(typeof(T), false);
            if (tAtts.Length == 0)
                return null;
            else
                return tAtts[0] as T;
        }
    }

}
