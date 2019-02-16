
using System;
using System.Threading.Tasks;

namespace HFM.Client.Internal
{
   internal static class TaskExtensions
   {
      internal static async Task WithTimeout(this Task task, int timeout, string message)
      {
         if (task == await Task.WhenAny(task, Task.Delay(timeout)))
         {
            return;
         }
         throw new TimeoutException(message);
      }
   }
}
