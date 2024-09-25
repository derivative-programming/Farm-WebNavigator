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

            AvailableCommands = new List<AvailableCommand>();
        }

        [Newtonsoft.Json.JsonProperty("appTitle")]
        public string AppTitle { get; set; }

        [Newtonsoft.Json.JsonProperty("pageTitleText")]
        public string PageTitleText { get; set; }

        [Newtonsoft.Json.JsonProperty("pageIntroText")]
        public string PageIntroText { get; set; }

        [Newtonsoft.Json.JsonProperty("pageFooterText")]
        public string PageFooterText { get; set; }

        [Newtonsoft.Json.JsonProperty("pageData")]
        public string PageData { get; set; }

        [Newtonsoft.Json.JsonProperty("availableCommands")]
        public List<AvailableCommand> AvailableCommands { get; set; }
    }
}
