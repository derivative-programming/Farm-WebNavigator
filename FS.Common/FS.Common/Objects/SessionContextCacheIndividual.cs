using System;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FS.Common.Authentication; 

namespace FS.Common.Objects
{ 
    public class SessionContextCacheIndividual: SessionContext
    { 

        public SessionContextCacheIndividual(bool useTransactions):base(useTransactions)
        {
            this.CacheIndividualForced = true;
        }

        public SessionContextCacheIndividual(bool useTransactions, AuthenticationToken authenticationToken)
            : base(useTransactions, authenticationToken)
        {
            this.CacheIndividualForced = true;
        }
         
    }
}