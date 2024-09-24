using System;
using System.Configuration;
namespace FS.Common.Configuration
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class ApplicationSetting
    {
        public ApplicationSetting()
        {
        }

        /// <summary>
        /// ReadApplicationSetting - Read application setting.
        /// </summary>
        /// <param name="settingName">Setting name to search for.</param>
        /// <param name="defaultValue">Default value to use if configuration setting is not found.</param>
        /// <returns>Contents of the configuration setting.</returns>
        static public string ReadApplicationSetting(string settingName, string defaultValue)
        {

            string result = string.Empty;

            if (defaultValue == null)
            {
                throw new ArgumentNullException("Null reference to the default value when reading the application settings.");
            }

            result = defaultValue; 
             
            if (System.Configuration.ConfigurationManager.AppSettings[settingName] == null)
            {
                string systemConfigurationFilePath = @"c:\vr\config\system.config";

                if (System.Configuration.ConfigurationManager.AppSettings["FS.Common.Configuration.ApplicationSetting.SystemConfigurationFilePath"] != null)
                {
                    systemConfigurationFilePath = System.Configuration.ConfigurationManager.AppSettings["FS.Common.Configuration.ApplicationSetting.SystemConfigurationFilePath"].ToString().Trim();
                }
                if (System.IO.File.Exists(systemConfigurationFilePath))
                {
                    if (!System.IO.File.Exists(systemConfigurationFilePath.ToLower().Replace(".config", "")))
                    {
                        System.IO.File.Create(systemConfigurationFilePath.ToLower().Replace(".config", "")).Close();
                    }
                    System.Configuration.Configuration systemConfiguration =
                        System.Configuration.ConfigurationManager.OpenExeConfiguration(
                        systemConfigurationFilePath.ToLower().Replace(".config", ""));

                    AppSettingsSection appSettings = systemConfiguration.AppSettings;

                    if (appSettings.Settings[settingName] != null)
                    {
                        result = appSettings.Settings[settingName].Value.ToString().Trim();
                    }
                }
            }
            else
            {
                result = System.Configuration.ConfigurationManager.AppSettings[settingName].ToString().Trim();
            } 

            string overrideConfig = FS.Common.IO.Directory.GetBinDirectory() + "override.config";
            if (System.IO.File.Exists(overrideConfig))
            {
                if (!System.IO.File.Exists(overrideConfig.Replace(".config", "")))
                { 
                    System.IO.File.Create(overrideConfig.ToLower().Replace(".config", "")).Close();
                }

                System.Configuration.Configuration systemConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(overrideConfig.Replace(".config", ""));
                System.Configuration.AppSettingsSection appSettings = systemConfiguration.AppSettings;
                if (appSettings.Settings[settingName] != null)
                {
                    result = appSettings.Settings[settingName].Value.ToString().Trim(); 
                }
            }

            return result;

        }


        /// <summary>
        /// ReadApplicationSetting - Overload takes on the application setting name.
        /// </summary>
        /// <param name="settingName">Setting name to search for.</param>
        /// <returns>Contents of the configuration setting.</returns>
        static public string ReadApplicationSetting(string settingName)
        {
            string result = "nonefound"; 

            if (System.Configuration.ConfigurationManager.AppSettings[settingName] == null)
            {
                string systemConfigurationFilePath = @"c:\vr\config\system.config";

                if (System.Configuration.ConfigurationManager.AppSettings["FS.Common.Configuration.ApplicationSetting.SystemConfigurationFilePath"] != null)
                {
                    systemConfigurationFilePath = System.Configuration.ConfigurationManager.AppSettings["FS.Common.Configuration.ApplicationSetting.SystemConfigurationFilePath"].ToString().Trim();
                }
                if (System.IO.File.Exists(systemConfigurationFilePath))
                {
                    if (!System.IO.File.Exists(systemConfigurationFilePath.ToLower().Replace(".config", "")))
                    {
                        System.IO.File.Create(systemConfigurationFilePath.ToLower().Replace(".config", "")).Close();
                    }
                    System.Configuration.Configuration systemConfiguration =
                        System.Configuration.ConfigurationManager.OpenExeConfiguration(
                        systemConfigurationFilePath.ToLower().Replace(".config", ""));

                    AppSettingsSection appSettings = systemConfiguration.AppSettings;

                    if (appSettings.Settings[settingName] != null)
                    {
                        result = appSettings.Settings[settingName].Value.ToString().Trim();
                    }
                }
            }
            else
            {
                result = System.Configuration.ConfigurationManager.AppSettings[settingName].ToString().Trim();
            }

            string overrideConfig = FS.Common.IO.Directory.GetBinDirectory() + "override.config";
            if (System.IO.File.Exists(overrideConfig))
            {
                if (!System.IO.File.Exists(overrideConfig.Replace(".config", "")))
                {
                    System.IO.File.Create(overrideConfig.ToLower().Replace(".config", "")).Close();
                }

                System.Configuration.Configuration systemConfiguration = System.Configuration.ConfigurationManager.OpenExeConfiguration(overrideConfig.Replace(".config", ""));
                System.Configuration.AppSettingsSection appSettings = systemConfiguration.AppSettings;
                if (appSettings.Settings[settingName] != null)
                {
                    result = appSettings.Settings[settingName].Value.ToString().Trim();
                }
            }

            if(result == "nonefound")
                throw new System.Configuration.ConfigurationErrorsException("Configuration setting name - " + settingName + " not found in configuration file. Verify the setting name used to retrieve values for the configuration file.");


            return result;

        }

    }
}
