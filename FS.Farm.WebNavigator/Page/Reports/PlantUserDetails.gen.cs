using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS.Farm.WebNavigator.Page.Reports.Init;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace FS.Farm.WebNavigator.Page.Reports
{
    public class PlantUserDetails : PageBase, IPage
    {
        public PlantUserDetails()
        {
            _pageName = "PlantUserDetails";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, Guid sessionCode, Guid contextCode, string postData = "")
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Plant Details";
            pageView.PageIntroText = "Plant Details page intro text";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new PlantUserDetailsInitReport();

            //  handle report init
            PlantUserDetailsInitReport.PlantUserDetailsGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            PlantUserDetailsListRequest apiRequestModel = new PlantUserDetailsListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            //  handle filter post
            PlantUserDetailsListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            //  handle report row buttons
            pageView = BuildAvailableCommandsForReportRowButtons(pageView, apiResponse);

            //  handle report rows

            string json = JsonConvert.SerializeObject(apiResponse);

            pageView.PageData = json;

            //TODO handle hidden columns

            // handle report buttons
            pageView = BuildAvailableCommandsForReportButtons(pageView);

            return pageView;
        }
        public PageView BuildAvailableCommandsForReportRowButtons(PageView pageView, PlantUserDetailsListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0 ||
                apiResponse.Items.Count > 1)
            {
                return pageView;
            }

            var rowData = apiResponse.Items.ToArray()[0];

            {
                pageView = BuildAvailableCommandForButton(pageView, "updateButtonTextLinkPlantCode",
                    "PlantAddPlant",
                    "updateButtonTextLinkPlantCode",
                    isVisible: false,
                    isEnabled: true,
                    "Update Button Text");
                pageView = BuildAvailableCommandForButton(pageView, "randomPropertyUpdatesLinkPlantCode",
                    "PlantUserDetails",
                    "randomPropertyUpdatesLinkPlantCode",
                    isVisible: true,
                    isEnabled: true,
                    "Random Property Updates");
                pageView = BuildAvailableCommandForButton(pageView, "backToDashboardLinkTacCode",
                    "PlantAddTac",
                    "backToDashboardLinkTacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Back To Dashboard");
                pageView = BuildAvailableCommandForButton(pageView, "testFileDownloadLinkPacCode",
                    "PacUserDetails",
                    "testFileDownloadLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test File Download");
                pageView = BuildAvailableCommandForButton(pageView, "testConditionalAsyncFileDownloadLinkPacCode",
                    "PacUserDetails",
                    "testConditionalAsyncFileDownloadLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Conditional Async File Download");
                pageView = BuildAvailableCommandForButton(pageView, "testAsyncFlowReqLinkPacCode",
                    "PacUserDetails",
                    "testAsyncFlowReqLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Async Flow Req");
                pageView = BuildAvailableCommandForButton(pageView, "testConditionalAsyncFlowReqLinkPacCode",
                    "PacUserDetails",
                    "testConditionalAsyncFlowReqLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Conditional Async Flow Req");
                pageView = BuildAvailableCommandForButton(pageView, "conditionalBtnExampleLinkTacCode",
                    "PlantAddTac",
                    "conditionalBtnExampleLinkTacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Conditional Btn Example");
            }

            return pageView;
        }

        public PageView BuildAvailableCommandsForReportButtons(PageView pageView)
        {
            pageView = BuildAvailableCommandForButton(pageView, "backButton",
                "LandPlantList",
                "LandCode",
                isVisible: true,
                isEnabled: true,
                "Plant List");

            return pageView;
        }

        public PageView BuildAvailableCommandForButton(
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

            var initReportProcessor = new PlantUserDetailsInitReport();

            PlantUserDetailsInitReport.PlantUserDetailsGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            if (!navDictionary.ContainsKey("PlantCode"))
            {
                navDictionary.Add("PlantCode", contextCode);
            }
            //  handle report buttons

            if (commandText == "backButton")
                pagePointer = new PagePointer(
                    "LandPlantList",
                    (Guid)navDictionary["LandCode"]);

            if (pagePointer != null)
            {
                return pagePointer;
            }

            pagePointer = new PagePointer(_pageName, contextCode);

            PlantUserDetailsListRequest apiRequestModel = new PlantUserDetailsListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            PlantUserDetailsListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0 ||
                apiResponse.Items.Count > 1)
            {
                pagePointer = new PagePointer(_pageName, contextCode);

                return pagePointer;
            }

            var rowData = apiResponse.Items.ToArray()[0];
            if (commandText == "updateButtonTextLinkPlantCode")
                pagePointer = new PagePointer(
                    "PlantUserDetails",
                    rowData.UpdateButtonTextLinkPlantCode);
            if (commandText == "randomPropertyUpdatesLinkPlantCode")
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "PlantPlantList",
                    contextCode);
            if (commandText == "backToDashboardLinkTacCode")
                pagePointer = new PagePointer(
                    "TacFarmDashboard",
                    rowData.BackToDashboardLinkTacCode);
            if (commandText == "testFileDownloadLinkPacCode")
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "PlantPacList",
                    contextCode);
            if (commandText == "testConditionalAsyncFileDownloadLinkPacCode")
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "PlantPacList",
                    contextCode);
            if (commandText == "testAsyncFlowReqLinkPacCode")
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "PlantPacList",
                    contextCode);
            if (commandText == "testConditionalAsyncFlowReqLinkPacCode")
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "PlantPacList",
                    contextCode);
            if (commandText == "conditionalBtnExampleLinkTacCode")
                pagePointer = new PagePointer(
                    "TacFarmDashboard",
                    rowData.ConditionalBtnExampleLinkTacCode);
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

        public async Task<PlantUserDetailsListModel> GetResponse(APIClient aPIClient, PlantUserDetailsListRequest model, Guid contextCode)
        {
            string url = $"/plant-user-details/{contextCode.ToString()}";

            // Serialize the model into a query string
            var queryString = ToQueryString(model);

            // Append the query string to the URL
            url = $"{url}?{queryString}";

            PlantUserDetailsListModel result = await aPIClient.GetAsync<PlantUserDetailsListModel>(url);

            return result;
        }

        private string ToQueryString(object obj)
        {
            // Serialize the object to a JSON string with custom settings (camelCase, etc.)
            var json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(), // Adjust case here (optional)
                NullValueHandling = NullValueHandling.Ignore // Ignore null values
            });

            // Deserialize the JSON into a dictionary
            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

            // Convert the dictionary to query string
            var queryString = string.Join("&", dict.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value?.ToString())}"));

            return queryString;
        }

        public class PlantUserDetailsListModel
        {
            [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int PageNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<PlantUserDetailsListModelItem> Items { get; set; }

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
            public PlantUserDetailsListRequest Request { get; set; }

        }

        public class PlantUserDetailsListModelItem
        {
            [Newtonsoft.Json.JsonProperty("flavorName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string FlavorName { get; set; }
            [Newtonsoft.Json.JsonProperty("isDeleteAllowed", Required = Newtonsoft.Json.Required.Always)]
            public bool IsDeleteAllowed { get; set; }
            [Newtonsoft.Json.JsonProperty("isEditAllowed", Required = Newtonsoft.Json.Required.Always)]
            public bool IsEditAllowed { get; set; }
            [Newtonsoft.Json.JsonProperty("otherFlavor", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string OtherFlavor { get; set; }
            [Newtonsoft.Json.JsonProperty("someBigIntVal", Required = Newtonsoft.Json.Required.Always)]
            public long SomeBigIntVal { get; set; }
            [Newtonsoft.Json.JsonProperty("someBitVal", Required = Newtonsoft.Json.Required.Always)]
            public bool SomeBitVal { get; set; }
            [Newtonsoft.Json.JsonProperty("someDateVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeDateVal { get; set; }
            [Newtonsoft.Json.JsonProperty("someDecimalVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeDecimalVal { get; set; }
            [Newtonsoft.Json.JsonProperty("someEmailAddress", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeEmailAddress { get; set; }
            [Newtonsoft.Json.JsonProperty("someFloatVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeFloatVal { get; set; }
            [Newtonsoft.Json.JsonProperty("someIntVal", Required = Newtonsoft.Json.Required.Always)]
            public int SomeIntVal { get; set; }
            [Newtonsoft.Json.JsonProperty("someMoneyVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeMoneyVal { get; set; }
            [Newtonsoft.Json.JsonProperty("someNVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeNVarCharVal { get; set; }
            [Newtonsoft.Json.JsonProperty("somePhoneNumber", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomePhoneNumber { get; set; }
            [Newtonsoft.Json.JsonProperty("someTextVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeTextVal { get; set; }
            [Newtonsoft.Json.JsonProperty("someUniqueidentifierVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid SomeUniqueidentifierVal { get; set; }
            [Newtonsoft.Json.JsonProperty("someUTCDateTimeVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeUTCDateTimeVal { get; set; }
            [Newtonsoft.Json.JsonProperty("someVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeVarCharVal { get; set; }
            [Newtonsoft.Json.JsonProperty("phoneNumConditionalOnIsEditable", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string PhoneNumConditionalOnIsEditable { get; set; }
            [Newtonsoft.Json.JsonProperty("nVarCharAsUrl", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string NVarCharAsUrl { get; set; }
            [Newtonsoft.Json.JsonProperty("updateButtonTextLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid UpdateButtonTextLinkPlantCode { get; set; }
            [Newtonsoft.Json.JsonProperty("randomPropertyUpdatesLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid RandomPropertyUpdatesLinkPlantCode { get; set; }
            [Newtonsoft.Json.JsonProperty("backToDashboardLinkTacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid BackToDashboardLinkTacCode { get; set; }
            [Newtonsoft.Json.JsonProperty("testFileDownloadLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid TestFileDownloadLinkPacCode { get; set; }
            [Newtonsoft.Json.JsonProperty("testConditionalAsyncFileDownloadLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid TestConditionalAsyncFileDownloadLinkPacCode { get; set; }
            [Newtonsoft.Json.JsonProperty("testAsyncFlowReqLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid TestAsyncFlowReqLinkPacCode { get; set; }
            [Newtonsoft.Json.JsonProperty("testConditionalAsyncFlowReqLinkPacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid TestConditionalAsyncFlowReqLinkPacCode { get; set; }
            [Newtonsoft.Json.JsonProperty("conditionalBtnExampleLinkTacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid ConditionalBtnExampleLinkTacCode { get; set; }
            [Newtonsoft.Json.JsonProperty("someImageUrlVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeImageUrlVal { get; set; }
            [Newtonsoft.Json.JsonProperty("isImageUrlAvailable", Required = Newtonsoft.Json.Required.Always)]
            public bool IsImageUrlAvailable { get; set; }
            [Newtonsoft.Json.JsonProperty("someConditionalImageUrlVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalImageUrlVal { get; set; }
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalImageUrl { get; set; }

        }

        public class PlantUserDetailsListRequest
        {

            public System.Guid SomeFilterUniqueIdentifier { get; set; }

            [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.Always)]
            public int PageNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("itemCountPerPage", Required = Newtonsoft.Json.Required.Always)]
            public int ItemCountPerPage { get; set; }

            [Newtonsoft.Json.JsonProperty("orderByColumnName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OrderByColumnName { get; set; }

            [Newtonsoft.Json.JsonProperty("orderByDescending", Required = Newtonsoft.Json.Required.Always)]
            public bool OrderByDescending { get; set; }

            [Newtonsoft.Json.JsonProperty("forceErrorMessage", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
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

