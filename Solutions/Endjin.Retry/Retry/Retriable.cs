using Endjin.Retry.Async;

namespace Endjin.Core.Retry
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Endjin.Core.Retry.Policies;
    using Endjin.Core.Retry.Strategies;
    using Endjin.Retry.Contracts;

    #endregion

    public static class Retriable
    {
        private static ISleepService sleepService;

        public static ISleepService SleepService
        {
            get { return sleepService ?? (sleepService = new SleepService()); }
            set { sleepService = value; }
        }

        public static T Retry<T>(Func<T> func)
        {
            return Retry(func, CancellationToken.None, new Count(10), new AnyException());            
        }

        public static Task<T> RetryAsync<T>(Func<Task<T>> asyncFunc)
        {
            return RetryAsync(asyncFunc, CancellationToken.None, new Count(10), new AnyException());
        }

        public static T Retry<T>(Func<T> func, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy)
        {
            do
            {
                try
                {
                    return func();
                }
                catch (Exception exception)
                {
                    var delay = strategy.PrepareToRetry(exception);

                    if (!WillRetry(exception, cancellationToken, strategy, policy))
                    {
                        throw;
                    }

                    strategy.OnRetrying(new RetryEventArgs(exception, delay));

                    if (delay != TimeSpan.Zero)
                    {
                        if (SleepService != null)
                        {
                            SleepService.Sleep(delay);
                        }
                    }
                }
            }
            while (true);
        }

        public static async Task<T> RetryAsync<T>(Func<Task<T>> asyncFunc, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy)
        {
            do
            {
                Exception exception;

                try
                {
                    return await asyncFunc();
                }
                catch (Exception ex)
                {
                    exception = ex;
                }

                var delay = strategy.PrepareToRetry(exception);

                if (!WillRetry(exception, cancellationToken, strategy, policy))
                {
                    throw exception;
                }

                strategy.OnRetrying(new RetryEventArgs(exception, delay));

                if (delay != TimeSpan.Zero)
                {
                    if (SleepService != null)
                    {
                        SleepService.Sleep(delay);
                    }
                }
            }
            while (true);
        }

        private static bool WillRetry(Exception exception, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return strategy.CanRetry && !cancellationToken.IsCancellationRequested && policy.CanRetry(exception);
        }
    }
}