using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices; // dll import하기 위함.

namespace DPS_B2B
{
    class Dll_import
    {

        //프로세스 ID를 받아오기 위한 메소드
        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        //ini 파일 읽기위한 임포트
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(String section, String key, String def, StringBuilder retVal, int size, String filePath);


            // 스탑워치 예제.
            //System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            //sw.Start();
            //sw.Stop();
       
    }
}
