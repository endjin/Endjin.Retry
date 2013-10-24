using System;

namespace Endjin.Core.Repeat.Strategies
{
    public interface IPeriodicityStrategy
    {
        TimeSpan GetPeriodicity();

        void Reset();

        void EnableOneTimeRunImmediate();
    }
}