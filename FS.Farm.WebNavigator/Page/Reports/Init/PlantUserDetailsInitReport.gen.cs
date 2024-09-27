using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Page.Reports.Init
{
    public class PlantUserDetailsInitReport
    {
        public PlantUserDetailsInitReport()
        {
        }

        public async Task<PlantUserDetailsGetInitResponse> GetInitResponse(APIClient aPIClient, Guid contextCode)
        {
            string url = $"/plant-user-details/{contextCode.ToString()}/init";

            PlantUserDetailsGetInitResponse result = await aPIClient.GetAsync<PlantUserDetailsGetInitResponse>(url);

            return result;
        }

        public List<PageHeader> GetPageHeaders(PlantUserDetailsGetInitResponse apiResponse)
        {
            List<PageHeader> result = new List<PageHeader>();

            return result;
        }

        public class PlantUserDetailsGetInitResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }
            [Newtonsoft.Json.JsonProperty("landCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid LandCode { get; set; }
            [Newtonsoft.Json.JsonProperty("tacCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid TacCode { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class PlantUserDetailsGetInitModel
        {

        }

        public partial class ValidationError
        {
            [Newtonsoft.Json.JsonProperty("property", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Property { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

        }
    }
}

