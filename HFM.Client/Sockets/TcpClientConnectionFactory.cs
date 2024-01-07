namespace HFM.Client.Sockets;

/// <summary>
/// Creates connections for TCP network services using a <see cref="TcpClientConnection"/>.
/// </summary>
public class TcpClientConnectionFactory : TcpConnectionFactory
{
    /// <summary>
    /// Creates a connection for TCP network services.
    /// </summary>
    public override TcpConnection Create()
    {
        var connection = new TcpClientConnection();
        connection.TcpClient.NoDelay = true;
        connection.TcpClient.SendBufferSize = 1024 * 8;
        connection.TcpClient.ReceiveBufferSize = 1024 * 8;
        return connection;
    }
}
