namespace Endjin.Core.Retry.Strategies
{
    using System;

    public class Count : RetryStrategy
    {
        private readonly int maxTries;
        private int tryCount;

        public Count() : this(5)
        {
        }

        public Count(int maxTries)
        {
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

            return TimeSpan.Zero;
        }
    }
}