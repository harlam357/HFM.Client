﻿
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
            using var reader = new StreamReader(ReadStream(resourceName));
            return reader.ReadToEnd();
        }

        internal static StringBuilder ReadStringBuilder(string resourceName)
        {
#if NETCOREAPP3_1
            return GetResourceStringBuilder(resourceName);
#else
            return new StringBuilder(ReadString(resourceName));
#endif
        }

        private const string TestDataNamespace = "HFM.Client.ObjectModel.TestData";

        internal static Stream ReadStream(string resourceName)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream($"{TestDataNamespace}.{resourceName}");
        }

#if NETCOREAPP3_1
        private static StringBuilder GetResourceStringBuilder(string resourceName)
        {
            // build StringBuilder from Stream with no intermediate heap allocations
            // https://docs.microsoft.com/en-us/archive/msdn-magazine/2018/january/csharp-all-about-span-exploring-a-new-net-mainstay

            var stream = ReadStream(resourceName);
            var result = new StringBuilder((int)stream.Length);
            Span<char> buffer = stackalloc char[128];

            using var reader = new StreamReader(stream);
            int bytesRead;
            while ((bytesRead = reader.ReadBlock(buffer)) > 0)
            {
                result.Append(buffer.Slice(0, bytesRead));
            }
            return result;
        }
#endif
    }
}
