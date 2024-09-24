using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace FS.Common.PubSub.Providers
{
    public class PublisherRedisProvider: IPublisher ,IDisposable  
    {
        private ConnectionMultiplexer _connection = null;
        private object _lockObject = new object();

        public void SendMessage(string channel, string message)
        {
            if(_connection == null)
            {
                lock (_lockObject)
                { 
                    if (_connection == null)
                    {
                        _connection = ConnectionMultiplexer.Connect(Common.Redis.Configuration.BuildConnectionString());
                    }
                }
            } 

            IDatabase db = _connection.GetDatabase();
            ISubscriber subscriber = _connection.GetSubscriber();
                 
            subscriber.Publish(channel, message,CommandFlags.FireAndForget);
           
        }

        public async Task SendMessageAsync(string channel, string message)
        {
            if (_connection == null)
            {
                lock (_lockObject)
                {
                    if (_connection == null)
                    {
                        _connection = ConnectionMultiplexer.Connect(Common.Redis.Configuration.BuildConnectionString());
                    }
                }
            }

            IDatabase db = _connection.GetDatabase();
            ISubscriber subscriber = _connection.GetSubscriber();

            await subscriber.PublishAsync(channel, message, CommandFlags.FireAndForget);
        }

        public void CloseConnection()
        {
            lock (_lockObject)
            {
                if (_connection != null)
                { 
                    _connection.Dispose();
                    _connection = null;
                }
            }
        }

        public void Dispose()
        {
            CloseConnection();
        }
    }
}
