using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IamDontKnowAsync
{
    class Program
    {
        static void Main()
        {
            MakeRequest();

            Console.ReadLine();
        }

        public static void MakeRequest()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            const string host = "localhost";
            
            var args0 = new SocketAsyncEventArgs{ RemoteEndPoint = new DnsEndPoint(host, 80) };
            args0.Completed += (s0, a0) =>
            {
                var request = $"GET http://{host}/ HTTP/1.1\r\n" +
                              "Host: localhost\r\n" +
                              "Accept: text/html\r\n\r\n";

                var buffer = Encoding.ASCII.GetBytes(request);
                var args1 = new SocketAsyncEventArgs();
                args1.SetBuffer(buffer, 0, buffer.Length);
                args1.Completed += (s1, a1) =>
                {
                    var sended = a1.BytesTransferred;

                    Console.WriteLine($"Request sended: {sended}");

                    var response = new byte[1 * 1024 * 1024];

                    var args2 = new SocketAsyncEventArgs();
                    args2.SetBuffer(response, 0, response.Length);
                    args2.Completed += (s2, a2) =>
                    {
                        var received = a2.BytesTransferred;

                        Console.WriteLine($"Response received: {received}");
                        Console.WriteLine(Encoding.UTF8.GetString(response, 0, received));

                        var args3 = new SocketAsyncEventArgs {DisconnectReuseSocket = false};
                        args3.Completed += (s3, a3) =>
                        {
                            Console.WriteLine("Disconnected");
                        };
                        socket.DisconnectAsync(args3);
                    };
                    socket.ReceiveAsync(args2);
                };
                socket.SendAsync(args1);
            };
            socket.ConnectAsync(args0);
        }
    }
}
