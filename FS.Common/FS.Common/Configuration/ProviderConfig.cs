using System;
using System.Configuration;
namespace FS.Common.Configuration
{
    public class ProviderConfig
    {
        public ProviderConfig()
        {
        }

        static public object Get(string targetNamespace, string providerName)
        {
            string sectionName = targetNamespace + "." + providerName;
            object result = ConfigurationManager.GetSection(sectionName);
             
            string overrideConfig = FS.Common.IO.Directory.GetBinDirectory()  + targetNamespace  + ".config";
            if (System.IO.File.Exists(overrideConfig))
            {
                if (!System.IO.File.Exists(overrideConfig.Replace(".config", "")))
                { 
                    System.IO.File.Create(overrideConfig.ToLower().Replace(".config", "")).Close();
                }

                System.Configuration.Configuration systemConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(overrideConfig.Replace(".config", ""));
                if (systemConfiguration.GetSection(sectionName) != null)
                {
                    result = (object)systemConfiguration.GetSection(sectionName); 
                }
            }

            if (result == null)
            {
                throw new System.Configuration.ConfigurationErrorsException("Provider Config - " + providerName + " not found in configuration file. .");
            }
            
            return result;

        }

    }
}
