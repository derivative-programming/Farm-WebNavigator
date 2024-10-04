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
    public class LandAddPlant : PageBase, IPage
    {
        string _contextCodeName = "LandCode"; 

        public LandAddPlant()
        {
            _pageName = "LandAddPlant";

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

            pageView.PageTitleText = "Add PlantAdd plant form title text"; 
            pageView.PageIntroText = "Add plant intro text.Add plant form intro text";  
            pageView.PageFooterText = "Add plant form footer text";  

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

            var initObjWFProcessor = new LandAddPlantInitObjWF();

            LandAddPlantInitObjWF.GetInitResponse apiInitResponse = await initObjWFProcessor.RequestGetInitResponse(apiClient, contextCode);

            LandAddPlantPostModel apiRequestModel = new LandAddPlantPostModel();

            MergeProperties(apiRequestModel, apiInitResponse);  

            //LandAddPlantPostResponse apiResponse = await PostResponse(apiClient, apiRequestModel, contextCode);

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
                    "LandPlantList",
                    "LandCode",
                    isVisible: true,
                    isEnabled: true,
                    "OK Button Text"
                    );

                pageView = BuildAvailableCommandForObjWFButton(pageView, "CancelButton",
                    "LandPlantList",
                    "LandCode",
                    isVisible: true,
                    isEnabled: true,
                    "Cancel Button Text"
                    );

                pageView = BuildAvailableCommandForObjWFButton(pageView, "OtherButton",
                    "TacFarmDashboard",
                    "TacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Go To Dashboard"
                    );
            }

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> apiInitResponseDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            //GENLOOPobjectWorkflowButtonStart
            //GENIF[buttonType=submit]Start 
            //GENIF[calculatedIsConditionalVisible=true]Start
            //GENREMOVECOMMENTif (apiInitResponseDictionary.ContainsKey("GENVALCamelconditionalVisiblePropertyName") && !(bool)apiInitResponseDictionary["GENVALCamelconditionalVisiblePropertyName"]){
            //GENREMOVECOMMENT    pageView.AvailableCommands = pageView.AvailableCommands.Where(x => x.Description != "GENVALButtonText").ToList();
            //GENREMOVECOMMENT}
            //GENIF[calculatedIsConditionalVisible=true]End
            //GENIF[buttonType=submit]End
            //GENLOOPobjectWorkflowButtonEnd

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
            LandAddPlantInitObjWF.GetInitResponse apiInitResponse,
            LandAddPlantPostModel apiRequestModel)
        {
            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestFlavorCode",
                "Select A Flavor",
                "Lookup",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestFlavorCode.ToString(),
                detailText: "Sample Details Text",
                isFKList: true,
                isFKLookup: true,
                fkObjectName: "Flavor");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestOtherFlavor",
                "Other Flavor",
                "Text",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.RequestOtherFlavor,
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeIntVal",
                "Some Int Val",
                "Number",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeIntVal.ToString(),
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeBigIntVal",
                "Some Big Int Val",
                "Number",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeBigIntVal.ToString(),
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeBitVal",
                "Some Bit Val",
                "Boolean",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeBitVal.ToString(),
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestIsEditAllowed",
                "Is Edit Allowed",
                "Boolean",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.RequestIsEditAllowed.ToString(),
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestIsDeleteAllowed",
                "Is Delete Allowed",
                "Boolean",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.RequestIsDeleteAllowed.ToString(),
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeFloatVal",
                "Some Float Val",
                "Number",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeFloatVal.ToString(),
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeDecimalVal",
                "Some Decimal Val",
                "Number",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeDecimalVal.ToString(),
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeUTCDateTimeVal",
                "Some UTC Date Time Val",
                "DateTime",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeUTCDateTimeVal.ToString("yyyy-MM-ddTHH:mm:ss"),
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeDateVal",
                "Some Date Val",
                "Date",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeDateVal.ToString("yyyy-MM-ddTHH:mm:ss"),
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeMoneyVal",
                "Some Money Val",
                "Number",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeMoneyVal.ToString(),
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeNVarCharVal",
                "Some N Var Char Val",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeNVarCharVal,
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeVarCharVal",
                "Some Secure Var Char Val",
                "Password",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeVarCharVal,
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeLongVarCharVal",
                "Some Long Var Char Val",
                "Text",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.RequestSomeLongVarCharVal,
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeLongNVarCharVal",
                "Some Long N Var Char Val",
                "Text",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.RequestSomeLongNVarCharVal,
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeTextVal",
                "Some Text Val",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeTextVal,
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomePhoneNumber",
                "Some Phone Number",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomePhoneNumber,
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSomeEmailAddress",
                "Some Email Address",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeEmailAddress,
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "requestSampleImageUploadFile",
                "Sample Image Upload",
                "File",
                isVisible: true,
                isRequired: false,
                currentValue: "",
                detailText: "Sample Details Text");

            pageView = await BuildFormField(apiClient, sessionData, pageView, "someImageUrlVal",
                "Some Image Url",
                "Text",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.SomeImageUrlVal,
                detailText: "Sample Details Text"); 

            return pageView;
        }
         
        public async Task<PagePointer> ProcessCommand(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText)
        {
            PagePointer pagePointer = ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            } 

            var initObjWFProcessor = new LandAddPlantInitObjWF();

            LandAddPlantInitObjWF.GetInitResponse apiInitResponse = await initObjWFProcessor.RequestGetInitResponse(apiClient, contextCode);
             
            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>(); 

            if(!navDictionary.ContainsKey("landCode"))
            {
                navDictionary.Add("landCode", contextCode);
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
                        "LandPlantList",
                        Guid.Parse(navDictionary["landCode"].ToString()));

            if (commandText.Equals("CancelButton", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "LandPlantList",
                    Guid.Parse(navDictionary["landCode"].ToString())); 

            if (commandText.Equals("OtherButton", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "TacFarmDashboard",
                    Guid.Parse(navDictionary["tacCode"].ToString())); 

            return pagePointer;
        }  

        public async Task<bool> TryFormSubmit(
            SessionData sessionData,
            APIClient apiClient,
            Guid contextCode,
            LandAddPlantInitObjWF.GetInitResponse apiInitResponse,
            Dictionary<string, object> navDictionary)
        {
            bool result = false;

            LandAddPlantPostModel apiRequestModel = new LandAddPlantPostModel();

            MergeProperties(apiRequestModel, apiInitResponse); 

            string proposedValuesJson = JsonConvert.SerializeObject(sessionData.FormFieldProposedValues);

            MergeProperties(apiRequestModel, proposedValuesJson);

            LandAddPlantPostResponse apiResponse = await PostResponse(apiClient, apiRequestModel, contextCode);

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

        public async Task<LandAddPlantPostResponse> PostResponse(APIClient aPIClient, LandAddPlantPostModel model, Guid contextCode)
        {
            string url = $"/land-add-plant/{contextCode.ToString()}";

            LandAddPlantPostResponse result = await aPIClient.PostAsync<LandAddPlantPostModel, LandAddPlantPostResponse>(url, model);

            return result;
        }
         
         
    }
}
