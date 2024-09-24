using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace FS.Common.PubSub.Providers
{
    public class SubscriberRedisProvider: ISubscriberRedis, IDisposable  
    {
        private ConnectionMultiplexer _connection = null;
        private object _lockObject = new object();

        public void ReadMessage(string channel, string logFolder)
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

            subscriber.Subscribe(channel, (ch, message) => {
                string logFileName = String.Format("{0:yyyyMMdd}.{1}.{2:hhmmss}.{3}", DateTime.Now, channel, DateTime.Now, Guid.NewGuid().ToString());
                WriterLog(System.IO.Path.Combine(logFolder, channel), logFileName, message);
            });
        }

        private void WriterLog(string logFolder, string fileName, string message)
        {
            if(!System.IO.Directory.Exists(logFolder))
            {
                System.IO.Directory.CreateDirectory(logFolder);
            }

            string filePath = System.IO.Path.Combine(logFolder, fileName);
            System.IO.File.AppendAllText(filePath, message);
        }

        public async Task ReadMessageAsync(string channel, string logFolder)
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

            await subscriber.SubscribeAsync(channel, (ch, message) => {
                string logFileName = String.Format("{0:yyyyMMdd}.{1}.{2:hhmmss}.{3}", DateTime.Now, channel, DateTime.Now, Guid.NewGuid().ToString());
                WriterLog(System.IO.Path.Combine(logFolder, channel), logFileName, message);
            });
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
