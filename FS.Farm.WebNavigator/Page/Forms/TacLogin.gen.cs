using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FS.Farm.WebNavigator.Page.Forms.Init.TacLoginInitObjWF;

namespace FS.Farm.WebNavigator.Page.Forms
{
    public class TacLogin : PageBase, IPage
    {
        public TacLogin()
        {
            _pageName = "TacLogin";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, Guid sessionCode, Guid contextCode, string postData = "")
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Log In";
            pageView.PageIntroText = "Please enter your email and password.";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);
            //TODO handle form init

            //TODO handle return of form

            //TODO handle hidden controls

            // handle objwf buttons
//endset
            pageView = HandleButton(pageView, "SubmitButton",
                "TacAdd",
                "TacCode",
                isVisible: true,
                isEnabled: true,
                "Log In");
            pageView = HandleButton(pageView, "OtherButton",
                "TacRegister",
                "TacCode",
                isVisible: true,
                isEnabled: true,
                "Register");
//endset
            return pageView;
        }

        public PageView HandleButton(
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

            //TODO handle post of form - good form

            //TODO handle post of form - with val errors

            //  handle objwf buttons
            pagePointer = new PagePointer(_pageName, contextCode);
            if (commandText == "SubmitButton")
                pagePointer = ProcessButtonCommand(
                    "SubmitButton",
                    "TacAdd",
                    "TacCode");
            if (commandText == "OtherButton")
                pagePointer = ProcessButtonCommand(
                    "OtherButton",
                    "TacRegister",
                    "TacCode");
            return pagePointer;
        }

        private PagePointer ProcessButtonCommand(
            string name,
            string destinationPageName,
            string codeName)
        {
            var result = new PagePointer(destinationPageName, Guid.Empty);

            return result;
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
            public System.Guid Code { get; set; }

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

        public partial class ValidationError
        {
            [Newtonsoft.Json.JsonProperty("property", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Property { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

        }
    }
}

