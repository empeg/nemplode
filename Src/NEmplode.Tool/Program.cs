using System;
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

            byte[] requestBytes = new byte[] { 0x3F }; // Single '?'

            var localEndPoint = new IPEndPoint(IPAddress.Any, empegDiscoveryPort);
            using (UdpClient client = new UdpClient(localEndPoint))
            {
                client.EnableBroadcast = true;

                var remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, empegDiscoveryPort);
                Console.WriteLine("Looking for empegs on {0}...", remoteEndPoint);
                client.Send(requestBytes, requestBytes.Length, remoteEndPoint);

                client.BeginReceive(ResponseCallback, client);

                // Then wait for 5s.
                Thread.Sleep(5000);
            }
        }

        public static void ResponseCallback(IAsyncResult ar)
        {
            UdpClient client = (UdpClient)ar.AsyncState;

            IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            byte[] bytesReceived = client.EndReceive(ar, ref ep);

            Console.WriteLine("Received response from {0}", ep);

            string responseString = Encoding.ASCII.GetString(bytesReceived);

            Console.WriteLine("Response contains: {0}", responseString);

            // Issue another BeginReceiveFrom
            client.BeginReceive(ResponseCallback, client);
        }
    }
}