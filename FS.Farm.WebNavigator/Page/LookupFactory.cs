using FS.Farm.WebNavigator.Page.Reports;
using FS.Farm.WebNavigator.Page.Forms;
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
                case "DateGreaterThanFilter":
                    PacUserDateGreaterThanFilterList.PacUserDateGreaterThanFilterListListRequest pacUserDateGreaterThanFilterListListRequest =
                        new PacUserDateGreaterThanFilterList.PacUserDateGreaterThanFilterListListRequest();
                    pacUserDateGreaterThanFilterListListRequest.PageNumber = 1;
                    pacUserDateGreaterThanFilterListListRequest.ItemCountPerPage = 100;
                    PacUserDateGreaterThanFilterList pacUserDateGreaterThanFilterList = new PacUserDateGreaterThanFilterList();
                    PacUserDateGreaterThanFilterList.PacUserDateGreaterThanFilterListListModel pacUserDateGreaterThanFilterListListModel =
                        await pacUserDateGreaterThanFilterList.GetResponse(apiClient, pacUserDateGreaterThanFilterListListRequest, Guid.Empty);
                    foreach(var item in pacUserDateGreaterThanFilterListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.DateGreaterThanFilterName, Value = item.DateGreaterThanFilterCode.ToString() });
                    }
                    break;
                //GENLearn[modelType=object,name=DateGreaterThanFilter,isLookup=true]End  
                case "Flavor":
                    PacUserFlavorList.PacUserFlavorListListRequest pacUserFlavorListListRequest =
                        new PacUserFlavorList.PacUserFlavorListListRequest();
                    pacUserFlavorListListRequest.PageNumber = 1;
                    pacUserFlavorListListRequest.ItemCountPerPage = 100;
                    PacUserFlavorList pacUserFlavorList = new PacUserFlavorList();
                    PacUserFlavorList.PacUserFlavorListListModel pacUserFlavorListListModel =
                        await pacUserFlavorList.GetResponse(apiClient, pacUserFlavorListListRequest, Guid.Empty);
                    foreach (var item in pacUserFlavorListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.FlavorName, Value = item.FlavorCode.ToString() });
                    }
                    break;
                case "Land":
                    PacUserLandList.PacUserLandListListRequest pacUserLandListListRequest =
                        new PacUserLandList.PacUserLandListListRequest();
                    pacUserLandListListRequest.PageNumber = 1;
                    pacUserLandListListRequest.ItemCountPerPage = 100;
                    PacUserLandList pacUserLandList = new PacUserLandList();
                    PacUserLandList.PacUserLandListListModel pacUserLandListListModel =
                        await pacUserLandList.GetResponse(apiClient, pacUserLandListListRequest, Guid.Empty);
                    foreach (var item in pacUserLandListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.LandName, Value = item.LandCode.ToString() });
                    }
                    break;
                case "Role":
                    PacUserRoleList.PacUserRoleListListRequest pacUserRoleListListRequest =
                        new PacUserRoleList.PacUserRoleListListRequest();
                    pacUserRoleListListRequest.PageNumber = 1;
                    pacUserRoleListListRequest.ItemCountPerPage = 100;
                    PacUserRoleList pacUserRoleList = new PacUserRoleList();
                    PacUserRoleList.PacUserRoleListListModel pacUserRoleListListModel =
                        await pacUserRoleList.GetResponse(apiClient, pacUserRoleListListRequest, Guid.Empty);
                    foreach (var item in pacUserRoleListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.RoleName, Value = item.RoleCode.ToString() });
                    }
                    break;
                case "Tac":
                    PacUserTacList.PacUserTacListListRequest pacUserTacListListRequest =
                        new PacUserTacList.PacUserTacListListRequest();
                    pacUserTacListListRequest.PageNumber = 1;
                    pacUserTacListListRequest.ItemCountPerPage = 100;
                    PacUserTacList pacUserTacList = new PacUserTacList();
                    PacUserTacList.PacUserTacListListModel pacUserTacListListModel =
                        await pacUserTacList.GetResponse(apiClient, pacUserTacListListRequest, Guid.Empty);
                    foreach (var item in pacUserTacListListModel.Items)
                    {
                        result.Add(new LookupItem() { Label = item.TacName, Value = item.TacCode.ToString() });
                    }
                    break;
                case "TriStateFilter":
                    PacUserTriStateFilterList.PacUserTriStateFilterListListRequest pacUserTriStateFilterListListRequest =
                        new PacUserTriStateFilterList.PacUserTriStateFilterListListRequest();
                    pacUserTriStateFilterListListRequest.PageNumber = 1;
                    pacUserTriStateFilterListListRequest.ItemCountPerPage = 100;
                    PacUserTriStateFilterList pacUserTriStateFilterList = new PacUserTriStateFilterList();
                    PacUserTriStateFilterList.PacUserTriStateFilterListListModel pacUserTriStateFilterListListModel =
                        await pacUserTriStateFilterList.GetResponse(apiClient, pacUserTriStateFilterListListRequest, Guid.Empty);
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
