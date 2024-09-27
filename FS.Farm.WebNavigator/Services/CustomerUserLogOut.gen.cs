using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Services
{
    public class CustomerUserLogOut
    {
        public CustomerUserLogOut()
        {
        }

        public async static Task<CustomerUserLogOutResponse> GetResponse(APIClient aPIClient, Guid customerCode)
        {
            string url = $"/customer-user-log-out/{customerCode.ToString()}";

            CustomerUserLogOutModel model = new CustomerUserLogOutModel();

            CustomerUserLogOutResponse result = await aPIClient.PostAsync<CustomerUserLogOutModel, CustomerUserLogOutResponse>(url, model);

            return result;
        }

        public class CustomerUserLogOutResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class CustomerUserLogOutModel
        {

        }
    }
}

