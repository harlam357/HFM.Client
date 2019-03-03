
using System;
using System.Text;
using System.Threading.Tasks;

namespace HFM.Client
{
   public abstract class FahClientCommandBase
   {
      public string CommandText { get; set; }

      protected FahClientCommandBase()
      {
         
      }

      protected FahClientCommandBase(string cmdText)
      {
         CommandText = cmdText;
      }

      public abstract int Execute();

      public abstract Task<int> ExecuteAsync();
   }

   public class FahClientCommand : FahClientCommandBase
   {
      public FahClientConnection Connection { get; }

      public FahClientCommand(FahClientConnection connection)
      {
         Connection = connection;
      }

      public FahClientCommand(FahClientConnection connection, string cmdText)
         : base(cmdText)
      {
         Connection = connection;
      }

      public override int Execute()
      {
         if (!Connection.Connected) throw new InvalidOperationException("The current state of the connection is closed.");

         byte[] buffer = GetBuffer(CommandText);
         Connection.GetDataWriter().Write(buffer);
         return buffer.Length;
      }

      public override async Task<int> ExecuteAsync()
      {
         if (!Connection.Connected) throw new InvalidOperationException("The current state of the connection is closed.");

         byte[] buffer = GetBuffer(CommandText);
         await Connection.GetDataWriter().WriteAsync(buffer).ConfigureAwait(false);
         return buffer.Length;
      }

      private static byte[] GetBuffer(string commandText)
      {
         if (!commandText.EndsWith("\n", StringComparison.Ordinal))
         {
            commandText += "\n";
         }
         return Encoding.ASCII.GetBytes(commandText);
      }
   }
}
