using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Page.Forms.Init
{
    public class TacRegisterInitObjWF
    {
        public TacRegisterInitObjWF()
        {
        }

        public async Task<TacRegisterGetInitResponse> GetInitResponse(APIClient aPIClient, Guid contextCode)
        {
            string url = $"/tac-register/{contextCode.ToString()}/init";

            TacRegisterGetInitResponse result = await aPIClient.GetAsync<TacRegisterGetInitResponse>(url);

            return result;
        }

        public List<PageHeader> GetPageHeaders(TacRegisterGetInitResponse apiResponse)
        {
            List<PageHeader> result = new List<PageHeader>();
            var emailHeaderIsVisible = false;
            var passwordHeaderIsVisible = false;
            var confirmPasswordHeaderIsVisible = false;
            var firstNameHeaderIsVisible = false;
            var lastNameHeaderIsVisible = false;
            if(emailHeaderIsVisible) //email
                result.Add(new PageHeader("Email", apiResponse.Email));
            if(passwordHeaderIsVisible) //password
                result.Add(new PageHeader("Password", apiResponse.Password));
            if(confirmPasswordHeaderIsVisible) //confirmPassword
                result.Add(new PageHeader("Confirm Password", apiResponse.ConfirmPassword));
            if(firstNameHeaderIsVisible) //firstName
                result.Add(new PageHeader("First Name", apiResponse.FirstName));
            if(lastNameHeaderIsVisible) //lastName
                result.Add(new PageHeader("Last Name", apiResponse.LastName));
            return result;
        }

        public class TacRegisterGetInitResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }
            [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Email { get; set; }
            [Newtonsoft.Json.JsonProperty("password", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Password { get; set; }
            [Newtonsoft.Json.JsonProperty("confirmPassword", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string ConfirmPassword { get; set; }
            [Newtonsoft.Json.JsonProperty("firstName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string FirstName { get; set; }
            [Newtonsoft.Json.JsonProperty("lastName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string LastName { get; set; }

            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public ICollection<ValidationError> ValidationErrors { get; set; }

        }

        private class TacRegisterGetInitModel
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

