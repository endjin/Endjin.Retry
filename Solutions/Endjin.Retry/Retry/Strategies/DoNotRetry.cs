namespace Endjin.Core.Retry.Strategies
{
    using System;

    public class DoNotRetry : RetryStrategy
    {
        public override bool CanRetry
        {
            get { return false; }
        }

        public override TimeSpan PrepareToRetry(Exception lastException)
        {
            return TimeSpan.Zero;
        }
    }
}