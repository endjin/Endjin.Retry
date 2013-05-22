namespace Endjin.Core.Async.Contracts
{
    using System;

    public interface ISleepService
    {
        void Sleep(TimeSpan timeSpan);
    }
}