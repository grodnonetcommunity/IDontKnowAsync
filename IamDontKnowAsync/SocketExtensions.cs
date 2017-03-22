using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace IamDontKnowAsync
{
    public static class SocketExtensions
    {
        public static Task ConnectTask(this Socket socket, EndPoint remote, TaskCreationOptions creationOptions = TaskCreationOptions.None)
        {
            return Task.Factory.FromAsync(socket.BeginConnect, socket.EndConnect, remote, null, creationOptions);
        }

        public static Task<int> SendTask(this Socket socket, byte[] buffer, int offset, int count, TaskCreationOptions creationOptions = TaskCreationOptions.None)
        {
            return Task.Factory.FromAsync(socket.BeginSend, socket.EndSend, buffer, offset, count, null, creationOptions);
        }

        public static Task<int> ReceivedTask(this Socket socket, byte[] buffer, int offset, int count, TaskCreationOptions creationOptions = TaskCreationOptions.None)
        {
            return Task.Factory.FromAsync(socket.BeginReceive, socket.EndReceive, buffer, offset, count, null, creationOptions);
        }

        public static Task DisconnectTask(this Socket socket, bool reuseSocket, TaskCreationOptions creationOptions = TaskCreationOptions.None)
        {
            return Task.Factory.FromAsync(socket.BeginDisconnect, socket.EndDisconnect, reuseSocket, null, creationOptions);
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