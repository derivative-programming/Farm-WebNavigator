using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Caches
{
    public static class StringCache
    {
        public static void SetData(string key, string data)
        {
            Providers.ProviderFactory.BuildStringCacheProvider().SetData(key, data);
        }
        public static async Task SetDataAsync(string key, string data)
        {
            await Providers.ProviderFactory.BuildStringCacheProvider().SetDataAsync(key, data);
        }
        public static void SetData(string key, string data, DateTime expiration)
        {
            Providers.ProviderFactory.BuildStringCacheProvider().SetData(key, data, expiration);
        }
        public static async Task SetDataAsync(string key, string data, DateTime expiration)
        {
            await Providers.ProviderFactory.BuildStringCacheProvider().SetDataAsync(key, data, expiration);
        }
        public static bool Exists(string key)
        {
            return Providers.ProviderFactory.BuildStringCacheProvider().Exists(key);
        }
        public static async Task<bool> ExistsAsync(string key)
        {
           return  await Providers.ProviderFactory.BuildStringCacheProvider().ExistsAsync(key);

        }
        public static string GetData(string key)
        {
            return Providers.ProviderFactory.BuildStringCacheProvider().GetData(key);
        }
        public static async Task<string> GetDataAsync(string key)
        {
            return await Providers.ProviderFactory.BuildStringCacheProvider().GetDataAsync(key);
        }
        public static string GetData(string key, DateTime expiration)
        {
            return Providers.ProviderFactory.BuildStringCacheProvider().GetData(key, expiration);
        }
        public static async Task<string> GetDataAsync(string key, DateTime expiration)
        {
            return await Providers.ProviderFactory.BuildStringCacheProvider().GetDataAsync(key, expiration);
        } 
        public static void RemoveData(string key)
        {
            Providers.ProviderFactory.BuildStringCacheProvider().RemoveData(key);
        }
        public static async Task RemoveDataAsync(string key)
        {
            await Providers.ProviderFactory.BuildStringCacheProvider().RemoveDataAsync(key);
        }

        public static List<string> GetKeys(string keyPattern)
        {
            return Providers.ProviderFactory.BuildStringCacheProvider().GetKeys(keyPattern);
        }
    }
}
