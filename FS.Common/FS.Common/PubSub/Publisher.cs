using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.PubSub
{
    public static class Publisher
    {
        private static Providers.IPublisher _publisher = null;
        private static object _lockObject = new object();

        public static void SendMessage(string channel, string message)
        {
            if(_publisher == null)
            {
                lock (_lockObject)
                {
                    if (_publisher == null)
                    {
                        _publisher = Providers.ProviderFactory.BuildPublisherProvider();
                    }
                }
            }
            _publisher.SendMessage(channel, message);
        }
        public static async Task SendMessageAsync(string channel, string message)
        {
            if (_publisher == null)
            {
                lock (_lockObject)
                {
                    if (_publisher == null)
                    {
                        _publisher = Providers.ProviderFactory.BuildPublisherProvider();
                    }
                }
            }
            await _publisher.SendMessageAsync(channel, message);
        }

        public static void CloseConnection()
        {
            lock (_lockObject)
            {
                if (_publisher != null)
                { 
                    _publisher.CloseConnection();
                    _publisher = null; 
                }
            }

        }
    }
}
