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
    public class LandPlantList : PageBase, IPage
    {
        public LandPlantList()
        {
            _pageName = "LandPlantList";
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

            pageView.PageTitleText = "Plant List";
            pageView.PageIntroText = "A list of plants on the land";  
            pageView.PageFooterText = "";

            pageView = AddDefaultAvailableCommands(pageView);

            var initReportProcessor = new LandPlantListInitReport();

            //  handle report init
            LandPlantListInitReport.LandPlantListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            LandPlantListListRequest apiRequestModel = new LandPlantListListRequest();

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

            //GENIF[visualizationType=DetailThreeColumn]Start
            apiRequestModel.PageNumber = 1;
            apiRequestModel.OrderByColumnName = "";
            apiRequestModel.OrderByDescending = false;
            //GENIF[visualizationType=DetailThreeColumn]End
            //GENIF[visualizationType=DetailTwoColumn]Start
            apiRequestModel.PageNumber = 1;
            apiRequestModel.OrderByColumnName = "";
            apiRequestModel.OrderByDescending = false;
            //GENIF[visualizationType=DetailTwoColumn]End
            //GENIF[visualizationType=Grid]Start
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
            //GENIF[visualizationType=Grid]End

            //  handle filter post
            LandPlantListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);



            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                LandPlantListListModelItem selectedItem = null;

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
                //PlantCode

                pageView = BuildTableHeader(pageView, "isEditAllowed",
                    isVisible: true,
                    "Edit Allowed");

                pageView = BuildTableHeader(pageView, "someIntVal",
                    isVisible: true,
                    "Int Val");

                pageView = BuildTableHeader(pageView, "someConditionalIntVal",
                    isVisible: true,
                    "Conditional Int Val");

                pageView = BuildTableHeader(pageView, "someBigIntVal",
                    isVisible: true,
                    "Big Int Val");

                pageView = BuildTableHeader(pageView, "someConditionalBigIntVal",
                    isVisible: true,
                    "Conditional Big Int Val");

                pageView = BuildTableHeader(pageView, "someBitVal",
                    isVisible: true,
                    "Bit Val");

                pageView = BuildTableHeader(pageView, "someConditionalBitVal",
                    isVisible: true,
                    "Conditional Bit Val");

                pageView = BuildTableHeader(pageView, "isDeleteAllowed",
                    isVisible: true,
                    "Delete Allowed");

                pageView = BuildTableHeader(pageView, "someFloatVal",
                    isVisible: true,
                    "Float Val");

                pageView = BuildTableHeader(pageView, "someConditionalFloatVal",
                    isVisible: true,
                    "Conditional Float Val");

                pageView = BuildTableHeader(pageView, "someDecimalVal",
                    isVisible: true,
                    "Decimal Val");

                pageView = BuildTableHeader(pageView, "someConditionalDecimalVal",
                    isVisible: true,
                    "Conditional Decimal Val");

                pageView = BuildTableHeader(pageView, "someUTCDateTimeVal",
                    isVisible: true,
                    "Date Time Val");

                pageView = BuildTableHeader(pageView, "someConditionalUTCDateTimeVal",
                    isVisible: true,
                    "Conditional Date Time Val");

                pageView = BuildTableHeader(pageView, "someDateVal",
                    isVisible: true,
                    "Date Val");

                pageView = BuildTableHeader(pageView, "someConditionalDateVal",
                    isVisible: true,
                    "Conditional Date Val");

                pageView = BuildTableHeader(pageView, "someMoneyVal",
                    isVisible: true,
                    "Money Val");

                pageView = BuildTableHeader(pageView, "someConditionalMoneyVal",
                    isVisible: true,
                    "Conditional Money Val");

                pageView = BuildTableHeader(pageView, "someNVarCharVal",
                    isVisible: true,
                    "N Var Char Val");

                pageView = BuildTableHeader(pageView, "someConditionalNVarCharVal",
                    isVisible: true,
                    "Conditional N Var Char Val");

                pageView = BuildTableHeader(pageView, "someVarCharVal",
                    isVisible: true,
                    "Var Char Val");

                pageView = BuildTableHeader(pageView, "someConditionalVarCharVal",
                    isVisible: true,
                    "Conditional Var Char Val");

                pageView = BuildTableHeader(pageView, "someTextVal",
                    isVisible: true,
                    "Text Val");

                pageView = BuildTableHeader(pageView, "someConditionalTextVal",
                    isVisible: true,
                    "Conditional Text Val");

                pageView = BuildTableHeader(pageView, "somePhoneNumber",
                    isVisible: true,
                    "Phone Number");

                pageView = BuildTableHeader(pageView, "someConditionalPhoneNumber",
                    isVisible: true,
                    "Conditional Phone Number");

                pageView = BuildTableHeader(pageView, "someEmailAddress",
                    isVisible: true,
                    "Email Address");

                pageView = BuildTableHeader(pageView, "someConditionalEmailAddress",
                    isVisible: true,
                    "Conditional Email Address");

                pageView = BuildTableHeader(pageView, "isImageUrlAvailable",
                    isVisible: false,
                    "Is Image Url Available");

                pageView = BuildTableHeader(pageView, "someImageUrlVal",
                    isVisible: true,
                    "Image Url");

                pageView = BuildTableHeader(pageView, "someConditionalImageUrl",
                    isVisible: true,
                    "Conditional Image Url");

                pageView = BuildTableHeader(pageView, "flavorName",
                    isVisible: true,
                    "Flavor Name");

                pageView = BuildTableHeader(pageView, "flavorCode",
                    isVisible: false,
                    "flavor Code");

                pageView = BuildTableHeader(pageView, "someIntConditionalOnDeletable",
                    isVisible: true,
                    "Int Conditional");

                pageView = BuildTableHeader(pageView, "nVarCharAsUrl",
                    isVisible: true,
                    "N Var Char As Url");

                pageView = BuildTableHeader(pageView, "nVarCharConditionalAsUrl",
                    isVisible: true,
                    "Conditional N Var Char As Url");

                //updateLinkPlantCode

                //deleteAsyncButtonLinkPlantCode

                //detailsLinkPlantCode

                //testFileDownloadLinkPacCode

                //testConditionalFileDownloadLinkPacCode

                //testAsyncFlowReqLinkPacCode

                //testConditionalAsyncFlowReqLinkPacCode

                //conditionalBtnExampleLinkPlantCode

            }

            return pageView;
        }


        public PageView BuildTableData(SessionData sessionData, PageView pageView, LandPlantListListModel apiResponse)
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

        public Dictionary<string, string> BuildTableDataRow(LandPlantListListModelItem rowData)
        {
            Dictionary<string,string> keyValuePairs = new Dictionary<string, string>();

            {
                //PlantCode

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "isEditAllowed",
                    isVisible: true,
                    value: rowData.IsEditAllowed.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someIntVal",
                    isVisible: true,
                    value: rowData.SomeIntVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalIntVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalIntVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someBigIntVal",
                    isVisible: true,
                    value: rowData.SomeBigIntVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalBigIntVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalBigIntVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someBitVal",
                    isVisible: true,
                    value: rowData.SomeBitVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalBitVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalBitVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "isDeleteAllowed",
                    isVisible: true,
                    value: rowData.IsDeleteAllowed.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someFloatVal",
                    isVisible: true,
                    value: rowData.SomeFloatVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalFloatVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalFloatVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someDecimalVal",
                    isVisible: true,
                    value: rowData.SomeDecimalVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalDecimalVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalDecimalVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someUTCDateTimeVal",
                    isVisible: true,
                    value: rowData.SomeUTCDateTimeVal.ToString("yyyy-MM-ddTHH:mm:ss"));

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalUTCDateTimeVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalUTCDateTimeVal.ToString("yyyy-MM-ddTHH:mm:ss"));

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someDateVal",
                    isVisible: true,
                    value: rowData.SomeDateVal.ToString("yyyy-MM-ddTHH:mm:ss"));

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalDateVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalDateVal.ToString("yyyy-MM-ddTHH:mm:ss"));

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someMoneyVal",
                    isVisible: true,
                    value: rowData.SomeMoneyVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalMoneyVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalMoneyVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someNVarCharVal",
                    isVisible: true,
                    value: rowData.SomeNVarCharVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalNVarCharVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalNVarCharVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someVarCharVal",
                    isVisible: true,
                    value: rowData.SomeVarCharVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalVarCharVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalVarCharVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someTextVal",
                    isVisible: true,
                    value: rowData.SomeTextVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalTextVal",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalTextVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "somePhoneNumber",
                    isVisible: true,
                    value: rowData.SomePhoneNumber.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalPhoneNumber",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalPhoneNumber.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someEmailAddress",
                    isVisible: true,
                    value: rowData.SomeEmailAddress.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalEmailAddress",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.SomeConditionalEmailAddress.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "isImageUrlAvailable",
                    isVisible: false,
                    value: rowData.IsImageUrlAvailable.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someImageUrlVal",
                    isVisible: true,
                    value: rowData.SomeImageUrlVal.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someConditionalImageUrl",
                    isVisible: true,
                    conditionallyVisible: rowData.IsImageUrlAvailable,
                    value: rowData.SomeConditionalImageUrl.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "flavorName",
                    isVisible: true,
                    value: rowData.FlavorName.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "flavorCode",
                    isVisible: false,
                    value: rowData.FlavorCode.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "someIntConditionalOnDeletable",
                    isVisible: true,
                    conditionallyVisible: rowData.IsDeleteAllowed,
                    value: rowData.SomeIntConditionalOnDeletable.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "nVarCharAsUrl",
                    isVisible: true,
                    value: rowData.NVarCharAsUrl.ToString());

                keyValuePairs = BuildTableDataCellValue(keyValuePairs, "nVarCharConditionalAsUrl",
                    isVisible: true,
                    conditionallyVisible: rowData.IsEditAllowed,
                    value: rowData.NVarCharConditionalAsUrl.ToString());

                //updateLinkPlantCode

                //deleteAsyncButtonLinkPlantCode

                //detailsLinkPlantCode

                //testFileDownloadLinkPacCode

                //testConditionalFileDownloadLinkPacCode

                //testAsyncFlowReqLinkPacCode

                //testConditionalAsyncFlowReqLinkPacCode

                //conditionalBtnExampleLinkPlantCode

            }

            return keyValuePairs;
        }

        public PageView BuildAvailableCommandsForReportSort(PageView pageView, LandPlantListListModel apiResponse)
        {
            if (apiResponse == null ||
                apiResponse.Items == null ||
                apiResponse.Items.Count < 2)
            {
                return pageView;
            }

            {
                //PlantCode

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "isEditAllowed", 
                    isVisible: true,
                    "Edit Allowed");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someIntVal",
                    isVisible: true,
                    "Int Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalIntVal",
                    isVisible: true,
                    "Conditional Int Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someBigIntVal",
                    isVisible: true,
                    "Big Int Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalBigIntVal",
                    isVisible: true,
                    "Conditional Big Int Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someBitVal",
                    isVisible: true,
                    "Bit Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalBitVal",
                    isVisible: true,
                    "Conditional Bit Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "isDeleteAllowed",
                    isVisible: true,
                    "Delete Allowed");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someFloatVal",
                    isVisible: true,
                    "Float Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalFloatVal",
                    isVisible: true,
                    "Conditional Float Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someDecimalVal",
                    isVisible: true,
                    "Decimal Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalDecimalVal",
                    isVisible: true,
                    "Conditional Decimal Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someUTCDateTimeVal",
                    isVisible: true,
                    "Date Time Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalUTCDateTimeVal",
                    isVisible: true,
                    "Conditional Date Time Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someDateVal",
                    isVisible: true,
                    "Date Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalDateVal",
                    isVisible: true,
                    "Conditional Date Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someMoneyVal",
                    isVisible: true,
                    "Money Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalMoneyVal",
                    isVisible: true,
                    "Conditional Money Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someNVarCharVal",
                    isVisible: true,
                    "N Var Char Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalNVarCharVal",
                    isVisible: true,
                    "Conditional N Var Char Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someVarCharVal",
                    isVisible: true,
                    "Var Char Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalVarCharVal",
                    isVisible: true,
                    "Conditional Var Char Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someTextVal",
                    isVisible: true,
                    "Text Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalTextVal",
                    isVisible: true,
                    "Conditional Text Val");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "somePhoneNumber",
                    isVisible: true,
                    "Phone Number");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalPhoneNumber",
                    isVisible: true,
                    "Conditional Phone Number");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someEmailAddress",
                    isVisible: true,
                    "Email Address");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalEmailAddress",
                    isVisible: true,
                    "Conditional Email Address");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "isImageUrlAvailable",
                    isVisible: false,
                    "Is Image Url Available");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someImageUrlVal",
                    isVisible: true,
                    "Image Url");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someConditionalImageUrl",
                    isVisible: true,
                    "Conditional Image Url");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "flavorName",
                    isVisible: true,
                    "Flavor Name");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "flavorCode",
                    isVisible: false,
                    "flavor Code");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "someIntConditionalOnDeletable",
                    isVisible: true,
                    "Int Conditional");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "nVarCharAsUrl",
                    isVisible: true,
                    "N Var Char As Url");

                pageView = BuildAvailableCommandForSortOnColumn(pageView, "nVarCharConditionalAsUrl",
                    isVisible: true,
                    "Conditional N Var Char As Url");

                //updateLinkPlantCode

                //deleteAsyncButtonLinkPlantCode

                //detailsLinkPlantCode

                //testFileDownloadLinkPacCode

                //testConditionalFileDownloadLinkPacCode

                //testAsyncFlowReqLinkPacCode

                //testConditionalAsyncFlowReqLinkPacCode

                //conditionalBtnExampleLinkPlantCode

            }

            return pageView;
        }

        public PageView BuildAvailableCommandsForReportRowButtons(PageView pageView, LandPlantListListModel apiResponse)
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
                pageView = BuildAvailableCommandForReportButton(pageView, "updateLinkPlantCode",
                    "PlantUserDetails",
                    "updateLinkPlantCode",
                    isVisible: false,
                    isEnabled: true,
                    "Update");

                pageView = BuildAvailableCommandForReportButton(pageView, "deleteAsyncButtonLinkPlantCode",
                    "PlantUserDetails",
                    "deleteAsyncButtonLinkPlantCode",
                    isVisible: true,
                    isEnabled: true,
                    "Delete");

                pageView = BuildAvailableCommandForReportButton(pageView, "detailsLinkPlantCode",
                    "LandAddPlant",
                    "detailsLinkPlantCode",
                    isVisible: true,
                    isEnabled: true,
                    "Details");

                pageView = BuildAvailableCommandForReportButton(pageView, "testFileDownloadLinkPacCode",
                    "LandAddPlant",
                    "testFileDownloadLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test File Download");

                pageView = BuildAvailableCommandForReportButton(pageView, "testConditionalFileDownloadLinkPacCode",
                    "LandAddPlant",
                    "testConditionalFileDownloadLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Conditional File Download",
                    conditionallyVisible: rowData.IsEditAllowed
                    );

                pageView = BuildAvailableCommandForReportButton(pageView, "testAsyncFlowReqLinkPacCode",
                    "LandAddPlant",
                    "testAsyncFlowReqLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Async Flow Req");

                pageView = BuildAvailableCommandForReportButton(pageView, "testConditionalAsyncFlowReqLinkPacCode",
                    "LandPlantList",
                    "testConditionalAsyncFlowReqLinkPacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Test Conditional Async Flow Req",
                    conditionallyVisible: rowData.IsEditAllowed
                    );

                pageView = BuildAvailableCommandForReportButton(pageView, "conditionalBtnExampleLinkPlantCode",
                    "PlantUserDetails",
                    "conditionalBtnExampleLinkPlantCode",
                    isVisible: true,
                    isEnabled: true,
                    "Conditional Btn Example",
                    conditionallyVisible: rowData.IsEditAllowed
                );
            } 

            return pageView;
        }

        public PageView BuildAvailableCommandsForReportButtons(PageView pageView)
        {

            //GENIF[visualizationType=Grid]Start
            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "ClearFilters", Description = "Clear all filters" }
                );

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "PageNumber:[page number value (or empty to remove filter)]", Description = "View a particular page of the report results" }
                );

            pageView.AvailableCommands.Add(
                new AvailableCommand { CommandText = "RowNumber:[row number value (or empty to remove filter)]", Description = "View a single row of the report results. More commands may then be available for that row." }
                );
            //GENIF[visualizationType=Grid]End

            {
                pageView = BuildAvailableCommandForReportButton(pageView, "backButton",
                    "TacFarmDashboard",
                    "TacCode",
                    isVisible: true,
                    isEnabled: true,
                    "Farm Dashboard");

                pageView = BuildAvailableCommandForReportButton(pageView, "addButton",
                    "LandAddPlant",
                    "LandCode",
                    isVisible: true,
                    isEnabled: true,
                    "Add A Plant");

                pageView = BuildAvailableCommandForReportButton(pageView, "otherAddButton",
                    "LandAddPlant",
                    "LandCode",
                    isVisible: true,
                    isEnabled: true,
                    "Other Add Button");
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

            var initReportProcessor = new LandPlantListInitReport();

            LandPlantListInitReport.LandPlantListGetInitResponse apiInitResponse = await initReportProcessor.GetInitResponse(apiClient, contextCode);

            string json = JsonConvert.SerializeObject(apiInitResponse);

            JObject jsonObject = JObject.Parse(json);

            Dictionary<string, object> navDictionary = jsonObject.ToObject<Dictionary<string, object>>();

            if (!navDictionary.ContainsKey("LandCode"))
            {
                navDictionary.Add("LandCode", contextCode);
            }

            //  handle report buttons
            {
                if (commandText.Equals("backButton", StringComparison.OrdinalIgnoreCase))
                    pagePointer = new PagePointer(
                        "TacFarmDashboard",
                        Guid.Parse(navDictionary["tacCode"].ToString()));

                if (commandText.Equals("addButton", StringComparison.OrdinalIgnoreCase))
                    pagePointer = new PagePointer(
                        "LandAddPlant",
                        Guid.Parse(navDictionary["landCode"].ToString()));

                if (commandText.Equals("otherAddButton", StringComparison.OrdinalIgnoreCase))
                    pagePointer = new PagePointer(
                        "LandAddPlant",
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

            LandPlantListListRequest apiRequestModel = new LandPlantListListRequest();

            MergeProperties(apiRequestModel, apiInitResponse);

            MergeProperties(apiRequestModel, postData);
             
            LandPlantListListModel apiResponse = await GetResponse(apiClient, apiRequestModel, contextCode);

            if (sessionData.Filters.ContainsKey("rowNumber"))
            {
                int rowNumber = (apiResponse.ItemCountPerPage * (apiResponse.PageNumber - 1)) + 1;

                int rowNumberToSelect = 0;

                rowNumberToSelect = int.Parse(sessionData.Filters["rowNumber"]);

                LandPlantListListModelItem selectedItem = null;

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

            if (commandText.Equals("updateLinkPlantCode", StringComparison.OrdinalIgnoreCase)) 
                pagePointer = new PagePointer(
                    "PlantUserDetails",
                    rowData.UpdateLinkPlantCode);

            if (commandText.Equals("deleteAsyncButtonLinkPlantCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(  //TODO handle async objwf
                    "LandPlantList",
                    contextCode);

            if (commandText.Equals("detailsLinkPlantCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer(
                    "PlantUserDetails",
                    rowData.DetailsLinkPlantCode);

            if (commandText.Equals("testFileDownloadLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer( //TODO handle async objwf
                    "LandPlantList",
                    contextCode);

            if (commandText.Equals("testConditionalFileDownloadLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer( //TODO handle async objwf
                    "LandPlantList",
                    contextCode);

            if (commandText.Equals("testAsyncFlowReqLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer( //TODO handle async objwf
                    "LandPlantList",
                    contextCode);

            if (commandText.Equals("testConditionalAsyncFlowReqLinkPacCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer( //TODO handle async objwf
                    "LandPlantList",
                    contextCode);

            if (commandText.Equals("conditionalBtnExampleLinkPlantCode", StringComparison.OrdinalIgnoreCase))
                pagePointer = new PagePointer( //TODO handle async objwf
                    "LandPlantList",
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


        public async Task<LandPlantListListModel> GetResponse(APIClient aPIClient, LandPlantListListRequest model, Guid contextCode)
        {
            string url = $"/land-plant-list/{contextCode.ToString()}";

            // Serialize the model into a query string
            var queryString = ToQueryString(model);

            // Append the query string to the URL
            url = $"{url}?{queryString}";

            LandPlantListListModel result = await aPIClient.GetAsync<LandPlantListListModel>(url);

            return result;
        }

        public class LandPlantListListModel
        {
            [Newtonsoft.Json.JsonProperty("pageNumber", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int PageNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("items", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Collections.Generic.ICollection<LandPlantListListModelItem> Items { get; set; }

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
            public LandPlantListListRequest Request { get; set; } 

        }

        
        public class LandPlantListListModelItem
        {
            [Newtonsoft.Json.JsonProperty("plantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid PlantCode { get; set; }

            [Newtonsoft.Json.JsonProperty("someIntVal", Required = Newtonsoft.Json.Required.Always)]
            public int SomeIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalIntVal", Required = Newtonsoft.Json.Required.Always)]
            public int SomeConditionalIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someBigIntVal", Required = Newtonsoft.Json.Required.Always)]
            public long SomeBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalBigIntVal", Required = Newtonsoft.Json.Required.Always)]
            public long SomeConditionalBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someBitVal", Required = Newtonsoft.Json.Required.Always)]
            public bool SomeBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalBitVal", Required = Newtonsoft.Json.Required.Always)]
            public bool SomeConditionalBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("isEditAllowed", Required = Newtonsoft.Json.Required.Always)]
            public bool IsEditAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("isDeleteAllowed", Required = Newtonsoft.Json.Required.Always)]
            public bool IsDeleteAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("someFloatVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalFloatVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someDecimalVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalDecimalVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someUTCDateTimeVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public DateTime SomeUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalUTCDateTimeVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public DateTime SomeConditionalUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someDateVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public DateTime SomeDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalDateVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public DateTime SomeConditionalDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someMoneyVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalMoneyVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someNVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalNVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalVarCharVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someTextVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalTextVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("somePhoneNumber", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomePhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalPhoneNumber", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalPhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("someEmailAddress", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalEmailAddress", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("flavorName", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string FlavorName { get; set; }

            [Newtonsoft.Json.JsonProperty("flavorCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid FlavorCode { get; set; }

            [Newtonsoft.Json.JsonProperty("someIntConditionalOnDeletable", Required = Newtonsoft.Json.Required.Always)]
            public int SomeIntConditionalOnDeletable { get; set; }

            [Newtonsoft.Json.JsonProperty("nVarCharAsUrl", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string NVarCharAsUrl { get; set; }

            [Newtonsoft.Json.JsonProperty("nVarCharConditionalAsUrl", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string NVarCharConditionalAsUrl { get; set; }

            [Newtonsoft.Json.JsonProperty("updateLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid UpdateLinkPlantCode { get; set; }

            [Newtonsoft.Json.JsonProperty("deleteAsyncButtonLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid DeleteAsyncButtonLinkPlantCode { get; set; }

            [Newtonsoft.Json.JsonProperty("detailsLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid DetailsLinkPlantCode { get; set; }

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

            [Newtonsoft.Json.JsonProperty("conditionalBtnExampleLinkPlantCode", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public System.Guid ConditionalBtnExampleLinkPlantCode { get; set; }

            [Newtonsoft.Json.JsonProperty("isImageUrlAvailable", Required = Newtonsoft.Json.Required.Always)]
            public bool IsImageUrlAvailable { get; set; }

            [Newtonsoft.Json.JsonProperty("someImageUrlVal", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeImageUrlVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someConditionalImageUrl", Required = Newtonsoft.Json.Required.Always)]
            [System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
            public string SomeConditionalImageUrl { get; set; } 

        }

        
        public class LandPlantListListRequest
        {
            [Newtonsoft.Json.JsonProperty("flavorFilterCode", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public System.Guid FlavorFilterCode { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterIntVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public int SomeFilterIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterBigIntVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public long SomeFilterBigIntVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterFloatVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterFloatVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterBitVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool SomeFilterBitVal { get; set; }

            [Newtonsoft.Json.JsonProperty("isFilterEditAllowed", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool IsFilterEditAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("isFilterDeleteAllowed", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public bool IsFilterDeleteAllowed { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterDecimalVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterDecimalVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someMinUTCDateTimeVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public DateTime SomeMinUTCDateTimeVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someMinDateVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public DateTime SomeMinDateVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterMoneyVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterMoneyVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterNVarCharVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterNVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterVarCharVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterVarCharVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterTextVal", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterTextVal { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterPhoneNumber", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterPhoneNumber { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterEmailAddress", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
            public string SomeFilterEmailAddress { get; set; }

            [Newtonsoft.Json.JsonProperty("someFilterUniqueIdentifier", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
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
