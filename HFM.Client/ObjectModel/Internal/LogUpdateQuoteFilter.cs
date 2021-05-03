using System.Text;

using static HFM.Client.Internal.StringBuilderExtensions;

namespace HFM.Client.ObjectModel.Internal
{
    internal class LogUpdateQuoteFilter : IJsonStringFilter
    {
        public StringBuilder Filter(string s) => new StringBuilder(s.Trim('\"'));

        public StringBuilder Filter(StringBuilder s) => s.Trim('\"');
    }
}
