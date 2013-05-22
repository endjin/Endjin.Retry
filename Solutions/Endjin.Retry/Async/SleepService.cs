namespace Endjin.Core.Async
{
    #region Using statements

    using System;
    using System.Threading;

    using Endjin.Core.Async.Contracts;

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