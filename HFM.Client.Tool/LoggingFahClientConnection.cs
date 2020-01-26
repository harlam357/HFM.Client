
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HFM.Client.Tool
{
    public class LoggingFahClientConnection : FahClientConnection
    {
        private readonly string _logFilePath;

        public LoggingFahClientConnection(string host, int port) : base(host, port)
        {
            string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "HFM");
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            _logFilePath = Path.Combine(folder, $"HFM.Client.Tool-{DateTime.UtcNow:yyyyMMddThhmmss}.log");
        }

        protected override FahClientReader OnCreateReader()
        {
            return new LoggingFahClientReader(this, _logFilePath);
        }

        private class LoggingFahClientReader : FahClientReader
        {
            private readonly string _logFilePath;

            public LoggingFahClientReader(FahClientConnection connection, string logFilePath) : base(connection)
            {
                _logFilePath = logFilePath;
            }

            protected override int OnReadStream(Stream stream, byte[] buffer, int offset, int count)
            {
                int bytesRead = base.OnReadStream(stream, buffer, offset, count);
                File.AppendAllText(_logFilePath, Encoding.ASCII.GetString(buffer, offset, bytesRead));
                return bytesRead;
            }

            protected override async Task<int> OnReadStreamAsync(Stream stream, byte[] buffer, int offset, int count)
            {
                int bytesRead = await base.OnReadStreamAsync(stream, buffer, offset, count).ConfigureAwait(false);
                File.AppendAllText(_logFilePath, Encoding.ASCII.GetString(buffer, offset, bytesRead));
                return bytesRead;
            }
        }
    }
}
