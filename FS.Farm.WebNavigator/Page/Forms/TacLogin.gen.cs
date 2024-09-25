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
using FS.Farm.WebNavigator.Page;

namespace FS.Farm.WebNavigator.Page.Forms
{
    public class TacLogin : PageBase, IPage
    {
        string _contextCodeName = "TacCode";

        public TacLogin()
        {
            _pageName = "TacLogin";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText = "", string postData = "")
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Log In";
            pageView.PageIntroText = "Please enter your email and password.";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initObjWFProcessor = new TacLoginInitObjWF();

            TacLoginInitObjWF.TacLoginGetInitResponse apiInitResponse = await initObjWFProcessor.GetInitResponse(apiClient, contextCode);

            TacLoginPostModel apiRequestModel = new TacLoginPostModel();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            //TacLoginPostResponse apiResponse = await PostResponse(apiClient, apiRequestModel, contextCode);

            pageView.PageHeaders = initObjWFProcessor.GetPageHeaders(apiInitResponse);

            //  handle return of form

            //TODO handle hidden controls

            // handle objwf buttons
            {
                pageView = BuildAvailableCommandForObjWFButton(pageView, "SubmitButton",
                    "TacFarmDashboard",
                    "TacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Log In");
                pageView = BuildAvailableCommandForObjWFButton(pageView, "OtherButton",
                    "TacRegister",
                    "TacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Register");
            }

            pageView.TableHeaders = null;

            return pageView;
        }

        public async Task<PagePointer> ProcessCommand(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText, string postData = "")
        {
            PagePointer pagePointer = ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            }

            var initObjWFProcessor = new TacLoginInitObjWF();

            TacLoginInitObjWF.TacLoginGetInitResponse apiInitResponse = await initObjWFProcessor.GetInitResponse(apiClient, contextCode);

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            if(!navDictionary.ContainsKey("tacCode"))
            {
                navDictionary.Add("tacCode", contextCode);
            }

            //TODO handle post of form - good form

            //TODO handle post of form - with val errors

            //  handle objwf buttons
            pagePointer = new PagePointer(_pageName, contextCode);
            if (commandText.Equals("SubmitButton",StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "TacFarmDashboard",
                    Guid.Parse(navDictionary["tacCode"].ToString()));
            if (commandText.Equals("OtherButton", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "TacRegister",
                    Guid.Parse(navDictionary["tacCode"].ToString()));
            return pagePointer;
        }

        public async Task<TacLoginPostResponse> PostResponse(APIClient aPIClient, TacLoginPostModel model, Guid contextCode)
        {
            string url = $"/tac-login/{contextCode.ToString()}";

            TacLoginPostResponse result = await aPIClient.PostAsync<TacLoginPostModel, TacLoginPostResponse>(url, model);

            return result;
        }

        public class TacLoginPostResponse
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

        public class TacLoginPostModel
        {
            [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string Email { get; set; }
            [Newtonsoft.Json.JsonProperty("password", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string Password { get; set; }
            public string RequestSomeLongVarCharVal { get; set; }

        }

    }
}

