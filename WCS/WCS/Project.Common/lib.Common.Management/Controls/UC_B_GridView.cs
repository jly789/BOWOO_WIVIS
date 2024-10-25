using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Data.SqlClient;
using bowoo.Lib;
using bowoo.Lib.DataBase;
using bowoo.Framework.common;
using System.Globalization;
using Telerik.WinControls;
using Telerik.WinControls.Primitives;

namespace lib.Common.Management
{
    public partial class UC_B_GridView : RadGridView
    {

        public DataSet ds_Gird;
        public Dictionary<string, object> Usp_Load_Parameters = new Dictionary<string, object>();
        public Dictionary<string, object> Add_Data_Parameters = new Dictionary<string, object>();
        public SqlParameter[] Usp_Save_Parameters = new SqlParameter[4];

        GridViewSummaryItem summaryItem;
        private GridViewSummaryRowItem topSummaryRow;

        public RadCheckBox radChkRefresh;
        public Timer refreshTimer;

        string rawHeaderText = "";
        string tagText = "";
        string columnHeaderText = "";
        int tagStartIndex = -1;
        int tagEndIndex = -1;
        string[] splitedTagText;
        string[] tagKeyandValue;
        string gridUspName = "";

        Dictionary<string, object> _paramss;

        Color selectedCellBackColor = Color.FromArgb(228, 247, 186);
        Color selectedCellBackColor2 = Color.FromArgb(188, 229, 92);

        public UC_B_GridView()
        {
            InitializeComponent();

            InitGrid();
        }

        private void InitGrid()
        {
            if (!Usp_Load_Parameters.ContainsKey("@HEADER_PARAMS")) Usp_Load_Parameters.Add("@HEADER_PARAMS", "");
            if (!Usp_Load_Parameters.ContainsKey("@GRID_PARAMS")) Usp_Load_Parameters.Add("@GRID_PARAMS", "");

            //* 그리드뷰 기본 설정 *

            //새 행 추가 불가
            this.AllowAddNewRow = false;

            //그룹 행 숨김
            this.ShowGroupPanel = false;

            //행 다중 선택 가능
            this.MultiSelect = true;

            //Full Row Select
            this.SelectionMode = Telerik.WinControls.UI.GridViewSelectionMode.FullRowSelect;

            //세로 스크롤 항상 표시
            this.MasterTemplate.VerticalScrollState = ScrollState.AlwaysShow;

            //행 크기 조절 불가
            this.AllowRowResize = false;

            //컬럼 자동 사이즈 조절
            this.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.Fill;

            //행 헤더 숨김
            this.ShowRowHeaderColumn = false;




            //새행 텍스트.,.
            this.MasterTemplate.NewRowText = "새 행을 추가하려면 이곳을 클릭하세요.";

            this.AllowDeleteRow = false;

            //*그리드뷰 이벤트
            this.ViewCellFormatting += GridViewData_ViewCellFormatting;
            this.RowFormatting += GridViewData_RowFormatting;
            
            this.ContextMenuOpening += GridViewData_ContextMenuOpening;
        }

        #region 그리드 스타일 관련(행 선택..)
        void GridViewData_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            GridDataCellElement dCell = e.CellElement as GridDataCellElement;
            if (dCell != null)
            {
                if (e.CellElement.IsCurrent)
                {
                    e.CellElement.IsCurrent = false;
                    e.CellElement.IsCurrentColumn = false;

                }
                else
                {
                    e.CellElement.ResetValue(LightVisualElement.DrawBorderProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                }


                if (e.CellElement.RowElement.IsCurrent || e.CellElement.RowElement.IsSelected)
                {
                    e.CellElement.BorderBottomColor = selectedCellBackColor;
                    e.CellElement.BorderTopColor = selectedCellBackColor;

                }
                else
                {
                    //마우스 오버 시 행 색상 변경
                    e.CellElement.BackColor = selectedCellBackColor;
                    e.CellElement.BorderColor = selectedCellBackColor;

                    if (e.CellElement.Value != null && !string.IsNullOrEmpty(e.CellElement.Value.ToString()))
                        e.CellElement.ToolTipText = e.CellElement.Value.ToString();

                }
            }



            GridSummaryCellElement cell = e.CellElement as GridSummaryCellElement;
            if (cell != null)
            {
                e.CellElement.ForeColor = Color.Red;

                e.CellElement.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                if ((cell.ColumnInfo.Tag as ParamType).Title)
                {
                    e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
                }
                else if ((cell.ColumnInfo.Tag as ParamType).Sum)
                {
                    e.CellElement.TextAlignment = ContentAlignment.MiddleRight;
                }
            }
            //헤더 폰트 사이즈 
            GridHeaderCellElement hcell = e.CellElement as GridHeaderCellElement;
            if (hcell != null)
            {
                e.CellElement.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            }
        }

        //선택 행 스타일
        void GridViewData_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            //행 선택 시 스타일
            if (e.RowElement.IsSelected || e.RowElement.IsCurrent)
            {
                e.RowElement.GradientStyle = GradientStyles.Linear;
                e.RowElement.BackColor = selectedCellBackColor;
                e.RowElement.BackColor2 = selectedCellBackColor2;

            }

        }
        
        #endregion

        #region 오른쪽 마우스 관련

        RadMenuItem menuItem1;
        RadMenuItem menuItem2;
        RadMenuItem menuItem3;
        RadMenuItem menuItem4;
        //오른쪽 마우스 오픈
        private void GridViewData_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            GridDataCellElement cell = e.ContextMenuProvider as GridDataCellElement;
            GridHeaderCellElement hcell = e.ContextMenuProvider as GridHeaderCellElement;
            //데이터영역 셀일 경우 
            if (cell != null)
            {
                foreach (object obj in e.ContextMenu.Items)
                {
                    RadMenuItem item = obj as RadMenuItem;
                    if (item != null)
                    {
                        switch (item.Text)
                        {
                            case "Copy": item.Text = "복사";
                                break;
                            case "Delete": item.Text = "삭제";
                                break;
                            case "Paste": item.Text = "붙여넣기";
                                break;
                            case "Edit": item.Text = "수정";
                                break;
                            case "Clear Value": item.Text = "값 지우기";
                                break;
                            case "Delete Row": item.Text = "행 삭제";
                                break;
                            default: break;
                        }
                    }

                }
                
                //체크박스 컬럼 존재 하지 않을 경우 return
                if (this.Columns["CheckBox"] != null)
                {
                    //오른쪽 마우스 선택 반전, 전체 선택, 선택 행 체크 추가
                    menuItem1 = new RadMenuItem("선택 영역 체크", "MENU1");
                    menuItem2 = new RadMenuItem("전체 체크", "MENU2");
                    menuItem3 = new RadMenuItem("체크 영역 반전", "MENU3");
                    menuItem4 = new RadMenuItem("체크 해제", "MENU4");

                    menuItem1.Click += new EventHandler(menuItem_Click);
                    menuItem2.Click += new EventHandler(menuItem_Click);
                    menuItem3.Click += new EventHandler(menuItem_Click);
                    menuItem4.Click += new EventHandler(menuItem_Click);


                    e.ContextMenu.Items.Insert(0, menuItem2);//전체체크
                    e.ContextMenu.Items.Insert(1, menuItem1);//선택영역체크
                    e.ContextMenu.Items.Insert(2, menuItem3);//체크영역반전
                    e.ContextMenu.Items.Insert(3, menuItem4);//체크해제
                    e.ContextMenu.Items.Insert(4, new RadMenuSeparatorItem());
                }
            }
            if (hcell != null)
            {
                e.ContextMenu = null;
            }
        }

        //오른쪽 마우스 상자 아이템 클릭
        /// <summary>
        /// 체크박스 헤더 오른쪽 마우스 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem_Click(object sender, EventArgs e)
        {
            RadMenuItem item = sender as RadMenuItem;
            if (item == null)
            {
                return;
            }
            CheckByCondition(item.Tag.ToString());
        }

        //전체체크, 선택반전, 체크 해제 ...
        private void CheckByCondition(string item_text)
        {
            switch (item_text)
            {
                case "MENU1":
                    //선택행 체크
                    for (int i = 0; i < Rows.Count; i++)
                    {
                        Rows[i].Cells["CheckBox"].Value = false;
                    }
                    for (int i = 0; i < SelectedRows.Count; i++)
                    {
                        SelectedRows[i].Cells["CheckBox"].Value = true;
                    }

                    break;
                case "MENU2":
                    //전체체크
                    for (int i = 0; i < Rows.Count; i++)
                    {
                        Rows[i].Cells["CheckBox"].Value = true;
                    }
                    break;
                case "MENU3":
                    //선택된 체크 반전
                    for (int i = 0; i < Rows.Count; i++)
                    {
                        Rows[i].Cells["CheckBox"].Value = (bool)Rows[i].Cells["CheckBox"].Value ? false : true;
                    }
                    break;
                case "MENU4":
                    //전체 체크 해제
                    for (int i = 0; i < Rows.Count; i++)
                    {
                        Rows[i].Cells["CheckBox"].Value = false;
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region 바인딩 관련

        //데이터 바인딩
        /// <summary>
        /// 헤더테이블 및 데이터 테이블 그리드뷰에 조건에 맞게 바인딩
        /// </summary>
        /// <param name="spName"></param>
        /// <param name="paramss"></param>
        public void BindData(string spName, Dictionary<string, object> paramss)
        {
            if (this.IsInEditMode)
            {
                this.EndEdit();
            }

            //SP 이름 저장
            if (string.IsNullOrWhiteSpace(gridUspName))
                gridUspName = spName;

            _paramss = paramss;



            //헤더 파라미터 설정
            if (_paramss == null)
            {
                Usp_Load_Parameters["@HEADER_PARAMS"] = BaseEntity.sessInq;
            }
            else
            {
                Usp_Load_Parameters["@HEADER_PARAMS"] = BaseEntity.sessInq + ";#" + paramss["@HEADER_PARAMS"].ToString();
            }


            //첫 번째 셀렉트 문은 헤더 두 번째는 데이터
            //데이터셋 초기화
            ds_Gird = new DataSet();

            //DB통신
            ds_Gird = BaseControl.DBUtil.ExecuteDataSet(gridUspName, Usp_Load_Parameters, CommandType.StoredProcedure);

            BindData(ds_Gird);
        }
        public void BindData(DataSet _ds_Gird)
        {
            //데이터 소스 초기화
            this.DataSource = null;
            this.Columns.Clear();

            //상위 요약 행 초기화
            topSummaryRow = new GridViewSummaryRowItem();
            this.SummaryRowsTop.Clear();

            //데이터 테이블 2개 존재 확인
            if (_ds_Gird == null || _ds_Gird.Tables.Count < 2) return;

            //헤더 테이블 데이터 테이블 컬럼 수 비교
            if (_ds_Gird.Tables[0].Columns.Count != _ds_Gird.Tables[1].Columns.Count)
            {
                BaseControl.bowooMessageBox.Show("헤더와 데이터 정보가 유효하지 않습니다.\r\n관리자에게 문의하세요.");
                return;
            }

            //헤더 데이터 존재 확인
            if (_ds_Gird.Tables[0].Rows.Count < 1) return;

            //그리드 업데이트 시작 (레이아웃 멈춤)
            if (!this.MasterTemplate.IsUpdating)
                this.MasterTemplate.BeginUpdate();

            //데이터 테이블 바인딩
            this.DataSource = _ds_Gird.Tables[1];
            
            //컬럼 별 설정
            for (int i = 0; i < _ds_Gird.Tables[0].Columns.Count; i++)
            {
                //기본 컬럼 읽기 전용
                this.Columns[i].ReadOnly = true;

                //기본 컬럼 리사이즈 불가
                this.Columns[i].AllowResize = true;

                //헤더에서 태그추출
                rawHeaderText = _ds_Gird.Tables[0].Rows[0][i].ToString();

                //태그 <> 확인
                if (rawHeaderText.Contains('<') && rawHeaderText.Contains('>'))
                {
                    //태그 별 설정 메서드
                    Proc_Column_By_Tag(rawHeaderText, this.Columns[i]);
                }
                else
                {
                    //태그 없을 시 헤더 텍스트 설정
                    columnHeaderText = rawHeaderText;
                }

                //태그 제거된 헤더 택스트 설정
                this.Columns[i].HeaderText = columnHeaderText;
            }

            //TYPE 별 컨트롤 BIND
            BindControlByType();

            //상위 요약 행 추가
            if (topSummaryRow.Count > 0)
                this.SummaryRowsTop.Add(topSummaryRow);

            if (this.Columns.Contains("CheckBox"))
            {
                ConditionalFormattingObject co1 = new ConditionalFormattingObject("CheckedBox Color Change", ConditionTypes.Equal, "True", "", true);
                co1.RowBackColor = Color.FromArgb(228, 247, 186);
                co1.CellBackColor = Color.FromArgb(228, 247, 186);

                this.Columns["CheckBox"].ConditionalFormattingObjectList.Add(co1);
            }

            //그리드 end 업데이트 (변경 레이아웃 적용)
            if (this.MasterTemplate.IsUpdating)
                this.MasterTemplate.EndUpdate();

            //기본 컬럼 리사이즈 불가
            this.AllowColumnResize = false;
        }
        public void BindData()
        {
            BindData(this.gridUspName, null);
        }

        /// <summary>
        /// 태그 값에 따라 컬럼 설정 변경
        /// </summary>
        /// <param name="rawHeaderText"></param>
        /// <param name="column"></param>
        private void Proc_Column_By_Tag(string rawHeaderText, GridViewDataColumn column)
        {
            var checkboxColumn = column as GridViewCheckBoxColumn;


            if (checkboxColumn != null)
            {
                //체크 박스일 경우 정렬 금지
                column.AllowSort = false;
                column.ReadOnly = false;
            }

            //KEY, DATA 컬럼 여부 지정 위해 컬럼 태그 설정 
            column.Tag = new ParamType();
            var colTag = column.Tag as ParamType;

            //헤더 태그 시작 인덱스
            tagStartIndex = rawHeaderText.IndexOf('<');

            //헤더 태그 끝 인덱스
            tagEndIndex = rawHeaderText.IndexOf('>');

            //태그 추출
            tagText = rawHeaderText.Substring(tagStartIndex + 1, tagEndIndex - tagStartIndex - 1);

            //태그 삭제 후 헤더 텍스트 설정
            columnHeaderText = rawHeaderText.Remove(tagStartIndex, tagEndIndex - tagStartIndex + 1);

            //태그 텍스트 분할
            splitedTagText = tagText.Split(';');

            if (splitedTagText.Length > 0)
            {
                foreach (string _tagText in splitedTagText)
                {
                    //태그 값 존재 확인
                    if (!string.IsNullOrWhiteSpace(_tagText))
                    {
                        //'=' 구분된 태그 값 확인, 대문자로 변경
                        tagKeyandValue = _tagText.ToUpper().Split('=');
                        if (tagKeyandValue.Length > 1)
                        {
                            //sql tag용
                            if (tagKeyandValue.Length > 2)
                            {
                                for (int i = 2; i < tagKeyandValue.Length; i++)
                                {
                                    tagKeyandValue[1] += "=" + tagKeyandValue[i];
                                }
                            }
                            //조건 별 태그 실행
                            switch (tagKeyandValue[0])
                            {
                                //숨김 처리
                                case "ISVISIBLE":
                                    if (tagKeyandValue[1] == "FALSE") column.IsVisible = false;
                                    if (tagKeyandValue[1] == "TRUE") column.IsVisible = true;
                                    break;
                                //행 너비
                                case "WIDTH":
                                    int colWidth = 0;
                                    //열 설정이 보임이고, 태그 값 숫자 일시
                                    if (column.IsVisible && Int32.TryParse(tagKeyandValue[1], out colWidth))
                                    {
                                        column.Width = colWidth;
                                    }
                                    else if (column.IsVisible && tagKeyandValue[1] == "BESTFIT")
                                    {
                                        column.BestFit();
                                    }
                                    break;
                                //읽기 전용
                                case "READONLY":
                                    if (column.IsVisible && tagKeyandValue[1] == "FALSE")
                                    {
                                        column.ReadOnly = false;
                                    }
                                    break;
                                //데이터 영역 정렬
                                case "TEXTALIGN":
                                    if (column.IsVisible && tagKeyandValue[1] == "LEFT")
                                    {
                                        column.TextAlignment = ContentAlignment.MiddleLeft;
                                    }
                                    if (column.IsVisible && tagKeyandValue[1] == "CENTER")
                                    {
                                        column.TextAlignment = ContentAlignment.MiddleCenter;
                                    }
                                    if (column.IsVisible && tagKeyandValue[1] == "RIGHT")
                                    {
                                        column.TextAlignment = ContentAlignment.MiddleRight;
                                    }
                                    break;
                                //헤더 체크 박스 여부
                                case "HEADERCHECKBOX":
                                    if (column.IsVisible && checkboxColumn != null && tagKeyandValue[1] == "TRUE")
                                    {
                                        checkboxColumn.EnableHeaderCheckBox = true;
                                    }
                                    break;
                                //키 컬럼, 데이터 컬럼 여부 지정
                                case "KEY":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        colTag.Key = true;
                                    }
                                    break;
                                case "DATA":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        colTag.Data = true;
                                    }
                                    break;
                                case "FILE":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        colTag.File = true;
                                    }
                                    break;
                                //세로 병합 설정
                                case "MERGE":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        MergeVertically(column.Name);
                                        //MERGE 시 정렬 기능 불가
                                        this.EnableSorting = false;
                                    }
                                    break;
                                //Sum 추가
                                case "SUM":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        colTag.Sum = true;
                                        //Summary Row Add
                                        summaryItem = new GridViewSummaryItem();
                                        summaryItem.Name = column.Name;
                                        //summaryItem.FormatString = "SUM : {0}";
                                        summaryItem.Aggregate = GridAggregateFunction.Sum; //Sum
                                        topSummaryRow.Add(summaryItem);
                                    }
                                    break;
                                //COUNT 추가
                                case "COUNT":
                                    if (tagKeyandValue[1] == "TRUE")
                                    {
                                        colTag.Count = true;
                                        //Summary Row Add
                                        summaryItem = new GridViewSummaryItem();
                                        summaryItem.Name = column.Name;
                                        //summaryItem.FormatString = "COUNT : {0}";
                                        summaryItem.Aggregate = GridAggregateFunction.Count; //Sum
                                        topSummaryRow.Add(summaryItem);
                                    }
                                    break;
                                case "STITLE":
                                    //Summary Row Add
                                    colTag.Title = true;
                                    summaryItem = new GridViewSummaryItem();
                                    summaryItem.Name = column.Name;
                                    summaryItem.FormatString = tagKeyandValue[1];
                                    summaryItem.Aggregate = GridAggregateFunction.Count; //
                                    topSummaryRow.Add(summaryItem);
                                    break;
                                case "TYPE":
                                    colTag.BindType = tagKeyandValue[1];
                                    break;
                                case "SOURCETYPE":
                                    colTag.BindSourceType = tagKeyandValue[1];
                                    break;
                                case "SOURCE":
                                    colTag.BindSource = tagKeyandValue[1];
                                    break;
                                default: break;
                            }
                        }
                    }
                }
            }
        }
        
        #endregion

        #region 컨트롤 바인딩 관련
        //타입별 컬럼 바인딩
        /// <summary>
        /// 바인드 타입별 컨트롤 바인딩
        /// </summary>
        private void BindControlByType()
        {
            for (int i = 0; i < this.Columns.Count; i++)
            {
                if ((this.Columns[i].Tag as ParamType).BindType == "")
                    continue;
                else
                {
                    string datecolname = this.Columns[i].FieldName;
                    int datecolidx = this.Columns[i].Index;

                    switch ((this.Columns[i].Tag as ParamType).BindType)
                    {
                        case "DATE":
                            //table for convert date column
                            DataTable dateConvertDt = new DataTable();

                            dateConvertDt = (this.DataSource as DataTable).Copy();
                            dateConvertDt.Columns.Remove(datecolname);

                            dateConvertDt.Columns.Add(datecolname, typeof(DateTime));
                            CultureInfo provider = CultureInfo.InvariantCulture;
                            string format = "yyyyMMdd";

                            for (int j = 0; j < dateConvertDt.Rows.Count; j++)
                            {
                                dateConvertDt.Rows[j][datecolname] = DateTime.ParseExact(ds_Gird.Tables[1].Rows[j][datecolname].ToString(), format, provider);
                            }

                            this.DataSource = dateConvertDt;


                            GridViewDateTimeColumn datecol = new GridViewDateTimeColumn(datecolname);
                            datecol.FormatString = "{0:yyyy-MM-dd}";
                            datecol.Format = DateTimePickerFormat.Custom;
                            datecol.CustomFormat = "yyyy-MM-dd";

                            datecol.HeaderText = this.Columns[i].HeaderText;
                            datecol.TextAlignment = this.Columns[i].TextAlignment;
                            datecol.Tag = this.Columns[i].Tag;
                            datecol.Width = this.Columns[i].Width;


                            //column swap
                            this.Columns.Remove(datecolname);
                            this.Columns.Insert(datecolidx, datecol);


                            break;

                        case "COMBO":
                            GridViewComboBoxColumn comboCol = new GridViewComboBoxColumn();

                            string bindSourceType = (this.Columns[i].Tag as ParamType).BindSourceType;
                            string bindSource = (this.Columns[i].Tag as ParamType).BindSource;

                            object r_comboBindSource = GetBindControlSource(bindSourceType, bindSource);

                            DataTable dtComboSource = r_comboBindSource as DataTable;

                            if (dtComboSource != null)
                            {
                                comboCol.ValueMember = dtComboSource.Columns[0].ColumnName;
                                if (dtComboSource.Columns.Count > 1)
                                {
                                    comboCol.DisplayMember = dtComboSource.Columns[1].ColumnName;
                                }
                            }

                            comboCol.DataSource = r_comboBindSource;

                            comboCol.HeaderText = this.Columns[i].HeaderText;
                            comboCol.TextAlignment = this.Columns[i].TextAlignment;
                            comboCol.Tag = this.Columns[i].Tag;
                            comboCol.Width = this.Columns[i].Width;

                            comboCol.DropDownStyle = RadDropDownStyle.DropDown;

                            comboCol.FieldName = datecolname;



                            //column swap
                            this.Columns.Remove(datecolname);
                            this.Columns.Insert(datecolidx, comboCol);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        //combobox열에 바인딩 될 소스를 가져온다 
        private object GetBindControlSource(string bindSourceType, string bindSource)
        {
            if (bindSourceType == "SP")
            {
                return BaseControl.DBUtil.ExecuteDataSet(bindSource, null, CommandType.StoredProcedure).Tables[0];
            }
            else if (bindSourceType == "SQL")
            {
                return BaseControl.DBUtil.ExecuteDataSet(bindSource).Tables[0];
            }
            else if (bindSourceType == "TEXT")
            {
                string[] textSource = bindSource.Split(',');
                return textSource;
            }
            else
            {
                return null;
            }

        }
        #endregion

        #region MERGE 기능 관련
        //세로 MERGE 기능
        /// <summary>
        /// MERGE CELL Vertically
        /// </summary>
        /// <param name="radGridView"></param>
        /// <param name="startColumnIndex"></param>
        /// <param name="endColumnIndex"></param>
        public void MergeVertically(string columnName)
        {
            GridViewRowInfo Prev = null;
            foreach (GridViewRowInfo item in this.Rows)
            {
                if (Prev != null)
                {
                    string firstCellText = string.Empty;
                    string secondCellText = string.Empty;

                    GridViewCellInfo firstCell = Prev.Cells[columnName];
                    GridViewCellInfo secondCell = item.Cells[columnName];

                    firstCellText = (firstCell != null && firstCell.Value != null ? firstCell.Value.ToString() : string.Empty);
                    secondCellText = (secondCell != null && secondCell.Value != null ? secondCell.Value.ToString() : string.Empty);

                    setCellBorders(firstCell, Color.FromArgb(209, 225, 245));
                    setCellBorders(secondCell, Color.FromArgb(209, 225, 245));

                    if (firstCellText == secondCellText)
                    {
                        firstCell.Style.BorderBottomColor = Color.Transparent;
                        secondCell.Style.BorderTopColor = Color.Transparent;
                        secondCell.Style.ForeColor = Color.Transparent;
                        //secondCell.Style.BorderBottomColor = Color.FromArgb(209, 225, 245);
                    }
                    else
                    {
                        secondCell.Style.BorderTopColor = Color.FromArgb(209, 225, 245);
                        secondCell.Style.BorderBottomColor = Color.FromArgb(209, 225, 245);
                        secondCell.Style.ForeColor = Color.Black;
                        Prev = item;
                        continue;
                    }
                }
                else
                {
                    Prev = item;
                }
            }
        }

        //세로 MERGE 기능
        /// <summary>
        /// 셀 테두리 설정
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="color"></param>
        private void setCellBorders(GridViewCellInfo cell, Color color)
        {
            cell.Style.CustomizeBorder = true;
            cell.Style.BorderBoxStyle = Telerik.WinControls.BorderBoxStyle.FourBorders;

            cell.Style.BorderLeftColor = color;
            cell.Style.BorderRightColor = color;

            if (cell.RowInfo.Index == 0) cell.Style.BorderTopColor = color;
            if (cell.RowInfo.Index == this.Rows.Count - 1) cell.Style.BorderBottomColor = color;

            //cell.Style.BorderBottomColor = color;
            //if (cell.Style.BorderTopColor != Color.Transparent)
            //{
            //    cell.Style.BorderTopColor = color;
            //}
        }
        #endregion

        #region 파라미터 생성 관련
        //자식 그리드 키 파라미터 생성
        /// <summary>
        /// 상세 그리드로 키 값을 전달하기 위해 파라미터 생성
        /// </summary>
        public string GetKeyParam()
        {
            string KeyParams = "";
            if (this.SelectedRows.Count > 0)
            {
                foreach (GridViewDataColumn col in this.Columns)
                {
                    if (col.Tag is ParamType && (col.Tag as ParamType).Key)
                    {
                        KeyParams += col.Name + "=" + this.SelectedRows[0].Cells[col.Name].Value.ToString() + ";#";
                    }
                }
            }
            return KeyParams;
        }

        //프로시저용 데이터 파라미터 생성
        /// <summary>
        /// 해당 행의 데이터 파라미터 생성
        /// </summary>
        /// <returns></returns>
        public string GetDataParam(int rowIndex)
        {
            string DataParams = "";
            if (this.Rows.Count > 0 && rowIndex > -1)
            {
                foreach (GridViewDataColumn col in this.Columns)
                {
                    if (col.Tag is ParamType && (col.Tag as ParamType).Data)
                    {
                        DataParams += col.Name + "=" + this.Rows[rowIndex].Cells[col.Name].Value.ToString() + ";#";
                    }
                }
            }
            //DATA Param 추가
            if (Add_Data_Parameters.Count > 0)
            {
                foreach (string key in Add_Data_Parameters.Keys)
                {
                    DataParams += key + "=" + Add_Data_Parameters[key].ToString() + ";#";
                }
            }
            return DataParams;
        }

        #endregion
    }
}
