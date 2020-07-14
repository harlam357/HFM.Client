
using System;
using System.IO;
using System.Threading.Tasks;

using HFM.Client.Sockets;

namespace HFM.Client.Mocks
{
    internal class MockTcpConnection : TcpConnection
    {
        private readonly Func<Stream> _streamFactory;

        public MockTcpConnection()
           : this(() => new MemoryStream())
        {

        }

        public MockTcpConnection(Func<Stream> streamFactory)
        {
            _streamFactory = streamFactory;
        }

        private bool _connected;
        private Stream _stream;

        public override bool Connected => _connected;

        public override void Connect(string host, int port, int timeout)
        {
            _connected = true;
            _stream = _streamFactory();
        }

        public override Task ConnectAsync(string host, int port, int timeout)
        {
            _connected = true;
            _stream = _streamFactory();
            return Task.FromResult(0);
        }

        public override void Close()
        {
            _connected = false;
            _stream?.Dispose();
            _stream = null;
        }

        public override Stream GetStream()
        {
            return _stream;
        }
    }
}