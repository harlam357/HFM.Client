using System;
using System.Collections.Generic;
using System.Text;

using HFM.Client.Internal;

namespace HFM.Client
{
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
         // ReSharper disable InconsistentNaming
         public const string PyONHeader = "PyON 1 ";
         public const string PyONFooter = "---";
         // ReSharper restore InconsistentNaming
      }
   }

   /// <summary>
   /// Folding@Home client message extractor that extracts messages in JSON format.
   /// </summary>
   public class FahClientJsonMessageExtractor : FahClientMessageExtractor
   {
      private const string BeginObject = nameof(BeginObject);

      /// <summary>
      /// Extracts indexes from the <paramref name="buffer"/> and stores them in the <paramref name="indexes"/> dictionary for later processing.
      /// </summary>
      /// <returns>true if all required indexes are found; otherwise, false.  Message extraction will not continue if this method returns false.</returns>
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

      /// <summary>
      /// Extracts the message text from the <paramref name="buffer"/>.
      /// </summary>
      /// <returns>The message text as a string.</returns>
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