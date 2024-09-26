using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Services
{
    public class CustomerBuildTempApiKey
    {
        public CustomerBuildTempApiKey()
        {
        }

        public async static Task<CustomerBuildTempApiKeyResponse> GetResponse(APIClient aPIClient, Guid customerCode)
        {
            string url = $"/customer-build-temp-api-key/{customerCode.ToString()}";

            CustomerBuildTempApiKeyModel model = new CustomerBuildTempApiKeyModel();

            CustomerBuildTempApiKeyResponse result = await aPIClient.PostAsync<CustomerBuildTempApiKeyModel, CustomerBuildTempApiKeyResponse>(url, model);

            return result;
        }

        public class CustomerBuildTempApiKeyResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class CustomerBuildTempApiKeyModel
        {

            public Guid CustomerCode { get; set; }

        }
    }
}

