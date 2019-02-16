
namespace HFM.Client.Sockets
{
   public abstract class TcpConnectionFactory
   {
      public abstract TcpConnection Create();

      public static TcpConnectionFactory Default { get; } = new TcpClientConnectionFactory();

      private class TcpClientConnectionFactory : TcpConnectionFactory
      {
         public override TcpConnection Create()
         {
            var connection = new TcpClientConnection();
            connection.TcpClient.NoDelay = true;
            connection.TcpClient.SendBufferSize = 1024 * 8;
            connection.TcpClient.ReceiveBufferSize = 1024 * 8;
            return connection;
         }
      }
   }
}
