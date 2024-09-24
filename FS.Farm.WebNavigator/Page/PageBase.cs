using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Page
{
    public class PageBase
    {
        protected string _pageName = string.Empty;

        protected PageView AddDefaultAvailableCommands(PageView pageView)
        {
            pageView.AvailableCommands.Add(new AvailableCommand { CommandText = "MM", CommandTitle = "Main Menu", CommandDescription = "Go To Main Menu" });
            pageView.AvailableCommands.Add(new AvailableCommand { CommandText = "RP", CommandTitle = "Refresh Page", CommandDescription = "Refresh the current page" });

            return pageView;
        }

        protected PagePointer ProcessDefaultCommands(string commandText, Guid contextCode)
        {
            PagePointer result = null;

            if (commandText == "MM")
            {
                result = new PagePointer("MainMenu", Guid.Empty);
            }

            if (commandText == "RP")
            {
                result = new PagePointer(this._pageName, contextCode);
            }

            return result;
        }
    }
}
