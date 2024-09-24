using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS.Common.Diagnostics.Loggers;

namespace FS.Base.Objects
{
    public class BaseFlow
    { 

        private Dictionary<string, string> queuedValidationErrors = new Dictionary<string, string>();

        protected static void Process(ref object obj)
        {
        }

        protected int GetQueuedValidationErrorCount()
        {
            return queuedValidationErrors.Count;
        }

        protected void Log(string flowname, System.Exception ex)
        {
            FS.Common.Diagnostics.Loggers.Manager.LogMessage(ex);
        }
        protected async Task LogAsync(FS.Common.Objects.SessionContext sessionContext, string flowname, System.Exception ex)
        {
            await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(sessionContext, ex);
        }

        protected void Log(string flowname, string message)
        {
            FS.Common.Diagnostics.Loggers.Manager.LogMessage(ApplicationLogEntrySeverities.Information_MidDetail, flowname + "::" + message);
        }
        protected async Task LogAsync(Common.Objects.SessionContext sessionContext, string flowname, string message)
        {
            await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(sessionContext, ApplicationLogEntrySeverities.Information_MidDetail, flowname + "::" + message);
        }

        protected void Log(string flowname, ApplicationLogEntrySeverities severity, string message)
        {
            FS.Common.Diagnostics.Loggers.Manager.LogMessage(severity, flowname + "::" + message);
        }

        protected async Task LogAsync(Common.Objects.SessionContext sessionContext, string flowname, ApplicationLogEntrySeverities severity, string message)
        {
            await FS.Common.Diagnostics.Loggers.Manager.LogMessageAsync(sessionContext, severity, flowname + "::" + message);
        }


        protected void ThrowValidationError(string message)
        {
            throw new FS.Base.Objects.ValidationError(message);
        }
        protected void ThrowValidationError(string fieldName, string message)
        {
            throw new FS.Base.Objects.ValidationError(fieldName, message);
        }

        protected void AddValidationError(string fieldName, string message)
        {
            if(this.queuedValidationErrors.Keys.Contains(fieldName))
            {
                string existingErrorMessage = this.queuedValidationErrors[fieldName];
                if (!existingErrorMessage.EndsWith("."))
                    existingErrorMessage += ".";
                if (!message.EndsWith("."))
                    message += ".";
                this.queuedValidationErrors[fieldName] = existingErrorMessage + " " + message;
            }
            else
            {
                this.queuedValidationErrors.Add(fieldName, message);
            }
        }

        protected void AddValidationError(string message)
        {
            AddValidationError("", message);
        }

        protected void ThrowQueuedValidationErrors()
        {
            if (this.queuedValidationErrors.Count > 0)
                throw new FS.Base.Objects.ValidationError(this.queuedValidationErrors);
        }
         
    }
     

}
