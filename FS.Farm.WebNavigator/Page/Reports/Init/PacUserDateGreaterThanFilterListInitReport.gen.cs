using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace FS.Farm.WebNavigator.Page.Reports.Init
{
    public class PacUserDateGreaterThanFilterListInitReport
    {
        public PacUserDateGreaterThanFilterListInitReport()
        {
        }

        public async Task<PacUserDateGreaterThanFilterListGetInitResponse> GetInitResponse(APIClient aPIClient, Guid contextCode)
        {
            string url = $"/pac-user-date-greater-than-filter-list/{contextCode.ToString()}/init";

            PacUserDateGreaterThanFilterListGetInitResponse result = await aPIClient.GetAsync<PacUserDateGreaterThanFilterListGetInitResponse>(url);

            return result;
        }
        public class PacUserDateGreaterThanFilterListGetInitResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            public string CurrentDateTimeHeaderVal { get; set; }

            [Newtonsoft.Json.JsonProperty("validationError", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<ValidationError> ValidationError { get; set; }

        }

        private class PacUserDateGreaterThanFilterListGetInitModel
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

