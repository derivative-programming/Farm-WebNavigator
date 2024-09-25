using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator
{
    public class PageHeader
    {
        public PageHeader()
        {  
            Title = "";
            Value = "";
        }
        public PageHeader(string title, string value)
        {
            Title = title;
            Value = value;
        } 

        [Newtonsoft.Json.JsonProperty("Title", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Title { get; set; }

        [Newtonsoft.Json.JsonProperty("Value", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string Value { get; set; } 
    }
}
