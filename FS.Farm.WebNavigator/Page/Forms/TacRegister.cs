using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator.Page.Forms
{
    public class TacRegister : PageBase, IPage
    {
        public TacRegister()
        {
            _pageName = "TacRegister";
        }
        public PageView BuildPageView(Guid sessionCode, Guid contextCode)
        {
            var pageView = new PageView();

            pageView.PageTitleText = ""; //TODO
            pageView.PageIntroText = ""; //TODO
            pageView.PageFooterText = ""; //TODO

            pageView = AddDefaultAvailableCommands(pageView);

            return pageView;
        }

        public PagePointer ProcessCommand(Guid sessionCode, Guid contextCode, string commandText, string postData = "")
        {
            PagePointer pagePointer = ProcessDefaultCommands(commandText, contextCode);

            if (pagePointer != null)
            {
                return pagePointer;
            }

            pagePointer = new PagePointer(_pageName, contextCode);

            return pagePointer;
        }
    }
}
