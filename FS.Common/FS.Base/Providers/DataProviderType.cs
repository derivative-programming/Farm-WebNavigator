using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Base.Providers
{
    public enum DataProviderType
    {
        File,
        Firebase,
        MessageQueue,
        MongoDB,
        MySql,
        Postgres,
        Redis,
        SqlServer
    }
}
