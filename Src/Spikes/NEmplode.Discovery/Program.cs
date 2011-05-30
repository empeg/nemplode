using System;
using System.Diagnostics;
using System.Threading;
using NEmplode.EmpegCar.Discovery;

namespace NEmplode.Discovery
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener()
                                    { TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ProcessId | TraceOptions.ThreadId });

            TimeSpan discoveryInterval = TimeSpan.FromSeconds(5);
            var empegs = EmpegObservable.Create(discoveryInterval);
            
            ManualResetEvent stop = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, e) =>
                                          {
                                              Console.WriteLine("^C");
                                              stop.Set();
                                          };

            empegs.Subscribe(Console.WriteLine);

            stop.WaitOne();
        }
    }
}