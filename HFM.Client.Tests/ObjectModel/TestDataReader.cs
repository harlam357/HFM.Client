
using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace HFM.Client.ObjectModel
{
    internal static class TestDataReader
    {
        internal static string ReadString(string resourceName)
        {
            return GetResourceString(resourceName);
        }

        internal static StringBuilder ReadStringBuilder(string resourceName)
        {
            return new StringBuilder(GetResourceString(resourceName));
        }

        internal static Stream ReadStream(string resourceName)
        {
            return GetResourceStream(resourceName);
        }

        private static String GetResourceString(string resourceName)
        {
            using var reader = new StreamReader(GetResourceStream(resourceName));
            return reader.ReadToEnd();
        }

        private const string TestDataNamespace = "HFM.Client.ObjectModel.TestData";

        private static Stream GetResourceStream(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream($"{TestDataNamespace}.{resourceName}");
        }
    }
}
