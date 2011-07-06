using NEmplode.Sync.Empeg;
using NEmplode.Sync.LocalMusic;

namespace NEmplode.Sync
{
    static class Program
    {
        static void Main(string[] args)
        {
            // At this point, it's push only.
            var source = new LocalMusicStore(@"\\server\Music\mp3");
            var destination = new EmpegMusicStore();
            var client = new SynchronizationClient(source, destination);
            client.Synchronize();
        }
    }
}
