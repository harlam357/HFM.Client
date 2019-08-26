
using System;
using System.Collections.Generic;
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

      private FahClientMessageExtractorBase _messageExtractor;
      /// <summary>
      /// Gets or sets the message extractor responsible for extracting messages from the data received from the Folding@Home client.  The default implementation extracts messages as JSON text.
      /// </summary>
      public FahClientMessageExtractorBase MessageExtractor
      {
         get => _messageExtractor ?? (_messageExtractor = new FahClientJsonMessageExtractor());
         set => _messageExtractor = value;
      }

      private bool GetNextMessage(byte[] buffer, int bytesRead)
      {
         _readBuffer.Append(Encoding.ASCII.GetChars(buffer, 0, bytesRead));
         var nextMessage = MessageExtractor.Extract(_readBuffer);
         if (nextMessage != null)
         {
            Message = nextMessage;
         }
         return nextMessage != null;
      }
   }

   /// <summary>
   /// Provides the abstract base class for a Folding@Home client message extraction.
   /// </summary>
   public abstract class FahClientMessageExtractorBase
   {
      /// <summary>
      /// Extracts a new <see cref="FahClientMessage"/> from the <paramref name="buffer"/> if a message is available.
      /// </summary>
      /// <returns>A new <see cref="FahClientMessage"/> or null if a message is not available.</returns>
      public abstract FahClientMessage Extract(StringBuilder buffer);
   }

   /// <summary>
   /// Folding@Home client message extractor that extracts messages in PyON format.
   /// </summary>
   public class FahClientMessageExtractor : FahClientMessageExtractorBase
   {
      /// <summary>
      /// Extracts a new <see cref="FahClientMessage"/> from the <paramref name="buffer"/> if a message is available.
      /// </summary>
      /// <returns>A new <see cref="FahClientMessage"/> or null if a message is not available.</returns>
      public override FahClientMessage Extract(StringBuilder buffer)
      {
         if (buffer == null) return null;

         var indexes = new Dictionary<string, int>();
         if (!ExtractIndexes(buffer, indexes)) return null;
         
         string messageType = ExtractMessageType(buffer, indexes);
         if (messageType == null) return null;

         var messageText = ExtractMessageText(buffer, indexes);
         if (messageText == null) return null;

         int endFooterIndex = indexes[IndexKey.EndFooter];
         // remove the extracted message from the buffer
         buffer.Remove(0, endFooterIndex);

         return new FahClientMessage(new FahClientMessageIdentifier(messageType, DateTime.UtcNow), messageText);
      }

      /// <summary>
      /// Extracts indexes from the <paramref name="buffer"/> and stores them in the <paramref name="indexes"/> dictionary for later processing.
      /// </summary>
      /// <returns>true if all required indexes are found; otherwise, false.  Message extraction will not continue if this method returns false.</returns>
      protected virtual bool ExtractIndexes(StringBuilder buffer, IDictionary<string, int> indexes)
      {
         indexes[IndexKey.StartHeader] = IndexOf(buffer, SearchValue.PyONHeader, 0);
         if (indexes[IndexKey.StartHeader] < 0) return false;

         int endHeaderStartMessageTypeIndex = indexes[IndexKey.StartHeader] + SearchValue.PyONHeader.Length;
         indexes[IndexKey.EndHeader] = endHeaderStartMessageTypeIndex;
         indexes[IndexKey.StartMessageType] = endHeaderStartMessageTypeIndex;

         string[] newLineValues =
         {
            SearchValue.NewLineCrLf,
            SearchValue.NewLineLf
         };
         indexes[IndexKey.EndMessageType] = IndexOfAny(buffer, newLineValues, endHeaderStartMessageTypeIndex);
         if (indexes[IndexKey.EndMessageType] < 0) return false;

         string[] footerValues =
         {
            String.Concat(SearchValue.NewLineCrLf, SearchValue.PyONFooter, SearchValue.NewLineCrLf),
            String.Concat(SearchValue.NewLineLf, SearchValue.PyONFooter, SearchValue.NewLineLf)
         };
         indexes[IndexKey.StartFooter] = IndexOfAny(buffer, footerValues, indexes[IndexKey.EndMessageType]);
         if (indexes[IndexKey.StartFooter] < 0) return false;

         indexes[IndexKey.EndFooter] = EndIndexOfAny(buffer, footerValues, indexes[IndexKey.StartFooter]);
         if (indexes[IndexKey.EndFooter] < 0) return false;

         return true;
      }

      /// <summary>
      /// Extracts the message type from the <paramref name="buffer"/>.
      /// </summary>
      /// <returns>The message type as a string.</returns>
      protected virtual string ExtractMessageType(StringBuilder buffer, IDictionary<string, int> indexes)
      {
         int start = indexes[IndexKey.StartMessageType];
         int end = indexes[IndexKey.EndMessageType];
         return buffer.Substring(start, end - start);
      }

      /// <summary>
      /// Extracts the message text from the <paramref name="buffer"/>.
      /// </summary>
      /// <returns>The message text as a string.</returns>
      protected virtual string ExtractMessageText(StringBuilder buffer, IDictionary<string, int> indexes)
      {
         int start = indexes[IndexKey.StartHeader];
         int end = indexes[IndexKey.EndFooter];
         return buffer.Substring(start, end - start);
      }

      /// <summary>
      /// Reports the index of the first occurrence of any of the specified strings in the given StringBuilder object.
      /// </summary>
      protected static int IndexOfAny(StringBuilder buffer, IEnumerable<string> values, int startIndex)
      {
         foreach (var value in values)
         {
            int index = IndexOf(buffer, value, startIndex);
            if (index >= 0)
            {
               return index;
            }
         }
         return -1;
      }

      /// <summary>
      /// Reports the end index of the first occurrence of any of the specified strings in the given StringBuilder object.
      /// </summary>
      protected static int EndIndexOfAny(StringBuilder buffer, IEnumerable<string> values, int startIndex)
      {
         foreach (var value in values)
         {
            int index = EndIndexOf(buffer, value, startIndex);
            if (index >= 0)
            {
               return index;
            }
         }
         return -1;
      }

      /// <summary>
      /// Reports the index of the first occurrence of the specified string in the given StringBuilder object.
      /// </summary>
      protected static int IndexOf(StringBuilder buffer, string value, int startIndex)
      {
         int index = buffer.IndexOf(value, startIndex, false);
         if (index >= 0)
         {
            return index;
         }
         return -1;
      }

      /// <summary>
      /// Reports the end index of the first occurrence of the specified string in the given StringBuilder object.
      /// </summary>
      protected static int EndIndexOf(StringBuilder buffer, string value, int startIndex)
      {
         int index = buffer.IndexOf(value, startIndex, false);
         if (index >= 0)
         {
            return index + value.Length;
         }
         return -1;
      }

      /// <summary>
      /// Provides well-known key values for the indexes dictionary.
      /// </summary>
      protected static class IndexKey
      {
         public const string StartHeader = nameof(StartHeader);
         public const string EndHeader = nameof(EndHeader);
         public const string StartMessageType = nameof(StartMessageType);
         public const string EndMessageType = nameof(EndMessageType);
         public const string StartFooter = nameof(StartFooter);
         public const string EndFooter = nameof(EndFooter);
      }

      /// <summary>
      /// Provides well-known string values for searching the buffer for indexes.
      /// </summary>
      protected static class SearchValue
      {
         public const string NewLineCrLf = "\r\n";
         public const string NewLineLf = "\n";
         public const string PyONHeader = "PyON 1 ";
         public const string PyONFooter = "---";
      }
   }

   public class FahClientJsonMessageExtractor : FahClientMessageExtractor
   {
      private const string BeginObject = nameof(BeginObject);

      protected override bool ExtractIndexes(StringBuilder buffer, IDictionary<string, int> indexes)
      {
         if (!base.ExtractIndexes(buffer, indexes))
         {
            return false;
         }

         string[] newLineValues =
         {
            SearchValue.NewLineCrLf,
            SearchValue.NewLineLf
         };
         indexes[BeginObject] = EndIndexOfAny(buffer, newLineValues, indexes[IndexKey.EndMessageType]);
         if (indexes[BeginObject] < 0)
         {
            return false;
         }

         return true;
      }

      protected override string ExtractMessageText(StringBuilder buffer, IDictionary<string, int> indexes)
      {
         int beginObjectIndex = indexes[BeginObject];
         int startFooterIndex = indexes[IndexKey.StartFooter];
         var json = buffer.SubstringBuilder(beginObjectIndex, null, startFooterIndex - beginObjectIndex);
         ConvertPythonValuesToJsonValues(json);
         return json.ToString();
      }

      private static void ConvertPythonValuesToJsonValues(StringBuilder buffer)
      {
         buffer.Replace(": None", ": null");
         buffer.Replace(": True", ": true");
         buffer.Replace(": False", ": false");
      }
   }
}
