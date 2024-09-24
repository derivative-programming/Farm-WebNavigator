using System;
using System.Collections.Generic;
using System.Text;

namespace FS.Common.Diagnostics.Loggers
{
    public enum ApplicationLogEntryEvents
    {
        Initialize = 1,
        BeginRequest = 2,
        AuthenticateRequest = 3,
        AuthorizeRequest = 4,
        ResolveRequestCache = 5,
        AcquireRequestState = 6,
        PreRequestHandlerExecute = 7,
        PostRequestHandlerExecute = 8,
        ReleaseRequestState = 9,
        UpdateRequestCache = 10,
        EndRequest = 11,
        PreSendRequestHeaders = 12,
        PreSendRequestContent = 13,
        ErrorOccurred = 14,
        Disposed = 15,
        Undefined = 16,
        BeginProcess = 17,
        EndProcess = 18
    }
}