using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Services
{
    public class PlantUserDelete
    {
        public PlantUserDelete()
        {
        }

        public async static Task<PlantUserDeleteResponse> GetResponse(APIClient aPIClient, Guid plantCode)
        {
            string url = $"/plant-user-delete/{plantCode.ToString()}";

            PlantUserDeleteModel model = new PlantUserDeleteModel();

            PlantUserDeleteResponse result = await aPIClient.PostAsync<PlantUserDeleteModel, PlantUserDeleteResponse>(url, model);

            return result;
        }

        public class PlantUserDeleteResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class PlantUserDeleteModel
        {

        }
    }
}

