using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Services
{
    public class LandUserPlantMultiSelectToEditable  
    {
        public LandUserPlantMultiSelectToEditable()
        { 
        }  

        public async Task<LandUserPlantMultiSelectToEditableResponse> GetInitResponse(APIClient aPIClient, Guid landCode)
        {
            string url = $"/land-user-plant-multi-select-to-editable/{landCode.ToString()}";

            LandUserPlantMultiSelectToEditableModel model = new LandUserPlantMultiSelectToEditableModel();

            LandUserPlantMultiSelectToEditableResponse result = await aPIClient.PostAsync<LandUserPlantMultiSelectToEditableModel, LandUserPlantMultiSelectToEditableResponse>(url, model);

            return result;
        } 

        public class LandUserPlantMultiSelectToEditableResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class LandUserPlantMultiSelectToEditableModel
        {

            [Newtonsoft.Json.JsonProperty("plantCodeListCsv", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string PlantCodeListCsv { get; set; }

            [Newtonsoft.Json.JsonProperty("landCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public Guid LandCode { get; set; }

        } 
    }
}
