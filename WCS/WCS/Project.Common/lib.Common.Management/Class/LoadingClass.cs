using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lib.Common.Management
{
    public static class LoadingClass
    {
        public static LoadingForm sf = null;

        public static Point ParentPosition;

        public static Thread LoadingThread;

        //public static bool isActive = false;

        public static void ShowSplashScreen()
        {
            if (sf == null)
            {
                sf = new LoadingForm();
                sf.StartPosition = FormStartPosition.Manual;
                sf.Left = ParentPosition.X;
                sf.Top = ParentPosition.Y;
                
                sf.ShowInTaskbar = false;
                //sf.BringToFront();
                //sf.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                //sf.TopMost = true;
                //sf.TopLevel = true;
                
                sf.ShowLoadingScreen();
                
            }
        }

        public static void CloseSplashScreen()
        {
            if (sf != null)
            {
                sf.CloseLoadingScreen();
                sf = null;
            }
        }
    }
    
}
