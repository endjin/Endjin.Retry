namespace Endjin.Core.Repeat.Strategies
{
    #region Using Directives

    using System;    

    #endregion 

    public class LinearPeriodicityStrategy : IPeriodicityStrategy
    {
        private readonly TimeSpan interval;
        private bool oneTimeRunImmediate;

        public LinearPeriodicityStrategy()
        {
            this.interval = TimeSpan.FromSeconds(30);
        }

        public LinearPeriodicityStrategy(TimeSpan interval)
        {
            this.interval = interval;
        }

        public TimeSpan GetPeriodicity()
        {
            if (this.oneTimeRunImmediate)
            {
                this.oneTimeRunImmediate = false;
                return TimeSpan.Zero;
            }

            return this.interval;
        }

        public void Reset()
        {
        }

        public void EnableOneTimeRunImmediate()
        {
            this.oneTimeRunImmediate = true;
        }
    }
}