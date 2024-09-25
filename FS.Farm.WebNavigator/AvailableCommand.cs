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
            Description = "";
        }

        public AvailableCommand(
            string commandText = "", 
            string commandDescription = "")
        {
            CommandText = commandText;
            Description = commandDescription;
        }

        [Newtonsoft.Json.JsonProperty("commandText")]
        public string CommandText { get; set; } 

        [Newtonsoft.Json.JsonProperty("description")]
        public string Description { get; set; }
    }
}
