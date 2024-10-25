using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Scheduler.Dialogs;

using System.Data.SqlClient;
using bowoo.Lib;
using bowoo.Lib.DataBase;
using bowoo.Framework.common;
using System.Diagnostics;

namespace DPS_B2B.Pages
{
    public partial class DPS999_AdminMenu : lib.Common.Management.BaseControl
    {
        //string sp_Load_uC01_GridView1 = "[USP_DPS999_01_L_BRAND_MASTER]";
        string sp_Load_uC01_GridView2 = "[USP_DPS999_02_L_POP_USER_REG]";
        //string ucGrid1SaveSpName = "[USP_DPS999_01_S_BRAND_MASTER]";
        string ucGrid2SaveSpName = "[USP_DPS999_02_S_POP_USER_REG]";
        string r_ok = "";
        string r_msg = "";

        public DPS999_AdminMenu()
        {
            InitializeComponent();
        }

        private void DPS999_AdminMenu_Load(object sender, EventArgs e)
        {
            //사용여부 로드
            menuId = "DPS999";
            menuTitle = "관리자 메뉴";

            //타이틀 설정
            //uC01_GridView1.GridTitleText = "브랜드 마스터";
            uC01_GridView2.GridTitleText = "사용자 등록 리스트";

            //uC01_GridView1.HideSearchCondition();
            uC01_GridView2.HideSearchCondition();

            //this.uC01_GridView1.GridViewData.AllowAddNewRow = true;
            //this.uC01_GridView1.GridViewData.AllowDeleteRow = true;

            this.uC01_GridView2.GridViewData.AllowAddNewRow = true;
            this.uC01_GridView2.GridViewData.AllowDeleteRow = true;

            //this.uC01_GridView1.GridViewData.CellValueChanged += uC01_GridView1_CellValueChanged;
            //this.uC01_GridView1.GridViewData.UserAddedRow += uC01_GridView1_UserAddedRow;
            //this.uC01_GridView1.GridViewData.UserDeletingRow += uC01_GridView1_UserDeletingRow;
            //this.uC01_GridView1.GridViewData.ContextMenuOpening += uC01_GridView1_ContextMenuOpening;

            //this.uC01_GridView2.GridViewData.CellValueChanged += uC01_GridView2_CellValueChanged;
            //this.uC01_GridView2.GridViewData.UserAddedRow += uC01_GridView2_UserAddedRow;
            //this.uC01_GridView2.GridViewData.UserDeletingRow += uC01_GridView2_UserDeletingRow;
            //this.uC01_GridView2.GridViewData.ContextMenuOpening += uC01_GridView2_ContextMenuOpening;

            //모니터
            //타이머 및 사용여부 가져오기
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

            Dictionary<string, object> paramMonitor = new Dictionary<string, object>();
            paramMonitor.Add("@GET_SET_TYPE", "GET");
            paramMonitor.Add("@CODE_NM", "WRK_STATUS_MORNITORS");
            DataRow drMonitor = DBUtil.ExecuteRow(null, "SP_CODE", paramMonitor, CommandType.StoredProcedure);

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
            this.radBtnIsUse.Click +=radBtnIsUse_Click;
            this.radBtnTimerUp.Click +=radBtnTimerUp_Click;
            this.radBtnTimerDown.Click +=radBtnTimerDown_Click;
            this.radBtnSave.Click +=radBtnSave_Click;
            this.radDdlUseMonitor.VisualListItemFormatting += new Telerik.WinControls.UI.VisualListItemFormattingEventHandler(this.radDdlUseMonitor_VisualListItemFormatting);

            
        }
        /// <summary>
        /// Change Screen WorkStatus Contents
        /// </summary>
        private void ConvertScreenWrkStatus(string SelectScreen)
        {
            BaseEntity.sessWrkStatus = this.radBtnIsUse.Text.Equals("ON");
            BaseEntity.sessWrkDelay = int.Parse(this.radTxtTimer.Text.Trim());
            if (this.radDdlUseMonitor.SelectedValue == null)
                BaseEntity.sessWrkMonitor = "0";
            else
                BaseEntity.sessWrkMonitor = this.radDdlUseMonitor.SelectedValue.ToString();

            KillWrkStatus_Process();

            if (BaseEntity.sessWrkStatus)
            {
                if (BaseEntity.sessWrkDelay <= 0)
                {
                    bowooMessageBox.Show("작업 현황판의 최신화 주기 설정이 잘못되었습니다.\r\n주기 재설정 후 사용하세요.");
                    return;
                }

                string SorterFullPath = "";
                string ExePath = "";

                try
                {
                    //Start New Process For enhancing Performance
                    
                    if (System.Diagnostics.Debugger.IsAttached)
                    {
                        SorterFullPath= System.IO.Directory.GetParent(Application.StartupPath).Parent.Parent.FullName;
                        ExePath = string.Format(@"{0}\DPS_B2B_DashBoard\bin\Release\DPS_B2B_DashBoard.exe", SorterFullPath);
                    }
                    else
                    {
                        SorterFullPath = Application.StartupPath;
                        ExePath = string.Format(@"{0}\DPS_B2B_DashBoard.exe", SorterFullPath);
                    }
                    

                    var Proc = new Process
                    {
                        StartInfo =
                        {
                            FileName = ExePath,
                            Arguments = string.Format("UseScreenSeq={0}", BaseEntity.sessWrkMonitor)
                        }
                        
                    };
                    

                    Proc.Start();
                }
                catch (Exception ex)
                {
                    bowooMessageBox.Show("작업 현황판 실행에 문제가 있습니다.\r\n관리자에게 문의하세요." + ExePath);
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
                if (process.ProcessName.StartsWith("DPS_B2B_DashBoard"))
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
                    bowooMessageBox.Show("연동된 모니터가 없으므로, 작업 현황판을 실행할 수 없습니다.\r\n디스플레이 연동 후 다시 실행하세요.");
                    return;
                }

                if (string.IsNullOrEmpty(this.radDdlUseMonitor.SelectedValue.ToString()))
                {
                    bowooMessageBox.Show("연동된 모니터를 확인하세요.");
                    return;
                }

                if (this.radDdlUseMonitor.Items.Count == 0)
                {
                    bowooMessageBox.Show("연동된 모니터가 없으므로, 작업 현황판을 실행할 수 없습니다.\r\n디스플레이 연동 후 다시 실행하세요.");
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

                    bowooMessageBox.Show("저장 되었습니다.");
                    if (this.radBtnIsUse.Text.Equals("ON")) ConvertScreenWrkStatus(this.radDdlUseMonitor.SelectedValue.ToString());
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    lib.Common.Log.LogFile.WriteError(ex);
                }
            }
        }

        //타이머 설정 위 화살표 클릭
        private void radBtnTimerUp_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.radTxtTimer.Text)) this.radTxtTimer.Text = "0";
            int time = int.Parse(this.radTxtTimer.Text);
            time = time >= 999 ? 999 : time + 1;
            this.radTxtTimer.Text = time.ToString();
        }
        //타이머설정 아래화살표 클릭
        private void radBtnTimerDown_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.radTxtTimer.Text)) this.radTxtTimer.Text = "0";
            int time = int.Parse(this.radTxtTimer.Text);
            time = time > 0 ? time - 1 : 0;
            this.radTxtTimer.Text = time.ToString();
        }
        //사용여부 버튼 클릭
        private void radBtnIsUse_Click(object sender, EventArgs e)
        {
            this.radBtnIsUse.Text = this.radBtnIsUse.Text.Trim().Equals("OFF") ? "ON" : "OFF";
            this.radDdlUseMonitor.Enabled = this.radDdlUseMonitor.Items.Count > 0 && this.radBtnIsUse.Text.Equals("ON") ? true : false;
        }

        void radTxtTimer_KeyDown(object sender, KeyEventArgs e)
        {
            RadTextBox textbox = sender as RadTextBox;
            if (textbox == null)
                return;

            if (string.IsNullOrEmpty(textbox.Text))
                return;

            //수량 Validation
            if (!lib.Common.ValidationEx.IsNumber(textbox.Text))
            {
                bowooMessageBox.Show("숫자를 입력하세요.");
                textbox.Text = "";
                return;
            }

            if (int.Parse(textbox.Text) < 1)
            {
                bowooMessageBox.Show("'1' 이상의 숫자를 입력하세요.");
                textbox.Text = "";
                return;
            }
        }

        private void uC01_GridView2_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            try
            {
                base.ShowLoading();
                r_ok = "";
                r_msg = "";

                //PARAM 추가
                if (!this.uC01_GridView2.Add_Data_Parameters.ContainsKey("DEL_YN"))
                    this.uC01_GridView2.Add_Data_Parameters.Add("DEL_YN", "");
                this.uC01_GridView2.Add_Data_Parameters["DEL_YN"] = "N";

                if (!this.uC01_GridView2.Add_Data_Parameters.ContainsKey("KEYID"))
                    this.uC01_GridView2.Add_Data_Parameters.Add("KEYID", "");
                this.uC01_GridView2.Add_Data_Parameters["KEYID"] = e.Row.Cells["USR_ID"].Value.ToString();

                if (!this.uC01_GridView2.Add_Data_Parameters.ContainsKey("USR_LV"))
                    this.uC01_GridView2.Add_Data_Parameters.Add("USR_LV", "");
                this.uC01_GridView2.Add_Data_Parameters["USR_LV"] = e.Row.Cells["USR_LV"].Value.ToString();

                if (!this.uC01_GridView2.Add_Data_Parameters.ContainsKey("USR_NM"))
                    this.uC01_GridView2.Add_Data_Parameters.Add("USR_NM", "");
                this.uC01_GridView2.Add_Data_Parameters["USR_NM"] = e.Row.Cells["USR_NM"].Value.ToString();

                if (!this.uC01_GridView2.Add_Data_Parameters.ContainsKey("S_USR_PW"))
                    this.uC01_GridView2.Add_Data_Parameters.Add("S_USR_PW", "");
                this.uC01_GridView2.Add_Data_Parameters["S_USR_PW"] = lib.Common.Management.BaseForm.ENC.EncryptData(e.Row.Cells["USR_PW"].Value.ToString());

                this.uC01_GridView2.ExcuteSaveSp(ucGrid2SaveSpName, -1);

                r_ok = this.uC01_GridView2.Usp_Save_Parameters[2].Value.ToString();
                r_msg = this.uC01_GridView2.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                if (r_ok == "OK")
                {
                    makeLog("사용자 추가", true, "추가 완료");
                    bowooMessageBox.Show("추가되었습니다.\r\n추가된 사항은 재시작 후 적용됩니다.");
                    this.uC01_GridView2.GridViewData.EndEdit();
                }
                else
                {
                    makeLog("사용자 추가", false, "추가 실패");
                    bowooMessageBox.Show(r_msg);
                    this.uC01_GridView2.GridViewData.EndEdit();
                }

                //데이터 바인딩
                this.uC01_GridView2.BindData(sp_Load_uC01_GridView2, null);
            }
            catch (Exception exc)
            {
                makeLog("사용자 추가", false, exc.Message.ToString());
                bowooMessageBox.Show("추가에 실패하였습니다.\r\n다시 시도해주세요.");
            }
            finally
            {
                base.HideLoading();
            }
        }

        private void uC01_GridView2_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            GridDataCellElement cell = e.ContextMenuProvider as GridDataCellElement;
            //데이터영역 셀일 경우 
            if (cell != null)
            {
                foreach (object obj in e.ContextMenu.Items)
                {
                    RadMenuItem item = obj as RadMenuItem;
                    if (item != null)
                    {
                        switch (item.Text)
                        {
                            case "행 삭제": item.Text = "사용자 삭제";
                                break;
                            default: break;
                        }
                    }

                }
            }
        }

        private void uC01_GridView2_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            try
            {
                base.ShowLoading();
                r_ok = "";
                r_msg = "";

                for (int i = 0; i < e.Rows.Length; i++)
                {
                    //PARAM 추가
                    if (!this.uC01_GridView2.Add_Data_Parameters.ContainsKey("DEL_YN"))
                        this.uC01_GridView2.Add_Data_Parameters.Add("DEL_YN", "");
                    this.uC01_GridView2.Add_Data_Parameters["DEL_YN"] = "Y";

                    if (!this.uC01_GridView2.Add_Data_Parameters.ContainsKey("KEYID"))
                        this.uC01_GridView2.Add_Data_Parameters.Add("KEYID", "");
                    this.uC01_GridView2.Add_Data_Parameters["KEYID"] = e.Rows[i].Cells["USR_ID"].Value.ToString();


                    this.uC01_GridView2.ExcuteSaveSp(ucGrid2SaveSpName, -1);


                    r_ok = this.uC01_GridView2.Usp_Save_Parameters[2].Value.ToString();
                    r_msg = this.uC01_GridView2.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                    if (r_ok == "OK")
                    {
                        makeLog("사용자 삭제", true, "삭제 완료");
                        bowooMessageBox.Show("삭제되었습니다.\r\n삭제된 사항은 재시작 후 적용됩니다.");
                        this.uC01_GridView2.GridViewData.EndEdit();
                    }
                    else
                    {
                        makeLog("사용자 삭제", false, "삭제 실패");
                        bowooMessageBox.Show(r_msg);
                        this.uC01_GridView2.GridViewData.EndEdit();
                    }


                }
                //데이터 바인딩
                this.uC01_GridView2.BindData(sp_Load_uC01_GridView2, null);
            }
            catch (Exception exc)
            {
                makeLog("사용자 삭제", false, exc.Message.ToString());
                bowooMessageBox.Show("삭제에 실패하였습니다.\r\n다시 시도해주세요.");
            }
            finally
            {
                base.HideLoading();
            }
        }

        private void uC01_GridView2_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            try
            {
                base.ShowLoading();
                //새행 추가 시
                if (e.RowIndex == -1)
                    return;


                r_ok = "";
                r_msg = "";

                if (e.Value == null)
                {

                }

                //pw 변경 시 EncryptData 사용하여 암호화 그 외는 그대로.
                if (e.Column.FieldName == "USR_PW")
                {
                    if (!this.uC01_GridView2.Add_Data_Parameters.ContainsKey("S_USR_PW"))
                        this.uC01_GridView2.Add_Data_Parameters.Add("S_USR_PW", "");
                    this.uC01_GridView2.Add_Data_Parameters["S_USR_PW"] = lib.Common.Management.BaseForm.ENC.EncryptData(e.Row.Cells["USR_PW"].Value.ToString());
                }
                else
                {
                    if (!this.uC01_GridView2.Add_Data_Parameters.ContainsKey("S_USR_PW"))
                        this.uC01_GridView2.Add_Data_Parameters.Add("S_USR_PW", "");
                    this.uC01_GridView2.Add_Data_Parameters["S_USR_PW"] = e.Row.Cells["USR_PW"].Value.ToString();
                }


                this.uC01_GridView2.ExcuteSaveSp(ucGrid2SaveSpName, e.RowIndex);

                r_ok = this.uC01_GridView2.Usp_Save_Parameters[2].Value.ToString();
                r_msg = this.uC01_GridView2.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                if (r_ok == "OK")
                {
                    makeLog("사용자 변경", true, "변경 완료");
                    bowooMessageBox.Show("변경되었습니다.");
                    //this.uC01_GridView2.GridViewData.EndEdit();
                }
                else
                {
                    makeLog("사용자 변경", false, "변경 실패");
                    bowooMessageBox.Show(r_msg);
                    this.uC01_GridView2.GridViewData.EndEdit();
                }
                //데이터 바인딩
                this.uC01_GridView2.BindData(sp_Load_uC01_GridView2, null);
            }
            catch (Exception exc)
            {
                makeLog("사용자 변경", false, exc.Message.ToString());
                bowooMessageBox.Show("변경에 실패하였습니다.\r\n다시 시도해주세요.");
            }
            finally
            {
                base.HideLoading();
            }
        }

        private void uC01_GridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            GridDataCellElement cell = e.ContextMenuProvider as GridDataCellElement;
            //데이터영역 셀일 경우 
            if (cell != null)
            {
                foreach (object obj in e.ContextMenu.Items)
                {
                    RadMenuItem item = obj as RadMenuItem;
                    if (item != null)
                    {
                        switch (item.Text)
                        {
                            case "행 삭제": item.Text = "브랜드 삭제";
                                break;
                            default: break;
                        }
                    }

                }
            }
        }

        //private void uC01_GridView1_UserAddedRow(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        base.ShowLoading();
        //        r_ok = "";
        //        r_msg = "";

        //        //PARAM 추가
        //        if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("DEL_YN"))
        //            this.uC01_GridView1.Add_Data_Parameters.Add("DEL_YN", "");
        //        this.uC01_GridView1.Add_Data_Parameters["DEL_YN"] = "N";
        //        if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("KEY_BRAND_CD"))
        //            this.uC01_GridView1.Add_Data_Parameters.Add("KEY_BRAND_CD", "");
        //        this.uC01_GridView1.Add_Data_Parameters["KEY_BRAND_CD"] = e.Row.Cells["BRAND_CD"].Value.ToString();
        //        if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("BRAND_NM"))
        //            this.uC01_GridView1.Add_Data_Parameters.Add("BRAND_NM", "");
        //        this.uC01_GridView1.Add_Data_Parameters["BRAND_NM"] = e.Row.Cells["BRAND_NM"].Value.ToString();

        //        this.uC01_GridView1.ExcuteSaveSp(ucGrid1SaveSpName, -1);

        //        r_ok = this.uC01_GridView1.Usp_Save_Parameters[2].Value.ToString();
        //        r_msg = this.uC01_GridView1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

        //        if (r_ok == "OK")
        //        {
        //            makeLog("브랜드 행 추가", true, "브랜드 추가 완료");
        //            bowooMessageBox.Show("추가되었습니다.\r\n추가된 사항은 재시작 후 적용됩니다.");
        //            this.uC01_GridView1.GridViewData.EndEdit();
        //        }

        //        //데이터 바인딩
        //        this.uC01_GridView1.BindData(sp_Load_uC01_GridView1, null);
        //    }
        //    catch (Exception exc)
        //    {
        //        makeLog("브랜드 행 추가", false, exc.Message.ToString());
        //        bowooMessageBox.Show("추가에 실패하였습니다.\r\n다시 시도해주세요.");
        //    }
        //    finally
        //    {
        //        base.HideLoading();
        //    }
        //}

        //private void uC01_GridView1_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        //{
        //    try
        //    {
        //        base.ShowLoading();
        //        r_ok = "";
        //        r_msg = "";

        //        for (int i = 0; i < e.Rows.Length; i++)
        //        {
        //            //PARAM 추가
        //            if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("DEL_YN"))
        //                this.uC01_GridView1.Add_Data_Parameters.Add("DEL_YN", "");
        //            this.uC01_GridView1.Add_Data_Parameters["DEL_YN"] = "Y";
        //            if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("KEY_BRAND_CD"))
        //                this.uC01_GridView1.Add_Data_Parameters.Add("KEY_BRAND_CD", "");
        //            this.uC01_GridView1.Add_Data_Parameters["KEY_BRAND_CD"] = e.Rows[i].Cells["BRAND_CD"].Value.ToString();


        //            this.uC01_GridView1.ExcuteSaveSp(ucGrid1SaveSpName, -1);

        //            r_ok = this.uC01_GridView1.Usp_Save_Parameters[2].Value.ToString();
        //            r_msg = this.uC01_GridView1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";
        //        }

        //        if (r_ok == "OK")
        //        {
        //            makeLog("브랜드 행 삭제", true, "브랜드 삭제 완료");
        //            bowooMessageBox.Show("삭제되었습니다.\r\n삭제된 사항은 재시작 후 적용됩니다.");
        //            this.uC01_GridView1.GridViewData.EndEdit();
        //        }

        //        //데이터 바인딩
        //        this.uC01_GridView1.BindData(sp_Load_uC01_GridView1, null);
        //    }
        //    catch (Exception exc)
        //    {
        //        makeLog("브랜드 행 삭제", false, exc.Message.ToString());
        //        bowooMessageBox.Show("삭제에 실패하였습니다.\r\n다시 시도해주세요.");
        //    }
        //    finally
        //    {
        //        base.HideLoading();
        //    }
        //}

        //private void uC01_GridView1_CellValueChanged(object sender, GridViewCellEventArgs e)
        //{
        //    try
        //    {
        //        base.ShowLoading();
        //        //새행 추가 시
        //        if (e.RowIndex == -1)
        //            return;
        //        r_ok = "";
        //        r_msg = "";

        //        this.uC01_GridView1.ExcuteSaveSp(ucGrid1SaveSpName, e.RowIndex);

        //        r_ok = this.uC01_GridView1.Usp_Save_Parameters[2].Value.ToString();
        //        r_msg = this.uC01_GridView1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

        //        if (r_ok == "OK")
        //        {
        //            makeLog("브랜드 셀 변경", true, "브랜드 변경 완료");
        //            bowooMessageBox.Show("변경되었습니다.\r\n변경 사항은 재시작 후 적용됩니다.");
        //            this.uC01_GridView1.GridViewData.EndEdit();
        //        }
        //    }
        //    catch (Exception exc)
        //    {
        //        makeLog("브랜드 셀 변경", false, exc.Message.ToString());
        //        bowooMessageBox.Show("변경에 실패하였습니다.\r\n다시 시도해주세요.");
        //    }
        //    finally
        //    {
        //        base.HideLoading();
        //    }
        //}

        public override void PageSearch()
        {
            base.PageSearch();
            ShowLoading();
            try
            {
                menuId = "DPS999";
                menuTitle = "관리자 메뉴";

                //uC01_GridView1.BindData(sp_Load_uC01_GridView1, null);

                uC01_GridView2.BindData(sp_Load_uC01_GridView2, null);
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("관리자 메뉴 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("조회에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                HideLoading();
            }
        }
    }
}
