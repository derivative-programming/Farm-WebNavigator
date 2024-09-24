using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.Base.Providers
{
    [Serializable]
    public  class DataFileRecord
    {
        public string Data { get; set; }
        public int PKID { get; set; }
        public string Code { get; set; }
         
    } 
}
