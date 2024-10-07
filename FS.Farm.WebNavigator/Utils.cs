using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator
{
    class ApplicationSetting
    {
        static public string ReadApplicationSetting(string settingName, string defaultValue)
        {

            string result = string.Empty;

            if (defaultValue == null)
            {
                throw new ArgumentNullException("Null reference to the default value when reading the application settings.");
            }

            result = defaultValue;
             
            string overrideConfig = "override.config";
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
    }
}
