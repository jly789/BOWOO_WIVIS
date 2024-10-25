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

namespace Sorter.Pages
{
    public partial class S999_AdminMenu : lib.Common.Management.BaseControl
    {
        //SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S999_01_L_SET_INDUCTION]";
        string ucGrid1SaveSpName = "[USP_S999_01_S_SET_INDUCTION]";

        string ucGrid2SelectSpName = "[USP_S999_02_L_BRAND_MASTER]";
        string ucGrid2SaveSpName = "[USP_S999_02_S_BRAND_MASTER]";

        string ucGrid3SelectSpName = "[USP_S999_03_L_JOB_CODE_LIST]";
        string ucGrid4SelectSpName = "[USP_S999_04_L_SORT_CODE_LIST]";

        string fullCountUploadSpName = "USP_S999_05_B_FULL_COUNT_UPLOAD";
        string barCodeUploadSpName = "USP_S999_05_B_88_CODE_UPLOAD";

        SqlParameter[] Usp_Save_Parameters = new SqlParameter[4];

        bool isAdmin = false;

        string r_ok = "";
        string r_msg = "";
        string r_msg_params = "";

        public S999_AdminMenu()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            this.radTxtAdminCode.PasswordChar = '*';

            //Collapsible Panel 사용 X
            this.UC01_SMSGridViewTop.HideSearchCondition();

            //top side grid
            this.UC01_SMSGridViewTop.GridTitleText = "투입구 설정";
            this.UC01_SMSGridViewTop.Button1_Text = "설정";
            this.UC01_SMSGridViewTop.Button1_Visible = true;
            this.UC01_SMSGridViewTop.Button1_Click = UC01_SMSGridViewTop_Button1;


            //Collapsible Panel 사용 X
            this.UC01_SMSGridViewLeft.HideSearchCondition();

            //left side grid
            this.UC01_SMSGridViewLeft.GridTitleText = "브랜드 마스터";
            this.UC01_SMSGridViewLeft.GridViewData.AllowAddNewRow = true;
            this.UC01_SMSGridViewLeft.GridViewData.AllowDeleteRow = true;

            this.UC01_SMSGridViewLeft.GridViewData.ContextMenuOpening += UC01_SMSGridViewLeft_ContextMenuOpening;

            //Collapsible Panel 사용 X
            this.UC01_SMSGridViewCenter.HideSearchCondition();

            //center side grid
            this.UC01_SMSGridViewCenter.GridTitleText = "작업코드 리스트";

            //Collapsible Panel 사용 X
            this.UC01_SMSGridViewRight.HideSearchCondition();

            //right side grid
            this.UC01_SMSGridViewRight.GridTitleText = "분류코드 리스트";

            tableLayoutPanel2.Enabled = isAdmin;

            radLabel1.Text = LanguagePack.Translate(radLabel1.Text);
            radLabel2.Text = LanguagePack.Translate(radLabel2.Text);
            radLabel3.Text = LanguagePack.Translate(radLabel3.Text);
            radLabel4.Text = LanguagePack.Translate(radLabel4.Text);
            radLabel5.Text = LanguagePack.Translate(radLabel5.Text);

            radBtnFullCountFile.Text = LanguagePack.Translate(radBtnFullCountFile.Text);
            radBtn88CodeFile.Text = LanguagePack.Translate(radBtn88CodeFile.Text);
            radBtnDisplaySet.Text = LanguagePack.Translate(radBtnDisplaySet.Text);
            radBtnUser.Text = LanguagePack.Translate(radBtnUser.Text);

            EnableByAuth(bowoo.Framework.common.BaseEntity.sessLv);
            
            this.radTxtAdminCode.KeyDown += radTxtAdminCode_KeyDown;

            this.UC01_SMSGridViewTop.GridViewData.CellEndEdit += UC01_SMSGridViewTop_CellEndEdit;
            this.UC01_SMSGridViewTop.GridViewData.EditorRequired += UC01_SMSGridViewTop_EditorRequired;
            this.UC01_SMSGridViewTop.GridViewData.CellValueChanged += UC01_SMSGridViewTop_CellValueChanged;

            this.UC01_SMSGridViewLeft.GridViewData.CellValueChanged += UC01_SMSGridViewLeft_CellValueChanged;
            this.UC01_SMSGridViewLeft.GridViewData.UserAddedRow += UC01_SMSGridViewLeft_UserAddedRow;
            this.UC01_SMSGridViewLeft.GridViewData.UserDeletingRow += UC01_SMSGridViewLeft_UserDeletingRow;
        }

        bool UC01_SMSGridViewTop_isModified = false;

        private void UC01_SMSGridViewTop_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            UC01_SMSGridViewTop_isModified = true;
        }

        void UC01_SMSGridViewTop_EditorRequired(object sender, EditorRequiredEventArgs e)
        {
            if (e.EditorType == typeof(RadDropDownListEditor))
            {
                e.Editor = new lib.Common.Management.CustomDropDownListEditor();
            }
        }

        //투입구 설정 셀 변경 시 저장
        void UC01_SMSGridViewTop_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (!UC01_SMSGridViewTop_isModified)
                {
                    return;
                }

                base.ShowLoading();
                
                r_ok = "";
                r_msg = "";
                r_msg_params = "";

                if (this.UC01_SMSGridViewTop.GridViewData.CurrentCell.Tag != null)
                {
                    if (this.UC01_SMSGridViewTop.GridViewData.CurrentCell.Value.ToString() != this.UC01_SMSGridViewTop.GridViewData.CurrentCell.Tag.ToString())
                    {
                        this.UC01_SMSGridViewTop.GridViewData.CurrentCell.Value = this.UC01_SMSGridViewTop.GridViewData.CurrentCell.Tag;
                    }

                    this.UC01_SMSGridViewTop.GridViewData.CurrentCell.Tag = null;
                }

                if (string.IsNullOrEmpty(e.Value.ToString()))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("공백으로 변경할 수 없습니다."));
                    return;
                }

                UC01_SMSGridViewTop.Usp_Save_Parameters = new SqlParameter[5];
                UC01_SMSGridViewTop.Usp_Save_Parameters[4] = new SqlParameter();
                UC01_SMSGridViewTop.Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
                UC01_SMSGridViewTop.Usp_Save_Parameters[4].Size = 8000;
                UC01_SMSGridViewTop.Usp_Save_Parameters[4].Value = "";
                UC01_SMSGridViewTop.Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

                this.UC01_SMSGridViewTop.ExcuteSaveSp(ucGrid1SaveSpName, e.RowIndex);

                r_ok = this.UC01_SMSGridViewTop.Usp_Save_Parameters[2].Value.ToString();
                r_msg = this.UC01_SMSGridViewTop.Usp_Save_Parameters[3].Value.ToString() + "\r\n";
                r_msg_params = this.UC01_SMSGridViewTop.Usp_Save_Parameters[4].Value.ToString();

                if (r_ok == "OK")
                {
                    makeLog("투입구 셀 변경", true, "투입구 셀 변경 완료");
                    bowooMessageBox.Show(LanguagePack.Translate("변경되었습니다."));

                    this.UC01_SMSGridViewTop.GridViewData.EndEdit();
                }

                else
                {
                    string[] r_split = r_msg_params.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                    makeLog("투입구 셀 변경", false, "투입구 셀 변경 실패," + r_msg);
                    bowooMessageBox.Show(string.Format(LanguagePack.Translate(r_msg), r_split[0], r_split[1], r_split[2]));
                    //bowooMessageBox.Show((LanguagePack.Translate(r_msg)));

                    this.UC01_SMSGridViewTop.GridViewData.EndEdit();
                }
                
                //데이터 바인딩
                this.UC01_SMSGridViewTop.BindData(ucGrid1SelectSpName, null);

                UC01_SMSGridViewTop_isModified = false;
            }

            catch (Exception exc)
            {
                makeLog("투입구 셀 변경", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("변경에 실패하였습니다.\r\n다시 시도해주세요."));
            }

            finally
            { 
                base.HideLoading();
            }
        }

        private void UC01_SMSGridViewLeft_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
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
                            case "행 삭제":

                                item.Text = "브랜드 삭제";

                                break;

                            default:

                                break;
                        }
                    }
                }
            }
        }

        private void UC01_SMSGridViewTop_Button1(object sender, EventArgs e)
        {
            string ssql = @"update IF_INDUCT_CONFIG set BIZ_DAY = a.BIZ_DAY , WORK_TYPE = a.WORK_TYPE ,WORK_NM = a.WORK_NM ,WORK_BATCH = a.WORK_BATCH 
							                            ,SORT_TYPE = a.SORT_TYPE , SORT_NM = a.SORT_NM , BRAND_CD = a.BRAND_CD , ERROR_CHUTE = A.ERROR_CHUTE
                            from (
		                            select *
		                            from IF_INDUCT_CONFIG
		                            where INPUT_NO = 1
	                            ) as a";

            DBUtil.ExecuteNonQuery(ssql);
            PageSearch();

        }

        //브랜드 설정 셀 변경 시 저장
        private void UC01_SMSGridViewLeft_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            try
            {
                base.ShowLoading();

                //새행 추가 시
                if (e.RowIndex == -1)
                {
                    return;
                }

                r_ok = "";
                r_msg = "";

                this.UC01_SMSGridViewLeft.ExcuteSaveSp(ucGrid2SaveSpName, e.RowIndex);

                r_ok = this.UC01_SMSGridViewLeft.Usp_Save_Parameters[2].Value.ToString();
                r_msg = this.UC01_SMSGridViewLeft.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                if (r_ok == "OK")
                {
                    makeLog("브랜드 셀 변경", true, "브랜드 변경 완료");
                    bowooMessageBox.Show(LanguagePack.Translate("변경되었습니다.\r\n변경 사항은 재시작 후 적용됩니다."));

                    this.UC01_SMSGridViewLeft.GridViewData.EndEdit();
                }
            }

            catch (Exception exc)
            {
                makeLog("브랜드 셀 변경", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("변경에 실패하였습니다.\r\n다시 시도해주세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //UC01_SMSGridViewLeft 새행 추가 시
        private void UC01_SMSGridViewLeft_UserAddedRow(object sender, GridViewRowEventArgs e)
        {
            try
            {
                base.ShowLoading();

                r_ok = "";
                r_msg = "";

                //PARAM 추가
                if (!this.UC01_SMSGridViewLeft.Add_Data_Parameters.ContainsKey("DEL_YN"))
                {
                    this.UC01_SMSGridViewLeft.Add_Data_Parameters.Add("DEL_YN", "");
                }

                this.UC01_SMSGridViewLeft.Add_Data_Parameters["DEL_YN"] = "N";

                if (!this.UC01_SMSGridViewLeft.Add_Data_Parameters.ContainsKey("KEY_BRAND_CD"))
                {
                    this.UC01_SMSGridViewLeft.Add_Data_Parameters.Add("KEY_BRAND_CD", "");
                }

                this.UC01_SMSGridViewLeft.Add_Data_Parameters["KEY_BRAND_CD"] = e.Row.Cells["BRAND_CD"].Value.ToString();

                if (!this.UC01_SMSGridViewLeft.Add_Data_Parameters.ContainsKey("BRAND_NM"))
                {
                    this.UC01_SMSGridViewLeft.Add_Data_Parameters.Add("BRAND_NM", "");
                }

                this.UC01_SMSGridViewLeft.Add_Data_Parameters["BRAND_NM"] = e.Row.Cells["BRAND_NM"].Value.ToString();

                this.UC01_SMSGridViewLeft.ExcuteSaveSp(ucGrid2SaveSpName, -1);

                r_ok = this.UC01_SMSGridViewLeft.Usp_Save_Parameters[2].Value.ToString();
                r_msg = this.UC01_SMSGridViewLeft.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                if (r_ok == "OK")
                {
                    makeLog("브랜드 행 추가", true, "브랜드 추가 완료");
                    bowooMessageBox.Show(LanguagePack.Translate("추가되었습니다.\r\n추가된 사항은 재시작 후 적용됩니다."));

                    this.UC01_SMSGridViewLeft.GridViewData.EndEdit();
                }

                //데이터 바인딩
                this.UC01_SMSGridViewLeft.BindData(ucGrid2SelectSpName, null);
            }

            catch (Exception exc)
            {
                makeLog("브랜드 행 추가", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("추가에 실패하였습니다.\r\n다시 시도해주세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //브랜드 삭제
        private void UC01_SMSGridViewLeft_UserDeletingRow(object sender, GridViewRowCancelEventArgs e)
        {
            try
            {
                base.ShowLoading();

                r_ok = "";
                r_msg = "";

                for (int i = 0; i < e.Rows.Length; i++)
                {
                    //PARAM 추가
                    if (!this.UC01_SMSGridViewLeft.Add_Data_Parameters.ContainsKey("DEL_YN"))
                    {
                        this.UC01_SMSGridViewLeft.Add_Data_Parameters.Add("DEL_YN", "");
                    }

                    this.UC01_SMSGridViewLeft.Add_Data_Parameters["DEL_YN"] = "Y";

                    if (!this.UC01_SMSGridViewLeft.Add_Data_Parameters.ContainsKey("KEY_BRAND_CD"))
                    {
                        this.UC01_SMSGridViewLeft.Add_Data_Parameters.Add("KEY_BRAND_CD", "");
                    }

                    this.UC01_SMSGridViewLeft.Add_Data_Parameters["KEY_BRAND_CD"] = e.Rows[i].Cells["BRAND_CD"].Value.ToString();

                    this.UC01_SMSGridViewLeft.ExcuteSaveSp(ucGrid2SaveSpName, -1);

                    r_ok = this.UC01_SMSGridViewLeft.Usp_Save_Parameters[2].Value.ToString();
                    r_msg = this.UC01_SMSGridViewLeft.Usp_Save_Parameters[3].Value.ToString() + "\r\n";
                }

                if (r_ok == "OK")
                {
                    makeLog("브랜드 행 삭제", true, "브랜드 삭제 완료");
                    bowooMessageBox.Show(LanguagePack.Translate("삭제되었습니다.\r\n삭제된 사항은 재시작 후 적용됩니다."));

                    this.UC01_SMSGridViewLeft.GridViewData.EndEdit();
                }

                //데이터 바인딩
                this.UC01_SMSGridViewLeft.BindData(ucGrid2SelectSpName, null);
            }

            catch (Exception exc)
            {
                makeLog("브랜드 행 삭제", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("삭제에 실패하였습니다.\r\n다시 시도해주세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //권한에 따른 메뉴 활성화
        private void EnableByAuth(int lv)
        {
            tableLayoutPanel2.Enabled = false;
            this.radBtnUser.Enabled = false;

            if (lv == 0)
            {
                tableLayoutPanel2.Enabled = true;
                this.radBtnUser.Enabled = true;
                this.radTxtAdminCode.Enabled = false;
            }

            else if (lv == 1)
            {
                this.radTxtAdminCode.Enabled = true;

                if (isAdmin)
                {
                    tableLayoutPanel2.Enabled = true;
                    this.radBtnUser.Enabled = false;
                }

                else
                {
                    tableLayoutPanel2.Enabled = false;
                    this.radBtnUser.Enabled = false;
                }
            }

            else
            {
                this.radTxtAdminCode.Enabled = false;
            }
        }

        //관리코드 엔터 시
        void radTxtAdminCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    base.ShowLoading();

                    if (string.IsNullOrEmpty(radTxtAdminCode.Text))
                    {
                        bowooMessageBox.Show(LanguagePack.Translate("관리 코드를 입력하세요."));
                    }

                    Dictionary<string, object> adminparma = new Dictionary<string, object>();
                    adminparma.Add("@GET_SET_TYPE", "GET");
                    adminparma.Add("@CODE_NM", "ADMIN_CODE");
                    DataSet dsadmin = new DataSet();
                    dsadmin = DBUtil.ExecuteDataSet("SP_CODE", adminparma, CommandType.StoredProcedure);
                    string getAdminCode = dsadmin.Tables[0].Rows[0][0].ToString();

                    if (getAdminCode == radTxtAdminCode.Text)
                    {
                        makeLog("관리 코드 확인", true, "관리 코드 일치");
                        isAdmin = true;
                    }

                    else
                    {
                        makeLog("관리 코드 확인", false, "관리 코드 불일치");
                        isAdmin = false;

                        bowooMessageBox.Show(LanguagePack.Translate("관리 코드가 일치하지 않습니다.\r\n확인 후 다시 입력하세요."));
                    }

                    EnableByAuth(bowoo.Framework.common.BaseEntity.sessLv);

                    this.radTxtAdminCode.Text = "";
                }

                catch (Exception exc)
                {
                    makeLog("관리 코드 확인", false, exc.Message.ToString());
                    bowooMessageBox.Show(LanguagePack.Translate("관리 코드 확인에 실패하였습니다.\r\n관리자에게 문의하세요."));
                }

                finally
                {
                    base.HideLoading();
                }
            }
        }

        //페이지 조회
        /// <summary>
        /// 페이지 조회 새로고침.
        /// </summary>
        public override void PageSearch()
        {
            try
            {
                base.ShowLoading();

                menuId = "S999";
                menuTitle = "관리자 메뉴";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = false;
                    mf.radDropDownListBrand.Enabled = false;
                }

                isAdmin = false;
                //데이터 바인딩
                this.UC01_SMSGridViewTop.BindData(ucGrid1SelectSpName, null);
                //데이터 바인딩
                this.UC01_SMSGridViewLeft.BindData(ucGrid2SelectSpName, null);
                //데이터 바인딩
                this.UC01_SMSGridViewCenter.BindData(ucGrid3SelectSpName, null);
                //데이터 바인딩
                this.UC01_SMSGridViewRight.BindData(ucGrid4SelectSpName, null);

                tableLayoutPanel2.Enabled = isAdmin;
                EnableByAuth(bowoo.Framework.common.BaseEntity.sessLv);
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("관리자 메뉴 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("조회에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        private void radButton3_Click(object sender, EventArgs e)
        {
            new Sorter.Popup.S999_AdminMenuPopup_SetUserInfo().ShowDialog();
        }

        private void radButton4_Click(object sender, EventArgs e)
        {
            new Sorter.Popup.S999_AdminMenuPopup_SetUserInfo().ShowDialog();
        }

        //full count 파일 선택
        private void radBtnFullCountFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "CSV 파일 (*.csv)|*.csv";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //base.ShowLoading();
                    DataTable csvDt = GetDataTableFromCsv(ofd.FileName, true);

                    if (csvDt != null && csvDt.Rows.Count > 0)
                    {
                        ProgressPopupW = new lib.Common.Management.ProgressFormW();
                        ProgressPopupW.progressBar1.Maximum = csvDt.Rows.Count;
                        ProgressPopupW.progressBar1.Value = 1;
                        ProgressPopupW.progressBar1.Step = 1;
                        ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                        ProgressPopupW.BringToFront();
                        ProgressPopupW.Show();

                        for (int i = 0; i < csvDt.Rows.Count; i++)
                        {
                            Usp_Save_Parameters = new SqlParameter[5];
                            Usp_Save_Parameters[0] = new SqlParameter();
                            Usp_Save_Parameters[0].ParameterName = "@HEADER_PARAMS";
                            Usp_Save_Parameters[0].DbType = DbType.String;
                            Usp_Save_Parameters[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터
                            Usp_Save_Parameters[1] = new SqlParameter();
                            Usp_Save_Parameters[1].ParameterName = "@DATA_PARAMS";
                            Usp_Save_Parameters[1].DbType = DbType.String;
                            Usp_Save_Parameters[1].Value = "CHUTE_NO=" + csvDt.Rows[i][0].ToString() + ";#FULL_COUNT=" + csvDt.Rows[i][1].ToString(); //데이터파라미터
                            Usp_Save_Parameters[2] = new SqlParameter();
                            Usp_Save_Parameters[2].ParameterName = "@R_OK";
                            Usp_Save_Parameters[2].Size = 10;
                            Usp_Save_Parameters[2].DbType = DbType.String;
                            Usp_Save_Parameters[2].Direction = ParameterDirection.Output;
                            Usp_Save_Parameters[3] = new SqlParameter();
                            Usp_Save_Parameters[3].ParameterName = "@R_MESSAGE";
                            Usp_Save_Parameters[3].Size = 8000;
                            Usp_Save_Parameters[3].Value = "";
                            Usp_Save_Parameters[3].Direction = ParameterDirection.Output;
                            Usp_Save_Parameters[4] = new SqlParameter();
                            Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
                            Usp_Save_Parameters[4].Size = 8000;
                            Usp_Save_Parameters[4].Value = "";
                            Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

                            DBUtil.ExecuteDataSetSqlParam(fullCountUploadSpName, Usp_Save_Parameters);

                            r_ok = Usp_Save_Parameters[2].Value.ToString();
                            r_msg = Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                            ProgressPopupW.progressBar1.PerformStep();

                            if (r_ok != "OK")
                            {
                                makeLog("FULL COUNT 파일 선택", false, "파일 업로드 실패, " + r_msg);
                                bowooMessageBox.Show(LanguagePack.Translate("파일 업로드 중 실패하였습니다.") + string.Format(LanguagePack.Translate(r_msg), r_msg_params));
                                return;
                            }
                        }

                        ProgressPopupW.Close();

                        if (r_ok == "OK")
                        {
                            makeLog("FULL COUNT 파일 선택", true, "파일 업로드 완료");
                            this.radTxtFullCountFile.Text = ofd.FileName;
                            bowooMessageBox.Show(LanguagePack.Translate("파일 업로드가 완료되었습니다."));
                        }
                    }
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("FULL COUNT 파일 선택", false, exc.Message.ToString());
                //변환 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("파일 업로드에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();

                ProgressPopupW.Close();
            }
        }

        //88code 파일 선택
        private void radBtn88CodeFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "CSV 파일 (*.csv)|*.csv";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    //base.ShowLoading();

                    DataTable csvDt = GetDataTableFromCsv(ofd.FileName, true);

                    if (csvDt != null && csvDt.Rows.Count > 0)
                    {
                        StringBuilder sbDataParam;

                        ProgressPopupW = new lib.Common.Management.ProgressFormW();
                        ProgressPopupW.progressBar1.Maximum = csvDt.Rows.Count;
                        ProgressPopupW.progressBar1.Value = 1;
                        ProgressPopupW.progressBar1.Step = 1;
                        ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                        ProgressPopupW.BringToFront();
                        ProgressPopupW.Show();

                        for (int i = 0; i < csvDt.Rows.Count; i++)
                        {
                            sbDataParam = new StringBuilder(20);

                            Usp_Save_Parameters[0] = new SqlParameter();
                            Usp_Save_Parameters[0].ParameterName = "@HEADER_PARAMS";
                            Usp_Save_Parameters[0].DbType = DbType.String;
                            Usp_Save_Parameters[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터
                            Usp_Save_Parameters[1] = new SqlParameter();
                            Usp_Save_Parameters[1].ParameterName = "@DATA_PARAMS";
                            Usp_Save_Parameters[1].DbType = DbType.String;

                            sbDataParam.Append("SATNR=" + csvDt.Rows[i][0].ToString() + ";#");
                            sbDataParam.Append("COLOR=" + csvDt.Rows[i][1].ToString() + ";#");
                            sbDataParam.Append("SIZE=" + csvDt.Rows[i][2].ToString() + ";#");
                            sbDataParam.Append("MATNR=" + csvDt.Rows[i][3].ToString() + ";#");
                            sbDataParam.Append("TAGCD=" + csvDt.Rows[i][4].ToString() + ";#");
                            sbDataParam.Append("BRAND=" + csvDt.Rows[i][5].ToString() + ";#");
                            sbDataParam.Append("ITEMNM=" + csvDt.Rows[i][6].ToString() + ";#");
                            sbDataParam.Append("ITEMCD=" + csvDt.Rows[i][7].ToString() + ";#");
                            sbDataParam.Append("ITEMGBN=" + csvDt.Rows[i][8].ToString() + ";#");
                            sbDataParam.Append("SAISJ=" + csvDt.Rows[i][9].ToString() + ";#");
                            sbDataParam.Append("SAISO=" + csvDt.Rows[i][10].ToString() + ";#");
                            sbDataParam.Append("ZZAPRL_TYPE=" + csvDt.Rows[i][11].ToString() + ";#");
                            sbDataParam.Append("SAITY=" + csvDt.Rows[i][12].ToString() + ";#");
                            sbDataParam.Append("ZZSEX=" + csvDt.Rows[i][13].ToString() + ";#");

                            Usp_Save_Parameters[1].Value = sbDataParam.ToString(); //데이터파라미터
                            Usp_Save_Parameters[2] = new SqlParameter();
                            Usp_Save_Parameters[2].ParameterName = "@R_OK";
                            Usp_Save_Parameters[2].Size = 10;
                            Usp_Save_Parameters[2].DbType = DbType.String;
                            Usp_Save_Parameters[2].Direction = ParameterDirection.Output;
                            Usp_Save_Parameters[3] = new SqlParameter();
                            Usp_Save_Parameters[3].ParameterName = "@R_MESSAGE";
                            Usp_Save_Parameters[3].Size = 8000;
                            Usp_Save_Parameters[3].Value = "";
                            Usp_Save_Parameters[3].Direction = ParameterDirection.Output;

                            DBUtil.ExecuteDataSetSqlParam(barCodeUploadSpName, Usp_Save_Parameters);

                            r_ok = Usp_Save_Parameters[2].Value.ToString();
                            r_msg = Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                            ProgressPopupW.progressBar1.PerformStep();

                            if (r_ok != "OK")
                            {
                                makeLog("88 CODE 파일 선택", false, "파일 업로드 실패, " + r_msg);
                                bowooMessageBox.Show(LanguagePack.Translate("파일 업로드 중 실패하였습니다.") + LanguagePack.Translate(r_msg));
                                return;
                            }
                        }

                        if (r_ok == "OK")
                        {
                            makeLog("88 CODE 파일 선택", true, "파일 업로드 완료");
                            this.radTxt88CodeFile.Text = ofd.FileName;
                            bowooMessageBox.Show(LanguagePack.Translate("파일 업로드가 완료되었습니다."));
                        }

                        ProgressPopupW.Close();
                    }
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("88 CODE 파일 선택", false, exc.Message.ToString());
                //변환 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("파일 업로드에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();

                ProgressPopupW.Close();
            }
        }
    }
}