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

namespace DPS_B2B.Pages
{
    public partial class DPS005_SendResult : lib.Common.Management.BaseControl
    {
        string sp_Load_uC01_GridView1 = "[USP_DPS005_01_L_PLAN_LIST]";
        string sp_Load_uC01_GridView2 = "[USP_DPS005_02_L_PLAN_DETAIL_LIST]";

        string sp_Load_uC01_Send = "[USP_DPS005_01_B_SEND]";

        public DPS005_SendResult()
        {
            InitializeComponent();
        }

        private void DPS005_SendResult_Load(object sender, EventArgs e)
        {
            this.tbtnAll.Tag = "0";
            this.tbtnYet.Tag = "1";
            this.tbtnDone.Tag = "100";

            this.tbtnAll.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;

            this.tbtnAll.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnYet.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnDone.ToggleStateChanged += tbtnTrans_ToggleStateChanged;


            //타이틀 설정
            uC01_GridView1.GridTitleText = "결과 전송 예정 리스트";
            uC01_GridView2.GridTitleText = "결과 전송 예정 상세 리스트";

            uC01_GridView1.childGrid = uC01_GridView2;
            
            //버튼 설정
            uC01_GridView1.Button1_Visible = true;
            uC01_GridView1.Button1_Text = "결과 전송";

            //80, 25
            uC01_GridView1.Button1_Click = uC01_GridView1_Button1_Click;

            uC01_GridView1.radButton1.Size = new Size(85, 30);
            uC01_GridView1.radButton1.Location = new Point(uC01_GridView1.radButton1.Location.X - 5, uC01_GridView1.radButton1.Location.Y - 5);
            uC01_GridView1.radButton1.Font = new Font("Segoe UI", 11, FontStyle.Bold);
           
        }

        private void uC01_GridView1_Button1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show("권한이 없습니다.\r\n관리자에게 문의하세요.");
                return;
            }

            //base.ShowLoading();
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
                    bowooMessageBox.Show("결과 전송할 데이터를 선택하세요.");
                    return;
                }

                bowooConfirmBox.Show("결과 전송 진행을 계속 하시겠습니까?");
                if (bowooConfirmBox.DialogResult != System.Windows.Forms.DialogResult.OK)
                    return;
                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {

                    //선택 정보 data param 추가
                    if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.uC01_GridView1.Add_Data_Parameters.Add("R_E1", "");
                    this.uC01_GridView1.Add_Data_Parameters["R_E1"] = "";
                    if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("R_E2"))
                        this.uC01_GridView1.Add_Data_Parameters.Add("R_E2", "");
                    this.uC01_GridView1.Add_Data_Parameters["R_E2"] = "";

                    makeLog("결과 전송", true, "결과 전송 진행 수락");

                    string r_ok = "";
                    string r_msg = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = chkedIdxs.Count;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(DPS_B2B.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    for (int i = 0; i < chkedIdxs.Count; i++)
                    {
                        do
                        {
                            //SP실행
                            this.uC01_GridView1.ExcuteSaveSp(sp_Load_uC01_Send, chkedIdxs[i]);

                            r_ok = this.uC01_GridView1.Usp_Save_Parameters[2].Value.ToString();
                            r_msg = this.uC01_GridView1.Usp_Save_Parameters[3].Value.ToString();

                            ProgressPopupW.progressBar1.PerformStep();

                            if (r_ok != "OK")
                            {
                                if (r_ok == "NG")
                                {
                                    makeLog("결과 전송", false, "결과 전송 실패," + r_msg);
                                    bowooMessageBox.Show(r_msg);
                                    return;
                                }

                                bowooRedConfirmBox.Show(r_msg);
                                if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    if (r_ok == "E1")
                                    {
                                        makeLog("결과 전송", false, "결품 시 결과 전송 수락");
                                        this.uC01_GridView1.Add_Data_Parameters["R_E1"] = "Y";
                                    }
                                    if (r_ok == "E2")
                                    {
                                        makeLog("결과 전송", false, "미 작업 데이터 결품 처리 후 전송 수락");
                                        this.uC01_GridView1.Add_Data_Parameters["R_E2"] = "Y";
                                    }
                                }
                                else
                                    return;
                            }
                        } while (r_ok != "OK");
                    }

                    ProgressPopupW.Close();

                    if (r_ok == "OK")
                    {
                        makeLog("결과 전송", true, "결과 전송 완료");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show("결과 전송이 완료되었습니다.");
                    }
                    else
                    {
                        makeLog("결과 전송", false, r_msg);
                        bowooMessageBox.Show("실패한 전송이 존재합니다.\r\n" + r_msg);
                    }



                    //데이터 바인딩
                    PageSearch();


                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("결과 전송", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("결과 전송에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }


        string toggleSearch = "0";
        void tbtnTrans_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {

            this.tbtnAll.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;
            this.tbtnYet.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;
            this.tbtnDone.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;

            this.tbtnAll.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            this.tbtnYet.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            this.tbtnDone.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;

            RadToggleButton tbtn = sender as RadToggleButton;
            if (tbtn != null)
            {
                toggleSearch = tbtn.Tag.ToString();
                tbtn.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
                PageSearch();
            }
            this.tbtnAll.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnYet.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnDone.ToggleStateChanged += tbtnTrans_ToggleStateChanged;


        }

        public override void PageSearch()
        {
            base.PageSearch();
            ShowLoading();
            try
            {
                menuId = "DPS006";
                menuTitle = "결과 전송";

                Dictionary<string, object> paramss = new Dictionary<string, object>();
                paramss["@HEADER_PARAMS"] = "TOGGLE_SEARCH=" + toggleSearch.ToString();

                uC01_GridView1.GridViewData.SelectionChanged -= uC01_GridView1_SelectionChanged;
                uC01_GridView1.BindData(sp_Load_uC01_GridView1, paramss);
                if (this.uC01_GridView1.GridViewData.Rows.Count > 0)
                {
                    uC01_GridView1.GridViewData.Rows[0].IsSelected = false;
                    uC01_GridView1.GridViewData.SelectionChanged += uC01_GridView1_SelectionChanged;
                    uC01_GridView1.GridViewData.Rows[0].IsSelected = true;
                }
                else
                {
                    if (!this.uC01_GridView2.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.uC01_GridView2.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                    this.uC01_GridView2.Usp_Load_Parameters["@KEY_PARAMS"] = "";
                    uC01_GridView2.BindData(sp_Load_uC01_GridView2, paramss);
                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("결과 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("조회에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                HideLoading();
            }
        }


        void uC01_GridView1_SelectionChanged(object sender, EventArgs e)
        {

            if (this.uC01_GridView1.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();
                Dictionary<string, object> paramss = new Dictionary<string, object>();
                paramss["@HEADER_PARAMS"] = "TOGGLE_SEARCH=" + toggleSearch.ToString();

                //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                if (!this.uC01_GridView2.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.uC01_GridView2.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                this.uC01_GridView2.Usp_Load_Parameters["@KEY_PARAMS"] = this.uC01_GridView1.GetKeyParam();


                //데이터 바인딩
                uC01_GridView2.BindData(sp_Load_uC01_GridView2, paramss);
                base.HideLoading();

            }

        }
    }
}
