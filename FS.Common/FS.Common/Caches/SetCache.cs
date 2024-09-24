using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Caches
{
    public static class SetCache
    {
        public static void SetAdd(string key, string data)
        {
            Providers.ProviderFactory.BuildSetCacheProvider().SetAdd(key, data);
        }
        public static async Task SetAddAsync(string key, string data)
        {
            await Providers.ProviderFactory.BuildSetCacheProvider().SetAddAsync(key, data);
        }
        public static long GetDataCount(string key)
        {
            return Providers.ProviderFactory.BuildSetCacheProvider().GetDataCount(key);
        }
        public static async Task<long> GetDataCountAsync(string key)
        {
            return await Providers.ProviderFactory.BuildSetCacheProvider().GetDataCountAsync(key);

        }
        public static bool Exists(string key)
        {
            return Providers.ProviderFactory.BuildSetCacheProvider().Exists(key);
        }
        public static async Task<bool> ExistsAsync(string key)
        {
           return  await Providers.ProviderFactory.BuildSetCacheProvider().ExistsAsync(key);

        }
        public static bool Exists(string key, string data)
        {
            return Providers.ProviderFactory.BuildSetCacheProvider().Exists(key, data);
        }
        public static async Task<bool> ExistsAsync(string key, string data)
        {
            return await Providers.ProviderFactory.BuildSetCacheProvider().ExistsAsync(key, data);

        }
        public static List<string> GetData(string key)
        {
            return Providers.ProviderFactory.BuildSetCacheProvider().GetData(key);
        }
        public static async Task<List<string>> GetDataAsync(string key)
        {
            return await Providers.ProviderFactory.BuildSetCacheProvider().GetDataAsync(key);
        }
        public static void RemoveSet(string key)
        {
            Providers.ProviderFactory.BuildSetCacheProvider().RemoveSet(key);
        }
        public static async Task RemoveSetAsync(string key)
        {
            await Providers.ProviderFactory.BuildSetCacheProvider().RemoveSetAsync(key);
        }
        public static void RemoveData(string key, string data)
        {
            Providers.ProviderFactory.BuildSetCacheProvider().RemoveData(key, data);
        }
        public static async Task RemoveDataAsync(string key, string data)
        {
            await Providers.ProviderFactory.BuildSetCacheProvider().RemoveDataAsync(key, data);
        }
         
    }
}
