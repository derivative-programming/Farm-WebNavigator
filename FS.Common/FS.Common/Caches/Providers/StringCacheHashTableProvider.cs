using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Caches.Providers
{
    public class StringCacheHashTableProvider : IStringCacheProvider
    {
        private  object _lockObject = new object();
        private System.Collections.Hashtable _cache = new System.Collections.Hashtable();
        private DateTime _dateTimeLastCleaned = System.DateTime.MinValue;

        public void SetData(string key, string data)
        {
            SetData(key,data,DateTime.MaxValue);
        }
        public async Task<bool> SetDataAsync(string key, string data)
        {
            SetData(key, data);
            return true;
        }
        public void SetData(string key, string data, DateTime expiration)
        {
            if (!System.Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("AllowCaching", "True")))
                return;
            if (key.ToLower() == "systemtest")
                return;
            Clean();
            if (Exists(key))
            {
                lock (_lockObject)
                {
                    CacheData cacheData = (CacheData)_cache[key];
                    cacheData.Data = data;
                    cacheData.ExpirationDateTime = expiration;
                    _cache[key] = cacheData;
                }
            }
            else
            {
                lock (_lockObject)
                {
                    CacheData cacheData = new CacheData(data, expiration);
                    _cache.Add(key, cacheData);
                }
            }
        }
        public async Task<bool> SetDataAsync(string key, string data, DateTime expiration)
        {
            SetData(key, data, expiration);
            return true;
        }
        public bool Exists(string key)
        {
            bool result = false;

            if (key.ToLower() == "systemtest")
                return false;
            lock (_lockObject)
            {
                if (_cache.Contains(key))
                {
                    CacheData cacheData = (CacheData)_cache[key];
                    if (FS.Common.Dates.Functions.XIsEarlierThanY(cacheData.ExpirationDateTime, System.DateTime.Now))
                    {
                        _cache.Remove(key);
                    }
                    else
                        result = true;
                }
            }
            return result;
        }

        public async Task<bool> ExistsAsync(string key)
        {
            return Exists(key);
        }

        public string GetData(string key)
        {
            Clean();
            if (!Exists(key))
                return null;
            return Read(key).Data;
        }
        public async Task<string> GetDataAsync(string key)
        {
            return GetData(key);
        }
        public string GetData(string key, DateTime expiration)
        {
            Clean();
            if (!Exists(key))
                return null;
            return Read(key, expiration).Data;
        }
        public async Task<string> GetDataAsync(string key, DateTime expiration)
        {
            return GetData(key, expiration);
        }
        private CacheData Read(string key)
        {
            CacheData cacheData = null;
            lock (_lockObject)
            {
                cacheData = (CacheData)_cache[key];
            }
            return cacheData;
        }
        private async Task<CacheData> ReadAsync(string key)
        {
            return Read(key);
        }
        private CacheData Read(string key, DateTime expiration)
        {
            CacheData cacheData = null;
            lock (_lockObject)
            {
                cacheData = (CacheData)_cache[key];
                cacheData.ExpirationDateTime = expiration;
            }
            return cacheData;
        }
        private async Task<CacheData> ReadAsync(string key, DateTime expiration)
        {
            return Read(key, expiration);
        }
        public void RemoveData(string key)
        {
            if (Exists(key))
            {
                lock (_lockObject)
                {
                    _cache.Remove(key);
                }
            }
        }
        public async Task<bool> RemoveDataAsync(string key)
        {
            RemoveData(key);
            return true;
        }

        public List<string> GetKeys(string keyPattern)
        {
            throw new SystemException("not implemented");
            return new List<string>();
        }

        private void Clean()
        {
            if (!FS.Common.Dates.Functions.XIsEarlierThanY(_dateTimeLastCleaned, System.DateTime.Now.AddMinutes(-5)))
                return;
            lock (_lockObject)
            {
                List<string> expiredKeys = new List<string>();
                foreach (string key in _cache.Keys)
                {
                    CacheData cacheData = (CacheData)_cache[key];
                    if (FS.Common.Dates.Functions.XIsEarlierThanY(cacheData.ExpirationDateTime, System.DateTime.Now))
                    {
                        expiredKeys.Add(key);
                    }
                }
                for (int i = 0; i < expiredKeys.Count; i++)
                {
                    _cache.Remove(expiredKeys[i]);
                }
                _dateTimeLastCleaned = System.DateTime.Now;
            }
        }

        private class CacheData
        {
            public string TypeName = string.Empty;
            public DateTime ExpirationDateTime = System.DateTime.MinValue;
            public string Data = string.Empty;

            public CacheData(string data, DateTime expiration)
            {
                this.Data = data;
                this.ExpirationDateTime = expiration;
            }
        }
    }
}
