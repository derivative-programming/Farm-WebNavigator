using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Caches.Providers
{
    public static class ProviderFactory
    {
        private static IStringCacheProvider _stringCacheProvider = null; 
        public static IStringCacheProvider BuildStringCacheProvider()
        {
            string stringCacheProviderName = ApplicationSetting.ReadApplicationSetting("StringCacheProvider", "Redis").ToLower();
            if (_stringCacheProvider == null)
            {
                if (stringCacheProviderName.ToLower() == "redis")
                    _stringCacheProvider = new StringCacheRedisProvider();
                else
                    _stringCacheProvider = new StringCacheHashTableProvider();
            } 

            return _stringCacheProvider;
        } 
    }
}
