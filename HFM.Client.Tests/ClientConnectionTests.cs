
using System;
using System.Linq;
using System.Net.Sockets;

using Moq;
using NUnit.Framework;

namespace HFM.Client
{
   public class ClientConnectionTests
   {
      //[Test]
      //public void ClientConnection_Connected_ReturnsClosedWhenConnectionHasNotBeenOpened_Test()
      //{
      //   // Arrange
      //   using (var connection = new ClientConnection("foo", 2000))
      //   {
      //      // Assert
      //      Assert.AreEqual(ClientConnectionState.Closed, connection.State);
      //   }
      //}

      //[Test]
      //public void ClientConnection_Connected_ReturnsClosedWhenInnerTcpClientConnectionIsNotConnected_Test()
      //{
      //   // Arrange
      //   var tcpClientMock = new Mock<ITcpClientConnection>();
      //   tcpClientMock.Setup(x => x.Client).Returns(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
      //   tcpClientMock.Setup(x => x.Connected).Returns(false);
      //   using (var connection = new ClientConnection(CreateClientFactory(tcpClientMock.Object), "foo", 2000))
      //   {
      //      // Assert
      //      Assert.AreEqual(ClientConnectionState.Closed, connection.State);
      //   }
      //}

      //[Test]
      //public void ClientConnection_Connected_ReturnsOpenWhenConnectionHasBeenOpened_Test()
      //{
      //   // Arrange
      //   var tcpClientMock = new Mock<ITcpClientConnection>();
      //   tcpClientMock.Setup(x => x.Client).Returns(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
      //   tcpClientMock.Setup(x => x.Connected).Returns(true);
      //   using (var connection = new ClientConnection(CreateClientFactory(tcpClientMock.Object), "foo", 2000))
      //   {
      //      // Act
      //      connection.Open();
      //      // Assert
      //      Assert.AreEqual(ClientConnectionState.Open, connection.State);
      //   }
      //}

      //[Test]
      //public void ClientConnection_Open_SuccessfulConnectionRaisesStateChangedEvent_Test()
      //{
      //   // Arrange
      //   ClientConnectionState stateChangedRaised = default(ClientConnectionState);
      //   var tcpClientMock = new Mock<ITcpClientConnection>();
      //   tcpClientMock.Setup(x => x.Client).Returns(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
      //   tcpClientMock.Setup(x => x.Connected).Returns(true);
      //   using (var connection = new ClientConnection(CreateClientFactory(tcpClientMock.Object), "foo", 2000))
      //   {
      //      connection.StateChanged += (sender, args) => stateChangedRaised = args.State;
      //      // Act
      //      connection.Open();
      //   }
      //   // Assert
      //   Assert.AreEqual(ClientConnectionState.Open, stateChangedRaised);
      //}

      //[Test]
      //public void ClientConnection_Open_AndCloseMultipleTimes_Test()
      //{
      //   // Arrange
      //   var tcpClientMock = new Mock<ITcpClientConnection>();
      //   tcpClientMock.Setup(x => x.Client).Returns(new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp));
      //   tcpClientMock.Setup(x => x.Connected).Returns(true);
      //   using (var connection = new ClientConnection(CreateClientFactory(tcpClientMock.Object), "foo", 2000))
      //   {
      //      connection.Open();
      //      // Act
      //      foreach (var _ in Enumerable.Range(0, 3))
      //      {
      //         connection.Close();
      //      }
      //      Assert.AreEqual(ClientConnectionState.Closed, connection.State);
      //   }
      //}

      //[Test]
      //public void ClientConnection_Open_ThrowsExceptionOnTimeout_Test()
      //{
      //   using (var connection = new ClientConnection("foo", 2000))
      //   {
      //      connection.ConnectionTimeout = 1000;
      //      // Act & Assert
      //      Assert.Throws<TimeoutException>(() => connection.Open());
      //   }
      //}

      ////private static ITcpClientFactory CreateClientFactory()
      ////{
      ////   return CreateClientFactory(Mock.Of<ITcpClient>());
      ////}

      //private static ITcpClientFactory CreateClientFactory(ITcpClientConnection tcpClient)
      //{
      //   var tcpClientFactory = new Mock<ITcpClientFactory>();
      //   tcpClientFactory.Setup(x => x.Create()).Returns(tcpClient);
      //   return tcpClientFactory.Object;
      //}
   }
}
