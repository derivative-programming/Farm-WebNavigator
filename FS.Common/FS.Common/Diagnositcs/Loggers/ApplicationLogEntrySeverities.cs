using System;
using System.Collections.Generic;
using System.Text;

namespace FS.Common.Diagnostics.Loggers
{
    public enum ApplicationLogEntrySeverities
    {
        ErrorOccurred = 1,
        Warning = 2,
        Information_LowDetail = 3,
        Information_MidDetail = 4, 
        Information_HighDetail = 5
    }
}
