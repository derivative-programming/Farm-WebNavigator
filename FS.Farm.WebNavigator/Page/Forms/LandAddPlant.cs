using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FS.Farm.WebNavigator.Page.Forms.Init.LandAddPlantInitObjWF;

namespace FS.Farm.WebNavigator.Page.Forms
{
    public class LandAddPlant : PageBase, IPage
    {
        public LandAddPlant()
        {
            _pageName = "LandAddPlant";
        }
        public PageView BuildPageView(Guid sessionCode, Guid contextCode)
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Add PlantAdd plant form title text"; 
            pageView.PageIntroText = "Add plant intro text.Add plant form intro text";  
            pageView.PageFooterText = "Add plant form footer text";  

            pageView = AddDefaultAvailableCommands(pageView);

            //TODO handle form init


            //TODO handle return of form


            //TODO handle hidden controls

            // handle objwf buttons
//endset
            pageView = HandleButton(pageView, "SubmitButton",
                "LandAddPlant", 
                "LandCode",
                isVisible: true,
                isEnabled: true,
                "OK Button Text");

            pageView = HandleButton(pageView, "CancelButton",
                "LandPlantList",
                "LandCode",
                isVisible: true,
                isEnabled: true,
                "Cancel Button Text");

            pageView = HandleButton(pageView, "OtherButton",
                "TacFarmDashboard",
                "TacCode",
                isVisible: true,
                isEnabled: true,
                "Go To Dashboard");
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
            {
                //same page and context code
            }

            if (commandText == "CancelButton")
            {
                pagePointer.PageName = "LandPlantList";
                pagePointer.ContextCode = Guid.Empty; //TODO set context code
            }

            if (commandText == "OtherButton")
            {
                pagePointer.PageName = "TacFarmDashboard";
                pagePointer.ContextCode = Guid.Empty; //TODO set context code
            }

            return pagePointer;
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

            [Newtonsoft.Json.JsonProperty("outputFlavorCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid OutputFlavorCode { get; set; }

            [Newtonsoft.Json.JsonProperty("outputOtherFlavor", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputOtherFlavor { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeIntVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int OutputSomeIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeBigIntVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public long OutputSomeBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeBitVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool OutputSomeBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputIsEditAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool OutputIsEditAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("outputIsDeleteAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool OutputIsDeleteAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeFloatVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeDecimalVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeUTCDateTimeVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeDateVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeMoneyVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeNVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeTextVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomePhoneNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomePhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("outputSomeEmailAddress", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OutputSomeEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("landCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid LandCode { get; set; }

            [Newtonsoft.Json.JsonProperty("plantCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid PlantCode { get; set; }

            [Newtonsoft.Json.JsonProperty("validationError", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<ValidationError> ValidationError { get; set; } 

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
            public string RequestSomeUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeDateVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RequestSomeDateVal { get; set; }

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
            public string RequestSomeLongNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("requestSomeLongVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
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
