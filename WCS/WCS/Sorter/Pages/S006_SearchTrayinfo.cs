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
    public partial class S006_SearchTrayinfo : lib.Common.Management.BaseControl
    {
        //SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S006_01_L_CHUTE_INFO]";
        string ucGrid2SelectSpName = "[USP_S006_02_L_TRAY_INFO]";

        string ucGrid2forcedProc = "USP_S006_02_B_FORCE_PROC";
        string ucGrid2Indicator = "USP_S006_02_B_INDICATOR";

        int leftselectedIdx = -1;
        int rightselectedIdx = -1;

        public S006_SearchTrayinfo()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            //left side grid
            this.UC01_SMSGridView1.GridTitleText = "슈트 정보";
            this.UC01_SMSGridView1.GridViewData.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;

            //right side grid
            this.UC01_SMSGridView2.GridTitleText = "트레이 정보";
            this.UC01_SMSGridView2.Button1_Visible = true;
            this.UC01_SMSGridView2.Button1_Text = "표시기 표시";
            this.UC01_SMSGridView2.Button1_Click = UC01_SMSGridView2_Button1_Click;
            this.UC01_SMSGridView2.Button2_Visible = true;
            this.UC01_SMSGridView2.Button2_Text = "새로 고침";
            this.UC01_SMSGridView2.Button2_Click = UC01_SMSGridView2_Button2_Click;
            this.UC01_SMSGridView2.Button5_Visible = true;
            this.UC01_SMSGridView2.Button5_Text = "강제 처리";
            this.UC01_SMSGridView2.Button5_Click = UC01_SMSGridView2_Button5_Click;

            this.CreateRefreshCheckBox(5000);

            //이벤트
            this.UC01_SMSGridView1.GridViewData.SelectionChanged += UC01_SMSGridView1_SelectionChanged;
            this.UC01_SMSGridView2.GridViewData.SelectionChanged += UC01_SMSGridView2_SelectionChanged;
        }

        private void UC01_SMSGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.UC01_SMSGridView1.GridViewData.SelectedRows.Count > 0)
            {
                leftselectedIdx = this.UC01_SMSGridView1.GridViewData.SelectedRows[0].Index;
            }
        }

        void UC01_SMSGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (this.UC01_SMSGridView2.GridViewData.SelectedRows.Count > 0)
            {
                rightselectedIdx = this.UC01_SMSGridView2.GridViewData.SelectedRows[0].Index;
            }
        }

        //강제처리버튼
        /// <summary>
        /// 강제 처리 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridView2_Button5_Click(object sender, EventArgs e)
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
                    bowooMessageBox.Show(LanguagePack.Translate("강제 처리할 데이터를 선택하세요."));
                    return;
                }

                bowooConfirmBox.Show(LanguagePack.Translate("강제 처리 진행을 계속 하시겠습니까?"));

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //선택 슈트 정보 data param 추가
                    //if (!this.UC01_SMSGridView2.Add_Data_Parameters.ContainsKey("USE_YN"))
                    //    this.UC01_SMSGridView2.Add_Data_Parameters.Add("USE_YN", "");
                    //this.UC01_SMSGridView2.Add_Data_Parameters["USE_YN"] = this.radDdlChangeWorkSeting.SelectedValue.ToString();

                    if (!this.UC01_SMSGridView2.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.UC01_SMSGridView2.Add_Data_Parameters.Add("R_E1", "");
                    this.UC01_SMSGridView2.Add_Data_Parameters["R_E1"] = "";

                    makeLog("강제 처리", true, "강제 처리 진행 수락");

                    string r_ok = "";
                    string r_msg = "";

                    for (int i = 0; i < chkedIdxs.Count; i++)
                    {
                        do
                        {
                            //SP실행
                            this.UC01_SMSGridView2.ExcuteSaveSp(ucGrid2forcedProc, chkedIdxs[i]);

                            r_ok = this.UC01_SMSGridView2.Usp_Save_Parameters[2].Value.ToString();
                            r_msg += this.UC01_SMSGridView2.Usp_Save_Parameters[3].Value.ToString();

                            if (r_ok != "OK")
                            {
                                if (r_ok == "NG" || r_ok == "E1")
                                {
                                    bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                                    return;
                                }

                                //else
                                //{
                                //    bowooConfirmBox.Show(r_msg);
                                //}

                                ////if (bowooMessageBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                                ////{
                                ////    //E1 : 분류수량 >= 슈트작업FULL수량
                                ////    if (r_ok == "E1")
                                ////    {
                                ////        makeLog("강제 처리", false, "해당 슈트 박스 마감 대기 상태에서 강제 처리 수락");
                                ////        this.UC01_SMSGridView2.Add_Data_Parameters["R_E1"] = "Y";
                                ////    }
                                ////}

                                ////else
                                ////{
                                ////    return;
                                ////}
                            }
                        } while (r_ok != "OK");
                    }

                    if (r_ok == "OK")
                    {
                        makeLog("강제 처리", true, "강제 처리 완료");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show(LanguagePack.Translate("강제 처리가 완료되었습니다."));
                    }

                    else
                    {
                        makeLog("강제 처리", false, r_msg);
                        bowooMessageBox.Show(LanguagePack.Translate("실패한 처리가 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                    }

                    rightSelectedIndex = this.UC01_SMSGridView2.GridViewData.Rows.IndexOf(this.UC01_SMSGridView2.GridViewData.SelectedRows[0]);

                    //데이터 바인딩
                    this.UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);
                    this.UC01_SMSGridView2.SelectRow(rightSelectedIndex);
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("강제 처리", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("강제 처리에 실패하였습니다.\r\n관리자에게 문의하세요."));
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
            PageSearch();
        }

        //표시기 표시 버튼
        /// <summary>
        /// 표시기 표시 버튼
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
                //int chkedCnt = 0;
                //List<int> chkedIdxs = new List<int>();

                //for (int i = 0; i < this.UC01_SMSGridView2.GridViewData.Rows.Count; i++)
                //{
                //    if ((bool)this.UC01_SMSGridView2.GridViewData.Rows[i].Cells["checkbox"].Value)
                //    {
                //        chkedIdxs.Add(i);
                //        chkedCnt++;
                //    }
                //}

                //if (chkedCnt == 0)
                //{
                //    bowooMessageBox.Show("표시기 표시할 데이터를 선택 하세요.");
                //    return;
                //}

                //bowooConfirmBox.Show("표시기 표시 처리 진행을 계속 하시겠습니까?");
                //if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                //{
                    //makeLog("표시기 표시", true, "표시기 표시 진행 수락");

                    string r_ok = "";
                    string r_msg = "";

                    //for (int i = 0; i < chkedIdxs.Count; i++)
                    //{
                        //SP실행
                        this.UC01_SMSGridView2.ExcuteSaveSp(ucGrid2Indicator, -1);

                        r_ok = this.UC01_SMSGridView2.Usp_Save_Parameters[2].Value.ToString();
                        r_msg += this.UC01_SMSGridView2.Usp_Save_Parameters[3].Value.ToString();
                    //}

                    if (r_ok == "OK")
                    {
                        makeLog("표시기 표시", true, "표시기 표시 처리 완료");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show(LanguagePack.Translate("표시기 표시 처리가 완료되었습니다."));
                    }

                    else
                    {
                        makeLog("표시기 표시", false, r_msg);
                        bowooMessageBox.Show(LanguagePack.Translate("실패한 처리가 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                    }

                    rightSelectedIndex = this.UC01_SMSGridView2.GridViewData.Rows.IndexOf(this.UC01_SMSGridView2.GridViewData.SelectedRows[0]);

                    //데이터 바인딩
                    this.UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);
                    this.UC01_SMSGridView2.SelectRow(rightSelectedIndex);
                //}
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("표시기 표시", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("표시기 표시 처리에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //페이지 조회 새로고침.
        /// <summary>
        /// 페이지 조회 새로고침.
        /// </summary>
        public override void PageSearch()
        {
            try
            {
                base.ShowLoading();

                menuId = "S006";
                menuTitle = "트레이 정보 조회";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = false;
                    mf.radDropDownListBrand.Enabled = false;
                }

                //데이터 바인딩
                this.UC01_SMSGridView1.GridViewData.SelectionChanged -= UC01_SMSGridView1_SelectionChanged;
                this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, null);
                this.UC01_SMSGridView1.GridViewData.SelectionChanged += UC01_SMSGridView1_SelectionChanged;
                this.UC01_SMSGridView1.SelectRow(leftselectedIdx);

                //데이터 바인딩
                this.UC01_SMSGridView2.GridViewData.SelectionChanged -= UC01_SMSGridView2_SelectionChanged;
                this.UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);
                this.UC01_SMSGridView2.GridViewData.SelectionChanged += UC01_SMSGridView2_SelectionChanged;
                this.UC01_SMSGridView2.SelectRow(rightselectedIdx);
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("트레이 정보 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("조회에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        public override void StopTimer()
        {
            this.RefreshTimerStop();
        }

        public RadCheckBox radChkRefresh;
        public Timer refreshTimer;

        //interval 초 간격 새로 고침 체크 박스 생성
        public void CreateRefreshCheckBox(int interval)
        {
            radChkRefresh = new RadCheckBox();
            radChkRefresh.Text = (interval / 1000).ToString() + LanguagePack.Translate("초 간격 새로 고침");
            radChkRefresh.Tag = "refresh";
            radChkRefresh.Location = new Point(this.UC01_SMSGridView2.radButton2.Location.X - 120, 5);

            this.UC01_SMSGridView2.radPnlTop.Controls.Add(radChkRefresh);
            radChkRefresh.CheckStateChanged += radChkRefresh_CheckStateChanged;

            refreshTimer = new Timer();
            refreshTimer.Interval = interval;
            refreshTimer.Tick += refreshTimer_Tick;
        }

        //새로고침 틱당 이벤트
        void refreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                //데이터 바인딩
                this.UC01_SMSGridView1.GridViewData.SelectionChanged -= UC01_SMSGridView1_SelectionChanged;
                this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, null);
                this.UC01_SMSGridView1.GridViewData.SelectionChanged += UC01_SMSGridView1_SelectionChanged;
                this.UC01_SMSGridView1.SelectRow(leftselectedIdx);

                //데이터 바인딩
                this.UC01_SMSGridView2.GridViewData.SelectionChanged -= UC01_SMSGridView2_SelectionChanged;
                this.UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);
                this.UC01_SMSGridView2.GridViewData.SelectionChanged += UC01_SMSGridView2_SelectionChanged;
                this.UC01_SMSGridView2.SelectRow(rightselectedIdx);
            }

            catch
            {
                refreshTimer.Stop();
            }
        }

        // 새로고침 체크 변경 시 
        void radChkRefresh_CheckStateChanged(object sender, EventArgs e)
        {
            RadCheckBox chk = sender as RadCheckBox;

            if (chk != null)
            {
                if (chk.Checked)
                {
                    //데이터 바인딩
                    this.UC01_SMSGridView1.GridViewData.SelectionChanged -= UC01_SMSGridView1_SelectionChanged;
                    this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, null);
                    this.UC01_SMSGridView1.GridViewData.SelectionChanged += UC01_SMSGridView1_SelectionChanged;
                    this.UC01_SMSGridView1.SelectRow(leftselectedIdx);

                    //데이터 바인딩
                    this.UC01_SMSGridView2.GridViewData.SelectionChanged -= UC01_SMSGridView2_SelectionChanged;
                    this.UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);
                    this.UC01_SMSGridView2.GridViewData.SelectionChanged += UC01_SMSGridView2_SelectionChanged;
                    this.UC01_SMSGridView2.SelectRow(rightselectedIdx);
                    refreshTimer.Start();
                }

                else
                {
                    refreshTimer.Stop();
                }
            }
        }

        public void RefreshTimerStop()
        {
            //refreshTimer.Stop();

            if (radChkRefresh.Checked) radChkRefresh.Checked = false;
        }
    }
}