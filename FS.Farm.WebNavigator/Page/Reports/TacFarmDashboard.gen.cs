using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FS.Farm.WebNavigator.Page.Reports.Init;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace FS.Farm.WebNavigator.Page.Reports
{
    public class TacFarmDashboard : PageBase, IPage
    {
        public TacFarmDashboard()
        {
            _pageName = "TacFarmDashboard";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText = "", string postData = "")
        {
            var pageView = new PageView();

            if(!sessionData.PageName.Equals("TacFarmDashboard",StringComparison.OrdinalIgnoreCase))
            {
                //new page, clear filters
                sessionData.Filters.Clear();
            }

            pageView.PageTitleText = "Farm Dashboard";
            pageView.PageIntroText = "Farm Dashboard page intro text";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new TacFarmDashboardInitReport();

            //  handle report init
            TacFarmDashboardInitReport.TacFarmDashboardGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            TacFarmDashboardListRequest apiRequestModel = new TacFarmDashboardListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            if (commandText.StartsWith("ClearFilters", StringComparison.OrdinalIgnoreCase))
            {
                sessionData.Filters.Clear();
            }
            if (commandText.StartsWith("pageNumber:", StringComparison.OrdinalIgnoreCase))
            {
                string pageNumberValue = commandText.Split(':')[1];

                if(pageNumberValue.Trim().Length == 0)
                {
                    if (sessionData.Filters.ContainsKey("pageNumber"))
                        sessionData.Filters.Remove("pageNumber");
                }
                else
                {
                    if (sessionData.Filters.ContainsKey("pageNumber"))
                    {
                        sessionData.Filters["pageNumber"] = pageNumberValue;
                    }
                    else
                    {
                        sessionData.Filters.Add("pageNumber", pageNumberValue);
                    }
                }
            }
            if (commandText.StartsWith("rowNumber:", StringComparison.OrdinalIgnoreCase))
            {
                string rowNumberValue = commandText.Split(':')[1];

                if (rowNumberValue.Trim().Length == 0)
                {
                    if (sessionData.Filters.ContainsKey("rowNumber"))
                        sessionData.Filters.Remove("rowNumber");
                }
                else
                {
                    if (sessionData.Filters.ContainsKey("rowNumber"))
                    {
                        sessionData.Filters["rowNumber"] = rowNumberValue;
                    }
                    else
                    {
                        sessionData.Filters.Add("rowNumber", rowNumberValue);
                    }
                }

            }

            MergeProperties(apiRequestModel, postData);

            //default values, can't override
            apiRequestModel.ForceErrorMessage = "";
            apiRequestModel.ItemCountPerPage = 5;

            apiRequestModel.PageNumber = 1;
            apiRequestModel.OrderByColumnName = "";
            apiRequestModel.OrderByDescending = false;

            //  handle filter post
            TacFarmDashboardListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            pageView.PageHeaders = initReportProcessor.GetPageHeaders(apiInitResponse);

            pageView = BuildTableHeaders(pageView);

            pageView = BuildTableData(sessionData, pageView, apiResponse);

            pageView = BuildAvailableCommandsForReportSort(pageView, apiResponse);

            //  handle report row buttons
            pageView = BuildAvailableCommandsForReportRowButtons(pageView, apiResponse);

            pageView.TableFilters = sessionData.Filters;

            //  handle report rows

            //string json = JsonConvert.SerializeObject(apiResponse);

            //pageView.PageData = json;

            //TODO handle hidden columns

            //TODO handle paging

            //TODO handle sorting

            //TODO handle filtering

            // handle report buttons
            pageView = BuildAvailableCommandsForReportButtons(pageView);

            return pageView;
        }

        public PageView BuildTableHeaders(PageView pageView)
        {
            {
                //fieldOnePlantListLinkLandCode
                //conditionalBtnExampleLinkLandCode
                pageView = BuildTableHeader(pageView, "isConditionalBtnAvailable",
                    isVisible: false,
                    "Is Conditional Btn Available");
                //testFileDownloadLinkPacCode
                //testConditionalFileDownloadLinkPacCode
                //testAsyncFlowReqLinkPacCode
                //testConditionalAsyncFlowReqLinkPacCode
            }

            return pageView;
        }

        public PageView BuildTableData(SessionData sessionData, PageView pageView, TacFarmDashboardListModel apiResponse)
        {
            List<Dictionary<string,string>> tableData = new List<Dictionary<string, string>>();

            int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

            int rowNumberToSelect = 0;

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);
            }

            foreach (var rowData in apiResponse.Items)
            {
                Dictionary<string, string> rowDict = BuildTableDataRow(rowData);

                rowDict.Add("rowNumber", rowNumber.ToString());

                if(rowNumberToSelect > 0)
                {
                    if(rowNumber == rowNumberToSelect)
                    tableData.Add(rowDict);
                }
                else
                {
                    tableData.Add(rowDict);
                }

                rowNumber++;
            }

            pageView.TableData = tableData;

            return pageView;
        }

        public Dictionary<string, string> BuildTableDataRow(TacFarmDashboardListModelItem rowData)
        {
            Dictionary<string,string> keyValuePairs = new Dictionary<string, string>();

            {
                //fieldOnePlantListLinkLandCode
                //conditionalBtnExampleLinkLandCode
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "isConditionalBtnAvailable",
                    isVisible: false,
                    value: rowData.IsConditionalBtnAvailable.ToString());
                //testFileDownloadLinkPacCode
                //testConditionalFileDownloadLinkPacCode
                //testAsyncFlowReqLinkPacCode
                //testConditionalAsyncFlowReqLinkPacCode
            }

            return keyValuePairs;
        }

        public PageView BuildAvailableCommandsForReportSort(PageView pageView, TacFarmDashboardListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count < 2)
            {
                return pageView;
            }

            {
                //fieldOnePlantListLinkLandCode
                //conditionalBtnExampleLinkLandCode
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "isConditionalBtnAvailable",
                    isVisible: false,
                    "Is Conditional Btn Available");
                //testFileDownloadLinkPacCode
                //testConditionalFileDownloadLinkPacCode
                //testAsyncFlowReqLinkPacCode
                //testConditionalAsyncFlowReqLinkPacCode
            }

            return pageView;
        }
        public PageView BuildAvailableCommandsForReportPaging(PageView pageView, TacFarmDashboardListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0)
            {
                return pageView;
            }

            return pageView;
        }
        public PageView BuildAvailableCommandsForReportRowButtons(PageView pageView, TacFarmDashboardListModel apiResponse)
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
                pageView = BuildAvailableCommandForReportButton(pageView, "fieldOnePlantListLinkLandCode",
                    "TacAddLand",
                    "fieldOnePlantListLinkLandCode",
                    isVisible: true,
                    isEnabled: true,
                    "Field One-Plants");
                pageView = BuildAvailableCommandForReportButton(pageView, "conditionalBtnExampleLinkLandCode",
                    "TacAddLand",
                    "conditionalBtnExampleLinkLandCode",
                    isVisible: true,
                    isEnabled: true,
                    "Conditional Btn Example");
                pageView = BuildAvailableCommandForReportButton(pageView, "testFileDownloadLinkPacCode",
                    "PacDetails",
                    "testFileDownloadLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test File Download");
                pageView = BuildAvailableCommandForReportButton(pageView, "testConditionalFileDownloadLinkPacCode",
                    "PacDetails",
                    "testConditionalFileDownloadLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Conditional File Download");
                pageView = BuildAvailableCommandForReportButton(pageView, "testAsyncFlowReqLinkPacCode",
                    "PacDetails",
                    "testAsyncFlowReqLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Async Flow Req");
                pageView = BuildAvailableCommandForReportButton(pageView, "testConditionalAsyncFlowReqLinkPacCode",
                    "PacDetails",
                    "testConditionalAsyncFlowReqLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Conditional Async Flow Req");
            }

            return pageView;
        }

        public PageView BuildAvailableCommandsForReportButtons(PageView pageView)
        {

            {
                pageView = BuildAvailableCommandForReportButton(pageView, "backButton",
                    "",
                    "",
                    isVisible: false,
                    isEnabled: true,
                    "");

            }

            return pageView;
        }

        public async Task<PagePointer> ProcessCommand(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText, string postData = "")
        {
            PagePointer pagePointer = ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            }

            var initReportProcessor = new TacFarmDashboardInitReport();

            TacFarmDashboardInitReport.TacFarmDashboardGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            if (!navDictionary.ContainsKey("TacCode"))
            {
                navDictionary.Add("TacCode", contextCode);
            }

            //  handle report buttons
            {

            }

            if (pagePointer != null)
            {
                return pagePointer;
            }

            pagePointer = new PagePointer(_pageName, contextCode);

            if (commandText.Equals("ClearFilters", StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }

            if (commandText.StartsWith("pageNumber:", StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }

            if (commandText.StartsWith("rowNumber:", StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }

            if (commandText.StartsWith("sortOnColumn:",StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }

            TacFarmDashboardListRequest apiRequestModel = new TacFarmDashboardListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            TacFarmDashboardListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0 ||
                apiResponse.Items.Count > 1)
            {
                pagePointer = new PagePointer(_pageName, contextCode);

                return pagePointer;
            }

            var rowData = apiResponse.Items.ToArray()[0];
            if (commandText.Equals("fieldOnePlantListLinkLandCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "LandPlantList",
                    rowData.FieldOnePlantListLinkLandCode);
            if (commandText.Equals("conditionalBtnExampleLinkLandCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "LandPlantList",
                    rowData.ConditionalBtnExampleLinkLandCode);
            if (commandText.Equals("testFileDownloadLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "TacPacList",
                    contextCode);
            if (commandText.Equals("testConditionalFileDownloadLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "TacPacList",
                    contextCode);
            if (commandText.Equals("testAsyncFlowReqLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "TacPacList",
                    contextCode);
            if (commandText.Equals("testConditionalAsyncFlowReqLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "TacPacList",
                    contextCode);
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

        public async Task<TacFarmDashboardListModel> GetResponse(APIClient aPIClient, TacFarmDashboardListRequest model, Guid contextCode)
        {
            string url = $"/tac-farm-dashboard/{contextCode.ToString()}";

            // Serialize the model into a query string
            var queryString = ToQueryString(model);

            // Append the query string to the URL
            url = $"{url}?{queryString}";

            TacFarmDashboardListModel result = await aPIClient.GetAsync<TacFarmDashboardListModel>(url);

            return result;
        }

        public class TacFarmDashboardListModel
        {
            [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int PageNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<TacFarmDashboardListModelItem> Items { get; set; }

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
            public TacFarmDashboardListRequest Request { get; set; }

        }

        public class TacFarmDashboardListModelItem
        {
            [Newtonsoft.Json.JsonProperty("fieldOnePlantListLinkLandCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid FieldOnePlantListLinkLandCode { get; set; }
            [Newtonsoft.Json.JsonProperty("conditionalBtnExampleLinkLandCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid ConditionalBtnExampleLinkLandCode { get; set; }
            [Newtonsoft.Json.JsonProperty("isConditionalBtnAvailable", Required = Newtonsoft.Json.Required.Always)]
            public bool IsConditionalBtnAvailable { get; set; }
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
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalImageUrl { get; set; }

        }

        public class TacFarmDashboardListRequest
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

