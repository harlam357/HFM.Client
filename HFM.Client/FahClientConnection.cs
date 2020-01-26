
using System;
using System.Threading.Tasks;

using HFM.Client.Internal;
using HFM.Client.Sockets;

namespace HFM.Client
{
    /// <summary>
    /// Folding@Home client connection.
    /// </summary>
    public class FahClientConnection : IDisposable
    {
        /// <summary>
        /// Gets the name of the remote host.
        /// </summary>
        public string Host { get; }

        /// <summary>
        /// Gets the port number of the remote host.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Gets or sets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
        /// </summary>
        /// <returns>The time (in milliseconds) to wait for a connection to open. The default value is 5000 milliseconds.</returns>
        public int ConnectionTimeout { get; set; } = 5000;

        /// <summary>
        /// Gets a value indicating whether the connection is connected to a client.
        /// </summary>
        public virtual bool Connected => (TcpConnection?.Connected).GetValueOrDefault();

        /// <summary>
        /// Gets the current <see cref="Sockets.TcpConnection"/>.
        /// </summary>
        public virtual TcpConnection TcpConnection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientConnection"/> class.
        /// </summary>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
        public FahClientConnection(string host, int port)
           : this(host, port, TcpConnectionFactory.Default)
        {

        }

        private readonly TcpConnectionFactory _tcpConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientConnection"/> class.
        /// </summary>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <param name="tcpConnectionFactory">The factory that will be used to create connections for TCP network services.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
        public FahClientConnection(string host, int port, TcpConnectionFactory tcpConnectionFactory)
        {
            if (host is null) throw new ArgumentNullException(nameof(host));
            if (!ValidationHelper.ValidateTcpPort(port)) throw new ArgumentOutOfRangeException(nameof(port));

            Host = host;
            Port = port;
            _tcpConnectionFactory = tcpConnectionFactory;
        }

        /// <summary>
        /// Opens the connection to the client.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is already open.</exception>
        public virtual void Open()
        {
            if (Connected) throw new InvalidOperationException("The connection is already open.");

            // dispose of existing connection
            TcpConnection?.Close();
            // create new TcpConnection and connect
            TcpConnection = _tcpConnectionFactory.Create();
            TcpConnection.Connect(Host, Port, ConnectionTimeout);
        }

        /// <summary>
        /// Asynchronously opens the connection to the client.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is already open.</exception>
        public virtual async Task OpenAsync()
        {
            if (Connected) throw new InvalidOperationException("The connection is already open.");

            // dispose of existing connection
            TcpConnection?.Close();
            // create new TcpConnection and connect
            TcpConnection = _tcpConnectionFactory.Create();
            await TcpConnection.ConnectAsync(Host, Port, ConnectionTimeout).ConfigureAwait(false);
        }

        /// <summary>
        /// Closes the connection to the client.
        /// </summary>
        public virtual void Close()
        {
            // dispose of existing connection
            TcpConnection?.Close();
            TcpConnection = null;
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientCommand"/> object associated with the current connection.
        /// </summary>
        public FahClientCommand CreateCommand()
        {
            return OnCreateCommand();
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientCommand"/> object associated with the current connection.
        /// </summary>
        /// <param name="commandText">The Folding@Home client command statement.</param>
        public FahClientCommand CreateCommand(string commandText)
        {
            var command = OnCreateCommand();
            command.CommandText = commandText;
            return command;
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientCommand"/> object associated with the current connection.
        /// </summary>
        protected virtual FahClientCommand OnCreateCommand()
        {
            return new FahClientCommand(this);
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientReader"/> object associated with the current connection.
        /// </summary>
        public FahClientReader CreateReader()
        {
            return OnCreateReader();
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientReader"/> object associated with the current connection.
        /// </summary>
        protected virtual FahClientReader OnCreateReader()
        {
            return new FahClientReader(this);
        }

        private bool _disposed;

        /// <summary>
        /// Releases all resources used by the <see cref="FahClientConnection"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="FahClientConnection"/> and optionally releases the managed resources.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Close();
                }
            }
            _disposed = true;
        }
    }
}
