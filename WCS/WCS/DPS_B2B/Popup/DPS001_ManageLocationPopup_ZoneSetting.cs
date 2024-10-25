using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DPS_B2B.Popup
{
    public partial class DPS001_ManageLocationPopup_ZoneSetting : lib.Common.Management.BaseForm
    {
        public bool isModified = false;
        string sp_Load_uC01_GridView1 = "[USP_DPS001_01_L_POP_ZONE_SET_LIST]";
        //string sp_Load_uC01_GridView2 = "[USP_DPS001_01_L_POP_LOC_MASTER_LIST]";

        string sp_Btn_Selected_Set = "[XML_DPS001_01_B_POP_ZONE_SET]";
        string sp_Btn_AddType = "[USP_DPS001_01_B_POP_TYPE_ADD]";
        string sp_Btn_SetType = "[USP_DPS001_01_B_POP_TYPE_SET]";


        public DPS001_ManageLocationPopup_ZoneSetting()
        {
            InitializeComponent();
            this.CenterToParent();
        }




        //팝업 로드시 메서드.
        private void DPS001_ManageLocationPopup_ZoneSetting_Load(object sender, EventArgs e)
        {

            menuId = "DPS001";
            menuTitle = "매장별 조회 - 존 설정 팝업";


            this.uC01_GridView1.GridTitleText = "설정 리스트";
            /*
            this.uC01_GridView1.radButton1.Width = 50;
            this.uC01_GridView1.radButton1.Location = new Point(150,4);
            //this.uC01_GridView1.radButton1.Font = new System.Drawing.Font("Segoe UI", 8, FontStyle.Bold);
            this.uC01_GridView1.radButton2.Width = 50;
            this.uC01_GridView1.radButton2.Location = new Point(100, 4);
            this.uC01_GridView1.radButton2.Font = new System.Drawing.Font("Segoe UI", 8, FontStyle.Bold);
            this.uC01_GridView1.radButton3.Width = 50;
            this.uC01_GridView1.radButton3.Location = new Point(50, 4);
            this.uC01_GridView1.radButton3.Font = new System.Drawing.Font("Segoe UI", 8, FontStyle.Bold);
            
            this.uC01_GridView1.Button1_Visible = true;
            this.uC01_GridView1.Button1_Text = "추가";
            this.uC01_GridView1.Button1_Click = uC01_GridView1_Button1_Click;
            */
            this.uC01_GridView1.Button1_Visible = true;
            this.uC01_GridView1.Button1_Text = "새로고침";
            this.uC01_GridView1.Button1_Click = uC01_GridView1_Button1_Click;

            this.uC01_GridView1.Button2_Visible = true;
            this.uC01_GridView1.Button2_Text = "선택설정";
            this.uC01_GridView1.Button2_Click = uC01_GridView1_Button2_Click;

            this.uC01_GridView1.GridViewData.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;

            //Sector list binding
            this.radDropDownListLine.ValueMember = "LINE_NO";
            this.radDropDownListLine.DisplayMember = "LINE_NO";
            this.radDropDownListLine.DataSource =
                DBUtil.ExecuteDataSet(string.Format("select distinct LINE_NO from IF_DPS_LOCATION where PC_NO = {0} order by 1", bowoo.Framework.common.BaseEntity.sessPc.ToString())).Tables[0];
            this.radDropDownListLine.SelectedValueChanged += radDropDownListType_ValueChanged;

            if (this.radDropDownListLine.Items.Count > 0)
                this.radDropDownListLine.SelectedIndex = 0;

            //master type binding
            this.radDropDownListType.ValueMember = "type_cd";
            this.radDropDownListType.DisplayMember = "type_cd";
            this.radDropDownListType.DataSource =
                DBUtil.ExecuteDataSet(string.Format("select distinct type_cd, type_cd from IF_DPS_LOCMASTER order by 1")).Tables[0];

            if (this.radDropDownListType.Items.Count > 0)
                this.radDropDownListType.SelectedIndex = 0;

            //add event
            this.radBtnAddType.Click += radBtnAddType_Click;
            this.radBtnSetType.Click += radBtnSetType_Click;

            PageSearch();
        }

        //타입 추가
        void radDropDownListType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                PageSearch();
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("로케이션 타입 추가", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("타입 추가에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                HideLoading();
            }
        }

        //타입 적용
        void radBtnSetType_Click(object sender, EventArgs e)
        {
            try
            {
                base.ShowLoading();

                if (bowoo.Framework.common.BaseEntity.sessLv == 2)
                {
                    bowooMessageBox.Show("권한이 없습니다.\r\n관리자에게 문의하세요.");
                    return;
                }

                //진행 확인 메시지 창
                bowooRedConfirmBox.Show("선택하신 타입을 설정 리스트에 적용하시겠습니까?");
                if (bowooRedConfirmBox.DialogResult == DialogResult.Cancel)
                    return;

                makeLog("로케이션 타입 적용", true, "로케이션 타입 적용 진행 수락");

                //선택된 타입 저장
                this.uC01_GridView1.Add_Data_Parameters["TYPE_CD"] = this.radDropDownListType.SelectedValue.ToString();

                string r_ok = "";
                string r_msg = "";
                do
                {
                    //SP실행
                    this.uC01_GridView1.ExcuteSaveSp(sp_Btn_SetType);

                    r_ok = this.uC01_GridView1.Usp_Save_Parameters[2].Value.ToString();
                    r_msg = this.uC01_GridView1.Usp_Save_Parameters[3].Value.ToString();

                    if (r_ok != "OK")
                    {
                        if (r_ok == "NG")
                        {
                            bowooMessageBox.Show(r_msg);
                            return;
                        }
                        else
                        {
                            bowooConfirmBox.Show(r_msg);

                            if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                //E1 :
                                if (r_ok == "E1")
                                {
                                    makeLog("로케이션 타입 적용", false, "");
                                    this.uC01_GridView1.Add_Data_Parameters["R_E1"] = "Y";
                                }
                            }
                        }
                    }
                } while (r_ok != "OK");

                makeLog("로케이션 타입 적용", true, "타입 적용 완료");
                //설정 완료 메시지 창
                bowooMessageBox.Show("타입 적용이 완료되었습니다.");

                PageSearch();
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("로케이션 타입 적용", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("타입 적용에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                HideLoading();
            }
        }

        //타입 추가
        void radBtnAddType_Click(object sender, EventArgs e)
        {
            try
            {
                base.ShowLoading();

                if (bowoo.Framework.common.BaseEntity.sessLv == 2)
                {
                    bowooMessageBox.Show("권한이 없습니다.\r\n관리자에게 문의하세요.");
                    return;
                }

                //진행 확인 메시지 창
                bowooRedConfirmBox.Show("현재의 설정 리스트를 타입 목록에 추가하시겠습니까?");
                if (bowooRedConfirmBox.DialogResult == DialogResult.Cancel)
                    return;

                makeLog("로케이션 타입 추가", true, "로케이션 타입 추가 진행 수락");

                string r_ok = "";
                string r_msg = "";
                do
                {
                    //SP실행
                    this.uC01_GridView1.ExcuteSaveSp(sp_Btn_AddType);

                    r_ok = this.uC01_GridView1.Usp_Save_Parameters[2].Value.ToString();
                    r_msg = this.uC01_GridView1.Usp_Save_Parameters[3].Value.ToString();

                    if (r_ok != "OK")
                    {
                        if (r_ok == "NG")
                        {
                            bowooMessageBox.Show(r_msg);
                            return;
                        }
                        else
                        {
                            bowooConfirmBox.Show(r_msg);

                            if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                //E1 :
                                if (r_ok == "E1")
                                {
                                    makeLog("로케이션 타입 추가", false, "");
                                    this.uC01_GridView1.Add_Data_Parameters["R_E1"] = "Y";
                                }
                            }
                        }
                    }
                } while (r_ok != "OK");

                makeLog("로케이션 타입 추가", true, "타입 추가 완료");
                //설정 완료 메시지 창
                bowooMessageBox.Show("타입 추가가 완료되었습니다.");

                //type 콤보 갱신
                this.radDropDownListType.DataSource =
                    DBUtil.ExecuteDataSet(string.Format("select distinct type_cd, type_cd from IF_DPS_LOCMASTER order by 1")).Tables[0];
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("로케이션 타입 추가", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("타입 추가에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                HideLoading();
            }
        }


        //새로고침
        private void uC01_GridView1_Button1_Click(object sender, EventArgs e)
        {
            PageSearch();
        }

        //선택 설정
        private void uC01_GridView1_Button2_Click(object sender, EventArgs e)
        {

            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show("권한이 없습니다.\r\n관리자에게 문의하세요.");
                return;
            }

            //슈트 설정 리스트 체크 카운트
            int chuteChkedCnt = 0;
            List<int> chuteChkedIdxs = new List<int>();

            for (int i = 0; i < this.uC01_GridView1.GridViewData.Rows.Count; i++)
            {
                if ((bool)this.uC01_GridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                {
                    chuteChkedIdxs.Add(i);
                    chuteChkedCnt++;
                }
            }
            if (chuteChkedCnt == 0)
            {
                bowooMessageBox.Show("선택 설정할 설정 리스트 데이터를 선택하세요.");
                { return; }
            }

            Popup.DPS001_ZoneSettingPopup_Input inputPopup = new Popup.DPS001_ZoneSettingPopup_Input();
            if (inputPopup.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                makeLog("ZONE NO 입력 팝업", true, "선택 설정 진행 수락");

                //숫자 검증
                if (string.IsNullOrEmpty(inputPopup.inputZoneNo))
                {
                    bowooMessageBox.Show("수량을 입력하세요.");
                    return;
                }
                if (!lib.Common.ValidationEx.IsNumber(inputPopup.inputZoneNo))
                {
                    bowooMessageBox.Show("숫자를 입력하세요.");
                    return;
                }

                if (!this.uC01_GridView1.Add_Data_Parameters.ContainsKey("INPUT_ZONE_NO"))
                    this.uC01_GridView1.Add_Data_Parameters.Add("INPUT_ZONE_NO", "");
                this.uC01_GridView1.Add_Data_Parameters["INPUT_ZONE_NO"] = inputPopup.inputZoneNo;


                //foreach (GridViewDataColumn col in this.uC01_GridView1.GridViewData.Columns)
                //{
                //    string wewe = col.Name;
                //}
                //string dwd = uC01_GridView1.GridViewData.Rows[0].Cells[1].Value.ToString();
                //string dwd1 = uC01_GridView1.GridViewData.Rows[0].Cells["LINE_NO"].Value.ToString();
                //JArray json_test = new JArray();
                //for (int i = 0; i < chuteChkedIdxs.Count; i++)
                //{
                //    JObject test11 = new JObject();
                //    test11.Add("line_no", "1");
                //    test11.Add("rack_no", "1");
                //    test11.Add("zone_no", "1");

                //    json_test.Add(test11);

                //    //var json_test = JObject.FromObject(new
                //    //{
                //    //    line_no = uC01_GridView1.GridViewData.Rows[chuteChkedIdxs[i]].Cells["line_no"].Value.ToString(),
                //    //    rack = uC01_GridView1.GridViewData.Rows[chuteChkedIdxs[i]].Cells["rack_no"].Value.ToString(),
                //    //    zone = uC01_GridView1.GridViewData.Rows[chuteChkedIdxs[i]].Cells["zone_no"].Value.ToString()
                //    //});
                //}
                //Console.WriteLine(json_test);

                //for (int i = 0; i < chuteChkedIdxs.Count; i++)
                //{
                //    //var json_test = JObject.FromObject(new
                //    //{
                //    //    line_no = uC01_GridView1.GridViewData.Rows[chuteChkedIdxs[i]].Cells["line_no"].Value.ToString(),
                //    //    rack = uC01_GridView1.GridViewData.Rows[chuteChkedIdxs[i]].Cells["rack_no"].Value.ToString(),
                //    //    zone = uC01_GridView1.GridViewData.Rows[chuteChkedIdxs[i]].Cells["zone_no"].Value.ToString()
                //    //});
                //    do
                //    {

                //        //SP실행
                //        this.uC01_GridView1.ExcuteSaveSp(sp_Btn_Selected_Set, chuteChkedIdxs[i]);

                //        r_ok = this.uC01_GridView1.Usp_Save_Parameters[2].Value.ToString();
                //        r_msg = this.uC01_GridView1.Usp_Save_Parameters[3].Value.ToString();

                //        if (r_ok != "OK")
                //        {
                //            if (r_ok == "NG")
                //            {
                //                bowooMessageBox.Show(r_msg);
                //                return;
                //            }
                //        }
                //    } while (r_ok != "OK");
                //}


                this.uC01_GridView1.ExcuteSaveSpXml(sp_Btn_Selected_Set, chuteChkedIdxs);

                string r_ok = "";
                string r_msg = "";

                r_ok = this.uC01_GridView1.Usp_Save_Parameters[2].Value.ToString();
                r_msg = this.uC01_GridView1.Usp_Save_Parameters[3].Value.ToString();

                if (r_ok != "OK")
                {
                    if (r_ok == "NG")
                    {
                        bowooMessageBox.Show(r_msg);
                        return;
                    }
                }

                makeLog("ZONE NO 입력 팝업", true, "선택 설정 완료");
                //설정 완료 메시지 창
                bowooMessageBox.Show("선택 설정이 완료되었습니다.");

                PageSearch();
            }

        }

        //조회
        public void PageSearch()
        {
            //ShowLoading();
            try
            {
                uC01_GridView1.DataBindings.Clear();
                menuId = "DPS001";
                menuTitle = "로케이션 관리 존 설정 팝업";

                Dictionary<string, object> paramss = new Dictionary<string, object>();
                paramss.Add("@HEADER_PARAMS", "SECTOR_NO=" + this.radDropDownListLine.SelectedValue.ToString()
                        + ";#" + "TYPE=" + this.radDropDownListType.SelectedValue.ToString());

                uC01_GridView1.BindData(sp_Load_uC01_GridView1, paramss);
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("로케이션 관리 존 설정 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("조회에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                //HideLoading();
            }
        }

    }
}
