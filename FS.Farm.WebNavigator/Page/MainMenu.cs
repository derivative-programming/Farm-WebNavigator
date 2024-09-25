using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Page
{
    public class MainMenu: PageBase, IPage
    { 
        public MainMenu()
        {
            this._pageName = "MainMenu";
        }
        public async Task<PageView> BuildPageView(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText = "", string postData = "")
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Main Menu";

            pageView.AvailableCommands.Add(
                new AvailableCommand(
                    "Dashboard", 
                    "Return to your dashboard"
                )
            );

            pageView.AvailableCommands.Add(
                new AvailableCommand(
                    "Admin",
                    "Go To Admin Dashboard"
                )
            );

            pageView.AvailableCommands.Add(
                new AvailableCommand(
                    "Config",
                    "Go To Config Dashboard"
                )
            );

            //TODO need to know if admin or config rol is available

            pageView = this.AddDefaultAvailableCommands(pageView);

            return pageView;
        }

        public async Task<PagePointer> ProcessCommand(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText, string postData = "")
        {
            PagePointer pagePointer = this.ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            } 

            pagePointer = new PagePointer(this._pageName, contextCode);
            

            if(commandText.Equals("Dashboard",StringComparison.OrdinalIgnoreCase))
            {
                pagePointer.PageName = "TacFarmDashboard";
                pagePointer.ContextCode = Guid.Empty;
            }

            if (commandText.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                pagePointer.PageName = "CustomerAdminDashboard";
                pagePointer.ContextCode = Guid.Empty;
            }

            if (commandText.Equals("Config", StringComparison.OrdinalIgnoreCase))
            {
                pagePointer.PageName = "PacConfigDashboard";
                pagePointer.ContextCode = Guid.Empty;
            }

            return pagePointer;
        }
    }
}
