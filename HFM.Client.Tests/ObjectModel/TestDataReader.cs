
using System.IO;
using System.Reflection;

namespace HFM.Client.ObjectModel
{
    internal static class TestDataReader
    {
        private const string TestDataNamespace = "HFM.Client.ObjectModel.TestData";

        internal static string ReadString(string resourceName)
        {
            string message;
            using (var sr = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream($"{TestDataNamespace}.{resourceName}")))
            {
                message = sr.ReadToEnd();
            }
            return message;
        }
    }
}
