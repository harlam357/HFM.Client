using System;
using System.Text;

namespace HFM.Client.ObjectModel.Internal
{
    internal class LogUpdateNewLineFilter : IJsonStringFilter
    {
        public StringBuilder Filter(string s) => new StringBuilder(
            s.Replace("\n", Environment.NewLine).Replace("\\n", Environment.NewLine));

        public StringBuilder Filter(StringBuilder s) =>
            s.Replace("\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
    }
}
