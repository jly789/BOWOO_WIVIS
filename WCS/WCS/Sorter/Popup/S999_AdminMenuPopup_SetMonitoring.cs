using bowoo.Framework.common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

using lib.Common;
using lib.Common.Management;

namespace Sorter.Popup
{
    public partial class S999_AdminMenuPopup_SetMonitoring : lib.Common.Management.BaseForm
    {
        public S999_AdminMenuPopup_SetMonitoring()
        {
            InitializeComponent();
            this.CenterToParent();
        }

        private void ManageLocationPopup_Initialized(object sender, EventArgs e)
        {
            //타이머 및 사용여부 가져오기
            //사용여부 로드
            menuId = "S999";
            menuTitle = "관리자 메뉴 - 작업 현황 페이지 설정 팝업";

            radTitleBarPopup.Text = LanguagePack.Translate(radTitleBarPopup.Text);

            radLabel1.Text = LanguagePack.Translate(radLabel1.Text);
            radLabel2.Text = LanguagePack.Translate(radLabel2.Text);
            radLabel3.Text = LanguagePack.Translate(radLabel3.Text);

            radBtnSave.Text = LanguagePack.Translate(radBtnSave.Text);
            radBtnCancel.Text = LanguagePack.Translate(radBtnCancel.Text);

            Dictionary<string, object> statusparma = new Dictionary<string, object>();
            statusparma.Add("@GET_SET_TYPE", "GET");
            statusparma.Add("@CODE_NM", "WRK_STATUS_YN");
            DataSet dsstatus = new DataSet();
            dsstatus = DBUtil.ExecuteDataSet("SP_CODE", statusparma, CommandType.StoredProcedure);
            string getStatusCode = dsstatus.Tables[0].Rows[0][0].ToString();

            this.radBtnIsUse.Text = getStatusCode == "Y" ? "ON" : "OFF";

            //사용모니터
            if (Screen.AllScreens.Length > 1)
            {
                for (int i = 1; i <= Screen.AllScreens.Length; i++)
                {
                    this.radDdlUseMonitor.Items.Add(new RadListDataItem(i.ToString(), i.ToString()));
                }
            }

            //radDdlUseMonitor.DropDownListElement.Alignment = ContentAlignment.MiddleCenter;
            //radDdlUseMonitor.ListElement.Alignment = ContentAlignment.MiddleCenter;5

            Dictionary<string, object> paramMonitor = new Dictionary<string, object>();
            paramMonitor.Add("@GET_SET_TYPE", "GET");
            paramMonitor.Add("@CODE_NM", "WRK_STATUS_MORNITORS");
            DataRow drMonitor = DBUtil.ExecuteRow(null,"SP_CODE", paramMonitor, CommandType.StoredProcedure);

            if (isExistsValue(this.radDdlUseMonitor, drMonitor["CODE_VAL"].ToString().Trim()))
            {
                this.radDdlUseMonitor.SelectedValue = drMonitor["CODE_VAL"].ToString().Trim();
                this.radDdlUseMonitor.SelectedText = drMonitor["CODE_VAL"].ToString().Trim();
            }

            this.radDdlUseMonitor.Enabled = this.radDdlUseMonitor.Items.Count > 0 && this.radBtnIsUse.Text.Trim().Equals("ON") ? true : false;
            
            //타이머 로드
            Dictionary<string, object> timerparma = new Dictionary<string, object>();
            timerparma.Add("@GET_SET_TYPE", "GET");
            timerparma.Add("@CODE_NM", "WRK_STATUS_DELAY");
            DataSet dstimer = new DataSet();
            dstimer = DBUtil.ExecuteDataSet("SP_CODE", timerparma, CommandType.StoredProcedure);
            string getTimerCode = dstimer.Tables[0].Rows[0][0].ToString();

            this.radTxtTimer.Text = getTimerCode;
            this.radTxtTimer.KeyDown += radTxtTimer_KeyDown;
        }

        void radTxtTimer_KeyDown(object sender, KeyEventArgs e)
        {
            RadTextBox textbox = sender as RadTextBox;

            if (textbox == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(textbox.Text))
            {
                return;
            }

            //수량 Validation
            if (!lib.Common.ValidationEx.IsNumber(textbox.Text))
            {
                bowooMessageBox.Show(LanguagePack.Translate("숫자를 입력하세요."));
                textbox.Text = "";
                return;
            }

            if (int.Parse(textbox.Text) < 1)
            {
                bowooMessageBox.Show(LanguagePack.Translate("'1' 이상의 숫자를 입력하세요."));
                textbox.Text = "";
                return;
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //사용여부 버튼 클릭
        private void radBtnIsUse_Click(object sender, EventArgs e)
        {
            this.radBtnIsUse.Text = this.radBtnIsUse.Text.Trim().Equals("OFF") ? "ON" : "OFF";
            this.radDdlUseMonitor.Enabled = this.radDdlUseMonitor.Items.Count > 0 && this.radBtnIsUse.Text.Equals("ON") ? true : false;
        }

        //타이머 설정 위 화살표 클릭
        private void radBtnTimerUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.radTxtTimer.Text))
            {
                this.radTxtTimer.Text = "0";
            }

            int time = int.Parse(this.radTxtTimer.Text);
            time = time >= 999 ? 999 : time+1;
            this.radTxtTimer.Text = time.ToString();
        }

        //타이머설정 아래화살표 클릭
        private void radBtnTimerDown_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.radTxtTimer.Text))
            {
                this.radTxtTimer.Text = "0";
            }

            int time = int.Parse(this.radTxtTimer.Text);
            time = time > 0 ? time-1 : 0;
            this.radTxtTimer.Text = time.ToString();
        }

        /// <summary>
        /// Save Button Click Event 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnSave_Click(object sender, EventArgs e)
        {
            if (this.radBtnIsUse.Text.Equals("ON"))
            {

                if (this.radDdlUseMonitor.SelectedValue == null)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("연동된 모니터가 없으므로, 작업 현황판을 실행할 수 없습니다.\r\n디스플레이 연동 후 다시 실행하세요."));
                    return;
                }

                if (string.IsNullOrEmpty(this.radDdlUseMonitor.SelectedValue.ToString()))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("연동된 모니터를 확인하세요."));
                    return;
                }

                if (this.radDdlUseMonitor.Items.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("연동된 모니터가 없으므로, 작업 현황판을 실행할 수 없습니다.\r\n디스플레이 연동 후 다시 실행하세요."));
                    return;
                }
            }

            else
            {
                KillWrkStatus_Process();
            }

            using (SqlConnection con = DBUtil.Conn)
            {
                con.Open();
                SqlTransaction tran = con.BeginTransaction();

                try
                {
                    //On or Off
                    Dictionary<string, object> ParamOnOff = new Dictionary<string, object>();
                    ParamOnOff.Add("@GET_SET_TYPE", "SET");
                    ParamOnOff.Add("@CODE_NM", "WRK_STATUS_YN");
                    ParamOnOff.Add("@CODE_VAL", this.radBtnIsUse.Text.Equals("ON") == true ? "Y" : "N");
                    DBUtil.ExecuteNonQuery("SP_CODE", ParamOnOff, CommandType.StoredProcedure);

                    //Mornitor Sequence
                    if (this.radDdlUseMonitor.SelectedValue != null)
                    { 
                        Dictionary<string, object> ParamMonitorSeq = new Dictionary<string, object>();
                        ParamMonitorSeq.Add("@GET_SET_TYPE", "SET");
                        ParamMonitorSeq.Add("@CODE_NM", "WRK_STATUS_MORNITORS");
                        ParamMonitorSeq.Add("@CODE_VAL", this.radDdlUseMonitor.SelectedValue.ToString());
                        DBUtil.ExecuteNonQuery("SP_CODE", ParamMonitorSeq, CommandType.StoredProcedure);
                    }
                
                    //Delay
                    Dictionary<string, object> ParamDelay = new Dictionary<string, object>();
                    ParamDelay.Add("@GET_SET_TYPE", "SET");
                    ParamDelay.Add("@CODE_NM", "WRK_STATUS_DELAY");
                    ParamDelay.Add("@CODE_VAL", this.radTxtTimer.Text);
                    DBUtil.ExecuteNonQuery("SP_CODE", ParamDelay, CommandType.StoredProcedure);

                    tran.Commit();

                    bowooMessageBox.Show(LanguagePack.Translate("저장 되었습니다."));

                    if (this.radBtnIsUse.Text.Equals("ON"))
                    {
                        ConvertScreenWrkStatus(this.radDdlUseMonitor.SelectedValue.ToString());
                    }

                    this.Close();
                }

                catch (Exception ex)
                {
                    tran.Rollback();
                    lib.Common.Log.LogFile.WriteError(ex);
                }
            }
        }

        /// <summary>
        /// Change Screen WorkStatus Contents
        /// </summary>
        private void ConvertScreenWrkStatus(string SelectScreen)
        {
            BaseEntity.sessWrkStatus = this.radBtnIsUse.Text.Equals("ON");
            BaseEntity.sessWrkDelay = int.Parse(this.radTxtTimer.Text.Trim());
            BaseEntity.sessWrkMonitor = this.radDdlUseMonitor.SelectedValue.ToString();

            KillWrkStatus_Process();

            if (BaseEntity.sessWrkStatus)
            {
                if (BaseEntity.sessWrkDelay <= 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("작업 현황판의 최신화 주기 설정이 잘못되었습니다.\r\n주기 재설정 후 사용하세요."));
                    return;
                }

                try
                {
                    //Start New Process For enhancing Performance
                    getFilePath("Sorter_DashBoard", "Sorter_DashBoard.exe");

                    //string SorterFullPath = "";
                    //string ExePath = "";
                    //if (System.Diagnostics.Debugger.IsAttached)
                    //{
                    //    SorterFullPath = System.IO.Directory.GetParent(Application.StartupPath).Parent.Parent.FullName;
                    //    ExePath = string.Format(@"{0}\Sorter_DashBoard\bin\Release\Sorter_DashBoard.exe", SorterFullPath);
                    //}

                    //else
                    //{
                    //    SorterFullPath = Application.StartupPath;
                    //    ExePath = string.Format(@"{0}\Sorter_DashBoard\bin\Release\Sorter_DashBoard.exe", SorterFullPath.Substring(0, SorterFullPath.IndexOf("WCS") + 3));
                    //}

                    var Process = new Process
                    {
                        StartInfo =
                        {
                            FileName = ExePath,
                            Arguments = string.Format("UseScreenSeq={0}" + " " + "Language={1}", BaseEntity.sessWrkMonitor, LanguagePack.Language)
                        }
                    };

                    Process.Start();
                }

                catch (Exception ex)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("작업 현황판 실행에 문제가 있습니다.\r\n관리자에게 문의하세요."));
                    lib.Common.Log.LogFile.WriteLog(ex.Message);
                }
            }
        }

        /// <summary>
        /// Set Process Status Killed
        /// </summary>
        private void KillWrkStatus_Process()
        {
            //Kill Process DashBoard
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName.StartsWith("Sorter_DashBoard"))
                {
                    process.Kill();
                }
            }
        }

        /// <summary>
        /// Set DropdownList items Style
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void radDdlUseMonitor_VisualListItemFormatting(object sender, VisualItemFormattingEventArgs args)
        {
            args.VisualItem.Font = new Font("Segoe UI", 14, FontStyle.Regular);
        }

        /// <summary>
        /// Check value whether value is exists in dropdownlist
        /// </summary>
        /// <param name="DropdownList"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool isExistsValue(RadDropDownList DropdownList, object value)
        {
            foreach (RadListDataItem item in DropdownList.Items)
            {
                if (item.Value.Equals(value))
                {
                    return true;
                }
            }

            return false;
        } 
    }
}