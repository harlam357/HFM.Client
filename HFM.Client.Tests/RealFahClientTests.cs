
using System;
using System.Diagnostics;
using System.Threading.Tasks;

using NUnit.Framework;

namespace HFM.Client
{
   [TestFixture]
   public class RealFahClientTests
   {
      [Test]
      [Category(TestCategoryNames.Integration)]
      public void FahClientConnection()
      {
         using (var connection = new FahClientConnection("192.168.1.188", 36330))
         {
            connection.Open();

            // ReSharper disable once UnusedVariable
            var closeConnectionLater = Task.Delay(5000).ContinueWith(t => connection.Close());

            var command = connection.CreateCommand();
            command.CommandText = "info";

            try
            {
               while (command.Execute() > 0)
               {
                  //System.Threading.Thread.Sleep(100);
               }
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex);
            }
         }
      }


      [Test]
      public void RealFahClient()
      {
         using (var connection = new FahClientConnection("192.168.1.188", 36330))
         {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = "info";
            command.Execute();
            command.CommandText = "log-updates restart";
            command.Execute();

            var reader = connection.CreateReader();
            reader.ReadTimeout = 5000;
            while (reader.Read())
            {
               Debug.WriteLine(reader.Message);
            }
         }
      }

      [Test]
      public async Task RealFahClient2()
      {
         using (var connection = new FahClientConnection("192.168.1.188", 36330))
         {
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "info";
            await command.ExecuteAsync();
            command.CommandText = "log-updates restart";
            await command.ExecuteAsync();

            var reader = connection.CreateReader();
            reader.ReadTimeout = 5000;
            while (await reader.ReadAsync())
            {
               Debug.WriteLine(reader.Message);
            }
         }
      }

      [Test]
      public void RealFahClient3()
      {
         using (var connection = new FahClientConnection("192.168.1.188", 36330))
         {
            connection.Open();

            // ReSharper disable once UnusedVariable
            var closeConnectionLater = Task.Delay(5000).ContinueWith(t => connection.Close());

            var command = connection.CreateCommand();
            command.CommandText = "info";
            command.Execute();
            command.CommandText = "log-updates restart";
            command.Execute();

            var reader = connection.CreateReader();
            reader.ReadTimeout = 60000;
            while (reader.Read())
            {
               Debug.WriteLine(reader.Message);
            }
         }
      }

      [Test]
      public async Task RealFahClient4()
      {
         using (var connection = new FahClientConnection("192.168.1.188", 36330))
         {
            await connection.OpenAsync();

            // ReSharper disable once UnusedVariable
            var closeConnectionLater = Task.Delay(5000).ContinueWith(t => connection.Close());

            var command = connection.CreateCommand();
            command.CommandText = "info";
            await command.ExecuteAsync();
            command.CommandText = "log-updates restart";
            await command.ExecuteAsync();

            var reader = connection.CreateReader();
            reader.ReadTimeout = 60000;
            while (await reader.ReadAsync())
            {
               Debug.WriteLine(reader.Message);
            }
         }
      }
   }
}
