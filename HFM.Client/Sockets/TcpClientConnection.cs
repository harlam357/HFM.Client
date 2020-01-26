
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

using HFM.Client.Internal;

namespace HFM.Client.Sockets
{
    /// <summary>
    /// Provides connections for TCP network services using a <see cref="System.Net.Sockets.TcpClient"/>.
    /// </summary>
    public class TcpClientConnection : TcpConnection, IDisposable
    {
        /// <summary>
        /// Gets the underlying <see cref="System.Net.Sockets.TcpClient" />.
        /// </summary>
        public TcpClient TcpClient { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpClientConnection" /> class.
        /// </summary>
        public TcpClientConnection()
        {
            TcpClient = new TcpClient();
        }

        /// <summary>
        /// Gets a value indicating whether the TCP connection is connected to a remote host.
        /// </summary>
        public override bool Connected => TcpClient.Client != null && TcpClient.Connected; //&& GetTcpState(TcpClient) == TcpState.Established;

        // https://stackoverflow.com/questions/1387459/how-to-check-if-tcpclient-connection-is-closed
        private static TcpState GetTcpState(TcpClient tcpClient)
        {
            var localEndPoint = ToIPEndPoint(tcpClient.Client.LocalEndPoint);
            var remoteEndPoint = ToIPEndPoint(tcpClient.Client.RemoteEndPoint);
            
            // if not IPEndPoints, assume Established state
            if (localEndPoint == null || remoteEndPoint == null) return TcpState.Established;

            var information = IPGlobalProperties.GetIPGlobalProperties()
                .GetActiveTcpConnections()
                .FirstOrDefault(x =>
                    x.LocalEndPoint.Equals(localEndPoint) &&
                    x.RemoteEndPoint.Equals(remoteEndPoint)
                );

            return information?.State ?? TcpState.Unknown;
        }

        private static IPEndPoint ToIPEndPoint(EndPoint endPoint)
        {
            return endPoint is IPEndPoint ipEndPoint
                ? ipEndPoint.Address.IsIPv4MappedToIPv6
                    ? new IPEndPoint(ipEndPoint.Address.MapToIPv4(), ipEndPoint.Port)
                    : ipEndPoint
                : null;
        }

        /// <summary>
        /// Connects to a remote TCP host using the specified host and port number.
        /// </summary>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <param name="timeout">The time (in milliseconds) to wait while trying to establish a connection before terminating the attempt and generating an error.</param>
        /// <exception cref="ObjectDisposedException" />
        /// <exception cref="InvalidOperationException">The connection is already connected.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
        /// <exception cref="TimeoutException">A connection was not made before the timeout duration.</exception>
        public override void Connect(string host, int port, int timeout)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().FullName);
            if (Connected) throw new InvalidOperationException("The connection is already connected.");
            if (host is null) throw new ArgumentNullException(nameof(host));
            if (!ValidationHelper.ValidateTcpPort(port)) throw new ArgumentOutOfRangeException(nameof(port));

            if (!TcpClient.ConnectAsync(host, port).Wait(timeout))
            {
                Close();
                throw new TimeoutException("Connection attempt has timed out.");
            }
        }

        /// <summary>
        /// Connects asynchronously to a remote TCP host using the specified host and port number.
        /// </summary>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <param name="timeout">The time (in milliseconds) to wait while trying to establish a connection before terminating the attempt and generating an error.</param>
        /// <exception cref="ObjectDisposedException" />
        /// <exception cref="InvalidOperationException">The connection is already connected.</exception>
        /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
        /// <exception cref="TimeoutException">A connection was not made before the timeout duration.</exception>
        public override async Task ConnectAsync(string host, int port, int timeout)
        {
            if (_disposed) throw new ObjectDisposedException(GetType().FullName);
            if (Connected) throw new InvalidOperationException("The connection is already connected.");
            if (host is null) throw new ArgumentNullException(nameof(host));
            if (!ValidationHelper.ValidateTcpPort(port)) throw new ArgumentOutOfRangeException(nameof(port));

            var connectTask = TcpClient.ConnectAsync(host, port);
            if (connectTask != await Task.WhenAny(connectTask, Task.Delay(timeout)).ConfigureAwait(false))
            {
                Close();
                throw new TimeoutException("Connection attempt has timed out.");
            }
        }

        /// <summary>
        /// Disposes this <see cref="TcpClientConnection" /> instance and requests that the TCP connection be closed.
        /// </summary>
        public override void Close()
        {
            ((IDisposable)this).Dispose();
        }

        /// <summary>
        /// Returns the <see cref="NetworkStream" /> used to send and receive data if connected; otherwise, null.
        /// </summary>
        public override Stream GetStream()
        {
            return Connected ? TcpClient.GetStream() : null;
        }

        private bool _disposed;

        void IDisposable.Dispose()
        {
            TcpClient.Close();
            _disposed = true;
        }
    }
}
