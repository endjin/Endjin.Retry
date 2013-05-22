namespace Endjin.Core.Retry
{
    using System;

    public class RetryEventArgs : EventArgs
    {
        public RetryEventArgs(Exception lastException, TimeSpan delayBeforeRetry)
        {
            this.LastException = lastException;
            this.DelayBeforeRetry = delayBeforeRetry;
        }

        public TimeSpan DelayBeforeRetry { get; private set; }

        public Exception LastException { get; private set; }
    }
}