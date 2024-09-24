
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Collections;
using System.Web.Script.Serialization;
using System.Collections.ObjectModel;


namespace HM.Common.JSON
{
    public class DynamicJsonObject : DynamicObject
    {
        private IDictionary<string, object> Dictionary { get; set; }

        public DynamicJsonObject(IDictionary<string, object> dictionary)
        {
            this.Dictionary = dictionary;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this.Dictionary[binder.Name];

            if (result is IDictionary<string, object>)
            {
                result = new DynamicJsonObject(result as IDictionary<string, object>);
            }
            else if (result is ArrayList && (result as ArrayList) is IDictionary<string, object>)
            {
                result = new List<DynamicJsonObject>((result as ArrayList).ToArray().Select(x => new DynamicJsonObject(x as IDictionary<string, object>)));
            }
            else if (result is ArrayList)
            {
                var list = new List<object>();
                foreach (var o in (result as ArrayList).ToArray())
                {
                    if (o is IDictionary<string, object>)
                    {
                        list.Add(new DynamicJsonObject(o as IDictionary<string, object>));
                    }
                    else
                    {
                        list.Add(o);
                    }
                }
                result = list;
            }

            return this.Dictionary.ContainsKey(binder.Name);
        }
    }
}
