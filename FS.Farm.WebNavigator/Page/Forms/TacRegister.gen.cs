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
using System.Xml.Linq;
using Microsoft.AspNetCore.Http.Internal;

namespace FS.Farm.WebNavigator.Page.Forms
{
    public class TacRegister : PageBase, IPage
    {
        string _contextCodeName = "TacCode";

        public TacRegister()
        {
            _pageName = "TacRegister";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText = "", string postData = "")
        {
            var pageView = new PageView();

            if (!sessionData.PageName.Equals(_pageName, StringComparison.OrdinalIgnoreCase))
            {
                //new page, clear filters
                sessionData.Filters.Clear();
                sessionData.ValidationErrors.Clear();
                sessionData.FormFieldProposedValues.Clear();
            }

            pageView.PageTitleText = "Create your account";
            pageView.PageIntroText = "A Couple Details Then We Are Off!";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            if (commandText.StartsWith("setFormFieldProposedValue:", StringComparison.OrdinalIgnoreCase))
            {
                string formFieldName = commandText.Split(':')[1];
                string formFieldProposedValue = commandText.Split(':')[2];

                if (formFieldName.Trim().Length == 0)
                {
                    if (sessionData.FormFieldProposedValues.ContainsKey(formFieldName))
                        sessionData.FormFieldProposedValues.Remove(formFieldName);
                }
                else
                {
                    if (sessionData.Filters.ContainsKey(formFieldName))
                    {
                        sessionData.Filters[formFieldName] = formFieldProposedValue;
                    }
                    else
                    {
                        sessionData.Filters.Add(formFieldName, formFieldProposedValue);
                    }
                }
            }

            var initObjWFProcessor = new TacRegisterInitObjWF();

            TacRegisterInitObjWF.TacRegisterGetInitResponse apiInitResponse = await initObjWFProcessor.GetInitResponse(apiClient, contextCode);

            TacRegisterPostModel apiRequestModel = new TacRegisterPostModel();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            //TacRegisterPostResponse apiResponse = await PostResponse(apiClient, apiRequestModel, contextCode);

            pageView.PageHeaders = initObjWFProcessor.GetPageHeaders(apiInitResponse);

            pageView = BuildFormFields(sessionData, pageView, apiInitResponse, apiRequestModel);

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "setFormFieldProposedValue:[field name]:[value (or empty to reset)]",
                    Description = "Give a particular form field a proposed value." }
                );

            // handle objwf buttons
            {
                pageView = BuildAvailableCommandForObjWFButton(pageView, "SubmitButton",
                    "TacFarmDashboard",
                    "TacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Register");
                pageView = BuildAvailableCommandForObjWFButton(pageView, "CancelButton",
                    "TacLogin",
                    "TacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Back To Log In");

            }

            pageView.TableHeaders = null;

            return pageView;
        }

        public PageView BuildFormField(
            SessionData sessionData,
            PageView pageView,
            string name,
            string label,
            string dataType,
            bool isVisible = true,
            bool isRequired = true,
            string currentValue = "",
            string proposedValue = "",
            string detailText = "")
        {
            if(!isVisible)
                return pageView;

            if (dataType == "Password")
                return pageView;

            if ((dataType == "File"))
                return pageView;

            if (sessionData.FormFieldProposedValues.ContainsKey(name))
                proposedValue = sessionData.FormFieldProposedValues[name];
            else
                proposedValue = currentValue;

            FormField formField = new FormField
            {
                Name = name,
                Label = label,
                DataType = dataType,
                DetailText = detailText,
                CurrentValue = currentValue,
                ProposedValue = proposedValue,
                isRequiredField = isRequired,
                ValidationErrorText = ""
            };

            pageView.FormFields.Add(formField);

            return pageView;
        }

        public PageView BuildFormFields(SessionData sessionData,
            PageView pageView,
            TacRegisterInitObjWF.TacRegisterGetInitResponse apiInitResponse,
            TacRegisterPostModel apiRequestModel)
        {
            pageView = BuildFormField(sessionData, pageView, "email",
                "Email",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.Email,
                detailText: "");
            pageView = BuildFormField(sessionData, pageView, "password",
                "Password",
                "Password",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.Password,
                detailText: "");
            pageView = BuildFormField(sessionData, pageView, "confirmPassword",
                "Confirm Password",
                "Password",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.ConfirmPassword,
                detailText: "");
            pageView = BuildFormField(sessionData, pageView, "firstName",
                "First Name",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.FirstName,
                detailText: "");
            pageView = BuildFormField(sessionData, pageView, "lastName",
                "Last Name",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.LastName,
                detailText: "");
            return pageView;
        }

        public async Task<PagePointer> ProcessCommand(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText, string postData = "")
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

            if(!navDictionary.ContainsKey("tacCode"))
            {
                navDictionary.Add("tacCode", contextCode);
            }

            //TODO handle post of form - good form

            //TODO handle post of form - with val errors

            //  handle objwf buttons
            pagePointer = new PagePointer(_pageName, contextCode);

            if (commandText.StartsWith("setFormFieldProposedValue:", StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }
            if (commandText.Equals("SubmitButton",StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "TacFarmDashboard",
                    Guid.Parse(navDictionary["tacCode"].ToString()));
            if (commandText.Equals("CancelButton", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "TacLogin",
                    Guid.Parse(navDictionary["tacCode"].ToString()));

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

    }
}

