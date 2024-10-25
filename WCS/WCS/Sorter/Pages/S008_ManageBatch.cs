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
using System.IO;
using System.Data.OleDb;
using System.Globalization;
using System.Configuration;
using System.Diagnostics;

namespace Sorter.Pages
{
    public partial class S008_ManageBatch : lib.Common.Management.BaseControl
    {
        //SELECT SP 설정
        string ucGrid3SelectSpName = "[USP_S008_03_L_REG_LIST]";
        string ucGrid2GetConvert = "[USP_S008_01_GET_CONVERT]";

        //SAVE SP 리스트 적용 버튼
        string ucGrid2SaveSpName = "[USP_S008_02_B_CONVERT_BUNDLE]";
        string ucGrid3DeleteSpName = "USP_S008_03_B_DELETE";
        string ucGrid2SelectSpName = "USP_S008_03_L_BUNDLE_LIST";

        //SP 적용
        string uploadFileSp = "USP_S008_01_B_APPLY_BUNDEL";



        //배치파일 리스트 클라이언트 경로
        string batchFileListPath = @"D:\RCV_DATA";

        DataTable fileDt;

        Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

        public S008_ManageBatch()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            batchFileListPath = config.AppSettings.Settings["batchfilepath"].Value;

            if (!Directory.Exists(batchFileListPath))
            {
                bowooMessageBox.Show(@"D드라이브에 RCV_DATA폴더가 없습니다.\r\n폴더와 파일을 생성후 실행 부탁드립니다.");
                //bowooConfirmBox.Show(LanguagePack.Translate("잘못된 경로입니다.\r\n경로를 선택하시겠습니까?"));

                //if (bowooConfirmBox.DialogResult == DialogResult.OK)
                //{
                //    UC01_SMSGridViewTop_Button3_Click(null,null);
                //}
            }

            //TOP SIDE GRID
            this.UC01_SMSGridViewTop.GridTitleText = "분류 배치 파일 리스트";
            //this.UC01_SMSGridViewTop.Button1_Visible = true;
            //this.UC01_SMSGridViewTop.Button1_Text = "파일 삭제";
            //this.UC01_SMSGridViewTop.Button1_Click = UC01_SMSGridViewTop_Button1_Click;
            this.UC01_SMSGridViewTop.Button1_Visible = true;
            this.UC01_SMSGridViewTop.Button1_Text = "업로드";
            this.UC01_SMSGridViewTop.Button1_Click = UC01_SMSGridViewTop_Button2_Click;
            this.UC01_SMSGridViewTop.Button2_Visible = true;
            this.UC01_SMSGridViewTop.Button2_Text = "경로 변경";
            this.UC01_SMSGridViewTop.Button2_Click = UC01_SMSGridViewTop_Button3_Click;
            this.UC01_SMSGridViewTop.HideSearchCondition();

            //RIGHT SIDE GRID
            this.UC01_SMSGridViewRight.GridTitleText = "번들 등록 리스트";
            this.UC01_SMSGridViewRight.Button1_Visible = true;
            this.UC01_SMSGridViewRight.Button1_Text = "리스트 적용";
            this.UC01_SMSGridViewRight.Button1_Click = UC01_SMSGridViewRight_Button1_Click;
            this.UC01_SMSGridViewRight.HideSearchCondition();

            //this.UC01_SMSGridViewRight.GridViewData.Columns.Add(LanguagePack.Translate("슈트"));
            //this.UC01_SMSGridViewRight.GridViewData.Columns.Add(LanguagePack.Translate("바코드"));
            //this.UC01_SMSGridViewRight.GridViewData.Columns.Add(LanguagePack.Translate("수량"));

            //this.UC01_SMSGridViewRight.GridViewData.Columns[LanguagePack.Translate("슈트")].Width = 30;       
            //this.UC01_SMSGridViewRight.GridViewData.Columns[LanguagePack.Translate("바코드")].Width = 150;
            //this.UC01_SMSGridViewRight.GridViewData.Columns[LanguagePack.Translate("수량")].Width = 30;

            //bot side grid
            //IF_RCV_DATA
            this.UC01_SMSGridViewBot.GridTitleText = "정리 등록 리스트";
            //this.UC01_SMSGridViewBot.Button1_Visible = true;
            //this.UC01_SMSGridViewBot.Button1_Text = "삭제";
            //this.UC01_SMSGridViewBot.Button1_Click = UC01_SMSGridViewBot_Button1_Click;
            //this.UC01_SMSGridViewBot.Button2_Visible = true;
            //this.UC01_SMSGridViewBot.Button2_Text = "파일 생성";
            //this.UC01_SMSGridViewBot.Button2_Click = UC01_SMSGridViewBot_Button2_Click;

            PageSearch();
        }

        //경로 변경 버튼
        /// <summary>
        /// 경로 변경 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewTop_Button3_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog forder = new FolderBrowserDialog();

                if (forder.ShowDialog() == DialogResult.OK)
                {
                    UpdateAppconfig("batchfilepath", forder.SelectedPath);
                    batchFileListPath = forder.SelectedPath;
                    //해당 경로의 파일 리스트를 가져옴
                    BindBatchFileList(batchFileListPath);
                    makeLog("경로 변경", true, "경로 변경 완료" + forder.SelectedPath);
                }
            }

            catch (Exception exc)
            {
                makeLog("결과 전송", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("경로 변경에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }
        }

        private void UpdateAppconfig (string _key, string _str)
        {
            config.AppSettings.Settings[_key].Value = _str;
            config.Save(ConfigurationSaveMode.Minimal);
        }
        
        //등록리스트 파일 삭제 버튼
        /// <summary>
        /// 등록리스트 파일 삭제 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewBot_Button1_Click(object sender, EventArgs e)
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
                for (int i = 0; i < this.UC01_SMSGridViewBot.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewBot.GridViewData.Rows[i].Cells["checkbox"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("삭제할 데이터를 선택하세요."));
                    return;
                }

                //삭제 확인 메시지 창
                bowooRedConfirmBox.Show(LanguagePack.Translate("데이터 삭제 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                for (int i = 0; i < chkedIdxs.Count; i++)
                {
                    this.UC01_SMSGridViewBot.ExcuteSaveSp(ucGrid3DeleteSpName, chkedIdxs[i]);
                }

                makeLog("데이터 삭제", true, "데이터 삭제 완료");
                //파일 삭제 완료 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("데이터 삭제가 완료되었습니다."));

                //데이터 바인딩
                this.UC01_SMSGridViewBot.BindData(ucGrid3SelectSpName, null);
                this.UC01_SMSGridViewRight.BindData(ucGrid2SelectSpName, null);
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("데이터 삭제", false, exc.Message.ToString());
                //파일 삭제 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("데이터 삭제에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //페이지 조회 새로고침.
        /// <summary>
        /// 페이지 조회 새로고침.
        /// </summary>
        public override void PageSearch()
        {
            try
            {
                base.ShowLoading();

                menuId = "S008";
                menuTitle = "배치 관리";

                //조회 메뉴 브랜드, 차수 활성/비활성화
                Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;

                if (mf != null)
                {
                    mf.radddlwrkseq.Enabled = true;
                    mf.radDropDownListBrand.Enabled = true;
                }

                SqlParameter[] parmData = new SqlParameter[2];
                parmData[0] = new SqlParameter();
                parmData[0].ParameterName = "@HEADER_PARAMS";
                parmData[0].DbType = DbType.String;
                parmData[0].Value = bowoo.Framework.common.BaseEntity.sessInq; 
                //헤더파라미터

                parmData[1] = new SqlParameter();
                parmData[1].ParameterName = "@CONVERT_STATUS"; //반환값
                parmData[1].DbType = DbType.Int32;
                parmData[1].Value = 1;
                parmData[1].Direction = ParameterDirection.Output;

                DBUtil.ExecuteDataSetSqlParam(ucGrid2GetConvert, parmData);

                //번들 작업 정보 적용 유무 확인 후 적용전 일경우 적용 버튼 활성화
                if(parmData[1].Value != null && Convert.ToInt32(parmData[1].Value) == 0)
                {
                    this.UC01_SMSGridViewRight.Button1_enabled = true;
                }
                else
                {
                    this.UC01_SMSGridViewRight.Button1_enabled = false;
                }

                //해당 경로의 파일 리스트를 가져옴
                BindBatchFileList(batchFileListPath);

                //데이터 바인딩
                this.UC01_SMSGridViewBot.BindData(ucGrid3SelectSpName, null);
                this.UC01_SMSGridViewRight.BindData(ucGrid2SelectSpName, null);


            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("배치 관리 조회", false, exc.Message.ToString());
                //설정 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("조회에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }

        //등록리스트 파일생성 버튼
        /// <summary>
        /// 등록리스트 파일생성 버튼
        /// DATA TABLE -> CSV 파일
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewBot_Button2_Click(object sender, EventArgs e)
        {
            //base.ShowLoading();

            try
            {
                SaveFileDialog file = new SaveFileDialog();
                file.InitialDirectory = batchFileListPath;
                file.Filter = "CSV file(*.csv)|*.csv";
                string fileNmaeCsv = this.UC01_SMSGridViewBot.GridTitleText + (Sorter.MainForm.Brand != string.Empty ? "_" + Sorter.MainForm.Brand : string.Empty) + (Sorter.MainForm.WorkSeq != string.Empty ? "_" + Sorter.MainForm.WorkSeq : string.Empty);
                file.FileName = string.Format("{0}_{1}", fileNmaeCsv, DateTime.Now.ToString("yyyyMMdd_hhmmss"));

                if (file.ShowDialog() == DialogResult.OK)
                {
                    ConvertToCsvFileFromGridView(UC01_SMSGridViewBot.GridViewData, file.FileName, true);

                    makeLog("파일 생성", true, "파일 생성 완료");
                    bowooMessageBox.Show(LanguagePack.Translate("파일 생성이 완료되었습니다."));

                    //생성 파일 실행
                    System.Diagnostics.Process.Start(file.FileName);

                    //해당 경로의 파일 리스트를 가져옴
                    BindBatchFileList(batchFileListPath);
                }
            }

            catch (Exception exc)
            {
                //로그
                makeLog("파일 생성", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("파일 생성에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                //base.HideLoading();
            }
        }

        //리스트 적용버튼 클릭
        /// <summary>
        /// 리스트 적용버튼 클릭
        /// 번들 정보 work_ord에 적용
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewRight_Button1_Click(object sender, EventArgs e)
        {
            if (bowoo.Framework.common.BaseEntity.sessLv == 2)
            {
                bowooMessageBox.Show(LanguagePack.Translate("권한이 없습니다.\r\n관리자에게 문의하세요."));
                return;
            }

            base.ShowLoading();

            try
            {
                makeLog("리스트 적용", true, "리스트 적용 진행 수락");

                
                if (this.UC01_SMSGridViewRight.GridViewData.Rows.Count > 0
                    || this.UC01_SMSGridViewBot.GridViewData.Rows.Count > 0)
                {
                    //ucGrid2SaveSpName


                    //makeLog("리스트 적용", true, "리스트 적용 완료");
                    ////리스트 적용 완료
                    //bowooMessageBox.Show(LanguagePack.Translate("리스트 적용이 완료되었습니다."));

                    //등록 리스트 데이터 바인딩
                    //this.UC01_SMSGridViewBot.BindData(ucGrid3SelectSpName, null);
                    //this.UC01_SMSGridViewRight.BindData(ucGrid2SelectSpName, null);


                    SqlParameter[] parmData = new SqlParameter[3];
                    parmData[0] = new SqlParameter();
                    parmData[0].ParameterName = "@HEADER_PARAMS";
                    parmData[0].DbType = DbType.String;
                    parmData[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터

                    parmData[1] = new SqlParameter();
                    parmData[1].ParameterName = "@R_OK"; //반환값
                    parmData[1].Size = 50;
                    parmData[1].Value = "NG";
                    parmData[1].Direction = ParameterDirection.Output;

                    parmData[2] = new SqlParameter();
                    parmData[2].ParameterName = "@R_MESSAGE"; //반환값
                    parmData[2].Size = 200;
                    parmData[2].Value = "";
                    parmData[2].Direction = ParameterDirection.Output;

                    DBUtil.ExecuteDataSetSqlParam(ucGrid2SaveSpName, parmData);

                    string r_ok = parmData[1].Value.ToString();
                    string rmessage = parmData[2].Value.ToString();

                    if(r_ok != "OK")
                    {
                        bowooMessageBox.Show(LanguagePack.Translate(rmessage));
                        return;
                    }
                    else
                    {
                        bowooMessageBox.Show(LanguagePack.Translate("리스트 적용이 완료 되었습니다."));
                        this.PageSearch();
                    }


                    Sorter.MainForm mf = this.FindForm() as Sorter.MainForm;
                    if (mf != null)
                    {
                        mf.radddlwrkseq.SelectedValueChanged -= mf.radddlwrkseq_SelectedValueChanged;
                        mf.setWorkSeq();
                        mf.MakeSessionInq();
                        mf.radddlwrkseq.SelectedValueChanged += mf.radddlwrkseq_SelectedValueChanged;
                    }
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog("리스트 적용", false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("리스트 적용에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();

                //ProgressPopupW.Close();
            }
        }

        //파일보기 버튼
        /// <summary>
        /// 파일보기 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewTop_Button2_Click(object sender, EventArgs e)
        {
            //base.ShowLoading();

            try
            {
                //파일보기 클릭
                //행 선택 확인

                List<int> chkedIdxs = new List<int>();

                for (int i = 0; i < this.UC01_SMSGridViewTop.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewTop.GridViewData.Rows[i].Cells["check"].Value)
                    {
                        chkedIdxs.Add(i);
                        //chkedCnt++;
                    }
                }


                if (chkedIdxs.Count <= 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("업로드 할 파일을 선택해 주세요."));
                    return;
                }

                if (chkedIdxs.Count > 1)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("1개의 파일만 선택하세요."));
                    return;
                }

                if (chkedIdxs.Count > 0)
                {

                    //바코드 수량 슈트만 보임.
                    string selectedFileName = this.UC01_SMSGridViewTop.GridViewData.Rows[chkedIdxs[0]].Cells[LanguagePack.Translate("전체 파일 명")].Value.ToString();

                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = 1;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    DataSet ds =lib.Common.Management.ExcelDB.OpenExcelDB(selectedFileName);
                    ds.Tables[0].Columns["그룹"].ColumnName = "group_no";
                    ds.Tables[0].Columns["스타일"].ColumnName = "barcode";
                    ds.Tables[0].Columns["구성수량"].ColumnName = "sku_qty";
                    ds.Tables[0].Columns["시작슈트"].ColumnName = "chute_str";
                    ds.Tables[0].Columns["마지막슈트"].ColumnName = "chute_end";
                    ds.Tables[0].Columns["박스수"].ColumnName = "box_ord_qty";
                    ds.Tables[0].Columns["상태"].ColumnName = "status";

                    //상태값이 1,2가 아닌 다른 값이 있을경우. 1: 번들 작업, 2: 정리작업
                    if (ds.Tables[0].AsEnumerable().Where(k => k.Field<Double>("status") < 1 || k.Field<Double>("status") > 2).Count() > 1)
                    {
                        bowooMessageBox.Show("상태 값은 1 또는 2만 입력이 가능 합니다.\r\n엑셀 데이터를 확인 해 주세요.");
                        return;
                    }

                    if(ds.Tables[0].AsEnumerable().Where(k => k.Field<Double>("box_ord_qty") <  k.Field<Double>("chute_str") - k.Field<Double>("chute_end") + 1).Count() > 1)
                    {

                        bowooMessageBox.Show("박스수 보다 지정된 슈트가 많습니다.\r\n엑셀 데이터를 확인 해 주세요.");
                        return;
                    }

                    List<Double> group_list = ds.Tables[0].AsEnumerable().Select(j => j.Field<Double>("group_no")).Distinct().ToList();
                    int group_count = group_list.Count;

                    for (int i = 0; i < group_list.Count(); i++)
                    {
                        var count1 = ds.Tables[0].AsEnumerable().Where(k=> k.Field<Double>("group_no") == group_list[i] && k.Field<Double>("status") == 1)
                                                .GroupBy(k => new
                        {
                            g = k.Field<Double>("group_no"),
                            s = k.Field<Double>("chute_str"),
                            e = k.Field<Double>("chute_end")
                        }).Count();

                        //같은 그룹에서 슈트 정보가 다른게 있을 경우.
                        if (count1 > 1)
                        {
                            bowooMessageBox.Show(LanguagePack.Translate(i + 1 + "그룹의 슈트 설정이 상이한 데이터가 있습니다.\r\n확인 후 업로드 부탁드립니다."));
                            return;
                        }
                    }
                    
                    //지정 슈트가 중복된게 있는지 확인.
                    var count2 = ds.Tables[0].AsEnumerable()
                                            .GroupBy(k => new
                                            {
                                                g = k.Field<Double>("group_no"),
                                                s = k.Field<Double>("chute_str"),
                                                e = k.Field<Double>("chute_end")
                                            });

                    foreach (var data in count2)
                    {
                        var check = count2.Where(k => (k.Key.s >= data.Key.s && k.Key.s <= data.Key.e
                                      || k.Key.e >= data.Key.s && k.Key.e <= data.Key.e)
                                      && k.Key.g != data.Key.g).Count();

                        //그룹이 다른데 같은 슈트로 지정된게 있을대 에러로 뺌
                        if (check > 0)
                        {
                            bowooMessageBox.Show(LanguagePack.Translate("중복 설정된 슈트가 존재합니다.\r\n정보를 수정 부탁드립니다."));
                            return;
                        }
                    }

                    //엑셀 데이터 임시 테이블에 업로드
                    DBUtil.bulkCopy(ds.Tables[0], "RCV_TEMP_BUNDLE");
                    
                    SqlParameter[] parmData = new SqlParameter[3];
                    parmData[0] = new SqlParameter();
                    parmData[0].ParameterName = "@HEADER_PARAMS";
                    parmData[0].DbType = DbType.String;
                    parmData[0].Value = bowoo.Framework.common.BaseEntity.sessInq; //헤더파라미터

                    parmData[1] = new SqlParameter();
                    parmData[1].ParameterName = "@R_OK"; //반환값
                    parmData[1].Size = 50;
                    parmData[1].Value = "NG";
                    parmData[1].Direction = ParameterDirection.Output;

                    parmData[2] = new SqlParameter();
                    parmData[2].ParameterName = "@R_MESSAGE"; //반환값
                    parmData[2].Size = 200;
                    parmData[2].Value = "";
                    parmData[2].Direction = ParameterDirection.Output;

                    DBUtil.ExecuteDataSetSqlParam(uploadFileSp, parmData);

                    if(parmData[1].Value.ToString() == "NG")
                    {
                        makeLog(((RadButton)sender).Text, false, parmData[2].Value.ToString());
                        //bowooMessageBox.Show(LanguagePack.Translate("파일 업로드에 실패 했습니다.\r\n관리자에게 문의하세요."));
                        bowooMessageBox.Show(parmData[2].Value.ToString());
                    }
                    else
                    {
                        makeLog(((RadButton)sender).Text, true, "파일 업로드 완료");
                        bowooMessageBox.Show(LanguagePack.Translate("파일 업로드 완료"));
                        PageSearch();
                    }
                    ProgressPopupW.progressBar1.PerformStep();
                    ProgressPopupW.Close();
                }
            }
            catch (Exception exc)
            {
                //*로그*
                makeLog(((RadButton)sender).Text, false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("파일 업로드에 실패 했습니다.\r\n관리자에게 문의하세요."));
            }
            finally
            {
                ProgressPopupW.Close();
            }
        }
        
        //파일 삭제 버튼
        /// <summary>
        /// 파일 삭제 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UC01_SMSGridViewTop_Button1_Click(object sender, EventArgs e)
        {
            base.ShowLoading();

            int chkedCnt = 0;
            List<int> chkedIdxs = new List<int>();

            try
            {
                for (int i = 0; i < this.UC01_SMSGridViewTop.GridViewData.Rows.Count; i++)
                {
                    if ((bool)this.UC01_SMSGridViewTop.GridViewData.Rows[i].Cells["check"].Value)
                    {
                        chkedIdxs.Add(i);
                        chkedCnt++;
                    }
                }

                if (chkedCnt == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("삭제할 파일을 선택하세요."));
                    return;
                }

                //삭제 확인 메시지 창
                bowooRedConfirmBox.Show(LanguagePack.Translate("파일 삭제 진행을 계속 하시겠습니까?"));

                if (bowooRedConfirmBox.DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                makeLog("파일 삭제", true, "파일 삭제 진행 수락");

                for (int i = 0; i < chkedIdxs.Count; i++)
                {
                    int chkedIdx = chkedIdxs[i];

                    FileInfo delFile = new FileInfo(this.UC01_SMSGridViewTop.GridViewData.Rows[chkedIdx].Cells[LanguagePack.Translate("전체 파일 명")].Value.ToString());
                    //삭제
                    delFile.Delete();
                }

                makeLog(((RadButton)sender).Text, true, "파일 삭제 완료");
                //파일 삭제 완료 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("파일 삭제가 완료되었습니다."));

                //파일리스트 다시 조회
                BindBatchFileList(batchFileListPath);
            }

            catch (IOException exc)
            {
                var errorCode = System.Runtime.InteropServices.Marshal.GetHRForException(exc) & ((1 << 16) - 1);

                if (errorCode == 32 || errorCode == 33)
                {
                    //*로그*
                    makeLog(((RadButton)sender).Text, false, exc.Message.ToString());
                    //파일 삭제 실패 메시지 창
                    bowooMessageBox.Show(LanguagePack.Translate("파일이 사용 중입니다.\r\n해당 파일을 닫고, 다시 시도해주세요."));
                }
            }

            catch (Exception exc)
            {
                //*로그*
                makeLog(((RadButton)sender).Text, false, exc.Message.ToString());
                //파일 삭제 실패 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("파일 삭제에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }

            finally
            {
                base.HideLoading();
            }
        }


        /// <summary>
        /// 해당하는 클라이언트 경로의 파일리스트를 가져온다.
        /// </summary>
        private void BindBatchFileList(string _batchFileListPath)
        {
            //클라이언트 PC 특정 로컬 경로의 CSV파일을 가져온다.
            this.UC01_SMSGridViewTop.GridViewData.Rows.Clear();

            if (this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.Columns.Count < 1)
            {
                this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.AutoGenerateColumns = true;
                
                GridViewCheckBoxColumn gCheckBoxCol = new GridViewCheckBoxColumn();

                gCheckBoxCol.Width = 10;
                gCheckBoxCol.HeaderText = "";
                gCheckBoxCol.FieldName = "Check";
                //gCheckBoxCol.EnableHeaderCheckBox = true;

                this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.Columns.Add(gCheckBoxCol);
                this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(LanguagePack.Translate("순번")));
                this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(LanguagePack.Translate("파일경로")));
                this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(LanguagePack.Translate("파일 명")));
                this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(LanguagePack.Translate("파일 생성 일자")));
                this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(LanguagePack.Translate("확장자")));
                this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.Columns.Add(new GridViewTextBoxColumn(LanguagePack.Translate("전체 파일 명")));

                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("순번")].Width = 30;
                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("순번")].TextAlignment = ContentAlignment.MiddleCenter;

                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("파일경로")].Width = 100;
                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("파일 명")].Width = 260;
                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("파일 생성 일자")].Width = 130;
                this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.Columns[LanguagePack.Translate("확장자")].IsVisible = false;
                this.UC01_SMSGridViewTop.GridViewData.MasterTemplate.Columns[LanguagePack.Translate("전체 파일 명")].IsVisible = false;

                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("순번")].ReadOnly = true;
                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("파일경로")].ReadOnly = true;
                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("파일 명")].ReadOnly = true;
                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("파일 생성 일자")].ReadOnly = true;
                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("확장자")].ReadOnly = true;
                this.UC01_SMSGridViewTop.GridViewData.Columns[LanguagePack.Translate("전체 파일 명")].ReadOnly = true;
            }
            
            DirectoryInfo directory = new DirectoryInfo(_batchFileListPath);

            //경로 존재 확인
            if (!directory.Exists)
            {
                bowooConfirmBox.Show(LanguagePack.Translate("존재하지 않는 경로입니다."));
                return;
            }

            //FileSystemInfo[] filesInDirectory = directory.GetFiles();
            FileSystemInfo[] filesInDirectory = directory.GetFileSystemInfos();

            int listIdx = 0;

            for (int i = 0; i < filesInDirectory.Length; i++)
            {
                //if (filesInDirectory[i].Extension.ToLower() == ".csv")
                //{
                    listIdx++;
                    this.UC01_SMSGridViewTop.GridViewData.Rows.Add(new object[] { false, listIdx, _batchFileListPath, filesInDirectory[i].Name, filesInDirectory[i].CreationTime, filesInDirectory[i].Extension, filesInDirectory[i].FullName });
                //}
            }

            this.UC01_SMSGridViewTop.SetGridTitleWithRowCount();
        }
    }

    public static class DataTableExtensions
    {
        public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
        {
            int columnIndex = 0;

            foreach (var columnName in columnNames)
            {
                table.Columns[columnName].SetOrdinal(columnIndex);
                columnIndex++;
            }
        }
    }
}