namespace Endjin.Core.Retry.Policies
{
    using System;

    public interface IRetryPolicy
    {
        bool CanRetry(Exception exception);
    }
}