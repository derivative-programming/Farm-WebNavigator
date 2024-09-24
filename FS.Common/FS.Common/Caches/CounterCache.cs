using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Caches
{
    public static class CounterCache
    {
        public static bool Exists(string counterName)
        {
            return Providers.ProviderFactory.BuildCounterCacheProvider().Exists(counterName);
        }
        public static async Task<bool> ExistsAsync(string counterName)
        {
            return await Providers.ProviderFactory.BuildCounterCacheProvider().ExistsAsync(counterName);
        }



        public static void Increment(string counterName)
        {
            Providers.ProviderFactory.BuildCounterCacheProvider().Increment(counterName);
        }
        public static async Task IncrementAsync(string counterName)
        {
            await Providers.ProviderFactory.BuildCounterCacheProvider().IncrementAsync(counterName);
        }

        public static void Decrement(string counterName)
        {
            Providers.ProviderFactory.BuildCounterCacheProvider().Decrement(counterName);
        }
        public static async Task DecrementAsync(string counterName)
        {
            await Providers.ProviderFactory.BuildCounterCacheProvider().DecrementAsync(counterName);
        }

        public static void Add(string counterName, long count)
        {
            Providers.ProviderFactory.BuildCounterCacheProvider().Add(counterName, count);
        }
        public static async Task AddAsync(string counterName, long count)
        {
            await Providers.ProviderFactory.BuildCounterCacheProvider().AddAsync(counterName, count);
        }

        public static void Subtract(string counterName, long count)
        {
            Providers.ProviderFactory.BuildCounterCacheProvider().Subtract(counterName, count);
        }
        public static async Task SubtractAsync(string counterName, long count)
        {
            await Providers.ProviderFactory.BuildCounterCacheProvider().SubtractAsync(counterName, count);
        }

        public static long GetValue(string counterName)
        { 
            return Providers.ProviderFactory.BuildCounterCacheProvider().GetValue(counterName);
        }

        public static async Task<long> GetValueAsync(string counterName)
        {
            return await Providers.ProviderFactory.BuildCounterCacheProvider().GetValueAsync(counterName); 
        }

        public static void Remove(string counterName)
        {
            Providers.ProviderFactory.BuildCounterCacheProvider().Remove(counterName);
        }

        public static async Task<bool> RemoveAsync(string counterName)
        {
            return await Providers.ProviderFactory.BuildCounterCacheProvider().RemoveAsync(counterName);
        }
         
    }
}
