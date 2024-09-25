using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS.Farm.WebNavigator.Page.Reports.Init;

namespace FS.Farm.WebNavigator.Page.Reports
{
    public class LandPlantList : PageBase, IPage
    {
        public LandPlantList()
        {
            _pageName = "LandPlantList";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, Guid sessionCode, Guid contextCode, string postData = "")
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Plant List";
            pageView.PageIntroText = "A list of plants on the land";  
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new LandPlantListInitReport();

            //TODO handle report init
            LandPlantListInitReport.LandPlantListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            LandPlantListListRequest apiRequestModel = new LandPlantListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            //TODO handle filter post
            LandPlantListListModel apiResponse = await PostResponse(apiClient, apiRequestModel, contextCode);


            //TODO handle report row buttons
            pageView = HandleReportRowButtons(pageView, apiResponse);

            //TODO handle report rows

            //TODO handle hidden columns

            // handle report buttons
            pageView = HandleReportButtons(pageView); 

            return pageView;
        }
        public PageView HandleReportRowButtons(PageView pageView, LandPlantListListModel apiResponse)
        {
            if (apiResponse == null || 
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0 ||
                apiResponse.Items.Count > 1)
            {
                return pageView;
            }

            var rowData = apiResponse.Items.ToArray()[0];

            pageView = HandleButton(pageView, "updateLinkPlantCode",
                "PlantUserDetails",
                "updateLinkPlantCode",
                isVisible: false,
                isEnabled: true,
                "Update");

            pageView = HandleButton(pageView, "deleteAsyncButtonLinkPlantCode",
                "PlantUserDetails",
                "deleteAsyncButtonLinkPlantCode",
                isVisible: true,
                isEnabled: true,
                "Delete");

            pageView = HandleButton(pageView, "detailsLinkPlantCode",
                "LandAddPlant",
                "detailsLinkPlantCode",
                isVisible: true,
                isEnabled: true,
                "Details");

            pageView = HandleButton(pageView, "testFileDownloadLinkPacCode",
                "LandAddPlant",
                "testFileDownloadLinkPacCode",
                isVisible: true,
                isEnabled: true,
                "Test File Download");

            pageView = HandleButton(pageView, "testConditionalFileDownloadLinkPacCode",
                "LandAddPlant",
                "testConditionalFileDownloadLinkPacCode",
                isVisible: true,
                isEnabled: true,
                "Test Conditional File Download",
                conditionallyVisible: rowData.IsEditAllowed
                );

            pageView = HandleButton(pageView, "testAsyncFlowReqLinkPacCode",
                "LandAddPlant",
                "testAsyncFlowReqLinkPacCode",
                isVisible: true,
                isEnabled: true,
                "Test Async Flow Req");

            pageView = HandleButton(pageView, "testConditionalAsyncFlowReqLinkPacCode",
                "LandPlantList",
                "testConditionalAsyncFlowReqLinkPacCode",
                isVisible: true,
                isEnabled: true,
                "Test Conditional Async Flow Req",
                conditionallyVisible: rowData.IsEditAllowed
                );

            pageView = HandleButton(pageView, "conditionalBtnExampleLinkPlantCode",
                "PlantUserDetails",
                "conditionalBtnExampleLinkPlantCode",
                isVisible: true,
                isEnabled: true,
                "Conditional Btn Example",
                conditionallyVisible: rowData.IsEditAllowed
                );

            return pageView;
        }

        public PageView HandleReportButtons(PageView pageView)
        {
            pageView = HandleButton(pageView, "backButton",
                "TacFarmDashboard",
                "TacCode",
                isVisible: true,
                isEnabled: true,
                "Farm Dashboard");

            pageView = HandleButton(pageView, "addButton",
                "LandAddPlant",
                "LandCode",
                isVisible: true,
                isEnabled: true,
                "Add A Plant");

            pageView = HandleButton(pageView, "otherAddButton",
                "LandAddPlant",
                "LandCode",
                isVisible: true,
                isEnabled: true,
                "Other Add Button");

            return pageView;
        }


        public PageView HandleButton(
            PageView pageView,
            string name,
            string destinationPageName,
            string codeName,
            bool isVisible,
            bool isEnabled,
            string buttonText,
            bool conditionallyVisible = true)
        {
            if(!isVisible)
                return pageView;

            if(!isEnabled)
                return pageView;

            if (!conditionallyVisible)
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


            //TODO handle report row buttons 

            //  handle report buttons
            if (commandText == "backButton")
                pagePointer = ProcessButtonCommand(
                    "backButton",
                    "TacFarmDashboard",
                    "TacCode");

            if (commandText == "addButton")
                pagePointer = ProcessButtonCommand(
                    "addButton",
                    "LandAddPlant",
                    "LandCode"); 

            if (commandText == "otherAddButton")
                pagePointer = ProcessButtonCommand(
                    "otherAddButton",
                    "LandAddPlant",
                    "LandCode"); 

            pagePointer = new PagePointer(_pageName, contextCode);

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


        public async Task<LandPlantListListModel> PostResponse(APIClient aPIClient, LandPlantListListRequest model, Guid contextCode)
        {
            string url = $"/land-plant-list/{contextCode.ToString()}";

            LandPlantListListModel result = await aPIClient.PostAsync<LandPlantListListRequest, LandPlantListListModel>(url, model);

            return result;
        }

        public class LandPlantListListModel
        {
            [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int PageNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<LandPlantListListModelItem> Items { get; set; }

            [Newtonsoft.Json.JsonProperty("itemCountPerPage", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int ItemCountPerPage { get; set; }

            [Newtonsoft.Json.JsonProperty("orderByColumnName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OrderByColumnName { get; set; }

            [Newtonsoft.Json.JsonProperty("orderByDescending", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool OrderByDescending { get; set; }

            [Newtonsoft.Json.JsonProperty("success", Required = Newtonsoft.Json.Required.Always)]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("recordsTotal", Required = Newtonsoft.Json.Required.Always)]
            public int RecordsTotal { get; set; }

            [Newtonsoft.Json.JsonProperty("recordsFiltered", Required = Newtonsoft.Json.Required.Always)]
            public int RecordsFiltered { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

            [Newtonsoft.Json.JsonProperty("appVersion", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string AppVersion { get; set; }

            [Newtonsoft.Json.JsonProperty("request", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public LandPlantListListRequest Request { get; set; } 

        }

        
        public class LandPlantListListModelItem
        {
            [Newtonsoft.Json.JsonProperty("plantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid PlantCode { get; set; }

            [Newtonsoft.Json.JsonProperty("someIntVal", Required = Newtonsoft.Json.Required.Always)]
            public int SomeIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalIntVal", Required = Newtonsoft.Json.Required.Always)]
            public int SomeConditionalIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someBigIntVal", Required = Newtonsoft.Json.Required.Always)]
            public long SomeBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalBigIntVal", Required = Newtonsoft.Json.Required.Always)]
            public long SomeConditionalBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someBitVal", Required = Newtonsoft.Json.Required.Always)]
            public bool SomeBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalBitVal", Required = Newtonsoft.Json.Required.Always)]
            public bool SomeConditionalBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("isEditAllowed", Required = Newtonsoft.Json.Required.Always)]
            public bool IsEditAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("isDeleteAllowed", Required = Newtonsoft.Json.Required.Always)]
            public bool IsDeleteAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("someFloatVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalFloatVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someDecimalVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalDecimalVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someUTCDateTimeVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalUTCDateTimeVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someDateVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalDateVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someMoneyVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalMoneyVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someNVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalNVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someTextVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalTextVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("somePhoneNumber", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomePhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalPhoneNumber", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalPhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("someEmailAddress", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalEmailAddress", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("flavorName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string FlavorName { get; set; }

            [Newtonsoft.Json.JsonProperty("flavorCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid FlavorCode { get; set; }

            [Newtonsoft.Json.JsonProperty("someIntConditionalOnDeletable", Required = Newtonsoft.Json.Required.Always)]
            public int SomeIntConditionalOnDeletable { get; set; }

            [Newtonsoft.Json.JsonProperty("nVarCharAsUrl", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string NVarCharAsUrl { get; set; }

            [Newtonsoft.Json.JsonProperty("nVarCharConditionalAsUrl", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string NVarCharConditionalAsUrl { get; set; }

            [Newtonsoft.Json.JsonProperty("updateLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid UpdateLinkPlantCode { get; set; }

            [Newtonsoft.Json.JsonProperty("deleteAsyncButtonLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid DeleteAsyncButtonLinkPlantCode { get; set; }

            [Newtonsoft.Json.JsonProperty("detailsLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid DetailsLinkPlantCode { get; set; }

            [Newtonsoft.Json.JsonProperty("testFileDownloadLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid TestFileDownloadLinkPacCode { get; set; }

            [Newtonsoft.Json.JsonProperty("testConditionalFileDownloadLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid TestConditionalFileDownloadLinkPacCode { get; set; }

            [Newtonsoft.Json.JsonProperty("testAsyncFlowReqLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid TestAsyncFlowReqLinkPacCode { get; set; }

            [Newtonsoft.Json.JsonProperty("testConditionalAsyncFlowReqLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid TestConditionalAsyncFlowReqLinkPacCode { get; set; }

            [Newtonsoft.Json.JsonProperty("conditionalBtnExampleLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid ConditionalBtnExampleLinkPlantCode { get; set; }

            [Newtonsoft.Json.JsonProperty("isImageUrlAvailable", Required = Newtonsoft.Json.Required.Always)]
            public bool IsImageUrlAvailable { get; set; }

            [Newtonsoft.Json.JsonProperty("someImageUrlVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeImageUrlVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalImageUrl", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalImageUrl { get; set; } 

        }

        
        public class LandPlantListListRequest
        {
            [Newtonsoft.Json.JsonProperty("flavorFilterCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid FlavorFilterCode { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterIntVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int SomeFilterIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterBigIntVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public long SomeFilterBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterFloatVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterBitVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool SomeFilterBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("isFilterEditAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool IsFilterEditAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("isFilterDeleteAllowed", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool IsFilterDeleteAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterDecimalVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someMinUTCDateTimeVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeMinUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someMinDateVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeMinDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterMoneyVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterNVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterVarCharVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterTextVal", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterPhoneNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterPhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterEmailAddress", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterUniqueIdentifier", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid SomeFilterUniqueIdentifier { get; set; }

            [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.Always)]
            public int PageNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("itemCountPerPage", Required = Newtonsoft.Json.Required.Always)]
            public int ItemCountPerPage { get; set; }

            [Newtonsoft.Json.JsonProperty("orderByColumnName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OrderByColumnName { get; set; }

            [Newtonsoft.Json.JsonProperty("orderByDescending", Required = Newtonsoft.Json.Required.Always)]
            public bool OrderByDescending { get; set; }

            [Newtonsoft.Json.JsonProperty("forceErrorMessage", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string ForceErrorMessage { get; set; } 

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
