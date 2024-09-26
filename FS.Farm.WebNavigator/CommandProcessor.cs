using FS.Farm.WebNavigator.Page;
using Newtonsoft.Json;
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

            //check if session exists.  if so, get current page.  if not, use default page

            string cacheKey = "WebNavigatorSession-" + sessionCode.ToString();

            bool isSessionAvailable = await FS.Common.Caches.StringCache.ExistsAsync(cacheKey);

            SessionData sessionData = new SessionData();

            if(isSessionAvailable)
            {
                string sessionDataVal = await FS.Common.Caches.StringCache.GetDataAsync(cacheKey); 

                sessionData = JsonConvert.DeserializeObject<SessionData>(sessionDataVal); 
            }
            else
            {  
                sessionData.PageName = "MainMenu";

                sessionData.PageContextCode = Guid.Empty;
            }

            currentPage = sessionData.PageName;

            currentContextCode = sessionData.PageContextCode;

            IPage currentPageProcessor = PageFactory.GetPage(currentPage);

            //if there is a command, make the corresponding request on the current page
            //determine destination page
            PagePointer destinationPagePointer = await currentPageProcessor.ProcessCommand(apiClient, sessionData, currentContextCode, commandText);

            //request destination page
            IPage destinationPageProcessor = PageFactory.GetPage(destinationPagePointer.PageName); 

            //create destination page view 
            result = await destinationPageProcessor.BuildPageView(apiClient, sessionData, destinationPagePointer.ContextCode, commandText);

            //store session data

            sessionData.PageName = destinationPagePointer.PageName;

            sessionData.PageContextCode = destinationPagePointer.ContextCode;


            string sessionDataJson = JsonConvert.SerializeObject(sessionData);

            await FS.Common.Caches.StringCache.SetDataAsync(cacheKey, sessionDataJson);
             
             

            return result;
        }

    }
}
