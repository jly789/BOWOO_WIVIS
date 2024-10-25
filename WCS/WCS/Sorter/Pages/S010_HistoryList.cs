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
    public partial class S010_HistoryList : lib.Common.Management.BaseControl
    {
        //SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S010_05_L_TRAY_LOG_LIST]";
        string ucGrid2SelectSpName = "[USP_S010_05_R_WORK_CFM_LIST]";


        public S010_HistoryList()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            //left side grid
            this.UC01_SMSGridView1.GridTitleText = "투입 로그";
            this.UC01_SMSGridView2.GridTitleText = "슈트 정보";

            this.UC01_SMSGridView2.Button1_Visible = true;
            this.UC01_SMSGridView2.Button1_Text = "선택 설정";
            this.UC01_SMSGridView2.Button1_Click = UC01_SMSGridView2_Button1_Click;


        }
        //출고 - 결과전송 버튼 
        private void UC01_SMSGridView2_Button1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            //base.ShowLoading();

            try
            {
                int chkedCnt = 0;
                List<int> chkedIdxs = new List<int>();

                for (int i = 0; i < this.UC01_SMSGridView2.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridView2.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("초기화할 데이터를 선택 하세요."));
                    return;
                }

                Sorter.Popup.S010_HistoryPopup_ChangeInfo page = new Popup.S010_HistoryPopup_ChangeInfo(ref this.UC01_SMSGridView2, ref chkedIdxs);
                page.ShowDialog();


            }

            catch (Exception exc)
            {
                //*로그*
                //makeLog("결과 전송", false, exc.Message.ToString());
                //설정 실패 메시지 창
                //bowooMessageBox.Show(LanguagePack.Translate("결과 전송에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                PageSearch();
                //base.HideLoading();
                //ProgressPopupW.Close();
            }
        }

        /// <summary>
        /// 페이지 조회 새로고침.
        /// </summary>
        public override void PageSearch()
        {
            try
            {
                base.ShowLoading();

                menuId = "S010";
                menuTitle = "히스토리 조회";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = true;
                    mf.radDropDownListBrand.Enabled = true;
                }

                Dictionary<string, object> paramss = null;
                this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, paramss);
                this.UC01_SMSGridView2.BindData(ucGrid2SelectSpName, paramss);
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


    }
}