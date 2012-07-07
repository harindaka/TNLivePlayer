using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace TNLivePlayer
{
    internal class WinAPI
    {        
        [DllImport("user32.dll", EntryPoint = "SetForegroundWindow")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "ShowWindowAsync")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int cmdShow);
        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOW = 5;

        [DllImport("User32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string strClassName, string strWindowName);
        
        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int ProcessId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern int GetWindowTextLength(IntPtr hWnd);
                
        internal static void FocusWindow(IntPtr handle)
        {
            ShowWindowAsync(handle, SW_SHOW);

            if (!ShowWindowAsync(handle, SW_SHOWNORMAL))
                throw new Exception();

            if (!SetForegroundWindow(handle))
                throw new Exception();
        }

        internal static IntPtr GetWindowHandleByProcessIDAndWindowTitle(int processID, string windowTitle)
        {
            IntPtr h = IntPtr.Zero;
            int tid;
            int pid;
            
            do
            {
                pid = 0;
                h = FindWindowEx(IntPtr.Zero, h, null, null);
                tid = GetWindowThreadProcessId(h, out pid);
                if (pid == processID)
                {
                    int length = GetWindowTextLength(h);
                    StringBuilder sb = new StringBuilder(length + 1);
                    GetWindowText(h, sb, sb.Capacity);

                    if (sb.ToString().Equals(windowTitle))
                        return h;                    
                }

            } while (!h.Equals(IntPtr.Zero));

            return IntPtr.Zero;
        }
    }
}
