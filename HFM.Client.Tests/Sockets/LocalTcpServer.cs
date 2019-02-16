
using System;
using System.Net;
using System.Net.Sockets;

namespace HFM.Client.Sockets
{
   public class LocalTcpServer : IDisposable
   {
      public const string Host = "127.0.0.1";
      public const int Port = 13000;

      private readonly TcpListener _tcpListener;
      private TcpClient _client;

      public LocalTcpServer()
      {
         _tcpListener = new TcpListener(IPAddress.Parse(Host), Port);
      }

      public void Start()
      {
         _tcpListener.Start();
      }

      public async void Listen()
      {
            try
            {
               _client = await _tcpListener.AcceptTcpClientAsync();
               var stream = _client.GetStream();

               int lengthRead;
               var buffer = new byte[1024];

               while ((lengthRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
               {
                  BytesRead(buffer, lengthRead);
               }
            }
            catch (Exception e)
            {
               Console.WriteLine(e);
            }
      }

      protected virtual void BytesRead(byte[] bytes, int lengthRead)
      {

      }

      //public void SendBytes(byte[] bytes)
      //{
      //   _client.GetStream().Write(bytes, 0, bytes.Length);
      //}

      public void Stop()
      {
         _client?.Close();
         _tcpListener.Stop();
      }
      
      protected virtual void Dispose(bool disposing)
      {
         if (disposing)
         {
            Stop();
            ((IDisposable)_client)?.Dispose();
         }
      }

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }
   }
}
