namespace Endjin.Core.Retry.Strategies
{
    using System;
    using System.Collections.Generic;

    public abstract class RetryStrategy : IRetryStrategy
    {
        private readonly List<Exception> exceptions = new List<Exception>();

        public event EventHandler<RetryEventArgs> Retrying;

        public AggregateException Exception
        {
            get
            {
                return new AggregateException(this.exceptions);
            }
        }

        public abstract bool CanRetry
        {
            get;
        }

        public abstract TimeSpan PrepareToRetry(Exception lastException);

        public void OnRetrying(RetryEventArgs e)
        {
            EventHandler<RetryEventArgs> handler = this.Retrying;
            
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected void AddException(Exception exception)
        {
            var aggregateException = exception as AggregateException;
            
            if (aggregateException != null)
            {
                foreach (var ex in aggregateException.InnerExceptions)
                {
                    this.exceptions.Add(ex);
                }
            }
            else
            {
                this.exceptions.Add(exception);                
            }
        }        
    }
}
