namespace Endjin.Core.Retry.Strategies
{
    using System;

    public class Linear : RetryStrategy
    {
        private readonly TimeSpan periodicity;
        private readonly int maxTries;
        private int tryCount; 

        public Linear(TimeSpan periodicity, int maxTries)
        {
            this.periodicity = periodicity;
            this.maxTries = maxTries;
        }

        public override bool CanRetry
        {
            get
            {
                return this.tryCount < this.maxTries;
            }
        }

        public override TimeSpan PrepareToRetry(Exception lastException)
        {
            this.AddException(lastException);

            this.tryCount += 1;

            return this.periodicity;
        }
    }
}