
using System.IO;

using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class LogUpdateTests
    {
        [Test]
        public void LogUpdate_Load_String_FromClientVersion_7_1_24()
        {
            var logUpdate = LogUpdate.Load(TestDataReader.ReadString("Client_7_1_24_log-restart.txt"));
            Assert.AreEqual(65571, logUpdate.Value.Length);
        }

        [Test]
        public void LogUpdate_Load_StringBuilder_FromClientVersion_7_1_24()
        {
            var logUpdate = LogUpdate.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_log-restart.txt"));
            Assert.AreEqual(65571, logUpdate.Value.Length);
        }

        [Test]
        public void LogUpdate_Load_TextReader_FromClientVersion_7_1_24()
        {
            using var reader = new StreamReader(TestDataReader.ReadStream("Client_7_1_24_log-restart.txt"));
            var logUpdate = LogUpdate.Load(reader);
            Assert.AreEqual(65571, logUpdate.Value.Length);
        }
    }
}
