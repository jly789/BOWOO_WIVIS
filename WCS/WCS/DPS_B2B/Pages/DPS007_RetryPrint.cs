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
using lib.Common;
using System.Reflection;
using System.Xml;
using bowoo.Framework.common;

namespace DPS_B2B.Pages
{
    public partial class DPS007_RetryPrint : lib.Common.Management.BaseControl
    {
        string sp_Load_uC01_GridView1 = "[USP_DPS007_01_L_LABEL_LIST]";
        string sp_Load_uC01_GridView2 = "[USP_DPS007_02_R_LASER_LIST]";

        string sp_Btn_Selected_Init = "[XML_DPS001_01_B_INIT]";
        string sp_Btn_Set_Selection = "[XML_DPS001_02_B_SET_SELECTED_ITEM]";
        string sp_Btn_Dps_Registration = "[USP_DPS001_01_B_DPS_REGISTRATION]";
        string sp_Btn_Dps_LocationUpLoad = "[XML_DPS001_03_loaciontUpload]";
        Execel execel = null;
        System.Data.DataTable laserPrintTabel = null;
        System.Data.DataTable labelPrintTabel = null;
        lib.Common.Management.Barcode barcode = lib.Common.Management.Barcode.getInstance();
        Printer_interpace print = new Printer_interpace();
        public string selectPrintinfo = string.Empty; //선택된 프린트 정보

        public DPS007_RetryPrint()
        {
            InitializeComponent();
        }

        private void DPS007_ManageLocation_Load(object sender, EventArgs e)
        {
            #region 라벨 리스트

            execel = new Execel();
            bool dpsBtnEnable = false;
            bool selectedSetBtnEnable = false;

            Dictionary<string, object> adminparma = new Dictionary<string, object>();
            adminparma.Add("@GET_SET_TYPE", "GET");
            adminparma.Add("@CODE_NM", "LOCMG_DPS_BTN_ENABLE");
            DataSet dsadmin = new DataSet();
            dsadmin = DBUtil.ExecuteDataSet("SP_CODE", adminparma, CommandType.StoredProcedure);
            if (dsadmin != null && dsadmin.Tables.Count > 0 && dsadmin.Tables[0].Rows.Count > 0)
            {
                dpsBtnEnable = dsadmin.Tables[0].Rows[0][0].ToString() == "1" ? true : false;
            }

            adminparma["@CODE_NM"] = "LOCMG_SELECTED_SET_BTN_ENABLE";
            dsadmin = DBUtil.ExecuteDataSet("SP_CODE", adminparma, CommandType.StoredProcedure);
            if (dsadmin != null && dsadmin.Tables.Count > 0 && dsadmin.Tables[0].Rows.Count > 0)
            {
                selectedSetBtnEnable = dsadmin.Tables[0].Rows[0][0].ToString() == "1" ? true : false;
            }
            

            laserPrintTabel = new DataTable();
            labelPrintTabel = new DataTable();

            if (laserPrintTabel.Columns.Count == 0)
            {
                laserPrintTabel.Columns.Add("work_seq");
                laserPrintTabel.Columns.Add("ord_no");
                laserPrintTabel.Columns.Add("loc_no");
                laserPrintTabel.Columns.Add("item_cd");
                laserPrintTabel.Columns.Add("item_nm");
                laserPrintTabel.Columns.Add("ord_qty");
                laserPrintTabel.Columns.Add("dork_no");
            }


            if (labelPrintTabel.Columns.Count == 0)
            {
                labelPrintTabel.Columns.Add("tns_cd");
                labelPrintTabel.Columns.Add("code128_Barcode");
                labelPrintTabel.Columns.Add("ord_no");
                labelPrintTabel.Columns.Add("work_seq");
                labelPrintTabel.Columns.Add("dork_no");
            }



            //타이틀 설정
            uC01_GridView_Left.GridTitleText = "출력 리스트";

            //버튼 설정
            uC01_GridView_Left.Button1_Visible = true;
            uC01_GridView_Left.Button1_Text = "라벨 출력";
            uC01_GridView_Left.Button1_Click = uC01_GridView_Left_Button1_Click;

            uC01_GridView_Left.Button2_Visible = true;
            uC01_GridView_Left.Button2_Text = "리스트 출력";
            uC01_GridView_Left.Button2_Click = uC01_GridView_Left_Button2_Click;


            uC01_GridView_Left.Button3_Visible = true;
            uC01_GridView_Left.Button3_Text = "체크박스";
            uC01_GridView_Left.Button3_Click = uC01_GridView_Left_Button3_Click;

            #endregion

            #region 디테일 리스트
            //타이틀 설정
            //uC01_GridView_Right.GridTitleText = "내품 상세 리스트";

            ////버튼 설정
            //uC01_GridView_Right.Button1_Visible = selectedSetBtnEnable;
            //uC01_GridView_Right.Button1_Text = "출력";
            //uC01_GridView_Right.Button1_Click = uC01_GridView_Right_Button1_Click;


            //uC01_GridView_Right.Button2_Visible = true;
            //uC01_GridView_Right.Button2_Text = "체크박스";
            //uC01_GridView_Right.Button2_Click = uC01_GridView_Right_Button2_Click;

            #endregion
        }




        //라벨 출력
        private void uC01_GridView_Left_Button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(BaseEntity.selectPrintvalue == string.Empty)
                {
                    bowooMessageBox.Show("출력할 프린터를 선택해 주세요.");
                    return;

                }
                if (bowoo.Framework.common.BaseEntity.sessLv == 2)
                {
                    bowooMessageBox.Show("권한이 없습니다.\r\n관리자에게 문의하세요.");
                    return;
                }

                //base.ShowLoading();
                bowooConfirmBox.DialogResult = DialogResult.Cancel;

                //슈트 설정 리스트 체크 카운트
                int chuteChkedCnt = 0;
                List<int> chuteChkedIdxs = new List<int>();

                for (int i = 0; i < this.uC01_GridView_Left.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView_Left.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {

                        chuteChkedIdxs.Add(i);
                        chuteChkedCnt++;
                    }
                }

                labelPrintTabel.AcceptChanges();
                if (chuteChkedCnt == 0)
                {
                    bowooMessageBox.Show("출력 리스트 데이터를 선택하세요.");
                    { return; }
                }
                //설정 예정 리스트 체크 카운트

                //DBUtil.ExecuteDataSetSqlParamXml();

                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                    bowooConfirmBox.Show("출력 진행을 계속 하시겠습니까?");
                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (int i in chuteChkedIdxs)
                    {
                        DataRow temprow = labelPrintTabel.NewRow();
                        temprow["tns_cd"] = uC01_GridView_Left.GridViewData.Rows[i].Cells["tns_cd"].Value;
                        temprow["ord_no"] = uC01_GridView_Left.GridViewData.Rows[i].Cells["ord_no"].Value;
                        temprow["work_seq"] = uC01_GridView_Left.GridViewData.Rows[i].Cells["work_seq"].Value;
                        temprow["dork_no"] = uC01_GridView_Left.GridViewData.Rows[i].Cells["dork_no"].Value;
                        labelPrintTabel.Rows.Add(temprow);
                    }
                    string err_cdoe = string.Empty;
                    
                    //코드 128로 변환
                    DataRow[] convert_barodeRow = labelPrintTabel.Select();
                    foreach (DataRow dr in convert_barodeRow)
                    {
                        dr["code128_Barcode"] = barcode.CODE128(dr["ord_no"].ToString(), "B", ref err_cdoe);
                    }
                    labelPrintTabel.AcceptChanges();

                    print.print(labelPrintTabel, BaseEntity.selectPrintvalue);
                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("라벨재 출력", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("출력에 실패 했습니다.관리자에게 문의하세요");
            }
            finally
            {
                //base.HideLoading();
                //ProgressPopupW.Close();
                labelPrintTabel.Rows.Clear();
            }
        }


        //내품리스트 출력
        private void uC01_GridView_Left_Button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (BaseEntity.selectPrintvalue == string.Empty)
                {
                    bowooMessageBox.Show("출력할 프린터를 선택해 주세요.");
                    return;

                }
                if (bowoo.Framework.common.BaseEntity.sessLv == 2)
                {
                    bowooMessageBox.Show("권한이 없습니다.\r\n관리자에게 문의하세요.");
                    return;
                }

                //base.ShowLoading();
                bowooConfirmBox.DialogResult = DialogResult.Cancel;

                //슈트 설정 리스트 체크 카운트
                int chuteChkedCnt = 0;
                List<int> chuteChkedIdxs = new List<int>();

                for (int i = 0; i < this.uC01_GridView_Left.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView_Left.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {

                        chuteChkedIdxs.Add(i);
                        chuteChkedCnt++;
                    }
                }

                labelPrintTabel.AcceptChanges();
                if (chuteChkedCnt == 0)
                {
                    bowooMessageBox.Show("출력 리스트 데이터를 선택하세요.");
                    { return; }
                }
                //설정 예정 리스트 체크 카운트

                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                    bowooConfirmBox.Show("출력 진행을 계속 하시겠습니까?");
                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {

                    char[] temp = new char[] { ';', '#' };
                    string[] tempString = BaseEntity.sessInq.Split(temp);
                    int index = 0;
                    for (int i = 0; i < tempString.Length; i++)
                    {
                        if (tempString[i].Contains("bizday"))
                        {
                            index = i;
                            break;
                        }
                    }
                    int serchIndex = tempString[index].IndexOf('=');
                    string biz_day = tempString[index].Substring(serchIndex + 1, tempString[index].Length - 1 - serchIndex);


                    foreach (int i in chuteChkedIdxs)
                    {
                        DataRow temprow = labelPrintTabel.NewRow();
                        temprow["tns_cd"] = uC01_GridView_Left.GridViewData.Rows[i].Cells["tns_cd"].Value;
                        temprow["ord_no"] = uC01_GridView_Left.GridViewData.Rows[i].Cells["ord_no"].Value;
                        temprow["work_seq"] = uC01_GridView_Left.GridViewData.Rows[i].Cells["work_seq"].Value;
                        temprow["dork_no"] = uC01_GridView_Left.GridViewData.Rows[i].Cells["dork_no"].Value;
                        labelPrintTabel.Rows.Add(temprow);
                    }

                    List<Dictionary<string, string>> ParamDataList = new List<Dictionary<string, string>>();

                    //work_data 업데이트 (XML로 전달)-----------------------------
                    DataRow[] chiled_data = labelPrintTabel.Select();
                    for (int i = 0; i < chiled_data.Length; i++)
                    {
                        Dictionary<string, string> ParamData = new Dictionary<string, string>();
                        ParamData.Add("work_seq", chiled_data[i]["work_seq"].ToString());
                        ParamData.Add("biz_day", biz_day);
                        ParamDataList.Add(ParamData);
                    }
                    DataSet tempSet = null;
                    if (ParamDataList.Count > 0)
                    {
                        XmlDocument xml = xmlMake(ParamDataList);
                        SqlParameter[] parmData = new SqlParameter[1];
                        parmData[0] = new SqlParameter();
                        parmData[0].ParameterName = "@DATA_PARAMS";
                        parmData[0].DbType = DbType.Xml;
                        parmData[0].Direction = ParameterDirection.Input;
                        parmData[0].Value = xml.OuterXml; //데이터파라미터


                        tempSet = DBUtil.ExecuteDataSetSqlParamXmlReturn(sp_Load_uC01_GridView2, parmData);
                    }

                    print.print(tempSet.Tables[0], BaseEntity.selectPrintvalue);
                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("라벨재 출력", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("출력에 실패 했습니다.관리자에게 문의하세요");
            }
            finally
            {
                //base.HideLoading();
                //ProgressPopupW.Close();
                labelPrintTabel.Rows.Clear();
            }
        }


        //그리드에서 선택 아이템 체크박스 체크하는 기능.
        private void uC01_GridView_Left_Button3_Click(object sender, EventArgs e)
        {
            Popup.DPS001_CheckBoxSelect inputPopup = new Popup.DPS001_CheckBoxSelect();
            if (inputPopup.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < this.uC01_GridView_Left.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView_Left.GridViewData.Rows[i].IsSelected == true)
                    {
                        this.uC01_GridView_Left.GridViewData.Rows[i].Cells["checkbox"].Value = true;
                    }
                }
            }
            else if (inputPopup.DialogResult == System.Windows.Forms.DialogResult.No)
            {
                for (int i = 0; i < this.uC01_GridView_Left.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView_Left.GridViewData.Rows[i].IsSelected == true)
                    {
                        this.uC01_GridView_Left.GridViewData.Rows[i].Cells["checkbox"].Value = false;
                    }
                }
            }
        }

        private XmlDocument xmlMake(List<Dictionary<string, string>> ParamDataList)
        {
            //---------------------------------------------------
            XmlDocument xdoc = new XmlDocument();

            // 루트노드
            XmlNode root = xdoc.CreateElement("root");
            xdoc.AppendChild(root);

            //데이터 노드
            XmlNode[] childNode = new XmlNode[ParamDataList.Count];

            string columnName = string.Empty;
            string columnValue = string.Empty;

            for (int i = 0; i < ParamDataList.Count; i++)
            {
                //데이터 집합 노드
                XmlNode data = xdoc.CreateElement("data");
                root.AppendChild(data);
                foreach (KeyValuePair<string, string> r in ParamDataList[i])
                {
                    columnName = r.Key;
                    columnValue = r.Value;
                    //자식 노드 태그명 설정
                    childNode[i] = xdoc.CreateElement(columnName);
                    //자식 노드 이너 값 설정
                    childNode[i].InnerText = columnValue;
                    data.AppendChild(childNode[i]);
                }
                root.AppendChild(data);
            }
            xdoc.Save(@"d:\Emp.xml");
            return xdoc;
        }




        public override void PageSearch()
        {
            base.PageSearch();
            ShowLoading();
            try
            {
                menuId = "DPS001";
                menuTitle = "로케이션 관리";

                uC01_GridView_Left.BindData(sp_Load_uC01_GridView1, null);
                uC01_GridView_Left.SelectRow(leftSelectedIndex);

                //uC01_GridView_Right.BindData(sp_Load_uC01_GridView2, null);
                //uC01_GridView_Right.SelectRow(rightSelectedIndex);
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("로케이션 관리 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("조회에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                HideLoading();
            }
        }

    }
}
