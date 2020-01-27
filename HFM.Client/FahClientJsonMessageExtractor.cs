
using System;
using System.Collections.Generic;
using System.Text;

using HFM.Client.Internal;

namespace HFM.Client
{
    /// <summary>
    /// Folding@Home client message extractor that extracts messages in JSON format.
    /// </summary>
    public class FahClientJsonMessageExtractor : FahClientPyonMessageExtractor
    {
        /// <summary>
        /// Extracts indexes from the <paramref name="buffer"/> and stores them in the <paramref name="indexes"/> dictionary for later processing.
        /// </summary>
        /// <exception cref="ArgumentNullException">buffer -or- indexes is null.</exception>
        /// <returns>true if all required indexes are found; otherwise, false.  Message extraction will not continue if this method returns false.</returns>
        protected override bool ExtractIndexes(StringBuilder buffer, IDictionary<string, int> indexes)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));
            if (indexes is null) throw new ArgumentNullException(nameof(indexes));

            if (!base.ExtractIndexes(buffer, indexes))
            {
                return false;
            }

            string[] newLineValues =
            {
                SearchValue.NewLineCrLf,
                SearchValue.NewLineLf
            };
            indexes[IndexKey.BeginObject] = EndIndexOfAny(buffer, newLineValues, indexes[IndexKey.EndMessageType]);
            if (indexes[IndexKey.BeginObject] < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Extracts the message text from the <paramref name="buffer"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">buffer -or- indexes is null.</exception>
        /// <returns>The message text as a string.</returns>
        protected override StringBuilder ExtractMessageText(StringBuilder buffer, IDictionary<string, int> indexes)
        {
            if (buffer is null) throw new ArgumentNullException(nameof(buffer));
            if (indexes is null) throw new ArgumentNullException(nameof(indexes));

            int start = indexes[IndexKey.BeginObject];
            int end = indexes[IndexKey.StartFooter];
            var text = new StringBuilder(end - start);
            buffer.CopyTo(start, text, 0, end - start);
            ConvertPyonValuesToJsonValues(text);
            return text;
        }

        private static void ConvertPyonValuesToJsonValues(StringBuilder buffer)
        {
            buffer.Replace(": None", ": null")
                .Replace(": True", ": true")
                .Replace(": False", ": false");
        }

        /// <summary>
        /// Provides well-known key values for the indexes dictionary.
        /// </summary>
        private static class IndexKey
        {
            public const string EndMessageType = nameof(EndMessageType);
            public const string BeginObject = nameof(BeginObject);
            public const string StartFooter = nameof(StartFooter);
        }

        /// <summary>
        /// Provides well-known string values for searching the buffer for indexes.
        /// </summary>
        private static class SearchValue
        {
            public const string NewLineCrLf = "\r\n";
            public const string NewLineLf = "\n";
        }
    }
}