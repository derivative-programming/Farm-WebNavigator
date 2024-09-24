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
    public static class PageFactory
    {
        public static IPage GetPage(string pageName)
        {
            switch (pageName)
            {
                case "MainMenu":
                    return new MainMenu(); 

                //GENLOOPObjectStart
                //GENTrainingBlock[c2]Start
                //GENLearn[modelType=object,name=Land]Start
                //GENLOOPReportStart
                //GENTrainingBlock[d2]Start
                //GENLearn[modelType=report,name=LandPlantList]Start
                case "LandPlantList":
                    return new LandPlantList();
                //GENLearn[modelType=report,name=LandPlantList]End
                //GENTrainingBlock[d2]End
                //GENLOOPReportEnd
                //GENLearn[modelType=object,name=Land]End 
                case "TacFarmDashboard":
                    return new TacFarmDashboard();
                case "PlantUserDetails":
                    return new PlantUserDetails(); 
                //case "PacConfigDynaFlowDFTBuildToDoList":
                //    return new PacConfigDynaFlowDFTBuildToDoList();
                //case "PacConfigDynaFlowRetryTaskBuildList":
                //    return new PacConfigDynaFlowRetryTaskBuildList();
                //case "PacConfigDynaFlowTaskRetryRunList":
                //    return new PacConfigDynaFlowTaskRetryRunList();
                //case "PacConfigDynaFlowTaskRunToDoList":
                //    return new PacConfigDynaFlowTaskRunToDoList();
                //case "PacConfigDynaFlowTaskSearch":
                //    return new PacConfigDynaFlowTaskSearch();
                case "PacUserDateGreaterThanFilterList":
                    return new PacUserDateGreaterThanFilterList();
                //case "PacUserDynaFlowTaskTypeList":
                //    return new PacUserDynaFlowTaskTypeList();
                //case "PacUserDynaFlowTypeList":
                //    return new PacUserDynaFlowTypeList();
                case "PacUserFlavorList":
                    return new PacUserFlavorList();
                case "PacUserLandList":
                    return new PacUserLandList();
                case "PacUserRoleList":
                    return new PacUserRoleList();
                case "PacUserTacList":
                    return new PacUserTacList();
                case "PacUserTriStateFilterList":
                    return new PacUserTriStateFilterList();
                //GENTrainingBlock[c2]End
                //GENLOOPObjectEnd

                //GENLOOPObjectStart
                //GENTrainingBlock[a2]Start
                //GENLearn[modelType=object,name=Land]Start
                //GENLOOPObjectWorkflowStart
                //GENTrainingBlock[b2]Start
                //GENLearn[modelType=objectWorkflow,name=LandAddPlant,calculatedIsPage=true]Start
                case "LandAddPlant":
                    return new LandAddPlant();
                //GENLearn[modelType=objectWorkflow,name=LandAddPlant,calculatedIsPage=true]End
                //GENTrainingBlock[b2]End
                //GENLOOPObjectWorkflowEnd
                //GENLearn[modelType=object,name=Land]End 
                case "TacLogin":
                    return new TacLogin();
                case "TacRegister":
                    return new TacRegister();
                //GENTrainingBlock[a2]End
                //GENLOOPObjectEnd 

                default:
                    return new MainMenu();
            }
        }

    }
}
