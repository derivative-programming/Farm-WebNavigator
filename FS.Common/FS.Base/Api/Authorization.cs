using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Base.Api
{
    public class AuthorizationBase
    {
        protected static AutorizationResponse CanReadObject(string objectName, List<string> userRoles)
        {
            return AutorizationResponse.Maybe;
        }
        protected static AutorizationResponse CanWriteObject(string objectName, List<string> userRoles)
        {
            return AutorizationResponse.Maybe;
        }
        protected static AutorizationResponse CanDeleteObject(string objectName, List<string> userRoles)
        {
            return AutorizationResponse.Maybe;
        }



        protected static AutorizationResponse CanReadObjectInstance(string objectName, object dataObj, FS.Common.Authentication.AuthenticationToken authTokenObj)
        {
            return AutorizationResponse.Maybe;
        }
        protected static AutorizationResponse CanWriteObjectInstance(string objectName, object dataObj, FS.Common.Authentication.AuthenticationToken authTokenObj)
        {
            return AutorizationResponse.Maybe;
        }
        protected static AutorizationResponse CanDeleteObjectInstance(string objectName, object dataObj, FS.Common.Authentication.AuthenticationToken authTokenObj)
        {
            return AutorizationResponse.Maybe;
        }

        protected static AutorizationResponse CanRequestReport(string reportName, List<string> userRoles)
        {
            return AutorizationResponse.Maybe;
        }
        protected static AutorizationResponse CanRequestDocumentStore(string documentStoreName, List<string> userRoles)
        {
            return AutorizationResponse.Maybe;
        }


        protected static AutorizationResponse CanRequestObjectWorkflow(string objectWorkflowName, List<string> userRoles)
        {
            return AutorizationResponse.Maybe;
        }

        protected static AutorizationResponse CanRequestObjectWorkflowInstance(string objectWorkflowName, object dataObj, FS.Common.Authentication.AuthenticationToken authTokenObj)
        {
            return AutorizationResponse.Maybe;
        }

    }
    
    public enum AutorizationResponse 
    {
        Yes,
        No,
        Maybe
    }
}
