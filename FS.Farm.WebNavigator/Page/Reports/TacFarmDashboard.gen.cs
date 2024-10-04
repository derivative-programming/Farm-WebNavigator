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
    public class TacFarmDashboard : PageBase, IPage
    {

        public TacFarmDashboard()
        {
            _pageName = "TacFarmDashboard";

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

            pageView.PageTitleText = "Farm Dashboard";
            pageView.PageIntroText = "Farm Dashboard page intro text";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new TacFarmDashboardInitReport();

            //  handle report init
            TacFarmDashboardInitReport.GetInitResponse apiInitResponse = await initReportProcessor.RequestGetInitResponse(apiClient, contextCode);

            TacFarmDashboardListRequest apiRequestModel = new TacFarmDashboardListRequest();

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

            apiRequestModel.PageNumber = 1;
            apiRequestModel.OrderByColumnName = "";
            apiRequestModel.OrderByDescending = false;

            //  handle filter post
            TacFarmDashboardListModel apiResponse = await RequestGetResponse(apiClient, apiRequestModel, contextCode);

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                TacFarmDashboardListModelItem selectedItem = null;

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

        public async Task<PageView> BuildTableAvailableFilters(APIClient apiClient, PageView pageView, TacFarmDashboardListModel apiResponse)
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

        public PageView BuildAvailableCommandsForReportSort(PageView pageView, TacFarmDashboardListModel apiResponse)
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

        public async Task<PagePointer> ProcessCommand(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText)
        {
            PagePointer pagePointer = ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            }

            var initReportProcessor = new TacFarmDashboardInitReport();

            TacFarmDashboardInitReport.GetInitResponse apiInitResponse = await initReportProcessor.RequestGetInitResponse(apiClient, contextCode);

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            if (!navDictionary.ContainsKey("TacCode"))
            {
                navDictionary.Add("TacCode", contextCode);
            }

            if (!navDictionary.ContainsKey("tacCode"))
            {
                navDictionary.Add("tacCode", contextCode);
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

            TacFarmDashboardListRequest apiRequestModel = new TacFarmDashboardListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            TacFarmDashboardListModel apiResponse = await RequestGetResponse(apiClient, apiRequestModel, contextCode);

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                TacFarmDashboardListModelItem selectedItem = null;

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
                if (commandText.Equals("fieldOnePlantListLinkLandCode", StringComparison.OrdinalIgnoreCase))
                    pagePointer = new PagePointer(
                        "LandPlantList",
                        rowData.FieldOnePlantListLinkLandCode);
                if (commandText.Equals("conditionalBtnExampleLinkLandCode", StringComparison.OrdinalIgnoreCase))
                    pagePointer = new PagePointer(
                        "LandPlantList",
                        rowData.ConditionalBtnExampleLinkLandCode);
                if (commandText.Equals("testFileDownloadLinkPacCode", StringComparison.OrdinalIgnoreCase))
                    if((await Services.PacUserTestAsyncFileDownload.GetResponse(apiClient,rowData.TestFileDownloadLinkPacCode)).Success)
                        pagePointer = new PagePointer(
                            _pageName,
                            contextCode);
                if (commandText.Equals("testConditionalFileDownloadLinkPacCode", StringComparison.OrdinalIgnoreCase))
                    if((await Services.PacUserTestAsyncFileDownload.GetResponse(apiClient,rowData.TestConditionalFileDownloadLinkPacCode)).Success)
                        pagePointer = new PagePointer(
                            _pageName,
                            contextCode);
                if (commandText.Equals("testAsyncFlowReqLinkPacCode", StringComparison.OrdinalIgnoreCase))
                    if((await Services.PacUserTestAsyncFlowReq.GetResponse(apiClient,rowData.TestAsyncFlowReqLinkPacCode)).Success)
                        pagePointer = new PagePointer(
                            _pageName,
                            contextCode);
                if (commandText.Equals("testConditionalAsyncFlowReqLinkPacCode", StringComparison.OrdinalIgnoreCase))
                    if((await Services.PacUserTestAsyncFlowReq.GetResponse(apiClient,rowData.TestConditionalAsyncFlowReqLinkPacCode)).Success)
                        pagePointer = new PagePointer(
                            _pageName,
                            contextCode);
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

        public async Task<TacFarmDashboardListModel> RequestGetResponse(APIClient aPIClient, TacFarmDashboardListRequest model, Guid contextCode)
        {
            string url = $"/tac-farm-dashboard/{contextCode.ToString()}";

            // Serialize the model into a query string
            var queryString = ToQueryString(model);

            // Append the query string to the URL
            url = $"{url}?{queryString}";

            TacFarmDashboardListModel result = await aPIClient.GetAsync<TacFarmDashboardListModel>(url);

            return result;
        }

    }
}

