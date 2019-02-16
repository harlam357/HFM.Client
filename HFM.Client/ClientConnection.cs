/*
 * HFM.NET - Client ClientConnection Class
 * Copyright (C) 2009-2016 Ryan Harlamert (harlam357)
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; version 2
 * of the License. See the included file GPLv2.TXT.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Diagnostics;
using System.IO;

using HFM.Client.Sockets;

namespace HFM.Client
{
   public enum ClientConnectionState
   {
      /// <summary>
      /// The connection is closed.
      /// </summary>
      Closed = 0,
      /// <summary>
      /// The connection is open.
      /// </summary>
      Open = 1
      ///// <summary>
      ///// The connection object is connecting to the client.
      ///// </summary>
      //Connecting = 2
   }

   public abstract class ClientConnectionBase
   {
      public abstract ClientConnectionState State { get; }

      /// <summary>
      /// Gets or sets the time to wait while trying to establish a connection before terminating the attempt and generating an error.
      /// </summary>
      /// <returns>The time (in milliseconds) to wait for a connection to open. The default value is 5000 milliseconds.</returns>
      public virtual int ConnectionTimeout { get; set; } = 5000;
   }

   /// <summary>
   /// Folding@Home client connection class.
   /// </summary>
   public class ClientConnection : ClientConnectionBase, IDisposable
   {
      /// <summary>
      /// Occurs when the value of the State property has changed.
      /// </summary>
      public event EventHandler<ClientConnectionStateChangedEventArgs> StateChanged;
      /// <summary>
      /// Occurs when data is sent by the ClientConnection.
      /// </summary>
      public event EventHandler<DataEventArgs> DataSent;
      /// <summary>
      /// Occurs when data is received by the ClientConnection.
      /// </summary>
      public event EventHandler<DataEventArgs> DataReceived;

      /// <summary>
      /// Gets a value indicating the state of the connection.
      ///  </summary>
      public override ClientConnectionState State => _tcpConnection.Connected ? ClientConnectionState.Open : ClientConnectionState.Closed;

      public string Host { get; }

      public int Port { get; }

      /// <summary>
      /// Initializes a new instance of the ClientConnection class.
      /// </summary>
      public ClientConnection(string host, int port)
         : this(TcpConnectionFactory.Default, host, port)
      {

      }

      private readonly TcpConnectionFactory _tcpConnectionFactory;

      public ClientConnection(TcpConnectionFactory tcpConnectionFactory, string host, int port)
      {
         _tcpConnectionFactory = tcpConnectionFactory;
         Host = host;
         Port = port;
      }

      #region Methods

      private TcpConnection _tcpConnection;

      /// <summary>
      /// Opens a connection to the client.
      /// </summary>
      public void Open()
      {
         // check connection state, callers should make sure the connection is not already open
         if (State == ClientConnectionState.Open) throw new InvalidOperationException("The connection is already open.");

         // capture the current connection state
         ClientConnectionState state = State;
         // dispose of any previous client
         _tcpConnection?.Close();
         // create new TcpClient and connect
         _tcpConnection = _tcpConnectionFactory.Create();
         _tcpConnection.Connect(Host, Port, ConnectionTimeout);
         // if connection state changed, raise event
         if (state != State)
         {
            OnStateChanged(new ClientConnectionStateChangedEventArgs(State));
         }
      }

      /// <summary>
      /// Close the connection to the Folding@Home client server.
      /// </summary>
      public void Close()
      {
         // capture the current connection state
         ClientConnectionState state = State;
         // close the connection
         _tcpConnection?.Close();
         // if connection state changed, raise event
         if (state != State)
         {
            OnStateChanged(new ClientConnectionStateChangedEventArgs(State));
         }
      }

      public ClientCommand CreateCommand()
      {
         return new ClientCommand(this);
      }

      public Stream GetStream()
      {
         return _tcpConnection.GetStream();
      }

      protected virtual void OnStateChanged(ClientConnectionStateChangedEventArgs e)
      {
         StateChanged?.Invoke(this, e);
      }

      private void OnDataSent(DataEventArgs e)
      {
         DataSent?.Invoke(this, e);
      }

      private void OnDataReceived(DataEventArgs e)
      {
         DataReceived?.Invoke(this, e);
      }

      #endregion

      #region IDisposable Members

      private bool _disposed;

      /// <summary>
      /// Releases all resources used by the ClientConnection.
      /// </summary>
      public void Dispose()
      {
         Dispose(true);
         GC.SuppressFinalize(this);
      }

      /// <summary>
      /// Releases the unmanaged resources used by the ClientConnection and optionally releases the managed resources.
      /// </summary>
      protected virtual void Dispose(bool disposing)
      {
         if (!_disposed)
         {
            if (disposing)
            {
               // close connection
               Close();
            }
         }
         _disposed = true;
      }

      #endregion
   }

   /// <summary>
   /// Provides data for connection state of a ClientConnection. This class cannot be inherited.
   /// </summary>
   public sealed class ClientConnectionStateChangedEventArgs : EventArgs
   {
      /// <summary>
      /// Gets the connection status.
      /// </summary>
      public ClientConnectionState State { get; }

      internal ClientConnectionStateChangedEventArgs(ClientConnectionState state)
      {
         State = state;
      }
   }

   /// <summary>
   /// Provides data for sent and received events of a ClientConnection. This class cannot be inherited.
   /// </summary>
   public sealed class DataEventArgs : EventArgs
   {
      /// <summary>
      /// Gets the data text value.
      /// </summary>
      public string Value { get; private set; }

      /// <summary>
      /// Gets the data length in bytes.
      /// </summary>
      public int Length { get; private set; }

      internal DataEventArgs(string value, int length)
      {
         Value = value;
         Length = length;
      }
   }
}
