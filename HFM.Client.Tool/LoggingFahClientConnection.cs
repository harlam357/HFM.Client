using System;
using System.IO;
using System.Threading.Tasks;

namespace HFM.Client.Tool
{
    public class LoggingFahClientConnection : FahClientConnection
    {
        private readonly string _logFolderPath;

        public LoggingFahClientConnection(string host, int port) : base(host, port)
        {
            _logFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HFM.Client", $"{DateTime.UtcNow:yyyyMMddThhmmss}");
            if (!Directory.Exists(_logFolderPath))
            {
                Directory.CreateDirectory(_logFolderPath);
            }
        }

        protected override FahClientReader OnCreateReader() => new LoggingFahClientReader(this, _logFolderPath);

        private class LoggingFahClientReader : FahClientReader
        {
            private readonly string _logFolderPath;

            public LoggingFahClientReader(FahClientConnection connection, string logFolderPath) : base(connection, new FahClientPyonMessageExtractor())
            {
                _logFolderPath = logFolderPath;
            }

            public override bool Read() => SaveMessageAfterRead(base.Read());

            public override async Task<bool> ReadAsync() => SaveMessageAfterRead(await base.ReadAsync());

            private bool SaveMessageAfterRead(bool result)
            {
                if (result)
                {
                    var m = Message;
                    string path = Path.Combine(_logFolderPath, $"{m.Identifier.MessageType}-{m.Identifier.Received:yyyyMMddThhmmss}.txt");
                    File.WriteAllText(path, m.MessageText.ToString());
                }
                return result;
            }
        }
    }
}
