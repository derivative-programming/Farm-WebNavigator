using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using FS.Farm.WebNavigator.Page.Reports.Init;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace FS.Farm.WebNavigator
{
    public class PageTable
    {
        public PageTable()
        { 
            TableHeaders = new Dictionary<string, string>();
            TableFilters = new Dictionary<string, string>();
            TableData = new List<Dictionary<string, string>>();
            tableAvailableFilters = new List<TableAvailableFilter>();
        }

        [Newtonsoft.Json.JsonProperty("tableHeaders", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Dictionary<string, string> TableHeaders { get; set; }

        [Newtonsoft.Json.JsonProperty("tableData", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<Dictionary<string, string>> TableData { get; set; }

        [Newtonsoft.Json.JsonProperty("tableAvailableFilters", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<TableAvailableFilter> tableAvailableFilters { get; set; }

        [Newtonsoft.Json.JsonProperty("tableFilters", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Dictionary<string, string> TableFilters { get; set; }

        [Newtonsoft.Json.JsonProperty("tableInfo", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TableInfo TableInfo { get; set; }

    }
}

