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

namespace DPS_B2B.Pages
{
    public partial class DPS001_ManageLocation : lib.Common.Management.BaseControl
    {
        string sp_Load_uC01_GridView1 = "[USP_DPS001_01_L_LOCATION_SET_LIST]";
        string sp_Load_uC01_GridView2 = "[USP_DPS001_02_L_PLAN_LIST]";

        string sp_Btn_Selected_Init = "[XML_DPS001_01_B_INIT]";
        string sp_Btn_Set_Selection = "[XML_DPS001_02_B_SET_SELECTED_ITEM]";
        string sp_Btn_Dps_Registration = "[USP_DPS001_01_B_DPS_REGISTRATION]";
        string sp_Btn_Dps_LocationUpLoad = "[XML_DPS001_03_loaciontUpload]";
        Execel execel = null;

        public DPS001_ManageLocation()
        {
            InitializeComponent();
        }

        private void DPS001_ManageLocation_Load(object sender, EventArgs e)
        {
            #region 로케이션 설정 리스트

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



            //타이틀 설정
            uC01_GridView_Left.GridTitleText = "로케이션 설정 리스트";
            
            //버튼 설정
            uC01_GridView_Left.Button1_Visible = selectedSetBtnEnable;
            uC01_GridView_Left.Button1_Text = "선택 초기화";
            uC01_GridView_Left.Button1_Click = uC01_GridView_Left_Button1_Click;

            uC01_GridView_Left.Button2_Visible = true;
            uC01_GridView_Left.Button2_Text = "존 설정";
            uC01_GridView_Left.Button2_Click = uC01_GridView_Left_Button2_Click;

            uC01_GridView_Left.Button3_Visible = dpsBtnEnable;
            uC01_GridView_Left.Button3_Text = "엑셀로드";
            uC01_GridView_Left.Button3_Click = uC01_GridView_Left_Button3_Click;


            uC01_GridView_Left.Button4_Visible = true;
            uC01_GridView_Left.Button4_Text = "체크박스";
            uC01_GridView_Left.Button4_Click = uC01_GridView_Left_Button4_Click;


            //uC01_GridView_Left.Button5_Visible = true;
            //uC01_GridView_Left.Button5_Text = "체크박스";
            //uC01_GridView_Left.Button5_Click = uC01_GridView_Left_Button5_Click;

            #endregion

            #region 설정 예정 리스트
            //타이틀 설정
            uC01_GridView_Right.GridTitleText = "설정 예정 리스트";

            //버튼 설정
            uC01_GridView_Right.Button1_Visible = selectedSetBtnEnable;
            uC01_GridView_Right.Button1_Text = "선택 설정";
            uC01_GridView_Right.Button1_Click = uC01_GridView_Right_Button1_Click;


            uC01_GridView_Right.Button2_Visible = true;
            uC01_GridView_Right.Button2_Text = "체크박스";
            uC01_GridView_Right.Button2_Click = uC01_GridView_Right_Button2_Click;

            #endregion
        }

        //로케이션 엑셀 로드 기능.
        private void uC01_GridView_Left_Button3_Click(object sender, EventArgs e)
        {

            string query = @"select line_no ,zone_no ,BLOCK_NO ,rack_no ,col_no , row_no ,MULTI_NO, kit_type, loc_no,item_cd from if_dps_location where description = 'cell' ";

            DataTable dt =  DBUtil.ExecuteDataSet(query).Tables[0];
            
            execel.excel_add(System.Configuration.ConfigurationManager.AppSettings["excelUplodaPath"].ToString(), dt);
            DataTable tempTable = execel.read(System.Configuration.ConfigurationManager.AppSettings["excelUplodaPath"].ToString());

            //var repeat = tempTable.Select("item_cd = 'M021983'").GroupBy(data => data.Field<string>("item_cd")
            //                                             )
            //                     .Select(g => new
            //                     {
            //                         key = g,
            //                     });

            var repeat = tempTable.Select().GroupBy(data => data.Field<string>("item_cd")
                                                         )
                                 .Select(g => new
                                 {
                                     key = g,
                                 });

            List<string> tempTest = new List<string>();
            foreach(var data in repeat)
            {

            }

            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                XmlDocument xml = null;
                List<Dictionary<string, string>> ParamDataList = new List<Dictionary<string, string>>();

                //work_data 업데이트 (XML로 전달)-----------------------------
                DataRow[] chiled_data = tempTable.Select();
                for (int i = 0; i < chiled_data.Length; i++)
                {
                    Dictionary<string, string> ParamData = new Dictionary<string, string>();
                    for (int j = 0; j < tempTable.Columns.Count; j++)
                    {
                        ParamData.Add(tempTable.Columns[j].ColumnName, chiled_data[i][j].ToString());
                    }
                    ParamDataList.Add(ParamData);
                }

                if (ParamDataList.Count > 0)
                {
                    xml = xmlMake(ParamDataList);
                }
                string[] returnString =  DBUtil.ExecuteUpdateParamReturnXml(sp_Btn_Dps_LocationUpLoad, xml);

                string r_ok = returnString[0];
                string r_msg = returnString[1];

                if (r_ok == "OK")
                {
                    this.PageSearch();
                    makeLog("엑셀로드", true, "로케이션 Excel 성공");
                    //설정 완료 메시지 창
                    bowooMessageBox.Show("로케이션 변경이 완료 되었습니다.");
                }
                else
                {
                    makeLog("엑셀로드", false, r_msg);
                    bowooMessageBox.Show("로케이션 등록이 실패 하였습니다.");
                }
            }
        }

        //그리드에서 선택 아이템 체크박스 체크하는 기능.
        private void uC01_GridView_Left_Button4_Click(object sender, EventArgs e)
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

        //존 설정 팝업
        private void uC01_GridView_Left_Button2_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show("권한이 없습니다.\r\n관리자에게 문의하세요.");
                return;
            }
            new Popup.DPS001_ManageLocationPopup_ZoneSetting().ShowDialog();
        }

        //선택 설정 버튼
        private void uC01_GridView_Right_Button1_Click(object sender, EventArgs e)
        {
            try
            {
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
                if (chuteChkedCnt == 0)
                {
                    bowooMessageBox.Show("선택 설정할 로케이션 설정 리스트 데이터를 선택하세요.");
                    { return; }
                }
                //설정 예정 리스트 체크 카운트
                int planChuteChkedCnt = 0;
                List<int> planChuteChkedIdxs = new List<int>();
                for (int i = 0; i < this.uC01_GridView_Right.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView_Right.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        planChuteChkedIdxs.Add(i);
                        planChuteChkedCnt++;
                    }
                }
                if (planChuteChkedCnt == 0)
                {
                    bowooMessageBox.Show("선택 설정할 설정 예정 리스트 데이터를 선택하세요.");
                    { return; }
                }

                //설정 예정 리스트가 더 많을 경우
                if (chuteChkedCnt < planChuteChkedCnt)
                {
                    
                    bowooConfirmBox.Show("선택 제품의 수가 선택 로케이션의 수보다 많습니다.\r\n선택 제품이 상단부터 순서대로, 선택하신 로케이션 수만큼만 설정됩니다.\r\n\r\n선택 설정 진행을 계속 하시겠습니까?");
                    if (bowooConfirmBox.DialogResult != DialogResult.OK)
                    { return; }
                }

                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                    bowooConfirmBox.Show("선택 설정 진행을 계속 하시겠습니까?");
                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    int roopCnt = chuteChkedCnt > planChuteChkedCnt ? planChuteChkedCnt : chuteChkedCnt;

                    if (!this.uC01_GridView_Right.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.uC01_GridView_Right.Add_Data_Parameters.Add("R_E1", "");
                    this.uC01_GridView_Right.Add_Data_Parameters["R_E1"] = "";

                    string r_ok = "";
                    string r_msg = "";

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = roopCnt;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    this.uC01_GridView_Right.Add_Set_Data_List = new List<object>();
                    for (int i = 0; i < roopCnt; i++)
                    {
                        var dataObject = new
                        {
                            //SET_ADDR = this.uC01_GridView_Left.GridViewData.Rows[chuteChkedIdxs[i]].Cells["ADDR"].Value.ToString(),
                            SET_COL_NO = this.uC01_GridView_Left.GridViewData.Rows[chuteChkedIdxs[i]].Cells["COL_NO"].Value.ToString(),
                            SET_LOC_NO = this.uC01_GridView_Left.GridViewData.Rows[chuteChkedIdxs[i]].Cells["LOC_NO"].Value.ToString()
                        };
                        this.uC01_GridView_Right.Add_Set_Data_List.Add(dataObject);
                        //PropertyInfo[] piAry = obj.GetType().GetProperties();
                        //dataObject.ConvertClassToXml
                        PropertyInfo[] piAry = dataObject.GetType().GetProperties();


                    }

                    List<int> copyArray = planChuteChkedIdxs.GetRange(0, roopCnt);


                    //Array.Copy(planChuteChkedIdxs, copyArray, roopCnt);

                    //SP실행
                    this.uC01_GridView_Right.ExcuteSaveSpXml(sp_Btn_Set_Selection, copyArray);
                    r_ok = this.uC01_GridView_Right.Usp_Save_Parameters[2].Value.ToString();
                    r_msg += this.uC01_GridView_Right.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                    if (r_ok == "OK")
                    {
                        makeLog("선택 설정", true, "설정 완료");
                        //설정 완료 메시지 창
                        bowooMessageBox.Show("선택 설정이 완료되었습니다.");

                        leftSelectedIndex = this.uC01_GridView_Left.GridViewData.Rows.IndexOf(this.uC01_GridView_Left.GridViewData.SelectedRows[0]);
                        rightSelectedIndex = this.uC01_GridView_Right.GridViewData.Rows.IndexOf(this.uC01_GridView_Right.GridViewData.SelectedRows[0]);
                        PageSearch();
                    }
                    else
                    {
                        makeLog("선택 설정", false, r_msg);
                        bowooMessageBox.Show("실패한 설정이 존재합니다.\r\n" + r_msg);
                    }

                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("선택 설정", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("선택 설정에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
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

        private void uC01_GridView_Right_Button2_Click(object sender, EventArgs e)
        {


            Popup.DPS001_CheckBoxSelect inputPopup = new Popup.DPS001_CheckBoxSelect();
            if (inputPopup.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                for (int i = 0; i < this.uC01_GridView_Right.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView_Right.GridViewData.Rows[i].IsSelected == true)
                    {
                        this.uC01_GridView_Right.GridViewData.Rows[i].Cells["checkbox"].Value = true;
                    }
                }
            }
            else if (inputPopup.DialogResult == System.Windows.Forms.DialogResult.No)
            {
                for (int i = 0; i < this.uC01_GridView_Right.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView_Right.GridViewData.Rows[i].IsSelected == true)
                    {
                        this.uC01_GridView_Right.GridViewData.Rows[i].Cells["checkbox"].Value = false;
                    }
                }
            }
        }

        //선택초기화버튼
        private void uC01_GridView_Left_Button1_Click(object sender, EventArgs e)
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
                for (int i = 0; i < this.uC01_GridView_Left.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView_Left.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chuteChkedIdxs.Add(i);
                        locChkedCnt++;
                    }
                }
                if (locChkedCnt == 0)
                {
                    bowooMessageBox.Show("초기화할 데이터를 선택하세요");
                    { return; }
                }

                //강제마감 확인 메시지 창
                bowooRedConfirmBox.Show("선택하신 로케이션의 설정 정보가 모두 삭제됩니다.\r\n\r\n초기화 진행을 계속 하시겠습니까?");
                if (bowooRedConfirmBox.DialogResult == DialogResult.Cancel)
                    return;



                makeLog("선택 초기화", true, "선택 초기화 진행 수락");

                if (!this.uC01_GridView_Left.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.uC01_GridView_Left.Add_Data_Parameters.Add("R_E1", "");
                this.uC01_GridView_Left.Add_Data_Parameters["R_E1"] = "";

                string r_ok = "";
                string r_msg = "";

                this.uC01_GridView_Left.ExcuteSaveSpXml(sp_Btn_Selected_Init, chuteChkedIdxs);
                r_ok = this.uC01_GridView_Left.Usp_Save_Parameters[2].Value.ToString();
                r_msg = this.uC01_GridView_Left.Usp_Save_Parameters[3].Value.ToString();


                if (r_ok == "OK")
                {
                    makeLog("선택 초기화", true, "선택 초기화 완료");
                    //설정 완료 메시지 창
                    bowooMessageBox.Show("초기화가 완료되었습니다.");
                }
                else
                {
                    makeLog("선택 초기화", false, r_msg);
                    bowooMessageBox.Show("실패한 처리가 존재합니다.\r\n" + r_msg);
                }

                leftSelectedIndex = this.uC01_GridView_Left.GridViewData.Rows.IndexOf(this.uC01_GridView_Left.GridViewData.SelectedRows[0]);
                PageSearch();
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("선택 초기화", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("초기화에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                base.HideLoading();
            }
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

                uC01_GridView_Right.BindData(sp_Load_uC01_GridView2, null);
                uC01_GridView_Right.SelectRow(rightSelectedIndex);
            }
            catch(Exception exc)
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
