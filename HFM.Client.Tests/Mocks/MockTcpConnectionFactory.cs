using HFM.Client.Sockets;

namespace HFM.Client.Mocks;

internal class MockTcpConnectionFactory : TcpConnectionFactory
{
    private readonly Func<TcpConnection> _factory;

    public MockTcpConnectionFactory()
        : this(() => new MockTcpConnection())
    {

    }

    public MockTcpConnectionFactory(Func<TcpConnection> factory)
    {
        _factory = factory;
    }

    public TcpConnection TcpConnection { get; private set; }

    public override TcpConnection Create()
    {
        return TcpConnection = _factory();
    }
}
