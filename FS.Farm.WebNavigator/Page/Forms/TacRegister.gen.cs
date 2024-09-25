using FS.Farm.WebNavigator.Page.Reports.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS.Farm.WebNavigator.Page.Forms.Init;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace FS.Farm.WebNavigator.Page.Forms
{
    public class TacRegister : PageBase, IPage
    {
        string _contextCodeName = "TacCode";

        public TacRegister()
        {
            _pageName = "TacRegister";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, Guid sessionCode, Guid contextCode, string postData = "")
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Create your account";
            pageView.PageIntroText = "A Couple Details Then We Are Off!";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initObjWFProcessor = new TacRegisterInitObjWF();

            TacRegisterInitObjWF.TacRegisterGetInitResponse apiInitResponse = await initObjWFProcessor.GetInitResponse(apiClient, contextCode);

            TacRegisterPostModel apiRequestModel = new TacRegisterPostModel();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            TacRegisterPostResponse apiResponse = await PostResponse(apiClient, apiRequestModel, contextCode);

            //  handle return of form

            string json = JsonConvert.SerializeObject(apiResponse);

            pageView.PageData = json;

            //TODO handle hidden controls

            // handle objwf buttons
            {
                pageView = BuildAvailableCommand(pageView, "SubmitButton",
                    "TacFarmDashboard",
                    "TacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Register");
                pageView = BuildAvailableCommand(pageView, "CancelButton",
                    "TacLogin",
                    "TacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Back To Log In");

            }

            return pageView;
        }

        public PageView BuildAvailableCommand(
            PageView pageView,
            string name,
            string destinationPageName,
            string codeName,
            bool isVisible,
            bool isEnabled,
            string buttonText)
        {
            if (!isVisible)
                return pageView;

            if (!isEnabled)
                return pageView;

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = name, CommandTitle = buttonText, CommandDescription = buttonText }
                );

            return pageView;
        }

        public async Task<PagePointer> ProcessCommand(APIClient apiClient, Guid sessionCode, Guid contextCode, string commandText, string postData = "")
        {
            PagePointer pagePointer = ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            }

            var initObjWFProcessor = new TacRegisterInitObjWF();

            TacRegisterInitObjWF.TacRegisterGetInitResponse apiInitResponse = await initObjWFProcessor.GetInitResponse(apiClient, contextCode);

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            if(!navDictionary.ContainsKey("TacCode"))
            {
                navDictionary.Add("TacCode", contextCode);
            }

            //TODO handle post of form - good form

            //TODO handle post of form - with val errors

            //  handle objwf buttons
            pagePointer = new PagePointer(_pageName, contextCode);
            if (commandText == "SubmitButton")
                pagePointer = new PagePointer(
                    "TacFarmDashboard",
                    (Guid)navDictionary["TacCode"]);
            if (commandText == "CancelButton")
                pagePointer = new PagePointer(
                    "TacLogin",
                    (Guid)navDictionary["TacCode"]);

            return pagePointer;
        }

        public async Task<TacRegisterPostResponse> PostResponse(APIClient aPIClient, TacRegisterPostModel model, Guid contextCode)
        {
            string url = $"/tac-register/{contextCode.ToString()}";

            TacRegisterPostResponse result = await aPIClient.PostAsync<TacRegisterPostModel, TacRegisterPostResponse>(url, model);

            return result;
        }

        public class TacRegisterPostResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }
            [Newtonsoft.Json.JsonProperty("customerCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid CustomerCode { get; set; }
            [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Email { get; set; }
            [Newtonsoft.Json.JsonProperty("userCodeValue", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid UserCodeValue { get; set; }
            [Newtonsoft.Json.JsonProperty("uTCOffsetInMinutes", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int UTCOffsetInMinutes { get; set; }
            [Newtonsoft.Json.JsonProperty("roleNameCSVList", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RoleNameCSVList { get; set; }
            [Newtonsoft.Json.JsonProperty("apiKey", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string ApiKey { get; set; }
            public string OutputSomeEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("validationError", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<ValidationError> ValidationError { get; set; }

        }

        public class TacRegisterPostModel
        {
            [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string Email { get; set; }
            [Newtonsoft.Json.JsonProperty("password", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string Password { get; set; }
            [Newtonsoft.Json.JsonProperty("confirmPassword", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string ConfirmPassword { get; set; }
            [Newtonsoft.Json.JsonProperty("firstName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string FirstName { get; set; }
            [Newtonsoft.Json.JsonProperty("lastName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string LastName { get; set; }
            public string RequestSomeLongVarCharVal { get; set; }

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

