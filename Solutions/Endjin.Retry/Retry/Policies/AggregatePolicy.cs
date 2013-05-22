namespace Endjin.Core.Retry.Policies
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AggregatePolicy : IRetryPolicy
    {
        private List<IRetryPolicy> policies;

        public List<IRetryPolicy> Policies
        {
            get { return this.policies ?? (this.policies = new List<IRetryPolicy>()); }
        }

        public bool CanRetry(Exception exception)
        {
            return this.policies.All(p => p.CanRetry(exception));
        }
    }
}