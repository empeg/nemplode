using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using NEmplode.EmpegCar.Discovery;

namespace NEmplode.Discovery
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener() { TraceOutputOptions = TraceOptions.DateTime | TraceOptions.ProcessId | TraceOptions.ThreadId });

            TaskScheduler.UnobservedTaskException += (sender, e) => Console.WriteLine(e.Exception);
            
            var finder = new NetworkBroadcastEmpegCarFinder();

            // It's designed to be used asynchronously. That is: you'll receive events when an empeg is found or lost.
            finder.FoundEmpeg +=(sender, e) => Console.WriteLine("Found empeg-car {0}", e.Locator);
            finder.LostEmpeg += (sender, e) => Console.WriteLine("Lost empeg-car {0}", e.Locator);

            ManualResetEvent stop = new ManualResetEvent(false);
            Console.CancelKeyPress += (sender, e) => 
                                          {
                                              Console.WriteLine("^C");
                                              stop.Set();
                                          };

            finder.Start();

            stop.WaitOne();

            finder.Dispose();
        }
    }
}