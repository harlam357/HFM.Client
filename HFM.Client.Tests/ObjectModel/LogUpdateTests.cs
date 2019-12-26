
using NUnit.Framework;

namespace HFM.Client.ObjectModel
{
    [TestFixture]
    public class LogUpdateTests
    {
        [Test]
        public void LogUpdate_Load_FromWindowsClientVersion_7_1_24()
        {
            var logUpdate = LogUpdate.Load(TestDataReader.ReadStringBuilder("Client_7_1_24_log-restart_Windows.txt"));
            Assert.AreEqual(65571, logUpdate.Value.Length);
        }
    }
}
