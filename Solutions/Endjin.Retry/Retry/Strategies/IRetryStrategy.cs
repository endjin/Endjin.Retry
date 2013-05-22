namespace Endjin.Core.Retry.Strategies
{
    using System;

    public interface IRetryStrategy
    {
        AggregateException Exception { get; }

        bool CanRetry { get; }

        /// <summary>
        /// Called when a Task has failed, and is about to
        /// be retried
        /// </summary>
        /// <param name="lastException">The last exception seen</param>
        /// <returns>The time to delay before retrying</returns>
        TimeSpan PrepareToRetry(Exception lastException);

        void OnRetrying(RetryEventArgs eventArgs);

        event EventHandler<RetryEventArgs> Retrying;
    }
}
