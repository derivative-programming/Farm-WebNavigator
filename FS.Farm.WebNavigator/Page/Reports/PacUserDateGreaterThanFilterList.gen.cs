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
    public class PacUserDateGreaterThanFilterList : PageBase, IPage
    {
        public PacUserDateGreaterThanFilterList()
        {
            _pageName = "PacUserDateGreaterThanFilterList";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, Guid sessionCode, Guid contextCode, string postData = "")
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Pac User Date Greater Than Filter List Report";
            pageView.PageIntroText = "";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new PacUserDateGreaterThanFilterListInitReport();

            //  handle report init
            PacUserDateGreaterThanFilterListInitReport.PacUserDateGreaterThanFilterListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            PacUserDateGreaterThanFilterListListRequest apiRequestModel = new PacUserDateGreaterThanFilterListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            //  handle filter post
            PacUserDateGreaterThanFilterListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

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

            var initReportProcessor = new PacUserDateGreaterThanFilterListInitReport();

            PacUserDateGreaterThanFilterListInitReport.PacUserDateGreaterThanFilterListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

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

            PacUserDateGreaterThanFilterListListRequest apiRequestModel = new PacUserDateGreaterThanFilterListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);

            PacUserDateGreaterThanFilterListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

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

        public partial class ValidationError
        {
            [Newtonsoft.Json.JsonProperty("property", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Property { get; set; }

            [Newtonsoft.Json.JsonProperty("message", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string Message { get; set; }

        }

    }
}

