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
using bowoo.Framework.common;
using System.Threading.Tasks;

using System.Data.SqlClient;
using bowoo.Lib;
using bowoo.Lib.DataBase;

namespace Sorter.Pages
{
    public partial class S013_DasList : lib.Common.Management.BaseControl
    {
        //SELECT SP 설정
        string ucGrid1SelectSpName = "[USP_S013_01_DAS_LIST]";
        DataSet ds_Gird = null;
        DataTable temp_table = null; 

        public S013_DasList()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            //left side grid
            this.UC01_SMSGridView1.GridTitleText = "DAS 작업 리스트";

            //버튼 설정
            UC01_SMSGridView1.Button1_Visible = true;
            UC01_SMSGridView1.Button1_Text = "발행";
            UC01_SMSGridView1.Button1_Click = UC01_UC01_SMSGridView1_TopButton1_Click;


        }


        /// <summary>
        /// 발행 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_UC01_SMSGridView1_TopButton1_Click(object sender, EventArgs e)
        {

            try
            {
                temp_table = new DataTable();

                List<int> chuteChkedIdxs = new List<int>();

                for (int i = 0; i < this.UC01_SMSGridView1.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridView1.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chuteChkedIdxs.Add(i);
                    }
                }

                if (chuteChkedIdxs.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("발행할 데이터를 선택해 주세요."));
                    return;
                }

                temp_table = UC01_SMSGridView1.ds_Gird.Tables[1].Copy();
                //temp_table = ds_Gird.Tables[1].Clone();

                //for(int i = 0; i < chuteChkedIdxs.Count; i++)
                //{
                //    temp_table.ImportRow(ds_Gird.Tables[1].Rows[i]); //copy
                //}

                printMethode("das_list");

                //makeLog("발행", true, "발행 완료");
                //설정 완료 메시지 창
                //bowooMessageBox.Show(LanguagePack.Translate("재발행이 완료되었습니다."));
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog("발행", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("발행을 실패하였습니다.\r\n관리자에게 문의하세요."));

            }
        }


        /// <summary>
        /// 프린트 정보를 파싱해서 프린트 클래스에 넘김.
        /// </summary>
        /// <param name="printList"></param>
        private void printMethode(string value)
        {

            Task.Run(() =>
            {
                TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
                //p.print(pickingSet.Tables[1], "Microsoft Print to PDF");
                p.print(temp_table, System.Configuration.ConfigurationManager.AppSettings["laserPrinter"].ToString(), value);
            });

            //Task.Run(() =>
            //{
            //    TelerikBarcode.Printer_interpace p = new TelerikBarcode.Printer_interpace();
            //    //p.print(pickingSet.Tables[1], "Microsoft Print to PDF");
            //    p.print(temp_table, "Microsoft Print to PDF", value);
            //});
        }

        /// <summary>
        /// 페이지 조회 새로고침.
        /// </summary>
        public override void PageSearch()
        {
            try
            {
                base.ShowLoading();

                menuId = "S013";
                menuTitle = "DAS 작업 리스트";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = true;
                    mf.radDropDownListBrand.Enabled = true;
                }

                Dictionary<string, object> paramss = null;

                if (this.UC01_SMSGridView1.GridViewData.IsInEditMode)
                {
                    this.UC01_SMSGridView1.GridViewData.EndEdit();
                }

                //this.GridViewData.Visible = false;
                //this.GridViewData.MasterTemplate.ShowColumnHeaders = false;
                //this.GridViewData. = false;


                //헤더 파라미터 설정
                if (paramss == null)
                {
                    UC01_SMSGridView1.Usp_Load_Parameters["@HEADER_PARAMS"] = BaseEntity.sessInq;
                }

                else
                {
                    UC01_SMSGridView1.Usp_Load_Parameters["@HEADER_PARAMS"] = BaseEntity.sessInq + ";#" + paramss["@HEADER_PARAMS"].ToString();
                }

                //조회 파라미터 설정
                UC01_SMSGridView1.Usp_Load_Parameters["@GRID_PARAMS"] = UC01_SMSGridView1.GetGridParams();

                //첫 번째 셀렉트 문은 헤더 두 번째는 데이터
                //데이터셋 초기화
                ds_Gird = new DataSet();

                //DateTime beforetime = DateTime.Now;
                //DB통신
                ds_Gird = DBUtil.ExecuteDataSet(ucGrid1SelectSpName, UC01_SMSGridView1.Usp_Load_Parameters, CommandType.StoredProcedure);
                UC01_SMSGridView1.ds_Gird = ds_Gird;
                UC01_SMSGridView1.gridUspName = ucGrid1SelectSpName;
                UC01_SMSGridView1.BindData(ds_Gird);





                //this.UC01_SMSGridView1.BindData(ucGrid1SelectSpName, paramss);


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