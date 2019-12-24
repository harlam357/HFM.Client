
using System;
using System.Net;
using System.Net.Sockets;

namespace HFM.Client.Sockets
{
    /// <summary>
    /// Simple TCP listener bound to localhost.
    /// </summary>
    internal class LocalTcpListener : IDisposable
    {
        internal const string Host = "127.0.0.1";
        internal const int Port = 13000;

        private readonly TcpListener _tcpListener;

        internal LocalTcpListener()
        {
            _tcpListener = new TcpListener(IPAddress.Parse(Host), Port);
            _tcpListener.Start();
        }

        void IDisposable.Dispose()
        {
            _tcpListener.Stop();
        }
    }
}
