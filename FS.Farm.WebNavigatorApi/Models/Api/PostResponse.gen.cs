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
//using FS.Farm.Business;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
//using FS.Base.Api;
using FS.Common.Objects;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using FS.Common.Diagnostics.Loggers;

namespace FS.Farm.FSFarmAPI.Models.API
{

    public class PostResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public List<ValidationError> validationErrors { get; set; }
    }

}
