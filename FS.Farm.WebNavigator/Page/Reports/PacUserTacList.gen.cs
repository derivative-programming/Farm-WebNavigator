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
    public class PacUserTacList : PageBase, IPage
    {
        public PacUserTacList()
        {
            _pageName = "PacUserTacList";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText = "", string postData = "")
        {
            var pageView = new PageView();

            if(!sessionData.PageName.Equals("PacUserTacList",StringComparison.OrdinalIgnoreCase))
            {
                //new page, clear filters
                sessionData.Filters.Clear();
            }

            pageView.PageTitleText = "Pac User Tac List Report";
            pageView.PageIntroText = "";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new PacUserTacListInitReport();

            //  handle report init
            PacUserTacListInitReport.PacUserTacListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            PacUserTacListListRequest apiRequestModel = new PacUserTacListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            if (commandText.StartsWith("ClearFilters", StringComparison.OrdinalIgnoreCase))
            {
                sessionData.Filters.Clear();
            }
            if (commandText.StartsWith("pageNumber:", StringComparison.OrdinalIgnoreCase))
            {
                string pageNumberValue = commandText.Split(':')[1];

                if (sessionData.Filters.ContainsKey("pageNumber"))
                {
                    sessionData.Filters["pageNumber"] = pageNumberValue;
                }
                else
                {
                    sessionData.Filters.Add("pageNumber", pageNumberValue);
                }
            }

            MergeProperties(apiRequestModel, postData);

            //default values, can't override
            apiRequestModel.ForceErrorMessage = "";
            apiRequestModel.ItemCountPerPage = 5;

            apiRequestModel.OrderByColumnName = sessionData.OrderByColumnName;

            if (sessionData.Filters.ContainsKey("pageNumber"))
            {
                apiRequestModel.PageNumber = int.Parse(sessionData.Filters["pageNumber"]);
            }

            if (commandText.StartsWith("sortOnColumn:",StringComparison.OrdinalIgnoreCase))
            {
                string columnName = commandText.Split(':')[1];
                if(apiRequestModel.OrderByColumnName.Equals(columnName,StringComparison.OrdinalIgnoreCase))
                {
                    apiRequestModel.OrderByDescending = true;
                }
                else
                {
                    apiRequestModel.OrderByColumnName = commandText.Split(':')[1];
                    apiRequestModel.OrderByDescending = false;
                }
                sessionData.OrderByColumnName = apiRequestModel.OrderByColumnName;
            }

            //  handle filter post
            PacUserTacListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            pageView.PageHeaders = initReportProcessor.GetPageHeaders(apiInitResponse);

            pageView = BuildTableHeaders(pageView);

            pageView = BuildTableData(pageView, apiResponse);

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
                //TacCode
                pageView = BuildTableHeader(pageView, "tacDescription",
                    isVisible: true,
                    "Description");
                pageView = BuildTableHeader(pageView, "tacDisplayOrder",
                    isVisible: true,
                    "Display Order");
                pageView = BuildTableHeader(pageView, "tacIsActive",
                    isVisible: true,
                    "Is Active");
                pageView = BuildTableHeader(pageView, "tacLookupEnumName",
                    isVisible: true,
                    "Lookup Enum Name");
                pageView = BuildTableHeader(pageView, "tacName",
                    isVisible: true,
                    "Name");
                pageView = BuildTableHeader(pageView, "pacName",
                    isVisible: true,
                    "Pac Name");
            }

            return pageView;
        }

        public PageView BuildTableData(PageView pageView, PacUserTacListListModel apiResponse)
        {
            List<Dictionary<string,string>> tableData = new List<Dictionary<string, string>>();

            int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

            foreach(var rowData in apiResponse.Items)
            {
                Dictionary<string, string> rowDict = BuildTableDataRow(rowData);

                rowDict.Add("rowNumber", rowNumber.ToString());

                tableData.Add(rowDict);

                rowNumber++;
            }

            pageView.TableData = tableData;

            return pageView;
        }

        public Dictionary<string, string> BuildTableDataRow(PacUserTacListListModelItem rowData)
        {
            Dictionary<string,string> keyValuePairs = new Dictionary<string, string>();

            {
                //TacCode
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "tacDescription",
                    isVisible: true,
                    value: rowData.TacDescription.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "tacDisplayOrder",
                    isVisible: true,
                    value: rowData.TacDisplayOrder.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "tacIsActive",
                    isVisible: true,
                    value: rowData.TacIsActive.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "tacLookupEnumName",
                    isVisible: true,
                    value: rowData.TacLookupEnumName.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "tacName",
                    isVisible: true,
                    value: rowData.TacName.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "pacName",
                    isVisible: true,
                    value: rowData.PacName.ToString());
            }

            return keyValuePairs;
        }

        public PageView BuildAvailableCommandsForReportSort(PageView pageView, PacUserTacListListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count < 2)
            {
                return pageView;
            }

            {
                //TacCode
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "tacDescription",
                    isVisible: true,
                    "Description");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "tacDisplayOrder",
                    isVisible: true,
                    "Display Order");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "tacIsActive",
                    isVisible: true,
                    "Is Active");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "tacLookupEnumName",
                    isVisible: true,
                    "Lookup Enum Name");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "tacName",
                    isVisible: true,
                    "Name");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "pacName",
                    isVisible: true,
                    "Pac Name");
            }

            return pageView;
        }
        public PageView BuildAvailableCommandsForReportPaging(PageView pageView, PacUserTacListListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0)
            {
                return pageView;
            }

            return pageView;
        }
        public PageView BuildAvailableCommandsForReportRowButtons(PageView pageView, PacUserTacListListModel apiResponse)
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

            }

            return pageView;
        }

        public PageView BuildAvailableCommandsForReportButtons(PageView pageView)
        {

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "ClearFilters", Description = "Clear all filters" }
                );

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "PageNumber:[page number value]", Description = "View a particular page of the report results" }
                );

            {

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

            var initReportProcessor = new PacUserTacListInitReport();

            PacUserTacListInitReport.PacUserTacListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            if (!navDictionary.ContainsKey("PacCode"))
            {
                navDictionary.Add("PacCode", contextCode);
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

            if (commandText.StartsWith("sortOnColumn:",StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }

            PacUserTacListListRequest apiRequestModel = new PacUserTacListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            PacUserTacListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0 ||
                apiResponse.Items.Count > 1)
            {
                pagePointer = new PagePointer(_pageName, contextCode);

                return pagePointer;
            }

            var rowData = apiResponse.Items.ToArray()[0];

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

        public async Task<PacUserTacListListModel> GetResponse(APIClient aPIClient, PacUserTacListListRequest model, Guid contextCode)
        {
            string url = $"/pac-user-tac-list/{contextCode.ToString()}";

            // Serialize the model into a query string
            var queryString = ToQueryString(model);

            // Append the query string to the URL
            url = $"{url}?{queryString}";

            PacUserTacListListModel result = await aPIClient.GetAsync<PacUserTacListListModel>(url);

            return result;
        }

        public class PacUserTacListListModel
        {
            [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int PageNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<PacUserTacListListModelItem> Items { get; set; }

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
            public PacUserTacListListRequest Request { get; set; }

        }

        public class PacUserTacListListModelItem
        {
            [Newtonsoft.Json.JsonProperty("tacCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid TacCode { get; set; }
            [Newtonsoft.Json.JsonProperty("tacDescription", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string TacDescription { get; set; }
            [Newtonsoft.Json.JsonProperty("tacDisplayOrder", Required = Newtonsoft.Json.Required.Always)]
            public int TacDisplayOrder { get; set; }
            [Newtonsoft.Json.JsonProperty("tacIsActive", Required = Newtonsoft.Json.Required.Always)]
            public bool TacIsActive { get; set; }
            [Newtonsoft.Json.JsonProperty("tacLookupEnumName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string TacLookupEnumName { get; set; }
            [Newtonsoft.Json.JsonProperty("tacName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string TacName { get; set; }
            [Newtonsoft.Json.JsonProperty("pacName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string PacName { get; set; }
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalImageUrl { get; set; }

        }

        public class PacUserTacListListRequest
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

