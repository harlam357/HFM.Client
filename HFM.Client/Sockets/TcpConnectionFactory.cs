namespace HFM.Client.Sockets;

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
}
