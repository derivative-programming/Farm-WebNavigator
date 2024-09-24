using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.PubSub.Providers
{
    public interface ISubscriberRedis
    {
        void ReadMessage(string channel, string logFolder);
        Task ReadMessageAsync(string channel, string logFolder);
        void CloseConnection();
    }
}
