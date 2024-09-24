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
            PageTitleText = "";
            PageIntroText = "";
            PageFooterText = "";
        }

        [Newtonsoft.Json.JsonProperty("pageTitleText")]
        public string PageTitleText { get; set; }

        [Newtonsoft.Json.JsonProperty("pageIntroText")]
        public string PageIntroText { get; set; }

        [Newtonsoft.Json.JsonProperty("pageFooterText")]
        public string PageFooterText { get; set; }

        [Newtonsoft.Json.JsonProperty("availableCommands")]
        public List<AvailableCommand> AvailableCommands { get; set; }
    }
}
