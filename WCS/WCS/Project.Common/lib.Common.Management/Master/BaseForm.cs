using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using bowoo.Lib;
using bowoo.Lib.DataBase;
using lib.Common;
using lib.Common.Management;
using System.Threading;
using bowoo.Framework.common;
using System.Net.NetworkInformation;
using System.Net;
using System.Diagnostics;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Data;


namespace lib.Common.Management
{
    public class BaseForm : Telerik.WinControls.UI.RadForm
    {
        public string qry = string.Empty;
        static public bowoo.Lib.DataBase.SqlQueryExecuter DBUtil = bowoo.Lib.DataBase.SqlQueryExecuter.getInstance();
        static public lib.Common.Management.MessageForm bowooMessageBox = new lib.Common.Management.MessageForm();
        static public lib.Common.Management.MessageConfirmForm bowooConfirmBox = new lib.Common.Management.MessageConfirmForm();
        static public lib.Common.Management.MessageRedConfirmForm bowooRedConfirmBox = new lib.Common.Management.MessageRedConfirmForm();
        static public lib.Common.Management.LoadingForm LoadingPopup = new lib.Common.Management.LoadingForm();
        static public lib.Common.Management.ProgressForm ProgressPopup = new lib.Common.Management.ProgressForm();
        static public lib.Common.Management.ProgressFormW ProgressPopupW = new lib.Common.Management.ProgressFormW();
        static public lib.Common.Management.LanguagePack LanguagePack = new lib.Common.Management.LanguagePack();
        static public TripleDESCrypto.CryptoUtil ENC = new TripleDESCrypto.CryptoUtil();

        static public System.Drawing.Point MainPosition = new System.Drawing.Point(0, 0);
        static public string Brand = "";
        static public string WorkSeq = "";

        static public string FullPath = "";
        static public string ExePath = "";

        public int threadCallCount = 0;

        public string menuId = "";
        public string menuTitle = "";

        public static RadGridView SGrid = null;
        public static RadLabel SLabel = null;

        /// <summary>
        /// OnLoad Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        /// <summary>
        /// Set Visible True Loading Paenl
        /// </summary>
        public void ShowLoading()
        {

            if (LoadingClass.LoadingThread == null)
            {
                LoadingClass.ParentPosition = BaseForm.MainPosition;
                LoadingClass.LoadingThread = new System.Threading.Thread(new ThreadStart(LoadingClass.ShowSplashScreen));
            }
            if (!LoadingClass.LoadingThread.IsAlive)
            {
                LoadingClass.LoadingThread.IsBackground = true;
                LoadingClass.LoadingThread.Start();
            }
            while (LoadingClass.sf == null)
            {
                Thread.Sleep(50);
            }
        }

        /// <summary>
        /// Set Loading panel Visible status
        /// </summary>
        public void HideLoading()
        {
            if (LoadingClass.LoadingThread != null && LoadingClass.LoadingThread.IsAlive)
            {
                LoadingClass.CloseSplashScreen();
                LoadingClass.LoadingThread = null;
            }
        }

        public void getFilePath(string projectName, string fileName)
        {
            //if (System.Diagnostics.Debugger.IsAttached)
            //{
            //    FullPath = System.IO.Directory.GetParent(Application.StartupPath).Parent.Parent.FullName;
            //    ExePath = string.Format(@"{0}\{1}\bin\Release\{2}", FullPath, projectName, fileName);
            //}

            //else
            //{
            //    FullPath = Application.StartupPath;
            //    ExePath = string.Format(@"{0}\{1}", FullPath, fileName);
            //}


            FullPath = Application.StartupPath;
            ExePath = string.Format(@"{0}\{1}", FullPath, fileName);
        }

        public void makeLog(string _ButtonNM, bool _isSuccess, string _LogMessage)
        {
            //Process:LogIn, 성공여부:N, 아이디:GHOSKTJS, 메뉴 명, 버튼 명, 실패원인
            //아이디: ... , 메뉴명: , 버튼명: , 성공여부:, 로그:

            string logId = "아이디: " + bowoo.Framework.common.BaseEntity.sessSID + ", ";
            string logMenuId = "메뉴번호: " + menuId + ", ";
            string logMenuTitle = "메뉴명: " + menuTitle + ", ";
            string logButtonNM = string.IsNullOrEmpty(_ButtonNM) ? ", " : "버튼명: " + _ButtonNM + ", ";
            string isSuccess = "성공여부: " + (_isSuccess ? "Y" : "N") + ", ";
            string log = "로그: " + _LogMessage;

            string LogText = logId + logMenuId + logMenuTitle + logButtonNM + isSuccess + log;

            lib.Common.Log.LogFile.WriteLog(LogText);
        }


        public void makeErrLog(string _ButtonNM, bool _isSuccess, string _LogMessage)
        {
            //Process:LogIn, 성공여부:N, 아이디:GHOSKTJS, 메뉴 명, 버튼 명, 실패원인
            //아이디: ... , 메뉴명: , 버튼명: , 성공여부:, 로그:

            //string logId = "아이디: " + bowoo.Framework.common.BaseEntity.sessSID + ", ";
            //string logMenuId = "메뉴번호: " + menuId + ", ";
            //string logMenuTitle = "메뉴명: " + menuTitle + ", ";
            //string logButtonNM = string.IsNullOrEmpty(_ButtonNM) ? ", " : "버튼명: " + _ButtonNM + ", ";
            //string isSuccess = "성공여부: " + (_isSuccess ? "Y" : "N") + ", ";
            string log = "로그: " + _LogMessage;

            string LogText =  log;

            lib.Common.Log.LogFile.WriteLog(LogText);
        }

        [System.Runtime.InteropServices.DllImportAttribute("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        //인터넷 연결 확인
        public static bool IsConnectedToInternet()
        {
            int Desc;
            return InternetGetConnectedState(out Desc, 0);
        }
        //public static void initControlsRecursive(Control.ControlCollection coll)
        //{
        //    foreach (Control c in coll)
        //    {
        //        c.MouseClick += new MouseEventHandler(MessageConfirmForm_MouseClick);
        //        initControlsRecursive(c.Controls);
        //    }
        //}


        //static void MessageConfirmForm_MouseClick(object sender, MouseEventArgs e)
        //{
        //    if (LoadingClass.sf != null)
        //    {
        //        if (LoadingClass.sf.InvokeRequired)
        //        {
        //            LoadingClass.sf.Invoke(new MethodInvoker(() => { MessageConfirmForm_MouseClick(null, null); }));
        //        }
        //        else
        //        {
        //            LoadingClass.sf.Activate();
        //            LoadingClass.sf.Focus();
        //        }
        //    }
        //}
    }
}
