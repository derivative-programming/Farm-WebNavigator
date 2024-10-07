using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Net; 
using System.Net.Http; 
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc; 
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using FS.Farm.WebNavigator;

namespace FS.Farm.FSFarmAPI.Controllers.API.V1_0
{
    [Route("api/v1_0/page")]
    public class PageController : FS.Farm.FSFarmAPI.Controllers.API.BaseApiController
    {

        [Route("{sessionCode}")]
        [HttpPost]
        public async Task<PageView> Post(Guid? sessionCode, [FromBody] PagePostModel model)
        {
            string authenticationToken = string.Empty;

            if (!sessionCode.HasValue)
            {
                sessionCode = Guid.NewGuid();
            }

            authenticationToken = GetAuthenticationToken(Request.Headers);

            WebNavigator.CommandProcessor commandProcessor = new WebNavigator.CommandProcessor();

            var resultPageView = await commandProcessor.ProcessCommand(authenticationToken, sessionCode.Value, model);

            return resultPageView; 
        } 
    }

}

