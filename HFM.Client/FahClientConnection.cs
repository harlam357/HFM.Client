
using System;
using System.Threading.Tasks;

using HFM.Client.Internal;

namespace HFM.Client
{
    /// <summary>
    /// Provides the abstract base class for a Folding@Home client connection.
    /// </summary>
    public abstract class FahClientConnection
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
        /// Initializes a new instance of the <see cref="FahClientConnection"/> class.
        /// </summary>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <exception cref="ArgumentNullException">The <paramref name="host" /> parameter is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">The <paramref name="port" /> parameter is not between zero and <see cref="Int16.MaxValue"/>.</exception>
        protected FahClientConnection(string host, int port)
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
        /// Creates and returns a <see cref="FahClientCommand"/> object associated with the current connection.
        /// </summary>
        public FahClientCommand CreateCommand()
        {
            return OnCreateCommand();
        }

        /// <summary>
        /// Creates and returns a <see cref="FahClientCommand"/> object associated with the current connection.
        /// </summary>
        protected abstract FahClientCommand OnCreateCommand();

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
        protected abstract FahClientReader OnCreateReader();
    }
}