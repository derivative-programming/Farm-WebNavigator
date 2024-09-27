using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Page.Reports.Init
{
    public class PacUserRoleListInitReport
    {
        public PacUserRoleListInitReport()
        {
        }

        public async Task<PacUserRoleListGetInitResponse> GetInitResponse(APIClient aPIClient, Guid contextCode)
        {
            string url = $"/pac-user-role-list/{contextCode.ToString()}/init";

            PacUserRoleListGetInitResponse result = await aPIClient.GetAsync<PacUserRoleListGetInitResponse>(url);

            return result;
        }

        public List<PageHeader> GetPageHeaders(PacUserRoleListGetInitResponse apiResponse)
        {
            List<PageHeader> result = new List<PageHeader>();

            return result;
        }

        public class PacUserRoleListGetInitResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class PacUserRoleListGetInitModel
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

