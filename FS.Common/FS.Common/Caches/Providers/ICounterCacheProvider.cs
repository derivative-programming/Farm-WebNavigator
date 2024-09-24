using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Caches.Providers
{
    public interface ICounterCacheProvider
    {
        void Increment(string counterName);
        Task IncrementAsync(string counterName);
        void Decrement(string counterName);
        Task DecrementAsync(string counterName);
        void Add(string counterName, long count);
        Task AddAsync(string counterName, long count);
        void Subtract(string counterName, long count);
        Task SubtractAsync(string counterName, long count); 
        bool Exists(string counterName);
        Task<bool> ExistsAsync(string counterName);

        long GetValue(string counterName);

        Task<long> GetValueAsync(string counterName);

        void Remove(string counterName);
        Task<bool> RemoveAsync(string counterName);
         
    }
}
