
using System;
using System.Threading.Tasks;

using NUnit.Framework;

namespace HFM.Client
{
    [TestFixture]
    public class FahClientConnectionIntegrationTests
    {
        private const string Host = "192.168.1.199";
        private const int Port = 36330;

        [Test]
        [Category(TestCategoryNames.Integration)]
        public void FahClientConnection_WritesCommandsAndReadsMessageSynchronously()
        {
            using (var connection = new FahClientConnection(Host, Port))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText = "info";
                command.Execute();

                var reader = connection.CreateReader();
                if (reader.Read())
                {
                    Console.WriteLine(reader.Message);
                }

                command.CommandText = "log-updates restart";
                command.Execute();
                if (reader.Read())
                {
                    Console.WriteLine(reader.Message);
                }
            }
        }

        [Test]
        [Category(TestCategoryNames.Integration)]
        public async Task FahClientConnection_WritesCommandsAndReadsMessageAsynchronously()
        {
            using (var connection = new FahClientConnection(Host, Port))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "info";
                await command.ExecuteAsync();

                var reader = connection.CreateReader();
                if (await reader.ReadAsync())
                {
                    Console.WriteLine(reader.Message);
                }

                command.CommandText = "log-updates restart";
                await command.ExecuteAsync();
                if (await reader.ReadAsync())
                {
                    Console.WriteLine(reader.Message);
                }
            }
        }

        [Test]
        [Category(TestCategoryNames.Integration)]
        public void FahClientConnection_SimulateHFMUpdateCommandsSynchronously()
        {
            using (var connection = new FahClientConnection(Host, Port))
            {
                connection.Open();

                connection.CreateCommand("log-updates restart").Execute();
                connection.CreateCommand("updates add 0 60 $heartbeat").Execute();
                connection.CreateCommand("updates add 1 1 $info").Execute();
                connection.CreateCommand("updates add 2 1 $(options -a)").Execute();
                connection.CreateCommand("updates add 3 1 $slot-info").Execute();

                var reader = connection.CreateReader();
                for (int i = 0; i < 10 && reader.Read(); i++)
                {
                    Console.WriteLine(reader.Message);
                }
            }
        }

        [Test]
        [Category(TestCategoryNames.Integration)]
        public async Task FahClientConnection_SimulateHFMUpdateCommandsAsynchronously()
        {
            using (var connection = new FahClientConnection(Host, Port))
            {
                await connection.OpenAsync();

                await connection.CreateCommand("log-updates restart").ExecuteAsync();
                await connection.CreateCommand("updates add 0 60 $heartbeat").ExecuteAsync();
                await connection.CreateCommand("updates add 1 1 $info").ExecuteAsync();
                await connection.CreateCommand("updates add 2 1 $(options -a)").ExecuteAsync();
                await connection.CreateCommand("updates add 3 1 $slot-info").ExecuteAsync();

                var reader = connection.CreateReader();
                for (int i = 0; i < 10 && await reader.ReadAsync(); i++)
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

        private static void CloseConnectionAfter(int millisecondsDelay, FahClientConnection connection) =>
            Task.Delay(millisecondsDelay).ContinueWith(_ => connection.Close(), TaskScheduler.Current);
    }
}
