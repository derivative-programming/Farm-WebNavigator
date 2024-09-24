using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FS.Farm.WebNavigator.Page.Forms.Init.TacRegisterInitObjWF;

namespace FS.Farm.WebNavigator.Page.Forms
{
    public class TacRegister : PageBase, IPage
    {
        public TacRegister()
        {
            _pageName = "TacRegister";
        }
        public PageView BuildPageView(Guid sessionCode, Guid contextCode)
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Create your account";
            pageView.PageIntroText = "A Couple Details Then We Are Off!";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);
            //TODO handle form init

            //TODO handle return of form

            //TODO handle hidden controls

            // handle objwf buttons
//endset
            pageView = HandleButton(pageView, "SubmitButton",
                "TacAddCustomer",
                "TacCode",
                isVisible: true,
                isEnabled: true,
                "Register");
            pageView = HandleButton(pageView, "CancelButton",
                "TacLogin",
                "TacCode",
                isVisible: true,
                isEnabled: true,
                "Back To Log In");

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

        public PagePointer ProcessCommand(Guid sessionCode, Guid contextCode, string commandText, string postData = "")
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
                    "TacAddCustomer",
                    "TacCode");
            if (commandText == "CancelButton")
                pagePointer = ProcessButtonCommand(
                    "CancelButton",
                    "TacLogin",
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
            [Newtonsoft.Json.JsonProperty("email", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Email { get; set; }
            [Newtonsoft.Json.JsonProperty("uTCOffsetInMinutes", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int UTCOffsetInMinutes { get; set; }
            public System.Guid CustomerCode { get; set; }

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

