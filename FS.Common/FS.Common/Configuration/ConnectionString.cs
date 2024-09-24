using System;
using System.Configuration;
namespace FS.Common.Configuration
{
    public class ConnectionString
    {
        public ConnectionString()
        {
        }

        static public string ReadConnectionString(string connectionStringName)
        {
            string result = string.Empty; 
            if (System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName] == null)
            {
                string systemConfigurationFilePath =  @"c:\vr\config\system.config";

                if (System.Configuration.ConfigurationManager.AppSettings["FS.Common.Configuration.ApplicationSetting.SystemConfigurationFilePath"] != null)
                {
                    if (System.Configuration.ConfigurationManager.AppSettings.Get("FS.Common.Configuration.ApplicationSetting.SystemConfigurationFilePath").Length != 0)
                    {
                        systemConfigurationFilePath = System.Configuration.ConfigurationManager.AppSettings["FS.Common.Configuration.ApplicationSetting.SystemConfigurationFilePath"].ToString().Trim();
                    }
                }
                if (System.IO.File.Exists(systemConfigurationFilePath))
                {
                    System.Configuration.Configuration systemConfiguration =
                        System.Configuration.ConfigurationManager.OpenExeConfiguration(
                        systemConfigurationFilePath.ToLower().Replace(".config", ""));
                    ConnectionStringsSection connStringsSection = systemConfiguration.ConnectionStrings;

                    if (connStringsSection.ConnectionStrings[connectionStringName] != null)
                    {
                        result = connStringsSection.ConnectionStrings[connectionStringName].ToString().Trim();
                    }
                    else
                    {
                        throw new System.Configuration.ConfigurationErrorsException("Configuration setting name - " + connectionStringName + " not found in configuration file. Verify the setting name used to retrieve values for the configuration file.");
                    }
                }
                else
                { 
                    //throw new System.Configuration.ConfigurationErrorsException("Configuration setting name - " + connectionStringName + " not found in configuration file. Verify the setting name used to retrieve values for the configuration file.");
                }
            }
            else
            {
                result = System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString.Trim();
            }

            string overrideConfig = FS.Common.IO.Directory.GetBinDirectory() + "override.config"; //test
            if (System.IO.File.Exists(overrideConfig))
            {
                if (!System.IO.File.Exists(overrideConfig.Replace(".config", "")))
                { 
                    System.IO.File.Create(overrideConfig.ToLower().Replace(".config", "")).Close();
                }

                System.Configuration.Configuration systemConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(overrideConfig.Replace(".config", ""));
                System.Configuration.ConnectionStringsSection appSettings = systemConfiguration.ConnectionStrings;
                if (appSettings.ConnectionStrings[connectionStringName] != null)
                {
                    result = appSettings.ConnectionStrings[connectionStringName].ToString().Trim(); 
                }
            }

            if (result.Length == 0)
            {
                throw new System.Configuration.ConfigurationErrorsException("Configuration setting name - " + connectionStringName + " not found in configuration file. Verify the setting name used to retrieve values for the configuration file.");
            }
            
            return result;

        }

    }
}
