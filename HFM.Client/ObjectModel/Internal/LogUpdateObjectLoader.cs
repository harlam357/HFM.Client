using System;
using System.IO;
using System.Text;

using HFM.Client.Internal;

namespace HFM.Client.ObjectModel.Internal
{
    internal class LogUpdateObjectLoader : ObjectLoader<LogUpdate>
    {
        public override LogUpdate Load(string json, ObjectLoadOptions options = ObjectLoadOptions.Default)
        {
            if (json is null) return null;

            return LoadInternal(new LogUpdate { Value = new StringBuilder(json) });
        }

        public override LogUpdate Load(StringBuilder json, ObjectLoadOptions options = ObjectLoadOptions.Default)
        {
            if (json is null) return null;

            var logUpdate = new LogUpdate { Value = new StringBuilder() };
            json.CopyTo(0, logUpdate.Value, 0, json.Length);
            return LoadInternal(logUpdate);
        }

        public override LogUpdate Load(TextReader textReader)
        {
            if (textReader is null) return null;

            return LoadInternal(new LogUpdate { Value = new StringBuilder(textReader.ReadToEnd()) });
        }

        private static LogUpdate LoadInternal(LogUpdate logUpdate)
        {
            SetEnvironmentNewLineCharacters(logUpdate.Value.Trim('\"'));
            return logUpdate;
        }

        private static void SetEnvironmentNewLineCharacters(StringBuilder value)
        {
            value.Replace("\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
        }
    }
}
