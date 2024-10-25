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

namespace Sorter.Pages
{
    public partial class S005_SearchByProduct : lib.Common.Management.BaseControl
    {
        //SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S005_01_L_PRODUCT_LIST]";
        string ucGrid2SelectSpName = "[USP_S005_02_L_SHOP_LIST]";

        string toggleSearch = "-1"; //전체검색

        public S005_SearchByProduct()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            //left side grid
            this.UC01_SMSGridView1.GridTitleText = "제품 리스트";


            //right side grid
            this.UC01_SMSGridView2.GridTitleText = "매장 리스트";

            this.UC01_SMSGridView2.Button1_Visible = false;
            this.UC01_SMSGridView2.Button1_Text = "정보 변경";
            this.UC01_SMSGridView2.Button1_Click = UC01_SMSGridView2_TopButton1_Click;

            this.tbtnAll.Tag = "-1";  //전체
            this.tbtnWorkYet.Tag = "0"; //미작업
            this.tbtnMissing.Tag = "1"; //결품
            this.tbtnWorkDone.Tag = "9"; //작업완료

            tbtnAll.Text = LanguagePack.Translate(tbtnAll.Text);
            tbtnWorkDone.Text = LanguagePack.Translate(tbtnWorkDone.Text);
            tbtnMissing.Text = LanguagePack.Translate(tbtnMissing.Text);
            tbtnWorkYet.Text = LanguagePack.Translate(tbtnWorkYet.Text);

            this.UC01_SMSGridView2.radButton1.Enabled = false;
            this.tbtnAll.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;
            this.tbtnAll.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnWorkDone.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnMissing.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnWorkYet.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
        }

        void tbtnTrans_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            this.tbtnAll.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;
            this.tbtnWorkDone.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;
            this.tbtnMissing.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;
            this.tbtnWorkYet.ToggleStateChanged -= tbtnTrans_ToggleStateChanged;

            this.tbtnAll.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            this.tbtnWorkDone.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            this.tbtnMissing.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;
            this.tbtnWorkYet.ToggleState = Telerik.WinControls.Enumerations.ToggleState.Off;

            RadToggleButton tbtn = sender as RadToggleButton;

            if (tbtn != null)
            {
                toggleSearch = tbtn.Tag.ToString();
                tbtn.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On;

                if (tbtn.Name == "tbtnAll")
                {
                    this.UC01_SMSGridView2.radButton1.Enabled = false;
                }

                else
                {
                    this.UC01_SMSGridView2.radButton1.Enabled = true;
                }

                PageSearch();
            }

            this.tbtnAll.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnWorkDone.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnMissing.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
            this.tbtnWorkYet.ToggleStateChanged += tbtnTrans_ToggleStateChanged;
        }

        /// <summary>
        /// 페이지 조회 새로고침.
        /// </summary>
        public override void PageSearch()
        {
            try
            {
                base.ShowLoading();

                menuId = "S005";
                menuTitle = "제품별 조회";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = true;
                    mf.radDropDownListBrand.Enabled = true;
                }

                Dictionary<string, object> paramss = new Dictionary<string, object>();
                paramss.Add("@HEADER_PARAMS", "TOGGLE_SEARCH=" + toggleSearch.ToString());

                //데이터 바인딩
                UC01_SMSGridView1.GridViewData.SelectionChanged -= UC01_SMSGridView1_SelectionChanged;
                this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, paramss);

                if (UC01_SMSGridView1.GridViewData.Rows.Count > 0)
                {
                    this.UC01_SMSGridView1.GridViewData.ClearSelection();
                    UC01_SMSGridView1.GridViewData.SelectionChanged += UC01_SMSGridView1_SelectionChanged;
                    this.UC01_SMSGridView1.SelectRow(leftSelectedIndex);
                }

                else
                {
                    //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                    if (!this.UC01_SMSGridView2.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.UC01_SMSGridView2.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                    this.UC01_SMSGridView2.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridView1.GetKeyParam();
                    //데이터 바인딩
                    UC01_SMSGridView2.BindData(ucGrid2SelectSpName, paramss);
                    this.UC01_SMSGridView2.SelectRow(rightSelectedIndex);
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("제품별 관리 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("조회에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        private void UC01_SMSGridView2_TopButton1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            int leftListchkedCnt = 0;
            List<int> leftListchkedIdxs = new List<int>();

            for (int i = 0; i < this.UC01_SMSGridView1.GridViewData.Rows.Count; i++)
            {
                if ((bool)this.UC01_SMSGridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                {
                    leftListchkedIdxs.Add(i);
                    leftListchkedCnt++;
                }
            }

            //leftListchkedIdxs.Add(this.UC01_SMSGridView1.GridViewData.Rows.IndexOf(this.UC01_SMSGridView1.GridViewData.SelectedRows[0]));
            //leftListchkedCnt = 1;

            int rightListchkedCnt = 0;
            List<int> rightListchkedIdxs = new List<int>();

            for (int i = 0; i < this.UC01_SMSGridView2.GridViewData.Rows.Count; i++)
            {
                if ((bool)this.UC01_SMSGridView2.GridViewData.Rows[i].Cells["checkbox"].Value)
                {
                    rightListchkedIdxs.Add(i);
                    rightListchkedCnt++;
                }
            }

            if (leftListchkedCnt == 0 && rightListchkedCnt == 0)
            {
                bowooMessageBox.Show(LanguagePack.Translate("정보 변경할 데이터를 선택하세요."));
                return;
            }

            if (leftListchkedCnt == 0)
            {
                bowooMessageBox.Show(LanguagePack.Translate("정보 변경할 제품을 선택하세요."));
                return;
            }

            if (leftListchkedCnt > 1 && rightListchkedCnt > 0)
            {
                bowooMessageBox.Show(LanguagePack.Translate("2개 이상의 제품 선택 시, 매장은 선택할 수 없습니다."));
                return;
            }

            if (leftListchkedCnt > 1)
            {
                bowooConfirmBox.Show(leftListchkedCnt.ToString() + LanguagePack.Translate("개의 제품이 선택되었습니다.\r\n\r\n정보 변경 진행을 계속 하시겠습니까?"));

                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                {
                    return;
                }
            }

            leftSelectedIndex = this.UC01_SMSGridView1.GridViewData.Rows.IndexOf(this.UC01_SMSGridView1.GridViewData.SelectedRows[0]);
            rightSelectedIndex = this.UC01_SMSGridView2.GridViewData.Rows.IndexOf(this.UC01_SMSGridView2.GridViewData.SelectedRows[0]);
            Sorter.Popup.S005_SearchByProductPopup_ChangeInfo popup = new Sorter.Popup.S005_SearchByProductPopup_ChangeInfo(ref this.UC01_SMSGridView1, ref this.UC01_SMSGridView2, ref leftListchkedIdxs, ref rightListchkedIdxs);
            popup.StartPosition = FormStartPosition.CenterParent;
            popup.ShowDialog();

            if (popup.isModified)
            {
                this.PageSearch();
            }
        }

        void UC01_SMSGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (this.UC01_SMSGridView1.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();
                
                //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                if (!this.UC01_SMSGridView2.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.UC01_SMSGridView2.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                this.UC01_SMSGridView2.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridView1.GetKeyParam();

                //데이터 바인딩
                UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);
                this.UC01_SMSGridView2.SelectRow(rightSelectedIndex);

                //마스터 선택 행과 체크된 행 다를 시, 마스터 행 두개 이상 체크 되었을 시 디테일 체크박스 read only
                int leftListchkedCnt = 0;
                List<int> leftListchkedIdxs = new List<int>();

                for (int i = 0; i < this.UC01_SMSGridView1.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        leftListchkedIdxs.Add(i);
                        leftListchkedCnt++;
                    }
                }

                if (leftListchkedCnt > 1 || (leftListchkedCnt == 1 && leftListchkedIdxs[0] != this.UC01_SMSGridView1.GridViewData.Rows.IndexOf(this.UC01_SMSGridView1.GridViewData.SelectedRows[0])))
                {
                    this.UC01_SMSGridView2.GridViewData.Columns["CheckBox"].ReadOnly = true;
                }
                
                base.HideLoading();
            }
        }

        private void bt_sku_Force_Click(object sender, EventArgs e)
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


            try
            {

                for (int i = 0; i < this.UC01_SMSGridView1.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chuteChkedIdxs.Add(i);
                        chuteChkedCnt++;
                        //rowList.Add(UC01_SMSGridViewRight.GridViewData.Rows[i]);
                    }
                }

                if (chuteChkedIdxs.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("강제확정할 데이터를 선택해 주세요."));
                    return;
                }



                if (bowooConfirmBox.DialogResult != DialogResult.OK)
                {
                    bowooConfirmBox.Show(LanguagePack.Translate("강제확정을 진행 하시겠습니까?"));
                }

                if (bowooConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {

                    if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("R_E1"))
                        this.UC01_SMSGridView1.Add_Data_Parameters.Add("R_E1", "");
                    this.UC01_SMSGridView1.Add_Data_Parameters["R_E1"] = "";

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



                    for (int i = 0; i < chuteChkedIdxs.Count; i++)
                    {

                        //SP실행
                        this.UC01_SMSGridView1.ExcuteSaveSp("USP_S004_01_L_SKU_FORCE", chuteChkedIdxs);


                        r_ok = this.UC01_SMSGridView1.Usp_Save_Parameters[2].Value.ToString();
                        r_msg += this.UC01_SMSGridView1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                        if (r_ok != "OK")
                        {
                            makeLog("tilt실패 강제확정", true, "tilt강제확정 실패");
                            //설정 완료 메시지 창
                            bowooMessageBox.Show(LanguagePack.Translate("강제확정을 실패했습니다."));
                            return;
                        }

                    }


                    makeLog("tilt실패 강제확정", true, "tilt강제확정 성골");
                    //설정 완료 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("강제확정를 완료했습니다."));
                    this.PageSearch();

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
    }
}