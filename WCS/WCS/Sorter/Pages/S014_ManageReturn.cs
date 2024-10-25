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

namespace Sorter.Pages
{
    public partial class S014_ManageReturn : lib.Common.Management.BaseControl
    {
        //SELECT SP 설정 등록리스트 파일 생성 버튼
        string ucGrid3SelectSpName = "[USP_S014_03_L_REG_LIST]";

        //SAVE SP 리스트 적용 버튼
        string ucGrid2SaveSpName = "[USP_S014_02_B_APPLY_LIST]";

        //등록리스트 파일 삭제 버튼
        string ucGrid3DeleteSpName = "USP_S014_03_B_DELETE";

        //배치파일 리스트 클라이언트 경로
        string batchFileListPath = @"C:\";

        DataTable fileDt;

        Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);

        public S014_ManageReturn()
        {
            InitializeComponent();
            RadPanelContents.Visible = true;

            batchFileListPath = config.AppSettings.Settings["batchfilepath"].Value;

            if (!Directory.Exists(batchFileListPath))
            {
                bowooConfirmBox.Show(LanguagePack.Translate("잘못된 경로입니다.\r\n경로를 선택하시겠습니까?"));

                if (bowooConfirmBox.DialogResult == DialogResult.OK)
                {
                    UC01_SMSGridViewTop_Button3_Click(null, null);
                }
            }

            //TOP SIDE GRID
            this.UC01_SMSGridViewTop.GridTitleText = "분류 반품 파일 리스트";
            this.UC01_SMSGridViewTop.Button1_Visible = true;
            this.UC01_SMSGridViewTop.Button1_Text = "파일 삭제";
            this.UC01_SMSGridViewTop.Button1_Click = UC01_SMSGridViewTop_Button1_Click;
            this.UC01_SMSGridViewTop.Button2_Visible = true;
            this.UC01_SMSGridViewTop.Button2_Text = "파일 보기";
            this.UC01_SMSGridViewTop.Button2_Click = UC01_SMSGridViewTop_Button2_Click;
            this.UC01_SMSGridViewTop.Button3_Visible = true;
            this.UC01_SMSGridViewTop.Button3_Text = "경로 변경";
            this.UC01_SMSGridViewTop.Button3_Click = UC01_SMSGridViewTop_Button3_Click;
            this.UC01_SMSGridViewTop.HideSearchCondition();

            //RIGHT SIDE GRID
            this.UC01_SMSGridViewRight.GridTitleText = "반품 파일 데이터 리스트";
            this.UC01_SMSGridViewRight.Button1_Visible = true;
            this.UC01_SMSGridViewRight.Button1_Text = "리스트 적용";
            this.UC01_SMSGridViewRight.Button1_Click = UC01_SMSGridViewRight_Button1_Click;
            this.UC01_SMSGridViewRight.HideSearchCondition();

            this.UC01_SMSGridViewRight.GridViewData.Columns.Add(LanguagePack.Translate("슈트"));
            this.UC01_SMSGridViewRight.GridViewData.Columns.Add(LanguagePack.Translate("바코드"));
            this.UC01_SMSGridViewRight.GridViewData.Columns.Add(LanguagePack.Translate("수량"));

            this.UC01_SMSGridViewRight.GridViewData.Columns[LanguagePack.Translate("슈트")].Width = 30;
            this.UC01_SMSGridViewRight.GridViewData.Columns[LanguagePack.Translate("바코드")].Width = 150;
            this.UC01_SMSGridViewRight.GridViewData.Columns[LanguagePack.Translate("수량")].Width = 30;

            //bot side grid
            //IF_RCV_DATA
            this.UC01_SMSGridViewBot.GridTitleText = "등록 리스트";
            this.UC01_SMSGridViewBot.Button1_Visible = true;
            this.UC01_SMSGridViewBot.Button1_Text = "삭제";
            this.UC01_SMSGridViewBot.Button1_Click = UC01_SMSGridViewBot_Button1_Click;
            this.UC01_SMSGridViewBot.Button2_Visible = true;
            this.UC01_SMSGridViewBot.Button2_Text = "파일 생성";
            this.UC01_SMSGridViewBot.Button2_Click = UC01_SMSGridViewBot_Button2_Click;

            //PageSearch();
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

        private void UpdateAppconfig(string _key, string _str)
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

                //makeLog("데이터 삭제", true, "데이터 삭제 완료");
                // 삭제 버튼을 눌렀을 시 winform 화면 상에서는 삭제가 된 것 처럼 보이는데 db상에서는 파일이 그대로 남아있어서 주석처리

                //파일 삭제 완료 메시지 창
                bowooMessageBox.Show(LanguagePack.Translate("데이터 삭제가 완료되었습니다."));

                //데이터 바인딩
                this.UC01_SMSGridViewBot.BindData(ucGrid3SelectSpName, null);
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

                //해당 경로의 파일 리스트를 가져옴
                BindBatchFileList(batchFileListPath);

                //데이터 바인딩
                this.UC01_SMSGridViewBot.BindData(ucGrid3SelectSpName, null);
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
        /// 화면에 보이는 csv 파일 데이터를 IF_RCV_DATA 테이블에 저장
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

            //base.ShowLoading();

            try
            {
                if (this.UC01_SMSGridViewRight.GridViewData.Rows.Count <= 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("적용할 리스트를 선택하세요."));
                    return;
                }

                bowooConfirmBox.Show(LanguagePack.Translate("리스트 적용 진행을 계속 하시겠습니까?"));

                if (bowooConfirmBox.DialogResult == DialogResult.Cancel)
                {
                    return;
                }

                makeLog("리스트 적용", true, "리스트 적용 진행 수락");

                //전체 행 save
                if (this.UC01_SMSGridViewRight.GridViewData.Rows.Count > 0)
                {
                    ProgressPopupW = new lib.Common.Management.ProgressFormW();
                    ProgressPopupW.progressBar1.Maximum = this.UC01_SMSGridViewRight.GridViewData.Rows.Count;
                    ProgressPopupW.progressBar1.Value = 1;
                    ProgressPopupW.progressBar1.Step = 1;
                    ProgressPopupW.SetLocation(Sorter.MainForm.MainPosition);
                    ProgressPopupW.BringToFront();
                    ProgressPopupW.Show();

                    for (int i = 0; i < this.UC01_SMSGridViewRight.GridViewData.Rows.Count; i++)
                    {



                        this.UC01_SMSGridViewRight.Usp_Save_Parameters[1].Value = this.UC01_SMSGridViewRight.GetDataParam(i);
                        this.UC01_SMSGridViewRight.ExcuteSaveSp(ucGrid2SaveSpName, i);
                        ProgressPopupW.progressBar1.PerformStep();
                    }

                    ProgressPopupW.Close();

                    makeLog("리스트 적용", true, "리스트 적용 완료");
                    //리스트 적용 완료
                    bowooMessageBox.Show(LanguagePack.Translate("리스트 적용이 완료되었습니다."));

                    //등록 리스트 데이터 바인딩
                    this.UC01_SMSGridViewBot.BindData(ucGrid3SelectSpName, null);
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
                //base.HideLoading();

                ProgressPopupW.Close();
            }
        }

        //파일보기 버튼
        /// <summary>
        /// 파일보기 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private DataTable GetDataTableFromCsv(string filePath, bool hasHeader)
        {
            DataTable dt = new DataTable();

            using (var reader = new StreamReader(filePath))
            {
                var line = reader.ReadLine();
                var columns = line.Split(',');

                if (hasHeader)
                {
                    foreach (var column in columns)
                    {
                        dt.Columns.Add(column.Trim());
                    }
                }
                else
                {
                    for (int i = 0; i < columns.Length; i++)
                    {
                        dt.Columns.Add($"Column{i + 1}");
                    }
                }

                while ((line = reader.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    dt.Rows.Add(values);
                }
            }

            return dt;
        }

        private void UC01_SMSGridViewTop_Button2_Click(object sender, EventArgs e)
        {
            //base.ShowLoading();

            try
            {
                //파일보기 클릭
                //행 선택 확인
                if (this.UC01_SMSGridViewTop.GridViewData.SelectedRows.Count <= 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("조회할 파일을 선택하세요."));
                    return;
                }

                if (this.UC01_SMSGridViewTop.GridViewData.SelectedRows.Count > 1)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("1개의 파일만 선택하세요."));
                    return;
                }

                // 파일 선택 및 CSV 검사
                string selectedFileName = this.UC01_SMSGridViewTop.GridViewData.SelectedRows[0].Cells[LanguagePack.Translate("전체 파일 명")].Value.ToString();
                bool isExistHeader = true;

                ProgressPopupW = new lib.Common.Management.ProgressFormW();
                ProgressPopupW.Show();

                DataTable fileDt = null;

                try
                {
                    fileDt = GetDataTableFromCsv(selectedFileName, isExistHeader);
                }
                catch (Exception ex)
                {
                    bowooMessageBox.Show($"CSV 파일을 읽는 도중 오류가 발생했습니다: {ex.Message}");
                    return;
                }
                finally
                {
                    ProgressPopupW.Close();
                }

                // 파일이 비어있거나 컬럼이 존재하지 않을 경우 처리
                if (fileDt == null || fileDt.Rows.Count == 0)
                {
                    bowooMessageBox.Show(LanguagePack.Translate("CSV 파일이 비어 있거나 잘못된 형식입니다."));
                    return;
                }

                if (!fileDt.Columns.Contains("ITEM_BAR") || !fileDt.Columns.Contains("ORD_QTY") || !fileDt.Columns.Contains("CHUTE_NO"))
                {
                    bowooMessageBox.Show(LanguagePack.Translate("필수 컬럼이 누락되었습니다."));
                    return;
                }

                this.UC01_SMSGridViewRight.GridViewData.Columns.Clear();
                fileDt.SetColumnsOrder(new string[] { "CHUTE_NO", "ITEM_BAR", "ORD_QTY" });
                this.UC01_SMSGridViewRight.GridViewData.DataSource = fileDt;

                // 컬럼 설정
                for (int i = 0; i < this.UC01_SMSGridViewRight.GridViewData.Columns.Count; i++)
                {
                    this.UC01_SMSGridViewRight.GridViewData.Columns[i].IsVisible = false;
                    this.UC01_SMSGridViewRight.GridViewData.Columns[i].ReadOnly = true;
                    this.UC01_SMSGridViewRight.GridViewData.Columns[i].Tag = new lib.Common.Management.ParamType() { Data = true };

                    // CHUTE_NO
                    if (this.UC01_SMSGridViewRight.GridViewData.Columns[i].FieldName == "CHUTE_NO")
                    {
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].IsVisible = true;
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].HeaderText = LanguagePack.Translate("슈트");
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].TextAlignment = ContentAlignment.MiddleCenter;
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].Width = 30;
                    }


                    // ITEM_BAR
                    if (this.UC01_SMSGridViewRight.GridViewData.Columns[i].FieldName == "ITEM_BAR")
                    {
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].IsVisible = true;
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].HeaderText = LanguagePack.Translate("바코드");
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].TextAlignment = ContentAlignment.MiddleCenter;
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].Width = 150;
                    }

                    // ORD_QTY
                    if (this.UC01_SMSGridViewRight.GridViewData.Columns[i].FieldName == "ORD_QTY")
                    {
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].IsVisible = true;
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].HeaderText = LanguagePack.Translate("수량");
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].TextAlignment = ContentAlignment.MiddleRight;
                        this.UC01_SMSGridViewRight.GridViewData.Columns[i].Width = 30;
                    }

                  
                }

                makeLog(((RadButton)sender).Text, true, "파일 보기 완료");
            }
            catch (Exception exc)
            {
                // 로그
                makeLog(((RadButton)sender).Text, false, exc.Message.ToString());
                bowooMessageBox.Show(LanguagePack.Translate("파일 보기에 실패하였습니다.\r\n관리자에게 문의하세요."));
            }
            finally
            {
                //base.HideLoading();
                ProgressPopupW.Close();
            }
        }        //분류 반품 파일리스트 삭제
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

            FileSystemInfo[] filesInDirectory = directory.GetFileSystemInfos();

            int listIdx = 0;

            for (int i = 0; i < filesInDirectory.Length; i++)
            {
                if (filesInDirectory[i].Extension.ToUpper() == ".CSV")
                {
                    listIdx++;
                    this.UC01_SMSGridViewTop.GridViewData.Rows.Add(new object[] { false, listIdx, _batchFileListPath, filesInDirectory[i].Name, filesInDirectory[i].CreationTime, filesInDirectory[i].Extension, filesInDirectory[i].FullName });
                }
            }

            this.UC01_SMSGridViewTop.SetGridTitleWithRowCount();
        }
    }

    //public static class DataTableExtensions
    //{
    //    public static void SetColumnsOrder(this DataTable table, params String[] columnNames)
    //    {
    //        int columnIndex = 0;

    //        foreach (var columnName in columnNames)
    //        {
    //            table.Columns[columnName].SetOrdinal(columnIndex);
    //            columnIndex++;
    //        }
    //    }
    //}
}