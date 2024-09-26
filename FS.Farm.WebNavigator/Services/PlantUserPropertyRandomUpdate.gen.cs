using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Services
{
    public class PlantUserPropertyRandomUpdate
    {
        public PlantUserPropertyRandomUpdate()
        {
        }

        public async static Task<PlantUserPropertyRandomUpdateResponse> GetResponse(APIClient aPIClient, Guid plantCode)
        {
            string url = $"/plant-user-property-random-update/{plantCode.ToString()}";

            PlantUserPropertyRandomUpdateModel model = new PlantUserPropertyRandomUpdateModel();

            PlantUserPropertyRandomUpdateResponse result = await aPIClient.PostAsync<PlantUserPropertyRandomUpdateModel, PlantUserPropertyRandomUpdateResponse>(url, model);

            return result;
        }

        public class PlantUserPropertyRandomUpdateResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class PlantUserPropertyRandomUpdateModel
        {

            public Guid PlantCode { get; set; }

        }
    }
}

