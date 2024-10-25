using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Telerik.WinControls.UI;
using System.Threading;
using bowoo.Framework.common;
using System.Data.OleDb;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Net;
using System.Diagnostics;
using System.Configuration;
using System.Text.RegularExpressions;

namespace lib.Common.Management
{
    
    public partial class BaseControl : UserControl
    {

        public string qry = string.Empty;
        static public bowoo.Lib.DataBase.SqlQueryExecuter DBUtil = DBUtil = bowoo.Lib.DataBase.SqlQueryExecuter.getInstance();
        static public lib.Common.Management.MessageForm bowooMessageBox = new lib.Common.Management.MessageForm();
        static public lib.Common.Management.MessageConfirmForm bowooConfirmBox = new lib.Common.Management.MessageConfirmForm();
        static public lib.Common.Management.MessageRedConfirmForm bowooRedConfirmBox = new lib.Common.Management.MessageRedConfirmForm();
        static public lib.Common.Management.ProgressForm ProgressPopup = new lib.Common.Management.ProgressForm();
        static public lib.Common.Management.ProgressFormW ProgressPopupW = new lib.Common.Management.ProgressFormW();
        static public lib.Common.Management.LoadingForm LoadingPopup = new lib.Common.Management.LoadingForm();
        static public lib.Common.Management.LanguagePack LanguagePack = new lib.Common.Management.LanguagePack();
        static public lib.Common.Management.BowooXml bowooXml = new lib.Common.Management.BowooXml();

        public lib.Common.Management.UC01_GridView childGrid;
        public static Configuration appConfig = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
        
        public int leftSelectedIndex = -1;
        public int rightSelectedIndex = -1;

        public string menuId = "";
        public string menuTitle = "";

        public BaseControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// OnLoad Event - (1) Check NetWork (2) Add Click Event for all GridView controls 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.Controls.Find("GridViewData", true).FirstOrDefault() != null)
            {
                this.Controls.Find("GridViewData", true)[0].MouseDown += BaseControl_MouseDown;
                this.Controls.Find("GridViewData", true)[0].Paint += BaseControl_Paint;
            }
        }

        public virtual void PageSearch()
        {        
  
        }

        public virtual void StopTimer()
        {

        }

        /// <summary>
        /// Set Control event for Print Function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BaseControl_MouseDown(object sender, EventArgs e)
        {
            if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
            {
                BaseForm.SGrid = this.Controls.Find("GridViewData", true) != null ? (this.Controls.Find("GridViewData", true)[0] as RadGridView) : null;
                BaseForm.SLabel = (this.Controls.Find("GridViewData", true)[0].Parent.Parent.Parent.Controls.Find("radLblGridTitle", true)) != null ?
                                 ((this.Controls.Find("GridViewData", true)[0].Parent.Parent.Parent.Controls.Find("radLblGridTitle", true))[0] as RadLabel) : null;

                if (this.Parent.Parent != null)
                {
                    if (!BaseEntity.SelectGridName.Equals(string.Empty) && !this.Name.Equals(BaseEntity.sessMenuName) && this.Parent.Parent.Controls.Find(BaseEntity.SelectGridName, true).Length > 0)
                    {
                        UserControl Control = this.Parent.Parent.Controls.Find(BaseEntity.SelectGridName, true)[0] as UserControl;

                        Control.BackColor = Color.Transparent;

                        Control[] pnlctrl = this.Parent.Parent.Controls.Find(BaseEntity.SelectGridName + "_pnl", true);
                        if (pnlctrl.Length > 0)
                        {
                            pnlctrl[0].BackColor = Color.Transparent;
                        }
                    }

                    if (this.Parent.Controls.Find(this.Name, true).Length > 0 && !this.Name.Equals(BaseEntity.sessMenuName))
                    {
                        BaseEntity.SelectGridName = this.Name;
                        UserControl Ctrl = (this.Parent.Controls.Find(BaseEntity.SelectGridName, true)[0] as UserControl);

                        Ctrl.BackColor = Color.FromArgb(178, 235, 244);

                        Control[] pnlctrl = this.Parent.Parent.Controls.Find(BaseEntity.SelectGridName + "_pnl", true);
                        if (pnlctrl.Length > 0)
                        {
                            pnlctrl[0].BackColor = Color.FromArgb(178, 235, 244);
                        }
                    }
                }
            }
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
        /// Set Loading panel Parameters value
        /// </summary>
        public void HideLoading()
        {
            if (LoadingClass.LoadingThread != null && LoadingClass.LoadingThread.IsAlive)
            {
                LoadingClass.CloseSplashScreen();
                LoadingClass.LoadingThread = null;
            }
        }

        /// <summary>
        /// Set Visible False Laoding Panel 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int PrintFormSeq = 0;
            bool existPrintForm = false;

            foreach (Form itemForm in Application.OpenForms)
            {
                if (itemForm.Name.Equals("PrintPopup"))
                {
                    existPrintForm = true;
                    break;
                }

                PrintFormSeq++;
            }

            if (existPrintForm && !BaseEntity.IsFiredPrint) (Application.OpenForms[PrintFormSeq] as lib.Common.Management.PrintPopup).ClosePopup();
        }

        /// <summary>
        /// Set Visible False Laoding Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseControl_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);

            int PrintFormSeq = 0;
            bool existPrintForm = false;

            foreach (Form itemForm in Application.OpenForms)
            {
                if (itemForm.Name.Equals("PrintPopup"))
                {
                    existPrintForm = true;
                    break;
                }

                PrintFormSeq++;
            }

            if (existPrintForm && !BaseEntity.IsFiredPrint) (Application.OpenForms[PrintFormSeq] as lib.Common.Management.PrintPopup).ClosePopup();
        }

        /// <summary>
        /// csv 파일 -> 데이터 테이블
        /// </summary>
        /// <param name="path"></param>
        /// <param name="isFirstRowHeader"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
        {
            //파일 열고 있으면 싫패하는 문제 때문에 복사하여 CSV접근
    
            string header = isFirstRowHeader ? "Yes" : "No";

            string _pathOnly = Path.GetDirectoryName(path);
            string _fileName = Path.GetFileNameWithoutExtension(path);
            string _extName = Path.GetExtension(path);

            string copyPath = System.IO.Path.Combine(_pathOnly, _fileName + "_TEMP" + _extName);
            //string copyPath = System.IO.Path.Combine(_pathOnly, _fileName +  _extName);

            System.IO.File.Copy(path, copyPath, true);

            string pathOnly = Path.GetDirectoryName(copyPath);
            string fileName = Path.GetFileName(copyPath);

            string sql = @"SELECT * FROM [" + fileName + "]";


            using (OleDbConnection connection = new OleDbConnection(
                    @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + _pathOnly +"\\" + fileName + ";" +
                    "Mode=ReadWrite|Share Deny None;" +
                    "Extended Properties='Excel 12.0 Xml; HDR=NO; IMEX=1';" +
                    "Persist Security Info=False;"))
            //using (OleDbConnection connection = new OleDbConnection(
            //        @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + pathOnly +
            //        ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                adapter.Fill(dataTable);
                
                if (System.IO.File.Exists(copyPath))
                {
                    System.IO.File.Delete(copyPath);
                }
                
                return dataTable;
            }
        }

        /// <summary>
        /// Generate CSV File From DataTable
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="filePath"></param>
        /// <param name="isExistsHeader"></param>
        public void ConvertToCsvFileFromDataTable (DataTable dt, string filePath, bool isExistsHeader)
        {
            StringBuilder sb = new StringBuilder();

            if (isExistsHeader)
            {
                IEnumerable<string> columnNames = dt.Columns.Cast<DataColumn>().Select(column => column.ColumnName);

                sb.AppendLine(string.Join(",", columnNames));
            }

            foreach (DataRow row in dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());

                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(filePath, sb.ToString());
        }

        /// <summary>
        /// Generate CSV File From GridView  
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="filePath"></param>
        /// <param name="isExistsHeader"></param>
        public void ConvertToCsvFileFromGridView(RadGridView gridView, string filePath, bool isExistsHeader)
        {
            StringBuilder sb = new StringBuilder();

            if (isExistsHeader)
            {
                IEnumerable<string> columnNames = from col in gridView.Columns
                                                  where (col.Tag as lib.Common.Management.ParamType).File
                                                  select col.FieldName;

                sb.AppendLine(string.Join(",", columnNames));
            }

            ProgressPopupW = new lib.Common.Management.ProgressFormW();
            ProgressPopupW.progressBar1.Maximum = gridView.Rows.Count;
            ProgressPopupW.progressBar1.Step = 1;
            //ProgressPopupW.SetLocation(Das_B2B.MainForm.MainPosition);
            ProgressPopupW.BringToFront();
            ProgressPopupW.Show();

            for (int i = 0; i < gridView.Rows.Count; i++)
            {

                List<string> fields = new List<string>();
                for (int j = 0; j < gridView.Columns.Count; j++)
                {
                    if ((gridView.Columns[j].Tag as lib.Common.Management.ParamType).File)
                    {
                        fields.Add(gridView.Rows[i].Cells[j].Value.ToString());
                    }
                }

                sb.AppendLine(string.Join(",", fields));
                ProgressPopupW.progressBar1.PerformStep();
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.Default);

            ProgressPopupW.Close();
        }

        public void makeLog(string _ButtonNM, bool _isSuccess, string _LogMessage)
        {
            //Process:LogIn, 성공여부:N, 아이디:GHOSKTJS, 메뉴 명, 버튼 명, 실패원인
            //아이디: ... , 메뉴명: , 버튼명: , 성공여부:, 로그:

            string logId =  "아이디: "+bowoo.Framework.common.BaseEntity.sessSID + ", ";
            string logMenuId = "메뉴번호: " + menuId + ", ";
            string logMenuTitle = "메뉴명: " + menuTitle + ", ";
            string logButtonNM = string.IsNullOrEmpty(_ButtonNM) ? ", ": "버튼명: " + _ButtonNM+ ", ";
            string isSuccess = "성공여부: " + (_isSuccess ? "Y" : "N") + ", ";
            string log = "로그: " + _LogMessage ;

            string LogText = logId + logMenuId + logMenuTitle + logButtonNM + isSuccess + log;

            lib.Common.Log.LogFile.WriteLog(LogText);
        }
    }
}