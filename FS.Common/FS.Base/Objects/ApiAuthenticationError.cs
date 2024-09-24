using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Base.Objects
{
    public class ApiAuthenticationError : System.Exception
    {
        public string FieldName = "";


        public Dictionary<string, string> ErrorDict = new Dictionary<string, string>();

        public ApiAuthenticationError(string errorMessage) : base(errorMessage)
        {
            this.ErrorDict.Add(FieldName, errorMessage); 
        } 
    }
}
