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
    public partial class S010_HistoryPopup_ChangeInfo : lib.Common.Management.BaseForm
    {
        public bool isModified = false;

        public S010_HistoryPopup_ChangeInfo()
        {
            InitializeComponent();
            this.CenterToParent();
        }

        lib.Common.Management.UC01_GridView grid1;
        List<int> grid1ChkList;

        string initBtnSpname = "USP_S010_B_POP_INITIALIZATION";

        string seletedBarcode = "";

        int prodChkCnt = 0;

        public S010_HistoryPopup_ChangeInfo(ref lib.Common.Management.UC01_GridView _grid1, ref List<int> _grid1ChkList)
        {
            InitializeComponent();
            this.CenterToParent();

            grid1 = _grid1;
            grid1ChkList = _grid1ChkList;
        }

        //팝업 로드시 메서드.
        private void S010_HistoryPopup_ChangeInfo_Load(object sender, EventArgs e)
        {
            menuId = "S004";
            menuTitle = "매장별 조회 - 정보 변경 팝업";

            radTitleBarPopup.Text = LanguagePack.Translate(radTitleBarPopup.Text);

            radBtnInit.Text = LanguagePack.Translate(radBtnInit.Text);
            radLabel30.Text = LanguagePack.Translate(radLabel30.Text);

            bool ItemLblIsVisible = true;

            prodChkCnt = grid1ChkList.Count;

            //제품 선택 시
            if (prodChkCnt == 1)
            {
                seletedBarcode = grid1.GridViewData.Rows[grid1ChkList[0]].Cells["barcode"].Value.ToString();
            }
            else if (prodChkCnt == 0)
            {
                ItemLblIsVisible = false;
            }
            else
            {
                seletedBarcode = prodChkCnt.ToString() + LanguagePack.Translate(" 개의");
            }

            SetSelectedItemDisplayLabel(ItemLblIsVisible);
            SetQtyTextAndSetEnableButton();

            this.radTxtInit.TextChanged += radTxt_TextChanged;
        }

        //수량 계산 및 버튼 활성화 여부 설정
        private void SetQtyTextAndSetEnableButton()
        {
            //초기화 가능 수량 계산
            string missingPosibleQty = "0";

            //제품 1개 선택
            if (prodChkCnt > 1)
            {
                int sum_missing = 0;

                foreach (int idx in grid1ChkList)
                {
                    sum_missing += (int)grid1.ds_Gird.Tables[1].Rows[idx]["wrk_qty"];
                }

                missingPosibleQty = sum_missing.ToString();

                this.radLblInit.Text = missingPosibleQty;
                this.radTxtInit.Text = missingPosibleQty;
                this.radTxtInit.Enabled = false;
            }

            //제품 1개이상 선택
            if (prodChkCnt == 1)
            {
                missingPosibleQty = grid1.ds_Gird.Tables[1].Rows[grid1ChkList[0]]["wrk_qty"].ToString();

                this.radLblInit.Text = missingPosibleQty;
                this.radTxtInit.Text = missingPosibleQty;
                this.radTxtInit.Enabled = true;
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

        private void SetSelectedItemDisplayLabel(bool _itemVisible)
        {

            if (_itemVisible)
            {
                this.radLblSelectedItem.Text = string.Format(LanguagePack.Translate("{0} 제품이 선택되었습니다."), seletedBarcode);
            }

            else
            {
                this.radLblSelectedItem.Visible = false;
            }
        }

        //초기화 버튼
        /// <summary>
        /// 초기화 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBtnInit_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            try
            {
                //숫자 유효성
                if (int.Parse(radTxtInit.Text) > int.Parse(radLblInit.Text))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("전체 수량보다 적은 숫자를 입력하세요."));
                    radTxtInit.Text = radLblInit.Text;
                    return;
                }

                if (int.Parse(radTxtInit.Text) <= 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("1 이상의 숫자를 입력하세요."));
                    radTxtInit.Text = radLblInit.Text;
                    return;
                }

                if (string.IsNullOrEmpty(radTxtInit.Text))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("수량을 입력하세요."));
                    return;
                }

                bowooRedConfirmBox.Show(LanguagePack.Translate("초기화 처리 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    makeLog("초기화", true, "초기화 처리 진행 수락");



                    //최기화할 수량 
                    if (!this.grid1.Add_Data_Parameters.ContainsKey("INITQTY"))
                        this.grid1.Add_Data_Parameters.Add("INITQTY", "");
                    this.grid1.Add_Data_Parameters["INITQTY"] = this.radTxtInit.Text.ToString();

                    //초기화할 수량 컬럼을 사용 할지 말지 결정하는 컬럼.
                    if (!this.grid1.Add_Data_Parameters.ContainsKey("ISALL"))
                        this.grid1.Add_Data_Parameters.Add("ISALL", "");

                    string r_ok = "";
                    string r_msg = "";

                    do
                    {
                        if(prodChkCnt > 1) //제품을 여러개 선택 했을때
                        {
                            this.grid1.Add_Data_Parameters["ISALL"] = "N"; //INITQTY사용안함
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
                            }
                        }
                        else if (prodChkCnt == 1)// 제품을 한개만 선택했을때
                        {
                            this.grid1.Add_Data_Parameters["ISALL"] = "Y"; //INITQTY사용함
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
                            }
                        }


                    } while (r_ok != "OK");

                    if (r_ok == "OK")
                    {
                        //설정 완료 메시지 창
                        makeLog("초기화 처리", true, "초기화 처리 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("결품 처리가 완료되었습니다."));
                    }

                    else
                    {
                        makeLog("초기화 처리", false, "초기화 처리 실패, " + r_msg);
                        bowooMessageBox.Show(LanguagePack.Translate("초기화 처리가 존재합니다.\r\n") + LanguagePack.Translate(r_msg));
                    }
                    isModified = true;
                    this.Close();
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("초기화 처리", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("초기화 처리에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }


    }
}