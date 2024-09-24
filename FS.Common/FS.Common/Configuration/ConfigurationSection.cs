using System;
using System.Configuration;
namespace FS.Common.Configuration
{
    public class ConfigurationSection
    {
        public ConfigurationSection()
        {
        }

        //static public object GetConfigurationSection(string configurationSectionName)
        //{
        //    object result = null;

        //    if (System.Configuration.ConfigurationManager.GetSection(configurationSectionName) == null)
        //    {
        //        string systemConfigurationFilePath = @"c:\vr\config\system.config";

        //        if (System.Configuration.ConfigurationManager.AppSettings["FS.Common.Configuration.ApplicationSetting.SystemConfigurationFilePath"] != null)
        //        {
        //            if (System.Configuration.ConfigurationManager.AppSettings.Get("FS.Common.Configuration.ApplicationSetting.SystemConfigurationFilePath").Length != 0)
        //            {
        //                systemConfigurationFilePath = System.Configuration.ConfigurationManager.AppSettings["FS.Common.Configuration.ApplicationSetting.SystemConfigurationFilePath"].ToString().Trim();
        //            }
        //        }
        //        if (System.IO.File.Exists(systemConfigurationFilePath))
        //        {
        //            System.Configuration.Configuration systemConfiguration =
        //                System.Configuration.ConfigurationManager.OpenExeConfiguration(
        //                systemConfigurationFilePath.ToLower().Replace(".config", ""));
        //            ConfigurationSectionCollection sectioncollection = systemConfiguration.Sections;
        //            result = sectioncollection.Get(configurationSectionName);

        //        }
        //        else
        //        {
        //            throw new System.Configuration.ConfigurationErrorsException("Configuration section name - " + configurationSectionName + " not found in configuration file.");
        //        }
        //    }
        //    else
        //    {
        //        result = System.Configuration.ConfigurationManager.GetSection(configurationSectionName);
        //    }
        //    if (result == null)
        //    {
        //        throw new System.Configuration.ConfigurationErrorsException("Configuration section name - " + configurationSectionName + " not found in configuration file.");
        //    }
        //    return result;

        //}

    }
}
