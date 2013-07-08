namespace Endjin.Retry.Contracts
{
    using System;

    public interface ISleepService
    {
        void Sleep(TimeSpan timeSpan);
    }
}