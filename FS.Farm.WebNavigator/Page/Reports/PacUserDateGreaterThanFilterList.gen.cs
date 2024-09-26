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
    public class PacUserDateGreaterThanFilterList : PageBase, IPage
    {
        public PacUserDateGreaterThanFilterList()
        {
            _pageName = "PacUserDateGreaterThanFilterList";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText = "", string postData = "")
        {
            var pageView = new PageView();

            if(!sessionData.PageName.Equals(_pageName, StringComparison.OrdinalIgnoreCase))
            {
                //new page, clear filters
                sessionData.Filters.Clear();
                sessionData.ValidationErrors.Clear();
                sessionData.FormFieldProposedValues.Clear();
            }

            pageView.PageTitleText = "Pac User Date Greater Than Filter List Report";
            pageView.PageIntroText = "";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new PacUserDateGreaterThanFilterListInitReport();

            //  handle report init
            PacUserDateGreaterThanFilterListInitReport.PacUserDateGreaterThanFilterListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            PacUserDateGreaterThanFilterListListRequest apiRequestModel = new PacUserDateGreaterThanFilterListListRequest();

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
            PacUserDateGreaterThanFilterListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            TableInfo tableInfo = new TableInfo();
            tableInfo.OrderByColumnName = apiResponse.OrderByColumnName;
            tableInfo.PageNumber = apiResponse.PageNumber;
            tableInfo.OrderByDescending = apiResponse.OrderByDescending;
            tableInfo.ItemCountPerPage = apiResponse.ItemCountPerPage;
            tableInfo.TotalItemCount = apiResponse.RecordsTotal;

            pageView.TableInfo = tableInfo;

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                PacUserDateGreaterThanFilterListListModelItem selectedItem = null;

                foreach (var rowData in apiResponse.Items)
                {

                    if (rowNumberToSelect > 0)
                    {
                        if (rowNumber == rowNumberToSelect)
                        {
                            selectedItem = rowData;
                        }
                    }

                    rowNumber++;
                }

                if (selectedItem != null)
                {
                    apiResponse.Items.Clear();

                    apiResponse.Items.Add(selectedItem);
                }
            }

            pageView.PageHeaders = initReportProcessor.GetPageHeaders(apiInitResponse);

            pageView = BuildTableHeaders(pageView);

            pageView = BuildTableData(sessionData, pageView, apiResponse);

            pageView = BuildTableAvailableFilters(pageView);

            pageView = BuildAvailableCommandsForReportSort(pageView, apiResponse);

            //  handle report row buttons
            pageView = BuildAvailableCommandsForReportRowButtons(pageView, apiResponse);

            pageView.TableFilters = sessionData.Filters;

            //  handle report rows

            //TODO handle filtering

            // handle report buttons
            pageView = BuildAvailableCommandsForReportButtons(pageView);

            return pageView;
        }

        public PageView BuildTableHeaders(PageView pageView)
        {
            {
                //DateGreaterThanFilterCode
                pageView = BuildTableHeader(pageView, "dateGreaterThanFilterDayCount",
                    isVisible: true,
                    "Day Count");
                pageView = BuildTableHeader(pageView, "dateGreaterThanFilterDescription",
                    isVisible: true,
                    "Description");
                pageView = BuildTableHeader(pageView, "dateGreaterThanFilterDisplayOrder",
                    isVisible: true,
                    "Display Order");
                pageView = BuildTableHeader(pageView, "dateGreaterThanFilterIsActive",
                    isVisible: true,
                    "Is Active");
                pageView = BuildTableHeader(pageView, "dateGreaterThanFilterLookupEnumName",
                    isVisible: true,
                    "Lookup Enum Name");
                pageView = BuildTableHeader(pageView, "dateGreaterThanFilterName",
                    isVisible: true,
                    "Name");
            }

            return pageView;
        }

        public PageView BuildTableData(SessionData sessionData, PageView pageView, PacUserDateGreaterThanFilterListListModel apiResponse)
        {
            List<Dictionary<string,string>> tableData = new List<Dictionary<string, string>>();

            int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

            foreach (var rowData in apiResponse.Items)
            {
                Dictionary<string, string> rowDict = BuildTableDataRow(rowData);

                rowDict.Add("rowNumber", rowNumber.ToString());

                tableData.Add(rowDict);

                rowNumber++;
            }

            pageView.TableData = tableData;

            return pageView;
        }

        public Dictionary<string, string> BuildTableDataRow(PacUserDateGreaterThanFilterListListModelItem rowData)
        {
            Dictionary<string,string> keyValuePairs = new Dictionary<string, string>();

            {
                //DateGreaterThanFilterCode
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "dateGreaterThanFilterDayCount",
                    isVisible: true,
                    value: rowData.DateGreaterThanFilterDayCount.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "dateGreaterThanFilterDescription",
                    isVisible: true,
                    value: rowData.DateGreaterThanFilterDescription.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "dateGreaterThanFilterDisplayOrder",
                    isVisible: true,
                    value: rowData.DateGreaterThanFilterDisplayOrder.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "dateGreaterThanFilterIsActive",
                    isVisible: true,
                    value: rowData.DateGreaterThanFilterIsActive.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "dateGreaterThanFilterLookupEnumName",
                    isVisible: true,
                    value: rowData.DateGreaterThanFilterLookupEnumName.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "dateGreaterThanFilterName",
                    isVisible: true,
                    value: rowData.DateGreaterThanFilterName.ToString());
            }

            return keyValuePairs;
        }

        public PageView BuildTableAvailableFilters(PageView pageView)
        {
            {

            }

            return pageView;
        }

        public PageView BuildAvailableCommandsForReportSort(PageView pageView, PacUserDateGreaterThanFilterListListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count < 2)
            {
                return pageView;
            }

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "sortOnColumn:[table header name]", Description = "Sort the table on a single column. requesting the same column again will change direction." }
                );

            return pageView;

            {
                //DateGreaterThanFilterCode
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "dateGreaterThanFilterDayCount",
                    isVisible: true,
                    "Day Count");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "dateGreaterThanFilterDescription",
                    isVisible: true,
                    "Description");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "dateGreaterThanFilterDisplayOrder",
                    isVisible: true,
                    "Display Order");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "dateGreaterThanFilterIsActive",
                    isVisible: true,
                    "Is Active");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "dateGreaterThanFilterLookupEnumName",
                    isVisible: true,
                    "Lookup Enum Name");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "dateGreaterThanFilterName",
                    isVisible: true,
                    "Name");
            }

            return pageView;
        }

        public PageView BuildAvailableCommandsForReportRowButtons(PageView pageView, PacUserDateGreaterThanFilterListListModel apiResponse)
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
                new AvailableCommand { CommandText = "PageNumber:[page number value (or empty to remove filter)]", Description = "View a particular page of the report results" }
                );

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "RowNumber:[row number value (or empty to remove filter)]", Description = "View a single row of the report results. More commands may then be available for that row." }
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

            var initReportProcessor = new PacUserDateGreaterThanFilterListInitReport();

            PacUserDateGreaterThanFilterListInitReport.PacUserDateGreaterThanFilterListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

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

            if (commandText.StartsWith("rowNumber:", StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }

            if (commandText.StartsWith("sortOnColumn:",StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }

            PacUserDateGreaterThanFilterListListRequest apiRequestModel = new PacUserDateGreaterThanFilterListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            PacUserDateGreaterThanFilterListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                PacUserDateGreaterThanFilterListListModelItem selectedItem = null;

                foreach (var rowDataItem in apiResponse.Items)
                {

                    if (rowNumberToSelect > 0)
                    {
                        if (rowNumber == rowNumberToSelect)
                        {
                            selectedItem = rowDataItem;
                        }
                    }

                    rowNumber++;
                }

                if (selectedItem != null)
                {
                    apiResponse.Items.Clear();

                    apiResponse.Items.Add(selectedItem);
                }
            }

            pagePointer = new PagePointer(_pageName, contextCode);

            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0 ||
                apiResponse.Items.Count > 1)
            {

                return pagePointer;
            }

            var rowData = apiResponse.Items.ToArray()[0];

            //  handle report row buttons
            {

            }

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

        public async Task<PacUserDateGreaterThanFilterListListModel> GetResponse(APIClient aPIClient, PacUserDateGreaterThanFilterListListRequest model, Guid contextCode)
        {
            string url = $"/pac-user-date-greater-than-filter-list/{contextCode.ToString()}";

            // Serialize the model into a query string
            var queryString = ToQueryString(model);

            // Append the query string to the URL
            url = $"{url}?{queryString}";

            PacUserDateGreaterThanFilterListListModel result = await aPIClient.GetAsync<PacUserDateGreaterThanFilterListListModel>(url);

            return result;
        }

        public class PacUserDateGreaterThanFilterListListModel
        {
            [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int PageNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<PacUserDateGreaterThanFilterListListModelItem> Items { get; set; }

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
            public PacUserDateGreaterThanFilterListListRequest Request { get; set; }

        }

        public class PacUserDateGreaterThanFilterListListModelItem
        {
            [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid DateGreaterThanFilterCode { get; set; }
            [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterDayCount", Required = Newtonsoft.Json.Required.Always)]
            public int DateGreaterThanFilterDayCount { get; set; }
            [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterDescription", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string DateGreaterThanFilterDescription { get; set; }
            [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterDisplayOrder", Required = Newtonsoft.Json.Required.Always)]
            public int DateGreaterThanFilterDisplayOrder { get; set; }
            [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterIsActive", Required = Newtonsoft.Json.Required.Always)]
            public bool DateGreaterThanFilterIsActive { get; set; }
            [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterLookupEnumName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string DateGreaterThanFilterLookupEnumName { get; set; }
            [Newtonsoft.Json.JsonProperty("dateGreaterThanFilterName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string DateGreaterThanFilterName { get; set; }
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalImageUrl { get; set; }

        }

        public class PacUserDateGreaterThanFilterListListRequest
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

