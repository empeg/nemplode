using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NEmplode.Discovery
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // To find an empeg, we do a UDP broadcast on port 8300 with a single question mark in it.
            const int empegDiscoveryPort = 8300;

            // Defer prevents us from creating the UdpClient and calling Send until someone actually subscribes.
            var discovery = Observable.Defer(
                // using (var client = new UdpClient(...)) { ... }
                () => Observable.Using(
                    () => new UdpClient(new IPEndPoint(IPAddress.Any, empegDiscoveryPort)) { EnableBroadcast = true },
                    client =>
                        {
                            // Send out the request.
                            byte[] requestBytes = new byte[] { 0x3F }; // Single '?'
                            var remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, empegDiscoveryPort);
                            client.Send(requestBytes, requestBytes.Length, remoteEndPoint);

                            // Set up the observable for receiveAsync.
                            IPEndPoint ep = null;
                            var receiveAsync = Observable.FromAsyncPattern(
                                client.BeginReceive,
                                ar => client.EndReceive(ar, ref ep));

                            // Combine Defer and Repeat -> repeatedly call receiveAsync()
                            return Observable.Defer(receiveAsync)
                                .Select(bytes => Tuple.Create(ep, bytes))
                                .Repeat();
                        }));

            discovery.Subscribe(x => Console.WriteLine("{0}: {1}", x.Item1, Encoding.ASCII.GetString(x.Item2)));

            // TODO: How to get Rx to just wait on the observable sequence on the current thread for the timeout?
            Thread.Sleep(5000);
        }
    }
}