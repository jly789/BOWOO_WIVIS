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
    public partial class S003_ConvertSorter : lib.Common.Management.BaseControl
    {
        //SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S003_01_L_ORDER_LIST]";
        string ucGrid2SelectSpName = "[USP_S003_02_L_COMPLETE_LIST]";
        string ucGrid3SelectSpName = "[USP_S003_03_L_COMPLETE_LIST_DETAIL]";
        string ucGrid1ConvertButtonSpName = "USP_S003_01_B_CONVERT";
        string ucGrid2DeleteButtonSpName = "USP_S003_02_B_DELETE";
        string ucGrid2BatchEndButtonSpName = "USP_S003_02_B_BATCH_END";

        public S003_ConvertSorter()
        {
            ////base.ShowLoading();

            InitializeComponent();
            RadPanelContents.Visible = true;

            //01 left grid
            this.UC01_SMSGridView1.GridTitleText = "수신 완료 리스트";
            this.UC01_SMSGridView1.Button1_Visible = true;
            this.UC01_SMSGridView1.Button1_Text = "변환";
            this.UC01_SMSGridView1.Button1_Click = UC01_SMSGridView1_Button1_Click;

            //조회 조건 숨김
            this.UC01_SMSGridView1.HideSearchCondition();
            
            //02 right grid
            this.UC01_SMSGridView2.GridTitleText = "변환 완료 리스트";
            this.UC01_SMSGridView2.Button1_Visible = false;
            this.UC01_SMSGridView2.Button1_Text = "삭제";
            this.UC01_SMSGridView2.Button1_Click = UC01_SMSGridView2_Button1_Click;
            this.UC01_SMSGridView2.Button2_Visible = false;
            this.UC01_SMSGridView2.Button2_Text = "새로 고침";
            this.UC01_SMSGridView2.Button2_Click = UC01_SMSGridView2_Button2_Click;
            this.UC01_SMSGridView2.Button4_Visible = true;
            this.UC01_SMSGridView2.Button4_Text = "작업 대기";
            this.UC01_SMSGridView2.Button4_Click = UC01_SMSGridView2_Button4_Click;

            //조회 조건 숨김
            this.UC01_SMSGridView2.HideSearchCondition();

            //03 bottom grid
            this.UC01_SMSGridView3.GridTitleText = "변환 데이터 상세 리스트";

            //조회 조건 숨김
            this.UC01_SMSGridView3.HideSearchCondition();

            string endBtnEnable = "0";
            Dictionary<string, object> adminparma = new Dictionary<string, object>();
            adminparma.Add("@GET_SET_TYPE", "GET");
            adminparma.Add("@CODE_NM", "END_BUTTON_ENABLE");
            DataSet dsadmin = new DataSet();
            dsadmin = DBUtil.ExecuteDataSet("SP_CODE", adminparma, CommandType.StoredProcedure);

            if (dsadmin != null && dsadmin.Tables.Count > 0 && dsadmin.Tables[0].Rows.Count > 0)
            {
                endBtnEnable = dsadmin.Tables[0].Rows[0][0].ToString();
            }

            if (endBtnEnable == "1")
            {
                this.UC01_SMSGridView2.Button3_Visible = true;
                this.UC01_SMSGridView2.Button3_Text = "차수 마감";
                this.UC01_SMSGridView2.Button3_Click = UC01_SMSGridView2_Button3_Click;
            }

            //PageSearch();
            //base.HideLoading();
        }

        //페이지 조회
        /// <summary>
        /// 페이지 조회 새로 고침
        /// </summary>
        public override void PageSearch()
        {
            try
            {
                base.ShowLoading();

                menuId = "S003";
                menuTitle = "SORTER 변환";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = false;
                    mf.radDropDownListBrand.Enabled = true;
                }

                //데이터 바인딩
                this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, null);

                //데이터 바인딩
                UC01_SMSGridView2.GridViewData.SelectionChanged -= UC01_SMSGridView2_SelectionChanged;
                this.UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);

                if (UC01_SMSGridView2.GridViewData.Rows.Count > 0)
                {
                    UC01_SMSGridView2.GridViewData.Rows[0].IsSelected = false;
                    UC01_SMSGridView2.GridViewData.SelectionChanged += UC01_SMSGridView2_SelectionChanged;
                    UC01_SMSGridView2.GridViewData.Rows[0].IsSelected = true;
                }

                else
                {
                    //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                    if (!this.UC01_SMSGridView3.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS"))
                    {
                        this.UC01_SMSGridView3.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                    }

                    this.UC01_SMSGridView3.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridView2.GetKeyParam();

                    //데이터 바인딩
                    this.UC01_SMSGridView3.BindData(ucGrid3SelectSpName, null);
                }

                if (DBUtil.ExecuteScalar("select top 1 * from if_delay_ord") != null || DBUtil.ExecuteScalar("select top 1 * from if_delay_cfm") != null)
                {
                    //레드 계열 버튼 색 변경
                    //UC01_SMSGridView2.radButton4.ButtonElement.ButtonFillElement.BackColor = Color.FromArgb(255, 231, 231);
                    UC01_SMSGridView2.radButton4.ButtonElement.ButtonFillElement.BackColor2 = Color.FromArgb(251, 174, 174);
                    UC01_SMSGridView2.radButton4.ButtonElement.ButtonFillElement.BackColor3 = Color.FromArgb(255, 251, 251);
                    UC01_SMSGridView2.radButton4.ButtonElement.ButtonFillElement.BackColor4 = Color.FromArgb(251, 230, 230);
                }

                else
                {
                    //기본
                    //UC01_SMSGridView2.radButton4.ButtonElement.ButtonFillElement.BackColor = Color.FromArgb(255, 255, 255, 255);
                    UC01_SMSGridView2.radButton4.ButtonElement.ButtonFillElement.BackColor2 = Color.FromArgb(255, 174, 174, 174);
                    UC01_SMSGridView2.radButton4.ButtonElement.ButtonFillElement.BackColor3 = Color.FromArgb(255, 254, 251, 205);
                    UC01_SMSGridView2.radButton4.ButtonElement.ButtonFillElement.BackColor4 = Color.FromArgb(255, 255, 255, 255);
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("Sorter 변환 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("조회에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //행 변경
        private void UC01_SMSGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (this.UC01_SMSGridView2.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();

                //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                if (!this.UC01_SMSGridView3.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS"))
                {
                    this.UC01_SMSGridView3.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                }

                this.UC01_SMSGridView3.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridView2.GetKeyParam();

                //데이터 바인딩
                this.UC01_SMSGridView3.BindData(ucGrid3SelectSpName, null);

                base.HideLoading();
            }
        }

        //변환
        /// <summary>
        /// 변환 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridView1_Button1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            //base.ShowLoading();

            int chkedCnt = 0;
            List<int> chkedIdxs = new List<int>();

            try
            {
                for (int i = 0; i < this.UC01_SMSGridView1.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("변환할 데이터를 선택하세요."));
                    return;
                }

                //확인 메시지 창
                bowooConfirmBox.Show(LanguagePack.Translate("데이터 변환 진행을 계속 하시겠습니까?"));

                if (bowooConfirmBox.DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                bowooConfirmBox.DialogResult = DialogResult.Cancel;
                makeLog("변환", true, "변환 진행 수락");

                string r_ok = "";
                string r_msg = "";

                //선택 슈트 정보 data param 추가
                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("DELETE_E1"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("DELETE_E1", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["DELETE_E1"] = "";

                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("DELETE_E2"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("DELETE_E2", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["DELETE_E2"] = "";

                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("DELETE_E3"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("DELETE_E3", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["DELETE_E3"] = "";

                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("DELETE_E4"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("DELETE_E4", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["DELETE_E4"] = "";

                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("EXISTS_CHUTE"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("EXISTS_CHUTE", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["EXISTS_CHUTE"] = "";

                ProgressPopupW = new lib.Common.Management.ProgressFormW();
                ProgressPopupW.progressBar1.Maximum = chkedIdxs.Count;
                ProgressPopupW.progressBar1.Value = 1;
                ProgressPopupW.progressBar1.Step = 1;
                ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                ProgressPopupW.BringToFront();
                ProgressPopupW.Show();

                do
                {
                    for (int i = 0; i < chkedIdxs.Count; i++)
                    {
                        //예정수량  = 완료수량 시 변환 불필요 메시지
                        if ((int)this.UC01_SMSGridView1.GridViewData.Rows[chkedIdxs[i]].Cells["ORDQTY"].Value
                            <= (int)this.UC01_SMSGridView1.GridViewData.Rows[chkedIdxs[i]].Cells["WRKQTY"].Value
                            + (int)this.UC01_SMSGridView1.GridViewData.Rows[chkedIdxs[i]].Cells["CANQTY"].Value)
                        {
                            bowooMessageBox.Show(LanguagePack.Translate("작업이 완료되었으므로 변환이 불필요합니다."));
                            return;
                        }

                        //sp 실행
                        this.UC01_SMSGridView1.ExcuteSaveSp(ucGrid1ConvertButtonSpName, chkedIdxs[i]);

                        //리턴 값
                        r_ok = this.UC01_SMSGridView1.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.UC01_SMSGridView1.Usp_Save_Parameters[3].Value.ToString();

                        if (r_ok != "OK")
                        {
                            break;
                        }

                        ProgressPopupW.progressBar1.PerformStep();
                    }

                    if (r_ok != "OK")
                    {
                        if (r_ok == "NG")
                        {
                            bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                            return;
                        }

                        else
                        {
                            bowooRedConfirmBox.Show(LanguagePack.Translate(r_msg));
                        }

                        if (bowooRedConfirmBox.DialogResult == DialogResult.OK)
                        {
                            if (r_ok == "E0")
                            {
                                makeLog("변환", true, "슈트에 설정되지 않은 매장 정보를 제외한 데이터 변환 수락");
                                this.UC01_SMSGridView1.Add_Data_Parameters["EXISTS_CHUTE"] = "Y";
                            }

                            else if (r_ok == "E1")
                            {
                                makeLog("변환", true, "이미 변환된 차수의 데이터에 대한 삭제 후 재변환 수락");
                                this.UC01_SMSGridView1.Add_Data_Parameters["DELETE_E1"] = "Y";
                            }

                            else if (r_ok == "E2")
                            {
                                makeLog("변환", true, "이미 변환되어 작업 중인 차수의 데이터에 대한 삭제 후 재변환 수락");
                                this.UC01_SMSGridView1.Add_Data_Parameters["DELETE_E2"] = "Y";
                            }

                            else if (r_ok == "E3")
                            {
                                makeLog("변환", true, "이미 변환되어 분류 완료된 차수의 데이터에 대한 삭제 후 재변환 수락");
                                this.UC01_SMSGridView1.Add_Data_Parameters["DELETE_E3"] = "Y";
                            }
                        }

                        else
                        {
                            return;
                        }
                    }
                } while (r_ok != "OK");

                if (r_ok == "OK")
                {
                    makeLog("변환", true, "변환 완료");
                    //파일 삭제 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("변환이 완료되었습니다."));
                }

                else
                {
                    makeLog("변환", false, r_msg);
                    //변환 실패 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("변환에 실패하였습니다.\r\n관리자에게 문의하세요."));
                }

                ProgressPopupW.Close();

                //조회
                //데이터 바인딩
                PageSearch();
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("변환", false, exc.Message.ToString());
                //변환 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("변환에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }

        //작업 대기 버튼
        private void UC01_SMSGridView2_Button4_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            new Popup.S003_ConvertSorterPopup_Wait().ShowDialog();

            PageSearch();
        }

        //차수 마감 버튼
        private void UC01_SMSGridView2_Button3_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            base.ShowLoading();

            try
            {
                int chkedCnt = 0;
                List<int> chkedIdxs = new List<int>();

                for (int i = 0; i < this.UC01_SMSGridView2.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridView2.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("차수 마감할 데이터를 선택하세요."));
                    return;
                }

                //확인 메시지 창
                bowooRedConfirmBox.Show(LanguagePack.Translate("차수 마감 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                makeLog("차수 마감", true, "차수 마감 수락");

                string r_ok = "";
                string r_msg = "";

                //data param 추가
                if (!this.UC01_SMSGridView2.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.UC01_SMSGridView2.Add_Data_Parameters.Add("R_E1", "");
                this.UC01_SMSGridView2.Add_Data_Parameters["R_E1"] = "";

                for (int i = 0; i < chkedIdxs.Count; i++)
                {
                    do
                    {
                        this.UC01_SMSGridView2.ExcuteSaveSp(ucGrid2BatchEndButtonSpName, chkedIdxs[i]);

                        r_ok = this.UC01_SMSGridView2.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.UC01_SMSGridView2.Usp_Save_Parameters[3].Value.ToString();

                        if (r_ok != "OK")
                        {
                            if (r_ok == "NG")
                            {
                                makeLog("차수 마감", false, r_msg);
                                bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                                return;
                            }

                            else
                            {
                                bowooRedConfirmBox.Show(LanguagePack.Translate(r_msg));
                            }

                            if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                //E1 : 분류 완료 존재 메시지
                                if (r_ok == "E1")
                                {
                                    makeLog("차수 마감", false, "분류 완료 존재 시 차수 마감 수락");
                                    this.UC01_SMSGridView2.Add_Data_Parameters["R_E1"] = "Y";
                                }
                            }

                            else
                            {
                                return;
                            }

                        }
                    } while (r_ok != "OK");
                }

                if (r_ok == "OK")
                {
                    makeLog("차수 마감", true, "차수 마감 완료");
                    //파일 삭제 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("차수 마감 처리가 완료되었습니다."));
                }

                else
                {
                    makeLog("차수 마감", false, "차수 마감 처리 실패, " + r_msg);
                    bowooMessageBox.Show(LanguagePack.Translate("실패한 처리가 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                }

                //조회
                PageSearch();
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("차수 마감", false, exc.Message.ToString());
                //파일 삭제 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("차수 마감 처리에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //삭제 버튼
        /// <summary>
        /// 삭제 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridView2_Button1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            base.ShowLoading();

            try
            {
                int chkedCnt = 0;
                List<int> chkedIdxs = new List<int>();

                for (int i = 0; i < this.UC01_SMSGridView2.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridView2.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("삭제할 데이터를 선택하세요."));
                    return;
                }

                //삭제 확인 메시지 창
                bowooRedConfirmBox.Show(LanguagePack.Translate("삭제 시, 분류 완료 및 박스 마감 데이터는 유지되지만, 미 작업 데이터는 모두 결품 처리됩니다.\r\n\r\n데이터 삭제 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                makeLog("삭제", true, "삭제 진행 수락");

                string r_ok = "";
                string r_msg = "";

                //선택 슈트 정보 data param 추가
                if (!this.UC01_SMSGridView2.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.UC01_SMSGridView2.Add_Data_Parameters.Add("R_E1", "");
                this.UC01_SMSGridView2.Add_Data_Parameters["R_E1"] = "";

                if (!this.UC01_SMSGridView2.Add_Data_Parameters.ContainsKey("R_E2"))
                    this.UC01_SMSGridView2.Add_Data_Parameters.Add("R_E2", "");
                this.UC01_SMSGridView2.Add_Data_Parameters["R_E2"] = "";

                for (int i = 0; i < chkedIdxs.Count; i++)
                {
                    do
                    {
                        //예정수량  = 완료수량 시 변환 불필요 메시지
                        if ((int)this.UC01_SMSGridView2.GridViewData.Rows[chkedIdxs[i]].Cells["ORDQTY"].Value
                            <= (int)this.UC01_SMSGridView2.GridViewData.Rows[chkedIdxs[i]].Cells["WRKQTY"].Value)
                        {
                            bowooMessageBox.Show(LanguagePack.Translate("작업이 완료되었으므로 삭제가 불가합니다."));
                            base.HideLoading();
                            return;
                        }

                        this.UC01_SMSGridView2.ExcuteSaveSp(ucGrid2DeleteButtonSpName, chkedIdxs[i]);

                        r_ok = this.UC01_SMSGridView2.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.UC01_SMSGridView2.Usp_Save_Parameters[3].Value.ToString();

                        if (r_ok != "OK")
                        {
                            if (r_ok == "NG")
                            {
                                makeLog("삭제", false, r_msg);
                                bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                                return;
                            }

                            else
                            {
                                bowooRedConfirmBox.Show(LanguagePack.Translate(r_msg));
                            }

                            if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                //E1 : 분류 완료 존재 메시지
                                if (r_ok == "E1")
                                {
                                    makeLog("삭제", false, "분류 완료 존재 시 삭제 수락");
                                    this.UC01_SMSGridView2.Add_Data_Parameters["R_E1"] = "Y";
                                }

                                //E2 : 박스마감 존재 메시지
                                if (r_ok == "E2")
                                {
                                    makeLog("삭제", false, "박스 마감 존재 시 삭제 수락");
                                    this.UC01_SMSGridView2.Add_Data_Parameters["R_E2"] = "Y";
                                }
                            }

                            else
                            {
                                return;
                            }
                        }
                    } while (r_ok != "OK");
                }

                if (r_ok == "OK")
                {
                    makeLog("삭제", true, "삭제 완료");
                    //파일 삭제 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("삭제가 완료되었습니다."));
                }

                else
                {
                    makeLog("삭제", false, "삭제 실패, " + r_msg);
                    bowooMessageBox.Show(LanguagePack.Translate("실패한 처리가 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                }

                //조회
                PageSearch();
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("삭제", false, exc.Message.ToString());
                //파일 삭제 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("삭제에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
            
        }

        //새로고침
        /// <summary>
        /// 새로고침 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridView2_Button2_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            //데이터 바인딩
            UC01_SMSGridView2.GridViewData.SelectionChanged -= UC01_SMSGridView2_SelectionChanged;
            this.UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);
            
            if (UC01_SMSGridView2.GridViewData.Rows.Count > 0)
            {
                UC01_SMSGridView2.GridViewData.Rows[0].IsSelected = false;
                UC01_SMSGridView2.GridViewData.SelectionChanged += UC01_SMSGridView2_SelectionChanged;
                UC01_SMSGridView2.GridViewData.Rows[0].IsSelected = true;
            }

            else
            {
                //데이터 바인딩
                this.UC01_SMSGridView3.BindData(ucGrid3SelectSpName, null);
            }

            base.HideLoading();
        }
    }
}