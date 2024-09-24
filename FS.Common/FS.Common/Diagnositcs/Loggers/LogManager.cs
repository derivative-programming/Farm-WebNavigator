using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FS.Common.Diagnostics.Loggers
{
    public static class Manager
    {
        private static FS.Common.Diagnostics.Loggers.ApplicationLog _appLog = new ApplicationLog();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="severity">-1=error,0=warning,1=lowdetail,2=meddetail,3=highdetail</param>
        /// <param name="message"></param>
        public static void LogMessage(int severity, string message)
        {
            switch (severity)
            {
                case -1:
                    FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(
                ApplicationLogEntrySeverities.ErrorOccurred, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 0:
                    FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(
                ApplicationLogEntrySeverities.Warning, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 1:
                    FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(
                ApplicationLogEntrySeverities.Information_LowDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 2:
                    FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(
                ApplicationLogEntrySeverities.Information_MidDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 3:
                    FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(
                ApplicationLogEntrySeverities.Information_HighDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                default:
                    throw new System.Exception("severity level not supported: " + severity.ToString());
            }

        }
        public static async Task LogMessageAsync(Objects.SessionContext sessionContext, int severity, string message)
        {
            string processID = sessionContext.UserName;
            if (processID.Trim().Length > 0)
            {
                processID = processID + "||";
            }
            processID = processID + sessionContext.SessionCode;
            switch (severity)
            {
                case -1:
                    await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext,
                ApplicationLogEntrySeverities.ErrorOccurred, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 0:
                    await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext,
                ApplicationLogEntrySeverities.Warning, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 1:
                    await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext,
                ApplicationLogEntrySeverities.Information_LowDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 2:
                    await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext,
                ApplicationLogEntrySeverities.Information_MidDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 3:
                    await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext,
                ApplicationLogEntrySeverities.Information_HighDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                default:
                    throw new System.Exception("severity level not supported: " + severity.ToString());
            }

        }



        public static void LogMessage( string message)
        {
            FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(ApplicationLogEntrySeverities.Information_HighDetail, ApplicationLogEntryEvents.Undefined, message);

        }
        public static async Task LogMessageAsync(Objects.SessionContext sessionContext, string message)
        {
            string processID = sessionContext.UserName;
            if (processID.Trim().Length > 0)
            {
                processID = processID + "||";
            }
            processID = processID + sessionContext.SessionCode;
            await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext, processID, ApplicationLogEntrySeverities.Information_HighDetail, ApplicationLogEntryEvents.Undefined, message);

        }



        public static void LogMessage(ApplicationLogEntrySeverities severity, string message)
        {
            FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(severity, ApplicationLogEntryEvents.Undefined, message);

        }

        public static void LogMessage(Objects.SessionContext sessionContext, ApplicationLogEntrySeverities severity, string message)
        {
            string processID = sessionContext.UserName;
            if (processID.Trim().Length > 0)
            {
                processID = processID + "||";
            }
            processID = processID + sessionContext.SessionCode;
            FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(processID, severity, ApplicationLogEntryEvents.Undefined, message);

        }
        public static async Task LogMessageAsync(Objects.SessionContext sessionContext, ApplicationLogEntrySeverities severity, string message)
        {
            string processID = sessionContext.UserName;
            if(processID.Trim().Length > 0)
            {
                processID = processID + "||";
            }
            processID = processID + sessionContext.SessionCode;
            await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext, processID, severity, ApplicationLogEntryEvents.Undefined, message);

        }



        public static void LogMessage(System.Exception ex)
        {
            FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(ApplicationLogEntrySeverities.ErrorOccurred, 
                ApplicationLogEntryEvents.Undefined,
                FS.Common.Diagnostics.Loggers.MessageBuilder.BuildErrorMessage(ex));

        }
        public static async Task LogMessageAsync(Objects.SessionContext sessionContext, System.Exception ex)
        {
            string processID = sessionContext.UserName;
            if (processID.Trim().Length > 0)
            {
                processID = processID + "||";
            }
            processID = processID + sessionContext.SessionCode;
            await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext, processID, ApplicationLogEntrySeverities.ErrorOccurred,
                ApplicationLogEntryEvents.Undefined,
                FS.Common.Diagnostics.Loggers.MessageBuilder.BuildErrorMessage(ex));

        }

        public static void LogMessage(ApplicationLogEntrySeverities severity, string processID, string message)
        {

            FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(processID, severity, ApplicationLogEntryEvents.Undefined, message);

        }

        public static async Task LogMessageAsync(Objects.SessionContext sessionContext, ApplicationLogEntrySeverities severity, string processID, string message)
        {

            await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext, processID, severity, ApplicationLogEntryEvents.Undefined, message);

        }

        public static void LogMessage(int severity, string processID, string message)
        {
            switch (severity)
            {
                case -1:
                    FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(processID,
                ApplicationLogEntrySeverities.ErrorOccurred, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 0:
                    FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(processID,
                ApplicationLogEntrySeverities.Warning, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 1:
                    FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(processID,
                ApplicationLogEntrySeverities.Information_LowDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 2:
                    FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(processID,
                ApplicationLogEntrySeverities.Information_MidDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 3:
                    FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntry(processID,
                ApplicationLogEntrySeverities.Information_HighDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                default:
                    throw new System.Exception("severity level not supported: " + severity.ToString());
            }
        }
        public static async Task LogMessageAsync(Objects.SessionContext sessionContext, int severity, string processID, string message)
        {
            switch (severity)
            {
                case -1:
                    await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext, processID,
                ApplicationLogEntrySeverities.ErrorOccurred, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 0:
                    await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext, processID,
                ApplicationLogEntrySeverities.Warning, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 1:
                    await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext, processID,
                ApplicationLogEntrySeverities.Information_LowDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 2:
                    await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext, processID,
                ApplicationLogEntrySeverities.Information_MidDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                case 3:
                    await FS.Common.Diagnostics.Loggers.Manager._appLog.AddEntryAsync(sessionContext, processID,
                ApplicationLogEntrySeverities.Information_HighDetail, ApplicationLogEntryEvents.Undefined, message);
                    break;
                default:
                    throw new System.Exception("severity level not supported: " + severity.ToString());
            }
        }
    }
}
