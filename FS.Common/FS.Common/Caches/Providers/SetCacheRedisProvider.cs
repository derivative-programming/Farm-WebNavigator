using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis; 

namespace FS.Common.Caches.Providers
{
    public class SetCacheRedisProvider : ISetCacheProvider
    {
        private static object _lockObject = new object();
        private static ConnectionMultiplexer _redisConn = null;
        private static int? _dbNumber = null;

        private IDatabase GetConnection()
        {
            if (_redisConn == null)
            {
                _redisConn = ConnectionMultiplexer.Connect(Common.Redis.Configuration.BuildConnectionString());
            }
            if (_dbNumber == null)
                _dbNumber = int.Parse(Configuration.ApplicationSetting.ReadApplicationSetting("Redis.DBNumber", "0"));
            return _redisConn.GetDatabase((int)_dbNumber);
        }
        public void SetAdd(string key, string data)
        {
            GetConnection().SetAdd(key, data); 
        }
        public async Task<bool> SetAddAsync(string key, string data)
        {
            return await GetConnection().SetAddAsync(key, data);
        }

        public long GetDataCount(string key)
        {
            return GetConnection().SetLength(key);
        }
        public async Task<long> GetDataCountAsync(string key)
        {
            return await GetConnection().SetLengthAsync(key);
        }

        public bool Exists(string key)
        {
            return GetConnection().KeyExists(key);
        }
        public async Task<bool> ExistsAsync(string key)
        {
            return await GetConnection().KeyExistsAsync(key);
        }

        public bool Exists(string key, string data)
        {
            return GetConnection().SetContains(key, data);
        }
        public async Task<bool> ExistsAsync(string key, string data)
        {
            return await GetConnection().SetContainsAsync(key, data);
        }

        public List<string> GetData(string key)
        { 
            List<string> result = new List<string>();

            if (_redisConn == null)
            {
                _redisConn = ConnectionMultiplexer.Connect(Common.Redis.Configuration.BuildConnectionString());
            } 
            RedisValue[] redisVals = GetConnection().SetMembers(key);
            foreach (RedisValue val in redisVals)
            {
                result.Add(val.ToString());
            }
            return result;
        }
        public async Task<List<string>> GetDataAsync(string key)
        {  
            List<string> result = new List<string>();

            if (_redisConn == null)
            {
                _redisConn = ConnectionMultiplexer.Connect(Common.Redis.Configuration.BuildConnectionString());
            } 
            RedisValue[] redisVals = await GetConnection().SetMembersAsync(key);
            foreach (RedisValue val in redisVals)
            {
                result.Add(val.ToString());
            }
            return result;
        } 

        public void RemoveSet(string key)
        {
            GetConnection().KeyDelete(key);
        }
        public async Task<bool> RemoveSetAsync(string key)
        {
            return await GetConnection().KeyDeleteAsync(key);
        }
        public void RemoveData(string key, string data)
        {
            GetConnection().SetRemove(key, data);
        }
        public async Task<bool> RemoveDataAsync(string key, string data)
        {
            return await GetConnection().SetRemoveAsync(key, data);
        }
         
    }
}
