
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Threading.Tasks;
using Sorter.Pages;
using System.Data.SqlClient;

namespace Sorter.Popup
{
    public partial class S002_ManageLocationPopup_ChangeInfo : lib.Common.Management.BaseForm
    {
        public bool isModified = false;

        string workChuteSettingBtnSpName = "USP_S002_01_B_POP_SET_CHUTE"; //선택 슈트 설정 SP
        string changeDefaultFullQtyBtnSpName = "USP_S002_01_B_POP_CHG_BASE_FULL_QTY"; //기본 FULL 수량 SP
        string changeWorkFullQtyBtnSpName = "USP_S002_01_B_POP_CHG_JOB_FULL_QTY"; //작업 FULL 수량 SP
        string changeGubunCdBtnSpName = "USP_S002_01_B_POP_GubunCd_Change"; //작업 FULL 수량 SP
        string forcedEndBtnSpName = "USP_S002_01_B_POP_FORCE_CLOSE"; //강제 마감 SP
        string chuteInitBtnSpName = "USP_S002_01_B_POP_INITIALIZATION"; //슈트 초기화 SP

        lib.Common.Management.WivisOracle wivisOracle = null;

        public S002_ManageLocationPopup_ChangeInfo()
        {
            ////base.ShowLoading();
            InitializeComponent();
        }

        lib.Common.Management.UC01_GridView uc01_SMSGridView;
        List<int> checkedIdxList;

        public S002_ManageLocationPopup_ChangeInfo(ref lib.Common.Management.UC01_GridView _uc01_SMSGridView, ref List<int> _list)
        {
            InitializeComponent();
            uc01_SMSGridView = _uc01_SMSGridView;
            checkedIdxList = _list;
        }

        private void S002_ManageLocationPopup_ChangeInfo_Load(object sender, EventArgs e)
        {
            menuId = "S002";
            menuTitle = "로케이션 관리 - 정보 변경 팝업";
            radTitleBarPopup.Text = LanguagePack.Translate(radTitleBarPopup.Text);
            radBtnChangeWorkSeting.Text = LanguagePack.Translate(radBtnChangeWorkSeting.Text);
            radBtnDefaultFullQty.Text = LanguagePack.Translate(radBtnDefaultFullQty.Text);
            radBtnWorkFullQty.Text = LanguagePack.Translate(radBtnWorkFullQty.Text);
            radBtnForcedEnd.Text = LanguagePack.Translate(radBtnForcedEnd.Text);
            radBtnInit.Text = LanguagePack.Translate(radBtnInit.Text);
            //this.CenterToParent();

            this.radDdlChangeWorkSeting.ListElement.Font = new System.Drawing.Font("Segoe UI", 13);

            //슈트 작업 설정 콤보 박스.
            radLabel1.Text = LanguagePack.Translate(radLabel1.Text);
            radLabel26.Text = LanguagePack.Translate(radLabel26.Text);

            RadListDataItem item = new RadListDataItem();
            item.Value = "4";
            item.Text = LanguagePack.Translate("정리");
            this.radDdlChangeWorkSeting.Items.Add(item);

            item = new RadListDataItem();
            item.Value = "2";
            item.Text = LanguagePack.Translate("출고");
            this.radDdlChangeWorkSeting.Items.Add(item);


            item = new RadListDataItem();
            item.Value = "5";
            item.Text = LanguagePack.Translate("번들");
            this.radDdlChangeWorkSeting.Items.Add(item);

            radLabel2.Text = LanguagePack.Translate(radLabel2.Text);
            radLabel3.Text = LanguagePack.Translate(radLabel3.Text);
            radLabel27.Text = LanguagePack.Translate(radLabel27.Text);

            //체크된 행 1일 시 체크 행의 작업설정 BInd
            //1행 이상이면 공란
            if (checkedIdxList.Count > 1)
            {
                this.radDdlChangeWorkSeting.SelectedValue = null;
                this.radTxtDefaultFullQty.Text = LanguagePack.Translate("숫자를 입력하세요.");
                this.radTxtWorkFullQty.Text = LanguagePack.Translate("숫자를 입력하세요.");
            }

            else if (checkedIdxList.Count == 1)
            {
                this.radDdlChangeWorkSeting.SelectedText = uc01_SMSGridView.GridViewData.Rows[checkedIdxList[0]].Cells["WORK_TYPE"].Value.ToString();
                this.radTxtDefaultFullQty.Text = uc01_SMSGridView.GridViewData.Rows[checkedIdxList[0]].Cells["FULLQTY"].Value.ToString();
                this.radTxtWorkFullQty.Text = uc01_SMSGridView.GridViewData.Rows[checkedIdxList[0]].Cells["MAXQTY"].Value.ToString();
            }

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

            radLabel4.Text = LanguagePack.Translate(radLabel4.Text);
            radLabel28.Text = LanguagePack.Translate(radLabel28.Text);

            if (endBtnEnable == "1")
            {
                this.radLabel4.Text = LanguagePack.Translate("정리 마감");
                this.radLabel28.Text = LanguagePack.Translate("※ 선택된 데이터가 강제 마감 처리됩니다. 출고 슈트는 마감처리 되지 않습니다.");
                this.radBtnForcedEnd.Text = LanguagePack.Translate("정리 마감");
            }

            //매장이 설정 되지 않은 슈트 초기화 버튼 비활성화
            radLabel5.Text = LanguagePack.Translate(radLabel5.Text);
            radLabel30.Text = LanguagePack.Translate(radLabel30.Text);

            //이벤트
            this.radTxtDefaultFullQty.Click += radTxt_Click;
            this.radTxtWorkFullQty.Click += radTxt_Click;

            this.radTxtDefaultFullQty.TextChanged += radTxt_TextChanged;
            this.radTxtWorkFullQty.TextChanged += radTxt_TextChanged;


        }

        void radTxt_TextChanged(object sender, EventArgs e)
        {
            RadTextBox textbox = sender as RadTextBox;

            if (textbox == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(textbox.Text))
            {
                return;
            }

            //수량 Validation
            if (!lib.Common.ValidationEx.IsNumber(textbox.Text))
            {
                bowooMessageBox.Show(LanguagePack.Translate("숫자를 입력하세요."));
                textbox.Text = "";
                return;
            }

            if (int.Parse(textbox.Text) < 1)
            {
                bowooMessageBox.Show(LanguagePack.Translate("1 이상의 숫자를 입력하세요."));
                textbox.Text = "";
                return;
            }
        }

        void radTxt_Click(object sender, EventArgs e)
        {
            RadTextBox textbox = sender as RadTextBox;

            if (textbox == null)
            {
                return;
            }

            if (textbox.Text == LanguagePack.Translate("숫자를 입력하세요."))
            {
                textbox.Text = "";
            }
        }

        //작업설정 변경
        /// <summary>
        /// 작업설정 변경 설정 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnChangeWorkSeting_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            try
            {
                if (this.radDdlChangeWorkSeting.SelectedValue == null || string.IsNullOrEmpty(this.radDdlChangeWorkSeting.SelectedValue.ToString()))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("해당 슈트의 작업 타입을 선택하세요."));
                    return;
                }

                bowooConfirmBox.Show(string.Format(LanguagePack.Translate("선택 슈트의 작업 타입을 {0}로 설정하시겠습니까?"), this.radDdlChangeWorkSeting.SelectedItem.Text.ToString()));

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //선택 슈트 정보 data param 추가
                    if (!this.uc01_SMSGridView.Add_Data_Parameters.ContainsKey("WORK_TYPE"))
                        this.uc01_SMSGridView.Add_Data_Parameters.Add("WORK_TYPE", "");
                    this.uc01_SMSGridView.Add_Data_Parameters["WORK_TYPE"] = this.radDdlChangeWorkSeting.SelectedValue.ToString();

                    string r_ok = "";
                    string r_msg = "";
                    string r_msg_params = "";

                    for (int i = 0; i < checkedIdxList.Count; i++)
                    {
                        uc01_SMSGridView.Usp_Save_Parameters = new SqlParameter[5];
                        uc01_SMSGridView.Usp_Save_Parameters[4] = new SqlParameter();
                        uc01_SMSGridView.Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
                        uc01_SMSGridView.Usp_Save_Parameters[4].Size = 8000;
                        uc01_SMSGridView.Usp_Save_Parameters[4].Value = "";
                        uc01_SMSGridView.Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

                        //SP실행
                        this.uc01_SMSGridView.ExcuteSaveSp(workChuteSettingBtnSpName, checkedIdxList[i]);

                        r_ok = this.uc01_SMSGridView.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.uc01_SMSGridView.Usp_Save_Parameters[3].Value.ToString();

                        if (r_ok != "OK")
                        {
                            string[] r_split = r_msg_params.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                            makeLog("작업 설정 변경", false, "작업 설정 변경 실패, " + r_msg);
                            //bowooMessageBox.Show(LanguagePack.Translate("실패한 설정이 존재합니다.\r\n") + string.Format(LanguagePack.Translate(r_msg), r_split[0], r_split[1], r_split[2], r_split[3]));
                            bowooMessageBox.Show(LanguagePack.Translate("실패한 설정이 존재합니다.\r\n") + r_msg);
                            return;
                        }
                    }

                    if (r_ok == "OK")
                    {
                        //설정 완료 메시지 창
                        makeLog("작업 설정 변경", true, "작업 설정 변경 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("설정이 완료되었습니다."));
                        isModified = true;
                        this.Close();
                    }
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("작업 설정 변경", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("설정에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //기본FULLQTY 변경
        /// <summary>
        /// 기본 FULL 수량 변경 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnDefaultFullQty_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            try
            {
                if (string.IsNullOrEmpty(radTxtDefaultFullQty.Text))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("수량을 입력하세요."));
                    return;
                }

                bowooConfirmBox.Show(LanguagePack.Translate("입력하신 수량으로 기본 FULL 수량을 변경하시겠습니까?"));

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //선택 슈트 정보 data param 추가
                    if (!this.uc01_SMSGridView.Add_Data_Parameters.ContainsKey("FULLQTY"))
                        this.uc01_SMSGridView.Add_Data_Parameters.Add("FULLQTY", "");
                    this.uc01_SMSGridView.Add_Data_Parameters["FULLQTY"] = this.radTxtDefaultFullQty.Text.ToString();

                    string r_ok = "";
                    string r_msg = "";

                    for (int i = 0; i < checkedIdxList.Count; i++)
                    {

                        uc01_SMSGridView.Usp_Save_Parameters = new SqlParameter[4];


                        //SP실행
                        this.uc01_SMSGridView.ExcuteSaveSp(changeDefaultFullQtyBtnSpName, checkedIdxList[i]);

                        r_ok = this.uc01_SMSGridView.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.uc01_SMSGridView.Usp_Save_Parameters[3].Value.ToString();

                        if (r_ok != "OK")
                        {
                            makeLog("기본 FULL 수량 변경", false, "기본 FULL 수량 변경 실패, " + r_msg);
                            bowooMessageBox.Show(LanguagePack.Translate("실패한 설정이 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                            return;
                        }
                    }

                    if (r_ok == "OK")
                    {
                        //설정 완료 메시지 창
                        makeLog("기본 FULL 수량 변경", true, "기본 FULL 수량 변경 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("설정이 완료되었습니다."));
                        isModified = true;
                        this.Close();
                    }
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("기본 FULL 수량 변경", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("설정에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();

            }
        }

        //작업FULLQTY 변경
        /// <summary>
        /// 작업 FULL 수량 변경 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnWorkFullQty_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            try
            {
                //VALIDATION
                if (string.IsNullOrEmpty(radBtnWorkFullQty.Text))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("수량을 입력하세요."));
                    return;
                }

                //변경 여부 DIALOG
                bowooConfirmBox.Show(LanguagePack.Translate("입력하신 수량으로 작업 FULL 수량을 변경하시겠습니까?"));

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //선택 슈트 정보 data param 추가
                    if (!this.uc01_SMSGridView.Add_Data_Parameters.ContainsKey("MAXQTY"))
                        this.uc01_SMSGridView.Add_Data_Parameters.Add("MAXQTY", "");
                    this.uc01_SMSGridView.Add_Data_Parameters["MAXQTY"] = this.radTxtWorkFullQty.Text.ToString();


                    string r_ok = "";
                    string r_msg = "";

                    for (int i = 0; i < checkedIdxList.Count; i++)
                    {
                        uc01_SMSGridView.Usp_Save_Parameters = new SqlParameter[4];

                        //SP실행
                        this.uc01_SMSGridView.ExcuteSaveSp(changeWorkFullQtyBtnSpName, checkedIdxList[i]);

                        r_ok = this.uc01_SMSGridView.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.uc01_SMSGridView.Usp_Save_Parameters[3].Value.ToString();

                        if (r_ok != "OK")
                        {
                            makeLog("작업 FULL 수량 변경", false, "작업 FULL 수량 변경 실패, " + r_msg);
                            bowooMessageBox.Show(LanguagePack.Translate("실패한 설정이 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                            return;

                        }
                    }

                    if (r_ok == "OK")
                    {
                        //설정 완료 메시지 창
                        makeLog("작업 FULL 수량 변경", true, "작업 FULL 수량 변경 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("설정이 완료되었습니다."));
                        isModified = true;
                        this.Close();
                    }
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("작업 FULL 수량 변경", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("설정에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();

            }
        }

        //배송처 구분 코드 변경
        /// <summary>
        /// 배송처 구분 코드 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnWorkGubunCd_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            try
            {
                //VALIDATION
                if (string.IsNullOrEmpty(radBtnWorkFullQty.Text))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("배송처 구분코드를 입력 해 주세요."));
                    return;
                }

                //변경 여부 DIALOG
                bowooConfirmBox.Show(LanguagePack.Translate("입력하신 배송처 코드코드로 변경하시겠습니까?"));

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    //선택 슈트 정보 data param 추가
                    if (!this.uc01_SMSGridView.Add_Data_Parameters.ContainsKey("GUNUN_CD"))
                        this.uc01_SMSGridView.Add_Data_Parameters.Add("GUNUN_CD", "");
                    this.uc01_SMSGridView.Add_Data_Parameters["GUNUN_CD"] = this.gubunCd.Text.ToString().ToUpper();


                    string r_ok = "";
                    string r_msg = "";

                    for (int i = 0; i < checkedIdxList.Count; i++)
                    {
                        uc01_SMSGridView.Usp_Save_Parameters = new SqlParameter[4];

                        //SP실행
                        this.uc01_SMSGridView.ExcuteSaveSp(changeGubunCdBtnSpName, checkedIdxList[i]);

                        r_ok = this.uc01_SMSGridView.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.uc01_SMSGridView.Usp_Save_Parameters[3].Value.ToString();

                        if (r_ok != "OK")
                        {
                            makeLog("배송처 구분 코드 변경", false, "배송처 구분 코드 변경 실패, " + r_msg);
                            bowooMessageBox.Show(LanguagePack.Translate("실패한 설정이 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                            return;
                        }
                    }

                    if (r_ok == "OK")
                    {
                        //설정 완료 메시지 창
                        makeLog("배송처 구분 코드 변경", true, "배송처 구분 코드 변경 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("설정이 완료되었습니다."));
                        isModified = true;
                        this.Close();
                    }
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("배송처 구분 코드 변경", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("설정에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();

            }
        }

        //강제마감
        /// <summary>
        /// 강제마감 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnForcedEnd_Click(object sender, EventArgs e)
        {
            //base.ShowLoading();

            //마감 에러난 슈트 정보.
            int chute_Force_errCount = 0;
            string dirPath = System.Configuration.ConfigurationManager.AppSettings["force_errFileDir"].ToString(); //에러 내용 저장할 파일이름

            try
            {
                //변경 여부 DIALOG
                bowooRedConfirmBox.Show(LanguagePack.Translate("강제 마감 처리 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    makeLog("강제 마감", true, "강제 마감 처리 진행 수락");

                    string r_ok = "";
                    string r_msg = "";
                    string r_msg_params = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = checkedIdxList.Count;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    //선택 슈트 정보 data param 추가
                    if (!this.uc01_SMSGridView.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.uc01_SMSGridView.Add_Data_Parameters.Add("R_E1", "");
                    this.uc01_SMSGridView.Add_Data_Parameters["R_E1"] = "";
                    if (!this.uc01_SMSGridView.Add_Data_Parameters.ContainsKey("R_E2"))
                        this.uc01_SMSGridView.Add_Data_Parameters.Add("R_E2", "");
                    this.uc01_SMSGridView.Add_Data_Parameters["R_E2"] = "";

                    for (int i = 0; i < checkedIdxList.Count; i++)
                    {
                        lib.Common.Management.Class.RfidSend wivis = new lib.Common.Management.Class.RfidSend();
                        int chute_no = Convert.ToInt32(uc01_SMSGridView.GridViewData.Rows[checkedIdxList[i]].Cells["chute_no"].Value);
                        string shop_cd = uc01_SMSGridView.GridViewData.Rows[checkedIdxList[i]].Cells["shop_cd"].Value.ToString();
                        if (shop_cd == "") continue;

                        object[] returnvalue = wivis.invoiceSerch(chute_no);
                        //object[] returnvalue = null;
                        //값이 null 일경우는 한진이 아닌 경우
                        if (returnvalue == null)
                        {
                            returnvalue = new object[5];
                            returnvalue[0] = ""; // 송장 번호
                            returnvalue[1] = ""; // 터미널 코드
                            returnvalue[2] = ""; // 터미널 명
                        }
                        //else if()
                        //{
                        //    returnvalue[0] = "testinvoice"; // 송장 번호
                        //    returnvalue[1] = "1564"; // 터미널 코드
                        //    returnvalue[2] = "테스트"; // 터미널 명
                        //}


                        if (!this.uc01_SMSGridView.Add_Data_Parameters.ContainsKey("V_INVOICE"))
                            this.uc01_SMSGridView.Add_Data_Parameters.Add("V_INVOICE", "");
                        this.uc01_SMSGridView.Add_Data_Parameters["V_INVOICE"] = returnvalue[0].ToString();

                        if (!this.uc01_SMSGridView.Add_Data_Parameters.ContainsKey("V_TML_CD"))
                            this.uc01_SMSGridView.Add_Data_Parameters.Add("V_TML_CD", "");
                        this.uc01_SMSGridView.Add_Data_Parameters["V_TML_CD"] = returnvalue[1].ToString();

                        if (!this.uc01_SMSGridView.Add_Data_Parameters.ContainsKey("V_TML_NM"))
                            this.uc01_SMSGridView.Add_Data_Parameters.Add("V_TML_NM", "");
                        this.uc01_SMSGridView.Add_Data_Parameters["V_TML_NM"] = returnvalue[2].ToString();

                        uc01_SMSGridView.Usp_Save_Parameters = new SqlParameter[7];
                        uc01_SMSGridView.Usp_Save_Parameters[4] = new SqlParameter();
                        uc01_SMSGridView.Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
                        uc01_SMSGridView.Usp_Save_Parameters[4].Size = 8000;
                        uc01_SMSGridView.Usp_Save_Parameters[4].Value = "";
                        uc01_SMSGridView.Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

                        uc01_SMSGridView.Usp_Save_Parameters[5] = new SqlParameter();
                        uc01_SMSGridView.Usp_Save_Parameters[5].ParameterName = "@O_LABEL_POS";
                        uc01_SMSGridView.Usp_Save_Parameters[5].DbType = DbType.Int32;
                        uc01_SMSGridView.Usp_Save_Parameters[5].Value = "";
                        uc01_SMSGridView.Usp_Save_Parameters[5].Direction = ParameterDirection.Output;

                        uc01_SMSGridView.Usp_Save_Parameters[6] = new SqlParameter();
                        uc01_SMSGridView.Usp_Save_Parameters[6].ParameterName = "@O_PrintMsg";
                        uc01_SMSGridView.Usp_Save_Parameters[6].Size = 8000;
                        uc01_SMSGridView.Usp_Save_Parameters[6].Value = "";
                        uc01_SMSGridView.Usp_Save_Parameters[6].Direction = ParameterDirection.Output;

                        //SP실행
                        DataSet tempSet = this.uc01_SMSGridView.ExcuteSaveSp(forcedEndBtnSpName, checkedIdxList[i]);

                        r_ok = this.uc01_SMSGridView.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.uc01_SMSGridView.Usp_Save_Parameters[3].Value.ToString();

                        ProgressPopupW.progressBar1.PerformStep();

                        if (r_ok != "OK")
                        {
                            if (r_ok == "E1") //마감할 제품이없는 비어있는 슈트
                            {
                                continue;
                            }
                            else if (r_ok == "NG") //마감에러
                            {
                                chute_Force_errCount++;

                                lib.Common.Log.LogFile.WriteLogLocal(dirPath, "마감 에러", r_msg + $"슈트번호{chute_no}");
                                continue;

                            }
                            else
                            {
                                continue;
                                //bowooRedConfirmBox.Show(LanguagePack.Translate(r_msg));
                            }

                        }
                        else
                        {
                            //RFID 전송및 프린트 사무실에서 테스트 하기위해 주석
                            lib.Common.Management.Class.RfidSend wivisRfid = new lib.Common.Management.Class.RfidSend();
                            wivisRfid.rfid_send(tempSet, uc01_SMSGridView.Usp_Save_Parameters[6].Value.ToString(), Convert.ToInt32(uc01_SMSGridView.Usp_Save_Parameters[5].Value));

                            //wivisRfid.printMethode(uc01_SMSGridView.Usp_Save_Parameters[6].Value.ToString(), 0);
                        }
                    }

                    if (chute_Force_errCount < 1)
                    {
                        //설정 완료 메시지 창
                        //tran.Commit();
                        makeLog("강제 마감", true, "강제 마감 처리 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("강제 마감 처리가 완료되었습니다."));
                        isModified = true;
                    }
                    else
                    {
                        makeLog("강제 마감", false, "강제 마감 처리 실패, " + r_msg);
                        string show_msg = string.Format(LanguagePack.Translate("슈트 마감이 실패한슈트가 있습니다.\r\n마감 실패한 슈트는 총({0}) 입니다."), chute_Force_errCount);
                        bowooMessageBox.Show(show_msg);
                    }

                    this.Close();
                }
            }

            catch (Exception exc)
            {
                //*로그*
                //tran.Rollback();
                makeLog("강제 마감", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("강제 마감 처리에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {

                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }

        //슈트 초기화
        /// <summary>
        /// 슈트초기화 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnInit_Click(object sender, EventArgs e)
        {
            //base.ShowLoading();
            try
            {
                //변경 여부 DIALOG
                bowooRedConfirmBox.Show(LanguagePack.Translate("선택하신 슈트의 설정 정보가 모두 삭제됩니다.\r\n\r\n초기화 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    makeLog("슈트 초기화", true, "슈트 초기화 진행 수락");

                    string r_ok = "";
                    string r_msg = "";
                    string r_msg_params = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = checkedIdxList.Count;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    for (int i = 0; i < checkedIdxList.Count; i++)
                    {
                        uc01_SMSGridView.Usp_Save_Parameters = new SqlParameter[5];
                        uc01_SMSGridView.Usp_Save_Parameters[4] = new SqlParameter();
                        uc01_SMSGridView.Usp_Save_Parameters[4].ParameterName = "@R_MESSAGE_PARAMS";
                        uc01_SMSGridView.Usp_Save_Parameters[4].Size = 8000;
                        uc01_SMSGridView.Usp_Save_Parameters[4].Value = "";
                        uc01_SMSGridView.Usp_Save_Parameters[4].Direction = ParameterDirection.Output;

                        //SP실행
                        this.uc01_SMSGridView.ExcuteSaveSp(chuteInitBtnSpName, checkedIdxList[i]);

                        r_ok = this.uc01_SMSGridView.Usp_Save_Parameters[2].Value.ToString();
                        r_msg = this.uc01_SMSGridView.Usp_Save_Parameters[3].Value.ToString();
                        r_msg_params = this.uc01_SMSGridView.Usp_Save_Parameters[4].Value.ToString();

                        ProgressPopupW.progressBar1.PerformStep();

                        if (r_ok != "OK")
                        {
                            string[] r_split = r_msg_params.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                            makeLog("초기화", false, "초기화 실패, " + r_msg);
                            bowooMessageBox.Show(LanguagePack.Translate("실패한 처리가 존재합니다.\r\n") + string.Format(LanguagePack.Translate(r_msg), r_split[0], r_split[1], r_split[2], r_split[3]));
                            return;
                        }
                    }

                    if (r_ok == "OK")
                    {
                        //설정 완료 메시지 창
                        makeLog("초기화", true, "초기화 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("초기화가 완료되었습니다."));
                        isModified = true;
                    }

                    this.Close();
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("초기화", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("초기화에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }


        /// <summary>
        /// 프린트 정보를 파싱해서 프린트 클래스에 넘김.
        /// </summary>
        /// <param name="printList"></param>
        private void printMethode(string printList, int label_no)
        {

            Task.Run(() =>
            {
                string[] printString_arr = null; //라벨 출력할 데이터를 담는 변수.
                printString_arr = printList.ToString().Split('/');
                //앞뒤 공백제거
                for (int i = 0; i < printString_arr.Length; i++)
                {
                    printString_arr[i] = printString_arr[i].TrimEnd().TrimStart();
                }
                TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                //p.print(printList, label_no);
                p.print(printString_arr, "LABEL_" + label_no.ToString().PadLeft(2, '0'));
            });
        }

    }
}
