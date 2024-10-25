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
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;

namespace Sorter.Pages
{
    public partial class S009_ManageLoad : lib.Common.Management.BaseControl
    {
        // SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S009_PAGE1_01_L_LOADING_LIST]";
        string ucGrid2SelectSpName = "[USP_S009_PAGE1_02_L_BOXING_LIST_DETAIL]";

        string ucGrid1_PAGE2_SelectSpName = "[USP_S009_PAGE2_01_L_LOADING_LIST]";
        string ucGrid2_PAGE2_SelectSpName = "[USP_S009_PAGE2_02_L_BOXING_LIST_DETAIL]";


        string ucGrid3_PAGE1_Remain_Label_print = "[USP_S009_PAGE1_01_XML_REMAIN_LABEL_PRINT]";
        string ucGrid3_PAGE1_Remain_Invoice_print = "[USP_S009_PAGE1_01_XML_REMAIN_INVOICE_PRINT]";

        string ucGrid3_PAGE2_Remain_Label_print = "[USP_S009_PAGE2_01_XML_REMAIN_LABEL_PRINT]";
        string ucGrid3_PAGE2_Remain_Invoice_print = "[USP_S009_PAGE2_01_XML_REMAIN_INVOICE_PRINT]";


        string ucGrid1_PAGE3_SelectSpName = "[USP_S009_PAGE3_01_L_LOADING_LIST]";
        string ucGrid2_PAGE3_SelectSpName = "[USP_S009_PAGE3_02_L_BOXING_LIST_DETAIL]";
        string ucGrid3_PAGE3_SelectSpName = "[USP_S009_PAGE3_03_B_WORK_CFM_LIST]";
        
        
        //SP 버튼 이벤트
        string ucGrid3_PAGE3_SaveData = "[USP_S009_PAGE3_02_SVAE_DATA]";
        string ucGrid3_PAGE3_Reissue = "[USP_S009_PAGE3_01_XML_REMAIN_PRINT]";
        string ucGrid3_PAGE3_Bundel_Complete = "[USP_S009_PAGE3_02_BUNDEL_COMPLETE]";
        string ucGrid3_PAGE3_Remain_Data = "[USP_S009_PAGE3_03_Remain_DATA]";


        //정리작업 엘셀 파일로 저장
        string batchFileListPath = @"D:\번들 결과";
        string remainFileListPath = @"D:\번들 정리 미출";

        //RadDropDownList radDropList;

        public S009_ManageLoad()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            this.radPageView1.SelectedPageChanged += radPageView1_SelectedPageChanged;


            #region 출고
            //버튼 설정
            UC01_SMSGridViewRight.Button1_Visible = true;
            UC01_SMSGridViewRight.Button1_Text = "라벨 발행";
            UC01_SMSGridViewRight.Button1_Click = UC01_SMSGridViewRight_TopButton1_Click;

            UC01_SMSGridViewRight.Button2_Visible = true;
            UC01_SMSGridViewRight.Button2_Text = "명세서 발행";
            UC01_SMSGridViewRight.Button2_Click = UC01_SMSGridViewRight_TopButton2_Click;

            //UC01_SMSGridViewRight.Button2_Visible = true;
            //UC01_SMSGridViewRight.Button2_Text = "엑셀저장";
            //UC01_SMSGridViewRight.Button2_Click = UC01_SMSGridViewRight_TopButton2_Click;


            // Left Side Grid
            this.UC01_SMSGridViewLeft.GridTitleText = "매장 별 박스 리스트";


            // Right Side Grid
            this.UC01_SMSGridViewRight.GridTitleText = "박스 상세 리스트";

            //PageSearch();

            this.UC01_SMSGridViewRight.GridViewData.HideSelection = true;

            #endregion

            #region 출고
            //버튼 설정
            UC01_SMSGridViewPage2Right.Button1_Visible = true;
            UC01_SMSGridViewPage2Right.Button1_Text = "라벨 발행";
            UC01_SMSGridViewPage2Right.Button1_Click = UC01_SMSGridViewRight_TopButton1_Click;

            UC01_SMSGridViewPage2Right.Button2_Visible = true;
            UC01_SMSGridViewPage2Right.Button2_Text = "명세서 발행";
            UC01_SMSGridViewPage2Right.Button2_Click = UC01_SMSGridViewRight_TopButton2_Click;

            //UC01_SMSGridViewRight.Button2_Visible = true;
            //UC01_SMSGridViewRight.Button2_Text = "엑셀저장";
            //UC01_SMSGridViewRight.Button2_Click = UC01_SMSGridViewRight_TopButton2_Click;


            // Left Side Grid
            this.UC01_SMSGridViewPage2Left.GridTitleText = "매장 별 박스 리스트";


            // Right Side Grid
            this.UC01_SMSGridViewPage2Right.GridTitleText = "박스 상세 리스트";

            //PageSearch();

            this.UC01_SMSGridViewPage2Right.GridViewData.HideSelection = true;

            #endregion





            radPageView1.Pages[0].Text = LanguagePack.Translate(radPageView1.Pages[0].Text);
           radPageView1.Pages[1].Text = LanguagePack.Translate(radPageView1.Pages[1].Text);
            radPageView1.Pages[2].Text = LanguagePack.Translate(radPageView1.Pages[2].Text);


            radPageView1.Pages[0].Tag = "출고";
            radPageView1.Pages[1].Tag = "반품";
            radPageView1.Pages[2].Tag = "정리";
     




            #region 정리

            //left side grid
            this.UC01_SMSGridViewPage3Left.GridTitleText = "정리 박스 리스트";

            //top side grid
            this.UC01_SMSGridViewPage3Top.GridTitleText = "정리 박스 상세 리스트";

            //this.UC01_SMSGridViewPage3Top.radButton1.Width = 80;
            //this.UC01_SMSGridViewPage3Top.radButton1.Height = 25;
            //this.UC01_SMSGridViewPage3Top.radButton1.Location = new Point(286, 2);

            this.UC01_SMSGridViewPage3Top.Button1_Visible = true;
            this.UC01_SMSGridViewPage3Top.Button1_Text = "재발행";
            this.UC01_SMSGridViewPage3Top.Button1_Click = UC01_SMSGridViewRightPage3_TopButton1_Click;

            this.UC01_SMSGridViewPage3Top.Button2_Visible = true;
            this.UC01_SMSGridViewPage3Top.Button2_Text = "엑셀 저장";
            this.UC01_SMSGridViewPage3Top.Button2_Click = UC01_SMSGridViewRightPage3_TopButton2_Click;


            this.UC01_SMSGridViewPage3Top.Button3_Visible = true;
            this.UC01_SMSGridViewPage3Top.Button3_Text = "번들 마감";
            this.UC01_SMSGridViewPage3Top.Button3_Click = UC01_SMSGridViewRightPage3_TopButton3_Click;

            this.UC01_SMSGridViewPage3Top.Button4_Visible = true;
            this.UC01_SMSGridViewPage3Top.Button4_Text = "미출 엑셀";
            this.UC01_SMSGridViewPage3Top.Button4_Click = UC01_SMSGridViewRightPage4_TopButton2_Click;
            //bottom side grid
            this.UC01_SMSGridViewPage3Bot.GridTitleText = "작업 중 리스트";

            #endregion




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

                menuId = "S009";
                menuTitle = "상차 관리";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = true;
                    mf.radDropDownListBrand.Enabled = true;
                }

                //// 데이터 바인딩
                //Dictionary<string, object> paramss = null;
                ////paramss.Add("@HEADER_PARAMS", "SHOP_CD=" + this.radDropList.SelectedValue.ToString());

                //// 행 변경 이벤트
                //UC01_SMSGridViewLeft.GridViewData.SelectionChanged -= GridViewData_SelectionChanged;
                //this.UC01_SMSGridViewLeft.BindData(ucGrid1SelectSpName, paramss);

                //if (UC01_SMSGridViewLeft.GridViewData.Rows.Count > 0)
                //{
                //    UC01_SMSGridViewLeft.GridViewData.Rows[0].IsSelected = false;
                //    UC01_SMSGridViewLeft.GridViewData.SelectionChanged += GridViewData_SelectionChanged;
                //    UC01_SMSGridViewLeft.GridViewData.Rows[0].IsSelected = true;
                //}

                //else
                //{
                //    if (!this.UC01_SMSGridViewRight.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS"))
                //        this.UC01_SMSGridViewRight.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                //    this.UC01_SMSGridViewRight.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewLeft.GetKeyParam();

                //    // 데이터 바인딩
                //    this.UC01_SMSGridViewRight.BindData(ucGrid2SelectSpName, null);
                //}
                Dictionary<string, object> paramss = null;

                switch (this.radPageView1.SelectedPage.Tag.ToString())
                {
                    case "출고":

                        #region //page1 출고

                        menuId = "S007";
                        menuTitle = "결과전송 - 출고";

                        // 데이터 바인딩
                        //Dictionary<string, object> paramss = null;
                        //paramss.Add("@HEADER_PARAMS", "SHOP_CD=" + this.radDropList.SelectedValue.ToString());
                        paramss = new Dictionary<string, object>();

                        if (this.check_box.Checked) //번들 제품만 보고 싶을때.
                        {
                            paramss.Add("@HEADER_PARAMS", "bundle_box=true;#");
                        }
                        else
                        {
                            paramss.Add("@HEADER_PARAMS", "bundle_box=false;#");
                        }

                        // 행 변경 이벤트
                        UC01_SMSGridViewLeft.GridViewData.SelectionChanged -= GridViewData_SelectionChanged;
                        this.UC01_SMSGridViewLeft.BindData(ucGrid1SelectSpName, paramss);

                        if (UC01_SMSGridViewLeft.GridViewData.Rows.Count > 0)
                        {
                            UC01_SMSGridViewLeft.GridViewData.Rows[0].IsSelected = false;
                            UC01_SMSGridViewLeft.GridViewData.SelectionChanged += GridViewData_SelectionChanged;
                            UC01_SMSGridViewLeft.GridViewData.Rows[0].IsSelected = true;
                        }

                        else
                        {
                            if (!this.UC01_SMSGridViewRight.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS"))
                                this.UC01_SMSGridViewRight.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                            this.UC01_SMSGridViewRight.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewLeft.GetKeyParam();

                            // 데이터 바인딩
                            this.UC01_SMSGridViewRight.BindData(ucGrid2SelectSpName, null);
                        }

                        #endregion

                        break;


                    case "반품":

                        #region //page1 반품

                        menuId = "S007";
                        menuTitle = "결과전송 - 반품";

                        // 데이터 바인딩
                        //Dictionary<string, object> paramss = null;
                        //paramss.Add("@HEADER_PARAMS", "SHOP_CD=" + this.radDropList.SelectedValue.ToString());
                        paramss = new Dictionary<string, object>();

                        if (this.check_box.Checked) //번들 제품만 보고 싶을때.
                        {
                            paramss.Add("@HEADER_PARAMS", "bundle_box=true;#");
                        }
                        else
                        {
                            paramss.Add("@HEADER_PARAMS", "bundle_box=false;#");
                        }

                        // 행 변경 이벤트
                        UC01_SMSGridViewPage2Left.GridViewData.SelectionChanged -= UC01_SMSGridViewPage2Left_SelectionChanged;
                        this.UC01_SMSGridViewPage2Left.BindData(ucGrid1_PAGE2_SelectSpName, paramss);

                        if (UC01_SMSGridViewPage2Left.GridViewData.Rows.Count > 0)
                        {
                            UC01_SMSGridViewPage2Left.GridViewData.Rows[0].IsSelected = false;
                            UC01_SMSGridViewPage2Left.GridViewData.SelectionChanged += UC01_SMSGridViewPage2Left_SelectionChanged;
                            UC01_SMSGridViewPage2Left.GridViewData.Rows[0].IsSelected = true;
                        }

                        else
                        {
                            if (!this.UC01_SMSGridViewPage2Right.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS"))
                                this.UC01_SMSGridViewPage2Right.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                            this.UC01_SMSGridViewPage2Right.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewPage2Left.GetKeyParam();

                            // 데이터 바인딩
                            this.UC01_SMSGridViewPage2Right.BindData(ucGrid2_PAGE2_SelectSpName, null);
                        }

                        #endregion

                        break;
                        
                            
                    case "정리":

                        #region //page3 정리
                        menuId = "S007";
                        menuTitle = "결과전송 - 정리";

                        //데이터 바인딩
                        //행 변경 이벤트
                        UC01_SMSGridViewPage3Left.GridViewData.SelectionChanged -= UC01_SMSGridViewPage3Left_SelectionChanged;
                        this.UC01_SMSGridViewPage3Left.BindData(ucGrid1_PAGE3_SelectSpName, paramss);

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
                            this.UC01_SMSGridViewPage3Top.BindData(ucGrid2_PAGE3_SelectSpName, null);
                        }

                        //데이터 바인딩
                        this.UC01_SMSGridViewPage3Bot.BindData(ucGrid3_PAGE3_SelectSpName, paramss);
                        #endregion

                        break;

                 

      


                default:

                        break;
                }
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
            if (this.UC01_SMSGridViewLeft.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();

                if (!this.UC01_SMSGridViewRight.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.UC01_SMSGridViewRight.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                this.UC01_SMSGridViewRight.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewLeft.GetKeyParam();

                // 데이터 바인딩
                this.UC01_SMSGridViewRight.BindData(ucGrid2SelectSpName, null);
                base.HideLoading();
            }
        }

        void UC01_SMSGridViewPage2Left_SelectionChanged(object sender, EventArgs e)
        {
            if (this.UC01_SMSGridViewPage2Left.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();

                if (!this.UC01_SMSGridViewPage2Right.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.UC01_SMSGridViewPage2Right.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                this.UC01_SMSGridViewPage2Right.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridViewPage2Left.GetKeyParam();

                // 데이터 바인딩
                this.UC01_SMSGridViewPage2Right.BindData(ucGrid2_PAGE2_SelectSpName, null);
                base.HideLoading();
            }
        }

        /// <summary>
        /// 출고 작업 라벨 재발행 버튼
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
            //List<GridViewRowInfo> rowList = new List<GridViewRowInfo>();

            try
            {

                for (int i = 0; i < this.UC01_SMSGridViewRight.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewRight.GridViewData.Rows[i].IsSelected)
                    {
                        chuteChkedIdxs.Add(i);
                        chuteChkedCnt++;
                    }
                }

                if (chuteChkedIdxs.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("재발행할 박스를 선택해 주세요."));
                    return;
                }

                if (!this.UC01_SMSGridViewRight.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.UC01_SMSGridViewRight.Add_Data_Parameters.Add("R_E1", "");
                this.UC01_SMSGridViewRight.Add_Data_Parameters["R_E1"] = "";

                string r_ok = "";
                string r_msg = "";
                string r_msg_params = "";

                //SP실행
                DataSet dataTable = this.UC01_SMSGridViewRight.ExcuteSaveSpXml(ucGrid3_PAGE1_Remain_Label_print, chuteChkedIdxs);

                foreach (DataRow dr in dataTable.Tables[0].Rows)
                {
                    string data = dr.ItemArray[0].ToString();
                    int last_index = data.LastIndexOf('/');
                    int label_pos = Convert.ToInt32(data.Substring(last_index + 1, data.Length - 1 - last_index));
                    data = data.Substring(0, last_index);

                    printMethode(data, 0);
                }

                r_ok = this.UC01_SMSGridViewRight.Usp_Save_Parameters[2].Value.ToString();
                r_msg += this.UC01_SMSGridViewRight.Usp_Save_Parameters[3].Value.ToString() + "\r\n";


                if (r_ok != "OK")
                {
                    string[] r_split = r_msg_params.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                    makeLog("재발행", false, r_msg);
                    bowooMessageBox.Show(LanguagePack.Translate("재발행을 실패한 박스가 존재합니다.\r\n") + string.Format(LanguagePack.Translate(r_msg), r_split[0], r_split[1], r_split[2], r_split[3]));
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
                //ProgressPopupW.Close();
            }
        }

        /// <summary>
        /// 출고 작업 명세서 재발행 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewRight_TopButton2_Click(object sender, EventArgs e)
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
            //List<GridViewRowInfo> rowList = new List<GridViewRowInfo>();

            try
            {

                for (int i = 0; i < this.UC01_SMSGridViewRight.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewRight.GridViewData.Rows[i].IsSelected)
                    {
                        chuteChkedIdxs.Add(i);
                        chuteChkedCnt++;
                    }
                }

                if (chuteChkedIdxs.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("재발행할 박스를 선택해 주세요."));
                    return;
                }

                if (!this.UC01_SMSGridViewRight.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.UC01_SMSGridViewRight.Add_Data_Parameters.Add("R_E1", "");
                this.UC01_SMSGridViewRight.Add_Data_Parameters["R_E1"] = "";

                string r_ok = "";
                string r_msg = "";
                string r_msg_params = "";


                //SP실행
                //DataSet dataTable = this.UC01_SMSGridViewRight.ExcuteSaveSpXml(ucGrid3_PAGE1_Remain_Invoice_print, chuteChkedIdxs);

                //List<string> invoiceList = dataTable.Tables[0].AsEnumerable().GroupBy(k => k.Field<string>("box_inv")).Select(k => k.Key).Distinct().ToList();
                //for (int i = 0; i < invoiceList.Count; i++)
                //{
                //    string tmpe_invoice = invoiceList[i];
                //    DataTable tempTabel = dataTable.Tables[0].AsEnumerable().Where(k => k.Field<string>("box_inv") == tmpe_invoice).CopyToDataTable();
                //    TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                //    p.print(tempTabel, System.Configuration.ConfigurationManager.AppSettings["laserPrinter"].ToString(), "statementPrint");
                //    //p.print(tempTabel, "Microsoft Print to PDF", "statementPrint");
                //}


                //foreach (DataRow dr in dataTable.Tables[0].Rows)
                //{
                //    string data = dr.ItemArray[0].ToString();
                //    int last_index = data.LastIndexOf('/');
                //    int label_pos = Convert.ToInt32(data.Substring(last_index + 1, data.Length - 1 - last_index));
                //    data = data.Substring(0, last_index);

                //    printMethode(data, 0);
                //}

                r_ok = this.UC01_SMSGridViewRight.Usp_Save_Parameters[2].Value.ToString();
                r_msg += this.UC01_SMSGridViewRight.Usp_Save_Parameters[3].Value.ToString() + "\r\n";


                if (r_ok != "OK")
                {
                    string[] r_split = r_msg_params.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                    makeLog("재발행", false, r_msg);
                    bowooMessageBox.Show(LanguagePack.Translate("재발행을 실패한 박스가 존재합니다.\r\n") + string.Format(LanguagePack.Translate(r_msg), r_split[0], r_split[1], r_split[2], r_split[3]));
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
                //ProgressPopupW.Close();
            }
        }

        /// <summary>
        /// 정리 작업재발행 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewRightPage3_TopButton1_Click(object sender, EventArgs e)
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
            //List<GridViewRowInfo> rowList = new List<GridViewRowInfo>();

            try
            {

                for (int i = 0; i < this.UC01_SMSGridViewPage3Top.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewPage3Top.GridViewData.Rows[i].IsSelected)
                    {
                        chuteChkedIdxs.Add(i);
                        chuteChkedCnt++;
                        //rowList.Add(UC01_SMSGridViewRight.GridViewData.Rows[i]);
                    }
                }

                if (chuteChkedIdxs.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("재발행할 박스를 선택해 주세요."));
                    return;
                }


                if (!this.UC01_SMSGridViewPage3Top.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.UC01_SMSGridViewPage3Top.Add_Data_Parameters.Add("R_E1", "");
                this.UC01_SMSGridViewPage3Top.Add_Data_Parameters["R_E1"] = "";

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

                //SP실행
                DataSet tempset = this.UC01_SMSGridViewPage3Top.ExcuteSaveSpXml(ucGrid3_PAGE3_Reissue, chuteChkedIdxs);
                DataTable tempTable = null;
                if (tempset.Tables[0].Rows.Count == 0)
                {
                    return;
                }

                //다중 선택했을때 박스별로 프린트 하기 위함.
                List<int> tempboxList = tempset.Tables[0].AsEnumerable().GroupBy(j => j.Field<int>("box_no")).Select(j => j.Key).ToList();
                for(int i = 0; i< tempboxList.Count; i++)
                {
                    tempTable = tempset.Tables[0].AsEnumerable().Where(k => k.Field<int>("box_no") == tempboxList[i]).CopyToDataTable();
                    printMethode(tempTable, 0);
                }

                //foreach (DataRow dr in dataTable.Tables[0].Rows)
                //{
                //    string data = dr.ItemArray[0].ToString();
                //    int last_index = data.LastIndexOf('/');
                //    int label_pos = Convert.ToInt32(data.Substring(last_index + 1, data.Length - 1 - last_index));
                //    data = data.Substring(0, last_index);

                //}


                r_ok = this.UC01_SMSGridViewPage3Top.Usp_Save_Parameters[2].Value.ToString();
                r_msg += this.UC01_SMSGridViewPage3Top.Usp_Save_Parameters[3].Value.ToString() + "\r\n";


                ProgressPopupW.progressBar1.PerformStep();


                if (r_ok == "OK")
                {
                    makeLog("재발행", true, "재발행 완료");
                    //설정 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("재발행이 완료되었습니다."));

                    //PageSearch();
                }
                else
                {
                    string[] r_split = r_msg_params.Split(new string[] { ";#" }, StringSplitOptions.RemoveEmptyEntries);
                    makeLog("재발행", false, r_msg);
                    bowooMessageBox.Show(LanguagePack.Translate("재발행을 실패한 박스가 존재합니다.\r\n") + string.Format(LanguagePack.Translate(r_msg), r_split[0], r_split[1], r_split[2], r_split[3]));
                }
                //}
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
        /// Generate CSV File From GridView  
        /// </summary>
        /// <param name="gridView"></param>
        /// <param name="filePath"></param>
        /// <param name="isExistsHeader"></param>
        public void ConvertToCsvFileFromGridView(DataTable gridView, string filePath, bool isExistsHeader)
        {
            StringBuilder sb = new StringBuilder();

            if (isExistsHeader)
            {
                List<string> columnList = new List<string>();
                foreach (DataColumn dr in gridView.Columns)
                {
                    columnList.Add(dr.ColumnName);
                }

                sb.AppendLine(string.Join(",", columnList));
            }

            ProgressPopupW = new lib.Common.Management.ProgressFormW();
            ProgressPopupW.progressBar1.Maximum = gridView.Rows.Count;
            ProgressPopupW.progressBar1.Step = 1;
            //ProgressPopupW.SetLocation(Das_B2B.MainForm.MainPosition);
            ProgressPopupW.BringToFront();
            ProgressPopupW.Show();

            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                List<string> fields = new List<string>();
                for (int j = 0; j < gridView.Columns.Count; j++)
                {
                    fields.Add(gridView.Rows[i][j].ToString());

                }

                sb.AppendLine(string.Join(",", fields));
                ProgressPopupW.progressBar1.PerformStep();
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.Default);

            ProgressPopupW.Close();
        }

        /// <summary>
        /// 번들 작업 데이터 엑셀 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewRightPage3_TopButton2_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            //Excel.Application excelApp = new Excel.Application();
            try
            {
                SqlParameter[] parmData = new SqlParameter[3];
                parmData[0] = new SqlParameter();
                parmData[0].ParameterName = "@HEADER_PARAMS";
                parmData[0].DbType = DbType.String;
                parmData[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터

                parmData[1] = new SqlParameter();
                parmData[1].ParameterName = "@R_OK"; //반환값
                parmData[1].Size = 50;
                parmData[1].Value = "NG";
                parmData[1].Direction = ParameterDirection.Output;

                parmData[2] = new SqlParameter();
                parmData[2].ParameterName = "@R_MESSAGE"; //반환값
                parmData[2].Size = 200;
                parmData[2].Value = "";
                parmData[2].Direction = ParameterDirection.Output;

                DataSet tempSet = DBUtil.ExecuteDataSetSqlParam(ucGrid3_PAGE3_SaveData, parmData);

                if(parmData[1].Value.ToString() == "NG")
                {
                    bowooMessageBox.Show(LanguagePack.Translate(parmData[2].Value.ToString()));
                    return;
                }
                tempSet.Tables[0].TableName = "번들정리";

                string file_name = string.Format("{0}_{1}", "번들작업결과", DateTime.Now.ToString("yyyyMMdd_hhmmss"));
                lib.Common.Management.ExcelDB.SaveExcelDB(batchFileListPath,file_name, tempSet);

                makeLog("파일 생성", true, "파일 생성 완료");
                bowooMessageBox.Show(LanguagePack.Translate("파일 생성이 완료되었습니다."));
            }

            catch (Exception exc)
            {
                //로그
                makeLog("파일 생성", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("파일 생성에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }


        /// <summary>
        /// 번들 작업 데이터 엑셀 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewRightPage4_TopButton2_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            //Excel.Application excelApp = new Excel.Application();
            try
            {
                SqlParameter[] parmData = new SqlParameter[3];
                parmData[0] = new SqlParameter();
                parmData[0].ParameterName = "@HEADER_PARAMS";
                parmData[0].DbType = DbType.String;
                parmData[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터

                parmData[1] = new SqlParameter();
                parmData[1].ParameterName = "@R_OK"; //반환값
                parmData[1].Size = 50;
                parmData[1].Value = "NG";
                parmData[1].Direction = ParameterDirection.Output;

                parmData[2] = new SqlParameter();
                parmData[2].ParameterName = "@R_MESSAGE"; //반환값
                parmData[2].Size = 200;
                parmData[2].Value = "";
                parmData[2].Direction = ParameterDirection.Output;

                DataSet tempSet = DBUtil.ExecuteDataSetSqlParam(ucGrid3_PAGE3_Remain_Data, parmData);

                if (parmData[1].Value.ToString() == "NG")
                {
                    bowooMessageBox.Show(LanguagePack.Translate(parmData[2].Value.ToString()));
                    return;
                }
                tempSet.Tables[0].TableName = "번들정리미출";

                string file_name = string.Format("{0}_{1}", "번들 정리 미출", DateTime.Now.ToString("yyyyMMdd_hhmmss"));
                lib.Common.Management.ExcelDB.SaveExcelDB(remainFileListPath, file_name, tempSet);

                makeLog("파일 생성", true, "파일 생성 완료");
                bowooMessageBox.Show(LanguagePack.Translate("파일 생성이 완료되었습니다."));
            }

            catch (Exception exc)
            {
                //로그
                makeLog("파일 생성", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("파일 생성에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }


        /// <summary>
        /// 번들 작업 데이터 마감
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewRightPage3_TopButton3_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            //Excel.Application excelApp = new Excel.Application();`
            try
            {
                SqlParameter[] parmData = new SqlParameter[3];
                parmData[0] = new SqlParameter();
                parmData[0].ParameterName = "@HEADER_PARAMS";
                parmData[0].DbType = DbType.String;
                parmData[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터

                parmData[1] = new SqlParameter();
                parmData[1].ParameterName = "@R_OK"; //반환값
                parmData[1].Size = 50;
                parmData[1].Value = "NG";
                parmData[1].Direction = ParameterDirection.Output;

                parmData[2] = new SqlParameter();
                parmData[2].ParameterName = "@R_MESSAGE"; //반환값
                parmData[2].Size = 200;
                parmData[2].Value = "";
                parmData[2].Direction = ParameterDirection.Output;

                DataSet tempSet = DBUtil.ExecuteDataSetSqlParam(ucGrid3_PAGE3_Bundel_Complete, parmData);

                if (parmData[1].Value.ToString() == "NG")
                {
                    makeLog("번들 마감", true, "번들 마감 실패");
                    bowooMessageBox.Show(LanguagePack.Translate(parmData[2].Value.ToString()));
                    return;
                }
                //tempSet.Tables[0].TableName = "번들정리";

                //string file_name = string.Format("{0}_{1}", "번들작업결과", DateTime.Now.ToString("yyyyMMdd_hhmmss"));
                //lib.Common.Management.ExcelDB.SaveExcelDB(batchFileListPath, file_name, tempSet);

                makeLog("번들 마감", true, "번들 마감 완료");
                bowooMessageBox.Show(LanguagePack.Translate("번들 데이터 마감이 완료 되었습니다."));
            }

            catch (Exception exc)
            {
                //로그
                makeLog("파일 생성", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("번들 마감에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
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
                this.UC01_SMSGridViewPage3Top.BindData(ucGrid2_PAGE3_SelectSpName, null);
                base.HideLoading();
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
                //p.print(printString_arr, "Intermec PC43d (203 dpi)");
                //p.print(printString_arr, "ZDesigner ZT410-300dpi ZPL");
                //p.print(printString_arr, "Microsoft Print to PDF");
                p.print(printString_arr, "LABEL_" + label_no.ToString().PadLeft(2, '0'));

            });
        }



        /// <summary>
        /// 정리작업 라벨
        /// </summary>
        /// <param name="printList"></param>
        private void printMethode(DataTable tmepDT, int label_no)
        {
            Task.Run(() =>
            {
                TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                //p.print(printList, label_no);
                p.print(tmepDT, "Label_" + label_no.ToString().PadLeft(2, '0'), "bundle_label");
                //p.print(printString_arr, "Intermec PC43d");
            });
        }

        private void check_box_CheckedChanged(object sender, EventArgs e)
        {
            this.PageSearch();
          
        }

        private void radPageView1_Click(object sender, EventArgs e)
        {
            if(radPageView1.SelectedPage == this.radPageViewPage1)
            {
                check_box.Visible = true;
            }
            else
            {
                check_box.Visible = false;
            }
        }
    }
}