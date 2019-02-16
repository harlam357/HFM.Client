
using System;
using System.Net.Sockets;
using System.Threading.Tasks;

using NUnit.Framework;

namespace HFM.Client.Sockets
{
   [TestFixture]
   public class TcpClientConnectionTests
   {
      [Test]
      public void TcpClientConnection_ConnectedReturnsFalseOnNewInstance()
      {
         // Arrange
         var connection = new TcpClientConnection();
         // Act & Assert
         Assert.IsFalse(connection.Connected);
      }

      [Test]
      public void TcpClientConnection_GetStreamReturnsNullOnNewInstance()
      {
         // Arrange
         var connection = new TcpClientConnection();
         // Act & Assert
         Assert.IsNull(connection.GetStream());
      }

      [Test]
      public void TcpClientConnection_ConnectThrowsArgumentNullExceptionWhenHostIsNull()
      {
         // Arrange
         var connection = new TcpClientConnection();
         // Act & Assert
         Assert.Throws<ArgumentNullException>(() => connection.Connect(null, LocalTcpListener.Port, 5000));
      }

      [Test]
      public void TcpClientConnection_ConnectAsyncThrowsArgumentNullExceptionWhenHostIsNull()
      {
         // Arrange
         var connection = new TcpClientConnection();
         // Act & Assert
         Assert.ThrowsAsync<ArgumentNullException>(() => connection.ConnectAsync(null, LocalTcpListener.Port, 5000));
      }

      [Test]
      public void TcpClientConnection_ConnectThrowsArgumentOutOfRangeExceptionWhenPortNumberIsNotValid()
      {
         // Arrange
         var connection = new TcpClientConnection();
         // Act & Assert
         Assert.Throws<ArgumentOutOfRangeException>(() => connection.Connect(LocalTcpListener.Host, -1, 5000));
      }

      [Test]
      public void TcpClientConnection_ConnectAsyncThrowsArgumentOutOfRangeExceptionWhenPortNumberIsNotValid()
      {
         // Arrange
         var connection = new TcpClientConnection();
         // Act & Assert
         Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => connection.ConnectAsync(LocalTcpListener.Host, -1, 5000));
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void TcpClientConnection_ConnectSuccessfullyAndClose()
      {
         // Arrange
         using (new LocalTcpListener())
         {
            var connection = new TcpClientConnection();
            // Act (Connect)
            connection.Connect(LocalTcpListener.Host, LocalTcpListener.Port, 5000);
            // Assert
            Assert.IsTrue(connection.Connected);
            var stream = connection.GetStream();
            Assert.IsNotNull(stream);
            Assert.IsInstanceOf<NetworkStream>(stream);
            // Act (Close)
            connection.Close();
            // Assert
            Assert.IsFalse(connection.Connected);
            Assert.IsNull(connection.GetStream());
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void TcpClientConnection_ConnectThrowsInvalidOperationExceptionWhenConnectionIsAlreadyConnected()
      {
         // Arrange
         using (new LocalTcpListener())
         {
            var connection = new TcpClientConnection();
            // Act (Connect)
            connection.Connect(LocalTcpListener.Host, LocalTcpListener.Port, 5000);
            // Act (Attempt Another Connection) & Assert
            Assert.Throws<InvalidOperationException>(() => connection.Connect(LocalTcpListener.Host, LocalTcpListener.Port, 5000));
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void TcpClientConnection_ConnectAttemptTimesOut()
      {
         // Arrange
         var connection = new TcpClientConnection();
         // use a local IP that no physical machine is using
         var host = "172.20.0.1";
         // Act & Assert
         Assert.Throws<TimeoutException>(() => connection.Connect(host, LocalTcpListener.Port, 2000));
         Assert.IsFalse(connection.Connected);
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void TcpClientConnection_ConnectThrowsObjectDisposedExceptionWhenAttemptingToConnectUsingConnectionThatWasClosed()
      {
         // Arrange
         using (new LocalTcpListener())
         {
            var connection = new TcpClientConnection();
            connection.Close();
            // Act & Assert
            Assert.Throws<ObjectDisposedException>(() => connection.Connect(LocalTcpListener.Host, LocalTcpListener.Port, 5000));
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public async Task TcpClientConnection_ConnectAsyncSuccessfullyAndClose()
      {
         // Arrange
         using (new LocalTcpListener())
         {
            var connection = new TcpClientConnection();
            // Act (Connect)
            await connection.ConnectAsync(LocalTcpListener.Host, LocalTcpListener.Port, 5000);
            // Assert
            Assert.IsTrue(connection.Connected);
            var stream = connection.GetStream();
            Assert.IsNotNull(stream);
            Assert.IsInstanceOf<NetworkStream>(stream);
            // Act (Close)
            connection.Close();
            // Assert
            Assert.IsFalse(connection.Connected);
            Assert.IsNull(connection.GetStream());
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public async Task TcpClientConnection_ConnectAsyncThrowsInvalidOperationExceptionWhenConnectionIsAlreadyConnected()
      {
         // Arrange
         using (new LocalTcpListener())
         {
            var connection = new TcpClientConnection();
            // Act (Connect)
            await connection.ConnectAsync(LocalTcpListener.Host, LocalTcpListener.Port, 5000);
            // Act (Attempt Another Connection) & Assert
            Assert.ThrowsAsync<InvalidOperationException>(() => connection.ConnectAsync(LocalTcpListener.Host, LocalTcpListener.Port, 5000));
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void TcpClientConnection_ConnectAsyncAttemptTimesOut()
      {
         // Arrange
         var connection = new TcpClientConnection();
         // use a local IP that no physical machine is using
         var host = "172.20.0.1";
         // Act & Assert
         Assert.ThrowsAsync<TimeoutException>(() => connection.ConnectAsync(host, LocalTcpListener.Port, 2000));
         Assert.IsFalse(connection.Connected);
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void TcpClientConnection_ConnectAsyncThrowsObjectDisposedExceptionWhenAttemptingToConnectUsingConnectionThatWasClosed()
      {
         // Arrange
         using (new LocalTcpListener())
         {
            var connection = new TcpClientConnection();
            connection.Close();
            // Act & Assert
            Assert.ThrowsAsync<ObjectDisposedException>(() => connection.ConnectAsync(LocalTcpListener.Host, LocalTcpListener.Port, 5000));
         }
      }
   }
}
