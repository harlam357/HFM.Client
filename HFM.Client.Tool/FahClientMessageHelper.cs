
using System;

namespace HFM.Client.Tool
{
    internal static class FahClientMessageHelper
    {
        internal static string FormatForDisplay(FahClientMessage message)
        {
            return SetEnvironmentNewLineCharacters(message.ToString());
        }

        private static string SetEnvironmentNewLineCharacters(string text)
        {
            return text
                .Replace("\n", Environment.NewLine)
                .Replace("\\n", Environment.NewLine);
        }
    }
}
