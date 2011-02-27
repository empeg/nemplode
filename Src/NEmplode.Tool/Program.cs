using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NEmplode.Tool
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // To find an empeg, we do a UDP broadcast on port 8300 with a single question mark in it.
            const int empegDiscoveryPort = 8300;

            var discovery = Observable.Defer(() =>
                                                 {
                                                     byte[] requestBytes = new byte[] { 0x3F }; // Single '?'

                                                     var localEndPoint = new IPEndPoint(IPAddress.Any, empegDiscoveryPort);
                                                     UdpClient client = new UdpClient(localEndPoint);
                                                     client.EnableBroadcast = true;

                                                     var remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, empegDiscoveryPort);
                                                     client.Send(requestBytes, requestBytes.Length, remoteEndPoint);

                                                     IPEndPoint ep = null;
                                                     var receiveAsync = Observable.FromAsyncPattern(client.BeginReceive,
                                                                                                    ar => client.EndReceive(ar, ref ep));

                                                     return Observable.Defer(receiveAsync).Repeat().Select(bytes => Tuple.Create(ep, bytes));
                                                 });

            discovery.Subscribe(x => Console.WriteLine("{0}: {1}", x.Item1, Encoding.ASCII.GetString(x.Item2)));
            Thread.Sleep(5000);
        }
    }
}