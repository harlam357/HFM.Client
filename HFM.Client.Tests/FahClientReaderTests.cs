
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
    public class FahClientReaderTests
    {
        [Test]
        public void FahClientReader_ReadThrowsInvalidOperationExceptionWhenConnectionIsNotConnected()
        {
            // Arrange
            var reader = new FahClientReader(new FahClientConnection("foo", 2000));
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => reader.Read());
        }

        [Test]
        public void FahClientReader_ReadAsyncThrowsInvalidOperationExceptionWhenConnectionIsNotConnected()
        {
            // Arrange
            var reader = new FahClientReader(new FahClientConnection("foo", 2000));
            // Act & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => reader.ReadAsync());
        }

        [Test]
        public void FahClientReader_ReadThrowsInvalidOperationExceptionWhenTcpConnectionIsNoLongerConnected()
        {
            // Arrange
            using (var connection = new MockFahClientConnection())
            {
                connection.Open();
                var reader = new FahClientReader(connection);
                // Act & Assert
                Assert.Throws<InvalidOperationException>(() => reader.Read());
            }
        }

        [Test]
        public void FahClientReader_ReadAsyncThrowsInvalidOperationExceptionWhenTcpConnectionIsNoLongerConnected()
        {
            // Arrange
            using (var connection = new MockFahClientConnection())
            {
                connection.Open();
                var reader = new FahClientReader(connection);
                // Act & Assert
                Assert.ThrowsAsync<InvalidOperationException>(() => reader.ReadAsync());
            }
        }

        private class MockFahClientConnection : FahClientConnection
        {
            private bool _connected;

            public override bool Connected => _connected;

            // simulate losing the TcpConnection after the FahClientConnection has been opened
            public override TcpConnection TcpConnection => _connected ? null : base.TcpConnection;

            public MockFahClientConnection()
               : base("foo", 2000, new MockTcpConnectionFactory())
            {

            }

            public override void Open()
            {
                base.Open();
                _connected = true;
            }
        }

        [Test]
        public void FahClientReader_ReadRethrowsExceptionFromStreamReadAndClosesTheConnection()
        {
            // Arrange
            Func<TcpConnection> factory = () => new MockTcpConnection(() => new MockStreamThrowsOnRead());
            using (var connection = new FahClientConnection("foo", 2000, new MockTcpConnectionFactory(factory)))
            {
                connection.Open();
                var reader = new FahClientReader(connection);
                // Act & Assert
                Assert.Throws<IOException>(() => reader.Read());
                Assert.IsFalse(connection.Connected);
            }
        }

        [Test]
        public void FahClientReader_ReadAsyncRethrowsExceptionFromStreamReadAsyncAndClosesTheConnection()
        {
            // Arrange
            Func<TcpConnection> factory = () => new MockTcpConnection(() => new MockStreamThrowsOnRead());
            using (var connection = new FahClientConnection("foo", 2000, new MockTcpConnectionFactory(factory)))
            {
                connection.Open();
                var reader = new FahClientReader(connection);
                // Act & Assert
                Assert.ThrowsAsync<IOException>(() => reader.ReadAsync());
                Assert.IsFalse(connection.Connected);
            }
        }

        private class MockStreamThrowsOnRead : MemoryStream
        {
            public override int Read(byte[] buffer, int offset, int count)
            {
                throw new IOException("Simulated IO error");
            }
        }

        [Test]
        public void FahClientReader_ReadReadsMessageFromConnection()
        {
            // Arrange
            using (var stream = CreateStreamWithMessage())
            {
                Func<TcpConnection> factory = () => new MockTcpConnection(() => stream);
                using (var connection = new FahClientConnection("foo", 2000, new MockTcpConnectionFactory(factory)))
                {
                    connection.Open();
                    var reader = new FahClientReader(connection);
                    reader.BufferSize = 8;
                    // Act
                    bool result = reader.Read();
                    // Assert
                    Assert.IsTrue(result);
                    var message = reader.Message;
                    Assert.AreEqual(FahClientMessageType.Info, message.Identifier.MessageType);
                    Assert.AreEqual(MessageFromReader, message.MessageText.ToString());
                }
            }
        }

        [Test]
        public async Task FahClientReader_ReadAsyncReadsMessageFromConnection()
        {
            // Arrange
            using (var stream = CreateStreamWithMessage())
            {
                Func<TcpConnection> factory = () => new MockTcpConnection(() => stream);
                using (var connection = new FahClientConnection("foo", 2000, new MockTcpConnectionFactory(factory)))
                {
                    connection.Open();
                    var reader = new FahClientReader(connection);
                    reader.BufferSize = 8;
                    // Act
                    bool result = await reader.ReadAsync();
                    // Assert
                    Assert.IsTrue(result);
                    var message = reader.Message;
                    Assert.AreEqual(FahClientMessageType.Info, message.Identifier.MessageType);
                    Assert.AreEqual(MessageFromReader, message.MessageText.ToString());
                }
            }
        }

        private static Stream CreateStreamWithMessage()
        {
            var stream = new MemoryStream();
            var buffer = Encoding.ASCII.GetBytes(MessageFromStream);
            stream.Write(buffer, 0, buffer.Length);
            stream.Position = 0;
            return stream;
        }

        private const string MessageFromStream = "PyON 1 info\r\n" +
                                                 "[\r\n" +
                                                 "  [\r\n" +
                                                 "    \"Folding@home Client\",\r\n" +
                                                 "  ]\r\n" +
                                                 "]\r\n" +
                                                 "---\r\n";

        private const string MessageFromReader = "[\r\n" +
                                                 "  [\r\n" +
                                                 "    \"Folding@home Client\",\r\n" +
                                                 "  ]\r\n" +
                                                 "]";

        [Test]
        public void FahClientReader_ReadReadsNoMessageFromConnection()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                Func<TcpConnection> factory = () => new MockTcpConnection(() => stream);
                using (var connection = new FahClientConnection("foo", 2000, new MockTcpConnectionFactory(factory)))
                {
                    connection.Open();
                    var reader = new FahClientReader(connection);
                    // Act
                    bool result = reader.Read();
                    // Assert
                    Assert.IsFalse(result);
                    Assert.IsNull(reader.Message);
                }
            }
        }

        [Test]
        public async Task FahClientReader_ReadAsyncReadsNoMessageFromConnection()
        {
            // Arrange
            using (var stream = new MemoryStream())
            {
                Func<TcpConnection> factory = () => new MockTcpConnection(() => stream);
                using (var connection = new FahClientConnection("foo", 2000, new MockTcpConnectionFactory(factory)))
                {
                    connection.Open();
                    var reader = new FahClientReader(connection);
                    // Act
                    bool result = await reader.ReadAsync();
                    // Assert
                    Assert.IsFalse(result);
                    Assert.IsNull(reader.Message);
                }
            }
        }
    }
}
