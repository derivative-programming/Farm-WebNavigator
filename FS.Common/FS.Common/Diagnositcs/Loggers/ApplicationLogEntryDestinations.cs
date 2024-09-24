using System;
using System.Collections.Generic;
using System.Text;

namespace FS.Common.Diagnostics.Loggers
{

    public enum ApplicationLogEntryDestinations
    {
        None = 1,
        LogFile = 2,
        ProcessID_XMLFile = 3,
        EventViewer = 4,
        DB = 5,
        ErrorLogFile = 6,
        ProcessID_LogFile = 7,
        XMLFile = 8,
        Email = 9,
        Log4Net = 10,
        Broadcast = 11,
        AzureCloud = 12,
        Console = 13
    }
}
