
using System;
using System.Threading.Tasks;

using HFM.Client.Sockets;

namespace HFM.Client
{
    /// <summary>
    /// Folding@Home client connection.
    /// </summary>
    public class FahClientTcpConnection : FahClientConnection, IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether the connection is connected to a client.
        /// </summary>
        public override bool Connected => (TcpConnection?.Connected).GetValueOrDefault();

        public virtual TcpConnection TcpConnection { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientTcpConnection"/> class.
        /// </summary>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
        public FahClientTcpConnection(string host, int port)
           : this(TcpConnectionFactory.Default, host, port)
        {

        }

        private readonly TcpConnectionFactory _tcpConnectionFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FahClientTcpConnection"/> class.
        /// </summary>
        /// <param name="tcpConnectionFactory">The factory that will be used to create connections for TCP network services.</param>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
        public FahClientTcpConnection(TcpConnectionFactory tcpConnectionFactory, string host, int port)
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
        /// Creates and returns a <see cref="FahClientTextCommand"/> object associated with the current connection.
        /// </summary>
        /// <param name="commandText">The Folding@Home client command statement.</param>
        public FahClientTextCommand CreateCommand(string commandText)
        {
            var command = CreateCommand();
            command.CommandText = commandText;
            return command;
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientTextCommand"/> object associated with the current connection.
        /// </summary>
        public new FahClientTextCommand CreateCommand()
        {
            return (FahClientTextCommand)OnCreateCommand();
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientCommand"/> object associated with the current connection.
        /// </summary>
        protected override FahClientCommand OnCreateCommand()
        {
            return new FahClientTextCommand(this);
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientMessageReader"/> object associated with the current connection.
        /// </summary>
        public new FahClientMessageReader CreateReader()
        {
            return (FahClientMessageReader)OnCreateReader();
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientReader"/> object associated with the current connection.
        /// </summary>
        protected override FahClientReader OnCreateReader()
        {
            return new FahClientMessageReader(this);
        }

        private bool _disposed;

        /// <summary>
        /// Releases all resources used by the <see cref="FahClientTcpConnection"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="FahClientTcpConnection"/> and optionally releases the managed resources.
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
