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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;
using System.Data.SqlClient;
using bowoo.Lib;
using bowoo.Lib.DataBase;
using Telerik.Windows.Diagrams.Core;

namespace DPS_B2B.Pages
{
    public partial class DPS002_ReceiveData : lib.Common.Management.BaseControl
    {

        string sp_Load_uC01_GridView1 = "[USP_DPS002_01_L_PLAN_LIST]";
        string sp_Load_uC01_GridView2 = "[USP_DPS002_02_L_COMPLETE_LIST]";
        string sp_Load_uC01_GridView3 = "[USP_DPS002_03_L_COMPLETE_LIST_DETAIL]";

        string sp_Btn_Receive = "[USP_DPS002_01_B_RECEIVE]";
        lib.Common.Management.JsonClass json = lib.Common.Management.JsonClass.getInstance();
        lib.Common.Management.Xml xml = lib.Common.Management.Xml.getInstance();

        string batch_no = string.Empty;
        string biz_day = string.Empty;

        public DPS002_ReceiveData()
        {
            InitializeComponent();
        }

        private void DPS002_ReceiveData_Load(object sender, EventArgs e)
        {
            //01. top left grid 설정

            //batch_no = 
            //biz_day = 
            //title
            uC01_GridView1.GridTitleText = "수신 예정 리스트";

            //버튼 설정
            uC01_GridView1.Button1_Visible = true;
            uC01_GridView1.Button1_Text = "수신";
            uC01_GridView1.Button1_Click = uC01_GridView1_TopButton1_Click;
            //uC01_GridView1.GridViewData.CreateCell += new GridViewCreateCellEventHandler(radGridView1_CreateCell);

            //조회 조건 숨김
            this.uC01_GridView1.HideSearchCondition();


            //02. top right grid 설정

            //title
            uC01_GridView2.GridTitleText = "수신 완료 리스트";

            uC01_GridView2.childGrid = uC01_GridView3;

            //버튼 설정
            uC01_GridView2.Button1_Visible = true;
            uC01_GridView2.Button1_Text = "새로 고침";
            uC01_GridView2.Button1_Click = uC01_GridView2_TopButton1_Click;

            //조회 조건 숨김
            this.uC01_GridView2.HideSearchCondition();


            //03. bottom grid 설정

            uC01_GridView3.GridTitleText = "수신 데이터 상세 리스트";


            //조회 조건 숨김
            this.uC01_GridView3.HideSearchCondition();

        }

        //새로고침
        private void uC01_GridView2_TopButton1_Click(object sender, EventArgs e)
        {
            base.PageSearch();
            ShowLoading();
            try
            {
                PageSearch();
                //uC01_GridView1.BindData(sp_Load_uC01_GridView1, null);

                //uC01_GridView2.GridViewData.SelectionChanged -= uC01_GridView2_SelectionChanged;
                //uC01_GridView2.BindData(sp_Load_uC01_GridView2, null);
                if (this.uC01_GridView2.GridViewData.Rows.Count > 0)
                {
                    uC01_GridView2.GridViewData.Rows[0].IsSelected = false;
                    uC01_GridView2.GridViewData.SelectionChanged += uC01_GridView2_SelectionChanged;
                    uC01_GridView2.GridViewData.Rows[0].IsSelected = true;
                }
                else
                {
                    if (!this.uC01_GridView3.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.uC01_GridView3.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                    this.uC01_GridView3.Usp_Load_Parameters["@KEY_PARAMS"] = "";
                    uC01_GridView3.BindData(sp_Load_uC01_GridView3, null);
                }
            }
            catch
            {

            }
            finally
            {
                HideLoading();
            }
        }

        //수신버튼
        private void uC01_GridView1_TopButton1_Click(object sender, EventArgs e)
        {
            try
            {
                base.ShowLoading();

                if (bowoo.Framework.common.BaseEntity.sessLv == 2)
                {
                    bowooMessageBox.Show("권한이 없습니다.\r\n관리자에게 문의하세요.");
                    return;
                }

                int locChkedCnt = 0;
                List<int> chuteChkedIdxs = new List<int>();
                for (int i = 0; i < this.uC01_GridView1.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chuteChkedIdxs.Add(i);
                        locChkedCnt++;
                        //break;
                    }
                }
                if (locChkedCnt == 0)
                {
                    bowooMessageBox.Show("수신할 데이터를 선택하세요.");
                    { return; }
                }

                //강제마감 확인 메시지 창
                bowooConfirmBox.Show("데이터 수신 진행을 계속 하시겠습니까?");
                if (bowooConfirmBox.DialogResult == DialogResult.Cancel)
                    return;



                makeLog("수신", true, "수신 진행 수락");

                if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.uC01_GridView1.Add_Data_Parameters.Add("R_E1", "");
                this.uC01_GridView1.Add_Data_Parameters["R_E1"] = "";

                string r_ok = "";
                string r_msg = "";
                string[] recive = new string[2];
                string r_e1 = "N";
                XmlDocument reciveDetailXml = null;

                string DataParams = "";


                for (int i = 0; i < chuteChkedIdxs.Count; i++)
                {
                    if (this.uC01_GridView1.GridViewData.Rows.Count > 0 && chuteChkedIdxs[i] > -1)
                    {
                        foreach (GridViewDataColumn col in this.uC01_GridView1.GridViewData.Columns)
                        {
                            if (col.Name == "Batch")
                            {

                                DataParams += col.Name + "=" + this.uC01_GridView1.GridViewData.Rows[chuteChkedIdxs[i]].Cells[col.Name].Value.ToString() + ";#";
                                //break;
                            }
                        }
                    }
                }
                //string[] splitValue = new string[] { ";#" };
                char[] splitValue = new char[] { '#' };
                string[] splitString = DataParams.Split(splitValue);
                //string[] splitString = DataParams.Split(splitValue, 0);
                DataTable reciveDetail = new DataTable();
                //수신 상세 리스트
                //reciveDetail = new DataTable();
                reciveDetail.Columns.Add("BizDay");
                reciveDetail.Columns.Add("TnsCode");
                reciveDetail.Columns.Add("Batch");
                reciveDetail.Columns.Add("OrdNo");
                reciveDetail.Columns.Add("OrdSeq");
                //reciveDetail.Columns.Add("ItemLoc");
                reciveDetail.Columns.Add("ItemBar");
                reciveDetail.Columns.Add("ItemCode");
                reciveDetail.Columns.Add("ItemName");
                reciveDetail.Columns.Add("OrdQty");
                for (int i = 0; i < splitString.Length; i++)
                {
                    if (splitString[i].Contains("Batch"))
                    {
                        int start_index = splitString[i].IndexOf('=') + 1;
                        int end_index = splitString[i].IndexOf(';');
                        splitString[i] = splitString[i].Substring(start_index, end_index - start_index);

                        //json 으로 추가
                        Dictionary<string, string> temp_data = new Dictionary<string, string>();
                        temp_data.Add("BizDay", biz_day);
                        temp_data.Add("WhCod", "CN01");
                        temp_data.Add("key", "DzOY4X32X9FLa4CI5eRF");
                        temp_data.Add("Batch", splitString[i]);
                        JObject jo = json.HttpCall("http://cbt.htns.com:8080/api/dps/RcvDataDL.do?", temp_data);
                        //JObject jo = json.HttpCall("http://cbtdev.htns.com:8090/api/dps/RcvDataDL.do?", temp_data);
                        var dataList = jo.SelectToken("DATA_LIST");
                        var status = jo.SelectToken("STATUS").ToString();

                        if (dataList.Count() > 0 && status == "SUCCESS")
                        {
                            foreach (var item in dataList)
                            {
                                DataRow temp_row = reciveDetail.NewRow();
                                temp_row["BizDay"] = item.SelectToken("BizDay").ToString();
                                temp_row["TnsCode"] = item.SelectToken("TnsCode").ToString();
                                temp_row["Batch"] = item.SelectToken("Batch").ToString();
                                temp_row["OrdNo"] = item.SelectToken("OrdNo").ToString();
                                temp_row["OrdSeq"] = item.SelectToken("OrdSeq").ToString();
                                //temp_row["ItemLoc"] = item.SelectToken("ItemLoc").ToString();
                                temp_row["ItemBar"] = item.SelectToken("ItemBar").ToString();
                                temp_row["ItemCode"] = item.SelectToken("ItemCode").ToString();
                                temp_row["ItemName"] = item.SelectToken("ItemName").ToString();
                                temp_row["OrdQty"] = item.SelectToken("OrdQty").ToString();
                                reciveDetail.Rows.Add(temp_row);
                            }

                        }
                        else
                        {
                            //*로그*
                            makeLog("수신", false, "수신 실패");
                            //설정 실패 메시지 창
                            bowooMessageBox.Show("수신에 실패하였습니다.\r\n관리자에게 문의하세요.");
                            return;
                        }
                    }
                }
                reciveDetailXml = xml.xmlMake(reciveDetail);
                ////string[] test12345 = DataParams.Split(test3, 1);
                //int start_index = DataParams.IndexOf('=') + 1;
                //int end_index = DataParams.IndexOf(';');
                //DataParams = DataParams.Substring(start_index, end_index - start_index);
                ////json 으로 추가
                //Dictionary<string, string> temp_data = new Dictionary<string, string>();
                //temp_data.Add("BizDay", biz_day);
                //temp_data.Add("WhCod", "CN01");
                //temp_data.Add("key", "DzOY4X32X9FLa4CI5eRF");
                //temp_data.Add("Batch", DataParams);
                //JObject jo = json.HttpCall("http://cbtdev.htns.com:8090/api/dps/RcvDataDL.do?", temp_data);
                //var dataList = jo.SelectToken("DATA_LIST");
                //var status = jo.SelectToken("STATUS").ToString();

                //if (dataList.Count() > 0 && status == "SUCCESS")
                //{
                //    //수신 상세 리스트
                //    DataTable reciveDetail = new DataTable();
                //    reciveDetail.Columns.Add("BizDay");
                //    reciveDetail.Columns.Add("TnsCode");
                //    reciveDetail.Columns.Add("Batch");
                //    reciveDetail.Columns.Add("OrdNo");
                //    reciveDetail.Columns.Add("OrdSeq");
                //    //reciveDetail.Columns.Add("ItemLoc");
                //    reciveDetail.Columns.Add("ItemBar");
                //    reciveDetail.Columns.Add("ItemCode");
                //    reciveDetail.Columns.Add("ItemName");
                //    reciveDetail.Columns.Add("OrdQty");


                //    foreach (var item in dataList)
                //    {
                //        DataRow temp_row = reciveDetail.NewRow();
                //        temp_row["BizDay"] = item.SelectToken("BizDay").ToString();
                //        temp_row["TnsCode"] = item.SelectToken("TnsCode").ToString();
                //        temp_row["Batch"] = item.SelectToken("Batch").ToString();
                //        temp_row["OrdNo"] = item.SelectToken("OrdNo").ToString();
                //        temp_row["OrdSeq"] = item.SelectToken("OrdSeq").ToString();
                //        //temp_row["ItemLoc"] = item.SelectToken("ItemLoc").ToString();
                //        temp_row["ItemBar"] = item.SelectToken("ItemBar").ToString();
                //        temp_row["ItemCode"] = item.SelectToken("ItemCode").ToString();
                //        temp_row["ItemName"] = item.SelectToken("ItemName").ToString();
                //        temp_row["OrdQty"] = item.SelectToken("OrdQty").ToString();
                //        reciveDetail.Rows.Add(temp_row);
                //    }
                //    reciveDetailXml = xml.xmlMake(reciveDetail);

                //}
                //else
                //{
                //    //*로그*
                //    makeLog("수신", false, "수신 실패");
                //    //설정 실패 메시지 창
                //    bowooMessageBox.Show("수신에 실패하였습니다.\r\n관리자에게 문의하세요.");
                //    return;
                //}
                do
                {
                    //SP실행
                    //this.uC01_GridView1.ExcuteSaveSp(sp_Btn_Receive, chuteChkedIdxs);
                    recive = DBUtil.ExecuteSaveParamXml(sp_Btn_Receive, reciveDetailXml, r_e1);

                    r_ok = recive[0];
                    r_msg = recive[1];

                    //ProgressPopupW.progressBar1.PerformStep();

                    if (r_ok != "OK")
                    {
                        if (r_ok == "NG")
                        {
                            bowooMessageBox.Show(r_msg);
                            return;
                        }
                        else
                        {
                            bowooRedConfirmBox.Show(r_msg);
                        }

                        if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                        {
                            //E1 :
                            if (r_ok == "E1")
                            {
                                makeLog("수신", false, "로케이션 정보 미 존재 시 수신 진행 수락");
                                r_e1 = "Y";
                            }
                        }
                        else
                        {
                            return;
                        }

                    }
                } while (r_ok != "OK");

                if (r_ok == "OK")
                {
                    //json 으로 추가
                    Dictionary<string, string> json_send = new Dictionary<string, string>();
                    json_send.Add("BizDay", biz_day);
                    json_send.Add("WhCod", "CN01");
                    json_send.Add("key", "DzOY4X32X9FLa4CI5eRF");
                    json_send.Add("Batch", DataParams);
                    json.HttpCall("http://cbt.htns.com:8080/api/dps/RcvDataUP.do?", json_send);
                    //json.HttpCall("http://cbtdev.htns.com:8090/api/dps/RcvDataUP.do?", json_send);

                    makeLog("수신", true, "수신 완료");
                    //설정 완료 메시지 창
                    bowooMessageBox.Show("수신이 완료되었습니다.");

                    DPS_B2B.MainForm mf = this.FindForm() as DPS_B2B.MainForm;
                    if (mf != null)
                    {
                        mf.radddlwrkseq.SelectedValueChanged -= mf.radddlwrkseq_SelectedValueChanged;
                        //mf.setWH();
                        mf.setWorkSeq();
                        mf.MakeSessionInq();
                        mf.radddlwrkseq.SelectedValueChanged += mf.radddlwrkseq_SelectedValueChanged;
                    }
                }
                else
                {
                    makeLog("수신", false, r_msg);
                    bowooMessageBox.Show("실패한 처리가 존재합니다.\r\n" + r_msg);
                }

                //leftSelectedIndex = this.uC01_GridView1.GridViewData.Rows.IndexOf(this.uC01_GridView1.GridViewData.SelectedRows[0]);
                PageSearch();
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("수신", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("수신에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                //ProgressPopupW.Close();
                base.HideLoading();
            }
        }

        public override void PageSearch()
        {
            base.PageSearch();
            ShowLoading();
            try
            {
                string temp_string = bowoo.Framework.common.BaseEntity.sessInq;
                char[] test = new char[] { ';', '#' };
                string[] temp_string_arr = temp_string.Split(test);

                foreach (string sr in temp_string_arr)
                {
                    if (sr.Contains("bizday"))
                    {
                        int index = sr.IndexOf('=');
                        biz_day = sr.Substring(index + 1, sr.Length - index - 1);
                    }
                    else if (sr.Contains("wrkseq"))
                    {
                        int index = sr.IndexOf('=');
                        batch_no = sr.Substring(index + 1, sr.Length - index - 1);
                    }
                }

                menuId = "DPS002";
                menuTitle = "데이터 수신";

                //json 으로 추가
                Dictionary<string, string> temp_data = new Dictionary<string, string>();
                temp_data.Add("BizDay", biz_day);
                temp_data.Add("WhCod", "CN01");
                temp_data.Add("key", "DzOY4X32X9FLa4CI5eRF");
                JObject jo = json.HttpCall("http://cbt.htns.com:8080/api/dps/RcvDataList.do?", temp_data);
                //JObject jo = json.HttpCall("http://cbtdev.htns.com:8090/api/dps/RcvDataList.do?", temp_data);
                var a = jo.SelectToken("DATA_LIST");

                GridViewCheckBoxColumn customColumn = new GridViewCheckBoxColumn("CheckBox");

                DataSet ds = new DataSet();
                DataTable headerTable = new DataTable();
                headerTable.Columns.Add("column1");
                headerTable.Columns.Add("column2");
                headerTable.Columns.Add("column3");
                headerTable.Columns.Add("column4");
                headerTable.Columns.Add("column5");
                headerTable.Columns.Add("column6");

                //그리드 해더 생성
                DataRow temp_addRow = headerTable.NewRow();
                temp_addRow["column1"] = "<HEADERCHECKBOX=FALSE;WIDTH=20>";
                temp_addRow["column2"] = "<WIDTH=100;TEXTALIGN=CENTER;STITLE=TOTAL>작업일자";
                temp_addRow["column3"] = "<WIDTH=100;TEXTALIGN=CENTER;DATA=TRUE>생성 차수";
                temp_addRow["column4"] = "<WIDTH=55;TEXTALIGN=RIGHT>오더 수";
                temp_addRow["column5"] = "<WIDTH=55;TEXTALIGN=RIGHT>제품 수";
                temp_addRow["column6"] = "<WIDTH=55;TEXTALIGN=RIGHT;SUM=TRUE>예정수량";
                headerTable.Rows.Add(temp_addRow);
                ds.Tables.Add(headerTable);

                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("CheckBox", typeof(Boolean));
                dataTable.Columns.Add("BizDay");
                dataTable.Columns.Add("Batch");
                dataTable.Columns.Add("CustCnt");
                dataTable.Columns.Add("ItemCnt");
                dataTable.Columns.Add("TotalQty");

                foreach (var item in a)
                {
                    DataRow temp_row = dataTable.NewRow();
                    temp_row["CheckBox"] = 0;
                    temp_row["BizDay"] = item.SelectToken("BizDay").ToString();
                    temp_row["Batch"] = item.SelectToken("Batch").ToString();
                    temp_row["CustCnt"] = item.SelectToken("CustCnt").ToString();
                    temp_row["ItemCnt"] = item.SelectToken("ItemCnt").ToString();
                    temp_row["TotalQty"] = item.SelectToken("TotalQty").ToString();
                    dataTable.Rows.Add(temp_row);
                }

                Dictionary<string, object> paramWorkSeq = new Dictionary<string, object>();
                paramWorkSeq["@BIZ_DAY"] = biz_day;

                //수신 받은 배치 데이터 확인 이미수신 받은 데이터는 그리드에 표시 안하기 위함.
                DataTable dtWrkSeq = DBUtil.ExecuteDataSet("DBO.SP_GET_RECIVE_ORDER", paramWorkSeq, CommandType.StoredProcedure).Tables[0];
                DataRow[] temp_recive_data = dtWrkSeq.Select();
                string batch_arr = string.Empty;
                foreach (DataRow dr in temp_recive_data)
                {
                    batch_arr += dr["batch"].ToString() + ",";
                }
                if (batch_arr.Length > 0)
                {
                    batch_arr = batch_arr.Substring(0, batch_arr.Length - 1);

                    DataRow[] select_batch = dataTable.Select("BizDay = " + biz_day + " and Batch in(" + batch_arr + ")", "batch asc");
                    foreach (DataRow dr in select_batch)
                    {
                        dr.Delete();
                    }
                    dataTable.AcceptChanges();
                }

                ds.Tables.Add(dataTable);
                uC01_GridView1.BindData(ds);

                uC01_GridView2.GridViewData.SelectionChanged -= uC01_GridView2_SelectionChanged;
                uC01_GridView2.BindData(sp_Load_uC01_GridView2, null);
                if (this.uC01_GridView2.GridViewData.Rows.Count > 0)
                {
                    uC01_GridView2.GridViewData.Rows[0].IsSelected = false;
                    uC01_GridView2.GridViewData.SelectionChanged += uC01_GridView2_SelectionChanged;
                    uC01_GridView2.GridViewData.Rows[0].IsSelected = true;
                }
                else
                {
                    if (!this.uC01_GridView3.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.uC01_GridView3.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                    this.uC01_GridView3.Usp_Load_Parameters["@KEY_PARAMS"] = "";
                    uC01_GridView3.BindData(sp_Load_uC01_GridView3, null);
                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("데이터 수신 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("데이터 조회에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                HideLoading();
            }
        }


        void uC01_GridView2_SelectionChanged(object sender, EventArgs e)
        {

            if (this.uC01_GridView2.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();
                //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                if (!this.uC01_GridView3.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.uC01_GridView3.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                this.uC01_GridView3.Usp_Load_Parameters["@KEY_PARAMS"] = this.uC01_GridView2.GetKeyParam();


                //데이터 바인딩
                uC01_GridView3.BindData(sp_Load_uC01_GridView3, null);
                base.HideLoading();
            }

        }


    }
}
