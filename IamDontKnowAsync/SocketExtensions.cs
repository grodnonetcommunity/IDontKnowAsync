using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace IamDontKnowAsync
{
    public static class SocketExtensions
    {
        public static Task ConnectTask(this Socket socket, EndPoint remote)
        {
            return Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, remote, null);
        }

        public static Task<int> SendTask(this Socket socket, byte[] buffer, int offset, int count)
        {
            return Task.Factory.FromAsync(socket.BeginSend, socket.EndSend, buffer, offset, count, null);
        }

        public static Task<int> ReceivedTask(this Socket socket, byte[] buffer, int offset, int count)
        {
            return Task.Factory.FromAsync(socket.BeginReceive, socket.EndReceive, buffer, offset, count, null);
        }

        public static Task DisconnectTask(this Socket socket, bool reuseSocket)
        {
            return Task.Factory.FromAsync(socket.BeginDisconnect, socket.EndDisconnect, reuseSocket, null);
        }

        private static IAsyncResult BeginSend(this Socket socket, byte[] buffer, int offset, int count,
            AsyncCallback asyncCallback, object state)
        {
            return socket.BeginSend(buffer, offset, count, SocketFlags.None, asyncCallback, state);
        }

        private static IAsyncResult BeginReceive(this Socket socket, byte[] buffer, int offset, int count,
            AsyncCallback asyncCallback, object state)
        {
            return socket.BeginReceive(buffer, offset, count, SocketFlags.None, asyncCallback, state);
        }
    }
}