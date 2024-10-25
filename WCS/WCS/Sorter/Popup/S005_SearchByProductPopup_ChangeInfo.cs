using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Sorter.Popup
{
    public partial class S005_SearchByProductPopup_ChangeInfo : lib.Common.Management.BaseForm
    {
        public bool isModified = false;
       
        lib.Common.Management.UC01_GridView grid1;
        lib.Common.Management.UC01_GridView grid2;
        List<int> grid1ChkList;
        List<int> grid2ChkList;

        string missingBtnSpName = "USP_S005_02_B_POP_MISSING_PRODUCT";
        string forceBtnSpName = "USP_S005_02_B_POP_FORCE_CONFIRM";
        string initBtnSpname = "USP_S005_02_B_POP_INITIALIZATION";

        string selectedShopNM = "";
        string selectedItemCd = ""; 
        string selectedItemColor = "";
        string selectedItemSize = "";

        int shopChkCnt = 0;
        int prodChkCnt = 0;
        
        public S005_SearchByProductPopup_ChangeInfo()
        {
            InitializeComponent();
            this.CenterToParent();
        }

        public S005_SearchByProductPopup_ChangeInfo(ref lib.Common.Management.UC01_GridView _grid1, ref lib.Common.Management.UC01_GridView _grid2, ref List<int> _grid1ChkList, ref List<int> _grid2ChkList)
        {
            InitializeComponent();
            this.CenterToParent();

            grid1 = _grid1;
            grid2 = _grid2;

            grid1ChkList = _grid1ChkList;
            grid2ChkList = _grid2ChkList;

            this.Load += S005_SearchByProductPopup_ChangeInfo_Load;
        }

        //팝업 로드시 메서드.
        void S005_SearchByProductPopup_ChangeInfo_Load(object sender, EventArgs e)
        {
            menuId = "S005";
            menuTitle = "제품별 조회 - 정보 변경 팝업";

            radTitleBarPopup.Text = LanguagePack.Translate(radTitleBarPopup.Text);

            radLabel2.Text = LanguagePack.Translate(radLabel2.Text);
            radLabel26.Text = LanguagePack.Translate(radLabel26.Text);
            radLabel3.Text = LanguagePack.Translate(radLabel3.Text);
            radLabel27.Text = LanguagePack.Translate(radLabel27.Text);
            radLabel5.Text = LanguagePack.Translate(radLabel5.Text);
            radLabel30.Text = LanguagePack.Translate(radLabel30.Text);

            bool shopLblIsVisible = true;
            bool ItemLblIsVisible = true;

            prodChkCnt = grid1ChkList.Count;
            shopChkCnt = grid2ChkList.Count;

            //제품 선택 시
            if (prodChkCnt == 1)
            {
                selectedItemCd = grid1.GridViewData.Rows[grid1ChkList[0]].Cells["ITEM_STYLE"].Value.ToString();
                selectedItemColor = grid1.GridViewData.Rows[grid1ChkList[0]].Cells["ITEM_COLOR"].Value.ToString();
                selectedItemSize = grid1.GridViewData.Rows[grid1ChkList[0]].Cells["ITEM_SIZE"].Value.ToString();
            }

            else if (prodChkCnt == 0)
            {
                ItemLblIsVisible = false;
            }

            else
            {
                selectedItemCd = prodChkCnt.ToString() + LanguagePack.Translate(" 개의");
                selectedItemColor = "";
                selectedItemSize = "";
            }

            //매장 선택시
            if (shopChkCnt == 1)
            {
                selectedShopNM = grid2.GridViewData.Rows[grid2ChkList[0]].Cells["SHOP_NM"].Value.ToString();
            }

            else if (shopChkCnt == 0)
            {
                shopLblIsVisible = false;
            }

            else
            {
                selectedShopNM = shopChkCnt.ToString() + LanguagePack.Translate(" 개의");
            }

            SetSelectedItemDisplayLabel(shopLblIsVisible, ItemLblIsVisible);
            SetQtyTextAndSetEnableButton();

            //이벤트
            this.radTxtMissing.TextChanged += radTxt_TextChanged;
            this.radTxtForce.TextChanged += radTxt_TextChanged;

            this.radBtnMissing.Click += radBtnMissing_Click;
            this.radBtnForce.Click += radBtnForce_Click;
            this.radBtnInit.Click += radBtnInit_Click;
        }

        private void SetQtyTextAndSetEnableButton()
        {
            //결품 가능 수량 계산
            string missingPosibleQty = "0";

            //매장 >= 1 선택 시
            if (prodChkCnt >= 1 && shopChkCnt == 0)
            {
                int sum_missing = 0;

                foreach (int idx in grid1ChkList)
                {
                    sum_missing += (int)grid1.ds_Gird.Tables[1].Rows[idx]["ORDQTY2"];
                }

                missingPosibleQty = sum_missing.ToString();

                this.radLblMissing.Text = missingPosibleQty;
                this.radTxtMissing.Text = missingPosibleQty;
                this.radTxtMissing.Enabled = false;

                this.radLblForce.Text = missingPosibleQty;
                this.radTxtForce.Text = missingPosibleQty;
                this.radTxtForce.Enabled = false;
            }

            //제품 1 매장 1 선택 시
            if (shopChkCnt == 1 && prodChkCnt == 1)
            {
                missingPosibleQty = grid2.ds_Gird.Tables[1].Rows[grid2ChkList[0]]["ORDQTY2"].ToString();

                this.radLblMissing.Text = missingPosibleQty;
                this.radTxtMissing.Text = missingPosibleQty;
                this.radTxtMissing.Enabled = true;

                this.radLblForce.Text = missingPosibleQty;
                this.radTxtForce.Text = missingPosibleQty;
                this.radTxtForce.Enabled = true;
            }

            //제품 1  매장>1 선택 시
            if (prodChkCnt == 1 && shopChkCnt > 1)
            {
                int sum_missing = 0;

                foreach (int idx in grid2ChkList)
                {
                    sum_missing += (int)grid2.ds_Gird.Tables[1].Rows[idx]["ORDQTY2"];
                }

                missingPosibleQty = sum_missing.ToString();

                this.radLblMissing.Text = missingPosibleQty;
                this.radTxtMissing.Text = missingPosibleQty;
                this.radTxtMissing.Enabled = false;

                this.radLblForce.Text = missingPosibleQty;
                this.radTxtForce.Text = missingPosibleQty;
                this.radTxtForce.Enabled = false;
            }

            //결품가능 수량 "0" 일 시 버튼 비활성화
            if (missingPosibleQty == "0")
            {
                radBtnMissing.Enabled = false;
                radBtnForce.Enabled = false;
                //radBtnInit.Enabled = false;
            }

            else
            {
                radBtnMissing.Enabled = true;
                radBtnForce.Enabled = true;
                //radBtnInit.Enabled = true;
            }
        }

        //수량 validation
        void radTxt_TextChanged(object sender, EventArgs e)
        {
            RadTextBox textbox = sender as RadTextBox;

            if (textbox == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(textbox.Text))
            {
                return;
            }

            //수량 Validation
            if (!lib.Common.ValidationEx.IsNumber(textbox.Text))
            {
                bowooMessageBox.Show(LanguagePack.Translate("숫자를 입력하세요."));
                textbox.Text = "";
                return;
            }

            if (int.Parse(textbox.Text) < 0)
            {
                bowooMessageBox.Show(LanguagePack.Translate("0 이상의 숫자를 입력하세요."));
                textbox.Text = "";
                return;
            }
        }

        private void SetSelectedItemDisplayLabel(bool _shopVisible, bool _itemVisible)
        {
            if (_itemVisible)
            {
                this.radLblSelectedItem.Text = string.Format(LanguagePack.Translate("{0} {1} {2} 제품이 선택되었습니다."), selectedItemCd, selectedItemColor, selectedItemSize);
            }

            else
            {
                this.radLblSelectedItem.Visible = false;
            }

            if (_shopVisible)
            {
                this.radLblSelectedShop.Text = string.Format(LanguagePack.Translate("{0} 매장이 선택되었습니다."), selectedShopNM);
            }

            else
            {
                this.radLblSelectedShop.Visible = false;
            }
        }

        //결품 버튼
        /// <summary>
        /// 결품 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnMissing_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            try
            {
                //숫자 유효성
                if (int.Parse(radTxtMissing.Text) > int.Parse(radLblMissing.Text))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("전체 수량보다 적은 숫자를 입력하세요."));
                    radTxtMissing.Text = radLblMissing.Text;
                    return;
                }

                if (int.Parse(radTxtMissing.Text) <= 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("1 이상의 숫자를 입력하세요."));
                    radTxtMissing.Text = radLblMissing.Text;
                    return;
                }

                if (string.IsNullOrEmpty(radTxtMissing.Text))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("수량을 입력하세요."));
                    return;
                }

                bowooRedConfirmBox.Show(LanguagePack.Translate("결품 처리 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    makeLog("결품", true, "결품 진행 수락");

                    //결품 수량 PARAM 추가
                    if (!this.grid1.Add_Data_Parameters.ContainsKey("ISALL"))
                        this.grid1.Add_Data_Parameters.Add("ISALL", "");

                    if (!this.grid1.Add_Data_Parameters.ContainsKey("ISPROC"))
                        this.grid1.Add_Data_Parameters.Add("ISPROC", "");
                    this.grid1.Add_Data_Parameters["ISPROC"] = "N";

                    if (!this.grid2.Add_Data_Parameters.ContainsKey("MISSINGQTY"))
                        this.grid2.Add_Data_Parameters.Add("MISSINGQTY", "");
                    this.grid2.Add_Data_Parameters["MISSINGQTY"] = this.radTxtMissing.Text.ToString();

                    //결품 수량 사용 여부(매장만 선택 시 OR 다수 제품 선택 시 전체 결품 처리)
                    if (!this.grid2.Add_Data_Parameters.ContainsKey("ISALL"))
                        this.grid2.Add_Data_Parameters.Add("ISALL", "");

                    if (!this.grid2.Add_Data_Parameters.ContainsKey("ISPROC"))
                        this.grid2.Add_Data_Parameters.Add("ISPROC", "");
                    this.grid2.Add_Data_Parameters["ISPROC"] = "N";

                    string r_ok = "";
                    string r_msg = "";

                    do
                    {
                        //제품만 선택 되어 있을 경우.
                        if (prodChkCnt >= 1 && shopChkCnt == 0)
                        {
                            this.grid1.Add_Data_Parameters["ISALL"] = "Y"; //MISSINGQTY사용안함

                            for (int i = 0; i < grid1ChkList.Count; i++)
                            {
                                //SP실행
                                this.grid1.ExcuteSaveSp(missingBtnSpName, grid1ChkList[i]);

                                r_ok = this.grid1.Usp_Save_Parameters[2].Value.ToString();
                                r_msg += this.grid1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                                if (r_ok != "OK")
                                {
                                    break;
                                }
                            }
                        }

                        //제품 1 매장 1> 선택
                        if (prodChkCnt == 1 && shopChkCnt > 1)
                        {
                            this.grid2.Add_Data_Parameters["ISALL"] = "Y"; //MISSINGQTY사용안함

                            for (int i = 0; i < grid2ChkList.Count; i++)
                            {
                                //SP실행
                                this.grid2.ExcuteSaveSp(missingBtnSpName, grid2ChkList[i]);

                                r_ok = this.grid2.Usp_Save_Parameters[2].Value.ToString();
                                r_msg += this.grid2.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                                if (r_ok != "OK")
                                {
                                    break;
                                }
                            }
                        }

                        //매장 1 제품 1 선택
                        if (prodChkCnt == 1 && shopChkCnt == 1)
                        {
                            this.grid2.Add_Data_Parameters["ISALL"] = "N"; //MISSINGQTY 사용하여 결품 처리

                            for (int i = 0; i < grid2ChkList.Count; i++)
                            {
                                //SP실행
                                this.grid2.ExcuteSaveSp(missingBtnSpName, grid2ChkList[i]);

                                r_ok = this.grid2.Usp_Save_Parameters[2].Value.ToString();
                                r_msg += this.grid2.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                                if (r_ok != "OK")
                                {
                                    break;
                                }
                            }
                        }

                        ////분류 시작 제품 존재 메시지
                        //if (r_ok != "OK")
                        //{
                        //    bowooRedConfirmBox.Show(r_msg);
                        //    if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                        //    {
                        //        makeLog("결품", true, "작업 예정 외의 데이터에 대한 결품 수락");

                        //        this.grid1.Add_Data_Parameters["ISPROC"] = "Y";
                        //        this.grid2.Add_Data_Parameters["ISPROC"] = "Y";
                        //    }
                        //    else
                        //        return;

                        //}
                    } while (r_ok != "OK");

                    if (r_ok == "OK")
                    {
                        //설정 완료 메시지 창
                        makeLog("결품 처리", true, "결품 처리 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("결품 처리가 완료되었습니다."));

                        //if (prodChkCnt >= 1 && shopChkCnt == 0)
                        //{
                        //    grid1.RefreshData();
                        //}
                        //else
                        //{
                        //    grid2.RefreshData();
                        //}

                        //SetQtyTextAndSetEnableButton();
                    }

                    else
                    {
                        makeLog("결품 처리", false, "결품 처리 실패, " + r_msg);
                        bowooMessageBox.Show(LanguagePack.Translate("실패한 처리가 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                    }

                    isModified = true;
                    this.Close();
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("결품 처리", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("결품 처리에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //강제확정 버튼
        /// <summary>
        /// 강제확정 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnForce_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            try
            {
                //숫자 유효성
                if (int.Parse(radTxtForce.Text) > int.Parse(radLblForce.Text))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("전체 수량보다 적은 숫자를 입력하세요."));
                    radTxtForce.Text = radLblForce.Text;
                    return;
                }

                if (int.Parse(radTxtForce.Text) <= 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("1 이상의 숫자를 입력하세요."));
                    radTxtForce.Text = radLblForce.Text;
                    return;
                }

                if (string.IsNullOrEmpty(radTxtForce.Text))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("수량을 입력하세요."));
                    return;
                }

                bowooRedConfirmBox.Show(LanguagePack.Translate("강제 확정 처리 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    makeLog("강제 확정", true, "강제 확정 진행 수락");

                    //결품 수량 PARAM 추가
                    if (!this.grid1.Add_Data_Parameters.ContainsKey("ISALL"))
                        this.grid1.Add_Data_Parameters.Add("ISALL", "");

                    if (!this.grid1.Add_Data_Parameters.ContainsKey("ISPROC"))
                        this.grid1.Add_Data_Parameters.Add("ISPROC", "");
                    this.grid1.Add_Data_Parameters["ISPROC"] = "N";

                    if (!this.grid2.Add_Data_Parameters.ContainsKey("FORCEQTY"))
                        this.grid2.Add_Data_Parameters.Add("FORCEQTY", "");
                    this.grid2.Add_Data_Parameters["FORCEQTY"] = this.radTxtForce.Text.ToString();

                    //결품 수량 사용 여부(매장만 선택 시 OR 다수 제품 선택 시 전체 결품 처리)
                    if (!this.grid2.Add_Data_Parameters.ContainsKey("ISALL"))
                        this.grid2.Add_Data_Parameters.Add("ISALL", "");

                    if (!this.grid2.Add_Data_Parameters.ContainsKey("ISPROC"))
                        this.grid2.Add_Data_Parameters.Add("ISPROC", "");
                    this.grid2.Add_Data_Parameters["ISPROC"] = "";

                    string r_ok = "";
                    string r_msg = "";

                    do
                    {
                        //제품만 선택 되어 있을 경우.
                        if (prodChkCnt >= 1 && shopChkCnt == 0)
                        {
                            this.grid1.Add_Data_Parameters["ISALL"] = "Y"; //MISSINGQTY사용안함

                            for (int i = 0; i < grid1ChkList.Count; i++)
                            {
                                //SP실행
                                this.grid1.ExcuteSaveSp(forceBtnSpName, grid1ChkList[i]);

                                r_ok = this.grid1.Usp_Save_Parameters[2].Value.ToString();
                                r_msg += this.grid1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                                if (r_ok != "OK")
                                {
                                    break;
                                }
                            }
                        }

                        //제품 1 매장 1> 선택
                        if (prodChkCnt == 1 && shopChkCnt > 1)
                        {
                            this.grid2.Add_Data_Parameters["ISALL"] = "Y"; //FORCEQTY 사용안함

                            for (int i = 0; i < grid2ChkList.Count; i++)
                            {
                                //SP실행
                                this.grid2.ExcuteSaveSp(forceBtnSpName, grid2ChkList[i]);

                                r_ok = this.grid2.Usp_Save_Parameters[2].Value.ToString();
                                r_msg += this.grid2.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                                if (r_ok != "OK")
                                {
                                    break;
                                }
                            }
                        }

                        //매장 1 제품 1 선택
                        if (prodChkCnt == 1 && shopChkCnt == 1)
                        {
                            this.grid2.Add_Data_Parameters["ISALL"] = "N"; //FORCEQTY 사용하여 결품 처리

                            for (int i = 0; i < grid2ChkList.Count; i++)
                            {
                                //SP실행
                                this.grid2.ExcuteSaveSp(forceBtnSpName, grid2ChkList[i]);

                                r_ok = this.grid2.Usp_Save_Parameters[2].Value.ToString();
                                r_msg += this.grid2.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                                if (r_ok != "OK")
                                {
                                    break;
                                }
                            }
                        }

                        //분류 시작 제품 존재 메시지
                        if (r_ok != "OK")
                        {
                            bowooRedConfirmBox.Show(LanguagePack.Translate(r_msg));

                            if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                makeLog("강제 확정", true, "작업 예정 외의 데이터에 대한 강제 확정 수락");

                                this.grid1.Add_Data_Parameters["ISPROC"] = "Y";
                                this.grid2.Add_Data_Parameters["ISPROC"] = "Y";
                            }

                            else
                            {
                                return;
                            }
                        }
                        //************************
                    } while (r_ok != "OK");

                    if (r_ok == "OK")
                    {
                        //설정 완료 메시지 창
                        makeLog("강제 확정", true, "강제 확정 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("강제 확정 처리가 완료되었습니다."));

                        //if (prodChkCnt >= 1 && shopChkCnt == 0)
                        //{
                        //    grid1.RefreshData();
                        //}
                        //else
                        //{
                        //    grid2.RefreshData();
                        //}

                        //SetQtyTextAndSetEnableButton();
                    }

                    else
                    {
                        makeLog("강제 확정", false, "강제 확정 실패, " + r_msg);
                        bowooMessageBox.Show(LanguagePack.Translate("실패한 처리가 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                    }

                    isModified = true;
                    this.Close();
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("강제 확정", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("강제 확정 처리에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //초기화 버튼
        /// <summary>
        /// 초기화 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnInit_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            try
            {
                bowooRedConfirmBox.Show(LanguagePack.Translate("선택하신 데이터가 작업하기 전 상태로 돌아갑니다.\r\n초기화된 데이터는 복구할 수 없습니다.\r\n\r\n초기화 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    makeLog("초기화", true, "초기화 진행 수락");

                    if (!this.grid1.Add_Data_Parameters.ContainsKey("DELETE_E0"))
                        this.grid1.Add_Data_Parameters.Add("DELETE_E0", "");
                    this.grid1.Add_Data_Parameters["DELETE_E0"] = "N";
                    if (!this.grid1.Add_Data_Parameters.ContainsKey("DELETE_E1"))
                        this.grid1.Add_Data_Parameters.Add("DELETE_E1", "");
                    this.grid1.Add_Data_Parameters["DELETE_E1"] = "N";
                    if (!this.grid1.Add_Data_Parameters.ContainsKey("DELETE_E2"))
                        this.grid1.Add_Data_Parameters.Add("DELETE_E2", "");
                    this.grid1.Add_Data_Parameters["DELETE_E2"] = "N";
                    if (!this.grid1.Add_Data_Parameters.ContainsKey("DELETE_E3"))
                        this.grid1.Add_Data_Parameters.Add("DELETE_E3", "");
                    this.grid1.Add_Data_Parameters["DELETE_E3"] = "N";
                    if (!this.grid1.Add_Data_Parameters.ContainsKey("DELETE_E9"))
                        this.grid1.Add_Data_Parameters.Add("DELETE_E9", "");
                    this.grid1.Add_Data_Parameters["DELETE_E9"] = "N";

                    if (!this.grid2.Add_Data_Parameters.ContainsKey("DELETE_E0"))
                        this.grid2.Add_Data_Parameters.Add("DELETE_E0", "");
                    this.grid2.Add_Data_Parameters["DELETE_E0"] = "N";
                    if (!this.grid2.Add_Data_Parameters.ContainsKey("DELETE_E1"))
                        this.grid2.Add_Data_Parameters.Add("DELETE_E1", "");
                    this.grid2.Add_Data_Parameters["DELETE_E1"] = "N";
                    if (!this.grid2.Add_Data_Parameters.ContainsKey("DELETE_E2"))
                        this.grid2.Add_Data_Parameters.Add("DELETE_E2", "");
                    this.grid2.Add_Data_Parameters["DELETE_E2"] = "N";
                    if (!this.grid2.Add_Data_Parameters.ContainsKey("DELETE_E3"))
                        this.grid2.Add_Data_Parameters.Add("DELETE_E3", "");
                    this.grid2.Add_Data_Parameters["DELETE_E3"] = "N";
                    if (!this.grid2.Add_Data_Parameters.ContainsKey("DELETE_E9"))
                        this.grid2.Add_Data_Parameters.Add("DELETE_E9", "");
                    this.grid2.Add_Data_Parameters["DELETE_E9"] = "N";

                    string r_ok = "";
                    string r_msg = "";
                    int rcv_value = 0;

                    do
                    {
                        //제품만 선택 되어 있을 경우.
                        if (prodChkCnt >= 1 && shopChkCnt == 0)
                        {
                            for (int i = 0; i < grid1ChkList.Count; i++)
                            {
                                //SP실행
                                this.grid1.ExcuteSaveSp(initBtnSpname, grid1ChkList[i]);

                                r_ok = this.grid1.Usp_Save_Parameters[2].Value.ToString();
                                r_msg += this.grid1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                                if (r_ok != "OK")
                                {
                                    break;
                                }

                                else
                                {
                                    rcv_value += int.Parse(this.grid1.Usp_Save_Parameters[3].Value.ToString());
                                }
                            }
                        }

                        //제품 1 매장 1>= 선택
                        if (prodChkCnt == 1 && shopChkCnt >= 1)
                        {
                            for (int i = 0; i < grid2ChkList.Count; i++)
                            {
                                //SP실행
                                this.grid2.ExcuteSaveSp(initBtnSpname, grid2ChkList[i]);

                                r_ok = this.grid2.Usp_Save_Parameters[2].Value.ToString();
                                r_msg += this.grid2.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                                if (r_ok != "OK")
                                {
                                    break;
                                }

                                else
                                {
                                    rcv_value += int.Parse(this.grid2.Usp_Save_Parameters[3].Value.ToString());
                                }
                            }
                        }
                        //**********************
                        //분류 작업중, 완료, 박스 마감, 결과 전송 제품 존재시
                        //if (r_ok == "E0") //작업중 
                        //{
                        //    bowooMessageBox.Show(r_msg);
                        //    return;
                        //}
                        if (r_ok != "OK")
                        {
                            if (r_ok == "NG" || r_ok == "E0" || r_ok == "E3")
                            {
                                bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                                return;
                            }

                            bowooRedConfirmBox.Show(LanguagePack.Translate(r_msg));

                            if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                            {
                                if (r_ok == "E0")
                                {
                                    makeLog("초기화", true, "작업 중인 데이터에 대한 초기화 수락");

                                    this.grid1.Add_Data_Parameters["DELETE_E0"] = "Y";
                                    this.grid2.Add_Data_Parameters["DELETE_E0"] = "Y";
                                }

                                else if (r_ok == "E1")
                                {
                                    makeLog("초기화", true, "분류 완료된 데이터에 대한 초기화 수락");

                                    this.grid1.Add_Data_Parameters["DELETE_E1"] = "Y";
                                    this.grid2.Add_Data_Parameters["DELETE_E1"] = "Y";
                                }

                                else if (r_ok == "E2")
                                {
                                    makeLog("초기화", true, "박스 마감된 데이터에 대한 초기화 수락");

                                    this.grid1.Add_Data_Parameters["DELETE_E2"] = "Y";
                                    this.grid2.Add_Data_Parameters["DELETE_E2"] = "Y";
                                }

                                else if (r_ok == "E3")
                                {
                                    makeLog("초기화", true, "결과 전송된 데이터에 대한 초기화 수락");

                                    this.grid1.Add_Data_Parameters["DELETE_E3"] = "Y";
                                    this.grid2.Add_Data_Parameters["DELETE_E3"] = "Y";
                                }

                                else if (r_ok == "E9")
                                {
                                    makeLog("초기화", true, "미 설정 슈트 데이터에 대한 초기화 수락");

                                    this.grid1.Add_Data_Parameters["DELETE_E9"] = "Y";
                                    this.grid2.Add_Data_Parameters["DELETE_E9"] = "Y";
                                }
                            }

                            else
                            {
                                return;
                            }
                        }
                        //************************
                    } while (r_ok != "OK");

                    if (r_ok == "OK")
                    {
                        //설정 완료 메시지 창
                        makeLog("초기화", true, "초기화 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("초기화가 완료되었습니다."));

                        //if (prodChkCnt >= 1 && shopChkCnt == 0)
                        //{
                        //    grid1.RefreshData();
                        //}
                        //else
                        //{
                        //    grid2.RefreshData();
                        //}

                        //SetQtyTextAndSetEnableButton();
                    }

                    else
                    {
                        makeLog("초기화", false, "초기화 실패, " + r_msg);
                        bowooMessageBox.Show(LanguagePack.Translate("실패한 처리가 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                    }

                    isModified = true;
                    this.Close();
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("초기화", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("초기화에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }
    }
}