
using System;
using System.Windows.Forms;

namespace HFM.Client.Tool
{
    internal static class WinFormsThreadHelper
    {
        internal static void BeginInvokeOnUIThread<T>(this Control control, Action<T> action, T arg0)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(action, arg0);
                return;
            }

            action(arg0);
        }

    }
}
