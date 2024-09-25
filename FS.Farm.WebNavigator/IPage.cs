using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator
{
    public interface IPage
    {
        Task<PageView> BuildPageView(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText = "", string postData = "");

         Task<PagePointer> ProcessCommand(APIClient apiClient, SessionData sessionData, Guid contextCode, string commandText, string postData = "");
    }
}
