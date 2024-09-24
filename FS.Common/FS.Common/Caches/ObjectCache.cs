using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Common.Caches
{
    public static class ObjectCache
    {  
        public static void SetData(string key, object data, DateTime expiration)
        {
            Providers.ProviderFactory.BuildObjectCacheProvider().SetData(key, data, expiration);
        }
        public static bool Exists(string key)
        {
            return Providers.ProviderFactory.BuildObjectCacheProvider().Exists(key);
        }
        public static object GetData(string key)
        {
            return Providers.ProviderFactory.BuildObjectCacheProvider().GetData(key);
        }
        public static object GetData(string key, DateTime expiration)
        {
            return Providers.ProviderFactory.BuildObjectCacheProvider().GetData(key, expiration);
        } 
        public static void RemoveData(string key)
        {
            Providers.ProviderFactory.BuildObjectCacheProvider().RemoveData(key);
        } 
    }
}
