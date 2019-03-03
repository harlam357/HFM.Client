
using System;
using System.Text;
using System.Threading.Tasks;

using NUnit.Framework;

namespace HFM.Client
{
   [TestFixture]
   public class FahClientCommandTests
   {
      [Test]
      public void FahClientCommand_ExecuteThrowsInvalidOperationExceptionWhenConnectionIsNotConnected()
      {
         // Arrange
         var command = new FahClientCommand(new FahClientConnection("foo", 2000));
         // Act & Assert
         Assert.Throws<InvalidOperationException>(() => command.Execute());
      }

      [Test]
      public void FahClientCommand_ExecuteAsyncThrowsInvalidOperationExceptionWhenConnectionIsNotConnected()
      {
         // Arrange
         var command = new FahClientCommand(new FahClientConnection("foo", 2000));
         // Act & Assert
         Assert.ThrowsAsync<InvalidOperationException>(() => command.ExecuteAsync());
      }

      [Test]
      public void FahClientCommand_ExecuteWritesCommandTextToConnectionDataWriter()
      {
         // Arrange
         var dataWriter = new MockFahClientDataWriter();
         var connection = new MockFahClientConnection(dataWriter);
         connection.Open();
         var command = new FahClientCommand(connection, "command text");
         // Act
         int bytesWritten = command.Execute();
         // Assert
         Assert.AreEqual(13, bytesWritten);
         Assert.AreEqual("command text\n", Encoding.ASCII.GetString(dataWriter.Buffer));
      }

      [Test]
      public async Task FahClientCommand_ExecuteAsyncWritesCommandTextToConnectionDataWriter()
      {
         // Arrange
         var dataWriter = new MockFahClientDataWriter();
         var connection = new MockFahClientConnection(dataWriter);
         connection.Open();
         var command = new FahClientCommand(connection, "command text");
         // Act
         int bytesWritten = await command.ExecuteAsync();
         // Assert
         Assert.AreEqual(13, bytesWritten);
         Assert.AreEqual("command text\n", Encoding.ASCII.GetString(dataWriter.Buffer));
      }

      private class MockFahClientConnection : FahClientConnection
      {
         private bool _connected;

         public override bool Connected => _connected;
         
         public MockFahClientConnection(FahClientDataWriter dataWriter) 
            : base("foo", 2000)
         {
            _dataWriter = dataWriter;
         }

         public override void Open()
         {
            _connected = true;
         }

         private readonly FahClientDataWriter _dataWriter;

         public override FahClientDataWriter GetDataWriter()
         {
            return _dataWriter;
         }
      }

      private class MockFahClientDataWriter : FahClientDataWriter
      {
         public byte[] Buffer { get; private set; }

         public override void Write(byte[] buffer)
         {
            Buffer = buffer;
         }

         public override Task WriteAsync(byte[] buffer)
         {
            Buffer = buffer;
            return Task.FromResult(0);
         }
      }
   }
}
