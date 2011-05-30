using System;
using System.Threading;

namespace NEmplode.Core
{
    public static class Repeat
    {
        public static void Interval(Action action, TimeSpan interval, CancellationToken cancellationToken)
        {
            for (; ; )
            {
                action();

                if (cancellationToken.WaitHandle.WaitOne(interval))
                    break;
            }
        }
    }
}