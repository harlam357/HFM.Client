
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
        protected override StringBuilder ExtractMessageText(StringBuilder buffer, IDictionary<string, int> indexes)
        {
            int start = indexes[BeginObject];
            int end = indexes[IndexKey.StartFooter];
            var text = new StringBuilder();
            buffer.CopyTo(start, text, 0, end - start);
            ConvertPyonValuesToJsonValues(text);
            return text;
        }

        private static void ConvertPyonValuesToJsonValues(StringBuilder buffer)
        {
            buffer.Replace(": None", ": null");
            buffer.Replace(": True", ": true");
            buffer.Replace(": False", ": false");
        }
    }
}