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
    public class PacUserRoleList : PageBase, IPage
    {
        public PacUserRoleList()
        {
            _pageName = "PacUserRoleList";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, Guid sessionCode, Guid contextCode, string commandText = "", string postData = "")
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Pac User Role List Report";
            pageView.PageIntroText = "";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new PacUserRoleListInitReport();

            //  handle report init
            PacUserRoleListInitReport.PacUserRoleListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            PacUserRoleListListRequest apiRequestModel = new PacUserRoleListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            //default values, can't override
            apiRequestModel.ForceErrorMessage = "";
            apiRequestModel.ItemCountPerPage = 5;

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
            }

            //  handle filter post
            PacUserRoleListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

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
                //RoleCode
                pageView = BuildTableHeader(pageView, "roleDescription",
                    isVisible: true,
                    "Description");
                pageView = BuildTableHeader(pageView, "roleDisplayOrder",
                    isVisible: true,
                    "Display Order");
                pageView = BuildTableHeader(pageView, "roleIsActive",
                    isVisible: true,
                    "Is Active");
                pageView = BuildTableHeader(pageView, "roleLookupEnumName",
                    isVisible: true,
                    "Lookup Enum Name");
                pageView = BuildTableHeader(pageView, "roleName",
                    isVisible: true,
                    "Name");
                pageView = BuildTableHeader(pageView, "pacName",
                    isVisible: true,
                    "Pac Name");
            }

            return pageView;
        }

        public PageView BuildTableData(PageView pageView, PacUserRoleListListModel apiResponse)
        {
            List<Dictionary<string,string>> tableData = new List<Dictionary<string, string>>();

            foreach(var rowData in apiResponse.Items)
            {
                tableData.Add(BuildTableDataRow(rowData));
            }

            pageView.TableData = tableData;

            return pageView;
        }

        public Dictionary<string, string> BuildTableDataRow(PacUserRoleListListModelItem rowData)
        {
            Dictionary<string,string> keyValuePairs = new Dictionary<string, string>();

            {
                //RoleCode
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "roleDescription",
                    isVisible: true,
                    value: rowData.RoleDescription.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "roleDisplayOrder",
                    isVisible: true,
                    value: rowData.RoleDisplayOrder.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "roleIsActive",
                    isVisible: true,
                    value: rowData.RoleIsActive.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "roleLookupEnumName",
                    isVisible: true,
                    value: rowData.RoleLookupEnumName.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "roleName",
                    isVisible: true,
                    value: rowData.RoleName.ToString());
                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "pacName",
                    isVisible: true,
                    value: rowData.PacName.ToString());
            }

            return keyValuePairs;
        }

        public PageView BuildAvailableCommandsForReportSort(PageView pageView, PacUserRoleListListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count < 2)
            {
                return pageView;
            }

            {
                //RoleCode
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "roleDescription",
                    isVisible: true,
                    "Description");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "roleDisplayOrder",
                    isVisible: true,
                    "Display Order");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "roleIsActive",
                    isVisible: true,
                    "Is Active");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "roleLookupEnumName",
                    isVisible: true,
                    "Lookup Enum Name");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "roleName",
                    isVisible: true,
                    "Name");
                pageView = BuildAvailableCommandForSortOnColumn(pageView, "pacName",
                    isVisible: true,
                    "Pac Name");
            }

            return pageView;
        }
        public PageView BuildAvailableCommandsForReportPaging(PageView pageView, PacUserRoleListListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count == 0)
            {
                return pageView;
            }

            return pageView;
        }
        public PageView BuildAvailableCommandsForReportRowButtons(PageView pageView, PacUserRoleListListModel apiResponse)
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

            return pageView;
        }

        public async Task<PagePointer> ProcessCommand(APIClient apiClient, Guid sessionCode, Guid contextCode, string commandText, string postData = "")
        {
            PagePointer pagePointer = ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            }

            var initReportProcessor = new PacUserRoleListInitReport();

            PacUserRoleListInitReport.PacUserRoleListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            if (!navDictionary.ContainsKey("PacCode"))
            {
                navDictionary.Add("PacCode", contextCode);
            }

            if (pagePointer != null)
            {
                return pagePointer;
            }

            pagePointer = new PagePointer(_pageName, contextCode);

            if(commandText.StartsWith("sortOnColumn:",StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }

            PacUserRoleListListRequest apiRequestModel = new PacUserRoleListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            PacUserRoleListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

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

        public async Task<PacUserRoleListListModel> GetResponse(APIClient aPIClient, PacUserRoleListListRequest model, Guid contextCode)
        {
            string url = $"/pac-user-role-list/{contextCode.ToString()}";

            // Serialize the model into a query string
            var queryString = ToQueryString(model);

            // Append the query string to the URL
            url = $"{url}?{queryString}";

            PacUserRoleListListModel result = await aPIClient.GetAsync<PacUserRoleListListModel>(url);

            return result;
        }

        public class PacUserRoleListListModel
        {
            [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int PageNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<PacUserRoleListListModelItem> Items { get; set; }

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
            public PacUserRoleListListRequest Request { get; set; }

        }

        public class PacUserRoleListListModelItem
        {
            [Newtonsoft.Json.JsonProperty("roleCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid RoleCode { get; set; }
            [Newtonsoft.Json.JsonProperty("roleDescription", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RoleDescription { get; set; }
            [Newtonsoft.Json.JsonProperty("roleDisplayOrder", Required = Newtonsoft.Json.Required.Always)]
            public int RoleDisplayOrder { get; set; }
            [Newtonsoft.Json.JsonProperty("roleIsActive", Required = Newtonsoft.Json.Required.Always)]
            public bool RoleIsActive { get; set; }
            [Newtonsoft.Json.JsonProperty("roleLookupEnumName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RoleLookupEnumName { get; set; }
            [Newtonsoft.Json.JsonProperty("roleName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RoleName { get; set; }
            [Newtonsoft.Json.JsonProperty("pacName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string PacName { get; set; }
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalImageUrl { get; set; }

        }

        public class PacUserRoleListListRequest
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

