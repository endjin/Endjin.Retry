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

        public Exception LastException { get; private set; }
        public TimeSpan DelayBeforeRetry { get; private set; }
    }
}