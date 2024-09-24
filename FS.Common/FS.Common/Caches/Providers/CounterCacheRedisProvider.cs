using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis; 

namespace FS.Common.Caches.Providers
{
    public class CounterCacheRedisProvider : ICounterCacheProvider
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
                _dbNumber = int.Parse(Configuration.ApplicationSetting.ReadApplicationSetting("Redis.DBNumber", "0"));
            return _redisConn.GetDatabase((int)_dbNumber);
            
        } 
        public bool Exists(string counterName)
        {
            return GetConnection().KeyExists(counterName); 
        }
        public async Task<bool> ExistsAsync(string counterName)
        {
            return await GetConnection().KeyExistsAsync(counterName);
        } 
         
          

        public void Increment(string counterName)
        {
            GetConnection().StringIncrement(counterName);
        }
        public async Task IncrementAsync(string counterName)
        {
            await GetConnection().StringIncrementAsync(counterName);
        }

        public void Decrement(string counterName)
        {
            GetConnection().StringDecrement(counterName);
        }
        public async Task DecrementAsync(string counterName)
        {
            await GetConnection().StringDecrementAsync(counterName);
        }

        public void Add(string counterName, long count)
        {
            GetConnection().StringIncrement(counterName, count);
        }
        public async Task AddAsync(string counterName, long count)
        {
            await GetConnection().StringIncrementAsync(counterName, count);
        }

        public void Subtract(string counterName, long count)
        {
            GetConnection().StringDecrement(counterName, count);
        }
        public async Task SubtractAsync(string counterName, long count)
        {
            await GetConnection().StringDecrementAsync(counterName, count);
        }

        public long GetValue(string counterName)
        {
            if (!this.Exists(counterName))
                return 0;
            return long.Parse(GetConnection().StringGet(counterName));
        }

        public async Task<long> GetValueAsync(string counterName)
        {
            if (!this.Exists(counterName))
                return 0;
            string val =  await GetConnection().StringGetAsync(counterName);
            return long.Parse(val);
        }

        public void Remove(string counterName)
        {
            GetConnection().KeyDelete(counterName); 
        }

        public async Task<bool> RemoveAsync(string counterName)
        {
            return await GetConnection().KeyDeleteAsync(counterName);
        }
    }
}
