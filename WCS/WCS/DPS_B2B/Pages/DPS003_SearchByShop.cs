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

namespace DPS_B2B.Pages
{
    
    public partial class DPS003_SearchByShop : lib.Common.Management.BaseControl
    {
        string sp_Load_uC01_GridView1 = "[USP_DPS003_01_L_SHOP_LIST]";
        string sp_Load_uC01_GridView2 = "[USP_DPS003_02_L_PRODUCT_LIST]";

        string ucGrid2SaveSpName = "[USP_DPS003_02_S_PRODUCT_LIST]";
        string toggleSearch = "0";

        
        public DPS003_SearchByShop()
        {
            InitializeComponent();
        }

        private void DPS003_SearchByShop_Load(object sender, EventArgs e)
        {
            this.tbtnAll.Tag = "0";
            this.tbtnCfm.Tag = "1";
            this.tbtnMissing.Tag = "2";
            this.tbtnYet.Tag = "3";

            this.tbtnAll.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            //this.uC01_GridView2.radButton1.Enabled = false;
            this.tbtnAll.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnCfm.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnMissing.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnYet.ToggleStateChanged += tbtnTrans_ToggleStateChanged;

            //타이틀 설정
            uC01_GridView1.GridTitleText = "오더 리스트";
            uC01_GridView2.GridTitleText = "제품 리스트";

            uC01_GridView1.childGrid = uC01_GridView2;

            this.uC01_GridView2.GridViewData.CellValueChanged += uC01_GridView2_CellValueChanged;
            
            this.uC01_GridView2.GridViewData.GridBehavior = new lib.Common.Management.CustomBehavior();
            
            #region 제품리스트 설정
            //버튼 설정
            uC01_GridView2.Button1_Visible = true;
            uC01_GridView2.Button1_Text = "정보 변경";
            uC01_GridView2.Button1_Click = uC01_GridView2_Button1_Click;
            #endregion
            
        }

        //bool UC01_GridView1_isModified = false;
        //셀 변경 시
        private void uC01_GridView2_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            //UC01_GridView1_isModified = true;
            try
            {
                if (e.Column.Name == "CheckBox")
                {
                    return;
                }
                base.ShowLoading();
                //새행 추가 시
                if (e.RowIndex == -1)
                    return;

                //*유효성
                //값 존재 확인
                if (string.IsNullOrEmpty(e.Value.ToString()))
                {
                    bowooMessageBox.Show("수량을 입력하세요.");
                    this.uC01_GridView2.GridViewData.CellValueChanged -= uC01_GridView2_CellValueChanged;
                    if (e.Column.Name == "WRK_QTY")
                        this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["WRK_QTY"].Value = this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["NOW_WRK_QTY"].Value;
                    if (e.Column.Name == "CAN_QTY")
                        this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["CAN_QTY"].Value = this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["NOW_CAN_QTY"].Value;
                    this.uC01_GridView2.GridViewData.CellValueChanged += uC01_GridView2_CellValueChanged;
                    return;
                }

                //수량 Validation
                if (!lib.Common.ValidationEx.IsNumber(e.Value.ToString()))
                {
                    bowooMessageBox.Show("숫자를 입력하세요.");
                    this.uC01_GridView2.GridViewData.CellValueChanged -= uC01_GridView2_CellValueChanged;
                    if (e.Column.Name == "WRK_QTY")
                        this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["WRK_QTY"].Value = this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["NOW_WRK_QTY"].Value;
                    if (e.Column.Name == "CAN_QTY")
                        this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["CAN_QTY"].Value = this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["NOW_CAN_QTY"].Value;
                    this.uC01_GridView2.GridViewData.CellValueChanged += uC01_GridView2_CellValueChanged;
                    return;
                }

                if (int.Parse(e.Value.ToString()) < 0)
                {
                    bowooMessageBox.Show("'0' 이상의 숫자를 입력하세요.");
                    this.uC01_GridView2.GridViewData.CellValueChanged -= uC01_GridView2_CellValueChanged;
                    if (e.Column.Name == "WRK_QTY")
                        this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["WRK_QTY"].Value = this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["NOW_WRK_QTY"].Value;
                    if (e.Column.Name == "CAN_QTY")
                        this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["CAN_QTY"].Value = this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["NOW_CAN_QTY"].Value;
                    this.uC01_GridView2.GridViewData.CellValueChanged += uC01_GridView2_CellValueChanged;
                    return;
                }

                string column_Str = "";
                string chg_Qty = "";

                if (e.Column.Name == "WRK_QTY")
                {
                    column_Str = "완료";
                    chg_Qty = this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["WRK_QTY"].Value.ToString();
                }
                else if (e.Column.Name == "CAN_QTY")
                {
                    column_Str = "결품";
                    chg_Qty = this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["CAN_QTY"].Value.ToString();
                }

                bowooConfirmBox.Show(string.Format("선택하신 제품의 {0}수량을 {1}(으)로 변경하시겠습니까?", column_Str, chg_Qty));
                if (bowooConfirmBox.DialogResult == DialogResult.Cancel)
                {
                    this.uC01_GridView2.GridViewData.CellValueChanged -= uC01_GridView2_CellValueChanged;
                    if (e.Column.Name == "WRK_QTY")
                        this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["WRK_QTY"].Value = this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["NOW_WRK_QTY"].Value;
                    if (e.Column.Name == "CAN_QTY")
                        this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["CAN_QTY"].Value = this.uC01_GridView2.GridViewData.Rows[e.RowIndex].Cells["NOW_CAN_QTY"].Value;
                    this.uC01_GridView2.GridViewData.CellValueChanged += uC01_GridView2_CellValueChanged;
                    return;
                }

                string r_ok = "";
                string r_msg = "";

                this.uC01_GridView2.ExcuteSaveSp(ucGrid2SaveSpName, e.RowIndex);

                r_ok = this.uC01_GridView2.Usp_Save_Parameters[2].Value.ToString();
                r_msg = this.uC01_GridView2.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                if (r_ok == "OK")
                {
                    makeLog("제품 리스트 셀 수량 변경", true, "제품 리스트 수량 변경 완료");
                    bowooMessageBox.Show(string.Format("선택하신 제품의 {0}수량이 {1}(으)로 변경 되었습니다.", column_Str, chg_Qty));
                    //this.uC01_GridView2.GridViewData.EndEdit();
                }
                else
                {
                    bowooMessageBox.Show(r_msg);
                }
                leftSelectedIndex = this.uC01_GridView1.GridViewData.Rows.IndexOf(this.uC01_GridView1.GridViewData.SelectedRows[0]);
                rightSelectedIndex = this.uC01_GridView2.GridViewData.Rows.IndexOf(this.uC01_GridView2.GridViewData.SelectedRows[0]);

                //this.uC01_GridView2.GridViewData.EndEdit();
                //this.uC01_GridView2.GridViewData.EndUpdate();
                //e.Column.ReadOnly = true;
                //if (this.uC01_GridView2.GridViewData.IsInEditMode)
                //{
                //    this.uC01_GridView2.GridViewData.EndEdit();
                //}
                PageSearch();
            }
            catch (Exception exc)
            {
                makeLog("제품 리스트 수량 변경", false, exc.Message.ToString());
                bowooMessageBox.Show("수량 변경에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                base.HideLoading();
            }
        }

        void tbtnTrans_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {

            this.tbtnAll.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;
            this.tbtnCfm.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;
            this.tbtnMissing.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;
            this.tbtnYet.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;

            this.tbtnAll.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            this.tbtnCfm.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            this.tbtnMissing.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            this.tbtnYet.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;

            RadToggleButton tbtn = sender as RadToggleButton;
            if (tbtn != null)
            {
                toggleSearch = tbtn.Tag.ToString();
                tbtn.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
                if (tbtn.Name == "tbtnAll")
                {
                    this.uC01_GridView2.radButton1.Enabled = false;
                }
                else
                {
                    this.uC01_GridView2.radButton1.Enabled = true;
                }
                PageSearch();
            }
            this.tbtnAll.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnCfm.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnMissing.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnYet.ToggleStateChanged += tbtnTrans_ToggleStateChanged;


        }

        //정보 변경 버튼
        private void uC01_GridView2_Button1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show("권한이 없습니다.\r\n관리자에게 문의하세요.");
                return;
            }

            int shopListchkedCnt = 0;
            List<int> shopListchkedIdxs = new List<int>();

            for (int i = 0; i < this.uC01_GridView1.GridViewData.Rows.Count; i++)
            {
                if ((bool)this.uC01_GridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                {
                    shopListchkedIdxs.Add(i);
                    shopListchkedCnt++;
                }
            }


            int productListchkedCnt = 0;
            List<int> productListchkedIdxs = new List<int>();
            for (int i = 0; i < this.uC01_GridView2.GridViewData.Rows.Count; i++)
            {
                if ((bool)this.uC01_GridView2.GridViewData.Rows[i].Cells["checkbox"].Value)
                {
                    productListchkedIdxs.Add(i);
                    productListchkedCnt++;
                }
            }

            if (shopListchkedCnt == 0 && productListchkedCnt == 0)
            {
                bowooMessageBox.Show("정보 변경할 데이터를 선택하세요.");
                return;
            }

            if (shopListchkedCnt == 0)
            {
                bowooMessageBox.Show("정보 변경할 오더를 선택하세요.");
                return;
            }
            if (shopListchkedCnt > 1 && productListchkedCnt > 0)
            {
                bowooMessageBox.Show("2개 이상의 오더 선택 시, 제품은 선택할 수 없습니다.");
                return;
            }

            if (shopListchkedCnt > 1)
            {
                bowooConfirmBox.Show(shopListchkedCnt.ToString() + "개의 오더가 선택되었습니다.\r\n\r\n정보 변경 진행을 계속 하시겠습니까?");
                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                    return;
            }



            leftSelectedIndex = this.uC01_GridView1.GridViewData.Rows.IndexOf(this.uC01_GridView1.GridViewData.SelectedRows[0]);
            rightSelectedIndex = this.uC01_GridView2.GridViewData.Rows.IndexOf(this.uC01_GridView2.GridViewData.SelectedRows[0]);
            DPS_B2B.Popup.DPS003_SearchByShopPopup_ChangeInfo popup = new DPS_B2B.Popup.DPS003_SearchByShopPopup_ChangeInfo(ref this.uC01_GridView1, ref this.uC01_GridView2, ref shopListchkedIdxs, ref productListchkedIdxs);
            popup.StartPosition = FormStartPosition.CenterParent;
            popup.ShowDialog();
            if (popup.isModified)
            {
                this.PageSearch();
            }


        }

        public override void PageSearch()
        {
            base.PageSearch();
            ShowLoading();
            try
            {
                menuId = "DPS004";
                menuTitle = "오더별 조회";

                Dictionary<string, object> paramss = new Dictionary<string, object>();
                paramss["@HEADER_PARAMS"] = "TOGGLE_SEARCH=" + toggleSearch.ToString();


                uC01_GridView1.GridViewData.SelectionChanged -= uC01_GridView1_SelectionChanged;
                uC01_GridView1.BindData(sp_Load_uC01_GridView1, paramss);
                
                if (this.uC01_GridView1.GridViewData.Rows.Count > 0)
                {
                    this.uC01_GridView1.GridViewData.ClearSelection();
                    uC01_GridView1.GridViewData.SelectionChanged += uC01_GridView1_SelectionChanged;
                    this.uC01_GridView1.SelectRow(leftSelectedIndex);
                }
                else
                {
                    if (this.uC01_GridView2.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS"))
                        this.uC01_GridView2.Usp_Load_Parameters["@KEY_PARAMS"] = "";
                    uC01_GridView2.BindData(sp_Load_uC01_GridView2, null);
                    this.uC01_GridView2.SelectRow(rightSelectedIndex);
                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("오더 별 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show("조회에 실패하였습니다.\r\n관리자에게 문의하세요.");
            }
            finally
            {
                HideLoading();
            }
        }

        void uC01_GridView1_SelectionChanged(object sender, EventArgs e)
        {

            if (this.uC01_GridView1.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();
                //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                if (!this.uC01_GridView2.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.uC01_GridView2.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                this.uC01_GridView2.Usp_Load_Parameters["@KEY_PARAMS"] = this.uC01_GridView1.GetKeyParam();


                //데이터 바인딩
                uC01_GridView2.BindData(sp_Load_uC01_GridView2, null);
                //this.uC01_GridView2.SelectRow(rightSelectedIndex);

                //마스터 선택 행과 체크된 행 다를 시, 마스터 행 두개 이상 체크 되었을 시 디테일 체크박스 read only
                int leftListchkedCnt = 0;
                List<int> leftListchkedIdxs = new List<int>();
                for (int i = 0; i < this.uC01_GridView1.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.uC01_GridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        leftListchkedIdxs.Add(i);
                        leftListchkedCnt++;
                    }
                }

                if (leftListchkedCnt > 1 || (leftListchkedCnt == 1 && leftListchkedIdxs[0] != this.uC01_GridView1.GridViewData.Rows.IndexOf(this.uC01_GridView1.GridViewData.SelectedRows[0])))
                {
                    this.uC01_GridView2.GridViewData.Columns["CheckBox"].ReadOnly = true;
                }

                base.HideLoading();

            }

        }

    }
}
