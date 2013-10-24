namespace Endjin.Core.Repeat.Strategies
{
    using System;

    public class BackOffPeriodicityStrategy : IPeriodicityStrategy
    {
        private readonly TimeSpan deltaBackoff;

        private int tryCount;
        private bool oneTimeRunImmediate;

        public BackOffPeriodicityStrategy() : this(TimeSpan.FromSeconds(2))
        {
        }

        public BackOffPeriodicityStrategy(TimeSpan deltaBackoff)
        {
            this.deltaBackoff = deltaBackoff;
            this.MinBackoff = this.DefaultMinBackoff;
            this.MaxBackoff = this.DefaultMaxBackoff;
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

        public TimeSpan GetPeriodicity()
        {
            if (oneTimeRunImmediate)
            {
                oneTimeRunImmediate = false;
                return TimeSpan.Zero;
            }

            this.tryCount += 1;

            var rand = new Random();
            var increment = (int)((Math.Pow(2, this.tryCount) - 1) * rand.Next((int)(this.deltaBackoff.TotalMilliseconds * 0.8), (int)(this.deltaBackoff.TotalMilliseconds * 1.2)));
            var delay = (int)Math.Min(this.MinBackoff.TotalMilliseconds + increment, this.MaxBackoff.TotalMilliseconds);

            return TimeSpan.FromMilliseconds(delay);
        }

        public void Reset()
        {
            this.tryCount = 0;
        }

        public void EnableOneTimeRunImmediate()
        {
            this.oneTimeRunImmediate = true;
        }
    }
}