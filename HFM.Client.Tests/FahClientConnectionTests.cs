
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using NUnit.Framework;

using HFM.Client.Sockets;

namespace HFM.Client
{
   public class FahClientConnectionTests
   {
      private int _shortTimeout = 250;

      [Test]
      public void FahClientConnection_ThrowsArgumentNullExceptionWhenHostIsNull()
      {
         // ReSharper disable once ObjectCreationAsStatement
         Assert.Throws<ArgumentNullException>(() => new FahClientConnection(null, 2000));
      }

      [Test]
      public void TcpClientConnection_ConnectThrowsArgumentOutOfRangeExceptionWhenPortNumberIsNotValid()
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
         // use a local IP that no physical machine is using
         var host = "172.20.0.1";
         using (var connection = new FahClientConnection(host, LocalTcpListener.Port))
         {
            connection.ConnectionTimeout = _shortTimeout;
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
         // use a local IP that no physical machine is using
         var host = "172.20.0.1";
         using (var connection = new FahClientConnection(host, LocalTcpListener.Port))
         {
            connection.ConnectionTimeout = _shortTimeout;
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
      public void FahClientConnection_GetDataWriterThrowsInvalidOperationExceptionWhenConnectionIsNotOpen()
      {
         // Arrange
         using (var connection = new FahClientConnection("foo", 2000))
         {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => connection.GetDataWriter());
         }
      }

      [Test]
      public void FahClientConnection_GetDataWriterWhenConnectionIsOpen()
      {
         // Arrange
         using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
         {
            connection.Open();
            // Act
            var dataWriter = connection.GetDataWriter();
            // Assert
            Assert.IsNotNull(dataWriter);
         }
      }

      [Test]
      public void FahClientConnection_GetDataWriterReturnsSameInstanceWhileConnectionRemainsOpen()
      {
         // Arrange
         using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
         {
            connection.Open();
            // Act
            var dataWriter1 = connection.GetDataWriter();
            var dataWriter2 = connection.GetDataWriter();
            // Assert
            Assert.AreSame(dataWriter1, dataWriter2);
         }
      }

      [Test]
      public void FahClientConnection_GetDataWriterReturnsDifferentInstanceEachTimeConnectionIsOpened()
      {
         // Arrange
         using (var connection = new FahClientConnection(new MockTcpConnectionFactory(), "foo", 2000))
         {
            connection.Open();
            // Act
            var dataWriter1 = connection.GetDataWriter();
            // close and open again
            connection.Close();
            connection.Open();
            // Act
            var dataWriter2 = connection.GetDataWriter();
            // Assert
            Assert.AreNotSame(dataWriter1, dataWriter2);
         }
      }

      [Test]
      public void FahClientConnection_DisposeClosesInnerTcpConnection()
      {
         // Arrange
         var tcpConnectionFactory = new MockTcpConnectionFactory();
         var tcpConnection = tcpConnectionFactory.TcpConnection;
         using (var connection = new FahClientConnection(tcpConnectionFactory, "foo", 2000))
         {
            // Act (Open)
            connection.Open();
            // Assert
            Assert.IsTrue(tcpConnection.Connected);
         }
         // Assert
         Assert.IsFalse(tcpConnection.Connected);
      }

      private class MockTcpConnectionFactory : TcpConnectionFactory
      {
         public TcpConnection TcpConnection { get; }

         public MockTcpConnectionFactory()
         {
            TcpConnection = new MockTcpConnection();
         }

         public override TcpConnection Create()
         {
            return TcpConnection;
         }
      }

      private class MockTcpConnection : TcpConnection
      {
         private bool _connected;

         public override bool Connected => _connected;

         public override void Connect(string host, int port, int timeout)
         {
            _connected = true;
         }

         public override Task ConnectAsync(string host, int port, int timeout)
         {
            _connected = true;
            return Task.FromResult(0);
         }

         public override void Close()
         {
            _connected = false;
         }

         public override Stream GetStream()
         {
            return new MemoryStream();
         }
      }
   }
}
