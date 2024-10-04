using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FS.Farm.WebNavigator.Page.Reports.Init;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using FS.Farm.WebNavigator.Page.Reports.Models;

namespace FS.Farm.WebNavigator.Page.Reports
{
    public class PacUserRoleList : PageBase, IPage
    {

        public PacUserRoleList()
        {
            _pageName = "PacUserRoleList";

            this.IsAutoSubmit = false;
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText = "")
        {
            var pageView = new PageView();

            if(!sessionData.PageName.Equals(_pageName, StringComparison.OrdinalIgnoreCase))
            {
                //new page, clear filters
                sessionData.Filters.Clear();
                sessionData.ValidationErrors.Clear();
                sessionData.FormFieldProposedValues.Clear();
            }

            pageView.PageTitleText = "Pac User Role List Report";
            pageView.PageIntroText = "";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new PacUserRoleListInitReport();

            //  handle report init
            PacUserRoleListInitReport.GetInitResponse apiInitResponse = await initReportProcessor.RequestGetInitResponse(apiClient, contextCode);

            PacUserRoleListListRequest apiRequestModel = new PacUserRoleListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            if (commandText.StartsWith("setFilter:", StringComparison.OrdinalIgnoreCase))
            {
                string filterName = commandText.Split(':')[1];
                string filterValue = commandText.Split(':')[2];

                if (filterName.Trim().Length == 0)
                {
                    if (sessionData.Filters.ContainsKey(filterName))
                        sessionData.Filters.Remove(filterName);
                }
                else
                {
                    if (sessionData.Filters.ContainsKey(filterName))
                    {
                        sessionData.Filters[filterName] = filterValue;
                    }
                    else
                    {
                        sessionData.Filters.Add(filterName, filterValue);
                    }
                }
            }

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

            string filtersJson = JsonConvert.SerializeObject(sessionData.Filters);

            MergeProperties(apiRequestModel, filtersJson);

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
            PacUserRoleListListModel apiResponse = await RequestGetResponse(apiClient, apiRequestModel, contextCode);

            TableInfo tableInfo = new TableInfo();
            tableInfo.OrderByColumnName = apiResponse.OrderByColumnName;
            tableInfo.PageNumber = apiResponse.PageNumber;
            tableInfo.OrderByDescending = apiResponse.OrderByDescending;
            tableInfo.ItemCountPerPage = apiResponse.ItemCountPerPage;
            tableInfo.TotalItemCount = apiResponse.RecordsTotal;

            pageView.PageTable.TableInfo = tableInfo;

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                PacUserRoleListListModelItem selectedItem = null;

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

                apiResponse.Items.Clear();

                if (selectedItem != null)
                {
                    apiResponse.Items.Add(selectedItem);
                }
            }

            pageView.PageHeaders = initReportProcessor.GetPageHeaders(apiInitResponse);

            pageView = BuildTableHeaders(pageView);

            pageView = BuildTableData(sessionData, pageView, apiResponse);

            pageView = await BuildTableAvailableFilters(apiClient, pageView, apiResponse);

            pageView = BuildAvailableCommandsForReportSort(pageView, apiResponse);

            //  handle report row buttons
            pageView = BuildAvailableCommandsForReportRowButtons(pageView, apiResponse);

            pageView.PageTable.TableFilters = sessionData.Filters;

            //  handle report rows

            //TODO handle filtering

            // handle report buttons
            pageView = BuildAvailableCommandsForReportButtons(pageView);

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

        public PageView BuildTableData(SessionData sessionData, PageView pageView, PacUserRoleListListModel apiResponse)
        {
            List<Dictionary<string,string>> tableData = new List<Dictionary<string, string>>();

            int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

            foreach (var rowData in apiResponse.Items)
            {
                Dictionary<string, string> rowDict = BuildTableDataRow(rowData);

                if (sessionData.Filters.ContainsKey("rowNumber") && apiResponse.Items.Count == 1)
                {
                    int rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);
                    rowDict.Add("rowNumber", rowNumberToSelect.ToString());
                }
                else
                {
                    rowDict.Add("rowNumber", rowNumber.ToString());
                }

                tableData.Add(rowDict);

                rowNumber++;
            }

            pageView.PageTable.TableData = tableData;

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

        public async Task<PageView> BuildTableAvailableFilter(
            APIClient apiClient,
            PageView pageView,
            string name,
            bool isVisible,
            string labelText,
            string dataType,
            bool isFKList,
            string fkObjectName)
        {
            if (!isVisible)
                return pageView;

            var availableFilter = new TableAvailableFilter()
            {
                DataType = dataType,
                Label = labelText,
                Name = name,
                LookupItems = null
            };

            if (isFKList)
            {
                availableFilter.LookupItems = await LookupFactory.GetLookupItems(apiClient, fkObjectName);
            }

            pageView.PageTable.tableAvailableFilters.Add(availableFilter);

            return pageView;
        }

        public async Task<PageView> BuildTableAvailableFilters(APIClient apiClient, PageView pageView, PacUserRoleListListModel apiResponse)
        {
            //if (apiResponse == null ||
            //    apiResponse.Items == null ||
            //    apiResponse.Items.Count < 2)
            //{
            //    return pageView;
            //}

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "setFilter:[available filter name]:[filter value]", Description = "Sort the table on a single column. requesting the same column again will change direction." }
                );

            {

            }

            return pageView;
        }

        public PageView BuildAvailableCommandsForReportSort(PageView pageView, PacUserRoleListListModel apiResponse)
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

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "clearFilters", Description = "Clear all filters" }
                );

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "pageNumber:[page number value (or empty to remove filter)]", Description = "View a particular page of the report results" }
                );

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "rowNumber:[row number value (or empty to remove filter)]", Description = "View a single row of the report results. More commands may then be available for that row." }
                );

            {

            }

            return pageView;
        }

        public async Task<PagePointer> ProcessCommand(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText)
        {
            PagePointer pagePointer = ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            }

            var initReportProcessor = new PacUserRoleListInitReport();

            PacUserRoleListInitReport.GetInitResponse apiInitResponse = await initReportProcessor.RequestGetInitResponse(apiClient, contextCode);

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            if (!navDictionary.ContainsKey("PacCode"))
            {
                navDictionary.Add("PacCode", contextCode);
            }

            if (!navDictionary.ContainsKey("pacCode"))
            {
                navDictionary.Add("pacCode", contextCode);
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

            if (commandText.StartsWith("setFilter:", StringComparison.OrdinalIgnoreCase))
            {
                return pagePointer;
            }

            PacUserRoleListListRequest apiRequestModel = new PacUserRoleListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            PacUserRoleListListModel apiResponse = await RequestGetResponse(apiClient, apiRequestModel, contextCode);

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                PacUserRoleListListModelItem selectedItem = null;

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

                apiResponse.Items.Clear();

                if (selectedItem != null)
                {

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

        public async Task<PacUserRoleListListModel> RequestGetResponse(APIClient aPIClient, PacUserRoleListListRequest model, Guid contextCode)
        {
            string url = $"/pac-user-role-list/{contextCode.ToString()}";

            // Serialize the model into a query string
            var queryString = ToQueryString(model);

            // Append the query string to the URL
            url = $"{url}?{queryString}";

            PacUserRoleListListModel result = await aPIClient.GetAsync<PacUserRoleListListModel>(url);

            return result;
        }

    }
}

