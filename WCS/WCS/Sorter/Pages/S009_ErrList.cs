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
using System.Xml;
using System.Data.SqlClient;
using bowoo.Lib;
using bowoo.Lib.DataBase;
using System.Threading.Tasks;

namespace Sorter.Pages
{
    public partial class S009_ErrList : lib.Common.Management.BaseControl
    {
        // SELECT SP 설정
        string ucGrid1SelectSpName = "USP_S010_01_B_RFID_ERR_SEND_LIST";
        string ucGrid2SelectSpName = "USP_S010_01_T_TRAY_TILT_ERR_LIST";
        
        //RadDropDownList radDropList;

        public S009_ErrList()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            // Left Side Grid
            this.UC01_SMSGridViewTop.GridTitleText = "RFID 미전송 리스트";

            //버튼 설정
            //RFID 재전송 버튼.
            UC01_SMSGridViewTop.Button1_Visible = true;
            UC01_SMSGridViewTop.Button1_Text = "전송";
            UC01_SMSGridViewTop.Button1_Click = UC01_SMSGridViewRight_TopButton1_Click;

            //조회조건 숨기기;
            UC01_SMSGridViewTop.HideSearchCondition();

            // Right Side Grid
            this.UC01_SMSGridViewBot.GridTitleText = "TILT에러 리스트";
            //조회조건 숨기기;
            UC01_SMSGridViewBot.HideSearchCondition();



            //버튼 설정
            //RFID 재전송 버튼.
            UC01_SMSGridViewBot.Button1_Visible = true;
            UC01_SMSGridViewBot.Button1_Text = "초기화";
            UC01_SMSGridViewBot.Button1_Click = UC01_SMSGridViewRight_BotButton2_Click;

            //버튼 설정
            //RFID 재전송 버튼.
            UC01_SMSGridViewBot.Button2_Visible = true;
            UC01_SMSGridViewBot.Button2_Text = "강제확정";
            UC01_SMSGridViewBot.Button2_Click = UC01_SMSGridViewRight_BotButton1_Click;

            this.UC01_SMSGridViewBot.GridViewData.HideSelection = true;
        }

        void radDropList_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
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

                menuId = "S010";
                menuTitle = "에러 리스트";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = false;
                    mf.radDropDownListBrand.Enabled = true;
                }

                // 데이터 바인딩
                this.UC01_SMSGridViewTop.BindData(ucGrid1SelectSpName, null);
                this.UC01_SMSGridViewBot.BindData(ucGrid2SelectSpName, null);

            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("상차 관리 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("조회에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        void GridViewData_SelectionChanged(object sender, EventArgs e)
        {
            if (this.UC01_SMSGridViewTop.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();

                if (!this.UC01_SMSGridViewBot.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.UC01_SMSGridViewBot.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                this.UC01_SMSGridViewBot.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewTop.GetKeyParam();

                // 데이터 바인딩
                this.UC01_SMSGridViewBot.BindData(ucGrid2SelectSpName, null);
                base.HideLoading();
            }
        }



        /// <summary>
        /// RFID 전송
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewRight_TopButton1_Click(object sender, EventArgs e)
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

                for (int i = 0; i < this.UC01_SMSGridViewTop.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewTop.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chuteChkedIdxs.Add(i);
                        chuteChkedCnt++;
                        //rowList.Add(UC01_SMSGridViewRight.GridViewData.Rows[i]);
                    }
                }

                if (chuteChkedIdxs.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("재전송할 데이터를 선택해 주세요."));
                    return;
                }



                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                {
                    bowooConfirmBox.Show(LanguagePack.Translate("재전송을 진행 하시겠습니까?"));
                }

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {

                    if (!this.UC01_SMSGridViewTop.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.UC01_SMSGridViewTop.Add_Data_Parameters.Add("R_E1", "");
                    this.UC01_SMSGridViewTop.Add_Data_Parameters["R_E1"] = "";

                    string r_ok = "";
                    string r_msg = "";
                    string r_msg_params = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = chuteChkedIdxs.Count;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    //XmlDocument xml = bowooXml.xmlMake(rowList);



                    for (int i = 0; i < chuteChkedIdxs.Count; i++)
                    {

                        try
                        {
                            //SP실행
                            DataSet tempSet = this.UC01_SMSGridViewTop.ExcuteSaveSp("USP_S010_02_B_RFID_ERR_SEND", chuteChkedIdxs[i]);
                            if (tempSet == null)
                            {
                                makeLog("RFID전송", true, "RFID재전송 실패");
                                //설정 완료 메시지 창
                                bowooMessageBox.Show(LanguagePack.Translate("RFID에 전송을 실패했습니다."));
                                return;
                            }
                            lib.Common.Management.WivisOracle wivisOracle = new lib.Common.Management.WivisOracle(System.Configuration.ConfigurationManager.AppSettings["OracleDatabaseip"].ToString());
                            wivisOracle.rfidInsert(tempSet.Tables[0]);
                            StringBuilder sb = new StringBuilder("update if_box_list set status = 1 where biz_day = '");
                            sb.Append(tempSet.Tables[0].Rows[0]["biz_day"].ToString());
                            sb.Append("' and box_no = ");
                            sb.Append(tempSet.Tables[0].Rows[0]["box_no"].ToString());
                            sb.Append(" and chute_no = ");
                            sb.Append(tempSet.Tables[0].Rows[0]["chute_no"].ToString());
                            sb.Append(" and batch = '");
                            sb.Append(tempSet.Tables[0].Rows[0]["batch"].ToString() + "'");

                            try
                            {
                                DBUtil.ExecuteNonQuery(sb.ToString()); //오라클 데이터 전송후 에러가 없을 경우 box_stauts 를 1로 바꿈.

                            }
                            catch (Exception ex)
                            {
                                lib.Common.Log.LogFile.WriteError(ex, "Query: " + "update if_box_list" + "\n" + ex.Message + ex.StackTrace);
                                throw ex;
                                //return;
                            }
                        }
                        catch (Exception ex)
                        {
                            makeLog("RFID전송", true, "RFID재전송 실패");
                            ////설정 완료 메시지 창
                            bowooMessageBox.Show(LanguagePack.Translate("RFID에 전송을 실패했습니다."));
                            return;

                        }

                    }

                    PageSearch();
                    makeLog("RFID전송", true, "RFID재전송 성공");
                    //설정 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("RFID에 전송을 완료했습니다."));


                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("재발행", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("재발행을 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }

        /// <summary>
        /// 틸트 실패 강제 확정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewRight_BotButton1_Click(object sender, EventArgs e)
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

                for (int i = 0; i < this.UC01_SMSGridViewBot.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewBot.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chuteChkedIdxs.Add(i);
                        chuteChkedCnt++;
                        //rowList.Add(UC01_SMSGridViewRight.GridViewData.Rows[i]);
                    }
                }

                if (chuteChkedIdxs.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("강제확정할 데이터를 선택해 주세요."));
                    return;
                }



                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                {
                    bowooConfirmBox.Show(LanguagePack.Translate("강제확정을 진행 하시겠습니까?"));
                }

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {

                    if (!this.UC01_SMSGridViewBot.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.UC01_SMSGridViewBot.Add_Data_Parameters.Add("R_E1", "");
                    this.UC01_SMSGridViewBot.Add_Data_Parameters["R_E1"] = "";

                    string r_ok = "";
                    string r_msg = "";
                    string r_msg_params = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = chuteChkedIdxs.Count;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    //XmlDocument xml = bowooXml.xmlMake(rowList);



                    for (int i = 0; i < chuteChkedIdxs.Count; i++)
                    {

                        //SP실행
                        this.UC01_SMSGridViewBot.ExcuteSaveSp("USP_S010_03_B_TRAY_TILT_ERR_FOR", chuteChkedIdxs[i]);


                        r_ok = this.UC01_SMSGridViewBot.Usp_Save_Parameters[2].Value.ToString();
                        r_msg += this.UC01_SMSGridViewBot.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                        if (r_ok != "OK")
                        {
                            makeLog("tilt실패 강제확정", true, "tilt강제확정 실패");
                            //설정 완료 메시지 창
                            bowooMessageBox.Show(LanguagePack.Translate("강제확정을 실패했습니다."));
                            return;
                        }

                    }


                    makeLog("tilt실패 강제확정", true, "tilt강제확정 성골");
                    //설정 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("강제확정를 완료했습니다."));
                    this.PageSearch();

                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("재발행", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("재발행을 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }

        /// <summary>
        /// 틸트 실패 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewRight_BotButton2_Click(object sender, EventArgs e)
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

                for (int i = 0; i < this.UC01_SMSGridViewBot.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewBot.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chuteChkedIdxs.Add(i);
                        chuteChkedCnt++;
                        //rowList.Add(UC01_SMSGridViewRight.GridViewData.Rows[i]);
                    }
                }

                if (chuteChkedIdxs.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("재전송할 데이터를 선택해 주세요."));
                    return;
                }



                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                {
                    bowooConfirmBox.Show(LanguagePack.Translate("재전송을 진행 하시겠습니까?"));
                }

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {

                    if (!this.UC01_SMSGridViewBot.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.UC01_SMSGridViewBot.Add_Data_Parameters.Add("R_E1", "");
                    this.UC01_SMSGridViewBot.Add_Data_Parameters["R_E1"] = "";

                    string r_ok = "";
                    string r_msg = "";
                    string r_msg_params = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = chuteChkedIdxs.Count;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    //XmlDocument xml = bowooXml.xmlMake(rowList);



                    for (int i = 0; i < chuteChkedIdxs.Count; i++)
                    {
                        this.UC01_SMSGridViewBot.ExcuteSaveSp("USP_S010_04_B_TRAY_TILT_ERR_RESET", chuteChkedIdxs[i]);


                        r_ok = this.UC01_SMSGridViewBot.Usp_Save_Parameters[2].Value.ToString();
                        r_msg += this.UC01_SMSGridViewBot.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                        if (r_ok != "OK")
                        {
                            makeLog("tilt실패 초기화", true, "tilt초기화 실패");
                            //설정 완료 메시지 창
                            bowooMessageBox.Show(LanguagePack.Translate("초기화를 실패했습니다."));
                            return;
                        }

                    }


                    makeLog("tilt실패 초기화", true, "tilt초기화 성골");
                    //설정 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("초기화를 완료했습니다."));
                    this.PageSearch();

                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("재발행", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("재발행을 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }
    }
}