using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using bowoo.Framework.common;
using System.Runtime.InteropServices;
using System.Resources;
using System.Threading;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;
//using Telerik.WinControls.RichTextBox.Model;
//using Telerik.WinControls.RichTextBox;
using System.IO;
using Telerik.WinControls.Primitives;
using Telerik.WinControls.RichTextBox.Winforms;

namespace Sorter
{
    public partial class MainForm : lib.Common.Management.BaseForm
    {
        //public static RadGridView SGrid = null;
        //public static RadLabel SLabel = null;
        //public static String Brand, WorkSeq = string.Empty;

        private static DataTable dtMenu;
        //private static Point mainPosition;
        private static int CallCount = 0;
        private static lib.Common.SuspendDrawingUpdate suspendLayout;

        /// <summary>
        /// Set Position Form
        /// </summary>
        //[DefaultValue(0)]
        //public static Point MainPosition
        //{
        //    get { return mainPosition; }
        //    set
        //    {
        //        mainPosition = value;
        //    }
        //}

        public MainForm()
        {
            InitializeComponent();
            InitCommonControls();


            //lib.Common.Management.WivisOracle wivisOracle = new lib.Common.Management.WivisOracle(System.Configuration.ConfigurationManager.AppSettings["OracleDatabaseip"].ToString());
            //wivisOracle.invoiceSerch("D1002"
            //                        , "1352"
            //                        , "MJ0060"); //송장 번호 검색

        }

        /// <summary>
        /// init All Controls
        /// </summary>
        private void InitCommonControls()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
        
            //for Test 
            //BaseEntity.sessSName = "개발자";
            //BaseEntity.sessLv = 0;
            //BaseEntity.sessSID = "SA";

            if (string.IsNullOrEmpty(BaseEntity.sessSName)) BaseEntity.sessSName = "개발자";
            if (string.IsNullOrEmpty(BaseEntity.sessSID)) BaseEntity.sessSID = "SA";

            BaseEntity.sessMenuName = string.Empty;

            setinitvalue();
            MakeSessionInq();
        }

        #region Set Value for Controls

        /// <summary>
        /// Set init Value all Controls
        /// </summary>
        private void setinitvalue()
        {
            setCalendar();
            setBrand();
            setUserInfo();
            setWorkSeq();
            setMenu();
            setNews();
            setLanguage();
        }

        private void setLanguage()
        {
            radLabelWrkdate.Text = LanguagePack.Translate(radLabelWrkdate.Text);
            radLabelBrand.Text = LanguagePack.Translate(radLabelBrand.Text);
            radLabelWrkSeq.Text = LanguagePack.Translate(radLabelWrkSeq.Text);
            //radLabelPrint.Text = LanguagePack.Translate(radLabelPrint.Text);
        }

        /// <summary>
        /// Set News - Main Bottom
        /// </summary>
        private void setNews()
        {
            try
            {
                if (!IsConnectedToInternet())
                {
                    return;
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://www.bowoosystem.com/");
                WebResponse response = request.GetResponse();
                var res = ((HttpWebResponse)response).StatusDescription;
                StreamReader reader = new StreamReader(response.GetResponseStream());

                DataTable dtInfo = new DataTable();
                dtInfo.Columns.Add("URL", typeof(string));
                dtInfo.Columns.Add("INFO", typeof(string));
                dtInfo.Columns.Add("DAY", typeof(string));

                bool IsNew = false;

                while (!reader.EndOfStream)
                {
                    DataRow row = dtInfo.NewRow();

                    string s = reader.ReadLine();
                    s = s.Replace("\t", string.Empty);

                    if (s.IndexOf("notice_list") > -1)
                    {
                        IsNew = true;
                    }

                    if (IsNew)
                    {
                        if (s.IndexOf("<li>") > -1)
                        {
                            s = s.Replace("<li>", string.Empty).Replace("</li>", string.Empty);

                            if (s.IndexOf("<a href=") > -1)
                            {
                                row["URL"] = s.IndexOf("#board") > 0 ? s.Substring(s.IndexOf("<a href=") + 9, s.IndexOf("#board") - 3) : string.Empty;
                                row["INFO"] = s.IndexOf("#board") > 0 && s.IndexOf("<span") > 0 ? s.Substring(s.IndexOf("#board") + 8, (s.IndexOf("<span") - s.IndexOf("#board") - 12)) : string.Empty;
                                row["DAY"] = s.IndexOf("<span class=") > 0 && s.IndexOf("</span>") > 0 ? s.Substring(s.IndexOf("<span class=") + 18, s.IndexOf("</span>") - (s.IndexOf("<span class=") + 18)) : string.Empty;

                                dtInfo.Rows.Add(row);
                                dtInfo.AcceptChanges();
                            }
                        }
                    }

                    if (IsNew && s.IndexOf("</div>") > -1)
                    {
                        IsNew = false;
                    }
                }

                reader.Close();

                foreach (DataRow item in dtInfo.Rows)
                {
                    RadLabelElement news = new RadLabelElement();
                    news.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    news.Name = item["URL"].ToString().Trim();
                    news.Text = string.Format("▶ {0}     [{1}]", item["INFO"].ToString(), item["DAY"].ToString());
                    news.ToolTipText = item["INFO"].ToString();

                    news.MouseHover += news_MouseHover;
                    news.MouseLeave += news_MouseLeave;
                    news.Click += news_Click;

                    this.radRotatorBottomNews.Items.Add(news);
                }

                radRotatorBottomNews.Start();
                radRotatorBottomNews.Interval = 5000;
                radRotatorBottomNews.AnimationFrames = 10;
                radRotatorBottomNews.ShouldStopOnMouseOver = false;
            }
            catch (Exception ex)
            {
                lib.Common.Log.LogFile.WriteLog(ex.Message);   
            }           
        }

        /// <summary>
        /// Set Calendar - Main Top
        /// </summary>
        private void setCalendar()
        {

            if (System.Diagnostics.Debugger.IsAttached)
            {
                //    this.calendar1.DefaultDate = "2022-04-04";
                    this.calendar1.DefaultDate = "2023-12-26";
            }
            else
            {
                this.calendar1.DefaultDate = DateTime.Now.ToShortDateString();
            }
            //else
            //{
            //    this.calendar1.DefaultDate = DateTime.Now.ToShortDateString();
            //}
            //this.UsrCalWork.DefaultDate = DateTime.Now.ToShortDateString();
            //this.UsrCalWork.DefaultDate = "2020-12-22";
            (this.calendar1.Controls.Find("radDatePickerCal", true)[0] as RadDateTimePicker).ValueChanged += Change_WorkSeq;
        }

        /// <summary>
        /// Set Menu - Main Top
        /// </summary>
        private void setMenu()
        {
            //Menu
            Dictionary<string, object> paramMenu = new Dictionary<string, object>();
            paramMenu["@USR_LV"] = BaseEntity.sessLv;

            dtMenu = DBUtil.ExecuteDataSet("SP_GET_MENU_INFO", paramMenu, CommandType.StoredProcedure).Tables[0];

            foreach (DataRow item in dtMenu.Rows)
            {
                RadMenuItem menuitem = new RadMenuItem();
                menuitem.AutoSize = false;
                menuitem.Bounds = new System.Drawing.Rectangle(0, 0, radPanelLeftMenu.Size.Width, radPanelLeftMenu.Size.Height / dtMenu.Rows.Count);
                menuitem.Click += menuitem_Click;
                //menuitem.ForeColor = System.Drawing.Color.DimGray;
                menuitem.BorderThickness = new System.Windows.Forms.Padding(2);
                //menuitem.BorderPrimitive.BorderThickness = new Padding(1);
                //menuitem.BorderPrimitive.BottomColor = System.Drawing.Color.Black;
                menuitem.Name = item["URL"].ToString();
                menuitem.Text = item["NAME"].ToString();
                menuitem.Text = "  " + LanguagePack.Translate(menuitem.Text);
                menuitem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                menuitem.Padding = new Padding(10, 0, 0, 0);

                //if (Properties.Resources.ResourceManager.GetObject(item["URL"].ToString()) != null)
                //{
                //    menuitem.Image = ((System.Drawing.Icon)(Properties.Resources.ResourceManager.GetObject(item["URL"].ToString()))).ToBitmap();
                //}

                if (Properties.Resources.ResourceManager.GetObject(item["URL"].ToString()) != null)
                {
                    menuitem.Image = ((System.Drawing.Bitmap)(Properties.Resources.ResourceManager.GetObject(item["URL"].ToString())));
                }


                this.radMenu.Items.Add(menuitem);
            }
        }

        /// <summary>
        /// Set WorkingSequence - Main Top
        /// </summary>
        public void setWorkSeq()
        {
            if(this.radddlwrkseq.Items.Count>0) this.radddlwrkseq.Items.Clear();

            Dictionary<string, object> paramWorkSeq = new Dictionary<string, object>();
            paramWorkSeq["@BIZ_DAY"] = this.calendar1.DefaultDate.Replace("-", string.Empty);
            paramWorkSeq["@BRAND_CD"] = this.radDropDownListBrand.SelectedValue;

            //Work Seq
            DataTable dtWrkSeq = DBUtil.ExecuteDataSet("SP_GET_WORK_ORDER", paramWorkSeq, CommandType.StoredProcedure).Tables[0];

            //this.radddlwrkseq.Items.Add("000");

            foreach (DataRow item in dtWrkSeq.Rows)
            {
                this.radddlwrkseq.Items.Add(new RadListDataItem(item["BATCH"].ToString(), item["BATCH"].ToString()));
            }

            this.radddlwrkseq.DropDownListElement.Enabled = this.radddlwrkseq.Items.Count == 0 ? false : true;
            this.radddlwrkseq.EnableMouseWheel = false;

            if (this.radddlwrkseq.Items.Count > 0)
            {
                this.radddlwrkseq.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Set UserInformation - Main Top
        /// </summary>
        private void setUserInfo()
        {
            //UserInfo
            this.radLabelUserName.Text = string.Format(LanguagePack.Translate("{0}님 환영합니다"), LanguagePack.Translate(BaseEntity.sessSName));
        }

        /// <summary>
        /// Set Brand - Main Top
        /// </summary>
        private void setBrand()
        {
            //Brand
            Dictionary<string, object> parambrand = new Dictionary<string, object>();
            parambrand["@BRAND_CD"] = string.Empty;
            parambrand["@BRAND_NM"] = string.Empty;

            DataTable dtBrand = DBUtil.ExecuteDataSet("SP_GET_BRAND_INFO", parambrand, CommandType.StoredProcedure).Tables[0];

            foreach (DataRow item in dtBrand.Rows)
            {
                this.radDropDownListBrand.Items.Add(new RadListDataItem(string.Format("{0} : {1}", item["BRAND_CD"].ToString(), item["BRAND_NM"].ToString()), item["BRAND_CD"].ToString()));
            }

            this.radDropDownListBrand.Items.Insert(0, new RadListDataItem("ALL", "ALL"));

            if (this.radDropDownListBrand.Items.Count > 1)
            {
                //this.radDropDownListBrand.SelectedIndex = 1;
                this.radDropDownListBrand.SelectedIndex = 3;
            }

            this.radDropDownListBrand.EnableMouseWheel = false;
        }

        /// <summary>
        /// Setting Session Value
        /// </summary>
        public void MakeSessionInq()
        {
            string sessInq = string.Empty;

            sessInq += string.Format("user={0}", BaseEntity.sessSID);
            sessInq += string.Format(";#auth={0}", BaseEntity.sessLv);
            sessInq += string.Format(";#bizday={0}", this.calendar1.DefaultDate.Replace("-",string.Empty));
            sessInq += string.Format(";#brand={0}", this.radDropDownListBrand.SelectedValue);
            sessInq += string.Format(";#wrkseq={0}", this.radddlwrkseq.SelectedValue);

            BaseEntity.sessInq = sessInq;
            Brand = this.radDropDownListBrand.Items.Count > 0 ? this.radDropDownListBrand.SelectedText.ToString() : string.Empty;

            Brand = this.radDropDownListBrand.SelectedItem.Text.IndexOf(":") == -1 ? this.radDropDownListBrand.SelectedItem.Text :
                (this.radDropDownListBrand.SelectedItem.Text.Substring(this.radDropDownListBrand.SelectedItem.Text.IndexOf(":") + 2, this.radDropDownListBrand.SelectedItem.Text.Length - this.radDropDownListBrand.SelectedItem.Text.IndexOf(":") - 2));

            WorkSeq = this.radddlwrkseq.Items.Count > 0 ? this.radddlwrkseq.SelectedValue.ToString() : string.Empty;
        }

        #endregion

        #region Set Event for Controls

        /// <summary>
        /// For Get Select MenuItem Control
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void menuitem_Click(object sender, EventArgs e)
        {
            this.radMenu.Enabled = false;

            BaseEntity.visibleLoadingCount = 0;
            SGrid = null;
            SLabel = null;

            RadMenuItem smenu = (sender as RadMenuItem);
            this.radLabelMenuNavigation.Text = string.Format("SMS > {0} ", smenu.Text);
            BaseEntity.sessMenuName = smenu.Name;

            SetMenuControl();

            this.radMenu.Enabled = true;
        }

        /// <summary>
        /// For Set SMS Button Click 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButtonSMS_Click(object sender, EventArgs e)
        {
            BaseEntity.sessMenuName = string.Empty;

            foreach (DataRow item in dtMenu.Rows)
            {
                foreach (UserControl itemControl in this.radPanelContents.Controls)
                {
                    itemControl.Visible = false;
                }
            }
        }

        /// <summary>
        /// Set Menu controls
        /// </summary>
        private void SetMenuControl()
        {
            if (suspendLayout == null)
                suspendLayout = new lib.Common.SuspendDrawingUpdate(this.radPanelContents);

            bool ExistMenu = false;
            ExistMenu = this.radPanelContents.Controls.Find(BaseEntity.sessMenuName, true).Length > 0 ? true : false;
            Type t = Type.GetType("Sorter.Pages." + BaseEntity.sessMenuName);
            if (ExistMenu)
            {

                UserControl TT = (UserControl)this.radPanelContents.Controls.Find(BaseEntity.sessMenuName, true)[0];
                SetMenuData((UserControl)this.radPanelContents.Controls.Find(BaseEntity.sessMenuName, true)[0], BaseEntity.sessMenuName, true);
            }
            else
            {
                if (t != null)
                {
                    UserControl MenuControl = (UserControl)Activator.CreateInstance(t);
                    this.radPanelContents.Controls.Add(MenuControl);
                    (MenuControl as lib.Common.Management.BaseControl).leftSelectedIndex = -1;
                    (MenuControl as lib.Common.Management.BaseControl).rightSelectedIndex = -1;
                    (MenuControl as lib.Common.Management.BaseControl).PageSearch();
                    MenuControl.BringToFront();
                }
                else if(BaseEntity.sessMenuName == "S012_DAS" || BaseEntity.sessMenuName == "S013_DASTEST")
                {
                    try
                    {
                        //Kill Process DAS Controller
                        //foreach (Process process in System.Diagnostics.Process.GetProcesses())
                        //{
                        //    if (process.ProcessName.StartsWith("Sorer_Indicator_Contorl"))
                        //    {
                        //        process.Kill();
                        //    }
                        //}
                        string DpsFullPath = Application.StartupPath;
                        string ExePath = string.Empty;

                        if (BaseEntity.sessMenuName == "S012_DAS")
                        {
                            ExePath = DpsFullPath + @"\contolor\Sorer_Indicator_Contorl.exe";
                        }
                        else if (BaseEntity.sessMenuName == "S013_DASTEST")
                        {
                            ExePath = DpsFullPath + @"\test_contolor\BOWOO_DPS_2F.exe";
                            //ExePath = DpsFullPath + @"\control\DPS_Controller.exe";
                        }


                        var Process = new Process
                        {
                            StartInfo =
                                    {
                                        FileName = ExePath,
                                        Arguments = string.Format("SMenu={0};PositionX={1};PositionY={2};Bizday={3};", BaseEntity.sessMenuName.Equals("DPS008") ? "DPS" : "TEST", this.Location.X.ToString(), this.Location.Y.ToString(), this.calendar1.DefaultDate)
                                    }
                        };

                        Process.Start();
                    }
                    catch (Exception ex)
                    {
                        bowooMessageBox.Show("DPS 제어형 프로그램을 실행하는데 문제가 있습니다. 관리자에게 문의하십시오.");
                        lib.Common.Log.LogFile.WriteLog(ex.Message);
                    }
                }

            }

            if (suspendLayout != null)
            {
                suspendLayout.Dispose();
                suspendLayout = null;
            }

            //try
            //{
            //    //Kill Process DAS Controller
            //    foreach (Process process in System.Diagnostics.Process.GetProcesses())
            //    {
            //        if (process.ProcessName.StartsWith("DPS_Controller"))
            //        {
            //            process.Kill();
            //        }
            //    }

            //    //Start New Process For enhancing Performance
            //    string DpsFullPath = Application.StartupPath;
            //    string ExePath = string.Empty;

            //    //if (System.Diagnostics.Debugger.IsAttached)
            //    //{
            //    //    ExePath = string.Format(@"{0}DPS_Controller\bin\Release\DPS_Controller.exe", DpsFullPath.Substring(0, DpsFullPath.IndexOf("DPS_B2B")));
            //    //}
            //    //else
            //    //{
            //    //    ExePath = string.Format(@"{0}\DPS_Controller.exe", DpsFullPath);
            //    //}
            //    if (BaseEntity.sessMenuName == "DPS007")
            //    {
            //        //ExePath = string.Format(@"{0}DPS_Controller\bin\Release\하나로TNS.exe", DpsFullPath.Substring(0, DpsFullPath.IndexOf("DPS_B2B")));
            //        ExePath = DpsFullPath + @"\control\하나로TNS.exe";
            //    }
            //    else if (BaseEntity.sessMenuName == "DPS006")
            //    {
            //        //ExePath = string.Format(@"{0}DPS_Controller\bin\Release\DPS_Controller.exe", DpsFullPath.Substring(0, DpsFullPath.IndexOf("DPS_B2B")));
            //        ExePath = DpsFullPath + @"\control\DPS_Controller.exe";
            //    }


            //    var Process = new Process
            //    {
            //        StartInfo =
            //                {
            //                    FileName = ExePath,
            //                    Arguments = string.Format("SMenu={0};PositionX={1};PositionY={2};Bizday={3};", BaseEntity.sessMenuName.Equals("DPS008") ? "DPS" : "TEST", this.Location.X.ToString(), this.Location.Y.ToString(), this.UsrCalWork.DefaultDate)
            //                }
            //    };

            //    Process.Start();
            //}
            //catch (Exception ex)
            //{
            //    bowooMessageBox.Show("DPS 제어형 프로그램을 실행하는데 문제가 있습니다. 관리자에게 문의하십시오.");
            //    lib.Common.Log.LogFile.WriteLog(ex.Message);
            //}
        }

        /// <summary>
        /// Set MenuData base on UserControls
        /// </summary>
        /// <param name="Ctrl"></param>
        /// <param name="MenuName"></param>
        /// <param name="VisibleStatus"></param>
        private void SetMenuData(UserControl Ctrl, string MenuName, bool VisibleStatus)
        {
            for (int i = 0; i < Ctrl.Controls.Find("GridViewData", true).Length; i++)
            {
                if (Ctrl.Controls.Find("GridViewData", true)[i].Parent.Parent.Parent.Parent != null) {
                    Ctrl.Controls.Find("GridViewData", true)[i].Parent.Parent.Parent.Parent.BackColor = Color.Transparent;
                }
            }

            (Ctrl as lib.Common.Management.BaseControl).leftSelectedIndex = -1;
            (Ctrl as lib.Common.Management.BaseControl).rightSelectedIndex = -1;
            (Ctrl as lib.Common.Management.BaseControl).PageSearch();

            Ctrl.BringToFront();
            Ctrl.Visible = VisibleStatus;
        }

        /// <summary>
        /// Change user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonLogOut_Click(object sender, EventArgs e)
        {
            bowooRedConfirmBox.Show(LanguagePack.Translate("현재 페이지에서 작업 중이던 내용은 저장되지 않습니다.\r\n\r\n로그아웃 하시겠습니까?"));

            if (bowooRedConfirmBox.DialogResult == DialogResult.OK)
            {
                LogoutProcess(false);
            }
        }

        /// <summary>
        /// Exit Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonExit_Click(object sender, EventArgs e)
        {
            LogoutProcess(true);
            //bowooRedConfirmBox.Show(LanguagePack.Translate("현재 페이지에서 작업 중이던 내용은 저장되지 않습니다.\r\n\r\n종료 하시겠습니까?"));

            //if (bowooRedConfirmBox.DialogResult == DialogResult.OK)
            //{
            //    LogoutProcess(true);
            //}
        }

        /// <summary>
        /// Set LogOut
        /// </summary>
        /// <param name="Exit">True=>Exit All, False=>Change User</param>
        private void LogoutProcess(bool Exit)
        {
            //Kill Process DashBoard
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.StartsWith("Sorter_DashBoard"))
                {
                    process.Kill();
                }
            }

            switch (Exit)
            {
                case true:
                    this.Dispose(true);
                    Application.Exit();
                    break;
                case false:
                    this.Dispose(false);
                    (new Sorter.LoginForm()).Show();
                    break;
            }
        }

        /// <summary>
        /// For Print or Save Excel 
        /// </summary>
        /// <param name="sender"></param>
        private void radButtonPrint_Click(object sender, EventArgs e)
        {
            if (SGrid != null)
            {
                if (SGrid.Rows.Count == 0) {
                    lib.Common.Management.BaseForm.bowooMessageBox.Show(LanguagePack.Translate("저장 또는 출력할 데이터가 없습니다.\r\n데이터 선택 후 진행하세요."));
                    return;
                }

                lib.Common.Management.PrintPopup PrintPopup = new lib.Common.Management.PrintPopup();
                Point PrintPopupLocation = (sender as RadButton).Location;

                PrintPopup.StartPosition = FormStartPosition.Manual;
                PrintPopup.Location = new Point(this.Left + this.radPanelUser.Width / 2 + PrintPopupLocation.X + 37, this.Top + this.radPanelUser.Height / 2 + PrintPopupLocation.Y - 10);
                PrintPopup.Size = new Size(142, 130);
                PrintPopup.sGrid = SGrid;
                PrintPopup.sLabel = SLabel;

                PrintPopup.BringToFront();
                PrintPopup.Show();
                //PrintPopup.ShowDialog();
            }
            else
            {
                bowooMessageBox.Show(LanguagePack.Translate("저장 또는 출력할 데이터가 없습니다.\r\n데이터 조회 후 리스트를 선택하세요."));
            }
        }

        /// <summary>
        /// Change Value Event for setting WorkSeq
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_WorkSeq(object sender, EventArgs e)
        {
            setWrkStatusDelay();
            setWorkSeq();
            MakeSessionInq();

            if (CallCount > 0 && this.radddlwrkseq.Items.Count == 0 && BaseEntity.sessMenuName != String.Empty)
            {
                SetMenuControl();
                CallCount = 0;    
            }

            CallCount++;
        }

        /// <summary>
        /// Change Wrk_Delay
        /// </summary>
        private void setWrkStatusDelay()
        {
            Dictionary<string, object> ParamDelay = new Dictionary<string, object>();
            ParamDelay.Add("@BIZ_DAY", this.calendar1.DefaultDate.Replace("-", string.Empty));

            DBUtil.ExecuteNonQuery("SP_SET_WRK_DELAY", ParamDelay, CommandType.StoredProcedure);
        }

        /// <summary>
        /// For Set UI WorkSeq
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void radddlwrkseq_ItemDataBound(object sender, ListItemDataBoundEventArgs e)
        {
            e.NewItem.TextAlignment = ContentAlignment.MiddleCenter;
        }

        /// <summary>
        /// Change Value Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void radddlwrkseq_SelectedValueChanged(object sender, EventArgs e)
        {
            MakeSessionInq();
            if(BaseEntity.sessMenuName != String.Empty
                && BaseEntity.sessMenuName != "S013_DASTEST"
                && BaseEntity.sessMenuName != "S012_DAS"
                && this.radddlwrkseq.Items.Count>0) SetMenuControl();
        }

        /// <summary>
        /// This essentially does exactly the same as grabbing the title bar of a window, from the window manager's point of view.
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void radPanelMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        /// <summary>
        /// Set Global Position Parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radPanelContents_Paint(object sender, PaintEventArgs e)
        {
            MainPosition = new Point(this.Left, this.Top);
        }

        /// <summary>
        /// Set Global Position Parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Move(object sender, EventArgs e)
        {
            MainPosition = new Point(this.Left, this.Top);
        }

        /// <summary>
        /// For Set Label MouseLeave
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void news_MouseLeave(object sender, EventArgs e)
        {
            (sender as RadLabelElement).ElementTree.Control.Cursor = Cursors.Default;
            (sender as RadLabelElement).Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        /// <summary>
        /// For Set Label MouseHover
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void news_MouseHover(object sender, EventArgs e)
        {
            (sender as RadLabelElement).ElementTree.Control.Cursor = Cursors.Hand;
            (sender as RadLabelElement).Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Underline | System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        /// <summary>
        /// For Set Label Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void news_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start((sender as RadLabelElement).Name);
        }


        #endregion

        private void calendar1_Load(object sender, EventArgs e)
        {

        }
    }
}
