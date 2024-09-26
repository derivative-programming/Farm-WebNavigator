using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Services
{
    public class LandUserPlantMultiSelectToNotEditable
    {
        public LandUserPlantMultiSelectToNotEditable()
        {
        }

        public async static Task<LandUserPlantMultiSelectToNotEditableResponse> GetResponse(APIClient aPIClient, Guid landCode)
        {
            string url = $"/land-user-plant-multi-select-to-not-editable/{landCode.ToString()}";

            LandUserPlantMultiSelectToNotEditableModel model = new LandUserPlantMultiSelectToNotEditableModel();

            LandUserPlantMultiSelectToNotEditableResponse result = await aPIClient.PostAsync<LandUserPlantMultiSelectToNotEditableModel, LandUserPlantMultiSelectToNotEditableResponse>(url, model);

            return result;
        }

        public class LandUserPlantMultiSelectToNotEditableResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class LandUserPlantMultiSelectToNotEditableModel
        {
            [Newtonsoft.Json.JsonProperty("plantCodeListCsv", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string PlantCodeListCsv { get; set; }
            public Guid LandCode { get; set; }

        }
    }
}

