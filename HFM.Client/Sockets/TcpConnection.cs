
using System.IO;
using System.Threading.Tasks;

namespace HFM.Client.Sockets
{
    /// <summary>
    /// Represents connections for TCP network services.
    /// </summary>
    public abstract class TcpConnection
    {
        /// <summary>
        /// Gets a value indicating whether the TCP connection is connected to a remote host.
        /// </summary>
        public abstract bool Connected { get; }

        /// <summary>
        /// Connects to a remote TCP host using the specified host and port number.
        /// </summary>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <param name="timeout">The time (in milliseconds) to wait while trying to establish a connection before terminating the attempt and generating an error.</param>
        public abstract void Connect(string host, int port, int timeout);

        /// <summary>
        /// Connects asynchronously to a remote TCP host using the specified host and port number.
        /// </summary>
        /// <param name="host">The name of the remote host.  The host can be an IP address or DNS name.</param>
        /// <param name="port">The port number of the remote host.</param>
        /// <param name="timeout">The time (in milliseconds) to wait while trying to establish a connection before terminating the attempt and generating an error.</param>
        public abstract Task ConnectAsync(string host, int port, int timeout);

        /// <summary>
        /// Requests that the TCP connection be closed.
        /// </summary>
        public abstract void Close();

        /// <summary>
        /// Returns the <see cref="Stream" /> used to send and receive data if connected; otherwise, null.
        /// </summary>
        public abstract Stream GetStream();
    }
}
