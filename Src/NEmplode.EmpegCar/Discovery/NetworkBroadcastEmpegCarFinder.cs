using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NEmplode.EmpegCar.Discovery
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

    // TODO: I had a thought that it would be nice to have a listener that simply repeated BeginReceive/EndReceive pairs, and another that periodically sent out the discovery packet.
    public class NetworkBroadcastEmpegCarFinder : IDisposable
    {
        private const int _empegDiscoveryPort = 8300;

        private Thread _finderThread;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly UdpClient _client;

        public NetworkBroadcastEmpegCarFinder()
        {
            _client = new UdpClient(new IPEndPoint(IPAddress.Any, _empegDiscoveryPort)) { EnableBroadcast = true };
        }

        public event EventHandler<EmpegCarFinderEventArgs> FoundEmpeg;
        public event EventHandler<EmpegCarFinderEventArgs> LostEmpeg;

        public void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _finderThread = new Thread(ThreadRoutine) { IsBackground = true };
            _finderThread.Start();
        }

        private void ThreadRoutine()
        {
            Console.WriteLine("ThreadRoutine");
            Repeat.Interval(Discover, TimeSpan.FromSeconds(30), _cancellationTokenSource.Token);
        }

        private void Discover()
        {
            Console.WriteLine("Discover");
            // TODO: using (EmpegCarDiscoveryClient client = ...) -- then we don't need to pass a bunch of stuff in ar.AsyncState.
            SendDiscoveryPacket();

            _client.BeginReceive(RequestCallback, DateTime.UtcNow.AddSeconds(10));
        }

        private void SendDiscoveryPacket()
        {
            Console.WriteLine("SendDiscoveryPacket");

            byte[] requestBytes = new byte[] { 0x3F };  // Single '?'
            var discoveryEndPoint = new IPEndPoint(IPAddress.Broadcast, _empegDiscoveryPort);
            _client.Send(requestBytes, requestBytes.Length, discoveryEndPoint);
        }

        private void RequestCallback(IAsyncResult ar)
        {
            Console.WriteLine("RequestCallback");

            DateTime expiryTime = (DateTime)ar.AsyncState;
            IPEndPoint remoteEndPoint = null;
            var responseBytes = _client.EndReceive(ar, ref remoteEndPoint);
            if (responseBytes.Length != 1)
            {
                var empegLocator = EmpegLocator.Create(remoteEndPoint, responseBytes);
                Console.WriteLine(empegLocator);
            }

            if (DateTime.UtcNow < expiryTime)
            {
                Console.WriteLine("Issuing another receive");
                _client.BeginReceive(RequestCallback, expiryTime);
            }
            else
            {
                Console.WriteLine("Not issuing another receive");
            }
        }

        private void Stop()
        {
            _cancellationTokenSource.Cancel();
            _finderThread.Join();
        }

        public void Dispose()
        {
            Stop();
            _client.Close();
        }
    }

    public class EmpegLocator
    {
        public static EmpegLocator Create(IPEndPoint ep, byte[] bytes)
        {
            return new EmpegLocator();
        }
    }

    public class EmpegCarFinderEventArgs : EventArgs
    {
        public EmpegCarFinderEventArgs(EmpegLocator locator)
        {
            Locator = locator;
        }

        public EmpegLocator Locator { get; private set; }
    }
}
