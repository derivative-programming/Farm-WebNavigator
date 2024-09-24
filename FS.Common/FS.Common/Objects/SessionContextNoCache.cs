using System;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FS.Common.Authentication; 

namespace FS.Common.Objects
{ 
    public class SessionContextNoCache: SessionContext
    {
         

        public SessionContextNoCache(bool useTransactions):base(useTransactions)
        { 
             this.CacheNoneForced = true;
        }

        public SessionContextNoCache(bool useTransactions, AuthenticationToken authenticationToken):base(useTransactions,authenticationToken)
        {
            this.CacheNoneForced = true;
        }
         
    }
}