
using System;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

using HFM.Client.Mocks;
using HFM.Client.Sockets;

namespace HFM.Client
{
    public class FahClientConnectionTests
    {
        private const int ShortTimeout = 250;

        [Test]
        public void FahClientConnection_ThrowsArgumentNullExceptionWhenHostIsNull()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentNullException>(() => new FahClientConnection(null, 2000));
        }

        [Test]
        public void FahClientConnection_ThrowsArgumentOutOfRangeExceptionWhenPortNumberIsNotValid()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Assert.Throws<ArgumentOutOfRangeException>(() => new FahClientConnection("foo", -1));
        }

        [Test]
        public void FahClientConnection_VerifyPropertiesOnNewInstance()
        {
            // Act
            using (var connection = new FahClientConnection("foo", 2000))
            {
                // Assert
                Assert.AreEqual("foo", connection.Host);
                Assert.AreEqual(2000, connection.Port);
                Assert.AreEqual(5000, connection.ConnectionTimeout);
                Assert.IsFalse(connection.Connected);
            }
        }

        [Test]
        public void FahClientConnection_CanSetConnectionTimeout()
        {
            // Arrange
            using (var connection = new FahClientConnection("foo", 2000))
            {
                // Act
                connection.ConnectionTimeout = 10000;
                // Assert
                Assert.AreEqual(10000, connection.ConnectionTimeout);
            }
        }

        [Test]
        public void FahClientConnection_OpenSuccessfullyAndClose()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                bool connectedChanged = false;
                connection.ConnectedChanged += (s, e) => connectedChanged = e.Connected;
                // Act (Open)
                connection.Open();
                // Assert
                Assert.IsTrue(connection.Connected);
                Assert.IsTrue(connectedChanged);
                // Act (Close)
                connection.Close();
                // Assert
                Assert.IsFalse(connection.Connected);
                Assert.IsFalse(connectedChanged);
            }
        }

        [Test]
        public async Task FahClientConnection_OpenAsyncSuccessfullyAndClose()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                bool connectedChanged = false;
                connection.ConnectedChanged += (s, e) => connectedChanged = e.Connected;
                // Act (Open)
                await connection.OpenAsync();
                // Assert
                Assert.IsTrue(connection.Connected);
                Assert.IsTrue(connectedChanged);
                // Act (Close)
                connection.Close();
                // Assert
                Assert.IsFalse(connection.Connected);
                Assert.IsFalse(connectedChanged);
            }
        }

        [Test]
        public void FahClientConnection_OpenSuccessfullyAndCloseMultipleTimes()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                int connectedChangedCount = 0;
                connection.ConnectedChanged += (s, e) => connectedChangedCount++;
                // Act (Open)
                connection.Open();
                // Act (Close)
                foreach (var _ in Enumerable.Range(0, 3))
                {
                    connection.Close();
                }
                // Assert - ConnectedChanged raised twice, on Open() and first Close()
                Assert.AreEqual(2, connectedChangedCount);
            }
        }

        [Test]
        public void FahClientConnection_OpenThrowsInvalidOperationExceptionWhenConnectionIsAlreadyConnected()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                // Act (Open)
                connection.Open();
                // Act (Attempt Another Connection) & Assert
                Assert.Throws<InvalidOperationException>(() => connection.Open());
            }
        }

        [Test]
        public async Task FahClientConnection_OpenAsyncThrowsInvalidOperationExceptionWhenConnectionIsAlreadyConnected()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                // Act (Open)
                await connection.OpenAsync();
                // Act (Attempt Another Connection) & Assert
                // ReSharper disable once AccessToDisposedClosure
                Assert.ThrowsAsync<InvalidOperationException>(() => connection.OpenAsync());
            }
        }

        [Test]
        [Category(TestCategoryNames.Integration)]
        public void FahClientConnection_OpenAttemptTimesOut()
        {
            // Arrange
            using (var connection = new FahClientConnection(LocalTcpListener.Host, LocalTcpListener.Port))
            {
                connection.ConnectionTimeout = ShortTimeout;
                // Act & Assert
                Assert.Throws<TimeoutException>(() => connection.Open());
                Assert.IsFalse(connection.Connected);
            }
        }

        [Test]
        [Category(TestCategoryNames.Integration)]
        public void FahClientConnection_OpenAsyncAttemptTimesOut()
        {
            // Arrange
            using (var connection = new FahClientConnection(LocalTcpListener.Host, LocalTcpListener.Port))
            {
                connection.ConnectionTimeout = ShortTimeout;
                // Act & Assert
                // ReSharper disable once AccessToDisposedClosure
                Assert.ThrowsAsync<TimeoutException>(() => connection.OpenAsync());
                Assert.IsFalse(connection.Connected);
            }
        }

        [Test]
        public void FahClientConnection_ReopenConnectionThatWasPreviouslyOpenedAndClosed()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                // Act (Open)
                connection.Open();
                // Assert
                Assert.IsTrue(connection.Connected);
                // Act (Close)
                connection.Close();
                // Assert
                Assert.IsFalse(connection.Connected);
                // Act (Open Again)
                connection.Open();
                // Assert
                Assert.IsTrue(connection.Connected);
            }
        }

        [Test]
        public async Task FahClientConnection_ReopenAsyncConnectionThatWasPreviouslyOpenedAndClosed()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                // Act (Open)
                await connection.OpenAsync();
                // Assert
                Assert.IsTrue(connection.Connected);
                // Act (Close)
                connection.Close();
                // Assert
                Assert.IsFalse(connection.Connected);
                // Act (Open Again)
                await connection.OpenAsync();
                // Assert
                Assert.IsTrue(connection.Connected);
            }
        }

        [Test]
        public void FahClientConnection_CreateCommandReturnsCommandWithCommandText()
        {
            // Arrange
            using (var connection = new FahClientConnection("foo", 2000))
            {
                // Act
                var command = connection.CreateCommand("bar");
                // Assert
                Assert.IsNotNull(command);
                Assert.AreEqual("bar", command.CommandText);
            }
        }

        [Test]
        public void FahClientConnection_CreateCommandReturnsCommandWhenNotConnected()
        {
            // Arrange
            using (var connection = new FahClientConnection("foo", 2000))
            {
                // Act
                var command = connection.CreateCommand();
                // Assert
                Assert.IsNotNull(command);
            }
        }

        [Test]
        public void FahClientConnectionBase_CreateCommandReturnsCommandWhenNotConnected()
        {
            // Arrange
            using (var connection = new FahClientConnection("foo", 2000))
            {
                FahClientConnectionBase connectionBase = connection;
                // Act
                var command = connectionBase.CreateCommand();
                // Assert
                Assert.IsNotNull(command);
            }
        }

        [Test]
        public void FahClientConnection_CreateReaderReturnsReaderWhenNotConnected()
        {
            // Arrange
            using (var connection = new FahClientConnection("foo", 2000))
            {
                // Act
                var reader = connection.CreateReader();
                // Assert
                Assert.IsNotNull(reader);
            }
        }

        [Test]
        public void FahClientConnectionBase_CreateReaderReturnsReaderWhenNotConnected()
        {
            // Arrange
            using (var connection = new FahClientConnection("foo", 2000))
            {
                FahClientConnectionBase connectionBase = connection;
                // Act
                var reader = connectionBase.CreateReader();
                // Assert
                Assert.IsNotNull(reader);
            }
        }

        [Test]
        public void FahClientConnection_TcpConnectionReturnsNullWhenConnectionIsNotOpen()
        {
            // Arrange
            using (var connection = new FahClientConnection("foo", 2000))
            {
                // Act & Assert
                Assert.IsNull(connection.TcpConnection);
            }
        }

        [Test]
        public void FahClientConnection_TcpConnectionReturnsInstanceWhenConnectionIsOpen()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                connection.Open();
                // Act & Assert
                Assert.IsNotNull(connection.TcpConnection);
            }
        }

        [Test]
        public void FahClientConnection_TcpConnectionReturnsNullAfterConnectionIsOpenedAndClosed()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                connection.Open();
                Assert.IsNotNull(connection.TcpConnection);
                connection.Close();
                // Act & Assert
                Assert.IsNull(connection.TcpConnection);
            }
        }

        [Test]
        public void FahClientConnection_TcpConnectionReturnsSameInstanceWhileConnectionRemainsOpen()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                connection.Open();
                // Act
                var tcpConnection1 = connection.TcpConnection;
                var tcpConnection2 = connection.TcpConnection;
                // Assert
                Assert.AreSame(tcpConnection1, tcpConnection2);
            }
        }

        [Test]
        public void FahClientConnection_TcpConnectionReturnsDifferentInstanceEachTimeConnectionIsOpened()
        {
            // Arrange
            using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
            {
                connection.Open();
                // Act
                var tcpConnection1 = connection.TcpConnection;
                // close and open again
                connection.Close();
                connection.Open();
                // Act
                var tcpConnection2 = connection.TcpConnection;
                // Assert
                Assert.AreNotSame(tcpConnection1, tcpConnection2);
            }
        }

        [Test]
        public void FahClientConnection_DisposeClosesInnerTcpConnection()
        {
            // Arrange
            var tcpConnectionFactory = new MockTcpConnectionFactory();
            using (var connection = new FahClientConnection(tcpConnectionFactory, "foo", 2000))
            {
                // Act (Open)
                connection.Open();
                // Assert
                Assert.IsTrue(tcpConnectionFactory.TcpConnection.Connected);
            }
            // Assert
            Assert.IsFalse(tcpConnectionFactory.TcpConnection.Connected);
        }
    }
}
