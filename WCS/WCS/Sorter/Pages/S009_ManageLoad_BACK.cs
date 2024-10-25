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

namespace Sorter.Pages
{
    public partial class S009_ManageLoad_BACK : lib.Common.Management.BaseControl
    {
        // SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S009_01_L_LOADING_LIST]";
        string ucGrid2SelectSpName = "[USP_S009_02_L_BOXING_LIST_DETAIL]";


        //정리작업 엘셀 파일로 저장
        string batchFileListPath = @"D:\CLEANUP_WORK";

        //RadDropDownList radDropList;

        public S009_ManageLoad_BACK()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            //버튼 설정
            UC01_SMSGridViewRight.Button1_Visible = true;
            UC01_SMSGridViewRight.Button1_Text = "재발행";
            UC01_SMSGridViewRight.Button1_Click = UC01_SMSGridViewRight_TopButton1_Click;

            UC01_SMSGridViewRight.Button2_Visible = true;
            UC01_SMSGridViewRight.Button2_Text = "엑셀저장";
            UC01_SMSGridViewRight.Button2_Click = UC01_SMSGridViewRight_TopButton2_Click;



            // Left Side Grid
            this.UC01_SMSGridViewLeft.GridTitleText = "매장 별 박스 리스트";

            // 배송업체 변경 이벤트
            //this.radDropList.SelectedIndexChanged += radDropList_SelectedIndexChanged;

            // Right Side Grid
            this.UC01_SMSGridViewRight.GridTitleText = "박스 상세 리스트";

            //PageSearch();

            this.UC01_SMSGridViewRight.GridViewData.HideSelection = true;

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

                menuId = "S009";
                menuTitle = "상차 관리";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = true;
                    mf.radDropDownListBrand.Enabled = true;
                }

                // 데이터 바인딩
                Dictionary<string, object> paramss = null;
                //paramss.Add("@HEADER_PARAMS", "SHOP_CD=" + this.radDropList.SelectedValue.ToString());

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

        /// <summary>
        /// 재발행 버튼
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
                        //rowList.Add(UC01_SMSGridViewRight.GridViewData.Rows[i]);
                    }
                }

                if (chuteChkedIdxs.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("재발행할 박스를 선택해 주세요."));
                    return;
                }



                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                {
                    bowooConfirmBox.Show(LanguagePack.Translate("재발행을 진행 하시겠습니까?"));
                }

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {

                    if (!this.UC01_SMSGridViewRight.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.UC01_SMSGridViewRight.Add_Data_Parameters.Add("R_E1", "");
                this.UC01_SMSGridViewRight.Add_Data_Parameters["R_E1"] = "";

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
                DataSet dataTable = this.UC01_SMSGridViewRight.ExcuteSaveSpXml("SP_XML_REMAIN_PRINT", chuteChkedIdxs);

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

        private void UC01_SMSGridViewRight_TopButton2_Click(object sender, EventArgs e)
        {
            //base.ShowLoading();

            try
            {



                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;
                string date = mf.calendar1.DefaultDate.Replace("-", string.Empty);
                string batch = mf.radddlwrkseq.SelectedValue.ToString();

                string ssql = "select ITEM_BAR, SUM(WRK_QTY) as WRKQTY ";
                ssql += "from IF_BOX_LIST ";
                ssql += "where BIZ_DAY = '" + date + "' ";
                ssql += "and WORK_TYPE = 4 ";
                ssql += "and batch = '" + batch + "' ";
                ssql += "group by BIZ_DAY,CHUTE_NO,ITEM_BAR,BATCH ";

                DataSet tempSet = DBUtil.ExecuteDataSet(ssql);
                SaveFileDialog file = new SaveFileDialog();
                file.InitialDirectory = batchFileListPath;
                file.Filter = "CSV file(*.csv)|*.csv";
                string fileNmaeCsv = "정리작업결과";
                file.FileName = string.Format("{0}_{1}", fileNmaeCsv, DateTime.Now.ToString("yyyyMMdd_hhmmss"));

                if (file.ShowDialog() == DialogResult.OK)
                {
                    this.ConvertToCsvFileFromGridView(tempSet.Tables[0], file.FileName, true);

                    makeLog("파일 생성", true, "파일 생성 완료");
                    bowooMessageBox.Show(LanguagePack.Translate("파일 생성이 완료되었습니다."));

                    //생성 파일 실행
                    System.Diagnostics.Process.Start(file.FileName);
                }
            }

            catch (Exception exc)
            {
                //로그
                makeLog("파일 생성", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("파일 생성에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
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