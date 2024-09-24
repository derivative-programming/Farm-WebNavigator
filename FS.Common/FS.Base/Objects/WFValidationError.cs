using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Base.Objects
{
    public class ValidationError: System.Exception
    {
        public string FieldName = "";


        public Dictionary<string, string> ErrorDict = new Dictionary<string, string>();

        public ValidationError(string errorMessage) : base(errorMessage)
        {
            this.ErrorDict.Add(FieldName, errorMessage); 
        }
        public ValidationError(string fieldName, string errorMessage)
            : base(errorMessage)
        {
            this.FieldName = fieldName;
            this.ErrorDict.Add(fieldName,errorMessage);
        }

        public ValidationError(Dictionary<string, string> errorDict)
            : base(errorDict.Values.First().ToString())
        {
            this.FieldName = errorDict.Keys.First().ToString();
            ErrorDict = new Dictionary<string, string>(errorDict);
        }
    }
}
