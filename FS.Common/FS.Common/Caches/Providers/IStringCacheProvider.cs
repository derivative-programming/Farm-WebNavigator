using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Caches.Providers
{
    public interface IStringCacheProvider
    {
        void SetData(string key, string data, DateTime expiration);
        Task<bool> SetDataAsync(string key, string data, DateTime expiration);
        void SetData(string key, string data);
        Task<bool> SetDataAsync(string key, string data);

        bool Exists(string key);
        Task<bool> ExistsAsync(string key);

        string GetData(string key);

        Task<string> GetDataAsync(string key);

        string GetData(string key, DateTime expiration);

        Task<string> GetDataAsync(string key, DateTime expiration);
        void RemoveData(string key);
        Task<bool> RemoveDataAsync(string key);

        List<string> GetKeys(string keyPattern);
    }
}
