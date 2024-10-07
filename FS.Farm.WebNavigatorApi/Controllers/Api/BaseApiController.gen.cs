using System;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http; 
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json; 
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http; 

namespace FS.Farm.FSFarmAPI.Controllers.API
{
    public  class BaseApiController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        protected string GetAuthenticationToken(IHeaderDictionary headers)
        {
            string result = string.Empty;
            if (Request.Headers.ContainsKey("Api-Key"))
            {
                result = Request.Headers["Api-Key"];
            }
            return result;
        }
          
    }
}
