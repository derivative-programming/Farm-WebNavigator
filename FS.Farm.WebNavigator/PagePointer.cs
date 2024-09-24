using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator
{
    public class PagePointer
    {
        public PagePointer() { }

        public PagePointer(string pageName, Guid contextCode) { 
            this.PageName = pageName;
            this.ContextCode = contextCode;
        }

        [Newtonsoft.Json.JsonProperty("pageName")]
        public string PageName { get; set; }

        [Newtonsoft.Json.JsonProperty("contextCode")]
        public Guid ContextCode { get; set; } 
    }
}
