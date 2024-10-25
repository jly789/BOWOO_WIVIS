using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Sorer_Indicator_Contorl
{
    class LogWrite
    {
        static private object _lock = new object();
        public string GetDateTime()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + " : " + NowDate.Millisecond.ToString("000");
        }

        //매개변수 입력할 내용, 어플리케이션 실행 위치,폴더명,파일명
        public void LogWriter(String str, string applicationPath, string dir_path, string fileName)
        {
            lock (_lock)
            {
                string DirPath = dir_path;
                string FilePath = applicationPath + DirPath + fileName + DateTime.Today.ToString("yyyyMMdd") + ".log";
                string temp;
                DirectoryInfo di = new DirectoryInfo(DirPath);
                FileInfo fi = new FileInfo(FilePath);
                try
                {
                    if (di.Exists != true) Directory.CreateDirectory(DirPath);
                    //if (fi.Exists != true)
                    //{
                    //파일 페쓰뒤 t/f 는 true면 해당파일에 이어서 텍스트 쓰기, false이면 파일 덮어씌우기.
                    using (StreamWriter sw = new StreamWriter(FilePath, true))
                    {
                        temp = string.Format("[{0}] : {1}", GetDateTime(), str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                    //}
                    //else
                    //{
                    //    using (StreamWriter sw = File.AppendText(FilePath))
                    //    {
                    //        temp = string.Format("[{0}] : {1}", GetDateTime(), str);
                    //        sw.WriteLine(temp);
                    //        sw.Close();
                    //    }
                    //}
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                }
            }
        }
    }
}
