using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator
{
    public class PagePostModel
    {
        public PagePostModel()
        {
            CommandText = string.Empty;
            FormData = string.Empty;
        }

        [Newtonsoft.Json.JsonProperty("commandText")]
        public string CommandText { get; set; }

        [Newtonsoft.Json.JsonProperty("formData")]
        public string FormData { get; set; }
    }
}
