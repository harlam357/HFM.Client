
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using HFM.Client.Internal;

namespace HFM.Client
{
   /// <summary>
   /// Provides the abstract base class for a Folding@Home client message reader.
   /// </summary>
   public abstract class FahClientReaderBase
   {
      /// <summary>
      /// Gets the last message read by the reader.
      /// </summary>
      public FahClientMessage Message { get; protected set; }

      /// <summary>
      /// Advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="Message"/> property.
      /// </summary>
      /// <returns>true if a message was read; otherwise false.</returns>
      public abstract bool Read();

      /// <summary>
      /// Asynchronously advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="Message"/> property.
      /// </summary>
      /// <returns>A task representing the asynchronous operation that can return true if a message was read; otherwise false.</returns>
      public abstract Task<bool> ReadAsync();
   }

   /// <summary>
   /// Folding@Home client message reader.
   /// </summary>
   public class FahClientReader : FahClientReaderBase
   {
      /// <summary>
      /// Gets the <see cref="FahClientConnection"/> used by this <see cref="FahClientReader"/>.
      /// </summary>
      public FahClientConnection Connection { get; }

      /// <summary>
      /// Initializes a new instance of the <see cref="FahClientReader"/> class.
      /// </summary>
      /// <param name="connection">A <see cref="FahClientConnection"/> that represents the connection to a Folding@Home client.</param>
      public FahClientReader(FahClientConnection connection)
      {
         Connection = connection;
      }

      /// <summary>
      /// Gets or sets the size of the read buffer, in bytes. The default value is 1024 bytes.
      /// </summary>
      public int BufferSize { get; set; } = 1024;

      /// <summary>
      /// Gets or sets the amount of time a <see cref="FahClientReaderBase" /> will wait to read the next message.  The default value is zero and specifies no timeout.
      /// </summary>
      public int ReadTimeout { get; set; } = 0;

      /// <summary>
      /// Advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="FahClientReaderBase.Message"/> property.
      /// </summary>
      /// <exception cref="InvalidOperationException">The connection is not open.</exception>
      /// <returns>true if a message was read; otherwise false.</returns>
      public override bool Read()
      {
         if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

         int bytesRead;
         var buffer = new byte[BufferSize];
         while ((bytesRead = ReadInternal(buffer)) != 0)
         {
            if (GetNextMessage(buffer, bytesRead))
            {
               return true;
            }
         }
         return false;
      }

      private int ReadInternal(byte[] buffer)
      {
         var stream = GetStream();

         try
         {
            int timeout = ReadTimeout;
            if (timeout > 0)
            {
               var readTask = stream.ReadAsync(buffer, 0, buffer.Length);
               return readTask == Task.WhenAny(readTask, Task.Delay(timeout)).GetAwaiter().GetResult() 
                  ? readTask.GetAwaiter().GetResult() 
                  : 0;
            }
            return stream.Read(buffer, 0, buffer.Length);
         }
         catch (Exception)
         {
            Connection.Close();
            throw;
         }
      }

      /// <summary>
      /// Asynchronously advances the reader to the next message received from the Folding@Home client and stores it in the <see cref="FahClientReaderBase.Message"/> property.
      /// </summary>
      /// <exception cref="InvalidOperationException">The connection is not open.</exception>
      /// <returns>A task representing the asynchronous operation that can return true if a message was read; otherwise false.</returns>
      public override async Task<bool> ReadAsync()
      {
         if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

         int bytesRead;
         var buffer = new byte[BufferSize];
         while ((bytesRead = await ReadAsyncInternal(buffer).ConfigureAwait(false)) != 0)
         {
            if (GetNextMessage(buffer, bytesRead))
            {
               return true;
            }
         }
         return false;
      }

      private async Task<int> ReadAsyncInternal(byte[] buffer)
      {
         var stream = GetStream();

         try
         {
            var readTask = stream.ReadAsync(buffer, 0, buffer.Length);

            int timeout = ReadTimeout;
            if (timeout > 0)
            {
               return readTask == await Task.WhenAny(readTask, Task.Delay(timeout)).ConfigureAwait(false) 
                  ? await readTask.ConfigureAwait(false) 
                  : 0;
            }
            return await readTask.ConfigureAwait(false);
         }
         catch (Exception)
         {
            Connection.Close();
            throw;
         }
      }

      private Stream GetStream()
      {
         var stream = Connection.TcpConnection?.GetStream();
         if (stream == null)
         {
            throw new InvalidOperationException("The connection is not open.");
         }
         return stream;
      }

      private readonly StringBuilder _readBuffer = new StringBuilder();

      private bool GetNextMessage(byte[] buffer, int bytesRead)
      {
         _readBuffer.Append(Encoding.ASCII.GetChars(buffer, 0, bytesRead));
         var nextMessage = FahClientMessageParser.GetNextMessage(_readBuffer);
         if (nextMessage != null)
         {
            Message = nextMessage;
         }
         return nextMessage != null;
      }
   }

   public static class FahClientMessageParser
   {
      private const string PyonHeader = "PyON 1 ";
      private const string PyonFooter = "---";

      public static FahClientMessage GetNextMessage(StringBuilder buffer)
      {
         if (buffer == null) return null;

         // find the header
         int messageIndex = buffer.IndexOf(PyonHeader, false);
         if (messageIndex < 0)
         {
            return null;
         }
         // set starting message index
         messageIndex += PyonHeader.Length;

         // find the first CrLf or Lf character after the header
         int startIndex = FindStartIndex(buffer, messageIndex);
         if (startIndex < 0) return null;

         // find the footer
         int endIndex = FindEndIndex(buffer, startIndex);
         if (endIndex < 0)
         {
            return null;
         }

         // get the message type
         string messageType = buffer.Substring(messageIndex, startIndex - messageIndex);
         var messageStringBuilder = new StringBuilder();

         // get the PyON message
         buffer.SubstringBuilder(startIndex, messageStringBuilder, endIndex - startIndex);
         // replace PyON values with JSON values
         messageStringBuilder.Replace(": None", ": null");
         messageStringBuilder.Replace(": True", ": true");
         messageStringBuilder.Replace(": False", ": false");

         // set the index so we know where to trim the string (end plus footer length)
         int nextStartIndex = endIndex + PyonFooter.Length;
         // if more buffer is available set it and return, otherwise set the buffer empty
         if (nextStartIndex < buffer.Length)
         {
            buffer.Remove(0, nextStartIndex);
         }
         else
         {
            buffer.Clear();
         }

         return new FahClientMessage(new FahClientMessageIdentifier(messageType, DateTime.UtcNow), messageStringBuilder.ToString());
      }

      private const string LineFeed = "\n";
      private const string CarriageReturnLineFeed = "\r\n";

      private static int FindStartIndex(StringBuilder buffer, int messageIndex)
      {
         int index = buffer.IndexOf(CarriageReturnLineFeed, messageIndex, false);
         return index >= 0 ? index : buffer.IndexOf(LineFeed, messageIndex, false);
      }

      private static int FindEndIndex(StringBuilder buffer, int startIndex)
      {
         int index = buffer.IndexOf(String.Concat(CarriageReturnLineFeed, PyonFooter, CarriageReturnLineFeed), startIndex, false);
         if (index >= 0)
         {
            return index;
         }

         index = buffer.IndexOf(String.Concat(LineFeed, PyonFooter, LineFeed), startIndex, false);
         if (index >= 0)
         {
            return index;
         }

         return -1;
      }
   }
}
