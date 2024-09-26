using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Services
{
    public class PacUserTestAsyncFlowReq
    {
        public PacUserTestAsyncFlowReq()
        {
        }

        public async static Task<PacUserTestAsyncFlowReqResponse> GetResponse(APIClient aPIClient, Guid pacCode)
        {
            string url = $"/pac-user-test-async-flow-req/{pacCode.ToString()}";

            PacUserTestAsyncFlowReqModel model = new PacUserTestAsyncFlowReqModel();

            PacUserTestAsyncFlowReqResponse result = await aPIClient.PostAsync<PacUserTestAsyncFlowReqModel, PacUserTestAsyncFlowReqResponse>(url, model);

            return result;
        }

        public class PacUserTestAsyncFlowReqResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class PacUserTestAsyncFlowReqModel
        {

            public Guid PacCode { get; set; }

        }
    }
}

