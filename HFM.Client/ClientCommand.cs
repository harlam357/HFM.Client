
using System;
using System.Text;
using System.Threading.Tasks;

namespace HFM.Client
{
   public class ClientCommand
   {
      //public string CommandText { get; set; }

      //public ClientConnection Connection { get; set; }

      //public ClientCommand(ClientConnection connection)
      //{
      //   Connection = connection;
      //}

      //public ClientCommand(string cmdText, ClientConnection connection)
      //{
      //   CommandText = cmdText;
      //   Connection = connection;
      //}

      //public void Execute()
      //{
      //   // check connection status, callers should make sure they're connected first
      //   if (Connection.State == ClientConnectionState.Closed) throw new InvalidOperationException("The current state of the connection is closed.");

      //   if (!CommandText.EndsWith("\n", StringComparison.Ordinal))
      //   {
      //      CommandText += "\n";
      //   }
      //   byte[] buffer = Encoding.ASCII.GetBytes(CommandText);
      //   Connection.GetStream().Write(buffer, 0, buffer.Length);
      //}

      //public async Task ExecuteAsync()
      //{
      //   // check connection status, callers should make sure they're connected first
      //   if (Connection.State == ClientConnectionState.Closed) throw new InvalidOperationException("The current state of the connection is closed.");

      //   if (!CommandText.EndsWith("\n", StringComparison.Ordinal))
      //   {
      //      CommandText += "\n";
      //   }
      //   byte[] buffer = Encoding.ASCII.GetBytes(CommandText);
      //   await Connection.GetStream().WriteAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
      //}
   }
}
