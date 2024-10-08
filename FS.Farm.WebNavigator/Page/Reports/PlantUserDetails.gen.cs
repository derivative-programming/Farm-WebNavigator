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
    public class PlantUserDetails : PageBase, IPage
    {

        public PlantUserDetails()
        {
            _pageName = "PlantUserDetails";

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

            pageView.PageTitleText = "Plant Details";
            pageView.PageIntroText = "Plant Details page intro text";
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new PlantUserDetailsInitReport();

            //  handle report init
            PlantUserDetailsInitReport.GetInitResponse apiInitResponse = await initReportProcessor.RequestGetInitResponse(apiClient, contextCode);

            PlantUserDetailsListRequest apiRequestModel = new PlantUserDetailsListRequest();

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
            PlantUserDetailsListModel apiResponse = await RequestGetResponse(apiClient, apiRequestModel, contextCode);

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                PlantUserDetailsListModelItem selectedItem = null;

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

        public PageView BuildTableData(SessionData sessionData, PageView pageView, PlantUserDetailsListModel apiResponse)
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
                    value: rowData.SomeDateVal.ToString("yyyy-MM-ddTHH:mm:ss"));
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
                    value: rowData.SomeUTCDateTimeVal.ToString("yyyy-MM-ddTHH:mm:ss"));
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

        public async Task<PageView> BuildTableAvailableFilters(APIClient apiClient, PageView pageView, PlantUserDetailsListModel apiResponse)
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

        public PageView BuildAvailableCommandsForReportSort(PageView pageView, PlantUserDetailsListModel apiResponse)
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

            {
                pageView = BuildAvailableCommandForReportButton(pageView, "backButton",
                    "LandPlantList",
                    "LandCode",
                    isVisible: true,
                    isEnabled: true,
                    "Plant List");

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

            var initReportProcessor = new PlantUserDetailsInitReport();

            PlantUserDetailsInitReport.GetInitResponse apiInitResponse = await initReportProcessor.RequestGetInitResponse(apiClient, contextCode);

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            if (!navDictionary.ContainsKey("PlantCode"))
            {
                navDictionary.Add("PlantCode", contextCode);
            }

            if (!navDictionary.ContainsKey("plantCode"))
            {
                navDictionary.Add("plantCode", contextCode);
            }

            //  handle report buttons
            {
                if (commandText.Equals("backButton", StringComparison.OrdinalIgnoreCase))
                    pagePointer = new PagePointer(
                        "LandPlantList",
                        Guid.Parse(navDictionary["landCode"].ToString()));

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

            PlantUserDetailsListRequest apiRequestModel = new PlantUserDetailsListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            PlantUserDetailsListModel apiResponse = await RequestGetResponse(apiClient, apiRequestModel, contextCode);

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                PlantUserDetailsListModelItem selectedItem = null;

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
                if (commandText.Equals("updateButtonTextLinkPlantCode", StringComparison.OrdinalIgnoreCase))
                    pagePointer = new PagePointer(
                        "PlantUserDetails",
                        rowData.UpdateButtonTextLinkPlantCode);
                if (commandText.Equals("randomPropertyUpdatesLinkPlantCode", StringComparison.OrdinalIgnoreCase))
                    if((await Services.PlantUserPropertyRandomUpdate.GetResponse(apiClient,rowData.RandomPropertyUpdatesLinkPlantCode)).Success)
                        pagePointer = new PagePointer(
                            _pageName,
                            contextCode);
                if (commandText.Equals("backToDashboardLinkTacCode", StringComparison.OrdinalIgnoreCase))
                    pagePointer = new PagePointer(
                        "TacFarmDashboard",
                        rowData.BackToDashboardLinkTacCode);
                if (commandText.Equals("testFileDownloadLinkPacCode", StringComparison.OrdinalIgnoreCase))
                    if((await Services.PacUserTestAsyncFileDownload.GetResponse(apiClient,rowData.TestFileDownloadLinkPacCode)).Success)
                        pagePointer = new PagePointer(
                            _pageName,
                            contextCode);
                if (commandText.Equals("testConditionalAsyncFileDownloadLinkPacCode", StringComparison.OrdinalIgnoreCase))
                    if((await Services.PacUserTestAsyncFileDownload.GetResponse(apiClient,rowData.TestConditionalAsyncFileDownloadLinkPacCode)).Success)
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
                if (commandText.Equals("conditionalBtnExampleLinkTacCode", StringComparison.OrdinalIgnoreCase))
                    pagePointer = new PagePointer(
                        "TacFarmDashboard",
                        rowData.ConditionalBtnExampleLinkTacCode);
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

        public async Task<PlantUserDetailsListModel> RequestGetResponse(APIClient aPIClient, PlantUserDetailsListRequest model, Guid contextCode)
        {
            string url = $"/plant-user-details/{contextCode.ToString()}";

            // Serialize the model into a query string
            var queryString = ToQueryString(model);

            // Append the query string to the URL
            url = $"{url}?{queryString}";

            PlantUserDetailsListModel result = await aPIClient.GetAsync<PlantUserDetailsListModel>(url);

            return result;
        }

    }
}

