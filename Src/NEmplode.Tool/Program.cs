using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NEmplode.Tool
{
    class EmpegFinder
    {
        class StateObject
        {
            public readonly Socket Socket;
            public readonly byte[] ResponseBytes;
            public const int ResponseBufferSize = 1024;

            public StateObject(Socket socket)
            {
                Socket = socket;
                ResponseBytes = new byte[ResponseBufferSize];
            }
        };

        [STAThread]
        static void Main(string[] args)
        {
            // To find an empeg, we do a UDP broadcast on port 8300 with a single question mark in it.
            const int empegDiscoveryPort = 8300;

            byte[] requestBytes = new byte[] { 0x3F }; // Single '?'

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                var localEndPoint = new IPEndPoint(IPAddress.Any, empegDiscoveryPort);
                socket.Bind(localEndPoint);

                socket.SetSocketOption(SocketOptionLevel.Socket,
                                  SocketOptionName.Broadcast, 1);

                var remoteEndPoint = new IPEndPoint(IPAddress.Broadcast, empegDiscoveryPort);
                Console.WriteLine("Looking for empegs on {0}...", remoteEndPoint);
                socket.SendTo(requestBytes, remoteEndPoint);

                StateObject so = new StateObject(socket);
                EndPoint ep = new IPEndPoint(0, 0);
                socket.BeginReceiveFrom(so.ResponseBytes, 0,
                                   StateObject.ResponseBufferSize, 0,
                                   ref ep,
                                   ResponseCallback, so);

                // Then wait for 5s.
                Thread.Sleep(5000);
            }
        }

        public static void ResponseCallback(IAsyncResult ar)
        {
            StateObject so = (StateObject)ar.AsyncState;
            Socket client = so.Socket;

            EndPoint ep = new IPEndPoint(IPAddress.Any, 0);
            int responseBytesReceived = client.EndReceiveFrom(ar, ref ep);

            Console.WriteLine("Received response from {0}", ep);

            string responseString = Encoding.ASCII.GetString(so.ResponseBytes, 0, responseBytesReceived);

            Console.WriteLine("Response contains: {0}", responseString);

            // Issue another BeginReceiveFrom
            client.BeginReceiveFrom(so.ResponseBytes, 0, StateObject.ResponseBufferSize, 0, ref ep, ResponseCallback, so);
        }
    }
}