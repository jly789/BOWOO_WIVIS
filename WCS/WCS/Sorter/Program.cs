using System;
using System.Linq;
using System.Windows.Forms;

namespace Sorter
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {


            // 프로그램 중복실행 방지
            System.Diagnostics.Process[] processes = null;
            // 실행중인 프로그램 이름 얻어오기.
            string strCurrentProcess = System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToUpper();
            processes = System.Diagnostics.Process.GetProcessesByName(strCurrentProcess);
            if (processes.Length > 1)
            {
                MessageBox.Show(string.Format("'{0}' 프로그램이 이미 실행중입니다.", System.Diagnostics.Process.GetCurrentProcess().ProcessName));
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            lib.Common.Log.LogFile.Project = "SMS";

           
            Application.Run(new Sorter.MainForm());

        }
    }
}