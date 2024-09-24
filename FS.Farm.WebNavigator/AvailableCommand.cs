using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Farm.WebNavigator
{
    public class AvailableCommand
    {

        public AvailableCommand()
        {
            CommandText = "";
            CommandTitle = "";
            CommandDescription = "";
        }

        public AvailableCommand(
            string commandText = "",
            string commandTitle = "",
            string commandDescription = "")
        {
            CommandText = commandText;
            CommandTitle = commandTitle;
            CommandDescription = commandDescription;
        }

        [Newtonsoft.Json.JsonProperty("commandText")]
        public string CommandText { get; set; }

        [Newtonsoft.Json.JsonProperty("commandTitle")]
        public string CommandTitle { get; set; }

        [Newtonsoft.Json.JsonProperty("commandDescription")]
        public string CommandDescription { get; set; }
    }
}
