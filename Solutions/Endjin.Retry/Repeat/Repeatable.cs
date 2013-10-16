using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Endjin.Core.Repeat
{
    public static class Repeatable
    {
        public static void Repeat(CancellationToken cancellationToken, TimeSpan periodicity, Action<CancellationToken> action)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                action(cancellationToken);
                cancellationToken.WaitHandle.WaitOne(periodicity);
            }
        }

        public static async Task RepeatAsync(CancellationToken cancellationToken, TimeSpan periodicity, Func<CancellationToken, Task> action)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await action(cancellationToken);
                cancellationToken.WaitHandle.WaitOne(periodicity);
            }
        }
    }
}
