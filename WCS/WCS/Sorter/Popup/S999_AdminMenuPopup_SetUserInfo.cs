using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Sorter.Popup
{
    public partial class S999_AdminMenuPopup_SetUserInfo : lib.Common.Management.BaseForm
    {
        //SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S999_05_L_POP_USER_REG]";
        string ucGrid1SaveSpName = "[USP_S999_05_S_POP_USER_REG]";

        public S999_AdminMenuPopup_SetUserInfo()
        {
            InitializeComponent();
            this.CenterToParent();
        }

        string r_ok = "";
        string r_msg = "";

        private void ManageLocationPopup_Initialized(object sender, EventArgs e)
        {
            menuId = "S999";
            menuTitle = "관리자 메뉴 - 사용자 등록 및 조회 팝업";

            radTitleBarPopup.Text = LanguagePack.Translate(radTitleBarPopup.Text);

            radLabel1.Text = LanguagePack.Translate(radLabel1.Text);

            radBtnSave.Text = LanguagePack.Translate(radBtnSave.Text);
            radBtnCancel.Text = LanguagePack.Translate(radBtnCancel.Text);

            //this.FormElement.TitleBar.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            //this.radTitleBarPopup.Text = "Popup Title";      

            //top side grid
            this.UC01_SMSGridView1.GridTitleText = "사용자 등록 리스트";

            //데이터 바인딩
            this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, null);

            //관리코드 로드
            Dictionary<string, object> adminparma = new Dictionary<string, object>();
            adminparma.Add("@GET_SET_TYPE", "GET");
            adminparma.Add("@CODE_NM", "ADMIN_CODE");
            DataSet dsadmin = new DataSet();
            dsadmin = DBUtil.ExecuteDataSet("SP_CODE", adminparma, CommandType.StoredProcedure);
            string getAdminCode = dsadmin.Tables[0].Rows[0][0].ToString();

            this.radTxtAdminCode.Text = getAdminCode;
            this.radTxtAdminCode.PasswordChar = '*';

            this.UC01_SMSGridView1.GridViewData.AllowAddNewRow = true;
            this.UC01_SMSGridView1.GridViewData.AllowDeleteRow = true;

            this.UC01_SMSGridView1.GridViewData.CellValueChanged += UC01_SMSGridView1_CellValueChanged;
            this.UC01_SMSGridView1.GridViewData.UserAddedRow += UC01_SMSGridView1_UserAddedRow;
            this.UC01_SMSGridView1.GridViewData.UserDeletingRow += UC01_SMSGridView1_UserDeletingRow;
            this.UC01_SMSGridView1.GridViewData.ContextMenuOpening += UC01_SMSGridView1_ContextMenuOpening;
        }

        private void UC01_SMSGridView1_ContextMenuOpening(object sender, Telerik.WinControls.UI.ContextMenuOpeningEventArgs e)
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
                                
                                item.Text = "사용자 삭제";

                                break;

                            default:
                                
                                break;
                        }
                    }
                }
            }
        }

        //값 변경 시
        private void UC01_SMSGridView1_CellValueChanged(object sender, Telerik.WinControls.UI.GridViewCellEventArgs e)
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

                if(e.Value == null)
                {

                }

                //pw 변경 시 EncryptData 사용하여 암호화 그 외는 그대로.
                if (e.Column.FieldName == "USR_PW")
                {
                    if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("S_USR_PW"))
                        this.UC01_SMSGridView1.Add_Data_Parameters.Add("S_USR_PW", "");
                    this.UC01_SMSGridView1.Add_Data_Parameters["S_USR_PW"] = ENC.EncryptData(e.Row.Cells["USR_PW"].Value.ToString());
                }

                else
                {
                    if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("S_USR_PW"))
                        this.UC01_SMSGridView1.Add_Data_Parameters.Add("S_USR_PW", "");
                    this.UC01_SMSGridView1.Add_Data_Parameters["S_USR_PW"] = e.Row.Cells["USR_PW"].Value.ToString();
                }

                this.UC01_SMSGridView1.ExcuteSaveSp(ucGrid1SaveSpName, e.RowIndex);

                r_ok = this.UC01_SMSGridView1.Usp_Save_Parameters[2].Value.ToString();
                r_msg = this.UC01_SMSGridView1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                if (r_ok == "OK")
                {
                    makeLog("사용자 변경", true, "변경 완료");
                    bowooMessageBox.Show(LanguagePack.Translate("변경되었습니다."));

                    //this.UC01_SMSGridView1.GridViewData.EndEdit();
                }

                else
                {
                    makeLog("사용자 변경", false, "변경 실패");
                    bowooMessageBox.Show(LanguagePack.Translate(r_msg));

                    this.UC01_SMSGridView1.GridViewData.EndEdit();
                }

                //데이터 바인딩
                this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, null);
            }

            catch (Exception exc)
            {
                makeLog("사용자 변경", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("변경에 실패하였습니다.\r\n다시 시도해주세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //새 행 추가 시
        private void UC01_SMSGridView1_UserAddedRow(object sender, Telerik.WinControls.UI.GridViewRowEventArgs e)
        {
            try
            {
                base.ShowLoading();

                r_ok = "";
                r_msg = "";

                //PARAM 추가
                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("DEL_YN"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("DEL_YN", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["DEL_YN"] = "N";

                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("KEYID"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("KEYID", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["KEYID"] = e.Row.Cells["USR_ID"].Value.ToString();

                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("USR_LV"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("USR_LV", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["USR_LV"] = e.Row.Cells["USR_LV"].Value.ToString();

                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("USR_NM"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("USR_NM", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["USR_NM"] = e.Row.Cells["USR_NM"].Value.ToString();

                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("S_USR_PW"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("S_USR_PW", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["S_USR_PW"] = ENC.EncryptData(e.Row.Cells["USR_PW"].Value.ToString());

                this.UC01_SMSGridView1.ExcuteSaveSp(ucGrid1SaveSpName, -1);

                r_ok = this.UC01_SMSGridView1.Usp_Save_Parameters[2].Value.ToString();
                r_msg = this.UC01_SMSGridView1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                if (r_ok == "OK")
                {
                    makeLog("사용자 추가", true, "추가 완료");
                    bowooMessageBox.Show(LanguagePack.Translate("추가되었습니다.\r\n추가된 사항은 재시작 후 적용됩니다."));
                    this.UC01_SMSGridView1.GridViewData.EndEdit();
                }

                else
                {
                    makeLog("사용자 추가", false, "추가 실패");
                    bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                    this.UC01_SMSGridView1.GridViewData.EndEdit();
                }

                //데이터 바인딩
                this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, null);
            }

            catch (Exception exc)
            {
                makeLog("사용자 추가", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("추가에 실패하였습니다.\r\n다시 시도해주세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //행 삭제 시
        private void UC01_SMSGridView1_UserDeletingRow(object sender, Telerik.WinControls.UI.GridViewRowCancelEventArgs e)
        {
            try
            {
                base.ShowLoading();

                r_ok = "";
                r_msg = "";

                for (int i = 0; i < e.Rows.Length; i++)
                {
                    //PARAM 추가
                    if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("DEL_YN"))
                        this.UC01_SMSGridView1.Add_Data_Parameters.Add("DEL_YN", "");
                    this.UC01_SMSGridView1.Add_Data_Parameters["DEL_YN"] = "Y";

                    if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("KEYID"))
                        this.UC01_SMSGridView1.Add_Data_Parameters.Add("KEYID", "");
                    this.UC01_SMSGridView1.Add_Data_Parameters["KEYID"] = e.Rows[i].Cells["USR_ID"].Value.ToString();

                    this.UC01_SMSGridView1.ExcuteSaveSp(ucGrid1SaveSpName, -1);

                    r_ok = this.UC01_SMSGridView1.Usp_Save_Parameters[2].Value.ToString();
                    r_msg = this.UC01_SMSGridView1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                    if (r_ok == "OK")
                    {
                        makeLog("사용자 삭제", true, "삭제 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("삭제되었습니다.\r\n삭제된 사항은 재시작 후 적용됩니다."));

                        this.UC01_SMSGridView1.GridViewData.EndEdit();
                    }

                    else
                    {
                        makeLog("사용자 삭제", false, "삭제 실패");
                        bowooMessageBox.Show(LanguagePack.Translate(r_msg));

                        this.UC01_SMSGridView1.GridViewData.EndEdit();
                    }
                }

                //데이터 바인딩
                this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, null);
            }

            catch (Exception exc)
            {
                makeLog("사용자 삭제", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("삭제에 실패하였습니다.\r\n다시 시도해주세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        private void radButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //저장 버튼 클릭
        private void radBtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (radTxtAdminCode.Text.Length < 4 || radTxtAdminCode.Text.Length > 6)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("관리 코드는 4-6자리로 작성해야 합니다."));
                    radTxtAdminCode.Text = "";
                    return;
                }

                //관리코드 저장
                Dictionary<string, object> adminparma = new Dictionary<string, object>();
                adminparma.Add("@GET_SET_TYPE", "SET");
                adminparma.Add("@CODE_NM", "ADMIN_CODE");
                adminparma.Add("@CODE_VAL", radTxtAdminCode.Text);
                DataSet dsadmin = new DataSet();
                dsadmin = DBUtil.ExecuteDataSet("SP_CODE", adminparma, CommandType.StoredProcedure);

                if (dsadmin.Tables[0].Rows[0][0].ToString() == "OK")
                {
                    makeLog("관리 코드 저장", true, "관리 코드 변경 완료");
                    bowooMessageBox.Show(LanguagePack.Translate("관리 코드가 저장되었습니다."));
                    this.Close();
                }
            }

            catch(Exception exc)
            {
                makeLog("관리 코드 저장", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("관리 코드 저장에 실패하였습니다.\r\n다시 시도해주세요."));
            }

            finally
            {

            }
        }
    }
}