
namespace HFM.Client.Sockets
{
    /// <summary>
    /// Creates connections for TCP network services.
    /// </summary>
    public abstract class TcpConnectionFactory
    {
        /// <summary>
        /// Creates a connection for TCP network services.
        /// </summary>
        public abstract TcpConnection Create();

        /// <summary>
        /// Gets the default <see cref="TcpConnectionFactory"/> that creates <see cref="TcpClientConnection"/> connections.
        /// </summary>
        public static TcpConnectionFactory Default { get; } = new TcpClientConnectionFactory();

        private class TcpClientConnectionFactory : TcpConnectionFactory
        {
            public override TcpConnection Create()
            {
                var connection = new TcpClientConnection();
                connection.TcpClient.NoDelay = true;
                connection.TcpClient.SendBufferSize = 1024 * 8;
                connection.TcpClient.ReceiveBufferSize = 1024 * 8;
                return connection;
            }
        }
    }
}
