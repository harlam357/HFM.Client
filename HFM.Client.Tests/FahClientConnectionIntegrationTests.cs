
using System;
using System.Threading.Tasks;

using NUnit.Framework;

namespace HFM.Client
{
   [TestFixture]
   public class FahClientConnectionIntegrationTests
   {
      private const string Host = "192.168.1.188";
      private const int Port = 36330;

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void FahClientConnection_CloseConnectionWhileExecutingCommand()
      {
         using (var connection = new FahClientConnection(Host, Port))
         {
            FahClientConnectionBase connectionBase = connection;
            connectionBase.Open();
            // close the connection later
            Task.Delay(1000).ContinueWith(t => connectionBase.Close());

            var command = connectionBase.CreateCommand();
            command.CommandText = "info";

            try
            {
               // continually execute the command
               while (command.Execute() > 0)
               {
                  
               }
            }
            catch (Exception ex)
            {
               Assert.IsFalse(connectionBase.Connected);
               Console.WriteLine(ex);
            }
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void FahClientConnection_WritesCommandsAndReadsMessagesSynchronouslyUntilTimeout()
      {
         using (var connection = new FahClientConnection(Host, Port))
         {
            FahClientConnectionBase connectionBase = connection;
            connectionBase.Open();

            var command = connectionBase.CreateCommand();
            command.CommandText = "info";
            command.Execute();
            command.CommandText = "log-updates restart";
            command.Execute();

            var reader = connectionBase.CreateReader();
            reader.ReadTimeout = 2000;
            while (reader.Read())
            {
               Console.WriteLine(reader.Message);
            }
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public async Task FahClientConnection_WritesCommandsAndReadsMessagesAsynchronouslyUntilTimeout()
      {
         using (var connection = new FahClientConnection(Host, Port))
         {
            FahClientConnectionBase connectionBase = connection;
            await connectionBase.OpenAsync();

            var command = connectionBase.CreateCommand();
            command.CommandText = "info";
            await command.ExecuteAsync();
            command.CommandText = "log-updates restart";
            await command.ExecuteAsync();

            var reader = connectionBase.CreateReader();
            reader.ReadTimeout = 2000;
            while (await reader.ReadAsync())
            {
               Console.WriteLine(reader.Message);
            }
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void FahClientConnection_CloseConnectionWhileExecutingReaderSynchronously()
      {
         using (var connection = new FahClientConnection(Host, Port))
         {
            FahClientConnectionBase connectionBase = connection;
            connectionBase.Open();

            // close the connection later
            Task.Delay(3000).ContinueWith(t => connectionBase.Close());

            var command = connectionBase.CreateCommand();
            command.CommandText = "info";
            command.Execute();
            command.CommandText = "log-updates restart";
            command.Execute();

            var reader = connectionBase.CreateReader();
            try
            {
               while (reader.Read())
               {
                  Console.WriteLine(reader.Message);
               }
            }
            catch (Exception ex)
            {
               Assert.IsFalse(connectionBase.Connected);
               Console.WriteLine(ex);
            }
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public async Task FahClientConnection_CloseConnectionWhileExecutingReaderAsynchronously()
      {
         using (var connection = new FahClientConnection(Host, Port))
         {
            FahClientConnectionBase connectionBase = connection;
            await connectionBase.OpenAsync();

            // close the connection later
            var closeConnectionLater = Task.Delay(3000).ContinueWith(t => connectionBase.Close());

            var command = connectionBase.CreateCommand();
            command.CommandText = "info";
            await command.ExecuteAsync();
            command.CommandText = "log-updates restart";
            await command.ExecuteAsync();

            var reader = connectionBase.CreateReader();
            try
            {
               while (await reader.ReadAsync())
               {
                  Console.WriteLine(reader.Message);
               }
            }
            catch (Exception ex)
            {
               Assert.IsFalse(connectionBase.Connected);
               Console.WriteLine(ex);
            }
         }
      }
   }
}
