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
    public partial class S001_ReceiveData : lib.Common.Management.BaseControl
    {
        //System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ManageLocation));
        //SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S001_01_L_PLAN_LIST]";
        string ucGrid1DataReceiveSpName = "[USP_S001_01_B_DATA_PROC]";
        string ucGrid2SelectSpName = "[USP_S001_02_L_COMPLETE_LIST]";
        string ucGrid3SelectSpName = "[USP_S001_03_L_COMPLETE_LIST_DETAIL]";

        public S001_ReceiveData()
        {
            InitializeComponent();
            
            RadPanelContents.Visible = true;

            //01. top left grid 설정

            //title
            UC01_SMSGridView1.GridTitleText = "수신 예정 리스트";

            //버튼 설정
            UC01_SMSGridView1.Button1_Visible = true;
            UC01_SMSGridView1.Button1_Text = "수신";
            UC01_SMSGridView1.Button1_Click = UC01_SMSGridView1_TopButton1_Click;

            //조회 조건 숨김
            this.UC01_SMSGridView1.HideSearchCondition();

            //02. top right grid 설정

            //title
            UC01_SMSGridView2.GridTitleText = "수신 완료 리스트";

            //버튼 설정
            UC01_SMSGridView2.Button1_Visible = true;
            UC01_SMSGridView2.Button1_Text = "새로 고침";
            UC01_SMSGridView2.Button1_Click = UC01_SMSGridView2_TopButton1_Click;

            //조회 조건 숨김
            this.UC01_SMSGridView2.HideSearchCondition();

            UC01_SMSGridView3.GridTitleText = "수신 데이터 상세 리스트";

            //조회 조건 숨김
            this.UC01_SMSGridView3.HideSearchCondition();
        }

        /// <summary>
        /// 페이지 조회, 새로고침.
        /// </summary>
        public override void PageSearch()
        {
            try
            {
                base.ShowLoading();

                menuId = "S001";
                menuTitle = "데이터 수신";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = false;
                    mf.radDropDownListBrand.Enabled = true;
                }

                //01 데이터 바인딩
                UC01_SMSGridView1.BindData(ucGrid1SelectSpName, null);

                //02 데이터 바인딩
                UC01_SMSGridView2.GridViewData.SelectionChanged -= UC01_SMSGridView2_SelectionChanged;
                UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);

                if (this.UC01_SMSGridView2.GridViewData.Rows.Count > 0)
                {
                    UC01_SMSGridView2.GridViewData.Rows[0].IsSelected = false;
                    UC01_SMSGridView2.GridViewData.SelectionChanged += UC01_SMSGridView2_SelectionChanged;
                    UC01_SMSGridView2.GridViewData.Rows[0].IsSelected = true;
                }

                else
                {
                    //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                    if (!this.UC01_SMSGridView3.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.UC01_SMSGridView3.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                    this.UC01_SMSGridView3.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridView2.GetKeyParam();
                    //03 데이터 바인딩
                    UC01_SMSGridView3.BindData(ucGrid3SelectSpName, null);
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("데이터 수신 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("조회에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        void UC01_SMSGridView2_SelectionChanged(object sender, EventArgs e)
        {
            if (this.UC01_SMSGridView2.GridViewData.SelectedRows.Count > 0)
            {
                base.ShowLoading();

                //Key Parameter 생성하여 다음 그리드 파라미터에 저장
                if (!this.UC01_SMSGridView3.Usp_Load_Parameters.ContainsKey("@KEY_PARAMS")) this.UC01_SMSGridView3.Usp_Load_Parameters.Add("@KEY_PARAMS", "");
                this.UC01_SMSGridView3.Usp_Load_Parameters["@KEY_PARAMS"] = this.UC01_SMSGridView2.GetKeyParam();
                               
                //데이터 바인딩
                UC01_SMSGridView3.BindData(ucGrid3SelectSpName, null);

                base.HideLoading();           
            }           
        }

        //수신 버튼
        private void UC01_SMSGridView1_TopButton1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            int chkedCnt = 0;
            List<int> chkedIdxs = new List<int>();

            base.ShowLoading();

            try
            {   
                for (int i = 0; i < this.UC01_SMSGridView1.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("수신할 데이터를 선택하세요."));
                    return;
                }

                //삭제 확인 메시지 창
                bowooConfirmBox.Show(LanguagePack.Translate("데이터 수신 진행을 계속 하시겠습니까?"));

                if (bowooConfirmBox.DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                makeLog("수신", true, "수신 진행 수락");

                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("R_E1"))
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("R_E1", "");
                this.UC01_SMSGridView1.Add_Data_Parameters["R_E1"] = "N";

                string r_ok = "";
                string r_msg = "";

                if (!this.UC01_SMSGridView1.Add_Data_Parameters.ContainsKey("batch"))
                {
                    this.UC01_SMSGridView1.Add_Data_Parameters.Add("batch", "");
                }
                else
                {
                    this.UC01_SMSGridView1.Add_Data_Parameters["batch"] = "";
                }

                for (int i = 0; i < chkedIdxs.Count; i++ )
                {
                    if(i == 0)
                    {
                        this.UC01_SMSGridView1.Add_Data_Parameters["batch"] += this.UC01_SMSGridView1.GridViewData.Rows[i].Cells["WRKSEQ"].Value.ToString();
                    }
                    else
                    {
                        this.UC01_SMSGridView1.Add_Data_Parameters["batch"] += "," + this.UC01_SMSGridView1.GridViewData.Rows[i].Cells["WRKSEQ"].Value.ToString();
                    }
                }

                this.UC01_SMSGridView1.ExcuteSaveSp(ucGrid1DataReceiveSpName, chkedIdxs[0]);

                r_ok = this.UC01_SMSGridView1.Usp_Save_Parameters[2].Value.ToString();
                r_msg += this.UC01_SMSGridView1.Usp_Save_Parameters[3].Value.ToString() + "\r\n";

                if (r_ok != "OK")
                {
                    bowooMessageBox.Show(LanguagePack.Translate(r_msg));
                    return;
                }
                else if (r_ok == "OK")
                {
                    //파일 삭제 완료 메시지 창
                    makeLog("수신", true, "수신 완료");
                    bowooMessageBox.Show(LanguagePack.Translate("수신이 완료되었습니다."));             
                }

                PageSearch();

                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.SelectedValueChanged -= mf.radddlwrkseq_SelectedValueChanged;
                    mf.setWorkSeq();
                    mf.MakeSessionInq();
                    mf.radddlwrkseq.SelectedValueChanged += mf.radddlwrkseq_SelectedValueChanged;
                }
            }

            catch (Exception exc) 
            {
                //*로그*
                makeLog("수신", false, exc.Message.ToString());
                //파일 삭제 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("수신에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }    
        }

        private void UC01_SMSGridView2_TopButton1_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            //버튼 1처리 새로고침
            UC01_SMSGridView2.GridViewData.SelectionChanged -= UC01_SMSGridView2_SelectionChanged;
            //데이터 바인딩
            UC01_SMSGridView2.BindData(ucGrid2SelectSpName, null);
            //행 변경 이벤트
            UC01_SMSGridView2.GridViewData.SelectionChanged += UC01_SMSGridView2_SelectionChanged;

            if (UC01_SMSGridView2.GridViewData.Rows.Count > 0)
            {
                UC01_SMSGridView2.GridViewData.Rows[0].IsSelected = false;
                UC01_SMSGridView2.GridViewData.Rows[0].IsSelected = true;
            }

            base.HideLoading();
        }
    }
}
