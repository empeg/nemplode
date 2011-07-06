using System.Diagnostics;
using NEmplode.Sync.Empeg;
using NEmplode.Sync.LocalMusic;

namespace NEmplode.Sync
{
    static class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            Trace.AutoFlush = true;

            // Note: At this point, it's push only. And it assumes a single-parent hierarchy on the empeg.
            var source = new LocalMusicStore(@"\\server\Music\mp3");
            var destination = new EmpegMusicStore("http://10.0.0.99");

            var client = new SynchronizationClient(source, destination);
            client.Synchronize();
        }
    }
}
