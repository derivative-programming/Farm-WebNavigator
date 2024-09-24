using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis; 

namespace FS.Common.Caches.Providers
{
    public class StringCacheRedisProvider : IStringCacheProvider
    {
        private  static object _lockObject = new object();
        private static ConnectionMultiplexer _redisConn = null;
        private static int? _dbNumber = null;

        private IDatabase GetConnection()
        {
            if (_redisConn == null)
            {
                _redisConn = ConnectionMultiplexer.Connect(Common.Redis.Configuration.BuildConnectionString());
            }
            if (_dbNumber == null)
                _dbNumber = int.Parse(Configuration.ApplicationSetting.ReadApplicationSetting("Redis.DBNumber", "1"));
            return _redisConn.GetDatabase((int)_dbNumber);
            
        }
        public void SetData(string key, string data)
        { 
            GetConnection().StringSet(key, data);  
        }
        public async Task<bool> SetDataAsync(string key, string data)
        {
            return await GetConnection().StringSetAsync(key, data);
        }
        public void SetData(string key, string data, DateTime expiration)
        { 
            TimeSpan? ts = expiration - DateTime.Now;
            GetConnection().StringSet(key, data, ts); 
       
        }
        public async Task<bool> SetDataAsync(string key, string data, DateTime expiration)
        {
            TimeSpan? ts = expiration - DateTime.Now;
            return await GetConnection().StringSetAsync(key, data, ts);

        }
        public bool Exists(string key)
        { 
            return GetConnection().KeyExists(key); 
        }
        public async Task<bool> ExistsAsync(string key)
        {
            return await GetConnection().KeyExistsAsync(key);
        }
        public string GetData(string key)
        { 
            if (!this.Exists(key))
                return null;
            return GetConnection().StringGet(key);
        }
        public async Task<string> GetDataAsync(string key)
        {
            if (!this.Exists(key))
                return null;
            return await GetConnection().StringGetAsync(key);
        }
        public string GetData(string key, DateTime expiration)
        {
            if (!this.Exists(key))
                return null;
            string result = GetData(key);
            SetData(key,result,expiration);
            return result;
        }
        public async Task<string> GetDataAsync(string key, DateTime expiration)
        {
            if (!this.Exists(key))
                return null;
            string result = await GetDataAsync(key);
            SetData(key, result, expiration);
            return result;
        }
        
        public void RemoveData(string key)
        {
            GetConnection().KeyDelete(key); 
        }
        public async Task<bool> RemoveDataAsync(string key)
        {
            return await GetConnection().KeyDeleteAsync(key);
        }

        public List<string> GetKeys(string keyPattern)
        {
            List<string> result = new List<string>();

            if (_redisConn == null)
            {
                _redisConn = ConnectionMultiplexer.Connect(Common.Redis.Configuration.BuildConnectionString());
            }
            ConnectionMultiplexer redisConn = _redisConn;
            IServer server = redisConn.GetServer(GetConnection().IdentifyEndpoint());
            RedisKey[] redisKeys = server.Keys((int)_dbNumber, keyPattern, 100).ToArray();
            foreach (RedisKey key in redisKeys)
            {
                result.Add(key.ToString());
            }
            return result;
        }
    }
}
