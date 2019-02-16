
using System;
using System.Net;
using System.Net.Sockets;

namespace HFM.Client.Sockets
{
   /// <summary>
   /// Simple TCP listener bound to localhost.
   /// </summary>
   internal class LocalTcpListener : IDisposable
   {
      public const string Host = "127.0.0.1";
      public const int Port = 13000;

      private readonly TcpListener _tcpListener;

      public LocalTcpListener()
      {
         _tcpListener = new TcpListener(IPAddress.Parse(Host), Port);
      }

      public void Start()
      {
         _tcpListener.Start();
      }

      public void Stop()
      {
         _tcpListener.Stop();
      }
      
      protected virtual void Dispose(bool disposing)
      {
         if (disposing)
         {
            Stop();
         }
      }

      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }
   }
}
