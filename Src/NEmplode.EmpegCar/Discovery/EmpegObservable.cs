using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace NEmplode.EmpegCar.Discovery
{
    // TODO: Make this non-static, call it NetworkBroadcastEmpegFinder and have it return the observable from an IFinder.GetObservable method. That way, they can be combined?
    public static class EmpegObservable
    {
        private const int _empegDiscoveryPort = 8300;

        public static IObservable<EmpegLocator> Create(TimeSpan discoveryInterval)
        {
            return Observable.Create<EmpegLocator>(observer =>
                                                       {
                                                           var localEndPoint = new IPEndPoint(IPAddress.Any, _empegDiscoveryPort);
                                                           UdpClient client = new UdpClient(localEndPoint) { EnableBroadcast = true };

                                                           // We want two observables -- one will watch for empeg notifications; one will periodically send out discovery packets.
                                                           var discoveryTimer = Observable.Timer(DateTimeOffset.UtcNow, discoveryInterval);
                                                           var discoverySubscription = discoveryTimer.Subscribe(_ => SendDiscoveryPacket(client));

                                                           var responseListener = CreateResponseObservable(client);
                                                           var responseListenerSubscription = responseListener.Subscribe(observer);

                                                           return () =>
                                                                      {
                                                                          discoverySubscription.Dispose();
                                                                          responseListenerSubscription.Dispose();
                                                                          client.Close();
                                                                      };
                                                       });
        }

        private static IObservable<EmpegLocator> CreateResponseObservable(UdpClient client)
        {
            // Don't call BeginReceive until someone subscribes.
            return Observable.Defer(
                () =>
                    {
                        IPEndPoint ep = null;
                        var receive = Observable.FromAsyncPattern(
                            client.BeginReceive,
                            ar => client.EndReceive(ar, ref ep));

                        // Repeat will repeatedly subscribe; Defer will call receive() each time.
                        return Observable.Defer(receive)
                            .Where(bytes => bytes.Length != 1)
                            .Select(bytes => EmpegLocator.Create(ep, bytes))
                            .Repeat();
                    });
        }

        private static void SendDiscoveryPacket(UdpClient client)
        {
            byte[] requestBytes = new byte[] { 0x3F }; // Single '?'
            var discoveryEndPoint = new IPEndPoint(IPAddress.Broadcast, _empegDiscoveryPort);
            client.Send(requestBytes, requestBytes.Length, discoveryEndPoint);
        }
    }
}