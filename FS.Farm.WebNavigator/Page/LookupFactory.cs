using FS.Farm.WebNavigator.Page.Reports;
using FS.Farm.WebNavigator.Page.Reports.Models;
using FS.Farm.WebNavigator.Page.Forms;
using FS.Farm.WebNavigator.Page.Forms.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Xml.Linq;

namespace FS.Farm.WebNavigator.Page
{
    public static class LookupFactory
    {
        public static async Task<List<LookupItem>> GetLookupItems(APIClient apiClient, string lookupName)
        {
            List<LookupItem > result = new List<LookupItem>();

            switch (lookupName)
            {  
                //GENLOOPObjectStart 
                //GENTrainingBlock[c2]Start
                //GENLearn[modelType=object,name=DateGreaterThanFilter,isLookup=true]Start  
                //GENIF[name!=Pac]Start
                case "DateGreaterThanFilter":
                    PacUserDateGreaterThanFilterListListRequest pacUserDateGreaterThanFilterListListRequest =
                        new PacUserDateGreaterThanFilterListListRequest();
                    pacUserDateGreaterThanFilterListListRequest.PageNumber = 1;
                    pacUserDateGreaterThanFilterListListRequest.ItemCountPerPage = 100;
                    PacUserDateGreaterThanFilterList pacUserDateGreaterThanFilterList = new PacUserDateGreaterThanFilterList();
                    PacUserDateGreaterThanFilterListListModel pacUserDateGreaterThanFilterListListModel =
                        await pacUserDateGreaterThanFilterList.RequestGetResponse(apiClient, pacUserDateGreaterThanFilterListListRequest, Guid.Empty);
                    foreach(var item in pacUserDateGreaterThanFilterListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.DateGreaterThanFilterName, Value = item.DateGreaterThanFilterCode.ToString() });
                    }
                    break;
                //GENIF[name!=Pac]End
                //GENLearn[modelType=object,name=DateGreaterThanFilter,isLookup=true]End  
                case "Flavor":
                    PacUserFlavorListListRequest pacUserFlavorListListRequest =
                        new PacUserFlavorListListRequest();
                    pacUserFlavorListListRequest.PageNumber = 1;
                    pacUserFlavorListListRequest.ItemCountPerPage = 100;
                    PacUserFlavorList pacUserFlavorList = new PacUserFlavorList();
                    PacUserFlavorListListModel pacUserFlavorListListModel =
                        await pacUserFlavorList.RequestGetResponse(apiClient, pacUserFlavorListListRequest, Guid.Empty);
                    foreach (var item in pacUserFlavorListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.FlavorName, Value = item.FlavorCode.ToString() });
                    }
                    break;
                case "Land":
                    PacUserLandListListRequest pacUserLandListListRequest =
                        new PacUserLandListListRequest();
                    pacUserLandListListRequest.PageNumber = 1;
                    pacUserLandListListRequest.ItemCountPerPage = 100;
                    PacUserLandList pacUserLandList = new PacUserLandList();
                    PacUserLandListListModel pacUserLandListListModel =
                        await pacUserLandList.RequestGetResponse(apiClient, pacUserLandListListRequest, Guid.Empty);
                    foreach (var item in pacUserLandListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.LandName, Value = item.LandCode.ToString() });
                    }
                    break;
                case "Role":
                    PacUserRoleListListRequest pacUserRoleListListRequest =
                        new PacUserRoleListListRequest();
                    pacUserRoleListListRequest.PageNumber = 1;
                    pacUserRoleListListRequest.ItemCountPerPage = 100;
                    PacUserRoleList pacUserRoleList = new PacUserRoleList();
                    PacUserRoleListListModel pacUserRoleListListModel =
                        await pacUserRoleList.RequestGetResponse(apiClient, pacUserRoleListListRequest, Guid.Empty);
                    foreach (var item in pacUserRoleListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.RoleName, Value = item.RoleCode.ToString() });
                    }
                    break;
                case "Tac":
                    PacUserTacListListRequest pacUserTacListListRequest =
                        new PacUserTacListListRequest();
                    pacUserTacListListRequest.PageNumber = 1;
                    pacUserTacListListRequest.ItemCountPerPage = 100;
                    PacUserTacList pacUserTacList = new PacUserTacList();
                    PacUserTacListListModel pacUserTacListListModel =
                        await pacUserTacList.RequestGetResponse(apiClient, pacUserTacListListRequest, Guid.Empty);
                    foreach (var item in pacUserTacListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.TacName, Value = item.TacCode.ToString() });
                    }
                    break;
                case "TriStateFilter":
                    PacUserTriStateFilterListListRequest pacUserTriStateFilterListListRequest =
                        new PacUserTriStateFilterListListRequest();
                    pacUserTriStateFilterListListRequest.PageNumber = 1;
                    pacUserTriStateFilterListListRequest.ItemCountPerPage = 100;
                    PacUserTriStateFilterList pacUserTriStateFilterList = new PacUserTriStateFilterList();
                    PacUserTriStateFilterListListModel pacUserTriStateFilterListListModel =
                        await pacUserTriStateFilterList.RequestGetResponse(apiClient, pacUserTriStateFilterListListRequest, Guid.Empty);
                    foreach (var item in pacUserTriStateFilterListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.TriStateFilterName, Value = item.TriStateFilterCode.ToString() });
                    }
                    break;
                //GENTrainingBlock[c2]End 
                //GENLOOPObjectEnd 

                default:break;
            }

            return result;
        }

    }
}
