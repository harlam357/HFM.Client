
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
      public void FahClientConnection_WritesCommandsAndReadsMessagesSynchronouslyUntilTimeout()
      {
         using (var connection = new FahClientConnection(Host, Port))
         {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "info";
            command.Execute();
            command.CommandText = "log-updates restart";
            command.Execute();

            var reader = connection.CreateReader();
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
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "info";
            await command.ExecuteAsync();
            command.CommandText = "log-updates restart";
            await command.ExecuteAsync();

            var reader = connection.CreateReader();
            reader.ReadTimeout = 2000;
            while (await reader.ReadAsync())
            {
               Console.WriteLine(reader.Message);
            }
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void FahClientConnection_CloseConnectionWhileExecutingCommandSynchronously()
      {
         using (var connection = new FahClientConnection(Host, Port))
         {
            connection.Open();
            CloseConnectionAfter(1000, connection);

            var command = connection.CreateCommand();
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
               Assert.IsFalse(connection.Connected);
               Console.WriteLine(ex);
            }
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public async Task FahClientConnection_CloseConnectionWhileExecutingCommandAsynchronously()
      {
         using (var connection = new FahClientConnection(Host, Port))
         {
            await connection.OpenAsync();
            CloseConnectionAfter(1000, connection);

            var command = connection.CreateCommand();
            command.CommandText = "info";

            try
            {
               // continually execute the command
               while (await command.ExecuteAsync() > 0)
               {

               }
            }
            catch (Exception ex)
            {
               Assert.IsFalse(connection.Connected);
               Console.WriteLine(ex);
            }
         }
      }

      [Test]
      [Category(TestCategoryNames.Integration)]
      public void FahClientConnection_CloseConnectionWhileExecutingReaderSynchronously()
      {
         using (var connection = new FahClientConnection(Host, Port))
         {
            connection.Open();
            CloseConnectionAfter(3000, connection);

            var command = connection.CreateCommand();
            command.CommandText = "info";
            command.Execute();
            command.CommandText = "log-updates restart";
            command.Execute();

            var reader = connection.CreateReader();
            try
            {
               while (reader.Read())
               {
                  Console.WriteLine(reader.Message);
               }
            }
            catch (Exception ex)
            {
               Assert.IsFalse(connection.Connected);
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
            await connection.OpenAsync();
            CloseConnectionAfter(3000, connection);

            var command = connection.CreateCommand();
            command.CommandText = "info";
            await command.ExecuteAsync();
            command.CommandText = "log-updates restart";
            await command.ExecuteAsync();

            var reader = connection.CreateReader();
            try
            {
               while (await reader.ReadAsync())
               {
                  Console.WriteLine(reader.Message);
               }
            }
            catch (Exception ex)
            {
               Assert.IsFalse(connection.Connected);
               Console.WriteLine(ex);
            }
         }
      }

      private static void CloseConnectionAfter(int millisecondsDelay, FahClientConnectionBase connection)
      {
         Task.Delay(millisecondsDelay).ContinueWith(t => connection.Close());
      }
   }
}
