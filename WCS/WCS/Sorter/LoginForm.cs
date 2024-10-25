using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using bowoo.Framework.common;
using System.Net;
using System.Net.NetworkInformation;
using Telerik.WinControls.UI;
using lib.Common;
using lib.Common.Management;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;


namespace Sorter
{
    public partial class LoginForm : lib.Common.Management.BaseForm
    {
        /// <summary>
        /// This essentially does exactly the same as grabbing the title bar of a window, from the window manager's point of view.
        /// </summary>
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public LoginForm()
        {
            InitializeComponent();

            ChangeBackColor(this.radTextBoxID, System.Drawing.Color.White);
            ChangeBackColor(this.radTextBoxPW, System.Drawing.Color.CornflowerBlue);
        }

        /// <summary>
        /// For Delete TitleBar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radPanelLoginMain_Initialized(object sender, EventArgs e)
        {
            setLanguage();

            this.FormElement.TitleBar.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.AcceptButton = this.radButtonLogin;
        }

        /// <summary>
        /// For Move the Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        
        /// <summary>
        /// For Close the Login Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButtonExit_Click(object sender, EventArgs e)
        {
            bowooRedConfirmBox.Show(LanguagePack.Translate("종료 하시겠습니까?"));

            if (bowooRedConfirmBox.DialogResult == DialogResult.OK)
            {
                this.Close();
                Application.Exit();
            }

            bowooConfirmBox.Close();
        }

        /// <summary>
        /// For LoginIn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButtonLogin_Click(object sender, EventArgs e)
        {
            LoginProcess();
        }

        private void setLanguage()
        {
            getFilePath("Sorter", "Language_Pack.csv");

            LanguagePack.csvDt = LanguagePack.GetDataTableFromCsvStream(ExePath);

            for (int i = 0; i < LanguagePack.csvDt.Columns.Count; i++)
            {
                this.radDropDownListLanguage.Items.Add(new RadListDataItem(string.Format("{0}", LanguagePack.csvDt.Columns[i].ColumnName.ToString())));
            }

            if (this.radDropDownListLanguage.Items.Count > 0)
            {
                this.radDropDownListLanguage.SelectedIndex = 0;
            }

            this.radDropDownListLanguage.EnableMouseWheel = false;

            LanguagePack.Language = this.radDropDownListLanguage.Items.Count > 0 ? this.radDropDownListLanguage.SelectedText.ToString() : string.Empty;

            this.radDropDownListLanguage.SelectedIndexChanged += radDropDownListLanguage_SelectedIndexChanged;

            //1번방법string add
            //for (int i = 0; i < csvDt.Columns.Count; i++)
            //{
            //    this.radDropDownListLanguage.Items.Add(new RadListDataItem(string.Format("{0}", csvDt.Columns[i].ColumnName.ToString())));
            //}

            //2번 방법 comboboxitem add
            //RadListDataItem item = new RadListDataItem("asdf",2342);

            //for (int i = 0; i < csvDt.Columns.Count; i++)
            //{

            //    this.radDropDownListLanguage.Items.Add(new RadListDataItem("asdf", 2342));
            //}

            //3번 방법 datasource binding
            //this.radDropDownListLanguage.DisplayMember = "한국어";
            //this.radDropDownListLanguage.ValueMember = "English";

            //this.radDropDownListLanguage.DataSource = csvDt;
        }

        void radDropDownListLanguage_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (this.radDropDownListLanguage.Items.Count > 0)
            {
                LanguagePack.Language = this.radDropDownListLanguage.SelectedText.ToString();
            }
        }


        /// <summary>
        /// 확장성을 위하여  public으로 선언. LoginProcess
        /// </summary>
        public void LoginProcess()
        {
            string ID = this.radTextBoxID.Text.Trim();
            string PW = this.radTextBoxPW.Text.Trim();
            string HostIP = Dns.GetHostAddresses(Dns.GetHostName())[1].ToString();
            string ConnectTime = DateTime.Now.ToShortTimeString();

            string ErrorMessage = string.Empty;
            string ErrorLogMessage = string.Empty;
            bool loginResult = false;

            if (ID == string.Empty) {
                ErrorMessage = LanguagePack.Translate("ID를 입력하세요.");
            } else {
                if (PW == string.Empty)
                {
                    ErrorMessage = LanguagePack.Translate("Password를 입력하세요.");
                }
                else {
                    switch (CheckUser(ID,PW))
                    {
                        case BaseEntity.enmLoginResult.LoginSucess:
                            loginResult = true;
                            break;
                        case BaseEntity.enmLoginResult.WrongPW:
                            ErrorMessage = LanguagePack.Translate("Password를 확인하세요.");
                            ErrorLogMessage = "Wrong Password";
                            loginResult = false;
                            this.radTextBoxPW.Text = string.Empty;
                            break;
                        case BaseEntity.enmLoginResult.WrongCn:
                            ErrorMessage = LanguagePack.Translate("데이터베이스 접속에 실패하였습니다.");
                            ErrorLogMessage = "Connection Fail";
                            loginResult = false;
                            break;
                        case BaseEntity.enmLoginResult.WrongID:
                            ErrorMessage = LanguagePack.Translate("ID를 확인하세요.");
                            ErrorLogMessage = "Wrong ID";
                            loginResult = false;
                            this.radTextBoxID.Text = string.Empty;
                            this.radTextBoxPW.Text = string.Empty;
                            break;
                    }
                }
            }

            makeLog(ID, ConnectTime, loginResult, ErrorMessage);

            if (loginResult)
            {
                this.Visible = false;
                this.Dispose(false);

                Form frm = new Sorter.MainForm();
                frm.Owner = this;
                frm.Show();

                bowooMessageBox.Show(LanguagePack.Translate("해당 어플리케이션의 최적화된 해상도는 1280 * 720입니다."));

                if (BaseEntity.sessWrkStatus) {

                    if (BaseEntity.sessWrkDelay <= 0)
                    {
                        bowooMessageBox.Show(LanguagePack.Translate("작업 현황판의 최신화 주기 설정이 잘못되었습니다.\r\n주기 재설정 후 사용하세요."));
                        return;
                    }

                    if (Screen.AllScreens.Length <= 1)
                    {
                        bowooMessageBox.Show(LanguagePack.Translate("연동된 모니터가 없으므로, 작업 현황판을 실행할 수 없습니다.\r\n디스플레이 연동 후 다시 실행하세요."));
                        return;
                    }

                    try
                    {
                        //Start New Process For enhancing Performance
                        //getFilePath("Sorter_DashBoard", "Sorter_DashBoard.exe");

                        ////string SorterFullPath = Application.StartupPath;
                        ////string ExePath = string.Format(@"{0}\Sorter_DashBoard\bin\Release\Sorter_DashBoard.exe", SorterFullPath.Substring(0, SorterFullPath.IndexOf("WCS") + 3));

                        //var Process = new Process
                        //{
                        //    StartInfo =
                        //    {
                        //        FileName = ExePath,
                        //        Arguments = string.Format("UseScreenSeq={0}" + " " + "Language={1}", BaseEntity.sessWrkMonitor, LanguagePack.Language)
                        //    }
                        //};

                        //Process.Start();                  
                    }
                    catch (Exception ex)
                    {
                        bowooMessageBox.Show(LanguagePack.Translate("작업 현황판 실행에 문제가 있습니다.\r\n관리자에게 문의하세요."));
                        lib.Common.Log.LogFile.WriteLog(ex.Message);
                    }
                }
            }
            else {
                bowooMessageBox.Show(ErrorMessage);
            }
        }

        /// <summary>
        /// For Writing the Log File
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="PW"></param>
        /// <param name="HostIP"></param>
        /// <param name="ConnectTime"></param>
        private void makeLog(string ID, string ConnectTime, bool LoginResult, string ErrorLogMessage)
        {
            String LogText = ErrorLogMessage.Length > 0 == false ? string.Format("Process:{0}, 성공여부:{1}, 아이디:{2}", "LogIn", LoginResult == true ? "Y" : "N", ID) :
                string.Format("Process:{0}, 성공여부:{1}, 아이디:{2}, 실패원인:{3}", "LogIn", LoginResult == true ? "Y" : "N", ID, ErrorLogMessage);
            lib.Common.Log.LogFile.WriteLog(LogText);
        }

        /// <summary>
        /// For setting session Value
        /// </summary>
        private void makeSession(DataRow drUsrInfo)
        {
            //Get UserInfo
            BaseEntity.sessSID = drUsrInfo["USR_ID"].ToString().Trim();
            BaseEntity.sessSName = drUsrInfo["USR_NM"].ToString().Trim();
            BaseEntity.sessLv = int.Parse(drUsrInfo["USR_LV"].ToString().Trim());
        }

        /// <summary>
        ///  확장성을 위하여 Entity로 Return Value 설정.Login 결과로직 추가시 Entity변경
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="PW"></param>
        /// <returns></returns>
        protected BaseEntity.enmLoginResult CheckUser(string ID,string PW) {
            try
            {
                if (DBUtil.Conn.State.ToString().Equals("Closed"))
                {
                    DBUtil.Conn.Open();

                    Dictionary<string, object> param = new Dictionary<string, object>();
                    param["@ID"] = ID;
                    param["@PW"] = ENC.EncryptData(PW);

                    DataSet dsUsrInfo = DBUtil.ExecuteDataSet("SP_CHK_LOGIN",param,CommandType.StoredProcedure);

                    Dictionary<string, object> paramStatusInfoYN = new Dictionary<string, object>();
                    paramStatusInfoYN["@GET_SET_TYPE"] = "GET";
                    paramStatusInfoYN["@CODE_NM"] = "WRK_STATUS_YN";

                    DataSet dsWrkStatusInfoYN = DBUtil.ExecuteDataSet("SP_CODE", paramStatusInfoYN, CommandType.StoredProcedure);

                    Dictionary<string, object> paramStatusInfoDelay = new Dictionary<string, object>();
                    paramStatusInfoDelay["@GET_SET_TYPE"] = "GET";
                    paramStatusInfoDelay["@CODE_NM"] = "WRK_STATUS_DELAY";

                    DataSet dsWrkStatusInfoDelay = DBUtil.ExecuteDataSet("SP_CODE", paramStatusInfoDelay, CommandType.StoredProcedure);

                    Dictionary<string, object> paramUseMornitors = new Dictionary<string, object>();
                    paramUseMornitors["@GET_SET_TYPE"] = "GET";
                    paramUseMornitors["@CODE_NM"] = "WRK_STATUS_MORNITORS";

                    DataSet dsWrkStatusInfoUseMonitors = DBUtil.ExecuteDataSet("SP_CODE", paramUseMornitors, CommandType.StoredProcedure);

                    if (dsUsrInfo.Tables[0].Rows[0]["result"].ToString().Trim().Equals("success"))
                    {
                        if (dsUsrInfo.Tables[1].Rows.Count > 0)
                        {
                            makeSession(dsUsrInfo.Tables[1].Rows[0]);
                            set_WrkStatus_YN(dsWrkStatusInfoYN.Tables[0].Rows[0]);
                            set_WrkStatus_Delay(dsWrkStatusInfoDelay.Tables[0].Rows[0]);
                            set_WrkStatus_UseMonitors(dsWrkStatusInfoUseMonitors.Tables[0].Rows[0]);
                            return BaseEntity.enmLoginResult.LoginSucess;
                        }
                        else
                        {
                            return BaseEntity.enmLoginResult.WrongPW;
                        }
                    }
                    else {
                        return BaseEntity.enmLoginResult.WrongID;
                    }
                }
            }
            catch (Exception)
            {
                return BaseEntity.enmLoginResult.WrongCn;
            }

            return BaseEntity.enmLoginResult.LoginSucess;
        }

        /// <summary>
        /// Set WRK_STAUTS Using Monitors
        /// </summary>
        /// <param name="dataRow"></param>
        private void set_WrkStatus_UseMonitors(DataRow drWrkStatusUseMonitors)
        {
            //Get WrkStatus Use Y or N
            BaseEntity.sessWrkMonitor = drWrkStatusUseMonitors["CODE_VAL"].ToString().Trim();
        }

        /// <summary>
        /// Set WRK_STATUS Menu Use YN
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void set_WrkStatus_YN(DataRow drWrkStatus)
        {
            //Set WrkStatus Use Y or N
            BaseEntity.sessWrkStatus = drWrkStatus["CODE_VAL"].ToString().Trim().Equals("Y");
        }

        /// <summary>
        /// Set WRK_STATUS Delay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void set_WrkStatus_Delay(DataRow drWrkStatusDelay)
        {
            //Set WrkStatus Delay Time
            BaseEntity.sessWrkDelay = int.Parse(drWrkStatusDelay["CODE_VAL"].ToString());
        }

        /// <summary>
        /// For Change the Focus 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTextBox_Leave(object sender, EventArgs e)
        {
            RadTextBox SelectBox = (sender as RadTextBox);

            if (SelectBox.Name == "radTextBoxID")
            {
                ChangeBackColor(this.radTextBoxID, System.Drawing.Color.CornflowerBlue);
                ChangeBackColor(this.radTextBoxPW, System.Drawing.Color.White);
            }

            else if (SelectBox.Name == "radTextBoxPW")
            {
                ChangeBackColor(this.radTextBoxPW, System.Drawing.Color.CornflowerBlue);
                ChangeBackColor(this.radTextBoxID, System.Drawing.Color.White);
            }

            else
            {
                ChangeBackColor(this.radTextBoxPW, System.Drawing.Color.CornflowerBlue);
                ChangeBackColor(this.radTextBoxID, System.Drawing.Color.CornflowerBlue);
            }
        }

        /// <summary>
        /// For Change the Focus 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTextBox_Click(object sender, EventArgs e)
        {
            ChangeBackColor((sender as RadTextBox), System.Drawing.Color.CornflowerBlue);
        }

        /// <summary>
        /// For Check Input value => only Alpha,Numeric
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            RadTextBox InputBox = (sender as RadTextBox);

            if (e.KeyChar.ToString() != "\b") {

                bool isSuccess = ValidationEx.IsAlphaNumeric(e.KeyChar.ToString());

                e.Handled = !isSuccess;
            }
        }

        /// <summary>
        /// for Change ID to Upper
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTextBoxID_TextChanged(object sender, EventArgs e)
        {
            this.radTextBoxID.CharacterCasing = CharacterCasing.Upper;
        }

        /// <summary>
        /// For Change TextBox BackGround Color
        /// </summary>
        /// <param name="radTextBox"></param>
        /// <param name="color"></param>
        private void ChangeBackColor(RadTextBox radTextBox, Color color)
        {
            ((Telerik.WinControls.UI.RadTextBoxItem)(radTextBox.GetChildAt(0).GetChildAt(0))).BackColor = color;
            ((Telerik.WinControls.Primitives.FillPrimitive)(radTextBox.GetChildAt(0).GetChildAt(1))).BackColor = color;  
        }
    }
}
