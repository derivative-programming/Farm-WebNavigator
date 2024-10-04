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
using System.Globalization;
using FS.Farm.WebNavigator.Page.Forms.Models;
using System.ComponentModel.DataAnnotations;

namespace FS.Farm.WebNavigator.Page.Forms
{
    public class TacLogin : PageBase, IPage
    {
        string _contextCodeName = "TacCode";

        public TacLogin()
        {
            _pageName = "TacLogin";

            this.IsAutoSubmit = false;
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText = "")
        {
            var pageView = new PageView();

            if (!sessionData.PageName.Equals(_pageName, StringComparison.OrdinalIgnoreCase))
            {
                //new page, clear filters
                sessionData.Filters.Clear();
                sessionData.ValidationErrors.Clear();
                sessionData.FormFieldProposedValues.Clear();
            }

            pageView.PageTitleText = "Log In";
            pageView.PageIntroText = "Please enter your email and password.";
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
                    if (sessionData.FormFieldProposedValues.ContainsKey(formFieldName))
                    {
                        sessionData.FormFieldProposedValues[formFieldName] = formFieldProposedValue;
                    }
                    else
                    {
                        sessionData.FormFieldProposedValues.Add(formFieldName, formFieldProposedValue);
                    }
                }
            }

            if (commandText.StartsWith("ClearProposedValues", StringComparison.OrdinalIgnoreCase))
            {
                sessionData.FormFieldProposedValues.Clear();
            }

            var initObjWFProcessor = new TacLoginInitObjWF();

            TacLoginInitObjWF.GetInitResponse apiInitResponse = await initObjWFProcessor.RequestGetInitResponse(apiClient, contextCode);

            TacLoginPostModel apiRequestModel = new TacLoginPostModel();

            MergeProperties(apiRequestModel, apiInitResponse);

            //TacLoginPostResponse apiResponse = await PostResponse(apiClient, apiRequestModel, contextCode);

            pageView.PageHeaders = initObjWFProcessor.GetPageHeaders(apiInitResponse);

            pageView.ValidationErrors = sessionData.ValidationErrors;

            pageView = await BuildFormFields(apiClient, sessionData, pageView, apiInitResponse, apiRequestModel);

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "setFormFieldProposedValue:[field name]:[value (or empty to reset)]",
                    Description = "Give a particular form field a proposed value." }
                );

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "ClearProposedValues", Description = "Clear all proposed values" }
                );

            // handle objwf buttons
            {
                pageView = BuildAvailableCommandForObjWFButton(pageView, "SubmitButton",
                    "TacFarmDashboard",
                    "TacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Log In"
                    );
                pageView = BuildAvailableCommandForObjWFButton(pageView, "OtherButton",
                    "TacRegister",
                    "TacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Register"
                    );
            }

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> apiInitResponseDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            pageView.PageTable = null;

            return pageView;
        }

        public async Task<PageView> BuildFormField(
            APIClient apiClient,
            SessionData sessionData,
            PageView pageView,
            string name,
            string label,
            string fieldType,
            bool isVisible = true,
            bool isRequired = true,
            string currentValue = "",
            string proposedValue = "",
            string detailText = "",
            bool isFKList = false,
            bool isFKLookup = false,
            string fkObjectName = "")
        {
            if(!isVisible)
                return pageView;

            if (fieldType == "Password")
                return pageView;

            if ((fieldType == "File"))
                return pageView;

            if(fieldType.Equals("date",StringComparison.OrdinalIgnoreCase))
            {
                DateTime dateTime = DateTime.UtcNow;
                if(System.DateTime.TryParse(currentValue,out dateTime))
                {
                    currentValue = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
                }
                else
                {
                    currentValue = "";
                }
                if (System.DateTime.TryParse(proposedValue, out dateTime))
                {
                    proposedValue = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
                }
                else
                {
                    proposedValue = "";
                }

            }

            if (fieldType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
            {
                DateTime dateTime = DateTime.UtcNow;
                if (System.DateTime.TryParse(currentValue, out dateTime))
                {
                    currentValue = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
                }
                else
                {
                    currentValue = "";
                }
                if (System.DateTime.TryParse(proposedValue, out dateTime))
                {
                    proposedValue = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");
                }
                else
                {
                    proposedValue = "";
                }

            }

            if (sessionData.FormFieldProposedValues.ContainsKey(name))
                proposedValue = sessionData.FormFieldProposedValues[name];
            else
                proposedValue = currentValue;

            string validationError = string.Empty;

            if(sessionData.ValidationErrors != null)
            {
                var validationErrorObj = sessionData.ValidationErrors.Find(x => x.Property.Equals(name,StringComparison.OrdinalIgnoreCase));

                if (validationErrorObj != null)
                {
                    validationError += validationErrorObj.Message;
                }
            }

            FormField formField = new FormField
            {
                Name = name,
                Label = label,
                FieldType = fieldType,
                DetailText = detailText,
                CurrentValue = currentValue,
                ProposedValue = proposedValue,
                isRequiredField = isRequired,
                ValidationErrorText = validationError,
                LookupItems = null
            };

            if (isFKLookup)
            {
                formField.LookupItems = await LookupFactory.GetLookupItems(apiClient, fkObjectName);
            }

            pageView.FormFields.Add(formField);

            return pageView;
        }

        public async Task<PageView> BuildFormFields(
            APIClient apiClient,
            SessionData sessionData,
            PageView pageView,
            TacLoginInitObjWF.GetInitResponse apiInitResponse,
            TacLoginPostModel apiRequestModel)
        {
            pageView = await BuildFormField(apiClient, sessionData, pageView, "email",
                "Email",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.Email,
                detailText: "");
            pageView = await BuildFormField(apiClient, sessionData, pageView, "password",
                "Password",
                "Password",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.Password,
                detailText: "");
            return pageView;
        }

        public async Task<PagePointer> ProcessCommand(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText)
        {
            PagePointer pagePointer = ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            }

            var initObjWFProcessor = new TacLoginInitObjWF();

            TacLoginInitObjWF.GetInitResponse apiInitResponse = await initObjWFProcessor.RequestGetInitResponse(apiClient, contextCode);

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

            if (commandText.Equals("ClearProposedValues", StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }
            if (commandText.Equals("SubmitButton",StringComparison.OrdinalIgnoreCase))
                if(await TryFormSubmit(sessionData, apiClient, contextCode, apiInitResponse, navDictionary))
                    pagePointer = new PagePointer(
                        "TacFarmDashboard",
                        Guid.Parse(navDictionary["tacCode"].ToString()));
            if (commandText.Equals("OtherButton", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "TacRegister",
                    Guid.Parse(navDictionary["tacCode"].ToString()));
            return pagePointer;
        }

        public async Task<bool> TryFormSubmit(
            SessionData sessionData,
            APIClient apiClient,
            Guid contextCode,
            TacLoginInitObjWF.GetInitResponse apiInitResponse,
            Dictionary<string, object> navDictionary)
        {
            bool result = false;

            TacLoginPostModel apiRequestModel = new TacLoginPostModel();

            MergeProperties(apiRequestModel, apiInitResponse);

            string proposedValuesJson = JsonConvert.SerializeObject(sessionData.FormFieldProposedValues);

            MergeProperties(apiRequestModel, proposedValuesJson);

            TacLoginPostResponse apiResponse = await PostResponse(apiClient, apiRequestModel, contextCode);

            sessionData.ValidationErrors.Clear();

            if(apiResponse.ValidationErrors != null &&
                apiResponse.ValidationErrors.Count > 0)
            {
                foreach(ValidationError validationError in apiResponse.ValidationErrors)
                {
                    sessionData.ValidationErrors.Add(
                        new ValidationError()
                        {
                            Property = validationError.Property,
                            Message = validationError.Message
                        }
                    );
                }
            }
            else
            {

                // Convert apiResponse to a dictionary
                string json = JsonConvert.SerializeObject(apiResponse);
                JObject jsonObject = JObject.Parse(json);
                var newItems = jsonObject.ToObject<Dictionary<string, object>>();

                // Merge newItems into navDictionary
                foreach (var kvp in newItems)
                {
                    navDictionary[kvp.Key] = kvp.Value;
                }
            }

            if (apiInitResponse.Success &&
                sessionData.ValidationErrors.Count == 0)
                return true;

            return result;
        }

        public async Task<TacLoginPostResponse> PostResponse(APIClient aPIClient, TacLoginPostModel model, Guid contextCode)
        {
            string url = $"/tac-login/{contextCode.ToString()}";

            TacLoginPostResponse result = await aPIClient.PostAsync<TacLoginPostModel, TacLoginPostResponse>(url, model);

            return result;
        }

    }
}

