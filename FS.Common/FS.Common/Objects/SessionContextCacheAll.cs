using System;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FS.Common.Authentication; 

namespace FS.Common.Objects
{ 
    public class SessionContextCacheAll: SessionContext
    {
         

        public SessionContextCacheAll(bool useTransactions):base(useTransactions)
        { 
             this.CacheAllForced = true;
        }

        public SessionContextCacheAll(bool useTransactions, AuthenticationToken authenticationToken)
            : base(useTransactions, authenticationToken)
        {
            this.CacheAllForced = true;
        }
         
    }
}