namespace Endjin.Core.Retry
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Endjin.Core.Async.Contracts;
    using Endjin.Core.Retry.Policies;
    using Endjin.Core.Retry.Strategies;

    public class RetryTaskFactory<T>
    {
        public Task<T> StartNew(Func<T> function)
        {
            return this.StartNew(function, new Count(), new AnyException());
        }

        public Task<T> StartNew(Func<T> function, CancellationToken cancellationToken)
        {
            return this.StartNew(function, cancellationToken, new Count(), new AnyException());
        }

        public Task<T> StartNew(Func<T> function, TaskCreationOptions taskCreationOptions)
        {
            return this.StartNew(function, taskCreationOptions, new Count(), new AnyException());
        }

        public Task<T> StartNew(Func<object, T> function, object state)
        {
            return this.StartNew(function, state, new Count(), new AnyException());
        }

        public Task<T> StartNew(Func<object, T> function, object state, TaskCreationOptions taskCreationOptions)
        {
            return this.StartNew(function, state, taskCreationOptions, new Count(), new AnyException());
        }

        public Task<T> StartNew(Func<object, T> function, object state, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler)
        {
            return this.StartNew(function, state, cancellationToken, taskCreationOptions, scheduler, new Count(), new AnyException());
        }

        public Task<T> StartNew(Func<T> function, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler)
        {
            return this.StartNew(function, cancellationToken, taskCreationOptions, scheduler, new Count(), new AnyException());
        }

        public Task<T> StartNew(Func<T> function, IRetryStrategy strategy)
        {
            return this.StartNew(function, strategy, new AnyException());
        }

        public Task<T> StartNew(Func<T> function, CancellationToken cancellationToken, IRetryStrategy strategy)
        {
            return this.StartNew(function, cancellationToken, strategy, new AnyException());
        }

        public Task<T> StartNew(Func<T> function, TaskCreationOptions taskCreationOptions, IRetryStrategy strategy)
        {
            return this.StartNew(function, taskCreationOptions, strategy, new AnyException());
        }

        public Task<T> StartNew(Func<object, T> function, object state, IRetryStrategy strategy)
        {
            return this.StartNew(function, state, strategy, new AnyException());
        }

        public Task<T> StartNew(Func<object, T> function, object state, TaskCreationOptions taskCreationOptions, IRetryStrategy strategy)
        {
            return this.StartNew(function, state, taskCreationOptions, strategy, new AnyException());
        }

        public Task<T> StartNew(Func<object, T> function, object state, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryStrategy strategy)
        {
            return this.StartNew(function, state, cancellationToken, taskCreationOptions, scheduler, strategy, new AnyException());
        }

        public Task<T> StartNew(Func<T> function, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryStrategy strategy)
        {
            return this.StartNew(function, cancellationToken, taskCreationOptions, scheduler, strategy, new AnyException());
        }

        public Task<T> StartNew(Func<T> function, IRetryPolicy policy)
        {
            return this.StartNew(function, new Count(), policy);
        }

        public Task<T> StartNew(Func<T> function, CancellationToken cancellationToken, IRetryPolicy policy)
        {
            return this.StartNew(function, cancellationToken, new Count(), policy);
        }

        public Task<T> StartNew(Func<T> function, TaskCreationOptions taskCreationOptions, IRetryPolicy policy)
        {
            return this.StartNew(function, taskCreationOptions, new Count(), policy);
        }

        public Task<T> StartNew(Func<object, T> function, object state, IRetryPolicy policy)
        {
            return this.StartNew(function, state, new Count(), policy);
        }

        public Task<T> StartNew(Func<object, T> function, object state, TaskCreationOptions taskCreationOptions, IRetryPolicy policy)
        {
            return this.StartNew(function, state, taskCreationOptions, new Count(), policy);
        }

        public Task<T> StartNew(Func<object, T> function, object state, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryPolicy policy)
        {
            return this.StartNew(function, state, cancellationToken, taskCreationOptions, scheduler, new Count(), policy);
        }

        public Task<T> StartNew(Func<T> function, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryPolicy policy)
        {
            return this.StartNew(function, cancellationToken, taskCreationOptions, scheduler, new Count(), policy);
        }

        public Task<T> StartNew(Func<T> function, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task<T>.Factory.StartNew(function).ContinueWith(t => HandleTask(t, () => new Task<T>(function), strategy, policy));
        }

        public Task<T> StartNew(Func<T> function, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task<T>.Factory.StartNew(function, cancellationToken).ContinueWith(t => HandleTask(t, () => new Task<T>(function, cancellationToken), strategy, policy));
        }

        public Task<T> StartNew(Func<T> function, TaskCreationOptions taskCreationOptions, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task<T>.Factory.StartNew(function, taskCreationOptions).ContinueWith(t => HandleTask(t, () => new Task<T>(function, taskCreationOptions), strategy, policy));
        }

        public Task<T> StartNew(Func<object, T> function, object state, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task<T>.Factory.StartNew(function, state).ContinueWith(t => HandleTask(t, () => new Task<T>(function, state), strategy, policy));
        }

        public Task<T> StartNew(Func<object, T> function, object state, TaskCreationOptions taskCreationOptions, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task<T>.Factory.StartNew(function, state, taskCreationOptions).ContinueWith(t => HandleTask(t, () => new Task<T>(function, state, taskCreationOptions), strategy, policy));
        }

        public Task<T> StartNew(Func<object, T> function, object state, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task<T>.Factory.StartNew(function, state, cancellationToken).ContinueWith(t => HandleTask(t, () => new Task<T>(function, state, cancellationToken), strategy, policy));
        }

        public Task<T> StartNew(Func<object, T> function, object state, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task<T>.Factory.StartNew(function, state, cancellationToken, taskCreationOptions, scheduler).ContinueWith(t => HandleTask(t, () => new Task<T>(function, state, cancellationToken, taskCreationOptions), strategy, policy));
        }

        public Task<T> StartNew(Func<T> function, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task<T>.Factory.StartNew(function, cancellationToken, taskCreationOptions, scheduler).ContinueWith(t => HandleTask(t, () => new Task<T>(function, cancellationToken, taskCreationOptions), strategy, policy));
        }

        private static T HandleTask(Task<T> task, Func<Task<T>> createTask, IRetryStrategy strategy, IRetryPolicy policy)
        {
            task = RetryTaskFactory.HandleRetry(task, createTask, strategy, policy) as Task<T>;

            Debug.Assert(task != null, "task != null");

            RetryTaskFactory.HandleException(task, strategy);

            return task.Result;
        }
    }

    public class RetryTaskFactory
    {
        public static ISleepService SleepService { get; set; }

        public Task StartNew(Action action)
        {
            return this.StartNew(action, new Count(), new AnyException());
        }

        public Task StartNew(Action action, CancellationToken cancellationToken)
        {
            return this.StartNew(action, cancellationToken, new Count(), new AnyException());
        }

        public Task StartNew(Action action, TaskCreationOptions taskCreationOptions)
        {
            return this.StartNew(action, taskCreationOptions, new Count(), new AnyException());
        }

        public Task StartNew(Action<object> action, object state)
        {
            return this.StartNew(action, state, new Count(), new AnyException());
        }

        public Task StartNew(Action<object> action, object state, TaskCreationOptions taskCreationOptions)
        {
            return this.StartNew(action, state, taskCreationOptions, new Count(), new AnyException());
        }

        public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler)
        {
            return this.StartNew(action, state, cancellationToken, taskCreationOptions, scheduler, new Count(), new AnyException());
        }

        public Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler)
        {
            return this.StartNew(action, cancellationToken, taskCreationOptions, scheduler, new Count(), new AnyException());
        }

        public Task StartNew(Action action, IRetryPolicy policy)
        {
            return this.StartNew(action, new Count(), policy);
        }

        public Task StartNew(Action action, CancellationToken cancellationToken, IRetryPolicy policy)
        {
            return this.StartNew(action, cancellationToken, new Count(), policy);
        }

        public Task StartNew(Action action, TaskCreationOptions taskCreationOptions, IRetryPolicy policy)
        {
            return this.StartNew(action, taskCreationOptions, new Count(), policy);
        }

        public Task StartNew(Action<object> action, object state, IRetryPolicy policy)
        {
            return this.StartNew(action, state, new Count(), policy);
        }

        public Task StartNew(Action<object> action, object state, TaskCreationOptions taskCreationOptions, IRetryPolicy policy)
        {
            return this.StartNew(action, state, taskCreationOptions, new Count(), policy);
        }

        public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryPolicy policy)
        {
            return this.StartNew(action, state, cancellationToken, taskCreationOptions, scheduler, new Count(), policy);
        }

        public Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryPolicy policy)
        {
            return this.StartNew(action, cancellationToken, taskCreationOptions, scheduler, new Count(), policy);
        }

        public Task StartNew(Action action, IRetryStrategy strategy)
        {
            return this.StartNew(action, strategy, new AnyException());
        }

        public Task StartNew(Action action, CancellationToken cancellationToken, IRetryStrategy strategy)
        {
            return this.StartNew(action, cancellationToken, strategy, new AnyException());
        }

        public Task StartNew(Action action, TaskCreationOptions taskCreationOptions, IRetryStrategy strategy)
        {
            return this.StartNew(action, taskCreationOptions, strategy, new AnyException());
        }

        public Task StartNew(Action<object> action, object state, IRetryStrategy strategy)
        {
            return this.StartNew(action, state, strategy, new AnyException());
        }

        public Task StartNew(Action<object> action, object state, TaskCreationOptions taskCreationOptions, IRetryStrategy strategy)
        {
            return this.StartNew(action, state, taskCreationOptions, strategy, new AnyException());
        }

        public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryStrategy strategy)
        {
            return this.StartNew(action, state, cancellationToken, taskCreationOptions, scheduler, strategy, new AnyException());
        }

        public Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryStrategy strategy)
        {
            return this.StartNew(action, cancellationToken, taskCreationOptions, scheduler, strategy, new AnyException());
        }

        public Task StartNew(Action action, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task.Factory.StartNew(action).ContinueWith(t => HandleTask(t, () => new Task(action), strategy, policy));
        }

        public Task StartNew(Action action, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task.Factory.StartNew(action, cancellationToken).ContinueWith(t => HandleTask(t, () => new Task(action, cancellationToken), strategy, policy));
        }

        public Task StartNew(Action action, TaskCreationOptions taskCreationOptions, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task.Factory.StartNew(action, taskCreationOptions).ContinueWith(t => HandleTask(t, () => new Task(action, taskCreationOptions), strategy, policy));
        }

        public Task StartNew(Action<object> action, object state, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task.Factory.StartNew(action, state).ContinueWith(t => HandleTask(t, () => new Task(action, state), strategy, policy));
        }

        public Task StartNew(Action<object> action, object state, TaskCreationOptions taskCreationOptions, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task.Factory.StartNew(action, state, taskCreationOptions).ContinueWith(t => HandleTask(t, () => new Task(action, state, taskCreationOptions), strategy, policy));
        }

        public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task.Factory.StartNew(action, state, cancellationToken).ContinueWith(t => HandleTask(t, () => new Task(action, state, cancellationToken), strategy, policy));
        }

        public Task StartNew(Action<object> action, object state, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task.Factory.StartNew(action, state, cancellationToken, taskCreationOptions, scheduler).ContinueWith(t => HandleTask(t, () => new Task(action, state, cancellationToken, taskCreationOptions), strategy, policy));
        }

        public Task StartNew(Action action, CancellationToken cancellationToken, TaskCreationOptions taskCreationOptions, TaskScheduler scheduler, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return Task.Factory.StartNew(action, cancellationToken, taskCreationOptions, scheduler).ContinueWith(t => HandleTask(t, () => new Task(action, cancellationToken, taskCreationOptions), strategy, policy));
        }

        private static void HandleTask(Task task, Func<Task> createTask, IRetryStrategy strategy, IRetryPolicy policy)
        {
            task = HandleRetry(task, createTask, strategy, policy);

            HandleException(task, strategy);
        }

        internal static void HandleException(Task task, IRetryStrategy strategy)
        {
            var exception = strategy.Exception;
            if (exception != null && task.Exception != null)
            {
                throw exception;
            }
        }

        internal static Task HandleRetry(Task task, Func<Task> createTask, IRetryStrategy strategy, IRetryPolicy policy)
        {
            while (task.Exception != null)
            {
                var delay = strategy.PrepareToRetry(task.Exception);

                if (!WillRetry(task, strategy, policy))
                {
                    break;
                }
                
                strategy.OnRetrying(new RetryEventArgs(task.Exception, delay));

                if (delay != TimeSpan.Zero)
                {
                    if (SleepService != null)
                    {
                        SleepService.Sleep(delay);
                    }
                }

                task = createTask();
                task.RunSynchronously();
            }

            return task;
        }

        private static bool WillRetry(Task task, IRetryStrategy strategy, IRetryPolicy policy)
        {
            return strategy.CanRetry && !task.IsCanceled && task.Exception.Flatten().InnerExceptions.All(policy.CanRetry);
        }
    }
}