namespace Endjin.Retry.Samples
{
    #region Using Directives

    using System;
    using System.Threading.Tasks;

    using Endjin.Core.Retry;
    using Endjin.Core.Retry.Strategies;

    #endregion

    public class Program
    {
        public static void Main(string[] args)
        {
            RunAsync().Wait();
            RunInlineAsync().Wait();
            RunWithNewTask().Wait();
            Run();
       
            Console.ReadKey();
        }

        private static void Run()
        {
            // Here we just retry some non-async service call
            ISomeService someTasks = new MyService();
            
            var result = Retriable.Retry(() => someTasks.SecondTask(someTasks.FirstTask()));
            
            Console.WriteLine(result);
        }

        private static async Task RunWithNewTask()
        {
             /*
             This example starts a new Task to execute the synchronous versions of the
             service calls in an asynchronous manner, retrying the operation until it is done.
             Notice how we're API-compatible with Task<T>.Factory.StartNew()
            
             We provide two additional parameters, an IRetryPolicy and an IRetryStrategy.

             Strategies
             Here we are using the Count strategy - which will retry immediately a number of times and then
             give up. We also provide Incremental, which retries a number of times, with an (optionally increasing) delay between
             retries; and Backoff which is similar, but provides an exponentially increasing  delay between retries, with a random element.
             Note - you don't want to use the Incremental or BackOff strategies in Azure. It is better to hammer the fabric and let it adapt
             to your preferred usage pattern.

             Policies
             Policies are used to determine whether we can retry given that a particular exception has occurred
             The default policy is AnyException - you can always retry regardless of the particular exception or its content
             We also provide an AggregatePolicy which allows you to retry if and only if all of a set of policies allow you to retry
             It is up to you to write custom policy if you want particular exceptions to be "non-retryable"
             For example, you might set up a policy that does not allow you to retry if you get a 404 (not found) from an http operation, but
             does retry if you get a 501 (internal server error)
             */

            ISomeService someTasks = new MyService();
            
            var result = await RetryTask<string>.Factory.StartNew(
                () => 
                someTasks.SecondTask(someTasks.FirstTask()), 
                new Count(10));

            Console.WriteLine(result);
        }

        private static async Task RunAsync()
        {
            ISomeService someTasks = new MyService();

            var result = await Retriable.RetryAsync(() => SomeFuncAsync(someTasks));
            
            Console.WriteLine(result);
        }

        private static async Task<string> SomeFuncAsync(ISomeService someTasks)
        {
            var response = await someTasks.FirstTaskAsync();
            
            return await someTasks.SecondTaskAsync(response);
        }

        private static async Task RunInlineAsync()
        {
            ISomeService someTasks = new MyService();

            var result = await Retriable.RetryAsync(async delegate
            {
                var response = await someTasks.FirstTaskAsync();
            
                return await someTasks.SecondTaskAsync(response);                
            });
            
            Console.WriteLine(result);
        }
    }
}
