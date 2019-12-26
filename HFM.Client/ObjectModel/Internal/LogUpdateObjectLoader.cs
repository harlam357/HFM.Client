
using System;
using System.IO;
using System.Text;

using HFM.Client.Internal;

namespace HFM.Client.ObjectModel.Internal
{
    internal class LogUpdateObjectLoader : ObjectLoader<LogUpdate>
    {
        public override LogUpdate Load(string json)
        {
            return LoadInternal(new LogUpdate { Value = new StringBuilder(json) });
        }

        public override LogUpdate Load(StringBuilder json)
        {
            var logUpdate = new LogUpdate { Value = new StringBuilder() };
            json.CopyTo(logUpdate.Value);
            return LoadInternal(logUpdate);
        }

        public override LogUpdate Load(TextReader textReader)
        {
            return LoadInternal(new LogUpdate { Value = new StringBuilder(textReader.ReadToEnd()) });
        }

        private static LogUpdate LoadInternal(LogUpdate logUpdate)
        {
            int startIndex = GetStartIndex(logUpdate.Value);
            int length = GetLength(logUpdate.Value, startIndex);
            SetEnvironmentNewLineCharacters(logUpdate.Value.SubstringBuilder(startIndex, length, true));
            return logUpdate;
        }

        private static int GetStartIndex(StringBuilder value)
        {
            int startIndex = value.EndIndexOf("\"", false);
            return startIndex != -1 ? startIndex : 0;
        }

        private static int GetLength(StringBuilder value, int startIndex)
        {
            return (value.EndsWith('\"') ? value.Length - 1 : value.Length) - startIndex;
        }

        private static void SetEnvironmentNewLineCharacters(StringBuilder value)
        {
            value.Replace("\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
        }
    }
}
