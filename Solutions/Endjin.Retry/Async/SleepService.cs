namespace Endjin.Retry.Async
{
    #region Using Directives

    using System;
    using System.Threading;

    using Endjin.Retry.Contracts;

    #endregion

    public class SleepService : ISleepService
    {
        private readonly ManualResetEvent monitor = new ManualResetEvent(false); 

        public void Sleep(TimeSpan timeSpan)
        {
            this.monitor.WaitOne(timeSpan);
        }
    }
}