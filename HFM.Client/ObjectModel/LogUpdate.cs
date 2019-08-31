
using System;
using System.Text;

using HFM.Client.Internal;

namespace HFM.Client.ObjectModel
{
   public class LogUpdate
   {
      /// <summary>
      /// Folding@Home client log update message.
      /// </summary>
      public StringBuilder Value { get; private set; }

      /// <summary>
      /// Creates a new <see cref="LogUpdate"/> object from the given log message.
      /// </summary>
      public static LogUpdate Create(StringBuilder message)
      {
         var result = new LogUpdate { Value = new StringBuilder() };
         message.CopyTo(result.Value);
         int startIndex = GetStartIndex(result.Value);
         int length = GetLength(result.Value, startIndex);
         SetEnvironmentNewLineCharacters(result.Value.SubstringBuilder(startIndex, length, true));
         return result;
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
         value.Replace("\n", Environment.NewLine);
         value.Replace("\\n", Environment.NewLine);
      }
   }
}
