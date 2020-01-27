
using System;
using System.Collections.Generic;
using System.Text;

using HFM.Client.Internal;

namespace HFM.Client
{
    /// <summary>
    /// Folding@Home client message extractor that extracts messages in PyON format.
    /// </summary>
    public class FahClientPyonMessageExtractor : FahClientMessageExtractor
    {
        /// <summary>
        /// Extracts a new <see cref="FahClientMessage"/> from the <paramref name="buffer"/> if a message is available.
        /// </summary>
        /// <returns>A new <see cref="FahClientMessage"/> or null if a message is not available.</returns>
        public override FahClientMessage Extract(StringBuilder buffer)
        {
            if (buffer is null) return null;

            var indexes = new Dictionary<string, int>();
            if (!ExtractIndexes(buffer, indexes)) return null;

            string messageType = ExtractMessageType(buffer, indexes);
            if (messageType is null) return null;

            var messageText = ExtractMessageText(buffer, indexes);
            if (messageText is null) return null;

            int endFooterIndex = indexes[IndexKey.EndFooter];
            // remove the extracted message from the buffer
            buffer.Remove(0, endFooterIndex);

            return new FahClientMessage(new FahClientMessageIdentifier(messageType, DateTime.UtcNow), messageText);
        }

        /// <summary>
        /// Extracts indexes from the <paramref name="buffer"/> and stores them in the <paramref name="indexes"/> dictionary for later processing.
        /// </summary>
        /// <exception cref="ArgumentNullException">buffer -or- indexes is null.</exception>
        /// <returns>true if all required indexes are found; otherwise, false.  Message extraction will not continue if this method returns false.</returns>
        protected virtual bool ExtractIndexes(StringBuilder buffer, IDictionary<string, int> indexes)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));
            if (indexes is null) throw new ArgumentNullException(nameof(indexes));

            var headerIndexes = IndexesOf(buffer, SearchValue.PyonHeader, 0);
            if (headerIndexes.Start < 0) return false;
            indexes[IndexKey.StartHeader] = headerIndexes.Start;
            indexes[IndexKey.EndHeader] = headerIndexes.End;
            indexes[IndexKey.StartMessageType] = headerIndexes.End;

            string[] newLineValues =
            {
                SearchValue.NewLineCrLf,
                SearchValue.NewLineLf
            };
            indexes[IndexKey.EndMessageType] = IndexOfAny(buffer, newLineValues, indexes[IndexKey.StartMessageType]);
            if (indexes[IndexKey.EndMessageType] < 0) return false;

            string[] footerValues =
            {
                String.Concat(SearchValue.NewLineCrLf, SearchValue.PyonFooter, SearchValue.NewLineCrLf),
                String.Concat(SearchValue.NewLineLf, SearchValue.PyonFooter, SearchValue.NewLineLf)
            };
            var footerIndexes = IndexesOfAny(buffer, footerValues, indexes[IndexKey.EndMessageType]);
            if (footerIndexes.Start < 0) return false;
            indexes[IndexKey.StartFooter] = footerIndexes.Start;
            indexes[IndexKey.EndFooter] = footerIndexes.End;

            return true;
        }

        /// <summary>
        /// Extracts the message type from the <paramref name="buffer"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">buffer -or- indexes is null.</exception>
        /// <returns>The message type as a string.</returns>
        protected virtual string ExtractMessageType(StringBuilder buffer, IDictionary<string, int> indexes)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));
            if (indexes is null) throw new ArgumentNullException(nameof(indexes));

            int start = indexes[IndexKey.StartMessageType];
            int end = indexes[IndexKey.EndMessageType];
            return buffer.ToString(start, end - start);
        }

        /// <summary>
        /// Extracts the message text from the <paramref name="buffer"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">buffer -or- indexes is null.</exception>
        /// <returns>The message text as a string.</returns>
        protected virtual StringBuilder ExtractMessageText(StringBuilder buffer, IDictionary<string, int> indexes)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));
            if (indexes is null) throw new ArgumentNullException(nameof(indexes));

            int start = indexes[IndexKey.StartHeader];
            int end = indexes[IndexKey.EndFooter];
            var text = new StringBuilder(end - start);
            buffer.CopyTo(start, text, 0, end - start);
            return text;
        }

        /// <summary>
        /// Provides well-known key values for the indexes dictionary.
        /// </summary>
        private static class IndexKey
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
        private static class SearchValue
        {
            public const string NewLineCrLf = "\r\n";
            public const string NewLineLf = "\n";
            // ReSharper disable InconsistentNaming
            public const string PyonHeader = "PyON 1 ";
            public const string PyonFooter = "---";
            // ReSharper restore InconsistentNaming
        }
    }
}
