
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.PubSub.Providers
{
    public static class ProviderFactory
    {
        public static IPublisher BuildPublisherProvider()
        {
            IPublisher result = new PublisherRedisProvider();
            return result;
        }

        public static ISubscriberRedis BuildSubscriberRedisProvider()
        {
            ISubscriberRedis result = new SubscriberRedisProvider();
            return result;
        }
    }
}
