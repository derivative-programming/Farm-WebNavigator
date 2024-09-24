using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Messaging
{
    public enum MessageType
    {
        Unknown,
        ObjectInsert,
        ObjectDelete,
        ObjectUpdate,
        ObjectFlow,
        Email,
        ObjectInsertEvent,
        ObjectDeleteEvent,
        ObjectUpdateEvent,
        ObjectMirrorInsert,
        ObjectMirrorDelete,
        ObjectMirrorUpdate,
        ReportWriterInsert,
        ObjectAuditInsert,
        ApiLogInsert,
        ApiPostInsert,
        DeadQueueInsert
    }
}
