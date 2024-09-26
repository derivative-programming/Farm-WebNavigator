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
    public class PacUserTriStateFilterList : PageBase, IPage
    {
        public PacUserTriStateFilterList()
        {
            _pageName = "PacUserTriStateFilterList";
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

            pageView.PageTitleText = "Pac User Tri State Filter List Report";
            pageView.PageIntroText = "";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new PacUserTriStateFilterListInitReport();

            //  handle report init
            PacUserTriStateFilterListInitReport.PacUserTriStateFilterListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            PacUserTriStateFilterListListRequest apiRequestModel = new PacUserTriStateFilterListListRequest();

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
            PacUserTriStateFilterListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

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

                PacUserTriStateFilterListListModelItem selectedItem = null;

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
                //TriStateFilterCode
                pageView = BuildTableHeader(pageView, "triStateFilterDescription",
                    isVisible: true,
                    "Description");
                pageView = BuildTableHeader(pageView, "triStateFilterDisplayOrder",
                    isVisible: true,
                    "Display Order");
                pageView = BuildTableHeader(pageView, "triStateFilterIsActive",
                    isVisible: true,
                    "Is Active");
                pageView = BuildTableHeader(pageView, "triStateFilterLookupEnumName",
                    isVisible: true,
                    "Lookup Enum Name");
                pageView = BuildTableHeader(pageView, "triStateFilterName",
                    isVisible: true,
                    "Name");
                pageView = BuildTableHeader(pageView, "triStateFilterStateIntValue",
                    isVisible: true,
                    "State Int Value");
            }

            return pageView;
        }

        public PageView BuildTableData(SessionData sessionData, PageView pageView, PacUserTriStateFilterListListModel apiResponse)
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

        public Dictionary<string, string> BuildTableDataRow(PacUserTriStateFilterListListModelItem rowData)
        {
            Dictionary<string,string> keyValuePairs = new Dictionary<string, string>();

            {
                //TriStateFilterCode
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "triStateFilterDescription",
                    isVisible: true,
                    value: rowData.TriStateFilterDescription.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "triStateFilterDisplayOrder",
                    isVisible: true,
                    value: rowData.TriStateFilterDisplayOrder.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "triStateFilterIsActive",
                    isVisible: true,
                    value: rowData.TriStateFilterIsActive.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "triStateFilterLookupEnumName",
                    isVisible: true,
                    value: rowData.TriStateFilterLookupEnumName.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "triStateFilterName",
                    isVisible: true,
                    value: rowData.TriStateFilterName.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "triStateFilterStateIntValue",
                    isVisible: true,
                    value: rowData.TriStateFilterStateIntValue.ToString());
            }

            return keyValuePairs;
        }

        public PageView BuildAvailableCommandsForReportSort(PageView pageView, PacUserTriStateFilterListListModel apiResponse)
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
                //TriStateFilterCode
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "triStateFilterDescription",
                    isVisible: true,
                    "Description");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "triStateFilterDisplayOrder",
                    isVisible: true,
                    "Display Order");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "triStateFilterIsActive",
                    isVisible: true,
                    "Is Active");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "triStateFilterLookupEnumName",
                    isVisible: true,
                    "Lookup Enum Name");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "triStateFilterName",
                    isVisible: true,
                    "Name");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "triStateFilterStateIntValue",
                    isVisible: true,
                    "State Int Value");
            }

            return pageView;
        }

        public PageView BuildAvailableCommandsForReportRowButtons(PageView pageView, PacUserTriStateFilterListListModel apiResponse)
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

            var initReportProcessor = new PacUserTriStateFilterListInitReport();

            PacUserTriStateFilterListInitReport.PacUserTriStateFilterListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

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

            PacUserTriStateFilterListListRequest apiRequestModel = new PacUserTriStateFilterListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            PacUserTriStateFilterListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                PacUserTriStateFilterListListModelItem selectedItem = null;

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

            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0 ||
                apiResponse.Items.Count > 1)
            {
                pagePointer = new PagePointer(_pageName, contextCode);

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

        public async Task<PacUserTriStateFilterListListModel> GetResponse(APIClient aPIClient, PacUserTriStateFilterListListRequest model, Guid contextCode)
        {
            string url = $"/pac-user-tri-state-filter-list/{contextCode.ToString()}";

            // Serialize the model into a query string
            var queryString = ToQueryString(model);

            // Append the query string to the URL
            url = $"{url}?{queryString}";

            PacUserTriStateFilterListListModel result = await aPIClient.GetAsync<PacUserTriStateFilterListListModel>(url);

            return result;
        }

        public class PacUserTriStateFilterListListModel
        {
            [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int PageNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<PacUserTriStateFilterListListModelItem> Items { get; set; }

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
            public PacUserTriStateFilterListListRequest Request { get; set; }

        }

        public class PacUserTriStateFilterListListModelItem
        {
            [Newtonsoft.Json.JsonProperty("triStateFilterCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid TriStateFilterCode { get; set; }
            [Newtonsoft.Json.JsonProperty("triStateFilterDescription", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string TriStateFilterDescription { get; set; }
            [Newtonsoft.Json.JsonProperty("triStateFilterDisplayOrder", Required = Newtonsoft.Json.Required.Always)]
            public int TriStateFilterDisplayOrder { get; set; }
            [Newtonsoft.Json.JsonProperty("triStateFilterIsActive", Required = Newtonsoft.Json.Required.Always)]
            public bool TriStateFilterIsActive { get; set; }
            [Newtonsoft.Json.JsonProperty("triStateFilterLookupEnumName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string TriStateFilterLookupEnumName { get; set; }
            [Newtonsoft.Json.JsonProperty("triStateFilterName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string TriStateFilterName { get; set; }
            [Newtonsoft.Json.JsonProperty("triStateFilterStateIntValue", Required = Newtonsoft.Json.Required.Always)]
            public int TriStateFilterStateIntValue { get; set; }
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalImageUrl { get; set; }

        }

        public class PacUserTriStateFilterListListRequest
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

