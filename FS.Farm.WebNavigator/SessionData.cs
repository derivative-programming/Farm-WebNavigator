using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FS.Farm.WebNavigator.Page.Reports.Init;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace FS.Farm.WebNavigator
{
    public class SessionData
    { 
        public SessionData()
        {
            this.Filters = new Dictionary<string, string>();
            this.SessionCode = Guid.Empty;
            this.OrderByColumnName = string.Empty;
            this.OrderByDescending = false;
            this.PageName = string.Empty;
        }

        [Newtonsoft.Json.JsonProperty("sessionCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Guid SessionCode { get; set; }
        [Newtonsoft.Json.JsonProperty("pageContextCode", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Guid PageContextCode { get; set; }
        [Newtonsoft.Json.JsonProperty("pageName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PageName { get; set; }
        [Newtonsoft.Json.JsonProperty("orderByColumnName", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string OrderByColumnName { get; set; }
        [Newtonsoft.Json.JsonProperty("orderByDescending", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public bool OrderByDescending { get; set; }

        [Newtonsoft.Json.JsonProperty("filters", Required = Newtonsoft.Json.Required.DisallowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Dictionary<string,string> Filters { get; set; }

          
    }
}

