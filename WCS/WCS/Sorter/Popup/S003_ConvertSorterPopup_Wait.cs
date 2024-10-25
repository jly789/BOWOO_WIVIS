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
    public partial class S003_ConvertSorterPopup_Wait : lib.Common.Management.BaseForm
    {
        //SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S003_02_01_L_POP_COMPLETE_LIST]";
        string ucGrid2SelectSpName = "[USP_S003_02_02_L_POP_WAIT_WORK_LIST]";
        string ucGrid1SaveSpName_Wait = "[USP_S003_02_01_B_POP_WAIT_PROC]";
        string ucGrid2SaveSpName_Rework = "[USP_S003_02_02_B_POP_REWORK_PROC]";

        public S003_ConvertSorterPopup_Wait()
        {
            InitializeComponent();
            this.CenterToParent();

            radTitleBarPopup.Text = LanguagePack.Translate(radTitleBarPopup.Text);

            //01 left grid
            this.uC01_GridView1.GridTitleText = "변환 완료 리스트";
            this.uC01_GridView1.Button1_Visible = true;
            this.uC01_GridView1.Button1_Text = "대기";
            this.uC01_GridView1.Button1_Click = uC01_GridView1_Button1_Click;

            //조회 조건 숨김
            this.uC01_GridView1.HideSearchCondition();

            //02 right grid
            this.uC01_GridView2.GridTitleText = "대기 작업 리스트";
            this.uC01_GridView2.Button1_Visible = true;
            this.uC01_GridView2.Button1_Text = "개시";
            this.uC01_GridView2.Button1_Click = uC01_GridView2_Button1_Click;

            //조회 조건 숨김
            this.uC01_GridView2.HideSearchCondition();

            //데이터 바인딩
            this.uC01_GridView1.BindData(ucGrid1SelectSpName, null);
            this.uC01_GridView2.BindData(ucGrid2SelectSpName, null);
        }

        //개시 버튼
        private void uC01_GridView2_Button1_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            try
            {
                int chkedCnt = 0;
                List<int> chkedIdxs = new List<int>();

                for (int i = 0; i < this.uC01_GridView2.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView2.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("작업 개시할 데이터를 선택하세요."));
                    return;
                }
                else if (chkedCnt > 1)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("1개 차수만 선택 해 주세요."));
                    return;
                }

                //삭제 확인 메시지 창
                bowooRedConfirmBox.Show(LanguagePack.Translate("작업 개시 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                makeLog("개시 처리", true, "개시 처리 수락");

                string r_ok = "";
                string r_msg = "";

                //data param 추가
                if (!this.uC01_GridView2.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.uC01_GridView2.Add_Data_Parameters.Add("R_E1", "");
                this.uC01_GridView2.Add_Data_Parameters["R_E1"] = "";

                for (int i = 0; i < chkedIdxs.Count; i++)
                {
                    do
                    {
                        this.uC01_GridView2.ExcuteSaveSp(ucGrid2SaveSpName_Rework, chkedIdxs[i]);

                        r_ok = this.uC01_GridView2.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.uC01_GridView2.Usp_Save_Parameters[3].Value.ToString();

                        if (r_ok != "OK")
                        {
                            if (r_ok == "NG")
                            {
                                makeLog("개시 처리", false, r_msg);
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
                                    makeLog("개시 처리", false, "분류 완료 존재 시 개시 처리 수락");//
                                    this.uC01_GridView2.Add_Data_Parameters["R_E1"] = "Y";
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
                    makeLog("개시 처리", true, "개시 처리 완료");
                    //파일 삭제 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("작업 개시 처리가 완료되었습니다."));
                }

                else
                {
                    makeLog("개시 처리", false, "개시 처리 실패, " + r_msg);
                    bowooMessageBox.Show(LanguagePack.Translate("실패한 처리가 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                }

                //조회
                this.uC01_GridView1.BindData(ucGrid1SelectSpName, null);
                this.uC01_GridView2.BindData(ucGrid2SelectSpName, null);
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("개시 처리", false, exc.Message.ToString());
                //파일 삭제 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("작업 개시 처리에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //대기 버튼
        private void uC01_GridView1_Button1_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            try
            {
                int chkedCnt = 0;
                List<int> chkedIdxs = new List<int>();

                for (int i = 0; i < this.uC01_GridView1.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("작업 대기할 데이터를 선택하세요."));
                    return;
                }
                

                //삭제 확인 메시지 창
                bowooRedConfirmBox.Show(LanguagePack.Translate("작업 대기 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                makeLog("대기 처리", true, "대기 처리 수락");

                string r_ok = "";
                string r_msg = "";

                //data param 추가
                if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.uC01_GridView1.Add_Data_Parameters.Add("R_E1", "");
                this.uC01_GridView1.Add_Data_Parameters["R_E1"] = "";

                for (int i = 0; i < chkedIdxs.Count; i++)
                {
                    do
                    {
                        this.uC01_GridView1.ExcuteSaveSp(ucGrid1SaveSpName_Wait, chkedIdxs[i]);

                        r_ok = this.uC01_GridView1.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.uC01_GridView1.Usp_Save_Parameters[3].Value.ToString();

                        if (r_ok != "OK")
                        {
                            if (r_ok == "NG")
                            {
                                makeLog("대기 처리", false, r_msg);
                                bowooMessageBox.Show(LanguagePack.Translate(LanguagePack.Translate(r_msg)));
                                return;
                            }

                            else
                            {
                                bowooRedConfirmBox.Show(LanguagePack.Translate(LanguagePack.Translate(r_msg)));
                            }

                            if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                //E1 : 분류 완료 존재 메시지
                                if (r_ok == "E1")
                                {
                                    makeLog("대기 처리", false, "분류 완료 존재 시 대기 처리 수락");//
                                    this.uC01_GridView1.Add_Data_Parameters["R_E1"] = "Y";
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
                    makeLog("대기 처리", true, "대기 처리 완료");
                    //파일 삭제 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("작업 대기 처리가 완료되었습니다."));
                }

                else
                {
                    makeLog("대기 처리", false, "대기 처리 실패, " + r_msg);
                    bowooMessageBox.Show(LanguagePack.Translate("실패한 처리가 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                }

                //조회
                this.uC01_GridView1.BindData(ucGrid1SelectSpName, null);
                this.uC01_GridView2.BindData(ucGrid2SelectSpName, null);
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("대기 처리", false, exc.Message.ToString());
                //파일 삭제 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("작업 대기 처리에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }
    }
}