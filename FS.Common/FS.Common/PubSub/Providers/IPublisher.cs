using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.PubSub.Providers
{
    public interface IPublisher
    {
        void SendMessage(string channel, string message);
        Task SendMessageAsync(string channel, string message);

        void CloseConnection();
    }
}
