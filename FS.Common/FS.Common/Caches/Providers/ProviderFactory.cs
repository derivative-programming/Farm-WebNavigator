using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Caches.Providers
{
    public static class ProviderFactory
    {
        private static IStringCacheProvider _stringCacheProvider = null;
        private static IObjectCacheProvider _objectCacheProvider = null;
        private static ISetCacheProvider _setCacheProvider = null;
        private static ICounterCacheProvider _counterCacheProvider = null;

        public static IObjectCacheProvider BuildObjectCacheProvider()
        {
            if (_objectCacheProvider == null)
                _objectCacheProvider = new ObjectCacheHashTableProvider();

            return _objectCacheProvider;
        }
        public static IStringCacheProvider BuildStringCacheProvider()
        {
            string stringCacheProviderName = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("FS.StringCacheProvider", "Redis").ToLower();
            if (_stringCacheProvider == null)
            {
                if (stringCacheProviderName.ToLower() == "redis")
                    _stringCacheProvider = new StringCacheRedisProvider();
                else
                    _stringCacheProvider = new StringCacheHashTableProvider();
            } 

            return _stringCacheProvider;
        }
        public static ISetCacheProvider BuildSetCacheProvider()
        {
            if (_setCacheProvider == null)
                _setCacheProvider = new SetCacheRedisProvider();

            return _setCacheProvider;
        }
        public static ICounterCacheProvider BuildCounterCacheProvider()
        {
            if (_counterCacheProvider == null)
                _counterCacheProvider = new CounterCacheRedisProvider();

            return _counterCacheProvider;
        }
    }
}
