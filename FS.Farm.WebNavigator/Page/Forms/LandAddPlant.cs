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

            LandAddPlantInitObjWF.LandAddPlantGetInitResponse apiInitResponse = await initObjWFProcessor.GetInitResponse(apiClient, contextCode);

            LandAddPlantPostModel apiRequestModel = new LandAddPlantPostModel();

            MergeProperties(apiRequestModel, apiInitResponse);  

            //LandAddPlantPostResponse apiResponse = await PostResponse(apiClient, apiRequestModel, contextCode);

            pageView.PageHeaders = initObjWFProcessor.GetPageHeaders(apiInitResponse);

            pageView.ValidationErrors = sessionData.ValidationErrors;

            pageView = BuildFormFields(sessionData, pageView, apiInitResponse, apiRequestModel);
             

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

            //GENLOOPobjectWorkflowButtonStart
            //GENIF[buttonType=submit]Start 
            //GENIF[calculatedIsConditionalVisible=true]Start
            //GENREMOVECOMMENTif (!apiInitResponse.GENVALCamelconditionalVisiblePropertyName){
            //GENREMOVECOMMENT    pageView.AvailableCommands = pageView.AvailableCommands.Where(x => x.Description != GENVALButtonText).ToList();
            //GENREMOVECOMMENT}
            //GENIF[calculatedIsConditionalVisible=true]End
            //GENIF[buttonType=submit]End
            //GENLOOPobjectWorkflowButtonEnd

            pageView.PageTable = null; 

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

            if(dataType.Equals("date",StringComparison.OrdinalIgnoreCase))
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

            if (dataType.Equals("datetime", StringComparison.OrdinalIgnoreCase))
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
                DataType = dataType,
                DetailText = detailText,
                CurrentValue = currentValue,
                ProposedValue = proposedValue,
                isRequiredField = isRequired,
                ValidationErrorText = validationError
            };

            pageView.FormFields.Add(formField);

            return pageView;
        }

        public PageView BuildFormFields(SessionData sessionData, 
            PageView pageView,
            LandAddPlantInitObjWF.LandAddPlantGetInitResponse apiInitResponse,
            LandAddPlantPostModel apiRequestModel)
        {
            pageView = BuildFormField(sessionData,pageView, "requestFlavorCode",
                "Select A Flavor",
                "Guid",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestFlavorCode.ToString(),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestOtherFlavor",
                "Other Flavor",
                "Text",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.RequestOtherFlavor,
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeIntVal",
                "Some Int Val",
                "Number",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeIntVal.ToString(),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeBigIntVal",
                "Some Big Int Val",
                "Number",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeBigIntVal.ToString(),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeBitVal",
                "Some Bit Val",
                "Boolean",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeBitVal.ToString(),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestIsEditAllowed",
                "Is Edit Allowed",
                "Boolean",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.RequestIsEditAllowed.ToString(),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestIsDeleteAllowed",
                "Is Delete Allowed",
                "Boolean",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.RequestIsDeleteAllowed.ToString(),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeFloatVal",
                "Some Float Val",
                "Number",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeFloatVal.ToString(),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeDecimalVal",
                "Some Decimal Val",
                "Number",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeDecimalVal.ToString(),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeUTCDateTimeVal",
                "Some UTC Date Time Val",
                "DateTime",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeUTCDateTimeVal.ToString("yyyy-MM-ddTHH:mm:ss"),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeDateVal",
                "Some Date Val",
                "Date",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeDateVal.ToString("yyyy-MM-ddTHH:mm:ss"),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeMoneyVal",
                "Some Money Val",
                "Number",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeMoneyVal.ToString(),
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeNVarCharVal",
                "Some N Var Char Val",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeNVarCharVal,
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeVarCharVal",
                "Some Secure Var Char Val",
                "Password",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeVarCharVal,
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeLongVarCharVal",
                "Some Long Var Char Val",
                "Text",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.RequestSomeLongVarCharVal,
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeLongNVarCharVal",
                "Some Long N Var Char Val",
                "Text",
                isVisible: true,
                isRequired: false,
                currentValue: apiRequestModel.RequestSomeLongNVarCharVal,
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeTextVal",
                "Some Text Val",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeTextVal,
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomePhoneNumber",
                "Some Phone Number",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomePhoneNumber,
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSomeEmailAddress",
                "Some Email Address",
                "Text",
                isVisible: true,
                isRequired: true,
                currentValue: apiRequestModel.RequestSomeEmailAddress,
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "requestSampleImageUploadFile",
                "Sample Image Upload",
                "File",
                isVisible: true,
                isRequired: false,
                currentValue: "",
                detailText: "Sample Details Text");

            pageView = BuildFormField(sessionData, pageView, "someImageUrlVal",
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

            LandAddPlantInitObjWF.LandAddPlantGetInitResponse apiInitResponse = await initObjWFProcessor.GetInitResponse(apiClient, contextCode);
             
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
            LandAddPlantInitObjWF.LandAddPlantGetInitResponse apiInitResponse,
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

        public class LandAddPlantPostResponse
        {
            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("outputFlavorCode", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid OutputFlavorCode { get; set; }

            [Newtonsoft.Json.JsonProperty("outputOtherFlavor", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputOtherFlavor { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeIntVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int OutputSomeIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeBigIntVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public long OutputSomeBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeBitVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool OutputSomeBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputIsEditAllowed", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool OutputIsEditAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("outputIsDeleteAllowed", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool OutputIsDeleteAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeFloatVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeDecimalVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeUTCDateTimeVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public DateTime OutputSomeUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeDateVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public DateTime OutputSomeDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeMoneyVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeNVarCharVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeVarCharVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomePhoneNumber", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomePhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("landCode", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid LandCode { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeTextVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("plantCode", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid PlantCode { get; set; }


            [Newtonsoft.Json.JsonProperty("outputSomeEmailAddress", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeEmailAddress { get; set; } //outputSomeEmailAddress


            [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<ValidationError> ValidationErrors { get; set; } 

        }
         
        public class LandAddPlantPostModel
        {
            [Newtonsoft.Json.JsonProperty("requestFlavorCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid RequestFlavorCode { get; set; }

            [Newtonsoft.Json.JsonProperty("requestOtherFlavor", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestOtherFlavor { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeIntVal", Required = Newtonsoft.Json.Required.Always)]
            public int RequestSomeIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeBigIntVal", Required = Newtonsoft.Json.Required.Always)]
            public long RequestSomeBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeBitVal", Required = Newtonsoft.Json.Required.Always)]
            public bool RequestSomeBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestIsEditAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool RequestIsEditAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("requestIsDeleteAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool RequestIsDeleteAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeFloatVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RequestSomeFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeDecimalVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RequestSomeDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeUTCDateTimeVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public DateTime RequestSomeUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeDateVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public DateTime RequestSomeDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeMoneyVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RequestSomeMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeNVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RequestSomeNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RequestSomeVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeTextVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RequestSomeTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomePhoneNumber", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RequestSomePhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeEmailAddress", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RequestSomeEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSampleImageUploadFile", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSampleImageUploadFile { get; set; }

            [Newtonsoft.Json.JsonProperty("someImageUrlVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeImageUrlVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeLongNVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeLongNVarCharVal { get; set; } //requestSomeLongNVarCharVal


            [Newtonsoft.Json.JsonProperty("requestSomeLongVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string RequestSomeLongVarCharVal { get; set; } 


        } 
         
    }
}
