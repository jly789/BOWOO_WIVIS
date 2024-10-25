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
    public partial class S007_SendResult : lib.Common.Management.BaseControl
    {
        //SELECT SP 설정
        string ucTab1Grid1SelectSpName = "[USP_S007_TAP1_01_L_SEND_PLAN_LIST]";
        string ucTab1Grid2SelectSpName = "[USP_S007_TAP1_02_L_SEND_PLAN_LIST_DETAIL]";
        //출고 결과 전송 SP
        string ucTab1Grid1SaveSpName = "USP_S007_TAP1_01_B_SEND";

        //SELECT SP 설정
        string ucTab2Grid1SelectSpName = "[USP_S007_TAP2_01_L_IN_BOX_LIST]";
        string ucTab2Grid2SelectSpName = "[USP_S007_TAP2_02_L_IN_BOX_LIST_DETAIL]";
        //반품 결과 전송 SP
        string ucTab2Grid1SaveSpName = "USP_S007_TAP2_02_B_SEND";

        //SELECT SP 설정
        string ucTab3Grid1SelectSpName = "[USP_S007_TAP3_01_L_OUT_BOX_LIST]";
        string ucTab3Grid2SelectSpName = "[USP_S007_TAP3_02_L_OUT_BOX_LIST_DETAIL]";
        string ucTab3Grid3SelectSpName = "[USP_S007_TAP3_03_L_WORKING_LIST]";
        //정리 결과 전송 SP
        string ucTab3Grid1SaveSpName = "USP_S007_TAP3_02_B_SEND";

        string toggleSearch = "2";

        public S007_SendResult()
        {
            InitializeComponent();

            radPageView1.Pages[0].Tag = "출고";
            radPageView1.Pages[1].Tag = "정리";

            this.radPageView1.SelectedPageChanged +=radPageView1_SelectedPageChanged;
            radPageView1.SelectedPage = radPageViewPage1;
            RadPanelContents.Visible = true;

            radLabel1.Text = LanguagePack.Translate(radLabel1.Text);
            tbtnTransAll.Text = LanguagePack.Translate(tbtnTransAll.Text);
            tbtnTransYet.Text = LanguagePack.Translate(tbtnTransYet.Text);
            tbtnTransDone.Text = LanguagePack.Translate(tbtnTransDone.Text);

            this.tbtnTransAll.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.tbtnTransAll.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnTransYet.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnTransDone.ToggleStateChanged += tbtnTrans_ToggleStateChanged;

            //IF_CODE에 의한 탭 별 활성화
            Dictionary<string, object> param = new Dictionary<string, object>();
            param.Add("@GET_SET_TYPE", "GET");
            param.Add("@CODE_NM", "RESULT_TAB1_ENABLE");
            DataSet tabEnable = new DataSet();
            tabEnable = DBUtil.ExecuteDataSet("SP_CODE", param, CommandType.StoredProcedure);
            string tab1Enable = tabEnable.Tables[0].Rows[0][0].ToString();

            param["@CODE_NM"] = "RESULT_TAB2_ENABLE";
            tabEnable = new DataSet();
            tabEnable = DBUtil.ExecuteDataSet("SP_CODE", param, CommandType.StoredProcedure);
            string tab2Enable = tabEnable.Tables[0].Rows[0][0].ToString();

            param["@CODE_NM"] = "RESULT_TAB3_ENABLE";
            tabEnable = new DataSet();
            tabEnable = DBUtil.ExecuteDataSet("SP_CODE", param, CommandType.StoredProcedure);
            string tab3Enable = tabEnable.Tables[0].Rows[0][0].ToString();

            radPageView1.Pages[0].Text = LanguagePack.Translate(radPageView1.Pages[0].Text);
            radPageView1.Pages[1].Text = LanguagePack.Translate(radPageView1.Pages[1].Text);
            //radPageView1.Pages[2].Text = LanguagePack.Translate(radPageView1.Pages[2].Text);

            radPageView1.Pages[0].Enabled = tab1Enable == "1" ? true : false;
            //radPageView1.Pages[1].Enabled = tab2Enable == "1" ? true : false;
            //radPageView1.Pages[2].Enabled = tab3Enable == "1" ? true : false;

            radPageView1.Pages[1].Enabled = false;
            //radPageView1.Pages[2].Tag = "반품";

            #region 출고

            //top side grid
            this.UC01_SMSGridViewPage1Top.GridTitleText = "결과 전송 예정 리스트";
            this.UC01_SMSGridViewPage1Top.Button1_Visible = true;
            this.UC01_SMSGridViewPage1Top.Button1_Text = "결과 전송";
            this.UC01_SMSGridViewPage1Top.Button1_Click = UC01_SMSGridViewPage1Top_Button1_Click;

            this.UC01_SMSGridViewPage1Bot.GridTitleText = "결과 전송 예정 상세 리스트";

            #endregion

            #region 반품

            //left side grid
            //this.UC01_SMSGridViewPage2Left.GridTitleText = "입고 박스 리스트";

            ////right side grid
            //this.UC01_SMSGridViewPage2Right.GridTitleText = "입고 박스 상세 리스트";

            //this.UC01_SMSGridViewPage2Right.Button1_Visible = true;
            //this.UC01_SMSGridViewPage2Right.Button1_Text = "결과 전송";
            //this.UC01_SMSGridViewPage2Right.Button1_Click = UC01_SMSGridViewPage2Right_Button1_Click;

            #endregion

            #region 정리

            //left side grid
            this.UC01_SMSGridViewPage3Left.GridTitleText = "정리 박스 리스트";

            //top side grid
            this.UC01_SMSGridViewPage3Top.GridTitleText = "정리 박스 상세 리스트";

            this.UC01_SMSGridViewPage3Top.radButton1.Width = 80;
            this.UC01_SMSGridViewPage3Top.radButton1.Height = 25;
            this.UC01_SMSGridViewPage3Top.radButton1.Location = new Point(286, 2);

            this.UC01_SMSGridViewPage3Top.Button1_Visible = true;
            this.UC01_SMSGridViewPage3Top.Button1_Text = "결과 전송";
            this.UC01_SMSGridViewPage3Top.Button1_Click = UC01_SMSGridViewPage3Top_Button1_Click;

            //bottom side grid
            this.UC01_SMSGridViewPage3Bot.GridTitleText = "작업 중 리스트";

            #endregion

            //PageSearch();
        }

        //출고 - 결과전송 버튼 
        private void UC01_SMSGridViewPage1Top_Button1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            //base.ShowLoading();

            try
            {
                int chkedCnt = 0;
                List<int> chkedIdxs = new List<int>();

                for (int i = 0; i < this.UC01_SMSGridViewPage1Top.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewPage1Top.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("결과 전송할 데이터를 선택하세요."));
                    return;
                }

                bowooConfirmBox.Show(LanguagePack.Translate("결과 전송 진행을 계속 하시겠습니까?"));

                if (bowooConfirmBox.DialogResult != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //선택 슈트 정보 data param 추가
                    if (!this.UC01_SMSGridViewPage1Top.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.UC01_SMSGridViewPage1Top.Add_Data_Parameters.Add("R_E1", "");
                    this.UC01_SMSGridViewPage1Top.Add_Data_Parameters["R_E1"] = "";

                    if (!this.UC01_SMSGridViewPage1Top.Add_Data_Parameters.ContainsKey("R_E2"))
                        this.UC01_SMSGridViewPage1Top.Add_Data_Parameters.Add("R_E2", "");
                    this.UC01_SMSGridViewPage1Top.Add_Data_Parameters["R_E2"] = "";

                    makeLog("결과 전송", true, "결과 전송 진행 수락");

                    string r_ok = "";
                    string r_msg = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = chkedIdxs.Count;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    for (int i = 0; i < chkedIdxs.Count; i++)
                    {
                        do
                        {
                            //SP실행
                            this.UC01_SMSGridViewPage1Top.ExcuteSaveSp(ucTab1Grid1SaveSpName, chkedIdxs[i]);

                            r_ok = this.UC01_SMSGridViewPage1Top.Usp_Save_Parameters[2].Value.ToString();
                            r_msg = this.UC01_SMSGridViewPage1Top.Usp_Save_Parameters[3].Value.ToString();

                            ProgressPopupW.progressBar1.PerformStep();

                            if (r_ok != "OK")
                            {
                                if (r_ok == "NG")
                                {
                                    makeLog("결과 전송", false, "결과 전송 실패," + r_msg);
                                    bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                                    return;
                                }

                                bowooRedConfirmBox.Show(LanguagePack.Translate(r_msg));

                                if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    if (r_ok == "E1")
                                    {
                                        makeLog("결과 전송", false, "결품 시 결과 전송 수락");
                                        this.UC01_SMSGridViewPage1Top.Add_Data_Parameters["R_E1"] = "Y";
                                    }

                                    if (r_ok == "E2")
                                    {
                                        makeLog("결과 전송", false, "미 작업 데이터 결품 처리 후 전송 수락");
                                        //this.UC01_SMSGridViewPage1Top.Add_Data_Parameters["R_E2"] = "Y";
                                    }
                                    this.UC01_SMSGridViewPage1Top.Add_Data_Parameters["R_E1"] = "Y";
                                }

                                else
                                {
                                    return;
                                }
                            }
                        } while (r_ok != "OK");
                    }

                    ProgressPopupW.Close();

                    if (r_ok == "OK")
                    {
                        makeLog("결과 전송", true, "결과 전송 완료");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show(LanguagePack.Translate("결과 전송이 완료되었습니다."));
                    }

                    else
                    {
                        makeLog("결과 전송", false, r_msg);
                        bowooMessageBox.Show(LanguagePack.Translate("실패한 전송이 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
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
                bowooMessageBox.Show(LanguagePack.Translate("결과 전송에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }

        //반품 - 결과전송 버튼
        private void UC01_SMSGridViewPage2Right_Button1_Click(object sender, EventArgs e)
        {
            //if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            //{
            //    bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
            //    return;
            //}

            ////base.ShowLoading();

            //try
            //{
            //    int chkedCnt = 0;
            //    List<int> chkedIdxs = new List<int>();

            //    for (int i = 0; i < this.UC01_SMSGridViewPage2Left.GridViewData.Rows.Count; i++)
            //    {
            //        if ((bool)this.UC01_SMSGridViewPage2Left.GridViewData.Rows[i].Cells["checkbox"].Value)
            //        {
            //            chkedIdxs.Add(i);
            //            chkedCnt++;
            //        }
            //    }

            //    if (chkedCnt == 0)
            //    {
            //        bowooMessageBox.Show(LanguagePack.Translate("결과 전송할 데이터를 선택하세요."));
            //        return;
            //    }

            //    bowooConfirmBox.Show(LanguagePack.Translate("결과 전송 진행을 계속 하시겠습니까?"));

            //    if (bowooConfirmBox.DialogResult != System.Windows.Forms.DialogResult.OK)
            //    {
            //        return;
            //    }

            //    if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
            //    {
            //        //선택 슈트 정보 data param 추가
            //        if (!this.UC01_SMSGridViewPage2Left.Add_Data_Parameters.ContainsKey("R_E1"))
            //            this.UC01_SMSGridViewPage2Left.Add_Data_Parameters.Add("R_E1", "");
            //        this.UC01_SMSGridViewPage2Left.Add_Data_Parameters["R_E1"] = "";

            //        makeLog("결과 전송", true, "결과 전송 진행 수락");

            //        string r_ok = "";
            //        string r_msg = "";

            //        ProgressPopupW = new lib.Common.Management.ProgressFormW();
            //        ProgressPopupW.progressBar1.Maximum = chkedIdxs.Count;
            //        ProgressPopupW.progressBar1.Value = 1;
            //        ProgressPopupW.progressBar1.Step = 1;
            //        ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
            //        ProgressPopupW.BringToFront();
            //        ProgressPopupW.Show();

            //        for (int i = 0; i < chkedIdxs.Count; i++)
            //        {
            //            do
            //            {
            //                //SP실행
            //                this.UC01_SMSGridViewPage2Left.ExcuteSaveSp(ucTab2Grid1SaveSpName, chkedIdxs[i]);

            //                r_ok = this.UC01_SMSGridViewPage2Left.Usp_Save_Parameters[2].Value.ToString();
            //                r_msg = this.UC01_SMSGridViewPage2Left.Usp_Save_Parameters[3].Value.ToString();

            //                ProgressPopupW.progressBar1.PerformStep();

            //                if (r_ok != "OK")
            //                {
            //                    if (r_ok == "NG")
            //                    {
            //                        makeLog("결과 전송", false, "결과 전송 실패," + r_msg);
            //                        bowooMessageBox.Show(LanguagePack.Translate(r_msg));
            //                        return;
            //                    }

            //                    bowooConfirmBox.Show(LanguagePack.Translate(r_msg));

            //                    if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
            //                    {
            //                        //if (r_ok == "E1")
            //                        //{
            //                        //    makeLog("결과 전송", false, "결품 시 결과 전송 수락");
            //                        //    this.UC01_SMSGridViewPage2Right.Add_Data_Parameters["R_E1"] = "Y";
            //                        //}
            //                    }

            //                    else
            //                    {
            //                        return;
            //                    }
            //                }
            //            } while (r_ok != "OK");
            //        }

            //        ProgressPopupW.Close();

            //        if (r_ok == "OK")
            //        {
            //            makeLog("결과 전송", true, "결과 전송 완료");
            //            //설정 완료 메시지 창
            //            bowooMessageBox.Show(LanguagePack.Translate("결과 전송이 완료되었습니다."));
            //        }

            //        else
            //        {
            //            makeLog("결과 전송", false, r_msg);
            //            bowooMessageBox.Show(LanguagePack.Translate("실패한 전송이 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
            //        }

            //        //데이터 바인딩
            //        PageSearch();
            //    }
            //}

            //catch (Exception exc)
            //{
            //    //*로그*
            //    makeLog("결과 전송", false, exc.Message.ToString());
            //    //설정 실패 메시지 창
            //    bowooMessageBox.Show(LanguagePack.Translate("결과 전송에 실패하였습니다.\r\n관리자에게 문의하세요."));
            //}

            //finally
            //{
            //    //base.HideLoading();
            //    ProgressPopupW.Close();
            //}
        }

        //정리 - 결과전송 버튼
        private void UC01_SMSGridViewPage3Top_Button1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            //base.ShowLoading();
            try
            {
                int chkedCnt = 0;
                List<int> chkedIdxs = new List<int>();

                for (int i = 0; i < this.UC01_SMSGridViewPage3Left.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewPage3Left.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("결과 전송할 데이터를 선택하세요."));
                    return;
                }

                bowooConfirmBox.Show(LanguagePack.Translate("결과 전송 진행을 계속 하시겠습니까?"));

                if (bowooConfirmBox.DialogResult != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //선택 슈트 정보 data param 추가
                    if (!this.UC01_SMSGridViewPage3Left.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.UC01_SMSGridViewPage3Left.Add_Data_Parameters.Add("R_E1", "");
                    this.UC01_SMSGridViewPage3Left.Add_Data_Parameters["R_E1"] = "";

                    makeLog("결과 전송", true, "결과 전송 진행 수락");

                    string r_ok = "";
                    string r_msg = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = chkedIdxs.Count;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    for (int i = 0; i < chkedIdxs.Count; i++)
                    {
                        do
                        {
                            //SP실행
                            this.UC01_SMSGridViewPage3Left.ExcuteSaveSp(ucTab3Grid1SaveSpName, chkedIdxs[i]);

                            r_ok = this.UC01_SMSGridViewPage3Left.Usp_Save_Parameters[2].Value.ToString();
                            r_msg = this.UC01_SMSGridViewPage3Left.Usp_Save_Parameters[3].Value.ToString();

                            ProgressPopupW.progressBar1.PerformStep();

                            if (r_ok != "OK")
                            {
                                if (r_ok == "NG")
                                {
                                    makeLog("결과 전송", false, "결과 전송 실패," + r_msg);
                                    bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                                    return;
                                }

                                bowooConfirmBox.Show(LanguagePack.Translate(r_msg));

                                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                                {
                                    //if (r_ok == "E1")
                                    //{
                                    //    makeLog("결과 전송", false, "결품 시 결과 전송 수락");
                                    //    this.UC01_SMSGridViewPage3Top.Add_Data_Parameters["R_E1"] = "Y";
                                    //}
                                }

                                else
                                {
                                    return;
                                }
                            }
                        } while (r_ok != "OK");
                    }

                    ProgressPopupW.Close();

                    if (r_ok == "OK")
                    {
                        makeLog("결과 전송", true, "결과 전송 완료");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show(LanguagePack.Translate("결과 전송이 완료되었습니다."));
                    }

                    else
                    {
                        makeLog("결과 전송", false, r_msg);
                        bowooMessageBox.Show(LanguagePack.Translate("실패한 전송이 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
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
                bowooMessageBox.Show(LanguagePack.Translate("결과 전송에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }

        void tbtnTrans_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            this.tbtnTransAll.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;
            this.tbtnTransYet.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;
            this.tbtnTransDone.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;

            this.tbtnTransAll.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            this.tbtnTransYet.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            this.tbtnTransDone.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;

            RadToggleButton tbtn = sender as RadToggleButton;

            if(tbtn != null)
            {
                toggleSearch = tbtn.Tag.ToString();
                tbtn.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
                PageSearch();
            }

            this.tbtnTransAll.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnTransYet.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnTransDone.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
        }

        void radPageView1_SelectedPageChanged(object sender, EventArgs e)
        {
            PageSearch();
        }

        /// <summary>
        /// 페이지 조회 새로고침.
        /// </summary>
        public override void PageSearch()
        {
            try
            {
                base.ShowLoading();

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = true;
                    mf.radDropDownListBrand.Enabled = true;
                }

                Dictionary<string, object> paramss = new Dictionary<string, object>();
                paramss.Add("@HEADER_PARAMS", "TOGGLE_TRANS=" + toggleSearch.ToString());

                switch (this.radPageView1.SelectedPage.Tag.ToString())
                {
                    case "출고":

                        #region //page1 출고

                        this.tbtnTransAll.Tag = "2";
                        this.tbtnTransYet.Tag = "1";
                        this.tbtnTransDone.Tag = "100";

                        menuId = "S007";
                        menuTitle = "결과전송 - 출고";

                        //데이터 바인딩
                        UC01_SMSGridViewPage1Top.GridViewData.SelectionChanged -= UC01_SMSGridViewPage1Top_SelectionChanged;
                        this.UC01_SMSGridViewPage1Top.BindData(ucTab1Grid1SelectSpName, paramss);

                        if (UC01_SMSGridViewPage1Top.GridViewData.Rows.Count > 0)
                        {
                            UC01_SMSGridViewPage1Top.GridViewData.Rows[0].IsSelected = false;
                            UC01_SMSGridViewPage1Top.GridViewData.SelectionChanged += UC01_SMSGridViewPage1Top_SelectionChanged;
                            UC01_SMSGridViewPage1Top.GridViewData.Rows[0].IsSelected = true;
                        }

                        else
                        {
                            //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                            if (!this.UC01_SMSGridViewPage1Bot.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS"))
                                this.UC01_SMSGridViewPage1Bot.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                            this.UC01_SMSGridViewPage1Bot.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewPage1Top.GetKeyParam();
                            //데이터 바인딩
                            this.UC01_SMSGridViewPage1Bot.BindData(ucTab1Grid2SelectSpName, null);
                        }

                        #endregion

                        break;

                    //case "반품":

                    //    #region //page2 반품

                    //    this.tbtnTransAll.Tag = "2";
                    //    this.tbtnTransYet.Tag = "0";
                    //    this.tbtnTransDone.Tag = "100";

                    //    menuId = "S007";
                    //    menuTitle = "결과전송 - 반품";

                    //    //데이터 바인딩
                    //    //행 변경 이벤트
                    //    UC01_SMSGridViewPage2Left.GridViewData.SelectionChanged -= UC01_SMSGridViewPage2Left_SelectionChanged;
                    //    this.UC01_SMSGridViewPage2Left.BindData(ucTab2Grid1SelectSpName, paramss);

                    //    if (UC01_SMSGridViewPage2Left.GridViewData.Rows.Count > 0)
                    //    {
                    //        UC01_SMSGridViewPage2Left.GridViewData.Rows[0].IsSelected = false;
                    //        UC01_SMSGridViewPage2Left.GridViewData.SelectionChanged += UC01_SMSGridViewPage2Left_SelectionChanged;
                    //        UC01_SMSGridViewPage2Left.GridViewData.Rows[0].IsSelected = true;
                    //    }

                    //    else
                    //    {
                    //        //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                    //        if (!this.UC01_SMSGridViewPage2Right.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS"))
                    //            this.UC01_SMSGridViewPage2Right.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                    //        this.UC01_SMSGridViewPage2Right.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewPage2Left.GetKeyParam();
                    //        //데이터 바인딩
                    //        this.UC01_SMSGridViewPage2Right.BindData(ucTab2Grid2SelectSpName, null);
                    //    }

                    //    #endregion

                    //    break;

                    case "정리":

                        #region //page3 정리

                        this.tbtnTransAll.Tag = "2";
                        this.tbtnTransYet.Tag = "1";
                        this.tbtnTransDone.Tag = "100";

                        menuId = "S007";
                        menuTitle = "결과전송 - 정리";

                        //데이터 바인딩
                        //행 변경 이벤트
                        UC01_SMSGridViewPage3Left.GridViewData.SelectionChanged -= UC01_SMSGridViewPage3Left_SelectionChanged;
                        this.UC01_SMSGridViewPage3Left.BindData(ucTab3Grid1SelectSpName, paramss);

                        if (UC01_SMSGridViewPage3Left.GridViewData.Rows.Count > 0)
                        {

                            UC01_SMSGridViewPage3Left.GridViewData.Rows[0].IsSelected = false;
                            UC01_SMSGridViewPage3Left.GridViewData.SelectionChanged += UC01_SMSGridViewPage3Left_SelectionChanged;
                            UC01_SMSGridViewPage3Left.GridViewData.Rows[0].IsSelected = true;
                        }

                        else
                        {
                            //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                            if (!this.UC01_SMSGridViewPage3Top.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS"))
                                this.UC01_SMSGridViewPage3Top.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                            this.UC01_SMSGridViewPage3Top.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewPage3Left.GetKeyParam();
                            //데이터 바인딩
                            this.UC01_SMSGridViewPage3Top.BindData(ucTab3Grid2SelectSpName, null);
                        }

                        //데이터 바인딩
                        this.UC01_SMSGridViewPage3Bot.BindData(ucTab3Grid3SelectSpName, paramss);

                        #endregion

                        break;

                    default:

                        break;
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("결과 전송 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("조회에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        /// <summary>
        /// 출고탭 행변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewPage1Top_SelectionChanged(object sender, EventArgs e)
        {
            if (this.UC01_SMSGridViewPage1Top.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();

                //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                if (!this.UC01_SMSGridViewPage1Bot.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.UC01_SMSGridViewPage1Bot.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                this.UC01_SMSGridViewPage1Bot.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewPage1Top.GetKeyParam();
                
                //데이터 바인딩
                this.UC01_SMSGridViewPage1Bot.BindData(ucTab1Grid2SelectSpName, null);
                base.HideLoading();
            }
        }

        /// <summary>
        /// 정리탭 행변경 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewPage3Left_SelectionChanged(object sender, EventArgs e)
        {
            if (this.UC01_SMSGridViewPage3Left.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();

                //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                if (!this.UC01_SMSGridViewPage3Top.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.UC01_SMSGridViewPage3Top.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                this.UC01_SMSGridViewPage3Top.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewPage3Left.GetKeyParam();
                
                //데이터 바인딩
                this.UC01_SMSGridViewPage3Top.BindData(ucTab3Grid2SelectSpName, null);
                base.HideLoading();
            }
        }
    }
}