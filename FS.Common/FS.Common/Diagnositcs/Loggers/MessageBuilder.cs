using System;
using System.Collections.Generic;
using System.Text;

namespace FS.Common.Diagnostics.Loggers
{
    public class MessageBuilder
    {

        public static string BuildErrorMessage(Exception ex)
        {
            string errortext = string.Empty;
            errortext += System.Environment.NewLine;
            errortext += System.Environment.NewLine + "****ERROR****";
            errortext += System.Environment.NewLine + "    Message: " + ex.Message;
            errortext += System.Environment.NewLine + "    Source: " + ex.Source;
            errortext += System.Environment.NewLine + "    StackTrace: " + ex.StackTrace;
            errortext += System.Environment.NewLine;
            if (ex.InnerException != null)
            {
                errortext += System.Environment.NewLine;
                errortext += System.Environment.NewLine + "     Inner Exception...";
                errortext += System.Environment.NewLine + "     Message: " + ex.InnerException.Message;
                errortext += System.Environment.NewLine + "     Source: " + ex.InnerException.Source;
                errortext += System.Environment.NewLine + "     StackTrace: " + ex.InnerException.StackTrace;
                errortext += System.Environment.NewLine;
            }
            return errortext;
        }
    }
}
