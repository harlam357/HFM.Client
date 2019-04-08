
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using HFM.Client.Internal;

namespace HFM.Client
{
   public abstract class FahClientReaderBase
   {
      public int ReadTimeout { get; set; } = 10000;

      public FahClientMessage Message { get; protected set; }

      public abstract bool Read();

      public abstract Task<bool> ReadAsync();
   }

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
         _readBuffer = new StringBuilder();
      }

      public int BufferSize { get; set; } = 1024;

      public override bool Read()
      {
         if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

         int bytesRead;
         var buffer = new byte[BufferSize];
         while ((bytesRead = ReadWithTimeout(buffer)) != 0)
         {
            var nextMessage = GetNextMessage(buffer, bytesRead);
            if (nextMessage != null)
            {
               Message = nextMessage;
               return true;
            }
         }
         return false;
      }

      private int ReadWithTimeout(byte[] buffer)
      {
         var stream = GetStream();

         try
         {
            var readTask = stream.ReadAsync(buffer, 0, buffer.Length);
            if (readTask == Task.WhenAny(readTask, Task.Delay(ReadTimeout)).Result)
            {
               return readTask.Result;
            }
            return 0;
         }
         catch (AggregateException ex)
         {
            Connection.Close();
            throw ex.GetBaseException();
         }
      }

      public override async Task<bool> ReadAsync()
      {
         if (!Connection.Connected) throw new InvalidOperationException("The connection is not open.");

         int bytesRead;
         var buffer = new byte[BufferSize];
         while ((bytesRead = await ReadAsyncWithTimeout(buffer).ConfigureAwait(false)) != 0)
         {
            var nextMessage = GetNextMessage(buffer, bytesRead);
            if (nextMessage != null)
            {
               Message = nextMessage;
               return true;
            }
         }
         return false;
      }

      private async Task<int> ReadAsyncWithTimeout(byte[] buffer)
      {
         var stream = GetStream();

         try
         {
            var readTask = stream.ReadAsync(buffer, 0, buffer.Length);
            if (readTask == await Task.WhenAny(readTask, Task.Delay(ReadTimeout)).ConfigureAwait(false))
            {
               return await readTask.ConfigureAwait(false);
            }
            return 0;
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

      private readonly StringBuilder _readBuffer;

      private FahClientMessage GetNextMessage(byte[] buffer, int bytesRead)
      {
         _readBuffer.Append(Encoding.ASCII.GetChars(buffer, 0, bytesRead));
         return FahClientMessageParser.GetNextMessage(_readBuffer);
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
