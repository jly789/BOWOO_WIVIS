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

namespace DPS_B2B.Pages
{
    public partial class DPS006_OrderData : lib.Common.Management.BaseControl
    {

        string sp_Load_uC01_GridView1 = "[USP_DPS006_01_L_PLAN_LIST]";
        string sp_Load_uC01_GridView2 = "[USP_DPS006_02_L_COMPLETE_LIST]";
        string sp_Load_uC01_GridView3 = "[USP_DPS006_03_L_COMPLETE_LIST_DETAIL]";

        string sp_Btn_Convert = "[USP_DPS006_01_B_CONVERT]";
        lib.Common.Management.JsonClass json = lib.Common.Management.JsonClass.getInstance();
        lib.Common.Management.Xml xml = lib.Common.Management.Xml.getInstance();

        string batch_no = string.Empty;
        string biz_day = string.Empty;

        public DPS006_OrderData()
        {
            InitializeComponent();
        }

        private void DPS002_OrderData_Load(object sender, EventArgs e)
        {
            //01. top left grid 설정

            //batch_no = 
            //biz_day = 
            //title
            uC01_GridView1.GridTitleText = "미변환 데이터";

            //버튼 설정
            uC01_GridView1.Button1_Visible = true;
            uC01_GridView1.Button1_Text = "변환";
            uC01_GridView1.Button1_Click = uC01_GridView1_TopButton1_Click;
            //uC01_GridView1.GridViewData.CreateCell += new GridViewCreateCellEventHandler(radGridView1_CreateCell);

            //조회 조건 숨김
            this.uC01_GridView1.HideSearchCondition();



            //02. top right grid 설정

            //title
            uC01_GridView2.GridTitleText = "변환 완료데이터";

            uC01_GridView2.childGrid = uC01_GridView3;

            //버튼 설정
            uC01_GridView2.Button1_Visible = true;
            uC01_GridView2.Button1_Text = "새로 고침";
            uC01_GridView2.Button1_Click = uC01_GridView2_TopButton1_Click;

            //조회 조건 숨김
            this.uC01_GridView2.HideSearchCondition();


            //03. bottom grid 설정

            uC01_GridView3.GridTitleText = "변환 완료 상세리스트";


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
                uC01_GridView1.BindData(sp_Load_uC01_GridView1, null);

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
            catch
            {

            }
            finally
            {
                HideLoading();
            }
        }

        //변환 버튼
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
                    bowooMessageBox.Show("변환할 데이터를 선택하세요.");
                    { return; }
                }

                //강제마감 확인 메시지 창
                bowooConfirmBox.Show("데이터 변환을 진행을 계속 하시겠습니까?");
                if (bowooConfirmBox.DialogResult == DialogResult.Cancel)
                    return;



                makeLog("변환", true, "변환 진행 수락");

                if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.uC01_GridView1.Add_Data_Parameters.Add("R_E1", "");
                this.uC01_GridView1.Add_Data_Parameters["R_E1"] = "";

                string r_ok = "";
                string r_msg = "";
                string[] recive = new string[2];
                string r_e1 = "N";
                XmlDocument convertXml = null;

                string DataParams = "";

                for (int i = 0; i < chuteChkedIdxs.Count; i++)
                {
                    if (this.uC01_GridView1.GridViewData.Rows.Count > 0 && chuteChkedIdxs[i] > -1)
                    {
                        foreach (GridViewDataColumn col in this.uC01_GridView1.GridViewData.Columns)
                        {
                            if (col.Name == "BATCH_NO")
                            {

                                DataParams += col.Name + "=" + this.uC01_GridView1.GridViewData.Rows[chuteChkedIdxs[i]].Cells[col.Name].Value.ToString() + ";#";
                                //break;
                            }
                        }
                    }
                    else
                    {
                        //*로그*
                        makeLog("변환", false, "변환 실패");
                        //설정 실패 메시지 창
                        bowooMessageBox.Show("데이터 변환에 실패하였습니다.\r\n관리자에게 문의하세요.");
                        return;
                    }
                }

                char[] splitValue = new char[] { '#' };
                string[] splitString = DataParams.Split(splitValue);
                DataTable convertTable = new DataTable();

                convertTable.Columns.Add("BATCH_NO");
                for (int i = 0; i < splitString.Length; i++)
                {


                    if (splitString[i].Contains("BATCH_NO"))
                    {
                        int start_index = splitString[i].IndexOf('=') + 1;
                        int end_index = splitString[i].IndexOf(';');
                        splitString[i] = splitString[i].Substring(start_index, end_index - start_index);

                        DataRow temp_row = convertTable.NewRow();
                        temp_row["batch_no"] = splitString[i];
                        convertTable.Rows.Add(temp_row);

                    }
                }
                convertTable.AcceptChanges();
                convertXml = xml.xmlMake(convertTable);

                do
                {
                    //SP실행
                    recive = DBUtil.ExecuteSaveParamXml(sp_Btn_Convert, convertXml, r_e1);
                    //DataSet ds = this.uC01_GridView1.ExcuteSaveSp(sp_Btn_Receive, chuteChkedIdxs);
                    //recive = DBUtil.ExecuteSaveParamXml(sp_Btn_Receive, reciveDetailXml,r_e1);

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
                                makeLog("변환", false, "로케이션 정보 미 존재 시 변환 진행 수락");
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

                    makeLog("변환", true, "변환 완료");
                    //설정 완료 메시지 창
                    bowooMessageBox.Show("데이터 변환이 완료되었습니다.");

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
                    makeLog("변환", false, r_msg);
                    bowooMessageBox.Show("실패한 처리가 존재합니다.\r\n" + r_msg);
                }

                //leftSelectedIndex = this.uC01_GridView1.GridViewData.Rows.IndexOf(this.uC01_GridView1.GridViewData.SelectedRows[0]);
                PageSearch();
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("변환", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("데이터 변환에 실패하였습니다.\r\n관리자에게 문의하세요.");
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

                menuId = "DPS006";
                menuTitle = "데이터 변환";

                uC01_GridView1.BindData(sp_Load_uC01_GridView1, null);

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
                makeLog("데이터 변환 조회", false, exc.Message.ToString());
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
