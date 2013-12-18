namespace Endjin.Core.Repeat
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Endjin.Core.Repeat.Strategies;

    #endregion 

    public static class Repeatable
    {
        public static void Repeat(CancellationToken cancellationToken, TimeSpan periodicity, Action<CancellationToken> action)
        {
            Repeat(cancellationToken, new LinearPeriodicityStrategy(periodicity), TimeSpan.FromSeconds(0), action);
        }

        public static void Repeat(CancellationToken cancellationToken, TimeSpan periodicity, TimeSpan initialDelay, Action<CancellationToken> action)
        {
            Repeat(cancellationToken, new LinearPeriodicityStrategy(periodicity), initialDelay, action);
        }

        public static Task RepeatAsync(CancellationToken cancellationToken, TimeSpan periodicity, Func<CancellationToken, Task> action)
        {
            return RepeatAsync(cancellationToken, new LinearPeriodicityStrategy(periodicity), TimeSpan.FromSeconds(0), action);
        }

        public static Task RepeatAsync(CancellationToken cancellationToken, TimeSpan periodicity, TimeSpan initialDelay, Func<CancellationToken, Task> action)
        {
            return RepeatAsync(cancellationToken, new LinearPeriodicityStrategy(periodicity), initialDelay, action);
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

        private static void Repeat(CancellationToken cancellationToken, IPeriodicityStrategy periodicity, TimeSpan initialDelay, Action<CancellationToken> action)
        {
            cancellationToken.WaitHandle.WaitOne(initialDelay);

            Repeat(cancellationToken, periodicity, action);
        }

        private static Task RepeatAsync(CancellationToken cancellationToken, IPeriodicityStrategy periodicity, TimeSpan initialDelay, Func<CancellationToken, Task> action)
        {
            cancellationToken.WaitHandle.WaitOne(initialDelay);

            return RepeatAsync(cancellationToken, periodicity, action);
        }
    }
}