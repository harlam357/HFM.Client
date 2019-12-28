
using System;
using System.Threading.Tasks;

using HFM.Client.Internal;
using HFM.Client.Sockets;

namespace HFM.Client
{
    /// <summary>
    /// Provides the abstract base class for a Folding@Home client connection.
    /// </summary>
    public abstract class FahClientConnectionBase
    {
        /// <summary>
        /// Occurs when the value of the <see cref="Connected"/> property has changed.
        /// </summary>
        public event EventHandler<FahClientConnectedChangedEventArgs> ConnectedChanged;

        /// <summary>
        /// Raises the <see cref="ConnectedChanged"/> event.
        /// </summary>
        /// <param name="e">A <see cref="FahClientConnectedChangedEventArgs"/> that contains event data.</param>
        protected virtual void OnConnectedChanged(FahClientConnectedChangedEventArgs e)
        {
            ConnectedChanged?.Invoke(this, e);
        }

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
        public abstract bool Connected { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientConnectionBase"/> class.
        /// </summary>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
        protected FahClientConnectionBase(string host, int port)
        {
            if (host is null) throw new ArgumentNullException(nameof(host));
            if (!ValidationHelper.ValidateTcpPort(port)) throw new ArgumentOutOfRangeException(nameof(port));

            Host = host;
            Port = port;
        }

        /// <summary>
        /// Opens the connection to the client.
        /// </summary>
        public abstract void Open();

        /// <summary>
        /// Asynchronously opens the connection to the client.
        /// </summary>
        public abstract Task OpenAsync();

        /// <summary>
        /// Closes the connection to the client.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Creates and returns a <see cref="FahClientCommandBase"/> object associated with the current connection.
        /// </summary>
        public FahClientCommandBase CreateCommand()
        {
            return CreateClientCommand();
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientCommandBase"/> object associated with the current connection.
        /// </summary>
        protected abstract FahClientCommandBase CreateClientCommand();

        /// <summary>
        /// Creates and returns a <see cref="FahClientReaderBase"/> object associated with the current connection.
        /// </summary>
        public FahClientReaderBase CreateReader()
        {
            return CreateClientReader();
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientReaderBase"/> object associated with the current connection.
        /// </summary>
        protected abstract FahClientReaderBase CreateClientReader();
    }

    /// <summary>
    /// Folding@Home client connection.
    /// </summary>
    public class FahClientConnection : FahClientConnectionBase, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the connection is connected to a client.
        /// </summary>
        public override bool Connected => (TcpConnection?.Connected).GetValueOrDefault();

        public virtual TcpConnection TcpConnection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientConnection"/> class.
        /// </summary>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
        public FahClientConnection(string host, int port)
           : this(TcpConnectionFactory.Default, host, port)
        {

        }

        private readonly TcpConnectionFactory _tcpConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientConnection"/> class.
        /// </summary>
        /// <param name="tcpConnectionFactory">The factory that will be used to create connections for TCP network services.</param>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
        public FahClientConnection(TcpConnectionFactory tcpConnectionFactory, string host, int port)
           : base(host, port)
        {
            _tcpConnectionFactory = tcpConnectionFactory;
        }

        /// <summary>
        /// Opens the connection to the client.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is already open.</exception>
        public override void Open()
        {
            if (Connected) throw new InvalidOperationException("The connection is already open.");

            // dispose of existing connection
            TcpConnection?.Close();
            // create new TcpConnection and connect
            TcpConnection = _tcpConnectionFactory.Create();
            TcpConnection.Connect(Host, Port, ConnectionTimeout);
            // if connection state changed, raise event
            if (Connected)
            {
                OnConnectedChanged(new FahClientConnectedChangedEventArgs(Connected));
            }
        }

        /// <summary>
        /// Asynchronously opens the connection to the client.
        /// </summary>
        /// <exception cref="InvalidOperationException">The connection is already open.</exception>
        public override async Task OpenAsync()
        {
            if (Connected) throw new InvalidOperationException("The connection is already open.");

            // dispose of existing connection
            TcpConnection?.Close();
            // create new TcpConnection and connect
            TcpConnection = _tcpConnectionFactory.Create();
            await TcpConnection.ConnectAsync(Host, Port, ConnectionTimeout).ConfigureAwait(false);
            // if connection state changed, raise event
            if (Connected)
            {
                OnConnectedChanged(new FahClientConnectedChangedEventArgs(Connected));
            }
        }

        /// <summary>
        /// Closes the connection to the client.
        /// </summary>
        public override void Close()
        {
            bool connected = Connected;
            // dispose of existing connection
            TcpConnection?.Close();
            TcpConnection = null;
            // if connection state changed, raise event
            if (connected != Connected)
            {
                OnConnectedChanged(new FahClientConnectedChangedEventArgs(Connected));
            }
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientCommand"/> object associated with the current connection.
        /// </summary>
        /// <param name="commandText">The Folding@Home client command statement.</param>
        public FahClientCommand CreateCommand(string commandText)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientCommand"/> object associated with the current connection.
        /// </summary>
        public new FahClientCommand CreateCommand()
        {
            return (FahClientCommand)CreateClientCommand();
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientCommandBase"/> object associated with the current connection.
        /// </summary>
        protected override FahClientCommandBase CreateClientCommand()
        {
            return new FahClientCommand(this);
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientReader"/> object associated with the current connection.
        /// </summary>
        /// <param name="readTimeout">The amount of time the <see cref="FahClientReader"/> will wait to read the next message.  Value of zero specifies no timeout.</param>
        public FahClientReader CreateReader(int readTimeout)
        {
            var reader = CreateReader();
            reader.ReadTimeout = readTimeout;
            return reader;
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientReader"/> object associated with the current connection.
        /// </summary>
        public new FahClientReader CreateReader()
        {
            return (FahClientReader)CreateClientReader();
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientReaderBase"/> object associated with the current connection.
        /// </summary>
        protected override FahClientReaderBase CreateClientReader()
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
