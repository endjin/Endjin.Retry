using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Endjin.Core.Repeat.Strategies;

namespace Endjin.Core.Repeat
{
    public static class Repeatable
    {
        public static void Repeat(CancellationToken cancellationToken, TimeSpan periodicity, Action<CancellationToken> action)
        {
            Repeat(cancellationToken, new LinearPeriodicityStrategy(periodicity), action);
        }

        public static Task RepeatAsync(CancellationToken cancellationToken, TimeSpan periodicity, Func<CancellationToken, Task> action)
        {
            return RepeatAsync(cancellationToken, new LinearPeriodicityStrategy(periodicity), action);
        }

        public static void Repeat(CancellationToken cancellationToken, IPeriodicityStrategy periodicity, Action<CancellationToken> action)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                action(cancellationToken);
                cancellationToken.WaitHandle.WaitOne(periodicity.GetPeriodicity());
            }
        }

        public static async Task RepeatAsync(CancellationToken cancellationToken, IPeriodicityStrategy periodicity, Func<CancellationToken, Task> action)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await action(cancellationToken);
                cancellationToken.WaitHandle.WaitOne(periodicity.GetPeriodicity());
            }
        }
    }
}
