using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.PubSub
{
    public static class SubscriberRedis
    {
        private static Providers.ISubscriberRedis _subscriberredis = null;
        private static object _lockObject = new object();

        public static void ReadMessage(string channel, string logFolder)
        {
            if(_subscriberredis == null)
            {
                lock (_lockObject)
                {
                    if (_subscriberredis == null)
                    {
                        _subscriberredis = Providers.ProviderFactory.BuildSubscriberRedisProvider();
                    }
                }
            }

            _subscriberredis.ReadMessage(channel, logFolder);
        }

        public static async Task ReadMessageAsync(string channel, string logFolder)
        {
            if (_subscriberredis == null)
            {
                lock (_lockObject)
                {
                    if (_subscriberredis == null)
                    {
                        _subscriberredis = Providers.ProviderFactory.BuildSubscriberRedisProvider();
                    }
                }
            }
            await _subscriberredis.ReadMessageAsync(channel, logFolder);
        }

        public static void CloseConnection()
        {
            lock (_lockObject)
            {
                if (_subscriberredis != null)
                {
                    _subscriberredis.CloseConnection();
                    _subscriberredis = null; 
                }
            }

        }
    }
}
