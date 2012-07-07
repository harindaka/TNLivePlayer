using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace TNLivePlayer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AssemblyInfo.Initialize();

            Process prevProcess = Program.GetPrevInstance();

            if (prevProcess == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Player mainForm = new Player();
                mainForm.Text = AssemblyInfo.Current.AssemblyProduct;
                Application.Run(mainForm);
            }
            else
            {
                try
                {
                    IntPtr prevProcWinHandle = WinAPI.GetWindowHandleByProcessIDAndWindowTitle(prevProcess.Id, AssemblyInfo.Current.AssemblyProduct);
                    WinAPI.FocusWindow(prevProcWinHandle);
                }
                catch (Exception)
                {
                    MessageBox.Show("This application is already running.", AssemblyInfo.Current.AssemblyProduct);
                }
            }
        }

        private static Process GetPrevInstance()
        {
            Process currentProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currentProcess.ProcessName);
            if (processes.Length > 1)
            {
                foreach (Process p in processes)
                {
                    if (p.Id != currentProcess.Id)
                        return p;
                }
            }

            return null;
        }
    }
}
