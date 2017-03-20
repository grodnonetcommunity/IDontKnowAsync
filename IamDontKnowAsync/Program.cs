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
        }

        public static void MakeRequest()
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            const string host = "localhost";

            socket.Connect(new DnsEndPoint(host, 80));

            var request = $"GET http://{host}/ HTTP/1.1\r\n" +
                          "Host: localhost\r\n" +
                          "Accept: text/html\r\n\r\n";

            var buffer = Encoding.ASCII.GetBytes(request);
            var sended = socket.Send(buffer, 0, buffer.Length, SocketFlags.None);

            Console.WriteLine($"Request sended: {sended}");

            socket.Disconnect(false);
        }
    }
}
