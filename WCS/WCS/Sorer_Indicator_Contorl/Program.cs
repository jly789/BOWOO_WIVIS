using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorer_Indicator_Contorl
{
    static class Program
    {
        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
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
            Application.Run(new Action());
            //Application.Run(new StatusBoardForm());
            //Application.Run(new StatusBoard());
        }
    }
}
