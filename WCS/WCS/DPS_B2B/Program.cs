using System;
using System.Linq;
using System.Windows.Forms;

namespace DPS_B2B
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            lib.Common.Log.LogFile.Project = "DPS";

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Application.Run(new DPS_B2B.MainForm());
            }
            else
            {
                Application.Run(new DPS_B2B.MainForm());
                //Application.Run(new DPS_B2B.LoginForm());
            }

        }
    }
}