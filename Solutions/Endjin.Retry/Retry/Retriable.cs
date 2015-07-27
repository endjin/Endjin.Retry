namespace Endjin.Core.Retry
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Endjin.Core.Retry.Policies;
    using Endjin.Core.Retry.Strategies;
    using Endjin.Retry.Async;
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
            return RetryAsync(asyncFunc, true);
        }

        public static Task<T> RetryAsync<T>(Func<Task<T>> asyncFunc, bool continueOnCapturedContext)
        {
            return RetryAsync(asyncFunc, CancellationToken.None, new Count(10), new AnyException(), continueOnCapturedContext);
        }

        public static void Retry(Action func)
        {
            Retry(func, CancellationToken.None, new Count(10), new AnyException());
        }

        public static Task RetryAsync(Func<Task> asyncFunc)
        {
            return RetryAsync(asyncFunc, true);
        }

        public static Task RetryAsync(Func<Task> asyncFunc, bool continueOnCapturedContext)
        {
            return RetryAsync(asyncFunc, CancellationToken.None, new Count(10), new AnyException(), continueOnCapturedContext);
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
        
        public static Task<T> RetryAsync<T>(Func<Task<T>> asyncFunc, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return RetryAsync(asyncFunc, cancellationToken, strategy, policy, true);
        }

        public static async Task<T> RetryAsync<T>(Func<Task<T>> asyncFunc, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy, bool continueOnCapturedContext)
        {
            do
            {
                Exception exception;

                try
                {
                    return await asyncFunc().ConfigureAwait(continueOnCapturedContext);
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

        public static void Retry(Action func, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy)
        {
            do
            {
                try
                {
                    func();
                    return;
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
        
        public static Task RetryAsync(Func<Task> asyncFunc, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return RetryAsync(asyncFunc, cancellationToken, strategy, policy, true);
        }

        public static async Task RetryAsync(Func<Task> asyncFunc, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy, bool continueOnCapturedContext)
        {
            do
            {
                Exception exception;

                try
                {
                    await asyncFunc().ConfigureAwait(continueOnCapturedContext);
                    return;
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