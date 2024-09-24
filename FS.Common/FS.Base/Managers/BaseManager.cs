using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using FS.Common.Objects;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks; 
using FS.Common.Diagnostics.Loggers;

namespace FS.Base.Managers
{
    public class BaseManager
    {
        public enum LookupEnum
        {
            Unknown
        }

        protected static void CustomProviderInitialization()
        {
        }

        protected static void CustomObjectInitialization(ref object sessionContextObj, ref object obj)
        {
        }

        protected static List<ValidationResult> CustomObjectValidation(object obj, List<ValidationResult> currentValidationResults)
        { 
            return currentValidationResults;
        }


        public static bool IsCacheAll(SessionContext context)
        {
            return false;
        }
        public static bool IsCacheIndividual(SessionContext context)
        {
            return false;
        }
        protected static int GetCacheLifetimeInMinutes(SessionContext context)
        {
            return 60;
        }
        protected static void InitializeProperty(ref object obj, string propertyName)
        {

            if (FS.Common.Reflection.Functions.GetPropertyDataType(obj, "GENVALPropName") == typeof(string))
            {
                FS.Common.Reflection.Functions.SetPropertyValue(ref obj, "GENVALPropName", "");
            }

            if (FS.Common.Reflection.Functions.GetPropertyDataType(obj, "GENVALPropName") == typeof(Int32) ||
                FS.Common.Reflection.Functions.GetPropertyDataType(obj, "GENVALPropName") == typeof(Int64))
            {
                FS.Common.Reflection.Functions.SetPropertyValue(ref obj, "GENVALPropName", "0");
            }

            if (FS.Common.Reflection.Functions.GetPropertyDataType(obj, "GENVALPropName") == typeof(bool))
            {
                FS.Common.Reflection.Functions.SetPropertyValue(ref obj, "GENVALPropName", false.ToString());
            }

            if (FS.Common.Reflection.Functions.GetPropertyDataType(obj, "GENVALPropName") == typeof(DateTime))
            {
                FS.Common.Reflection.Functions.SetPropertyValue(ref obj, "GENVALPropName", ((System.DateTime)System.Data.SqlTypes.SqlDateTime.MinValue).ToString());
            }
             
        }
         

        protected static void WriteXMLAttribute(ref System.Xml.XmlTextWriter tw, string propertyName, string propertyValue)
        {
            tw.WriteAttributeString(propertyName.ToLower(), propertyValue);
        }


        protected static void Log(System.Exception ex)
        {
            FS.Common.Diagnostics.Loggers.Manager.LogMessage(ex);
        }
        protected static async Task LogAsync(SessionContext sessionContext, System.Exception ex)
        {
            await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(sessionContext, ex);
        }

        protected static void Log(string objectName, string message)
        {
            FS.Common.Diagnostics.Loggers.Manager.LogMessage(ApplicationLogEntrySeverities.Information_MidDetail, objectName + "::" + message);
        }
        protected static async Task LogAsync(string objectName, SessionContext sessionContext, string message)
        {
            await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(sessionContext, ApplicationLogEntrySeverities.Information_MidDetail, objectName + "::" + message);
        }
        protected static void Log(string objectName, ApplicationLogEntrySeverities severity, string message)
        {
            FS.Common.Diagnostics.Loggers.Manager.LogMessage(severity, objectName + "::" + message);
        }
        protected static async Task LogAsync(string objectName, SessionContext sessionContext, ApplicationLogEntrySeverities severity, string message)
        {
            await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(sessionContext, severity, objectName + "::" + message);
        }
    }
}
 
