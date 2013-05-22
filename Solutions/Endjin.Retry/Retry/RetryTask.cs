namespace Endjin.Core.Retry
{
    using System.Diagnostics.CodeAnalysis;

    using Endjin.Core.Async;
    using Endjin.Core.Async.Contracts;

    public static class RetryTask
    {
        private static RetryTaskFactory factory;

        public static ISleepService SleepService { get; set; }

        public static RetryTaskFactory Factory
        {
            get
            {
                if (factory == null)
                {
                    factory = new RetryTaskFactory();
                    RetryTaskFactory.SleepService = SleepService ?? new SleepService();
                }

                return factory;
            }
        }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed. Suppression is OK here.")]
    public static class RetryTask<T>
    {
        private static RetryTaskFactory<T> factory;

        public static ISleepService SleepService { get; set; }

        public static RetryTaskFactory<T> Factory
        {
            get
            {
                if (factory == null)
                {
                    factory = new RetryTaskFactory<T>();
                    RetryTaskFactory.SleepService = SleepService ?? new SleepService();
                }

                return factory;
            }
        }
    }
}