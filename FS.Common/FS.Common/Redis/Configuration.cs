using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Redis
{
    public static class Configuration
    {
        public static string BuildConnectionString()
        {
            string result = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("Redis.Connection", "");
            if (result.Trim().Length == 0)
                throw new SystemException("applicaiton setting 'Redis.Connection' not found");
            result += ",abortConnect=false";
            return result;
        }
    }
}
