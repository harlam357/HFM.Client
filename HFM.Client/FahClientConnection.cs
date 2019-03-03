
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
         if (host == null) throw new ArgumentNullException(nameof(host));
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
      /// Creates a <see cref="FahClientCommandBase"/> object that can be used to send a command to the client.
      /// </summary>
      public abstract FahClientCommandBase CreateCommand();
   }

   /// <summary>
   /// Folding@Home client connection.
   /// </summary>
   public class FahClientConnection : FahClientConnectionBase, IDisposable
   {
      /// <summary>
      /// Gets a value indicating whether the connection is connected to a client.
      /// </summary>
      public override bool Connected => (_tcpConnection?.Connected).GetValueOrDefault();

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

      private TcpConnection _tcpConnection;

      /// <summary>
      /// Opens the connection to the client.
      /// </summary>
      /// <exception cref="InvalidOperationException">The connection is already open.</exception>
      public override void Open()
      {
         if (Connected) throw new InvalidOperationException("The connection is already open.");

         // dispose of any previous client
         _tcpConnection?.Close();
         // create new TcpConnection and connect
         _tcpConnection = _tcpConnectionFactory.Create();
         _tcpConnection.Connect(Host, Port, ConnectionTimeout);
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

         // dispose of any previous client
         _tcpConnection?.Close();
         // create new TcpConnection and connect
         _tcpConnection = _tcpConnectionFactory.Create();
         await _tcpConnection.ConnectAsync(Host, Port, ConnectionTimeout).ConfigureAwait(false);
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
         // close the connection
         _tcpConnection?.Close();
         // clear the cached data writer
         _dataWriter = null;
         // if connection state changed, raise event
         if (connected != Connected)
         {
            OnConnectedChanged(new FahClientConnectedChangedEventArgs(Connected));
         }
      }

      /// <summary>
      /// Creates a <see cref="FahClientCommand"/> object that can be used to send a command to the client.
      /// </summary>
      public override FahClientCommandBase CreateCommand()
      {
         return new FahClientCommand(this);
      }

      private FahClientDataWriter _dataWriter;

      /// <summary>
      /// Gets a <see cref="FahClientDataWriter"/> that can be used to write data to the client.
      /// </summary>
      /// <exception cref="InvalidOperationException">The connection is not open.</exception>
      public virtual FahClientDataWriter GetDataWriter()
      {
         if (!Connected) throw new InvalidOperationException("The connection is not open.");

         return _dataWriter ?? (_dataWriter = FahClientDataWriter.Create(_tcpConnection.GetStream()));
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
