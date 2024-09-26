using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Services
{
    public class ErrorLogConfigResolveErrorLog
    {
        public ErrorLogConfigResolveErrorLog()
        {
        }

        public async static Task<ErrorLogConfigResolveErrorLogResponse> GetResponse(APIClient aPIClient, Guid errorLogCode)
        {
            string url = $"/error-log-config-resolve-error-log/{errorLogCode.ToString()}";

            ErrorLogConfigResolveErrorLogModel model = new ErrorLogConfigResolveErrorLogModel();

            ErrorLogConfigResolveErrorLogResponse result = await aPIClient.PostAsync<ErrorLogConfigResolveErrorLogModel, ErrorLogConfigResolveErrorLogResponse>(url, model);

            return result;
        }

        public class ErrorLogConfigResolveErrorLogResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class ErrorLogConfigResolveErrorLogModel
        {

            public Guid ErrorLogCode { get; set; }

        }
    }
}

