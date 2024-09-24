using FS.Farm.WebNavigator.Page.Reports;
using FS.Farm.WebNavigator.Page.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
//endset
                case "LandPlantList":
                    return new LandPlantList();
                case "TacFarmDashboard":
                    return new TacFarmDashboard();
                case "PlantUserDetails":
                    return new PlantUserDetails();
                case "LandAddPlant":
                    return new LandAddPlant();
                case "TacLogin":
                    return new TacLogin();
                case "TacRegister":
                    return new TacRegister();
//endset
                default:
                    return new MainMenu();
            }
        }

    }
}
