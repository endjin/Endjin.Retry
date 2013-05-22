namespace Endjin.Core.Retry.Strategies
{
    using System;

    public class Backoff : RetryStrategy
    {
        private readonly TimeSpan deltaBackoff;
        private readonly int maxTries;

        private int tryCount;

        public Backoff() : this(5,TimeSpan.FromSeconds(2))
        {
        }

        public Backoff(int maxTries, TimeSpan deltaBackoff)
        {
            this.maxTries = maxTries;
            this.deltaBackoff = deltaBackoff;
            this.MinBackoff = this.DefaultMinBackoff;
            this.MaxBackoff = this.DefaultMaxBackoff;
        }

        public override bool CanRetry
        {
            get
            {
                return this.tryCount < this.maxTries;
            }
        }

        public TimeSpan DefaultMinBackoff
        {
            get { return TimeSpan.FromSeconds(1); }
        }
        
        public TimeSpan DefaultMaxBackoff
        {
            get { return TimeSpan.FromSeconds(30); }
        }

        public TimeSpan DeltaBackoff
        {
            get { return this.deltaBackoff; }
        }

        public TimeSpan MinBackoff { get; set; }
        
        public TimeSpan MaxBackoff { get; set; }

        public override TimeSpan PrepareToRetry(Exception lastException)
        {
            this.AddException(lastException);

            this.tryCount += 1;

            if (this.CanRetry)
            {
                var rand = new Random();
                var increment = (int)((Math.Pow(2, this.tryCount) - 1) * rand.Next((int)(this.deltaBackoff.TotalMilliseconds * 0.8), (int)(this.deltaBackoff.TotalMilliseconds * 1.2)));
                var delay = (int)Math.Min(this.MinBackoff.TotalMilliseconds + increment, this.MaxBackoff.TotalMilliseconds);

                return TimeSpan.FromMilliseconds(delay);
            }

            return TimeSpan.Zero;
        }
    }
}