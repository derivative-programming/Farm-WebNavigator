using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS.Farm.WebNavigator.Page.Reports.Init;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FS.Farm.WebNavigator.Page.Reports
{
    public class PacUserRoleList : PageBase, IPage
    {
        public PacUserRoleList()
        {
            _pageName = "PacUserRoleList";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, Guid sessionCode, Guid contextCode, string postData = "")
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

            //  handle filter post
            PacUserRoleListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

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

        private string ToQueryString(object obj)
        {
            var properties = from p in obj.GetType().GetProperties()
                             where p.GetValue(obj, null) != null
                             select $"{Uri.EscapeDataString(p.Name)}={Uri.EscapeDataString(p.GetValue(obj, null).ToString())}";

            return string.Join("&", properties);
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
            [Newtonsoft.Json.JsonProperty("roleDescription", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string RoleDescription { get; set; }
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

