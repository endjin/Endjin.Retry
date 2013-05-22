namespace Endjin.Core.Retry.Strategies
{
    using System;

    public class Incremental : RetryStrategy
    {
        private readonly int maxTries;
        private readonly TimeSpan step;
        private readonly TimeSpan initialDelay;
        private int tryCount;

        public Incremental()
            : this(5, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2))
        {
        }

        public Incremental(int maxTries, TimeSpan intialDelay, TimeSpan step)
        {
            this.maxTries = maxTries;
            this.initialDelay = intialDelay;
            this.step = step;
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

            if (this.CanRetry)
            {
                var delay = ((this.tryCount - 1) * this.step.TotalMilliseconds) + this.initialDelay.TotalMilliseconds;
                return TimeSpan.FromMilliseconds(delay);
            }

            return TimeSpan.Zero;
        }
    }
}
