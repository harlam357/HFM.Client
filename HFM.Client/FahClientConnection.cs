
using System;
using System.Threading.Tasks;

using HFM.Client.Internal;
using HFM.Client.Sockets;

namespace HFM.Client
{
   public abstract class FahClientConnectionBase
   {
      /// <summary>
      /// Occurs when the value of the <see cref="Connected"/> property has changed.
      /// </summary>
      public event EventHandler<FahClientConnectedChangedEventArgs> ConnectedChanged;

      protected virtual void OnConnectedChanged(FahClientConnectedChangedEventArgs e)
      {
         ConnectedChanged?.Invoke(this, e);
      }

      public string Host { get; }

      public int Port { get; }

      /// <summary>
      /// Gets or sets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
      /// </summary>
      /// <returns>The time (in milliseconds) to wait for a connection to open. The default value is 5000 milliseconds.</returns>
      public int ConnectionTimeout { get; set; } = 5000;

      public abstract bool Connected { get; }

      /// <summary>
      /// Initializes a new instance of the <see cref="FahClientConnectionBase"/> class.
      /// </summary>
      /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
      /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
      protected FahClientConnectionBase(string host, int port)
      {
         Host = host ?? throw new ArgumentNullException(nameof(host));
         if (!ValidationHelper.ValidateTcpPort(port)) throw new ArgumentOutOfRangeException(nameof(port));
         Port = port;
      }

      public abstract void Open();

      public abstract Task OpenAsync();

      public abstract void Close();
   }

   /// <summary>
   /// Folding@Home client connection class.
   /// </summary>
   public class FahClientConnection : FahClientConnectionBase, IDisposable
   {
      public override bool Connected => (_tcpConnection?.Connected).GetValueOrDefault();

      /// <summary>
      /// Initializes a new instance of the <see cref="FahClientConnection"/> class.
      /// </summary>
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
      /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
      /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
      public FahClientConnection(TcpConnectionFactory tcpConnectionFactory, string host, int port)
         : base(host, port)
      {
         _tcpConnectionFactory = tcpConnectionFactory;
      }

      private TcpConnection _tcpConnection;

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

      public override void Close()
      {
         bool connected = Connected;
         // close the connection
         _tcpConnection?.Close();
         // if connection state changed, raise event
         if (connected != Connected)
         {
            OnConnectedChanged(new FahClientConnectedChangedEventArgs(Connected));
         }
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

   /// <summary>
   /// Provides event data for connection status events of a <see cref="FahClientConnection"/>. This class cannot be inherited.
   /// </summary>
   public sealed class FahClientConnectedChangedEventArgs : EventArgs
   {
      public bool Connected { get; }

      internal FahClientConnectedChangedEventArgs(bool connected)
      {
         Connected = connected;
      }
   }
}
