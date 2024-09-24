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
        public PageView BuildPageView(Guid sessionCode, Guid contextCode)
        {
            var pageView = new PageView();

            pageView.PageTitleText = "Main Menu";

            pageView.AvailableCommands.Add(
                new AvailableCommand(
                    "Dashboard",
                    "Dashboard",
                    "Return to your dashboard"
                )
            );

            //TODO need to know if admin or config rol is available

            pageView = this.AddDefaultAvailableCommands(pageView);

            return pageView;
        }

        public PagePointer ProcessCommand(Guid sessionCode, Guid contextCode, string commandText, string postData = "")
        {
            PagePointer pagePointer = this.ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            } 

            pagePointer = new PagePointer(this._pageName, contextCode);
            

            if(commandText == "Dashboard")
            {
                pagePointer.PageName = "TacFarmDashboard";
                pagePointer.ContextCode = Guid.Empty;
            }

            return pagePointer;
        }
    }
}
