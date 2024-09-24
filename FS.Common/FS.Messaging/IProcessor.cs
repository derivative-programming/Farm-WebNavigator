using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Messaging
{
    public interface IProcessor
    {
        void Run(FS.Messaging.Message message);
    } 
}
