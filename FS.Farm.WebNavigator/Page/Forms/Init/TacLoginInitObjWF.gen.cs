using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Page.Forms.Init
{
    public class TacLoginInitObjWF
    {
        public TacLoginInitObjWF()
        {
        }

        public async Task<TacLoginGetInitResponse> GetInitResponse(APIClient aPIClient, Guid contextCode)
        {
            string url = $"/tac-login/{contextCode.ToString()}/init";

            TacLoginGetInitResponse result = await aPIClient.GetAsync<TacLoginGetInitResponse>(url);

            return result;
        }

        public List<PageHeader> GetPageHeaders(TacLoginGetInitResponse apiResponse)
        {
            List<PageHeader> result = new List<PageHeader>();
            var emailHeaderIsVisible = false;
            var passwordHeaderIsVisible = false;
            if(emailHeaderIsVisible) //email
                result.Add(new PageHeader("Email", apiResponse.Email));
            if(passwordHeaderIsVisible) //password
                result.Add(new PageHeader("Password", apiResponse.Password));
            return result;
        }

        public class TacLoginGetInitResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }
            [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Email { get; set; }
            [Newtonsoft.Json.JsonProperty("password", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Password { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class TacLoginGetInitModel
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

