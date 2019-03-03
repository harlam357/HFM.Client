
using System;
using System.IO;
using System.Threading.Tasks;

namespace HFM.Client
{
   public abstract class FahClientDataWriter
   {
      public abstract void Write(byte[] buffer);

      public abstract Task WriteAsync(byte[] buffer);

      public static FahClientDataWriter Create(Stream stream)
      {
         return new FahClientStreamWriter(stream);
      }

      private class FahClientStreamWriter : FahClientDataWriter
      {
         private readonly Stream _stream;

         public FahClientStreamWriter(Stream stream)
         {
            _stream = stream ?? throw new ArgumentNullException(nameof(stream));
         }

         public override void Write(byte[] buffer)
         {
            _stream.Write(buffer, 0, buffer.Length);
         }

         public override async Task WriteAsync(byte[] buffer)
         {
            await _stream.WriteAsync(buffer, 0, buffer.Length);
         }
      }
   }
}