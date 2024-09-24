using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Caches.Providers
{
    public interface IObjectCacheProvider
    {
        void SetData(string key, object data, DateTime expiration);

        bool Exists(string key);

        object GetData(string key);

        object GetData(string key, DateTime expiration);
        void RemoveData(string key);

    }
}
