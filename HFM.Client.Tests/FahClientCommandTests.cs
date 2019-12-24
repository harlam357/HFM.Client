
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

using HFM.Client.Mocks;
using HFM.Client.Sockets;

namespace HFM.Client
{
    [TestFixture]
    public class FahClientCommandTests
    {
        [Test]
        public void FahClientCommand_ExecuteThrowsInvalidOperationExceptionWhenConnectionIsNotConnected()
        {
            // Arrange
            var command = new FahClientCommand(new FahClientConnection("foo", 2000));
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => command.Execute());
        }

        [Test]
        public void FahClientCommand_ExecuteAsyncThrowsInvalidOperationExceptionWhenConnectionIsNotConnected()
        {
            // Arrange
            var command = new FahClientCommand(new FahClientConnection("foo", 2000));
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync());
        }

        [Test]
        public void FahClientCommand_ExecuteThrowsInvalidOperationExceptionWhenTcpConnectionIsNoLongerConnected()
        {
            // Arrange
            using (var connection = new MockFahClientConnection())
            {
                connection.Open();
                var command = new FahClientCommand(connection);
                // Act & Assert
                Assert.Throws<InvalidOperationException>(() => command.Execute());
            }
        }

        [Test]
        public void FahClientCommand_ExecuteAsyncThrowsInvalidOperationExceptionWhenTcpConnectionIsNoLongerConnected()
        {
            // Arrange
            using (var connection = new MockFahClientConnection())
            {
                connection.Open();
                var command = new FahClientCommand(connection);
                // Act & Assert
                Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync());
            }
        }

        private class MockFahClientConnection : FahClientConnection
        {
            private bool _connected;

            public override bool Connected => _connected;

            // simulate losing the TcpConnection after the FahClientConnection has been opened
            public override TcpConnection TcpConnection => _connected ? null : base.TcpConnection;

            public MockFahClientConnection()
               : base(new MockTcpConnectionFactory(), "foo", 2000)
            {

            }

            public override void Open()
            {
                base.Open();
                _connected = true;
            }
        }

        [Test]
        public void FahClientCommand_ExecuteRethrowsExceptionFromStreamWriteAndClosesTheConnection()
        {
            // Arrange
            Func<TcpConnection> factory = () => new MockTcpConnection(() => new MockStreamThrowsOnWrite());
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(factory), "foo", 2000))
            {
                connection.Open();
                var command = new FahClientCommand(connection);
                // Act & Assert
                Assert.Throws<IOException>(() => command.Execute());
                Assert.IsFalse(connection.Connected);
            }
        }

        [Test]
        public void FahClientCommand_ExecuteAsyncRethrowsExceptionFromStreamWriteAsyncAndClosesTheConnection()
        {
            // Arrange
            Func<TcpConnection> factory = () => new MockTcpConnection(() => new MockStreamThrowsOnWrite());
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(factory), "foo", 2000))
            {
                connection.Open();
                var command = new FahClientCommand(connection);
                // Act & Assert
                Assert.ThrowsAsync<IOException>(() => command.ExecuteAsync());
                Assert.IsFalse(connection.Connected);
            }
        }

        private class MockStreamThrowsOnWrite : MemoryStream
        {
            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new IOException("Simulated IO error");
            }
        }

        [Test]
        public void FahClientCommand_ExecuteWritesCommandTextToConnection()
        {
            // Arrange
            var tcpConnectionFactory = new MockTcpConnectionFactory();
            using (var connection = new FahClientConnection(tcpConnectionFactory, "foo", 2000))
            {
                connection.Open();
                var command = new FahClientCommand(connection, "command text");
                // Act
                int bytesWritten = command.Execute();
                // Assert
                Assert.AreEqual(13, bytesWritten);
                var memoryStream = (MemoryStream)tcpConnectionFactory.TcpConnection.GetStream();
                Assert.AreEqual("command text\n", Encoding.ASCII.GetString(memoryStream.ToArray()));
            }
        }

        [Test]
        public async Task FahClientCommand_ExecuteAsyncWritesCommandTextToConnection()
        {
            // Arrange
            var tcpConnectionFactory = new MockTcpConnectionFactory();
            using (var connection = new FahClientConnection(tcpConnectionFactory, "foo", 2000))
            {
                connection.Open();
                var command = new FahClientCommand(connection, "command text");
                // Act
                int bytesWritten = await command.ExecuteAsync();
                // Assert
                Assert.AreEqual(13, bytesWritten);
                var memoryStream = (MemoryStream)tcpConnectionFactory.TcpConnection.GetStream();
                Assert.AreEqual("command text\n", Encoding.ASCII.GetString(memoryStream.ToArray()));
            }
        }

        [Test]
        public void FahClientCommand_ExecuteWritesNullCommandTextToConnection()
        {
            // Arrange
            var tcpConnectionFactory = new MockTcpConnectionFactory();
            using (var connection = new FahClientConnection(tcpConnectionFactory, "foo", 2000))
            {
                connection.Open();
                var command = new FahClientCommand(connection, null);
                // Act
                int bytesWritten = command.Execute();
                // Assert
                Assert.AreEqual(0, bytesWritten);
                var memoryStream = (MemoryStream)tcpConnectionFactory.TcpConnection.GetStream();
                Assert.AreEqual("", Encoding.ASCII.GetString(memoryStream.ToArray()));
            }
        }

        [Test]
        public async Task FahClientCommand_ExecuteAsyncWritesNullCommandTextToConnection()
        {
            // Arrange
            var tcpConnectionFactory = new MockTcpConnectionFactory();
            using (var connection = new FahClientConnection(tcpConnectionFactory, "foo", 2000))
            {
                connection.Open();
                var command = new FahClientCommand(connection, null);
                // Act
                int bytesWritten = await command.ExecuteAsync();
                // Assert
                Assert.AreEqual(0, bytesWritten);
                var memoryStream = (MemoryStream)tcpConnectionFactory.TcpConnection.GetStream();
                Assert.AreEqual("", Encoding.ASCII.GetString(memoryStream.ToArray()));
            }
        }
    }
}
