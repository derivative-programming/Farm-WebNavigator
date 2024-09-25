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
        public async Task<PageView> BuildPageView(APIClient apiClient, Guid sessionCode, Guid contextCode, string commandText = "", string postData = "")
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

            //default values, can't override
            apiRequestModel.ForceErrorMessage = "";
            apiRequestModel.ItemCountPerPage = 5;

            apiRequestModel.PageNumber = 1;
            apiRequestModel.OrderByColumnName = "";
            apiRequestModel.OrderByDescending = false;

            //  handle filter post
            PlantUserDetailsListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            pageView.PageHeaders = initReportProcessor.GetPageHeaders(apiInitResponse);

            pageView = BuildTableHeaders(pageView);

            pageView = BuildTableData(pageView, apiResponse);

            pageView = BuildAvailableCommandsForReportSort(pageView, apiResponse);

            //  handle report row buttons
            pageView = BuildAvailableCommandsForReportRowButtons(pageView, apiResponse);

            //  handle report rows

            string json = JsonConvert.SerializeObject(apiResponse);

            pageView.PageData = json;

            //TODO handle hidden columns

            //TODO handle paging

            //TODO handle sorting

            //TODO handle filtering

            // handle report buttons
            pageView = BuildAvailableCommandsForReportButtons(pageView);

            return pageView;
        }

        public PageView BuildPageHeaders(PageView pageView)
        {
            return pageView;
        }

        public PageView BuildTableHeaders(PageView pageView)
        {
            {
                pageView = BuildTableHeader(pageView, "flavorName",
                    isVisible: true,
                    "Name");
                pageView = BuildTableHeader(pageView, "isDeleteAllowed",
                    isVisible: true,
                    "Is Delete Allowed");
                pageView = BuildTableHeader(pageView, "isEditAllowed",
                    isVisible: true,
                    "Is Edit Allowed");
                pageView = BuildTableHeader(pageView, "otherFlavor",
                    isVisible: true,
                    "Other Flavor");
                pageView = BuildTableHeader(pageView, "someBigIntVal",
                    isVisible: true,
                    "Some Big Int Val");
                pageView = BuildTableHeader(pageView, "someBitVal",
                    isVisible: true,
                    "Some Bit Val");
                pageView = BuildTableHeader(pageView, "someDateVal",
                    isVisible: true,
                    "Some Date Val");
                pageView = BuildTableHeader(pageView, "someDecimalVal",
                    isVisible: true,
                    "Some Decimal Val");
                pageView = BuildTableHeader(pageView, "someEmailAddress",
                    isVisible: true,
                    "Some Email Address");
                pageView = BuildTableHeader(pageView, "someFloatVal",
                    isVisible: true,
                    "Some Float Val");
                pageView = BuildTableHeader(pageView, "someIntVal",
                    isVisible: true,
                    "Some Int Val");
                pageView = BuildTableHeader(pageView, "someMoneyVal",
                    isVisible: true,
                    "Some Money Val");
                pageView = BuildTableHeader(pageView, "someNVarCharVal",
                    isVisible: true,
                    "Some N Var Char Val");
                pageView = BuildTableHeader(pageView, "somePhoneNumber",
                    isVisible: true,
                    "Some Phone Number");
                pageView = BuildTableHeader(pageView, "someTextVal",
                    isVisible: true,
                    "Some Text Val");
                //SomeUniqueidentifierVal
                pageView = BuildTableHeader(pageView, "someUTCDateTimeVal",
                    isVisible: true,
                    "Some UTC Date Time Val");
                pageView = BuildTableHeader(pageView, "someVarCharVal",
                    isVisible: true,
                    "Some Var Char Val");
                pageView = BuildTableHeader(pageView, "phoneNumConditionalOnIsEditable",
                    isVisible: true,
                    "Conditional Column");
                pageView = BuildTableHeader(pageView, "nVarCharAsUrl",
                    isVisible: true,
                    "N Var Char As Url");
                //updateButtonTextLinkPlantCode
                //randomPropertyUpdatesLinkPlantCode
                //backToDashboardLinkTacCode
                //testFileDownloadLinkPacCode
                //testConditionalAsyncFileDownloadLinkPacCode
                //testAsyncFlowReqLinkPacCode
                //testConditionalAsyncFlowReqLinkPacCode
                //conditionalBtnExampleLinkTacCode
                pageView = BuildTableHeader(pageView, "someImageUrlVal",
                    isVisible: true,
                    "Some Image Url Val");
                pageView = BuildTableHeader(pageView, "isImageUrlAvailable",
                    isVisible: true,
                    "Is Image Url Available");
                pageView = BuildTableHeader(pageView, "someConditionalImageUrlVal",
                    isVisible: true,
                    "Some Conditional Image Url Val");
            }

            return pageView;
        }

        public PageView BuildTableData(PageView pageView, PlantUserDetailsListModel apiResponse)
        {
            List<Dictionary<string,string>> tableData = new List<Dictionary<string, string>>();

            foreach(var rowData in apiResponse.Items)
            {
                tableData.Add(BuildTableDataRow(rowData));
            }

            pageView.TableData = tableData;

            return pageView;
        }

        public Dictionary<string, string> BuildTableDataRow(PlantUserDetailsListModelItem rowData)
        {
            Dictionary<string,string> keyValuePairs = new Dictionary<string, string>();

            {
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "flavorName",
                    isVisible: true,
                    value: rowData.FlavorName.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "isDeleteAllowed",
                    isVisible: true,
                    value: rowData.IsDeleteAllowed.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "isEditAllowed",
                    isVisible: true,
                    value: rowData.IsEditAllowed.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "otherFlavor",
                    isVisible: true,
                    value: rowData.OtherFlavor.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someBigIntVal",
                    isVisible: true,
                    value: rowData.SomeBigIntVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someBitVal",
                    isVisible: true,
                    value: rowData.SomeBitVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someDateVal",
                    isVisible: true,
                    value: rowData.SomeDateVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someDecimalVal",
                    isVisible: true,
                    value: rowData.SomeDecimalVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someEmailAddress",
                    isVisible: true,
                    value: rowData.SomeEmailAddress.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someFloatVal",
                    isVisible: true,
                    value: rowData.SomeFloatVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someIntVal",
                    isVisible: true,
                    value: rowData.SomeIntVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someMoneyVal",
                    isVisible: true,
                    value: rowData.SomeMoneyVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someNVarCharVal",
                    isVisible: true,
                    value: rowData.SomeNVarCharVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "somePhoneNumber",
                    isVisible: true,
                    value: rowData.SomePhoneNumber.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someTextVal",
                    isVisible: true,
                    value: rowData.SomeTextVal.ToString());
                //SomeUniqueidentifierVal
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someUTCDateTimeVal",
                    isVisible: true,
                    value: rowData.SomeUTCDateTimeVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someVarCharVal",
                    isVisible: true,
                    value: rowData.SomeVarCharVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "phoneNumConditionalOnIsEditable",
                    isVisible: true,
                    value: rowData.PhoneNumConditionalOnIsEditable.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "nVarCharAsUrl",
                    isVisible: true,
                    value: rowData.NVarCharAsUrl.ToString());
                //updateButtonTextLinkPlantCode
                //randomPropertyUpdatesLinkPlantCode
                //backToDashboardLinkTacCode
                //testFileDownloadLinkPacCode
                //testConditionalAsyncFileDownloadLinkPacCode
                //testAsyncFlowReqLinkPacCode
                //testConditionalAsyncFlowReqLinkPacCode
                //conditionalBtnExampleLinkTacCode
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someImageUrlVal",
                    isVisible: true,
                    value: rowData.SomeImageUrlVal.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "isImageUrlAvailable",
                    isVisible: true,
                    value: rowData.IsImageUrlAvailable.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalImageUrlVal",
                    isVisible: true,
                    value: rowData.SomeConditionalImageUrlVal.ToString());
            }

            return keyValuePairs;
        }

        public PageView BuildAvailableCommandsForReportSort(PageView pageView, PlantUserDetailsListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count < 2)
            {
                return pageView;
            }

            {
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "flavorName",
                    isVisible: true,
                    "Name");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "isDeleteAllowed",
                    isVisible: true,
                    "Is Delete Allowed");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "isEditAllowed",
                    isVisible: true,
                    "Is Edit Allowed");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "otherFlavor",
                    isVisible: true,
                    "Other Flavor");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someBigIntVal",
                    isVisible: true,
                    "Some Big Int Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someBitVal",
                    isVisible: true,
                    "Some Bit Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someDateVal",
                    isVisible: true,
                    "Some Date Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someDecimalVal",
                    isVisible: true,
                    "Some Decimal Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someEmailAddress",
                    isVisible: true,
                    "Some Email Address");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someFloatVal",
                    isVisible: true,
                    "Some Float Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someIntVal",
                    isVisible: true,
                    "Some Int Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someMoneyVal",
                    isVisible: true,
                    "Some Money Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someNVarCharVal",
                    isVisible: true,
                    "Some N Var Char Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "somePhoneNumber",
                    isVisible: true,
                    "Some Phone Number");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someTextVal",
                    isVisible: true,
                    "Some Text Val");
                //SomeUniqueidentifierVal
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someUTCDateTimeVal",
                    isVisible: true,
                    "Some UTC Date Time Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someVarCharVal",
                    isVisible: true,
                    "Some Var Char Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "phoneNumConditionalOnIsEditable",
                    isVisible: true,
                    "Conditional Column");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "nVarCharAsUrl",
                    isVisible: true,
                    "N Var Char As Url");
                //updateButtonTextLinkPlantCode
                //randomPropertyUpdatesLinkPlantCode
                //backToDashboardLinkTacCode
                //testFileDownloadLinkPacCode
                //testConditionalAsyncFileDownloadLinkPacCode
                //testAsyncFlowReqLinkPacCode
                //testConditionalAsyncFlowReqLinkPacCode
                //conditionalBtnExampleLinkTacCode
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someImageUrlVal",
                    isVisible: true,
                    "Some Image Url Val");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "isImageUrlAvailable",
                    isVisible: true,
                    "Is Image Url Available");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalImageUrlVal",
                    isVisible: true,
                    "Some Conditional Image Url Val");
            }

            return pageView;
        }
        public PageView BuildAvailableCommandsForReportPaging(PageView pageView, PlantUserDetailsListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0)
            {
                return pageView;
            }

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
                pageView = BuildAvailableCommandForReportButton(pageView, "updateButtonTextLinkPlantCode",
                    "PlantAddPlant",
                    "updateButtonTextLinkPlantCode",
                    isVisible: false,
                    isEnabled: true,
                    "Update Button Text");
                pageView = BuildAvailableCommandForReportButton(pageView, "randomPropertyUpdatesLinkPlantCode",
                    "PlantUserDetails",
                    "randomPropertyUpdatesLinkPlantCode",
                    isVisible: true,
                    isEnabled: true,
                    "Random Property Updates");
                pageView = BuildAvailableCommandForReportButton(pageView, "backToDashboardLinkTacCode",
                    "PlantAddTac",
                    "backToDashboardLinkTacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Back To Dashboard");
                pageView = BuildAvailableCommandForReportButton(pageView, "testFileDownloadLinkPacCode",
                    "PacUserDetails",
                    "testFileDownloadLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test File Download");
                pageView = BuildAvailableCommandForReportButton(pageView, "testConditionalAsyncFileDownloadLinkPacCode",
                    "PacUserDetails",
                    "testConditionalAsyncFileDownloadLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Conditional Async File Download");
                pageView = BuildAvailableCommandForReportButton(pageView, "testAsyncFlowReqLinkPacCode",
                    "PacUserDetails",
                    "testAsyncFlowReqLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Async Flow Req");
                pageView = BuildAvailableCommandForReportButton(pageView, "testConditionalAsyncFlowReqLinkPacCode",
                    "PacUserDetails",
                    "testConditionalAsyncFlowReqLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Conditional Async Flow Req");
                pageView = BuildAvailableCommandForReportButton(pageView, "conditionalBtnExampleLinkTacCode",
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
            pageView = BuildAvailableCommandForReportButton(pageView, "backButton",
                "LandPlantList",
                "LandCode",
                isVisible: true,
                isEnabled: true,
                "Plant List");

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

            if (commandText.Equals("backButton",StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "LandPlantList",
                    Guid.Parse(navDictionary["landCode"].ToString()));

            if (pagePointer != null)
            {
                return pagePointer;
            }

            pagePointer = new PagePointer(_pageName, contextCode);

            if(commandText.StartsWith("sortOnColumn:",StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }

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
            if (commandText.Equals("updateButtonTextLinkPlantCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "PlantUserDetails",
                    rowData.UpdateButtonTextLinkPlantCode);
            if (commandText.Equals("randomPropertyUpdatesLinkPlantCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "PlantPlantList",
                    contextCode);
            if (commandText.Equals("backToDashboardLinkTacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "TacFarmDashboard",
                    rowData.BackToDashboardLinkTacCode);
            if (commandText.Equals("testFileDownloadLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "PlantPacList",
                    contextCode);
            if (commandText.Equals("testConditionalAsyncFileDownloadLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "PlantPacList",
                    contextCode);
            if (commandText.Equals("testAsyncFlowReqLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "PlantPacList",
                    contextCode);
            if (commandText.Equals("testConditionalAsyncFlowReqLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "PlantPacList",
                    contextCode);
            if (commandText.Equals("conditionalBtnExampleLinkTacCode", StringComparison.OrdinalIgnoreCase))
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

            [Newtonsoft.Json.JsonProperty("orderByColumnName", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string OrderByColumnName { get; set; }

            [Newtonsoft.Json.JsonProperty("orderByDescending", Required = Newtonsoft.Json.Required.Always)]
            public bool OrderByDescending { get; set; }

            [Newtonsoft.Json.JsonProperty("forceErrorMessage", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string ForceErrorMessage { get; set; }

        }

    }
}

