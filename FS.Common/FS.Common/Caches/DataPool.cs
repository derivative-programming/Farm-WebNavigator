using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Common.Caches
{
    public class DataPoolCache
    {
        private static object _lockObject = new object();
        private System.Collections.Hashtable _objectCache = new System.Collections.Hashtable();
        private DateTime _dateTimeLastCleaned = System.DateTime.MinValue;
        private int MaxSize = 0;


        public DataPoolCache(int maxSize)
        {
            this.MaxSize = maxSize;
        }

        public void SetData(string key, object data, DateTime expiration)
        {
            if (!System.Convert.ToBoolean(FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("AllowCaching", "True")))
                return;
            if (key.ToLower() == "systemtest")
                return; 
            if (Exists(key))
            {
                lock (_lockObject)
                {
                    CacheData cacheData = (CacheData)_objectCache[key];
                    cacheData.Data = data;
                    cacheData.ExpirationDateTime = expiration;
                    _objectCache[key] = cacheData;
                }
            }
            else
            {
                lock (_lockObject)
                {
                    CacheData cacheData = new CacheData(data, expiration);
                    _objectCache.Add(key, cacheData);
                }
            }
        }
        public   bool Exists(string key)
        {
            Clean();
            bool result = false;

            if (key.ToLower() == "systemtest")
                return false;
            lock (_lockObject)
            {
                if (_objectCache.Contains(key))
                {
                    CacheData cacheData = (CacheData)_objectCache[key];
                    if (FS.Common.Dates.Functions.XIsEarlierThanY(cacheData.ExpirationDateTime, System.DateTime.Now))
                    {
                        _objectCache.Remove(key);
                    }
                    else
                        result = true;
                }
            }
            return result;
        }
        public   object GetData(string key)
        {  
            return Read(key).Data;
        }
        public   object GetData(string key, DateTime expiration)
        {  
            return Read(key, expiration).Data;
        }
        private   CacheData Read(string key)
        {
            CacheData cacheData = null;
            lock (_lockObject)
            {
                cacheData = (CacheData)_objectCache[key];
            }
            return cacheData;
        }
        private   CacheData Read(string key, DateTime expiration)
        {
            CacheData cacheData = null;
            lock (_lockObject)
            {
                cacheData = (CacheData)_objectCache[key];
                cacheData.ExpirationDateTime = expiration;
            }
            return cacheData;
        }
        public   void RemoveData(string key)
        {
            if (Exists(key))
            {
                lock (_lockObject)
                {
                    _objectCache.Remove(key);
                }
            }
        }

        private   void Clean()
        {
            if(!FS.Common.Dates.Functions.XIsEarlierThanY(_dateTimeLastCleaned,System.DateTime.Now.AddMinutes(-5)))
                return;
            lock (_lockObject)
            {
                if (_objectCache.Count > MaxSize)
                    _objectCache.Clear();
                List<string> expiredKeys = new List<string>();
                foreach (string key in _objectCache.Keys)
                {
                    CacheData cacheData = (CacheData)_objectCache[key];
                    if (FS.Common.Dates.Functions.XIsEarlierThanY(cacheData.ExpirationDateTime, System.DateTime.Now))
                    {
                        expiredKeys.Add(key);
                    }
                }
                for (int i = 0; i < expiredKeys.Count; i++)
                {
                    _objectCache.Remove(expiredKeys[i]);
                }
                _dateTimeLastCleaned = System.DateTime.Now;
            }
        }

        private class CacheData 
        {
            public string TypeName = string.Empty;
            public DateTime ExpirationDateTime = System.DateTime.MinValue; 
            public object Data = null;

            public CacheData(object data, DateTime expiration)
            {
                this.Data = data;
                this.ExpirationDateTime = expiration;
            }
        }
    }
}
