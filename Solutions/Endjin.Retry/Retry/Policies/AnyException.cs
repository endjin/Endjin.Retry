namespace Endjin.Core.Retry.Policies
{
    using System;

    public class AnyException : IRetryPolicy
    {
        public bool CanRetry(Exception exception)
        {
            return true;
        }
    }
}