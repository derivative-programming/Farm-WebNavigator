using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FS.Base.Api
{
    public class ObjectPostResponse
    {
        public string success = null; //"success","fail","error" 
        public string message = null;
        public Dictionary<string, string> errors = null;
        public string appVersion = string.Empty;
        
        public void LoadValidationError(string field, string errror)
        {
            this.errors = new Dictionary<string, string>();
            this.errors.Add(field, errror);
        }
        public void LoadValidationError(FS.Base.Objects.ValidationError ex)
        {
            this.errors = new Dictionary<string, string>(ex.ErrorDict); 
            
        }
        public void LoadValidationErrors(List<ValidationResult> validationErrors)
        {
            this.errors = new Dictionary<string,string>();
            for(int i = 0;i < validationErrors.Count;i++)
            {
                List<string> memberNames = validationErrors[i].MemberNames.ToList<string>();
                if(memberNames.Count > 0) {
                    for(int j = 0;j < memberNames.Count;j++)
                    {
                        string memberName = memberNames[j];
                        if (memberName.ToUpper().EndsWith("ID"))
                            memberName = memberName.Substring(0, memberName.Length - 2) + "Code";
                        if (!this.errors.ContainsKey(CamelCasePropNames(memberName)))
                            this.errors.Add(CamelCasePropNames(memberName), validationErrors[i].ErrorMessage);
                    }
                } else {
                    this.errors.Add("customError" + i.ToString(), validationErrors[i].ErrorMessage);
                }
            }
        }

        private static string CamelCasePropNames(string propName)
        {
            var array = propName.Split('.');
            var camelCaseList = new string[array.Length];
            for (var i = 0; i < array.Length; i++)
            {
                var prop = array[i];
                camelCaseList[i] = prop.Substring(0, 1).ToLower() + prop.Substring(1, prop.Length - 1);
            }
            return string.Join(".", camelCaseList);
        }
    }
}
