using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Base.Api
{
    public class ErrorLogBase
    {
        protected static void LogObjectError(string objectName, 
            string httpMethod, string id, string post, 
            System.Exception ex, FS.Common.Authentication.AuthenticationToken authenticationToken)
        { 
        }
        protected static void LogDocumentStoreError(string documentStoreName,
            string request,  
            System.Exception ex, FS.Common.Authentication.AuthenticationToken authenticationToken)
        {
        }
        protected static void LogReportError(string reportName, string request, System.Exception ex, FS.Common.Authentication.AuthenticationToken authenticationToken)
        {
        } 
    }
     
}
