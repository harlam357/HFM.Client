using System.Text;

namespace HFM.Client.ObjectModel.Internal
{
    internal interface IJsonStringFilter
    {
        StringBuilder Filter(string s);

        StringBuilder Filter(StringBuilder s);
    }
}
