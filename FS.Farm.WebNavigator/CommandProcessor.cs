using FS.Farm.WebNavigator.Page;
using System;
using System.CodeDom.Compiler;

namespace FS.Farm.WebNavigator
{
    public class CommandProcessor
    {
        public async Task<PageView> ProcessCommand(string apiKey, Guid sessionCode, PagePostModel requestModel)
        {
            PageView result = new PageView();

            string commandText = string.Empty;

            string postJsonData = string.Empty;

            string currentPage = "";

            Guid currentContextCode = Guid.Empty;

            string destinationPage = "";

            string baseUrl = FS.Common.Configuration.ApplicationSetting.ReadApplicationSetting("apiBaseUrl", "");

            APIClient apiClient = new APIClient(apiKey, baseUrl);

            if (requestModel == null)
            {
                requestModel = new PagePostModel();
            }

            //TODO throw error if no session code set

            commandText = requestModel.CommandText;

            postJsonData = requestModel.FormData;

            //check if session exists.  if so, get current page.  if not, use default page

            string cacheKey = "WebNavigatorSession-" + sessionCode.ToString();

            bool isSessionAvailable = await FS.Common.Caches.StringCache.ExistsAsync(cacheKey);

            if(isSessionAvailable)
            {
                string sessionData = await FS.Common.Caches.StringCache.GetDataAsync(cacheKey);

                string[] sessionDataParts = sessionData.Split('|');

                currentPage = sessionDataParts[0];

                currentContextCode = new Guid(sessionDataParts[1]);
            }
            else
            {
                currentPage = "MainMenu";

                currentContextCode = Guid.Empty;
            }

            IPage currentPageProcessor = PageFactory.GetPage(currentPage);

            //if there is a command, make the corresponding request on the current page
            //determine destination page
            PagePointer destinationPagePointer = await currentPageProcessor.ProcessCommand(apiClient, sessionCode, currentContextCode, commandText, postJsonData);

            //request destination page
            IPage destinationPageProcessor = PageFactory.GetPage(destinationPagePointer.PageName);

            //create destination page view 
            result = await destinationPageProcessor.BuildPageView(apiClient, sessionCode, destinationPagePointer.ContextCode, commandText, postJsonData);

            //store session data
            await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, destinationPagePointer.PageName + "|" + destinationPagePointer.ContextCode.ToString());
             

            if (result.PageTitleText != null &&
                result.PageTitleText == "")
                result.PageTitleText = null; 

            if (result.PageIntroText != null &&
                result.PageIntroText == "")
                result.PageIntroText = null;

            if (result.PageHeaders != null &&
                result.PageHeaders.Count == 0)
                result.PageHeaders = null;

            if (result.TableHeaders != null &&
                result.TableHeaders.Count == 0)
                result.TableHeaders = null;

            if (result.ValidationErrors != null &&
                result.ValidationErrors.Count == 0)
                result.ValidationErrors = null;

            if (result.PageFooterText != null &&
                result.PageFooterText == "")
                result.PageFooterText = null;

            return result;
        }

    }
}
