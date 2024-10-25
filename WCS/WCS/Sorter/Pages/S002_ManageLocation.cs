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
    public partial class S002_ManageLocation : lib.Common.Management.BaseControl
    {
        //SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S002_01_L_CHUTE_SET_LIST]";
        string ucGrid2SelectSpName = "[USP_S002_02_L_PLAN_LIST]";

        string ucGrid2SaveSpName = "USP_S002_02_B_SET_SELECED_ITEM";

        public S002_ManageLocation()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            //01 left grid 설정
            UC01_SMSGridView1.GridTitleText = "슈트 설정 리스트";
            
            //버튼 설정
            UC01_SMSGridView1.Button1_Visible = true;
            UC01_SMSGridView1.Button1_Text = "정보 변경";
            UC01_SMSGridView1.Button1_Click = UC01_SMSGridView1_TopButton1_Click;
            
            
            
            //02 right grid 설정
            UC01_SMSGridView2.GridTitleText = "설정 예정 리스트";

            //버튼 설정
            UC01_SMSGridView2.Button1_Visible = true;
            UC01_SMSGridView2.Button1_Text = "선택 설정";
            UC01_SMSGridView2.Button1_Click = UC01_SMSGridView2_TopButton1_Click;


            //string endBtnEnable = "0";
            //Dictionary<string, object> adminparma = new Dictionary<string, object>();
            //adminparma.Add("@GET_SET_TYPE", "GET");
            //adminparma.Add("@CODE_NM", "END_BUTTON_ENABLE");
            //DataSet dsadmin = new DataSet();
            //dsadmin = DBUtil.ExecuteDataSet("SP_CODE", adminparma, CommandType.StoredProcedure);
            //if (dsadmin != null && dsadmin.Tables.Count > 0 && dsadmin.Tables[0].Rows.Count > 0)
            //{
            //    endBtnEnable = dsadmin.Tables[0].Rows[0][0].ToString();
            //}
        }

        /// <summary>
        /// 페이지 조회 새로고침.
        /// </summary>
        public override void PageSearch()
        {
            try
            {
                base.ShowLoading();

                menuId = "S002";
                menuTitle = "로케이션 관리";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = true;
                    mf.radDropDownListBrand.Enabled = true;
                }

                //데이터 바인딩
                UC01_SMSGridView1.BindData(ucGrid1SelectSpName, null);
                this.UC01_SMSGridView1.SelectRow(leftSelectedIndex);

                //데이터 바인딩
                UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);
                this.UC01_SMSGridView2.SelectRow(rightSelectedIndex);
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("로케이션 관리 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("조회에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        /// <summary>
        /// 정보 변경 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridView1_TopButton1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            int chkedCnt = 0;
            List<int> chkedIdxs = new List<int>();

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
                bowooMessageBox.Show(LanguagePack.Translate("정보 변경할 데이터를 선택하세요."));
                return;
            }

            leftSelectedIndex = this.UC01_SMSGridView1.GridViewData.Rows.IndexOf(this.UC01_SMSGridView1.GridViewData.SelectedRows[0]);

            //버튼 1처리
            Sorter.Popup.S002_ManageLocationPopup_ChangeInfo pop = new Sorter.Popup.S002_ManageLocationPopup_ChangeInfo(ref this.UC01_SMSGridView1, ref chkedIdxs);
            pop.StartPosition = FormStartPosition.CenterParent;

            pop.ShowDialog();

            if (pop.isModified)
            {
                this.PageSearch();
            }        
        }

        /// <summary>
        /// 선택설정 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridView2_TopButton1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            //base.ShowLoading();
            bowooConfirmBox.DialogResult = DialogResult.Cancel;

            //슈트 설정 리스트 체크 카운트
            int chuteChkedCnt = 0;
            List<int> chuteChkedIdxs = new List<int>();

            try
            {
                for (int i = 0; i < this.UC01_SMSGridView1.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chuteChkedIdxs.Add(i);
                        chuteChkedCnt++;
                    }
                }

                if (chuteChkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("선택 설정할 슈트 설정 리스트 데이터를 선택하세요."));
                    return;
                }

                //설정 예정 리스트 체크 카운트
                int planChuteChkedCnt = 0;
                List<int> planChuteChkedIdxs = new List<int>();

                for (int i = 0; i < this.UC01_SMSGridView2.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridView2.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        planChuteChkedIdxs.Add(i);
                        planChuteChkedCnt++;
                    }
                }

                if (planChuteChkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("선택 설정할 설정 예정 리스트 데이터를 선택하세요."));
                    return;
                }

                //설정 예정 리스트가 더 많을 경우
                if (chuteChkedCnt < planChuteChkedCnt)
                {
                    bowooConfirmBox.Show(LanguagePack.Translate("선택 매장의 수가 선택 슈트의 수보다 많습니다.\r\n선택 매장이 상단부터 순서대로, 선택하신 슈트 수만큼만 설정됩니다.\r\n\r\n선택 설정 진행을 계속 하시겠습니까?"));
                    if (bowooConfirmBox.DialogResult != DialogResult.OK)
                    {
                        return;
                    }
                }

                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                {
                    bowooConfirmBox.Show(LanguagePack.Translate("선택 설정 진행을 계속 하시겠습니까?"));
                }

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    int roopCnt = chuteChkedCnt > planChuteChkedCnt ? planChuteChkedCnt : chuteChkedCnt;

                    if (!this.UC01_SMSGridView2.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.UC01_SMSGridView2.Add_Data_Parameters.Add("R_E1", "");
                    this.UC01_SMSGridView2.Add_Data_Parameters["R_E1"] = "";

                    string r_ok = "";
                    string r_msg = "";
                    string r_msg_params = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = roopCnt;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    for (int i = 0; i < roopCnt; i++)
                    {
                        do
                        {
                            //선택 슈트 정보 data param 추가
                            if (!this.UC01_SMSGridView2.Add_Data_Parameters.ContainsKey("CHUTE_NO"))
                                this.UC01_SMSGridView2.Add_Data_Parameters.Add("CHUTE_NO", "");
                            this.UC01_SMSGridView2.Add_Data_Parameters["CHUTE_NO"] = this.UC01_SMSGridView1.GridViewData.Rows[chuteChkedIdxs[i]].Cells["CHUTE_NO"].Value.ToString();

                            UC01_SMSGridView2.Usp_Save_Parameters = new SqlParameter[5];
                            UC01_SMSGridView2.Usp_Save_Parameters[4] = new SqlParameter();
                            UC01_SMSGridView2.Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
                            UC01_SMSGridView2.Usp_Save_Parameters[4].Size = 8000;
                            UC01_SMSGridView2.Usp_Save_Parameters[4].Value = "";
                            UC01_SMSGridView2.Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

                            //SP실행
                            this.UC01_SMSGridView2.ExcuteSaveSp(ucGrid2SaveSpName, planChuteChkedIdxs[i]);

                            r_ok = this.UC01_SMSGridView2.Usp_Save_Parameters[2].Value.ToString();
                            r_msg += this.UC01_SMSGridView2.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                            if (r_ok != "OK")
                            {
                                if (r_ok == "NG" || r_ok == "E1" || r_ok == "E2")
                                {
                                    makeLog("선택 설정", false, r_msg);
                                    bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                                    return;
                                }

                                else
                                {
                                    bowooRedConfirmBox.Show(LanguagePack.Translate(r_msg));
                                }

                                if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    if (r_ok == "E1")
                                    {
                                        makeLog("선택 설정", false, "정리 작업 데이터 존재 시 강제 마감 수락");
                                        this.UC01_SMSGridView2.Add_Data_Parameters["R_E1"] = "Y";
                                    }
                                }

                                else
                                {
                                    return;
                                }
                            }

                            ProgressPopupW.progressBar1.PerformStep();

                        } while (r_ok != "OK");
                    }

                    if (r_ok == "OK")
                    {
                        makeLog("선택 설정", true, "설정 완료");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show(LanguagePack.Translate("설정이 완료되었습니다."));

                        leftSelectedIndex = this.UC01_SMSGridView1.GridViewData.Rows.IndexOf(this.UC01_SMSGridView1.GridViewData.SelectedRows[0]);
                        rightSelectedIndex = this.UC01_SMSGridView2.GridViewData.Rows.IndexOf(this.UC01_SMSGridView2.GridViewData.SelectedRows[0]);

                        PageSearch();
                    }

                    else
                    {
                        string[] r_split = r_msg_params.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                        makeLog("선택 설정", false, r_msg);
                        bowooMessageBox.Show(LanguagePack.Translate("실패한 설정이 존재합니다.\r\n") + string.Format(LanguagePack.Translate(r_msg), r_split[0], r_split[1], r_split[2], r_split[3]));
                    }
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("선택 설정", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("설정에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }  
    }
}