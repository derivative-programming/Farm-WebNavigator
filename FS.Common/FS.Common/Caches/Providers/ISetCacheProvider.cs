using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Caches.Providers
{
    public interface ISetCacheProvider
    {
        void SetAdd(string key, string data);
        Task<bool> SetAddAsync(string key, string data);

        long GetDataCount(string key);
        Task<long> GetDataCountAsync(string key);
        bool Exists(string key);
        Task<bool> ExistsAsync(string key);

        bool Exists(string key, string data);
        Task<bool> ExistsAsync(string key, string data);

        List<string> GetData(string key);

        Task<List<string>> GetDataAsync(string key);

        void RemoveSet(string key);
        Task<bool> RemoveSetAsync(string key);

        void RemoveData(string key, string data);
        Task<bool> RemoveDataAsync(string key, string data);
    }
}
