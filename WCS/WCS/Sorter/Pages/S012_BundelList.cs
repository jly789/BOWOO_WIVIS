using bowoo.Framework.common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sorter.Pages
{
    public partial class S012_BundelList : lib.Common.Management.BaseControl
    {
        // SELECT SP 설정
        string ucGrid1SelectSpName = "USP_S012_L_01_Bundel_LIST";
        string ucGrid1CloseSpName = "[USP_S012_B_01_Bundel_CLOSE]"; //ㅅㅑㅍㅋㅗㄷㅡ ㅂㅏㄷㅇㅏㅇㅗㄱㅣ
        string ucGrid1CloseSpName2 = "[USP_S012_B_02_Bundel_CLOSE]"; //ㅂㅓㄴㄷㅡㄹㅁㅏㄱㅏㅁ
        string ucGrid1ClearSpName = "[USP_S012_B_02_Bundel_BOX_CLEAR]";
        string ucGrid1Remain_PrintSpName = "[USP_S012_B_03_Bundel_REMAIN_PRINT]";
        string remain_day = string.Empty;
        string remain_batch = string.Empty;
        DataSet pickingSet = new DataSet();

        public string seleted_print = string.Empty; //재발행할 프린트 종류
        //RadDropDownList radDropList;

        public S012_BundelList()
        {
            InitializeComponent();
            //RadPanelContents.Visible = true;

            //버튼 설정
            //UC01_SMSGridViewRight.Button1_Visible = true;
            //UC01_SMSGridViewRight.Button1_Text = "재발행";
            //UC01_SMSGridViewRight.Button1_Click = UC01_SMSGridViewRight_TopButton1_Click;



            // Left Side Grid
            this.UC01_SMSGridViewLeft.GridTitleText = "번들리스트";

            //버튼 설정
            UC01_SMSGridViewLeft.Button1_Visible = true;
            UC01_SMSGridViewLeft.Button1_Text = "발행";
            UC01_SMSGridViewLeft.Button1_Click = UC01_SMSGridViewLeft_TopButton1_Click;


            UC01_SMSGridViewLeft.Button2_Visible = true;
            UC01_SMSGridViewLeft.Button2_Text = "검색";
            UC01_SMSGridViewLeft.Button2_Click = seaerch;

            UC01_SMSGridViewLeft.Button3_Visible = true;
            UC01_SMSGridViewLeft.Button3_Text = "번들마감";
            UC01_SMSGridViewLeft.Button3_Click = close_the_bundle;

            //번들 마감한 박스 마감 취소
            UC01_SMSGridViewLeft.Button4_Visible = true;
            UC01_SMSGridViewLeft.Button4_Text = "번들초기화";
            UC01_SMSGridViewLeft.Button4_Click = clear_the_bundle;

            UC01_SMSGridViewLeft.Button5_Visible = true;
            UC01_SMSGridViewLeft.Button5_Text = "재발행";
            UC01_SMSGridViewLeft.Button5_Click = remain_print_bundle;

        }

        void radDropList_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            for (int i = 0; i < radCheckedDropDownList1.Items.Count(); i++)
            {
                radCheckedDropDownList1.CheckedMember.ToString();
            }
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

                Dictionary<string, object> paramss = new Dictionary<string, object>();
                menuId = "S012";
                menuTitle = "번들리스트";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = true;
                    mf.radDropDownListBrand.Enabled = true;
                }
                //user=SA;#auth=0;#bizday=20210107;#brand=Z;#wrkseq=

                string tt = BaseEntity.sessInq;
                string biz_day = string.Empty;
                string batch = string.Empty;
                string[] stringArr = BaseEntity.sessInq.Split(new char[] { ';', '#' });
                for (int i = 0; i < stringArr.Length; i++)
                {
                    if (stringArr[i].Contains("bizday"))
                    {
                        int startIndex = stringArr[i].IndexOf('=');
                        biz_day = stringArr[i].Substring(startIndex + 1, stringArr[i].Length - startIndex - 1);

                    }
                    else if (stringArr[i].Contains("wrkseq"))
                    {

                        int startIndex = stringArr[i].IndexOf('=');
                        batch = stringArr[i].Substring(startIndex + 1, stringArr[i].Length - startIndex - 1);
                    }
                }
                string ssql = string.Empty;

                if (remain_day != biz_day || remain_batch != batch)
                {
                    comboBox1.DataSource = null;
                    radCheckedDropDownList1.Items.Clear();
                    if (batch == string.Empty) batch = "001";
                    ssql = @"select substring(item_style,1,1)
							from IF_ORDER
							where BIZ_DAY LIKE '" + biz_day + "' ";
                    ssql += "and BATCH LIKE CASE WHEN '" + batch + "' = '000' THEN '%' ELSE '" + batch + "' end ";

                    ssql += @"group by substring(item_style,1,1) 

							select substring(item_style,3,2) 
							from IF_ORDER
							where BIZ_DAY = '" + biz_day + "' ";

                    ssql += "and BATCH LIKE CASE WHEN '" + batch + "' = '000' THEN '%' ELSE '" + batch + "' end ";
                    ssql += "group by substring(item_style,3,2)";
                    remain_day = biz_day;
                    remain_batch = batch;
                    DataSet ds = DBUtil.ExecuteDataSet(ssql);
                    string all_string = string.Empty;

                    Dictionary<string, string> type = new Dictionary<string, string>();
                    type.Add("%", "all");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        type.Add(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][0].ToString());
                    }

                    comboBox1.DataSource = new BindingSource(type, null);
                    comboBox1.DisplayMember = "value";
                    comboBox1.ValueMember = "key";
                    comboBox1.SelectedItem = "all"; //처음 로딩시 콤보박스 기본선택을 전체로 하기 위함.


                    //for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    //{
                    //    radCheckedDropDownList1.Items.Add(ds.Tables[1].Rows[i][0].ToString());
                    //}
                }


                if (radCheckedDropDownList1.SelectedItems.Count == 0 || comboBox1.SelectedItem == null)
                {


                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < radCheckedDropDownList1.Items.Count(); i++)
                    {
                        if (i == radCheckedDropDownList1.Items.Count() - 1)
                        {
                            sb.Append(radCheckedDropDownList1.Items[i].ToString());
                        }
                        else
                        {
                            sb.Append(radCheckedDropDownList1.Items[i].ToString() + ",");

                        }
                    }
                    paramss.Add("@HEADER_PARAMS", ";#SexCd=" + comboBox1.SelectedValue.ToString() + ";#item_cd =" + sb.ToString());
                }
                else
                {

                    //Dictionary<string, object> paramss = new Dictionary<string, object>();
                    List<string> checkList = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    if (radCheckedDropDownList1.CheckedItems.Count() > 0)
                    {
                        for (int i = 0; i < radCheckedDropDownList1.CheckedItems.Count(); i++)
                        {
                            if (i == radCheckedDropDownList1.CheckedItems.Count() - 1)
                            {
                                sb.Append(radCheckedDropDownList1.CheckedItems[i].ToString());
                            }
                            else
                            {
                                sb.Append(radCheckedDropDownList1.CheckedItems[i].ToString() + ",");

                            }
                        }

                    }
                    else
                    {

                        for (int i = 0; i < radCheckedDropDownList1.Items.Count(); i++)
                        {
                            if (i == radCheckedDropDownList1.Items.Count() - 1)
                            {
                                sb.Append(radCheckedDropDownList1.Items[i].ToString());
                            }
                            else
                            {
                                sb.Append(radCheckedDropDownList1.Items[i].ToString() + ",");

                            }
                        }
                    }
                    paramss.Add("@HEADER_PARAMS", ";#SexCd=" + comboBox1.SelectedValue.ToString() + ";#item_cd =" + sb.ToString());
                }


                paramss.Add("@GRID_PARAMS", UC01_SMSGridViewLeft.GetGridParams());
                this.UC01_SMSGridViewLeft.BindData(ucGrid1SelectSpName, paramss);
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("번들리스트 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("조회에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }
            finally
            {
                base.HideLoading();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>복종값
        private void style_select(string value)
        {
            string ssql = string.Empty;

            string biz_day = string.Empty;
            string batch = string.Empty;
            string[] stringArr = BaseEntity.sessInq.Split(new char[] { ';', '#' });
            for (int i = 0; i < stringArr.Length; i++)
            {
                if (stringArr[i].Contains("bizday"))
                {
                    int startIndex = stringArr[i].IndexOf('=');
                    biz_day = stringArr[i].Substring(startIndex + 1, stringArr[i].Length - startIndex - 1);

                }
                else if (stringArr[i].Contains("wrkseq"))
                {

                    int startIndex = stringArr[i].IndexOf('=');
                    batch = stringArr[i].Substring(startIndex + 1, stringArr[i].Length - startIndex - 1);
                }
            }

            ssql = @"	select substring(item_style,3,2) 
					from IF_ORDER
					where BIZ_DAY LIKE '" + biz_day + "' " +
                    "AND ASSORT_CD <> '' ";
            ssql += "and substring(item_style,1,1) like '" + value + "' ";
            ssql += "and BATCH LIKE CASE WHEN '" + batch + "' = '000' THEN '%' ELSE '" + batch + "' end ";
            ssql += "group by substring(item_style,3,2)";
            remain_day = biz_day;
            remain_batch = batch;
            DataSet ds = DBUtil.ExecuteDataSet(ssql);

            radCheckedDropDownList1.Items.Clear();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                radCheckedDropDownList1.Items.Add(ds.Tables[0].Rows[i][0].ToString());
            }
        }

        void GridViewData_SelectionChanged(object sender, EventArgs e)
        {
            if (this.UC01_SMSGridViewLeft.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();

                //if (!this.UC01_SMSGridViewRight.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.UC01_SMSGridViewRight.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                //this.UC01_SMSGridViewRight.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewLeft.GetKeyParam();

                //// 데이터 바인딩
                //this.UC01_SMSGridViewRight.BindData(ucGrid2SelectSpName, null);
                base.HideLoading();
            }
        }

        private void seaerch(object sender, EventArgs e)
        {

            this.PageSearch();
        }


        private void close_the_bundle(object sender, EventArgs e)
        {


            MainForm mf = this.FindForm() as Sorter.MainForm;
            if (mf.radddlwrkseq.SelectedValue.ToString() == "000")
            {
                bowooMessageBox.Show(LanguagePack.Translate("작업 차수를 선택 해 주세요."));
                return;
            }

            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            //슈트 설정 리스트 체크 카운트
            List<string> check_barcode = new List<string>();
            int bxoEndCount = 0; //박스 마감할 번들 수량
            List<string> check_bundleCode = new List<string>();
            List<string> check_bundlestyle = new List<string>();

            try
            {
                for (int i = 0; i < this.UC01_SMSGridViewLeft.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        check_barcode.Add(UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["BARCODE"].Value.ToString());
                        check_bundleCode.Add(UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["ASSORT_CD"].Value.ToString());
                        check_bundlestyle.Add(UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["ITEM_STYLE"].Value.ToString());
                        bxoEndCount = bxoEndCount > Convert.ToInt32(this.UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["REMAIN_QTY"].Value) ? bxoEndCount : Convert.ToInt32(this.UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["REMAIN_QTY"].Value);
                    }
                }

                if (check_barcode.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("번들 마감할 제품을 선택 해 주세요."));
                    return;
                }

                check_bundlestyle = check_bundlestyle.Distinct().ToList();
                if (check_bundlestyle.Count > 1)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("번들 스타일이 같은 제품을 선택 해 주세요."));
                    return;
                }

                check_bundleCode = check_bundleCode.Distinct().ToList();
                if (check_bundleCode.Count > 1)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("번들 유형이 같은 제품을 선택 해 주세요."));
                    return;
                }



                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                {
                    bowooConfirmBox.Show(LanguagePack.Translate("번들 마감을 진행 하시겠습니까?"));
                }

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    check_barcode = check_barcode.Distinct().ToList(); //중복 바코드 제거
                    DataTable tempdt = new DataTable();
                    tempdt.Columns.Add("barcode");

                    for (int i = 0; i < check_barcode.Count; i++)
                    {
                        DataRow temprow = tempdt.NewRow();
                        temprow["barcode"] = check_barcode[i];
                        tempdt.Rows.Add(temprow);
                    }

                    lib.Common.Management.Xml xml = new lib.Common.Management.Xml();
                    System.Xml.XmlDocument tempXml = xml.xmlMake(tempdt);


                    if (bxoEndCount == 0)
                    {
                        bowooMessageBox.Show(LanguagePack.Translate("번들 마감이 가능한 제품이 없습니다."));
                        return;
                    }

                    lib.Common.Management.Class.RfidSend rfid = new lib.Common.Management.Class.RfidSend();

                    string r_ok = "";
                    string r_msg = "";
                    string r_msg_params = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = bxoEndCount;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    SqlParameter[] parmData = new SqlParameter[4];
                    parmData[0] = new SqlParameter();
                    parmData[0].ParameterName = "@HEADER_PARAMS";
                    parmData[0].DbType = DbType.String;
                    parmData[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터

                    parmData[1] = new SqlParameter();
                    parmData[1].ParameterName = "BARCODEXML"; //반환값
                    parmData[1].DbType = DbType.Xml;
                    parmData[1].Value = tempXml.InnerXml;
                    parmData[1].Direction = ParameterDirection.Input;

                    parmData[2] = new SqlParameter();
                    parmData[2].ParameterName = "@R_OK"; //반환값
                    parmData[2].Size = 50;
                    parmData[2].Value = "NG";
                    parmData[2].Direction = ParameterDirection.Output;

                    parmData[3] = new SqlParameter();
                    parmData[3].ParameterName = "@R_MESSAGE"; //반환값
                    parmData[3].Size = 200;
                    parmData[3].Value = "";
                    parmData[3].Direction = ParameterDirection.Output;

                    DataSet tempSet1 = DBUtil.ExecuteDataSetSqlParam(ucGrid1CloseSpName, parmData);

                    r_ok = parmData[2].Value.ToString();
                    r_msg = parmData[3].Value.ToString() + "\r\n";

                    if (tempSet1.Tables[0].Rows.Count == 0  /*r_ok != "OK" || tempSet.Tables[0].Rows.Count == 0*/)
                    {
                        makeLog("번들마감", true, "번들 마감 실패");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                        return;
                    }
                    else
                    {
                        lib.Common.Management.Class.RfidSend wivis = new lib.Common.Management.Class.RfidSend();

                        for (int j = 0; j < tempSet1.Tables[0].Rows.Count; j++)
                        {
                            int chute_no = Convert.ToInt32(tempSet1.Tables[0].Rows[j]["chute_no"]);
                            object[] returnvalue = wivis.invoiceSerch(chute_no);
                            //값이 null 일경우는 한진이 아닌 경우
                            if (returnvalue == null)
                            {
                                returnvalue = new object[5];
                                returnvalue[0] = ""; // 송장 번호
                                returnvalue[1] = ""; // 터미널 코드
                                returnvalue[2] = ""; // 터미널 명
                            }

                            parmData = new SqlParameter[10];
                            parmData[0] = new SqlParameter();
                            parmData[0].ParameterName = "@HEADER_PARAMS";
                            parmData[0].DbType = DbType.String;
                            parmData[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터

                            parmData[1] = new SqlParameter();
                            parmData[1].ParameterName = "BARCODEXML"; //반환값
                            parmData[1].DbType = DbType.Xml;
                            parmData[1].Value = tempXml.InnerXml;
                            parmData[1].Direction = ParameterDirection.Input;

                            parmData[2] = new SqlParameter();
                            parmData[2].ParameterName = "@V_INVOICE"; //INVOICE
                            parmData[2].DbType = DbType.String;
                            parmData[2].Value = returnvalue[0].ToString();
                            parmData[2].Direction = ParameterDirection.Input;

                            parmData[3] = new SqlParameter();
                            parmData[3].ParameterName = "@V_CHUTE_NO"; //INVOICE
                            parmData[3].DbType = DbType.Int32;
                            parmData[3].Value = chute_no;
                            parmData[3].Direction = ParameterDirection.Input;

                            parmData[4] = new SqlParameter();
                            parmData[4].ParameterName = "@V_TML_CD"; //TERMINAL_CD
                            parmData[4].DbType = DbType.String;
                            parmData[4].Value = returnvalue[1].ToString();
                            parmData[4].Direction = ParameterDirection.Input;

                            parmData[5] = new SqlParameter();
                            parmData[5].ParameterName = "@V_TML_NM"; //TERMINAL_NM
                            parmData[5].DbType = DbType.String;
                            parmData[5].Value = returnvalue[2].ToString();
                            parmData[5].Direction = ParameterDirection.Input;

                            parmData[6] = new SqlParameter();
                            parmData[6].ParameterName = "@R_OK"; //반환값
                            parmData[6].Size = 50;
                            parmData[6].Value = "NG";
                            parmData[6].Direction = ParameterDirection.Output;

                            parmData[7] = new SqlParameter();
                            parmData[7].ParameterName = "@R_MESSAGE"; //반환값
                            parmData[7].Size = 200;
                            parmData[7].Value = "";
                            parmData[7].Direction = ParameterDirection.Output;

                            parmData[8] = new SqlParameter();
                            parmData[8].ParameterName = "@O_PrintMsg"; //반환값
                            parmData[8].Size = 1000;
                            parmData[8].Value = "";
                            parmData[8].Direction = ParameterDirection.Output;

                            parmData[9] = new SqlParameter();
                            parmData[9].ParameterName = "@O_LABEL_POS"; //반환값
                            parmData[9].DbType = DbType.Int32;
                            parmData[9].Value = 0;
                            parmData[9].Direction = ParameterDirection.Output;

                            DataSet tempSet = DBUtil.ExecuteDataSetSqlParam(ucGrid1CloseSpName2, parmData);

                            r_ok = parmData[6].Value.ToString();
                            r_msg = parmData[7].Value.ToString() + "\r\n";

                            if (r_ok != "OK")
                            {
                                makeLog("번들마감", true, "번들 마감 실패");
                                //설정 완료 메시지 창
                                bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                                return;
                            }
                            else
                            {
                                int label_pos = 0;
                                string print_msg = parmData[8].Value.ToString();

                                // RFID로 전송하는 로직.. 미할당 매장 추소하고 재전송시 유성쪽 primary key 에러가 발생하여 일딴 RFID 전송 하지 않음
                                if (tempSet.Tables[0] != null)
                                {
                                    //커넥션 나중에 오픈해야함. (실제 오픈시 주석풀어야함.)
                                    lib.Common.Management.WivisOracle wivisOracle = new lib.Common.Management.WivisOracle(System.Configuration.ConfigurationManager.AppSettings["OracleDatabaseip"].ToString());
                                    //lib.Common.Management.WivisOracle wivisOracle = null;
                                    wivisOracle.rfidInsert(tempSet.Tables[0]);
                                    StringBuilder sb = new StringBuilder("update if_box_list set status = 3 where biz_day = '");
                                    sb.Append(tempSet.Tables[0].Rows[0]["biz_day"].ToString());
                                    sb.Append("' and box_no = ");
                                    sb.Append(tempSet.Tables[0].Rows[0]["box_no"].ToString());
                                    sb.Append(" and chute_no = ");
                                    sb.Append(tempSet.Tables[0].Rows[0]["chute_no"].ToString());
                                    sb.Append(" and batch = '");
                                    sb.Append(tempSet.Tables[0].Rows[0]["batch"].ToString() + "'");

                                    DBUtil.ExecuteNonQuery(sb.ToString()); //오라클 데이터 전송후 에러가 없을 경우 box_stauts 를 1로 바꿈.
                                }

                                printMethode(print_msg, label_pos);
                                if (checkBox1.Checked == true)
                                {
                                    TelerikBarcode.Printer_interpace p1 = new TelerikBarcode.Printer_interpace();
                                    p1.print(tempSet.Tables[1], System.Configuration.ConfigurationManager.AppSettings["laserPrinter"].ToString(), "statementPrint");

                                }
                            }
                            ProgressPopupW.progressBar1.PerformStep();
                        }
                        makeLog("번들마감", true, "번들 마감 성공");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show(LanguagePack.Translate("번들마감완료"));
                        this.PageSearch();
                    }
                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("재발행", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("번들 마감중 에러가 발생했습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }

        }

        /// <summary>
        /// 번들 마감 취소 박스 초기화
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clear_the_bundle(object sender, EventArgs e)
        {
            base.ShowLoading();

            MainForm mf = this.FindForm() as Sorter.MainForm;
            if (mf.radddlwrkseq.SelectedValue.ToString() == "000")
            {
                bowooMessageBox.Show(LanguagePack.Translate("작업 차수를 선택 해 주세요."));
                return;
            }

            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            //슈트 설정 리스트 체크 카운트
            List<string> check_barcode = new List<string>();
            List<string> check_bundleCode = new List<string>();

            try
            {
                for (int i = 0; i < this.UC01_SMSGridViewLeft.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        check_barcode.Add(UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["BARCODE"].Value.ToString());
                        check_bundleCode.Add(UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["ASSORT_CD"].Value.ToString());
                        if (Convert.ToInt32(this.UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["REMAIN_QTY"].Value) == Convert.ToInt32(this.UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["BUNDEL_QTY"].Value))
                        {
                            bowooMessageBox.Show(LanguagePack.Translate("선택한 제품중 초기화할 박스가 없는 제품이있습니다."));
                            return;
                        }
                    }
                }

                if (check_barcode.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("번들 마감박스 초기화 할 제품을 선택 해 주세요."));
                    return;
                }

                check_bundleCode = check_bundleCode.Distinct().ToList();
                if (check_bundleCode.Count > 1)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("번들 유형이 같은 제품을 선택 해 주세요."));
                    return;
                }

                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                {
                    bowooConfirmBox.Show(LanguagePack.Translate("번들 마감 박스 초기화를 진행 하시겠습니까?"));
                }

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    check_barcode = check_barcode.Distinct().ToList(); //중복 바코드 제거
                    DataTable tempdt = new DataTable();
                    tempdt.Columns.Add("barcode");

                    for (int i = 0; i < check_barcode.Count; i++)
                    {
                        DataRow temprow = tempdt.NewRow();
                        temprow["barcode"] = check_barcode[i];
                        tempdt.Rows.Add(temprow);
                    }

                    lib.Common.Management.Xml xml = new lib.Common.Management.Xml();
                    System.Xml.XmlDocument tempXml = xml.xmlMake(tempdt);

                    string r_ok = "";
                    string r_msg = "";

                    SqlParameter[] parmData = new SqlParameter[4];
                    parmData[0] = new SqlParameter();
                    parmData[0].ParameterName = "@HEADER_PARAMS";
                    parmData[0].DbType = DbType.String;
                    parmData[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터

                    parmData[1] = new SqlParameter();
                    parmData[1].ParameterName = "BARCODEXML"; //반환값
                    parmData[1].DbType = DbType.Xml;
                    parmData[1].Value = tempXml.InnerXml;
                    parmData[1].Direction = ParameterDirection.Input;

                    parmData[2] = new SqlParameter();
                    parmData[2].ParameterName = "@R_OK"; //반환값
                    parmData[2].Size = 50;
                    parmData[2].Value = "NG";
                    parmData[2].Direction = ParameterDirection.Output;

                    parmData[3] = new SqlParameter();
                    parmData[3].ParameterName = "@R_MESSAGE"; //반환값
                    parmData[3].Size = 200;
                    parmData[3].Value = "";
                    parmData[3].Direction = ParameterDirection.Output;

                    DataSet tempSet = DBUtil.ExecuteDataSetSqlParam(ucGrid1ClearSpName, parmData);

                    r_ok = parmData[2].Value.ToString();
                    r_msg += parmData[3].Value.ToString() + "\r\n";

                    if (r_ok != "OK")
                    {
                        makeLog("번들 마감 초기화", true, "번들 마감 초기화 실패");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                        return;
                    }
                    else
                    {
                        makeLog("번들 마감 초기화", true, "번들 마감 초기화 성공");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show(LanguagePack.Translate("번들 마감 박스 초기화 성공"));
                    }
                    this.PageSearch();
                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("재발행", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("번들 마감중 에러가 발생했습니다.\r\n관리자에게 문의하세요."));
            }
            finally
            {
                base.HideLoading();
            }

        }


        private void remain_print_bundle(object sender, EventArgs e)
        {
            //base.ShowLoading();

            MainForm mf = this.FindForm() as Sorter.MainForm;
            if (mf.radddlwrkseq.SelectedValue.ToString() == "000")
            {
                bowooMessageBox.Show(LanguagePack.Translate("작업 차수를 선택 해 주세요."));
                return;
            }

            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            //슈트 설정 리스트 체크 카운트
            List<string> check_barcode = new List<string>();
            List<string> check_bundleCode = new List<string>();

            try
            {
                for (int i = 0; i < this.UC01_SMSGridViewLeft.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        check_barcode.Add(UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["BARCODE"].Value.ToString());
                        check_bundleCode.Add(UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["ASSORT_CD"].Value.ToString());
                        if (Convert.ToInt32(this.UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["REMAIN_QTY"].Value) == Convert.ToInt32(this.UC01_SMSGridViewLeft.GridViewData.Rows[i].Cells["BUNDEL_QTY"].Value))
                        {
                            bowooMessageBox.Show(LanguagePack.Translate("선택한 제품중 미작업 상태의 제품이 있습니다."));
                            return;
                        }
                    }
                }

                if (check_barcode.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("번들 마감할 제품을 선택 해 주세요."));
                    return;
                }

                check_bundleCode = check_bundleCode.Distinct().ToList();
                if (check_bundleCode.Count > 1)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("번들 유형이 같은 제품을 선택 해 주세요."));
                    return;
                }

                Sorter.Popup.S012_BundlePopup_RemainPrint select_print = new Sorter.Popup.S012_BundlePopup_RemainPrint(this);
                select_print.ShowDialog();



                check_barcode = check_barcode.Distinct().ToList(); //중복 바코드 제거
                DataTable tempdt = new DataTable();
                tempdt.Columns.Add("barcode");

                for (int i = 0; i < check_barcode.Count; i++)
                {
                    DataRow temprow = tempdt.NewRow();
                    temprow["barcode"] = check_barcode[i];
                    tempdt.Rows.Add(temprow);
                }

                lib.Common.Management.Xml xml = new lib.Common.Management.Xml();
                System.Xml.XmlDocument tempXml = xml.xmlMake(tempdt);

                lib.Common.Management.Class.RfidSend rfid = new lib.Common.Management.Class.RfidSend();


                SqlParameter[] parmData = new SqlParameter[3];
                parmData[0] = new SqlParameter();
                parmData[0].ParameterName = "@HEADER_PARAMS";
                parmData[0].DbType = DbType.String;
                parmData[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터

                parmData[1] = new SqlParameter();
                parmData[1].ParameterName = "BARCODEXML"; //재발행할 아이템 정보
                parmData[1].DbType = DbType.Xml;
                parmData[1].Value = tempXml.InnerXml;
                parmData[1].Direction = ParameterDirection.Input;

                parmData[2] = new SqlParameter();
                parmData[2].ParameterName = "@PRINT_TYPE"; //재발행할 프린트 타입
                parmData[2].Size = 1000;
                parmData[2].Value = seleted_print;
                parmData[2].Direction = ParameterDirection.Input;


                DataSet tempSet = DBUtil.ExecuteDataSetSqlParam(ucGrid1Remain_PrintSpName, parmData);


                if (tempSet.Tables.Count == 0)
                {
                    makeLog("번들 재발행", true, "번들 재 발행 실패");
                    //설정 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("재발행에 실패했습니다."));
                    return;
                }
                else
                {
                    //rfid.rfid_send(tempSet, parmData[6].Value.ToString(), Convert.ToInt32(parmData[7].Value));
                    if (seleted_print == "LABEL") // 라벨 재 발행
                    {
                        for (int i = 0; i < tempSet.Tables[0].Rows.Count; i++)
                        {
                            printMethode(tempSet.Tables[0].Rows[i][0].ToString(), 0);
                        }
                    }
                    else if (seleted_print == "LASER") // 명세서 재 발행
                    {
                        List<string> invoiceList = tempSet.Tables[0].AsEnumerable().GroupBy(k => k.Field<string>("box_inv")).Select(k => k.Key).Distinct().ToList();
                        for (int i = 0; i < invoiceList.Count; i++)
                        {
                            string tmpe_invoice = invoiceList[i];
                            DataTable tempTabel = tempSet.Tables[0].AsEnumerable().Where(k => k.Field<string>("box_inv") == tmpe_invoice).CopyToDataTable();
                            TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                            //p.print(tempTabel, "Microsoft Print to PDF", "statementPrint");
                            p.print(tempTabel, System.Configuration.ConfigurationManager.AppSettings["laserPrinter"].ToString(), "statementPrint");
                        }
                    }
                }
                makeLog("번들 재발행", true, "번들 재 발행 성공");
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("재발행", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("번들 재 발행중 에러가 발생했습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                seleted_print = string.Empty;
                //base.HideLoading();
            }

        }


        /// <summary>
        /// 프린트 정보를 파싱해서 프린트 클래스에 넘김.
        /// </summary>
        /// <param name="printList"></param>
        private void printMethode(string printList, int label_no)
        {


            string[] printString_arr = null; //라벨 출력할 데이터를 담는 변수.
            printString_arr = printList.ToString().Split('/');
            //앞뒤 공백제거
            for (int i = 0; i < printString_arr.Length; i++)
            {
                printString_arr[i] = printString_arr[i].TrimEnd().TrimStart();
            }
            TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
            p.print(printString_arr, "LABEL_" + label_no.ToString().PadLeft(2, '0'));
            //p.print(printString_arr, "Microsoft Print to PDF");

            //Task.Run(() =>
            //{
            //    string[] printString_arr = null; //라벨 출력할 데이터를 담는 변수.
            //    printString_arr = printList.ToString().Split('/');
            //    //앞뒤 공백제거
            //    for (int i = 0; i < printString_arr.Length; i++)
            //    {
            //        printString_arr[i] = printString_arr[i].TrimEnd().TrimStart();
            //    }
            //    TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
            //    //p.print(printList, label_no);
            //    p.print(printString_arr, "LABEL_" + label_no.ToString().PadLeft(2, '0'));
            //});
        }


        /// <summary>
        /// 발행 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewLeft_TopButton1_Click(object sender, EventArgs e)
        {

            try
            {

                printMethode("bundle");
                makeLog("번들리스트 발행", true, "번들리스트행 완료");
                //설정 완료 메시지 창
                //bowooMessageBox.Show(LanguagePack.Translate("재발행이 완료되었습니다."));
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("번들리스트 발행", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("번들리스트 발행을 실패하였습니다.\r\n관리자에게 문의하세요."));

            }
        }




        /// <summary>
        /// 프린트 정보를 파싱해서 프린트 클래스에 넘김.
        /// </summary>
        /// <param name="printList"></param>
        private void printMethode(string value)
        {

            Task.Run(() =>
            {

                TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                //p.print(pickingSet.Tables[1], "Microsoft Print to PDF");
                //p.print(pickingSet.Tables[1], "Microsoft Print to PDF", value);
                //p.print(this.UC01_SMSGridViewLeft.ds_Gird.Tables[1], System.Configuration.ConfigurationManager.AppSettings["laserPrinter"].ToString(), value);
                p.print(this.UC01_SMSGridViewLeft.ds_Gird.Tables[1], "Microsoft Print to PDF", value);
            });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //paramss.Add("@HEADER_PARAMS", "SexCd=" + comboBox1.SelectedValue.ToString() + ";#item_cd" + comboBox2.SelectedValue.ToString());
            //this.PageSearch();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //paramss.Add("@HEADER_PARAMS", "SexCd=" + comboBox1.SelectedValue.ToString() + ";#item_cd" + comboBox2.SelectedValue.ToString());
            //this.PageSearch();

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
                style_select(((KeyValuePair<string, string>)comboBox1.SelectedItem).Key.ToString());


            //string dddd = comboBox1.SelectedValue.ToString();
        }
    }
}