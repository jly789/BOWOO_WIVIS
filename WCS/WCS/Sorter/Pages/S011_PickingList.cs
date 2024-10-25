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
using bowoo.Framework.common;

namespace Sorter.Pages
{
    public partial class S011_PickingList : lib.Common.Management.BaseControl
    {
        // SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S011_01_L_PICKING_LIST]";
        string remain_day = string.Empty;
        string remain_batch = string.Empty;
        DataSet pickingSet = new DataSet();

        //RadDropDownList radDropList;

        public S011_PickingList()
        {
            InitializeComponent();
            //RadPanelContents.Visible = true;
            //RadPanelContents.Visible = true;

            //버튼 설정
            //UC01_SMSGridViewRight.Button1_Visible = true;
            //UC01_SMSGridViewRight.Button1_Text = "재발행";
            //UC01_SMSGridViewRight.Button1_Click = UC01_SMSGridViewRight_TopButton1_Click;



            // Left Side Grid
            this.UC01_SMSGridViewLeft.GridTitleText = "피킹리스트";

            //버튼 설정
            UC01_SMSGridViewLeft.Button1_Visible = true;
            UC01_SMSGridViewLeft.Button1_Text = "발행";
            UC01_SMSGridViewLeft.Button1_Click = UC01_SMSGridViewLeft_TopButton1_Click;


            UC01_SMSGridViewLeft.Button2_Visible = true;
            UC01_SMSGridViewLeft.Button2_Text = "검색";
            UC01_SMSGridViewLeft.Button2_Click = seaerch;



        }

        void radDropList_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            for(int i = 0; i< radCheckedDropDownList1.Items.Count(); i++)
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
                menuId = "S011";
                menuTitle = "피킹리스트";

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
                for(int i = 0; i < stringArr.Length; i++)
                {
                    if(stringArr[i].Contains("bizday"))
                    {
                        int startIndex = stringArr[i].IndexOf('=');
                        biz_day = stringArr[i].Substring(startIndex + 1, stringArr[i].Length - startIndex - 1);

                    }
                    else if(stringArr[i].Contains("wrkseq"))
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
							where BIZ_DAY = '" + biz_day + "' ";
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
                    this.UC01_SMSGridViewLeft.ds_Gird = ds;
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
                    //paramss.Add("@HEADER_PARAMS", BaseEntity.sessInq + ";#SexCd=" + comboBox1.SelectedValue.ToString() + ";#item_cd =" + sb.ToString());
                    paramss.Add("@HEADER_PARAMS", "SexCd=" + comboBox1.SelectedValue.ToString() + ";#item_cd =" + sb.ToString());
                }
                else
                {

                    //Dictionary<string, object> paramss = new Dictionary<string, object>();
                    List<string> checkList = new List<string>();
                    StringBuilder sb = new StringBuilder();
                    if(radCheckedDropDownList1.CheckedItems.Count() > 0)
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
                    //paramss.Add("@HEADER_PARAMS", BaseEntity.sessInq + ";#SexCd=" + comboBox1.SelectedValue.ToString() + ";#item_cd =" + sb.ToString());
                    paramss.Add("@HEADER_PARAMS", "SexCd=" + comboBox1.SelectedValue.ToString() + ";#item_cd =" + sb.ToString());
                }

               
                paramss.Add("@GRID_PARAMS",UC01_SMSGridViewLeft.GetGridParams());

                this.UC01_SMSGridViewLeft.BindData(ucGrid1SelectSpName, paramss);
                //pickingSet = DBUtil.ExecuteDataSet(ucGrid1SelectSpName, paramss, CommandType.StoredProcedure);

                //pickingSet = this.UC01_SMSGridViewLeft.ds_Gird;


                // 행 변경 이벤트
                //this.UC01_SMSGridViewLeft.BindData(pickingSet);
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

	        ssql =	@"	select substring(item_style,3,2) 
					from IF_ORDER
					where BIZ_DAY = '" + biz_day + "' " +
                    "and assort_cd = '' ";
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

        /// <summary>
        /// 발행 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewLeft_TopButton1_Click(object sender, EventArgs e)
        {

            try
            {

                printMethode("picking");
                makeLog("재발행", true, "재발행 완료");
                //설정 완료 메시지 창
                //bowooMessageBox.Show(LanguagePack.Translate("재발행이 완료되었습니다."));
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("재발행", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("재발행을 실패하였습니다.\r\n관리자에게 문의하세요."));

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


        /// <summary>
        /// 프린트 정보를 파싱해서 프린트 클래스에 넘김.
        /// </summary>
        /// <param name="printList"></param>
        private void printMethode(string value)
        {



            Task.Run(() =>
            {
                TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                //p.print(pickingSet.Tables[1], "Microsoft Print to PDF", value);
                //p.print(pickingSet.Tables[1], "DocuCentre-IV C2263", value);
                p.print(this.UC01_SMSGridViewLeft.ds_Gird.Tables[1], System.Configuration.ConfigurationManager.AppSettings["laserPrinter"].ToString(), value);
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
            if (comboBox1.SelectedItem != null )
          
            style_select(((KeyValuePair<string, string>)comboBox1.SelectedItem).Key.ToString());
            


        }
    }
}