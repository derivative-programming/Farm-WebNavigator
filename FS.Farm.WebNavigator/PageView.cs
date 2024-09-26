using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator
{
    public class PageView
    {
        public PageView()
        {
            AppTitle = "Simple Api";
            PageTitleText = "";
            PageIntroText = "";
            PageFooterText = "";
            PageHeaders = new List<PageHeader>();
            TableHeaders = new Dictionary<string, string>();
            TableFilters = new Dictionary<string, string>();
            ValidationErrors = new List<ValidationError>();
            AvailableCommands = new List<AvailableCommand>();
            FormFields = new List<FormField>(); 
        }

        [Newtonsoft.Json.JsonProperty("appTitle", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string AppTitle { get; set; }

        [Newtonsoft.Json.JsonProperty("pageTitleText", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PageTitleText { get; set; }

        [Newtonsoft.Json.JsonProperty("pageIntroText", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PageIntroText { get; set; }

        [Newtonsoft.Json.JsonProperty("pageHeaders", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<PageHeader> PageHeaders { get; set; }

        [Newtonsoft.Json.JsonProperty("tableHeaders", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Dictionary<string, string> TableHeaders { get; set; }

        [Newtonsoft.Json.JsonProperty("tableData", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<Dictionary<string, string>> TableData { get; set; }

        [Newtonsoft.Json.JsonProperty("tableFilters", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public Dictionary<string, string> TableFilters { get; set; }

        [Newtonsoft.Json.JsonProperty("tableInfo", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public TableInfo TableInfo { get; set; }


        [Newtonsoft.Json.JsonProperty("formFields", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<FormField> FormFields { get; set; }

        [Newtonsoft.Json.JsonProperty("pageFooterText", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public string PageFooterText { get; set; }

        [Newtonsoft.Json.JsonProperty("validationErrors", Required = Newtonsoft.Json.Required.AllowNull, NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public List<ValidationError> ValidationErrors { get; set; }

        [Newtonsoft.Json.JsonProperty("availableCommands")]
        public List<AvailableCommand> AvailableCommands { get; set; }
    }
}
